using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Windows.Common;
using Firebase.Windows.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Firebase.Windows.Auth
{
	public class FirebaseAuth
	{
		/// <summary>
		/// Firebase Auth Reference
		/// </summary>
		private JavaScriptObjectReference Reference { get; }

		/// <summary>
		/// FirebaseApp instance
		/// </summary>
		public FirebaseApp App
		{
			get
			{
				var r = this.Reference.GetPropertyToReference("app");
				foreach (var app in FirebaseApp.Apps)
				{
					if ((bool)JavaScriptBinding.Default.ExecuteScript($"return variables.{app.ReferenceVariableName} === variables.{r.VariableName}"))
					{
						return app;
					}
				}
				return null;
			}
		}

		/// <summary>
		/// Firebase auth current user
		/// </summary>
		public FirebaseUser CurrentUser
		{
			get
			{
				return new FirebaseUser(this.Reference.GetPropertyToReference("currentUser"));
			}
		}

		/// <summary>
		/// Get auth object from JavaScript reference
		/// </summary>
		/// <param name="reference">JavaScript Auth instance</param>
		internal FirebaseAuth(JavaScriptObjectReference reference)
		{
			if (reference == null || reference.IsNull) throw new NullReferenceException();
			this.Reference = reference;
		}

		/// <summary>
		/// Create email-password user
		/// </summary>
		/// <param name="email">E-mail</param>
		/// <param name="password">password</param>
		/// <returns>FirebasePromise callbacks</returns>
		public FirebasePromise CreateUserWithEmailAndPassword(string email, string password)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("createUserWithEmailAndPassword", $"'{email}', '{password}'"));
		}

		/// <summary>
		/// Sign in email-password user
		/// </summary>
		/// <param name="email">E-mail</param>
		/// <param name="password">password</param>
		/// <returns>FirebasePromise callbacks</returns>
		public FirebasePromise SignInWithEmailAndPassword(string email, string password)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("signInWithEmailAndPassword", $"'{email}', '{password}'"));
		}

		/// <summary>
		/// Sign in with twitter account
		/// </summary>
		/// <returns>FirebasePromise callbacks</returns>
		public async Task<FirebasePromise> SignInWithTwitterAsync()
		{
			return await this.SignInWithAuthServiceAsync(
				providerName: "TwitterAuthProvider",
				serviceDomanin: "twitter.com",
				credentialGetter: (e) =>
				{
					var twitterToken = e.Reference.GetProperty("credential.accessToken");
					var twitterSecret = e.Reference.GetProperty("credential.secret");

					// auth on main app to get promise object
					var credential = new JavaScriptObjectReference();
					credential.SetValue($"firebase.auth.TwitterAuthProvider.credential('{twitterToken}', '{twitterSecret}')");

					return credential;
				}
				);
		}

		/// <summary>
		/// Sign in with github account
		/// </summary>
		/// <returns>FirebasePromise callbacks</returns>
		public async Task<FirebasePromise> SignInWithGithubAsync()
		{
			return await this.SignInWithAuthServiceAsync(
				providerName: "GithubAuthProvider",
				serviceDomanin: "github.com",
				credentialGetter: (e) =>
				{
					var githubToken = e.Reference.GetProperty("credential.accessToken");

					// auth on main app to get promise object
					var credential = new JavaScriptObjectReference();
					credential.SetValue($"firebase.auth.GithubAuthProvider.credential('{githubToken}')");

					return credential;
				}
				);
		}

		/// <summary>
		/// Sign in with facebook account
		/// </summary>
		/// <returns>FirebasePromise callbacks</returns>
		public async Task<FirebasePromise> SignInWithFacebookAsync()
		{
			return await this.SignInWithAuthServiceAsync(
				providerName: "FacebookAuthProvider",
				serviceDomanin: "facebook.com",
				credentialGetter: (e) =>
				{
					var facebookToken = e.Reference.GetProperty("credential.accessToken");

					// auth on main app to get promise object
					var credential = new JavaScriptObjectReference();
					credential.SetValue($"firebase.auth.FacebookAuthProvider.credential('{facebookToken}')");

					return credential;
				}
				);
		}

		private async Task<FirebasePromise> SignInWithAuthServiceAsync(
			string providerName,
			string serviceDomanin,
			Func<FirebasePromise.ResolvedEventArgs, JavaScriptObjectReference> credentialGetter
			)
		{
			var options = this.App.Options;
			options.IsPrivateApp = true;			// struct

			// make chrome browser driver
			var driverService = ChromeDriverService.CreateDefaultService();
			driverService.HideCommandPromptWindow = true;
			var driver = new ChromeDriver(driverService, new ChromeOptions());
			var jsbinding = new JavaScriptBinding(options.FirebaseHtmlUrl, driver);

			// create app before auth
			var beforeAuthApp = new FirebaseApp(options, jsbinding);

			// redirect twitter auth page
			jsbinding.ExecuteScript("variables.provider = new firebase.auth." + providerName + "();");
			jsbinding.ExecuteScript("firebase.auth().signInWithRedirect(variables.provider);");

			// monitor url changes and back
			string currentUrl = null;
			bool isServiceAuthPageAccessed = false;
			while (true)
			{
				if (currentUrl != driver.Url)
				{
					currentUrl = driver.Url;
					if (!isServiceAuthPageAccessed)
					{
						if (currentUrl.Contains(serviceDomanin) && currentUrl.StartsWith("https://"))
						{
							isServiceAuthPageAccessed = true;
						}
					}
					else
					{
						if (currentUrl == options.FirebaseHtmlUrl)
						{
							break;
						}
					}
				}
				await Task.Delay(10);
			}

			// wait for end of load
			WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
			wait.Until(wd => (string)((IJavaScriptExecutor)wd).ExecuteScript("return document.readyState") == "complete");

			// get promiss object
			var afterAuthApp = new FirebaseApp(options, jsbinding);
			jsbinding.ExecuteScript("variables.promise = firebase.auth().getRedirectResult()");
			var authPromissReference = new JavaScriptObjectReference(jsbinding, "promise");
			var authPromise = new FirebasePromise(authPromissReference);

			// promise received flag
			bool isAuthStatusChanged = false;

			// get twitter token and secret
			FirebasePromise promise = null;
			authPromise.Resolved += (sender, e) =>
			{
				var credential = credentialGetter(e);

				var promiseReference = new JavaScriptObjectReference();
				promiseReference.SetValue($"firebase.auth().signInWithCredential(variables.{credential.VariableName})");
				promise = new FirebasePromise(promiseReference);

				isAuthStatusChanged = true;
				driver.Dispose();
			};

			authPromise.Rejected += (sender, e) =>
			{
				isAuthStatusChanged = true;
				driver.Dispose();
			};

			// start waiting authPromise
			authPromise.StartReceiving();

			// wait for auth
			while (!isAuthStatusChanged)
			{
				// browser close check
				if (!driverService.IsRunning)
				{
					isAuthStatusChanged = true;
					return null;
				}

				await Task.Delay(10);
			}

			return promise;
		}

		/// <summary>
		/// Sign in annonymously
		/// </summary>
		/// <returns>FirebasePromise callbacks</returns>
		public FirebasePromise SignInAnonymously()
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("signInAnonymously"));
		}

		/// <summary>
		/// Sign out any auth
		/// </summary>
		/// <returns>FirebasePromise callbacks</returns>
		public FirebasePromise SignOut()
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("signOut"));
		}
	}
}
