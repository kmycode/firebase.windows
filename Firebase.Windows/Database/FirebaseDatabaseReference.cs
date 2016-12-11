using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Windows.Common;

namespace Firebase.Windows.Database
{
	public class FirebaseDatabaseReference
	{
		/// <summary>
		/// Firebase Auth Reference
		/// </summary>
		internal JavaScriptObjectReference Reference { get; }

		/// <summary>
		/// database reference's path
		/// </summary>
		public string Key
		{
			get
			{
				return this.Reference.GetProperty("key");
			}
		}

		/// <summary>
		/// database reference's path
		/// </summary>
		public string Path => this.Key;

		/// <summary>
		/// database this reference's parent
		/// </summary>
		public FirebaseDatabaseReference Parent
		{
			get
			{
				var reference = this.Reference.GetPropertyToReference("parent");
				if (reference == null || reference.IsNull) return null;
				return new FirebaseDatabaseReference(reference);
			}
		}

		/// <summary>
		/// database reference (self)
		/// </summary>
		public FirebaseDatabaseReference Ref
		{
			get
			{
				return new FirebaseDatabaseReference(this.Reference.GetPropertyToReference("ref"));
			}
		}

		/// <summary>
		/// database root
		/// </summary>
		public FirebaseDatabaseReference Root
		{
			get
			{
				return new FirebaseDatabaseReference(this.Reference.GetPropertyToReference("root"));
			}
		}

		/// <summary>
		/// Get auth object from JavaScript reference
		/// </summary>
		/// <param name="reference">JavaScript Database reference instance</param>
		internal FirebaseDatabaseReference(JavaScriptObjectReference reference)
		{
			if (reference == null || reference.IsNull) throw new NullReferenceException();
			this.Reference = reference;
		}

		/// <summary>
		/// get this reference child path
		/// </summary>
		/// <param name="path">child path name</param>
		/// <returns>child reference</returns>
		public FirebaseDatabaseReference Child(string path)
		{
			return new FirebaseDatabaseReference(this.Reference.InvokeMethodToReference("child", $"'{path}'"));
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
		/// push current data
		/// </summary>
		/// <returns>database reference</returns>
		public FirebaseDatabaseReference Push()
		{
			return new FirebaseDatabaseReference(this.Reference.InvokeMethodToReference("push"));
		}

		/// <summary>
		/// push current data
		/// </summary>
		/// <param name="value">the value written at generated location</param>
		/// <returns>database reference</returns>
		public FirebaseDatabaseReference Push(string value)
		{
			return new FirebaseDatabaseReference(this.Reference.InvokeMethodToReference("push", $"'{value}'"));
		}

		/// <summary>
		/// push current data
		/// </summary>
		/// <param name="value">the value written at generated location</param>
		/// <returns>database reference</returns>
		public FirebaseDatabaseReference Push(int value)
		{
			return new FirebaseDatabaseReference(this.Reference.InvokeMethodToReference("push", value.ToString()));
		}

		/// <summary>
		/// push current data
		/// </summary>
		/// <param name="value">the value written at generated location</param>
		/// <returns>database reference</returns>
		public FirebaseDatabaseReference Push(double value)
		{
			return new FirebaseDatabaseReference(this.Reference.InvokeMethodToReference("push", value.ToString()));
		}

		/// <summary>
		/// push current data
		/// </summary>
		/// <param name="value">the value written at generated location</param>
		/// <returns>database reference</returns>
		public FirebaseDatabaseReference Push(bool value)
		{
			return new FirebaseDatabaseReference(this.Reference.InvokeMethodToReference("push", value.ToString().ToLower()));
		}

		/// <summary>
		/// push current data
		/// </summary>
		/// <param name="value">the value written at generated location</param>
		/// <returns>database reference</returns>
		public FirebaseDatabaseReference Push(object value)
		{
			return new FirebaseDatabaseReference(this.Reference.InvokeMethodToReference("push", Newtonsoft.Json.JsonConvert.SerializeObject(value)));
		}

		/// <summary>
		/// set value on this reference
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise Set(string value)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("set", $"'{value}'"));
		}

		/// <summary>
		/// set value on this reference
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise Set(int value)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("set", value.ToString()));
		}

		/// <summary>
		/// set value on this reference
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise Set(double value)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("set", value.ToString()));
		}

		/// <summary>
		/// set value on this reference
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise Set(bool value)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("set", value.ToString().ToLower()));
		}

		/// <summary>
		/// set value on this reference
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise Set(object value)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("set", Newtonsoft.Json.JsonConvert.SerializeObject(value)));
		}

		/// <summary>
		/// update value on this reference
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise Update(string value)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("update", $"'{value}'"));
		}

		/// <summary>
		/// update value on this reference
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise Update(int value)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("update", value.ToString()));
		}

		/// <summary>
		/// update value on this reference
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise Update(double value)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("update", value.ToString()));
		}

		/// <summary>
		/// update value on this reference
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise Update(bool value)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("update", value.ToString().ToLower()));
		}

		/// <summary>
		/// update value on this reference
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise Update(object value)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("update", Newtonsoft.Json.JsonConvert.SerializeObject(value)));
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
				if (value != null)
					result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value.GetJsonValue());
				else
					result = default(T);
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
					if (value != null)
						result.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value.GetJsonValue()));
					else
						result.Add(default(T));
				}
			};
			promise.StartReceiving();
			await promise.WaitForStatusChangedAsync();

			return result;
		}

		/// <summary>
		/// set priority
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise SetPriority(string value)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("setPriority", $"'{value}'"));
		}

		/// <summary>
		/// set priority
		/// </summary>
		/// <param name="value">value</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise SetPriority(int value)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("setPriority", value.ToString()));
		}

		/// <summary>
		/// set priority
		/// </summary>
		/// <param name="newValue">new value</param>
		/// <param name="newPriority">new priority</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise SetWithPriority(string newValue, string newPriority)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("setWithPriority", $"'{newValue}','{newPriority}'"));
		}

		/// <summary>
		/// set priority
		/// </summary>
		/// <param name="newValue">new value</param>
		/// <param name="newPriority">new priority</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise SetWithPriority(int newValue, string newPriority)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("setWithPriority", $"{newValue},'{newPriority}'"));
		}

		/// <summary>
		/// set priority
		/// </summary>
		/// <param name="newValue">new value</param>
		/// <param name="newPriority">new priority</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise SetWithPriority(bool newValue, string newPriority)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("setWithPriority", $"{newValue.ToString().ToLower()},'{newPriority}'"));
		}

		/// <summary>
		/// set priority
		/// </summary>
		/// <param name="newValue">new value</param>
		/// <param name="newPriority">new priority</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise SetWithPriority(double newValue, string newPriority)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("setWithPriority", $"{newValue},'{newPriority}'"));
		}

		/// <summary>
		/// set priority
		/// </summary>
		/// <param name="newValue">new value</param>
		/// <param name="newPriority">new priority</param>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise SetWithPriority(object newValue, string newPriority)
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("setWithPriority", $"{Newtonsoft.Json.JsonConvert.SerializeObject(newValue)},'{newPriority}'"));
		}

		/// <summary>
		/// remove current data
		/// </summary>
		/// <returns>Firebase promise callbacks</returns>
		public FirebasePromise Remove()
		{
			return new FirebasePromise(this.Reference.InvokeMethodToReference("remove"));
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
