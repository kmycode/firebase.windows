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
			//this.SignIn();
			//this.SignOut();
		}

		private async void SignIn()
		{
			// auth with twitter
			//var promise = await FirebaseApp.Default.Auth().SignInWithTwitterAsync();

			// create new user with email-password
			//var promise = FirebaseApp.Default.Auth().CreateUserWithEmailAndPassword("___", "Asuka786");

			// sign in with email-password
			//var promise = FirebaseApp.Default.Auth().SignInWithEmailAndPassword("___", "Asuka786");

			// sign in anonymously
			var promise = FirebaseApp.Default.Auth().SignInAnonymously();

			// set promise event
			promise.Rejected += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = "Auth failed: " + e.ErrorCode);
			promise.Resolved += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = "Auth success");

			// start receiving callbacks
			promise.StartReceiving();
			promise.WaitForStatusChanged();

			// return if auth promise failed
			if (!promise.IsResolve) return;

			// display current user name
			this.Dispatcher.Invoke(() => this.TestLabel.Content = FirebaseApp.Default.Auth().CurrentUser.DisplayName);
		}

		// see https://firebase.google.com/docs/auth/web/account-linking
		private async void Link()
		{
			// get new credential
			var newCredential = await FirebaseApp.Default.Auth().GetTwitterCredentialAsync();

			// try link accounts
			var linkPromise = FirebaseApp.Default.Auth().CurrentUser.Link(newCredential);

			// set link callbacks
			linkPromise.Resolved += (ss, ee) =>
			{
				this.Dispatcher.Invoke(() => this.TestLabel.Content = "Success to link");
			};
			linkPromise.Rejected += (ss, ee) =>
			{
				this.Dispatcher.Invoke(() => this.TestLabel.Content = "Link failed: " + ee.ErrorCode);
				FirebaseApp.Default.Auth().CurrentUser.Delete();
			};

			// start receiving callbacks
			linkPromise.StartReceiving();
		}

		private void SignOut()
		{
			// sign out
			var promise = FirebaseApp.Default.Auth().SignOut();
			promise.Rejected += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = "サインアウト失敗: " + e.ErrorCode);
			promise.Resolved += (sender, e) => this.Dispatcher.Invoke(() => this.TestLabel.Content = "サインアウトに成功しました");
			promise.StartReceiving();
		}
	}
}
