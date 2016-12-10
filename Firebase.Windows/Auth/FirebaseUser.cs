using System;
using System.Collections.Generic;
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
		/// Get user from reference
		/// </summary>
		/// <param name="reference">JavaScript User instance</param>
		internal FirebaseUser(JavaScriptObjectReference reference)
		{
			this.Reference = reference;
		}
	}
}
