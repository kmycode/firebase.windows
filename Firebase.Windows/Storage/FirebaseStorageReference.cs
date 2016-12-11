using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Windows.Common;

namespace Firebase.Windows.Storage
{
	public class FirebaseStorageReference
	{
		/// <summary>
		/// Firebase Auth Reference
		/// </summary>
		private JavaScriptObjectReference Reference { get; }

		/// <summary>
		/// name of bucket
		/// </summary>
		public string Bucket
		{
			get
			{
				return this.Reference.GetProperty("bucket");
			}
		}

		/// <summary>
		/// full path of file
		/// </summary>
		public string FullPath
		{
			get
			{
				return this.Reference.GetProperty("fullPath");
			}
		}

		/// <summary>
		/// file name
		/// </summary>
		public string Name
		{
			get
			{
				return this.Reference.GetProperty("name");
			}
		}

		/// <summary>
		/// storage this reference's parent
		/// </summary>
		public FirebaseStorageReference Parent
		{
			get
			{
				var reference = this.Reference.GetPropertyToReference("parent");
				if (reference == null || reference.IsNull) return null;
				return new FirebaseStorageReference(reference);
			}
		}

		/// <summary>
		/// storage root
		/// </summary>
		public FirebaseStorageReference Root
		{
			get
			{
				return new FirebaseStorageReference(this.Reference.GetPropertyToReference("root"));
			}
		}

		/// <summary>
		/// storage service
		/// </summary>
		public FirebaseStorage Storage
		{
			get
			{
				return new FirebaseStorage(this.Reference.GetPropertyToReference("storage"));
			}
		}

		/// <summary>
		/// Get auth object from JavaScript reference
		/// </summary>
		/// <param name="reference">JavaScript Database instance</param>
		internal FirebaseStorageReference(JavaScriptObjectReference reference)
		{
			if (reference == null || reference.IsNull) throw new NullReferenceException();
			this.Reference = reference;
		}

		/// <summary>
		/// get this reference child path
		/// </summary>
		/// <param name="path">child path name</param>
		/// <returns>child reference</returns>
		public FirebaseStorageReference Child(string path)
		{
			return new FirebaseStorageReference(this.Reference.InvokeMethodToReference("child", $"'{path}'"));
		}

		/// <summary>
		/// remove this location of reference
		/// </summary>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise Delete()
		{
			var promise = this.Reference.InvokeMethodToReference("delete");
			return new FirebasePromise(promise);
		}

		/// <summary>
		/// get download url
		/// </summary>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise GetDownloadUrl()
		{
			var promise = this.Reference.InvokeMethodToReference("getDownloadURL");
			return new FirebasePromise(promise);
		}

		/// <summary>
		/// get file download url
		/// </summary>
		/// <returns>download url</returns>
		public async Task<string> GetDownloadUrlAsync()
		{
			string result = null;
			var promise = this.GetDownloadUrl();
			promise.Resolved += (sender, e) => result = e.Reference.GetValue();
			promise.StartReceiving();
			await promise.WaitForStatusChangedAsync();
			return result;
		}

		/// <summary>
		/// put the data
		/// </summary>
		/// <param name="data">binary data</param>
		/// <returns>Firebase file upload task callbacks</returns>
		public FirebaseUploadTask Put(byte[] data)
		{
			var blob = new JavaScriptObjectReference();
			blob.SetBlob(data);
			return new FirebaseUploadTask(this.Reference.InvokeMethodToReference("put", "variables." + blob.VariableName));
		}

		/// <summary>
		/// put the data
		/// </summary>
		/// <param name="data">binary data</param>
		/// <param name="contentType">content type</param>
		/// <returns>Firebase file upload task callbacks</returns>
		public FirebaseUploadTask Put(byte[] data, string contentType)
		{
			var blob = new JavaScriptObjectReference();
			blob.SetBlob(data);
			return new FirebaseUploadTask(this.Reference.InvokeMethodToReference("put", "variables." + blob.VariableName + ",{contentType:'" + contentType + "'}"));
		}

		/// <summary>
		/// put the data (call Javascript 'putString' method)
		/// </summary>
		/// <param name="data">string data</param>
		/// <returns>Firebase file upload task callbacks</returns>
		public FirebaseUploadTask Put(string data)
		{
			return new FirebaseUploadTask(this.Reference.InvokeMethodToReference("putString", $"\"{data.Replace("\"", "\\\"")}\",firebase.storage.StringFormat.RAW,{{contentType:'text/plain'}}"));
		}

		/// <summary>
		/// get storage url string (call Javascript 'toString' method)
		/// </summary>
		/// <returns>storage url string</returns>
		public string ToUrlString()
		{
			return this.Reference.InvokeMethodToString("toString");
		}
	}
}
