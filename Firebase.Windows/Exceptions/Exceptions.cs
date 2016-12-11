using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firebase.Windows.Exceptions
{
	public class FirebaseException : Exception
	{
		public FirebaseException() { }
		public FirebaseException(string message) : base(message) { }
		public FirebaseException(string message, Exception innerException) : base(message, innerException) { }
		public FirebaseException(Exception innerException) : base(innerException.GetType().Name + ": " + innerException.Message, innerException) { }
	}

	public class FirebaseNotInitializedException : FirebaseException
	{
		public FirebaseNotInitializedException() { }
		public FirebaseNotInitializedException(string message) : base(message) { }
	}

	public class FirebasePromissAlreadyCalledException : FirebaseException
	{
		public FirebasePromissAlreadyCalledException() { }
		public FirebasePromissAlreadyCalledException(string message) : base(message) { }
	}

	public class JavaScriptBindingException : FirebaseException
	{
		public JavaScriptBindingException() { }
		public JavaScriptBindingException(string message) : base(message) { }
		public JavaScriptBindingException(string message, Exception innerException) : base(message, innerException) { }
	}

	public class ChromeBrowserException : FirebaseException
	{
		public ChromeBrowserException() { }
		public ChromeBrowserException(string message) : base(message) { }
		public ChromeBrowserException(string message, Exception innerException) : base(message, innerException) { }
	}

	public class ChromeBrowserWaitingAuthException : ChromeBrowserException
	{
		public ChromeBrowserWaitingAuthException() { }
		public ChromeBrowserWaitingAuthException(string message) : base(message) { }
		public ChromeBrowserWaitingAuthException(string message, Exception innerException) : base(message, innerException) { }
	}

	public class ChromeBrowserWaitingAuthResultException : ChromeBrowserException
	{
		public ChromeBrowserWaitingAuthResultException() { }
		public ChromeBrowserWaitingAuthResultException(string message) : base(message) { }
		public ChromeBrowserWaitingAuthResultException(string message, Exception innerException) : base(message, innerException) { }
	}
}
