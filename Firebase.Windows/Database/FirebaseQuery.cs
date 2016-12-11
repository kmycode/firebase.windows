using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Windows.Common;

namespace Firebase.Windows.Database
{
	public class FirebaseQuery
	{
		/// <summary>
		/// Firebase Auth Reference
		/// </summary>
		private JavaScriptObjectReference Reference { get; }

		/// <summary>
		/// database reference
		/// </summary>
		public FirebaseDatabaseReference Ref
		{
			get
			{
				return new FirebaseDatabaseReference(this.Reference.GetPropertyToReference("ref"));
			}
		}

		/// <summary>
		/// Get auth object from JavaScript reference
		/// </summary>
		/// <param name="reference">JavaScript Database query reference instance</param>
		internal FirebaseQuery(JavaScriptObjectReference reference)
		{
			if (reference == null || reference.IsNull) throw new NullReferenceException();
			this.Reference = reference;
		}

		/// <summary>
		/// creates a query with start point
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>database query</returns>
		public FirebaseQuery StartAt(string value)
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("startAt", $"'{value}'"));
		}

		/// <summary>
		/// creates a query with start point
		/// </summary>
		/// <param name="value">value</param>
		/// <param name="key">key</param>
		/// <returns>database query</returns>
		public FirebaseQuery StartAt(string value, string key)
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("startAt", $"'{value}','{key}'"));
		}

		/// <summary>
		/// creates a query with end point
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>database query</returns>
		public FirebaseQuery EndAt(string value)
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("endAt", $"'{value}'"));
		}

		/// <summary>
		/// creates a query with end point
		/// </summary>
		/// <param name="value">value</param>
		/// <param name="key">key</param>
		/// <returns>database query</returns>
		public FirebaseQuery EndAt(string value, string key)
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("endAt", $"'{value}','{key}'"));
		}

		/// <summary>
		/// check equal value
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>database query</returns>
		public FirebaseQuery EqualTo(string value)
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("equalTo", $"'{value}'"));
		}

		/// <summary>
		/// check equal value
		/// </summary>
		/// <param name="value">value</param>
		/// <param name="key">key</param>
		/// <returns>database query</returns>
		public FirebaseQuery EqualTo(string value, string key)
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("equalTo", $"'{value}','{key}'"));
		}

		/// <summary>
		/// check equal value
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>database query</returns>
		public FirebaseQuery EqualTo(int value)
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("equalTo", value.ToString()));
		}

		/// <summary>
		/// check equal value
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>database query</returns>
		public FirebaseQuery EqualTo(int value, string key)
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("equalTo", $"'{value}','{key}'"));
		}

		/// <summary>
		/// check equal value
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>database query</returns>
		public FirebaseQuery EqualTo(double value)
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("equalTo", value.ToString()));
		}

		/// <summary>
		/// check equal value
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>database query</returns>
		public FirebaseQuery EqualTo(double value, string key)
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("equalTo", $"'{value}','{key}'"));
		}

		/// <summary>
		/// check equals value
		/// </summary>
		/// <param name="reference">other reference</param>
		/// <returns>equals or not</returns>
		public bool IsEqual(FirebaseDatabaseReference reference)
		{
			return this.Reference.InvokeMethodToBool("isEqual", "variables." + reference.Reference.VariableName);
		}

		/// <summary>
		/// limited to the first number of children
		/// </summary>
		/// <param name="limit">limit</param>
		/// <returns>database query</returns>
		public FirebaseQuery LimitToFirst(int limit)
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("limitToFirst", limit.ToString()));
		}

		/// <summary>
		/// limited to the last number of children
		/// </summary>
		/// <param name="limit">limit</param>
		/// <returns>database query</returns>
		public FirebaseQuery LimitToLast(int limit)
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("limitToLast", limit.ToString()));
		}

		/// <summary>
		/// order by data child
		/// </summary>
		/// <param name="path">child member path</param>
		/// <returns>database query</returns>
		public FirebaseQuery OrderByChild(string path)
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("orderByChild", $"'{path}'"));
		}

		/// <summary>
		/// order by key name
		/// </summary>
		/// <returns>database query</returns>
		public FirebaseQuery OrderByKey()
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("orderByKey"));
		}

		/// <summary>
		/// order by priority
		/// </summary>
		/// <returns>database query</returns>
		public FirebaseQuery OrderByPriority()
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("orderByPriority"));
		}

		/// <summary>
		/// order by value
		/// </summary>
		/// <returns>database query</returns>
		public FirebaseQuery OrderByValue()
		{
			return new FirebaseQuery(this.Reference.InvokeMethodToReference("orderByValue"));
		}

		/// <summary>
		/// get database url string (call Javascript 'toString' method)
		/// </summary>
		/// <returns>database url string</returns>
		public string ToUrlString()
		{
			return this.Reference.InvokeMethodToString("toString");
		}
	}
}
