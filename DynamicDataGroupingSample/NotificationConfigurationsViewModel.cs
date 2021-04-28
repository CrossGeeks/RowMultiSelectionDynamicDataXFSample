using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace DynamicDataGroupingSample
{
    public class NotificationConfigurationViewModel : ReactiveObjectEx 
    {
        public NotificationConfigurationViewModel()
        {
            _notificationConfigurationsSourceCache.AddOrUpdate(new List<NotificationConfiguration>()
            {
                new NotificationConfiguration("Information", 0,"Development", 0,false, false, true, false),
                new NotificationConfiguration("Status Changed", 1,"Administration", 1, false, true, false, false),
                new NotificationConfiguration("Error", 3,"Development", 0, false, true, false, false),
                new NotificationConfiguration("New Report", 4,"Administration", 1, false, false, false, false),
                new NotificationConfiguration("Warning", 1,"Development", 0, false, true, false, false),
                new NotificationConfiguration("Security Update", 2, "Administration", 1, false, false, true, true),
                new NotificationConfiguration("Verbose", 2, "Development", 0, false, false, false, false),
                new NotificationConfiguration("Policy Update", 3, "Administration", 1, false, false, false, false)
            });
           
            var smsChannelChangedCommand = ReactiveCommand.CreateFromTask<NotificationConfiguration>(OnSmsChannelChanged);
            var emailChannelChangedCommand = ReactiveCommand.CreateFromTask<NotificationConfiguration>(OnEmailChannelChanged);
            var pushNotificationChannelChangedCommand = ReactiveCommand.CreateFromTask<NotificationConfiguration>(OnPushNotificationChannelChanged);
            var phoneChannelChangedCommand = ReactiveCommand.CreateFromTask<NotificationConfiguration>(OnPhoneChannelChanged);

            var notificationConfigurationItemChanges = _notificationConfigurationsSourceCache.Connect()
                                                                                             .RefCount();

            notificationConfigurationItemChanges
                        .Sort(SortExpressionComparer<NotificationConfiguration>.Ascending(a => a.GroupOrder))
                        .Group(x => x.GroupName)
                        .Transform(g => new ObservableGroupedCollection<string, NotificationConfiguration, string>(g, null, Observable.Return<IComparer<NotificationConfiguration>>(SortExpressionComparer<NotificationConfiguration>.Ascending(a => a.Order))))
                        .Bind(out _groups)
                        .DisposeMany()
                        .Subscribe()
                        .DisposeWith(Subscriptions);

            notificationConfigurationItemChanges
                        .WhenPropertyChanged(x => x.SmsChannel , notifyOnInitialValue: false)
                        .Throttle(TimeSpan.FromMilliseconds(SelectionDueMilliseconds), RxApp.TaskpoolScheduler)
                        .Select(x => x.Sender)
                        .InvokeCommand(smsChannelChangedCommand)
                        .DisposeWith(Subscriptions);

            notificationConfigurationItemChanges
                        .WhenPropertyChanged(x => x.EmailChannel, notifyOnInitialValue: false)
                        .Throttle(TimeSpan.FromMilliseconds(SelectionDueMilliseconds), RxApp.TaskpoolScheduler)
                        .Select(x => x.Sender)
                        .InvokeCommand(emailChannelChangedCommand)
                        .DisposeWith(Subscriptions);

            notificationConfigurationItemChanges
                        .WhenPropertyChanged(x => x.PushNotificationChannel, notifyOnInitialValue: false)
                        .Throttle(TimeSpan.FromMilliseconds(SelectionDueMilliseconds), RxApp.TaskpoolScheduler)
                        .Select(x => x.Sender)
                        .InvokeCommand(pushNotificationChannelChangedCommand)
                        .DisposeWith(Subscriptions);

            notificationConfigurationItemChanges
                      .WhenPropertyChanged(x => x.PhoneChannel, notifyOnInitialValue: false)
                      .Throttle(TimeSpan.FromMilliseconds(SelectionDueMilliseconds), RxApp.TaskpoolScheduler)
                      .Select(x => x.Sender)
                      .InvokeCommand(phoneChannelChangedCommand)
                      .DisposeWith(Subscriptions);
        }

        private Task OnPhoneChannelChanged(NotificationConfiguration notificationConfiguration)
        {
            System.Diagnostics.Debug.WriteLine($"Phone Channel - Changed - {notificationConfiguration.GroupName}.{notificationConfiguration.Name} - {notificationConfiguration.PhoneChannel}");

            return Task.CompletedTask;
        }

        private Task OnPushNotificationChannelChanged(NotificationConfiguration notificationConfiguration)
        {
            System.Diagnostics.Debug.WriteLine($"Push Notification Channel - Changed - {notificationConfiguration.GroupName}.{notificationConfiguration.Name} - {notificationConfiguration.PushNotificationChannel}");

            return Task.CompletedTask;
        }

        private Task OnEmailChannelChanged(NotificationConfiguration notificationConfiguration)
        {
            System.Diagnostics.Debug.WriteLine($"Email Notification Channel - Changed - {notificationConfiguration.GroupName}.{notificationConfiguration.Name} - {notificationConfiguration.EmailChannel}");

            return Task.CompletedTask;
        }

        private Task OnSmsChannelChanged(NotificationConfiguration notificationConfiguration)
        {
            System.Diagnostics.Debug.WriteLine($"SMS Notification Channel - Changed - {notificationConfiguration.GroupName}.{notificationConfiguration.Name} - {notificationConfiguration.SmsChannel}");

            return Task.CompletedTask;
        }

        public ReadOnlyObservableCollection<ObservableGroupedCollection<string, NotificationConfiguration, string>> NotificationGroups => _groups;

        private readonly ReadOnlyObservableCollection<ObservableGroupedCollection<string, NotificationConfiguration, string>> _groups;

        private SourceCache<NotificationConfiguration, string> _notificationConfigurationsSourceCache = new SourceCache<NotificationConfiguration, string>(x => x.Name);


        private const int SelectionDueMilliseconds = 250;
    }
}