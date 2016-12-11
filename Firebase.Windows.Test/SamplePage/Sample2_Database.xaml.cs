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
	/// Sample2_Database.xaml の相互作用ロジック
	/// </summary>
	public partial class Sample2_Database : Page
	{
		public Sample2_Database()
		{
			InitializeComponent();

			//if (this.SignIn())
			{
				//this.SetData();
				//this.GetData();
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
			// get database object
			var db = FirebaseApp.Default.Database();

			// get database path reference
			var dbref = db.Ref("dbtest/data");

			// set string data
			//dbref.Set("WPFからのテストだよ！");

			// set structure data
			dbref.Set(new TestStruct { Text = "Hello, Firebase Database!", Number = 32 });

			// push structure data to array
			// dbref.Push(new TestStruct { Text = "Hello, Firebase Database!", Number = 32 });
		}

		private async void GetData()
		{
			// get database object
			var db = FirebaseApp.Default.Database();

			// get database path reference
			var dbref = db.Ref("dbtest/data");

			// get data (was set 'Set' method) and display
			this.TestLabel.Content = (await dbref.GetObjectAsync<TestStruct>()).Text;

			// get datas array (were set 'Put' method) and display
			//this.TestLabel.Content = (await dbref.GetObjectsAsync<TestStruct>())[0].Text;
		}

		struct TestStruct
		{
			public string Text { get; set; }
			public int Number { get; set; }
		}
	}
}
