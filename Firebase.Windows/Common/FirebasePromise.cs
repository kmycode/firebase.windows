using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Windows.Exceptions;

namespace Firebase.Windows.Common
{
	public class FirebasePromise
	{
		private JavaScriptObjectReference ResultReference;
		private JavaScriptObjectReference ErrorReference;
		private JavaScriptObjectReference ValueReference;

		private JavaScriptObjectReferenceWatcher ResultWatcher;

		internal FirebasePromise(JavaScriptObjectReference reference)
		{
			this.ErrorReference = new JavaScriptObjectReference(reference.JSBinding);
			this.ValueReference = new JavaScriptObjectReference(reference.JSBinding);

			// watching result status
			this.ResultReference = new JavaScriptObjectReference(reference.JSBinding);
			this.ResultWatcher = new JavaScriptObjectReferenceWatcher(this.ResultReference);
			this.ResultWatcher.ValueChanged += (sender, e) =>
			{
				this.CheckValue(e.NewValue);
			};

			// set result status when event occured
			reference.JSBinding.ExecuteScript($@"
				variables.{reference.VariableName}.then(function(result){{
					variables.{this.ResultReference.VariableName} = ""resolve"";
					variables.{this.ValueReference.VariableName} = result;
				}},function(error){{
					variables.{this.ResultReference.VariableName} = ""reject"";
					variables.{this.ErrorReference.VariableName} = """" + error.code;
				}}).catch(function(error){{
					variables.{this.ResultReference.VariableName} = ""reject"";
					variables.{this.ErrorReference.VariableName} = """" + error;
				}});
			");
		}

		public void StartReceiving()
		{
			if (this.ResultWatcher == null)
			{
				throw new FirebasePromissAlreadyCalledException();
			}
			if (!this.CheckValue(this.ResultReference.GetValue()))
			{
				this.ResultWatcher.Start();
			}
		}

		private bool CheckValue(string result)
		{
			bool hit = false;

			// check result
			if (result == "resolve")
			{
				this.Resolved?.Invoke(this, new ResolvedEventArgs(this.ValueReference));
				hit = true;
			}
			else if (result == "reject")
			{
				this.Rejected?.Invoke(this, new RejectedEventArgs(this.ErrorReference.GetValue()));
				hit = true;
			}

			// exit watching
			if (hit)
			{
				this.StatusChanged?.Invoke(this, new EventArgs());
				this.ResultWatcher.Stop();
				this.ResultWatcher = null;
			}

			return hit;
		}

		/// <summary>
		/// OnResolve
		/// </summary>
		public event ResolvedEventHandler Resolved;
		public delegate void ResolvedEventHandler(object sender, ResolvedEventArgs e);
		public class ResolvedEventArgs : EventArgs
		{
			internal ResolvedEventArgs(JavaScriptObjectReference reference)
			{
				this.Reference = reference;
			}
			internal JavaScriptObjectReference Reference { get; }
		}

		/// <summary>
		/// OnReject
		/// </summary>
		public event RejectedEventHandler Rejected;
		public delegate void RejectedEventHandler(object sender, RejectedEventArgs e);
		public class RejectedEventArgs : EventArgs
		{
			internal RejectedEventArgs(string code)
			{
				this.ErrorCode = code;
			}
			public string ErrorCode { get; }
		}

		/// <summary>
		/// Status Changed (resolve or reject)
		/// </summary>
		public event EventHandler StatusChanged;
	}
}
