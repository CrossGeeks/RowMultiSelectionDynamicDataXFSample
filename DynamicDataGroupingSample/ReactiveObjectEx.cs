using System;
using System.Reactive.Disposables;
using ReactiveUI;

namespace DynamicDataGroupingSample
{
    public abstract class ReactiveObjectEx : ReactiveObject, IDisposable
    {
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose all of your Rx subscriptions with this property. 
        /// </summary>
        protected readonly CompositeDisposable Subscriptions = new CompositeDisposable();

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Subscriptions?.Dispose();
            }
        }
    }
}