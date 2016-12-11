using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Windows.Common;
using Firebase.Windows.Core;

namespace Firebase.Windows.Storage
{
	public class FirebaseStorage
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
		/// operation timeout milliseconds
		/// </summary>
		public int MaxOperationRetryTime
		{
			get
			{
				return int.Parse(this.Reference.GetProperty("maxOperationRetryTime"));
			}
			set
			{
				this.Reference.InvokeMethod("setMaxOperationRetryTime", value.ToString());
			}
		}

		/// <summary>
		/// upload timeout milliseconds
		/// </summary>
		public int MaxUploadRetryTime
		{
			get
			{
				return int.Parse(this.Reference.GetProperty("maxUploadRetryTime"));
			}
			set
			{
				this.Reference.InvokeMethod("setMaxUploadRetryTime", value.ToString());
			}
		}

		/// <summary>
		/// Get auth object from JavaScript reference
		/// </summary>
		/// <param name="reference">JavaScript Database instance</param>
		internal FirebaseStorage(JavaScriptObjectReference reference)
		{
			if (reference == null || reference.IsNull) throw new NullReferenceException();
			this.Reference = reference;
		}

		/// <summary>
		/// get storage root reference
		/// </summary>
		/// <returns>storage reference</returns>
		public FirebaseStorageReference Ref()
		{
			return new FirebaseStorageReference(this.Reference.InvokeMethodToReference("ref"));
		}

		/// <summary>
		/// get storage the path reference
		/// </summary>
		/// <param name="path">storage reference path</param>
		/// <returns>storage reference</returns>
		public FirebaseStorageReference Ref(string path)
		{
			return new FirebaseStorageReference(this.Reference.InvokeMethodToReference("ref", $"'{path}'"));
		}

		/// <summary>
		/// get storage the path reference from url: gs://
		/// </summary>
		/// <param name="path">storage reference path</param>
		/// <returns>storage reference</returns>
		public FirebaseStorageReference RefFromUrl(string path)
		{
			return new FirebaseStorageReference(this.Reference.InvokeMethodToReference("refFromURL", $"'{path}'"));
		}
	}
}
