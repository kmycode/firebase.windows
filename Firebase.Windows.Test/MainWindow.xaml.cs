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

namespace Firebase.Windows.Test
{
	/// <summary>
	/// MainWindow.xaml の相互作用ロジック
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			FirebaseApp.InitializeApp(new FirebaseAppOptions
			{
				ApiKey = "---",
				AuthDomain = "---",
				DatabaseUrl = "---",
				StorageBucket = "---",
				MessagingSenderId = "---",
				FirebaseHtmlUrl = "---",
			});

			InitializeComponent();
		}
	}
}
