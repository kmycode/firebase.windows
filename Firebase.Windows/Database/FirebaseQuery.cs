using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		/// handle once value changed event
		/// </summary>
		/// <returns>Firebase promise callbacks</returns>
		internal FirebasePromise Once()
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("once", "'value'"));
		}

		/// <summary>
		/// Get value at this reference
		/// </summary>
		/// <returns>database reference value</returns>
		public async Task<string> GetStringAsync()
		{
			string result = null;
			var promise = this.Once();
			promise.Resolved += (sender, e) => result = e.Reference.InvokeMethodToReference("val").GetValue();
			promise.StartReceiving();
			await promise.WaitForStatusChangedAsync();

			return result;
		}

		/// <summary>
		/// Get value at this reference
		/// </summary>
		/// <returns>database reference value</returns>
		public async Task<int> GetIntegerAsync()
		{
			int result = 0;
			var promise = this.Once();
			promise.Resolved += (sender, e) => result = int.Parse(e.Reference.InvokeMethodToReference("val").GetValue());
			promise.StartReceiving();
			await promise.WaitForStatusChangedAsync();

			return result;
		}

		/// <summary>
		/// Get value at this reference
		/// </summary>
		/// <returns>database reference value</returns>
		public async Task<double> GetDoubleAsync()
		{
			double result = 0;
			var promise = this.Once();
			promise.Resolved += (sender, e) => result = double.Parse(e.Reference.InvokeMethodToReference("val").GetValue());
			promise.StartReceiving();
			await promise.WaitForStatusChangedAsync();

			return result;
		}

		/// <summary>
		/// Get value at this reference
		/// </summary>
		/// <returns>database reference value</returns>
		public async Task<bool> GetBoolAsync()
		{
			bool result = false;
			var promise = this.Once();
			promise.Resolved += (sender, e) => result = bool.Parse(e.Reference.InvokeMethodToReference("val").GetValue());
			promise.StartReceiving();
			await promise.WaitForStatusChangedAsync();

			return result;
		}

		/// <summary>
		/// Get value at this reference
		/// </summary>
		/// <returns>database reference value</returns>
		public async Task<T> GetObjectAsync<T>()
		{
			T result = default(T);
			var promise = this.Once();
			promise.Resolved += (sender, e) =>
			{
				var value = e.Reference.InvokeMethodToReference("val");
				result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value.GetJsonValue());
			};
			promise.StartReceiving();
			await promise.WaitForStatusChangedAsync();

			return result;
		}

		/// <summary>
		/// Get value at this reference
		/// </summary>
		/// <returns>database reference value</returns>
		public async Task<Collection<T>> GetObjectsAsync<T>()
		{
			Collection<T> result = new Collection<T>();
			var promise = this.Once();
			promise.Resolved += (sender, e) =>
			{
				var values = e.Reference.InvokeMethodToArrayFromJson("val");
				foreach (var value in values)
				{
					result.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value.GetJsonValue()));
				}
			};
			promise.StartReceiving();
			await promise.WaitForStatusChangedAsync();

			return result;
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
