using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Windows.Exceptions;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;

namespace Firebase.Windows.Common
{
	/// <summary>
	/// JavaScript Binding class.
	/// This class is able to run JavaScript backend.
	/// </summary>
	internal class JavaScriptBinding : IDisposable
	{
		private static JavaScriptBinding _default;
		internal static JavaScriptBinding Default
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
		
		private IWebDriver _driver;
		private IJavaScriptExecutor _javaScriptExecutor;
		
		internal JavaScriptBinding(string url, IWebDriver webDriver = null)
		{
			//var html = @"<!DOCTYPE html/><html><head><script src=""https://www.gstatic.com/firebasejs/3.6.3/firebase.js""></script><script>var variables={};</script></head><body></body></html>";

			// create dummy HTML loading firebase for access from Selenium
			//StreamWriter sw = new StreamWriter("./firebase.html", false, Encoding.UTF8);
			//sw.Write(html);
			//sw.Close();

			// connect to dummy HTML
			if (webDriver == null)
			{
				var driverService = PhantomJSDriverService.CreateDefaultService();
				driverService.HideCommandPromptWindow = true;
				this._driver = new PhantomJSDriver(driverService, new PhantomJSOptions());
				//this._driver.Navigate().GoToUrl("./firebase.html");
			}
			else
			{
				this._driver = webDriver;
			}
			this._driver.Navigate().GoToUrl(url);

			// delete dummy HTML
			//File.Delete("./firebase.html");

			// get javascript executor
			this._javaScriptExecutor = this._driver as IJavaScriptExecutor;
			if (this._javaScriptExecutor == null)
			{
				throw new WebDriverException("This driver doesn't support JavaScript execution.");
			}

			// set default
			if (_default == null)
			{
				_default = this;
			}
		}

		/// <summary>
		/// Execute JavaScript
		/// </summary>
		/// <param name="script">Script string</param>
		/// <returns>Script result</returns>
		internal object ExecuteScript(string script)
		{
			System.Diagnostics.Debug.WriteLine(script);
			return this._javaScriptExecutor.ExecuteScript(script);
		}

		/// <summary>
		/// Execute JavaScript async
		/// </summary>
		/// <param name="script">Script string</param>
		/// <returns>Script result</returns>
		internal object ExecuteScriptAsync(string script)
		{
			return this._javaScriptExecutor.ExecuteAsyncScript(script);
		}

		/// <summary>
		/// Dispose driver
		/// </summary>
		public void Dispose()
		{
			this._driver.Dispose();
		}
	}
}
