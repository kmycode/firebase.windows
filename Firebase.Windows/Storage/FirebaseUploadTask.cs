using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Windows.Common;
using Firebase.Windows.Exceptions;

namespace Firebase.Windows.Storage
{
	public class FirebaseUploadTask
	{
		private JavaScriptObjectReference ResultReference;
		private JavaScriptObjectReference ErrorReference;
		private JavaScriptObjectReference ValueReference;

		private JavaScriptObjectReferenceWatcher ResultWatcher;

		/// <summary>
		/// Firebase Auth Reference
		/// </summary>
		private JavaScriptObjectReference Reference { get; }

		public string Result
		{
			get
			{
				return this.ResultReference.GetValue();
			}
		}

		public bool IsResolve
		{
			get
			{
				return this.Result == "resolve";
			}
		}

		public bool IsReject
		{
			get
			{
				return this.Result == "reject";
			}
		}

		internal FirebaseUploadTask(JavaScriptObjectReference reference)
		{
			this.Reference = reference;

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
					variables.{this.ResultReference.VariableName} = ""fulfilled"";
					variables.{this.ValueReference.VariableName} = result;
				}},function(error){{
					variables.{this.ResultReference.VariableName} = ""rejected"";
					variables.{this.ErrorReference.VariableName} = """" + error.code;
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
			if (result == "fulfilled")
			{
				this.Fulfilled?.Invoke(this, new FulFilledEventArgs(this.ValueReference));
				hit = true;
			}
			else if (result == "rejected")
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
		/// wait until status changed
		/// </summary>
		public void WaitForStatusChanged()
		{
			while (this.ResultWatcher != null)
			{
				Task.Delay(10).Wait();
			}
		}

		/// <summary>
		/// wait until status changed
		/// </summary>
		public async Task WaitForStatusChangedAsync()
		{
			while (this.ResultWatcher != null)
			{
				await Task.Delay(10);
			}
		}

		/// <summary>
		/// cancel upload task
		/// </summary>
		/// <returns>success or not</returns>
		public bool Cancel()
		{
			this.ResultWatcher.Stop();
			return this.Reference.InvokeMethodToBool("cancel");
		}

		/// <summary>
		/// pause the task
		/// </summary>
		/// <returns>success or not</returns>
		public bool Pause()
		{
			return this.Reference.InvokeMethodToBool("pause");
		}

		/// <summary>
		/// resume the task
		/// </summary>
		/// <returns>success or not</returns>
		public bool Resume()
		{
			return this.Reference.InvokeMethodToBool("resume");
		}

		/// <summary>
		/// OnResolve
		/// </summary>
		public event FulFilledEventHandler Fulfilled;
		public delegate void FulFilledEventHandler(object sender, FulFilledEventArgs e);
		public class FulFilledEventArgs : EventArgs
		{
			internal FulFilledEventArgs(JavaScriptObjectReference reference)
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
