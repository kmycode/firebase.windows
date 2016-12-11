using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Windows.Common;
using Firebase.Windows.Core;

namespace Firebase.Windows.Database
{
	public class FirebaseDatabase
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
		/// Get auth object from JavaScript reference
		/// </summary>
		/// <param name="reference">JavaScript Database instance</param>
		internal FirebaseDatabase(JavaScriptObjectReference reference)
		{
			if (reference == null || reference.IsNull) throw new NullReferenceException();
			this.Reference = reference;
		}

		/// <summary>
		/// Go database offline mode
		/// </summary>
		public void GoOffline()
		{
			this.Reference.InvokeMethod("goOffline");
		}

		/// <summary>
		/// Go database online mode
		/// </summary>
		public void GoOnline()
		{
			this.Reference.InvokeMethod("goOnline");
		}

		/// <summary>
		/// get database root reference
		/// </summary>
		/// <returns>database reference</returns>
		public FirebaseDatabaseReference Ref()
		{
			return new FirebaseDatabaseReference(this.Reference.InvokeMethodToReference("ref"));
		}

		/// <summary>
		/// get database the path reference
		/// </summary>
		/// <param name="path">database reference path</param>
		/// <returns>database reference</returns>
		public FirebaseDatabaseReference Ref(string path)
		{
			return new FirebaseDatabaseReference(this.Reference.InvokeMethodToReference("ref", $"'{path}'"));
		}

		/// <summary>
		/// get database root reference from custom url
		/// </summary>
		/// <param name="url">database url</param>
		/// <returns>database reference</returns>
		public FirebaseDatabaseReference RefFromUrl(string url)
		{
			return new FirebaseDatabaseReference(this.Reference.InvokeMethodToReference("refFromURL", $"'{url}'"));
		}
	}
}
