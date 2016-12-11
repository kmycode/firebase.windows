using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Windows.Common;

namespace Firebase.Windows.Auth
{
	public class FirebaseUserInfo
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
		/// the user unique id
		/// </summary>
		public string Uid
		{
			get
			{
				return this.Reference.GetProperty("uid");
			}
		}

		internal FirebaseUserInfo(JavaScriptObjectReference reference)
		{
			this.Reference = reference;
		}
	}
}
