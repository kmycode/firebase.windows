using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Windows.Common;

namespace Firebase.Windows.Auth
{
	public abstract class FirebaseCredential
	{
		/// <summary>
		/// Firebase User Reference
		/// </summary>
		internal JavaScriptObjectReference Reference { get; }

		/// <summary>
		/// auth service provider id for example 'facebook.com' and 'twitter.com'
		/// </summary>
		public string ProviderId
		{
			get
			{
				return this.Reference.GetProperty("providerId");
			}
		}

		internal FirebaseCredential(JavaScriptObjectReference reference)
		{
			this.Reference = reference;
		}

		/// <summary>
		/// Sign in with this credential on default auth
		/// </summary>
		/// <returns>FirebasePromise callbacks</returns>
		public FirebasePromise SignIn()
		{
			var promiseReference = new JavaScriptObjectReference();
			promiseReference.SetValue($"firebase.auth().signInWithCredential(variables.{this.Reference.VariableName})");
			return new FirebasePromise(promiseReference);
		}
	}

	internal class FirebaseTwitterCredential : FirebaseCredential
	{
		internal FirebaseTwitterCredential(JavaScriptObjectReference reference) : base(reference) { }
	}

	internal class FirebaseFacebookCredential : FirebaseCredential
	{
		internal FirebaseFacebookCredential(JavaScriptObjectReference reference) : base(reference) { }
	}

	internal class FirebaseGithubCredential : FirebaseCredential
	{
		internal FirebaseGithubCredential(JavaScriptObjectReference reference) : base(reference) { }
	}

	internal class FirebaseEmailAndPasswordCredential : FirebaseCredential
	{
		internal FirebaseEmailAndPasswordCredential(JavaScriptObjectReference reference) : base(reference) { }
	}
}
