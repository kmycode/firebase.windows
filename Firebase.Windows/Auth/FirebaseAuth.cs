using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Windows.Common;
using Firebase.Windows.Core;
using Firebase.Windows.Exceptions;
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
				var reference = this.Reference.GetPropertyToReference("currentUser");
				if (reference == null || reference.IsNull) return null;
				return new FirebaseUser(reference);
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
		/// Sign in with email and password account to get credential
		/// </summary>
		/// <returns>Firebase credential instance</returns>
		public FirebaseCredential GetEmailAndPasswordCredential(string email, string password)
		{
			var credential = new JavaScriptObjectReference();
			credential.SetValue($"firebase.auth.EmailAuthProvider.credential('{email}', '{password}')");
			return new FirebaseEmailAndPasswordCredential(credential);
		}

		/// <summary>
		/// Sign in with twitter account
		/// </summary>
		/// <returns>FirebasePromise callbacks</returns>
		public async Task<FirebasePromise> SignInWithTwitterAsync()
		{
			var credential = await this.GetTwitterCredentialAsync();
			return credential.SignIn();
		}

		/// <summary>
		/// Sign in with twitter account to get credential
		/// </summary>
		/// <returns>Firebase credential instance</returns>
		public async Task<FirebaseCredential> GetTwitterCredentialAsync()
		{
			return await this.AuthServiceAsync(
				providerName: "TwitterAuthProvider",
				serviceDomanin: "twitter.com",
				credentialGetter: (e) =>
				{
					var twitterToken = e.Reference.GetProperty("credential.accessToken");
					var twitterSecret = e.Reference.GetProperty("credential.secret");

					// auth on main app to get promise object
					var credential = new JavaScriptObjectReference();
					credential.SetValue($"firebase.auth.TwitterAuthProvider.credential('{twitterToken}', '{twitterSecret}')");

					return new FirebaseTwitterCredential(credential);
				}
				);
		}

		/// <summary>
		/// Sign in with github account
		/// </summary>
		/// <returns>FirebasePromise callbacks</returns>
		public async Task<FirebasePromise> SignInWithGithubAsync()
		{
			var credential = await this.GetGithubCredentialAsync();
			return credential.SignIn();
		}

		/// <summary>
		/// Sign in with github account to get credential
		/// </summary>
		/// <returns>Firebase credential instance</returns>
		public async Task<FirebaseCredential> GetGithubCredentialAsync()
		{
			return await this.AuthServiceAsync(
				providerName: "GithubAuthProvider",
				serviceDomanin: "github.com",
				credentialGetter: (e) =>
				{
					var githubToken = e.Reference.GetProperty("credential.accessToken");

					// auth on main app to get promise object
					var credential = new JavaScriptObjectReference();
					credential.SetValue($"firebase.auth.GithubAuthProvider.credential('{githubToken}')");

					return new FirebaseGithubCredential(credential);
				}
				);
		}

		/// <summary>
		/// Sign in with facebook account
		/// </summary>
		/// <returns>FirebasePromise callbacks</returns>
		public async Task<FirebasePromise> SignInWithFacebookAsync()
		{
			var credential = await this.GetFacebookCredentialAsync();
			return credential.SignIn();
		}

		/// <summary>
		/// Sign in with facebook account to get credential
		/// </summary>
		/// <returns>Firebase credential instance</returns>
		public async Task<FirebaseCredential> GetFacebookCredentialAsync()
		{
			return await this.AuthServiceAsync(
				providerName: "FacebookAuthProvider",
				serviceDomanin: "facebook.com",
				credentialGetter: (e) =>
				{
					var facebookToken = e.Reference.GetProperty("credential.accessToken");

					// auth on main app to get promise object
					var credential = new JavaScriptObjectReference();
					credential.SetValue($"firebase.auth.FacebookAuthProvider.credential('{facebookToken}')");

					return new FirebaseFacebookCredential(credential);
				}
				);
		}

		private async Task<FirebaseCredential> AuthServiceAsync(
			string providerName,
			string serviceDomanin,
			Func<FirebasePromise.ResolvedEventArgs, FirebaseCredential> credentialGetter
			)
		{
			var options = this.App.Options;
			options.IsPrivateApp = true;            // struct

			// make chrome browser driver
			ChromeDriverService driverService = null;
			ChromeDriver driver = null;
			JavaScriptBinding jsbinding = null;
			try
			{
				driverService = ChromeDriverService.CreateDefaultService();
				driverService.HideCommandPromptWindow = true;
				driver = new ChromeDriver(driverService, new ChromeOptions());
				jsbinding = new JavaScriptBinding(options.FirebaseHtmlUrl, driver);
			}
			catch (Exception e)
			{
				throw new ChromeBrowserException("Cannot operate Google Chrome browser because of any reasons.", e);
			}

			// create app before auth
			var beforeAuthApp = new FirebaseApp(options, jsbinding);

			// redirect twitter auth page
			jsbinding.ExecuteScript("variables.provider = new firebase.auth." + providerName + "();");
			jsbinding.ExecuteScript("firebase.auth().signInWithRedirect(variables.provider);");

			// monitor url changes and back
			try
			{
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
			}
			catch (Exception e)
			{
				throw new ChromeBrowserWaitingAuthException("Cannot operate Chrome browser. Do you close browser handly?", e);
			}

			FirebasePromise authPromise = null;
			try
			{
				// wait for end of load
				WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
				wait.Until(wd => (string)((IJavaScriptExecutor)wd).ExecuteScript("return document.readyState") == "complete");

				// get promiss object
				var afterAuthApp = new FirebaseApp(options, jsbinding);
				jsbinding.ExecuteScript("variables.promise = firebase.auth().getRedirectResult()");
				var authPromissReference = new JavaScriptObjectReference(jsbinding, "promise");
				authPromise = new FirebasePromise(authPromissReference);
			}
			catch (Exception e)
			{
				throw new ChromeBrowserWaitingAuthResultException("Cannot get auth result by Chrome browser. Do you close browser handly? Browser will be closed automatic.", e);
			}

			// promise received flag
			bool isAuthStatusChanged = false;

			// get twitter token and secret
			FirebaseCredential credential = null;
			authPromise.Resolved += (sender, e) =>
			{
				credential = credentialGetter(e);
				isAuthStatusChanged = true;

				// close chrome browser
				driver.Dispose();
			};

			authPromise.Rejected += (sender, e) =>
			{
				isAuthStatusChanged = true;

				// close chrome browser
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
					throw new ChromeBrowserWaitingAuthResultException("Cannot connect Chrome browser.");
				}

				await Task.Delay(10);
			}

			return credential;
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
		/// Sign in with already gotten credential object
		/// </summary>
		/// <param name="credential">Firebase credential</param>
		/// <returns>FirebasePromise callbacks</returns>
		public FirebasePromise SignInWithCredential(FirebaseCredential credential)
		{
			var promiseReference = this.Reference.InvokeMethodToReference("signInWithCredential", "variables." + credential.Reference.VariableName);
			return new FirebasePromise(promiseReference);
		}

		/// <summary>
		/// Sign in with custom token key
		/// </summary>
		/// <param name="token">custom token</param>
		/// <returns>FirebasePromise callbacks</returns>
		public FirebasePromise SignInWithCustomToken(string token)
		{
			var promiseReference = this.Reference.InvokeMethodToReference("signInWithCustomToken",$"'{token}'");
			return new FirebasePromise(promiseReference);
		}

		/// <summary>
		/// Sign out any auth
		/// </summary>
		/// <returns>FirebasePromise callbacks</returns>
		public FirebasePromise SignOut()
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("signOut"));
		}

		/// <summary>
		/// send email to reset password
		/// </summary>
		/// <param name="email">send to email address</param>
		/// <returns>FirebasePromise callbacks</returns>
		public FirebasePromise SendPasswordResetEmail(string email)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("sendPasswordResetEmail", $"'{email}'"));
		}

		/// <summary>
		/// verify password reset code
		/// </summary>
		/// <param name="password">verify password</param>
		/// <returns>FirebasePromise callbacks</returns>
		public FirebasePromise VerifyPasswordResetCode(string password)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("verifyPasswordResetCode", $"'{password}'"));
		}

		/// <summary>
		/// confirm new password change
		/// </summary>
		/// <returns>FirebasePromise callbacks</returns>
		public FirebasePromise ConfirmPasswordReset(string code, string newPassword)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("confirmPasswordReset", $"'{code}', '{newPassword}'"));
		}

		/// <summary>
		/// apply action code
		/// </summary>
		/// <param name="code">action code sent to the user</param>
		/// <returns>FirebasePromise callbacks</returns>
		public FirebasePromise ApplyActionCode(string code)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("applyActionCode", $"'{code}'"));
		}

		/// <summary>
		/// check action code
		/// </summary>
		/// <param name="code">action code sent to the user</param>
		/// <returns>FirebasePromise callbacks</returns>
		public FirebasePromise CheckActionCode(string code)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("checkActionCode", $"'{code}'"));
		}
	}
}
