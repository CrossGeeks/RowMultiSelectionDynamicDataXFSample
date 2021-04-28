using Xamarin.Forms;

namespace DynamicDataGroupingSample
{
    public partial class NotificationConfigurationsPage : ContentPage
    {
        public NotificationConfigurationsPage()
        {
            InitializeComponent();
            BindingContext = new NotificationConfigurationViewModel();
        }
    }
}
