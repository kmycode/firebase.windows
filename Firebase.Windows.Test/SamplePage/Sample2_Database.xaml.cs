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

			if (this.SignIn())
			{
				this.TestLabel.Content = FirebaseApp.Default.Auth().CurrentUser.DisplayName;

				this.SetData();
				this.GetData();
			}
		}

		private bool SignIn()
		{
			//var promise = FirebaseApp.Default.Auth().CreateUserWithEmailAndPassword("test@example.com", "Asuka786");
			var promise = FirebaseApp.Default.Auth().SignInWithEmailAndPassword("test@example.com", "Asuka786");
			promise.Rejected += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = "認証失敗: " + e.ErrorCode);
			promise.StartReceiving();
			promise.WaitForStatusChanged();

			return FirebaseApp.Default.Auth().CurrentUser != null;
		}

		private void SetData()
		{
			var db = FirebaseApp.Default.Database();
			var dbref = db.Ref("dbtest/data");
			//dbref.Set("WPFからのテストだよ！");
			dbref.Set(new TestStruct { Text = "WPFから構造体をもっていきました", Number = 32 });
		}

		private async void GetData()
		{
			var db = FirebaseApp.Default.Database();
			var dbref = db.Ref("dbtest/data");
			//this.TestLabel.Content = (await dbref.GetObjectsAsync<TestStruct>())[0].Text;		// push
			this.TestLabel.Content = (await dbref.GetObjectAsync<TestStruct>()).Text;		// set
		}

		struct TestStruct
		{
			public string Text { get; set; }
			public int Number { get; set; }
		}
	}
}
