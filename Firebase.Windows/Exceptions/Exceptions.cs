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
}
