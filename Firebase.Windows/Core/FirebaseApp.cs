using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Windows.Auth;
using Firebase.Windows.Common;
using Firebase.Windows.Database;
using Firebase.Windows.Exceptions;

namespace Firebase.Windows.Core
{
	/// <summary>
	/// Firebaseアプリのエントリーポイント
	/// </summary>
	public class FirebaseApp : IDisposable
	{
		private static FirebaseApp _default;
		public static FirebaseApp Default
		{
			get
			{
				if (_default == null)
				{
					throw new FirebaseNotInitializedException();
				}
				return _default;
			}
		}

		/// <summary>
		/// get all FirebaseApp instances
		/// </summary>
		public static Collection<FirebaseApp> Apps
		{
			get
			{
				return new Collection<FirebaseApp>(_apps);
			}
		}
		private static Collection<FirebaseApp> _apps = new Collection<FirebaseApp>();

		/// <summary>
		/// get Firebase SDK version
		/// </summary>
		public static string SdkVersion
		{
			get
			{
				if (_sdkVersion == null)
				{
					_sdkVersion = (string)JavaScriptBinding.Default.ExecuteScript("return \"\" + firebase.SDK_VERSION");
				}
				return _sdkVersion;
			}
		}
		private static string _sdkVersion;

		/// <summary>
		/// FirebaseApp Reference
		/// </summary>
		private JavaScriptObjectReference Reference { get; }
		internal string ReferenceVariableName
		{
			get
			{
				return this.Reference.VariableName;
			}
		}

		/// <summary>
		/// App instance name
		/// </summary>
		public string Name
		{
			get
			{
				return this.Reference.GetProperty("name");
			}
		}

		/// <summary>
		/// FirebaseAppの設定データ
		/// </summary>
		public FirebaseAppOptions Options { get; }

		/// <summary>
		/// FirebaseAppを初期化します
		/// </summary>
		/// <param name="options">設定データ</param>
		/// <param name="name">Firebase App instance name</param>
		public FirebaseApp(FirebaseAppOptions options, string name = null) : this(options, new JavaScriptBinding(options.FirebaseHtmlUrl), name)
		{
		}

		internal FirebaseApp(FirebaseAppOptions options, JavaScriptBinding jsbinding, string name = null)
		{
			this.Options = options;

			// initialize app
			this.Reference = new JavaScriptObjectReference(jsbinding);
			this.Reference.SetValue(
				$@"firebase.initializeApp({{
					apiKey: ""{this.Options.ApiKey}"",
					authDomain: ""{this.Options.AuthDomain}"",
					databaseURL: ""{this.Options.DatabaseUrl}"",
					storageBucket: ""{this.Options.StorageBucket}"",
					messagingSenderId: ""{this.Options.MessagingSenderId}""
				 }}" + (name == null ? "" : ",'" + name + "'") + ");");

			// set default app
			if (_default == null)
			{
				_default = this;
			}

			// add app list
			if (!options.IsPrivateApp)
			{
				_apps.Add(this);
			}
		}

		/// <summary>
		/// get FirebaseApp from reference
		/// </summary>
		/// <param name="reference">app reference</param>
		internal FirebaseApp(JavaScriptObjectReference reference)
		{
			if (reference == null) throw new NullReferenceException();
			this.Reference = reference;
		}

		/// <summary>
		/// FirebaseAppを初期化します
		/// </summary>
		/// <param name="options">設定データ</param>
		/// <returns>Firebaseのアプリデータ</returns>
		public static FirebaseApp InitializeApp(FirebaseAppOptions options)
		{
			return new FirebaseApp(options);
		}

		/// <summary>
		/// Delete firebase app
		/// </summary>
		public FirebasePromise Delete()
		{
			_apps.Remove(this);
			return new FirebasePromise(this.Reference.InvokeMethodToReference("delete"));
		}

		public FirebaseAuth Auth()
		{
			return new FirebaseAuth(this.Reference.InvokeMethodToReference("auth"));
		}

		public FirebaseDatabase Database()
		{
			return new FirebaseDatabase(this.Reference.InvokeMethodToReference("database"));
		}

		public object Storage()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			JavaScriptBinding.Default.Dispose();
		}
	}
}
