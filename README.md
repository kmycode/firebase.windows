# firebase.windows
Firebase for Windows (for example WPF) It will support Auth, Database and Storage.

## Getting started

### Prepare Firebase account

Go to https://firebase.google.com/ and make Firebase account and project.

### Set dummy HTML file to the step of connect Firebase SDK

Make HTML file with following code:

```html
<!DOCTYPE html/>
<html>
<head>
	<script src="https://www.gstatic.com/firebasejs/3.6.3/firebase.js"></script>
	<script>
var variables = [];
	</script>
</head>
<body>
</body>
</html>
```

Next, upload the HTML file to remote for example your HP spaces or localhost.

Next, go Firebase console and [Authentication] -> [Sign-in Method] -> [OAuth Redirect Domains] and add the domain of dummy HTML file.

### Download Selenium drivers

You must put one or two exe file(s) on same path of your application.

1. [PhantomJS](http://phantomjs.org/) - "phantomjs.exe"
1. [Selenium Google Chrome Driver](http://www.seleniumhq.org/download/) - "chromedriver.exe" if you use Twitter, Facebook and GitHub authentications.

Put those file to bin\Debug directory.

### Write initialize

```csharp
FirebaseApp.InitializeApp(new FirebaseAppOptions
{
	ApiKey = "<your key>",
	AuthDomain = "<your domain>",
	DatabaseUrl = "<your url>",
	StorageBucket = "<your bucket>",
	MessagingSenderId = "<your id>",
	FirebaseHtmlUrl = "<your dummy HTML url>",
});
```

### Read sample codes and run!

[Sample codes are here.](https://github.com/kmycode/firebase.windows/tree/master/Firebase.Windows.Test/SamplePage) Let's enjoy!

## Addtional informations (Japanese)

[Qiitaに説明記事をあげています](http://qiita.com/askyq/items/91269544fd2e4d9c18bc)

現状でできることは以下のとおりです

* WPFからFirebaseを利用
* Auth（Email-Password・Twitter・Facebook・GitHub・匿名認証、サインアウト、アカウントのリンク、プロフィール変更など）
* Database（データのセット、プッシュ、取得（文字列、数値、浮動小数点数、任意のオブジェクトに対応）、Queryを使った簡単な操作など）
* Storage（文字列・バイナリデータのアップロード、ダウンロードURL取得など）

まだ実装されていない機能は以下のとおりです

* Databaseのトランザクション（後回し）
* StorageのMetaDataとか
* Database、Storageのon、offなど、各種イベントの処理
* その他細かいところ
