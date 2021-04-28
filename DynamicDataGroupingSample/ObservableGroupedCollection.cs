using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace DynamicDataGroupingSample
{
    public class ObservableGroupedCollection<TGroupKey, TObject, TKey> : ObservableCollectionExtended<TObject>, IDisposable
    {
        public TGroupKey Key { get; }

        public ObservableGroupedCollection(IGroup<TObject, TKey, TGroupKey> group, IObservable<Func<TObject, bool>> filter = null, IObservable<IComparer<TObject>> comparer = null)
        {
            this.Key = group.Key;

            //load and sort the grouped list
            var dataLoader = group.Cache.Connect()
                .RefCount()
                .Filter(filter ?? Observable.Return<Func<TObject, bool>>(x => true))
                .Sort(comparer ?? Observable.Return<IComparer<TObject>>(SortExpressionComparer<TObject>.Ascending(a => a.ToString())))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(this) //make the reset threshold large because xamarin is slow when reset is called (or at least I think it is @erlend, please enlighten me )
                .Subscribe();

            _cleanUp = new CompositeDisposable(dataLoader);
        }

        public void Dispose()
        {
            _cleanUp.Dispose();
        }

        private readonly IDisposable _cleanUp;
    }
}
