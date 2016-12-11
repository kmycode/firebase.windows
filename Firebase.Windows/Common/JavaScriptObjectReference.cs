using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firebase.Windows.Common
{
	/// <summary>
	/// Reference to JavaScript object
	/// </summary>
	internal class JavaScriptObjectReference
	{
		internal JavaScriptBinding JSBinding { get; }

		/// <summary>
		/// Name of the variable
		/// </summary>
		internal string VariableName { get; }

		internal bool IsNull
		{
			get
			{
				return (bool)this.JSBinding.ExecuteScript("return typeof variables." + this.VariableName + " === 'undefined'");
			}
		}

		internal JavaScriptObjectReference() : this(JavaScriptBinding.Default) { }

		internal JavaScriptObjectReference(JavaScriptBinding jsbinding)
			: this(jsbinding, "v" + Guid.NewGuid().ToString().Replace('-', '_'))
		{
		}

		internal JavaScriptObjectReference(JavaScriptBinding jsbinding, string variableName)
		{
			this.JSBinding = jsbinding;
			this.VariableName = variableName;
		}

		/// <summary>
		/// Set Script object for example json, string, ...
		/// </summary>
		/// <param name="valueScript">Script string to set value in JavaScript</param>
		internal void SetValue(string valueScript)
		{
			this.JSBinding.ExecuteScript("variables." + this.VariableName + " = " + valueScript + ";");
		}

		internal void SetBlob(byte[] data)
		{
			StringBuilder sb = new StringBuilder();
			foreach (byte b in data)
			{
				sb.Append(",");
				sb.Append(b);
			}
			sb.Remove(0, 1);            // first ','

			this.SetValue("new Blob([new Uint8Array([" + sb.ToString() + "])])");
		}

		/// <summary>
		/// Get Script object such as string
		/// </summary>
		/// <returns>Object string value</returns>
		internal string GetValue()
		{
			return (string)this.JSBinding.ExecuteScript("return \"\" + variables." + this.VariableName);
		}

		internal string GetJsonValue()
		{
			return (string)this.JSBinding.ExecuteScript("return \"\" + JSON.stringify(variables." + this.VariableName + ")");
		}

		internal void SetProperty(string propertyName, string valueScript)
		{
			this.JSBinding.ExecuteScript("variables." + this.VariableName + "." + propertyName + " = " + valueScript + ";");
		}

		internal string GetProperty(string propertyName)
		{
			return (string)this.JSBinding.ExecuteScript("return \"\" + variables." + this.VariableName + "." + propertyName);
		}

		internal bool GetPropertyToBool(string propertyName)
		{
			return (bool)this.JSBinding.ExecuteScript("return \"\" + variables." + this.VariableName + "." + propertyName);
		}

		internal JavaScriptObjectReferenceCollection GetPropertyToArray(string propertyName)
		{
			return this.JSBinding.ExecuteScriptToReferenceArray("variables." + this.VariableName + "." + propertyName);
		}

		internal JavaScriptObjectReference GetPropertyToReference(string propertyName)
		{
			var reference = new JavaScriptObjectReference();
			this.JSBinding.ExecuteScript("variables." + reference.VariableName + " = variables." + this.VariableName + "." + propertyName);
			return reference.IsNull ? null : reference;
		}

		internal void InvokeMethod(string methodName)
		{
			this.JSBinding.ExecuteScript("variables." + this.VariableName + "." + methodName + "();");
		}

		internal JavaScriptObjectReference InvokeMethodToReference(string methodName)
		{
			var reference = new JavaScriptObjectReference();
			this.JSBinding.ExecuteScript("variables." + reference.VariableName + " = variables." + this.VariableName + "." + methodName + "();");
			return reference.IsNull ? null : reference;
		}

		internal bool InvokeMethodToBool(string methodName)
		{
			return (bool)this.JSBinding.ExecuteScript("return variables." + this.VariableName + "." + methodName + "();");
		}

		internal string InvokeMethodToString(string methodName)
		{
			return (string)this.JSBinding.ExecuteScript("return variables." + this.VariableName + "." + methodName + "();");
		}

		internal JavaScriptObjectReferenceCollection InvokeMethodToArray(string methodName)
		{
			return this.JSBinding.ExecuteScriptToReferenceArray("variables." + this.VariableName + "." + methodName + "()");
		}

		internal JavaScriptObjectReferenceCollection InvokeMethodToArrayFromJson(string methodName)
		{
			return this.JSBinding.ExecuteScriptToReferenceArrayFromJson("variables." + this.VariableName + "." + methodName + "()");
		}

		internal void InvokeMethod(string methodName, string paramScript)
		{
			this.JSBinding.ExecuteScript("variables." + this.VariableName + "." + methodName + "(" + paramScript + ");");
		}

		internal JavaScriptObjectReference InvokeMethodToReference(string methodName, string paramScript)
		{
			var reference = new JavaScriptObjectReference();
			this.JSBinding.ExecuteScript("variables." + reference.VariableName + " = variables." + this.VariableName + "." + methodName + "(" + paramScript + ");");
			return reference.IsNull ? null : reference;
		}

		internal bool InvokeMethodToBool(string methodName, string paramScript)
		{
			return (bool)this.JSBinding.ExecuteScript("return variables." + this.VariableName + "." + methodName + "(" + paramScript + ");");
		}

		internal string InvokeMethodToString(string methodName, string paramScript)
		{
			return (string)this.JSBinding.ExecuteScript("return variables." + this.VariableName + "." + methodName + "(" + paramScript + ");");
		}

		internal JavaScriptObjectReferenceCollection InvokeMethodToArray(string methodName, string paramScript)
		{
			return this.JSBinding.ExecuteScriptToReferenceArray("variables." + this.VariableName + "." + methodName + "(" + paramScript + ")");
		}
	}

	internal class JavaScriptObjectReferenceCollection : Collection<JavaScriptObjectReference> { }
}
