using System;

namespace LightRx
{
	public sealed class SerialDisposable : IDisposable, ICancelable
	{
		IDisposable current;
		bool canceled;

		public bool IsCanceled { get {  return canceled; } }

		public IDisposable Disposable
		{
			get
			{
				return current;
			}
			set
			{
				var shouldDispose = false;
				var old = default(IDisposable);
			
				shouldDispose = canceled;
				if (!shouldDispose)
				{
					old = current;
					current = value;
				}
			
				if (old != null)
				{
					old.Dispose();
				}
			
				if (shouldDispose && value != null)
				{
					value.Dispose();
				}
			}
		}

		public void Dispose()
		{
			var old = default(IDisposable);

			if (!canceled)
			{
				canceled = true;
				old = current;
				current = null;
			}

			if (old != null)
			{
				old.Dispose();
			}
		}
	}
}
