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
	/// Sample1_Auth.xaml の相互作用ロジック
	/// </summary>
	public partial class Sample1_Auth : Page
	{
		public Sample1_Auth()
		{
			InitializeComponent();
			this.FirebaseTest();
		}

		private void FirebaseTest()
		{
			this.SignIn();
			//this.SignOut();
		}

		private void SignIn()
		{
			//var promise = await FirebaseApp.Default.Auth().SignInWithTwitterAsync();
			//var promise = FirebaseApp.Default.Auth().CreateUserWithEmailAndPassword("tt@kmycode.net", "Asuka786");
			var promise = FirebaseApp.Default.Auth().SignInWithEmailAndPassword("tt@kmycode.net", "Asuka786");
			promise.Rejected += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = "認証失敗: " + e.ErrorCode);
			promise.Resolved += (sender, e) =>
			{
				this.Dispatcher.Invoke(() => this.TestLabel.Content = FirebaseApp.Default.Auth().CurrentUser.DisplayName);
			};
			promise.StartReceiving();
		}

		private void SignOut()
		{
			var promise = FirebaseApp.Default.Auth().SignOut();
			promise.Rejected += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = "サインアウト失敗: " + e.ErrorCode);
			promise.Resolved += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = "サインアウトに成功しました");
			promise.StartReceiving();
		}
	}
}
