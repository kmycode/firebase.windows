using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Firebase.Windows.Core;

namespace Firebase.Windows.Test.SamplePage
{
	/// <summary>
	/// Sample3_Storage.xaml の相互作用ロジック
	/// </summary>
	public partial class Sample3_Storage : Page
	{
		public Sample3_Storage()
		{
			InitializeComponent();

			//if (this.SignIn())
			{
				//this.SetData();
				//this.GetData();
				//this.UploadImage();
				//this.DeleteData();
			}
		}

		private bool SignIn()
		{
			// auth
			var promise = FirebaseApp.Default.Auth().SignInWithEmailAndPassword("test@example.com", "Asuka786");
			promise.Rejected += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = "認証失敗: " + e.ErrorCode);
			promise.StartReceiving();
			promise.WaitForStatusChanged();
			return FirebaseApp.Default.Auth().CurrentUser != null;
		}

		private void SetData()
		{
			// get storage object
			var storage = FirebaseApp.Default.Storage();

			// get storage path reference and put string data
			var task = storage.Ref("test.txt").Put("Hello!");

			// set storage upload task events
			task.Fulfilled += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = "Upload Success");
			task.Rejected += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = "Upload Failed: " + e.ErrorCode);

			// start receiving storage upload event
			task.StartReceiving();
		}

		private void UploadImage()
		{
			// get storage object
			var storage = FirebaseApp.Default.Storage();

			// convert png file to byte array
			System.IO.FileStream fs = new System.IO.FileStream(@".\local.png", System.IO.FileMode.Open, System.IO.FileAccess.Read);
			byte[] bs = new byte[fs.Length];
			fs.Read(bs, 0, bs.Length);
			fs.Close();

			// upload binary data and content type
			var task = storage.Ref("test.png").Put(bs, "image/png");

			// set storage upload task events
			task.Fulfilled += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = "アップロード成功");
			task.Rejected += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = "アップロード失敗: " + e.ErrorCode);

			// start receiving storage upload event
			task.StartReceiving();
		}

		private void GetData()
		{
			// get storage object
			var storage = FirebaseApp.Default.Storage();

			// get storage file path reference
			var r = storage.Ref("test3.txt");

			// get download url for get file
			var promise = r.GetDownloadUrl();

			// set storage download events
			promise.Resolved += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = promise.StringValue);
			promise.Rejected += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = "ダウンロード失敗: " + e.ErrorCode);

			// start receiving storage download event
			promise.StartReceiving();

			// todo: please write here the code downloading file with url
		}

		private void DeleteData()
		{
			FirebaseApp.Default.Storage().Ref("test3.txt").Delete();
		}
	}
}
