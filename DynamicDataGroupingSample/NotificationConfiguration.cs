using ReactiveUI;

namespace DynamicDataGroupingSample
{
    public class NotificationConfiguration : ReactiveObjectEx
    {
        public NotificationConfiguration(string name, int order, string groupName, int groupOrder,bool smsChannel, bool emailChannel, bool pushNotificationChannel, bool phoneChannel)
        {
            Name = name;
            Order = order;
            GroupName = groupName;
            GroupOrder = groupOrder;
            SmsChannel = smsChannel;
            EmailChannel = emailChannel;
            PushNotificationChannel = pushNotificationChannel;
            PhoneChannel = phoneChannel;
        }

        public string Name { get;}
        public int Order { get; }

        //Group
        public string GroupName { get; }
        public int GroupOrder { get; }
        public string GroupCssIcon { get; }
        public string GroupFontIcon { get; }

        //Channels
        public bool SmsChannel
        {
            get => _smsChannel;
            set => this.RaiseAndSetIfChanged(ref _smsChannel, value);
        }

        public bool EmailChannel
        {
            get => _emailChannel;
            set => this.RaiseAndSetIfChanged(ref _emailChannel, value);
        }

        public bool PushNotificationChannel
        {
            get => _pushNotificationChannel;
            set => this.RaiseAndSetIfChanged(ref _pushNotificationChannel, value);
        }

        public bool PhoneChannel
        {
            get => _phoneChannel;
            set => this.RaiseAndSetIfChanged(ref _phoneChannel, value);
        }

        private bool _smsChannel;
        private bool _emailChannel;
        private bool _pushNotificationChannel;
        private bool _phoneChannel;
    }
}
