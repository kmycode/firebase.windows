using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firebase.Windows.Common
{
	/// <summary>
	/// Monitor JavaScript object value chages
	/// </summary>
	internal class JavaScriptObjectReferenceWatcher : IDisposable
	{
		/// <summary>
		/// ValueChanged event.
		/// This event is called by async threads.
		/// </summary>
		internal event ValueChangedEventHandler ValueChanged;
		internal delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);
		internal class ValueChangedEventArgs : EventArgs
		{
			internal ValueChangedEventArgs(string oldValue, string newValue)
			{
				this.OldValue = oldValue;
				this.NewValue = newValue;
			}
			internal string NewValue { get; }
			internal string OldValue { get; }
		}

		private JavaScriptObjectReference Reference { get; }
		private string _oldValue;
		private volatile bool _isStopped;

		public JavaScriptObjectReferenceWatcher(JavaScriptObjectReference reference)
		{
			this.Reference = reference;

			this._oldValue = reference.GetValue();
		}

		/// <summary>
		/// Stop watching
		/// </summary>
		internal void Stop()
		{
			this.Dispose();
		}

		/// <summary>
		/// Start watching
		/// </summary>
		internal void Start()
		{
			// start watching value reference
			Task.Run(() =>
			{
				while (!this._isStopped)
				{
					string newValue = this.Reference.GetValue();
					if (newValue != this._oldValue)
					{
						this.ValueChanged?.Invoke(this, new ValueChangedEventArgs(this._oldValue, newValue));
						this._oldValue = newValue;
					}
					Task.Delay(50).Wait();
				}
			});
		}

		public void Dispose()
		{
			this._isStopped = true;
		}
	}
}
