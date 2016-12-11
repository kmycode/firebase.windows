using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Windows.Common;

namespace Firebase.Windows.Auth
{
	public class FirebaseUser
	{
		/// <summary>
		/// Firebase User Reference
		/// </summary>
		private JavaScriptObjectReference Reference { get; }

		/// <summary>
		/// Display name
		/// </summary>
		public string DisplayName
		{
			get
			{
				return this.Reference.GetProperty("displayName");
			}
		}

		/// <summary>
		/// Email address
		/// </summary>
		public string Email
		{
			get
			{
				return this.Reference.GetProperty("email");
			}
		}

		/// <summary>
		/// Email verified or not
		/// </summary>
		public bool IsEmailVerified
		{
			get
			{
				return this.Reference.GetPropertyToBool("emailVerified");
			}
		}

		/// <summary>
		/// is annoymous user
		/// </summary>
		public bool IsAnonymous
		{
			get
			{
				return this.Reference.GetPropertyToBool("isAnonymous");
			}
		}

		/// <summary>
		/// If user icon or photoes are set, its image url
		/// </summary>
		public string PhotoUrl
		{
			get
			{
				return this.Reference.GetProperty("photoURL");
			}
		}

		/// <summary>
		/// UserInfo array
		/// </summary>
		public Collection<FirebaseUserInfo> ProviderDatas
		{
			get
			{
				var array = new Collection<FirebaseUserInfo>();
				var datas = this.Reference.GetPropertyToArray("providerData");
				foreach (var data in datas)
				{
					array.Add(new FirebaseUserInfo(data));
				}
				return array;
			}
		}

		/// <summary>
		/// UserInfo array
		/// </summary>
		public Collection<FirebaseUserInfo> UserInfoes => this.ProviderDatas;

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

		/// <summary>
		/// the user account refresh token
		/// </summary>
		public string RefreshToken
		{
			get
			{
				return this.Reference.GetProperty("refreshToken");
			}
		}
		
		/// <summary>
		/// the user unique id
		/// </summary>
		public string Uid
		{
			get
			{
				return this.Reference.GetProperty("uid");
			}
		}

		/// <summary>
		/// Get user from reference
		/// </summary>
		/// <param name="reference">JavaScript User instance</param>
		internal FirebaseUser(JavaScriptObjectReference reference)
		{
			this.Reference = reference;
		}

		/// <summary>
		/// link user to user
		/// </summary>
		/// <param name="credential">credential to additional link</param>
		/// <returns>Firebase Promise callback</returns>
		public FirebasePromise Link(FirebaseCredential credential)
		{
			var promise = this.Reference.InvokeMethodToReference("link", "variables." + credential.Reference.VariableName);
			return new FirebasePromise(promise);
		}

		/// <summary>
		/// unlink user and user
		/// </summary>
		/// <param name="providerId">provider id</param>
		/// <returns>Firebase Promise callback</returns>
		public FirebasePromise Unlink(string providerId)
		{
			var promise = this.Reference.InvokeMethodToReference("unlink", "'" + providerId + "'");
			return new FirebasePromise(promise);
		}

		/// <summary>
		/// unlink user and user
		/// </summary>
		/// <param name="userInfo">user info instance</param>
		/// <returns>Firebase Promise callback</returns>
		public FirebasePromise Unlink(FirebaseUserInfo userInfo)
		{
			return this.Unlink(userInfo.ProviderId);
		}

		/// <summary>
		/// delete the user
		/// </summary>
		/// <returns>Firebase Promise callback</returns>
		public FirebasePromise Delete()
		{
			var promise = this.Reference.InvokeMethodToReference("delete");
			return new FirebasePromise(promise);
		}

		/// <summary>
		/// get the user token
		/// </summary>
		/// <param name="isForceRefresh">force refresh</param>
		/// <returns>Firebase Promise callback</returns>
		public FirebasePromise GetToken(bool isForceRefresh = false)
		{
			var promise = this.Reference.InvokeMethodToReference("getToken", isForceRefresh.ToString().ToLower());
			return new FirebasePromise(promise);
		}

		/// <summary>
		/// re-auth credential
		/// </summary>
		/// <param name="credential">target credential</param>
		/// <returns>Firebase Promise callback</returns>
		public FirebasePromise Reauthenticate(FirebaseCredential credential)
		{
			var promise = this.Reference.InvokeMethodToReference("getToken", "variables." + credential.Reference.VariableName);
			return new FirebasePromise(promise);
		}

		/// <summary>
		/// reload the user
		/// </summary>
		/// <returns>Firebase Promise callback</returns>
		public FirebasePromise Reload()
		{
			var promise = this.Reference.InvokeMethodToReference("reload");
			return new FirebasePromise(promise);
		}

		/// <summary>
		/// send the email to verification
		/// </summary>
		/// <returns>Firebase Promise callback</returns>
		public FirebasePromise SendEmailVerification()
		{
			var promise = this.Reference.InvokeMethodToReference("sendEmailVerification");
			return new FirebasePromise(promise);
		}

		/// <summary>
		/// update this user email
		/// </summary>
		/// <param name="email">email address</param>
		/// <returns>Firebase Promise callback</returns>
		public FirebasePromise UpdateEmail(string email)
		{
			var promise = this.Reference.InvokeMethodToReference("updateEmail", "'" + email + "'");
			return new FirebasePromise(promise);
		}

		/// <summary>
		/// update this user password
		/// </summary>
		/// <param name="password">email address</param>
		/// <returns>Firebase Promise callback</returns>
		public FirebasePromise UpdatePassword(string password)
		{
			var promise = this.Reference.InvokeMethodToReference("updatePassword", "'" + password + "'");
			return new FirebasePromise(promise);
		}

		/// <summary>
		/// update this user profile
		/// </summary>
		/// <param name="displayName">the user display name</param>
		/// <param name="photoUrl">the user photo picture url</param>
		/// <returns>Firebase Promise callback</returns>
		public FirebasePromise UpdateProfile(string displayName, string photoUrl)
		{
			var promise = this.Reference.InvokeMethodToReference("updateProfile", "{displayName:'" + displayName + "',photoURL:'" + photoUrl + "'}");
			return new FirebasePromise(promise);
		}
	}
}
