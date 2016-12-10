using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firebase.Windows.Core
{
	/// <summary>
	/// Firebaseアプリの設定
	/// </summary>
	public struct FirebaseAppOptions
	{
		public string ApiKey { get; set; }

		public string AuthDomain { get; set; }

		public string DatabaseUrl { get; set; }

		public string StorageBucket { get; set; }

		public string MessagingSenderId { get; set; }

		public string FirebaseHtmlUrl { get; set; }

		internal bool IsPrivateApp { get; set; }
	}
}
