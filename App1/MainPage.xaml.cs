using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Notifications;
using Windows.UI.Notifications.Management;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Threading.Tasks;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace App1
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        UserNotificationListener listener;
        FileSavePicker fileSavePicker;
        StorageFile storageFile;


        public MainPage()
        {
            this.InitializeComponent();

            listener = UserNotificationListener.Current;
        }




        private async void Listener_NotificationChanged(UserNotificationListener sender, UserNotificationChangedEventArgs args)
        {
            if(args.ChangeKind == UserNotificationChangedKind.Added)
            {


                UserNotification temp = listener.GetNotification(args.UserNotificationId);

                // Get the toast binding, if present
                NotificationBinding toastBinding = temp.Notification.Visual.GetBinding(KnownNotificationBindings.ToastGeneric);

                if (toastBinding != null)
                {
                    // And then get the text elements from the toast binding
                    IReadOnlyList<AdaptiveNotificationText> textElements = toastBinding.GetTextElements();

                    // Treat the first text element as the title text
                    string titleText = textElements.FirstOrDefault()?.Text;

                    // We'll treat all subsequent text elements as body text,
                    // joining them together via newlines.
                    string bodyText = string.Join("\n", textElements.Skip(1).Select(t => t.Text));

                    // 여기에 문자가옴

                   
                    // 
                    //await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    //async () =>
                    //{
                    //    await new MessageDialog(titleText, "Title of the message dialog").ShowAsync();
                    //    await new MessageDialog(bodyText, "Title of the message dialog").ShowAsync();
                    //});

                   
                }


            }
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            if (ApiInformation.IsTypePresent("Windows.UI.Notifications.Management.UserNotificationListener"))
            {
                // And request access to the user's notifications (must be called from UI thread)
                UserNotificationListenerAccessStatus accessStatus = await listener.RequestAccessAsync();

                switch (accessStatus)
                {
                    // This means the user has granted access.
                    case UserNotificationListenerAccessStatus.Allowed:
                        await new MessageDialog("OK", "Title of the message dialog").ShowAsync();
                        // Yay! Proceed as normal
                        break;

                    // This means the user has denied access.
                    // Any further calls to RequestAccessAsync will instantly
                    // return Denied. The user must go to the Windows settings
                    // and manually allow access.
                    case UserNotificationListenerAccessStatus.Denied:
                        await new MessageDialog("Denied", "Title of the message dialog").ShowAsync();
                        // Show UI explaining that listener features will not
                        // work until user allows access.
                        break;

                    // This means the user closed the prompt without
                    // selecting either allow or deny. Further calls to
                    // RequestAccessAsync will show the dialog again.
                    case UserNotificationListenerAccessStatus.Unspecified:
                        await new MessageDialog("Unspecified", "Title of the message dialog").ShowAsync();
                        // Show UI that allows the user to bring up the prompt again
                        break;
                }

                listener.NotificationChanged += Listener_NotificationChanged;
                return;

                // Get the toast binding, if present
                IReadOnlyList<UserNotification> notifs = await listener.GetNotificationsAsync(NotificationKinds.Toast);
                //Debug.WriteLine(notifs.Count.ToString());
                //await new MessageDialog(notifs[1].AppInfo.DisplayInfo.DisplayName, "Title of the message dialog").ShowAsync();
                //await new MessageDialog(notifs[1].Notification., "Title of the message dialog").ShowAsync();


                // Get the toast binding, if present
                NotificationBinding toastBinding = notifs[1].Notification.Visual.GetBinding(KnownNotificationBindings.ToastGeneric);

                if (toastBinding != null)
                {
                    // And then get the text elements from the toast binding
                    IReadOnlyList<AdaptiveNotificationText> textElements = toastBinding.GetTextElements();

                    // Treat the first text element as the title text
                    string titleText = textElements.FirstOrDefault()?.Text;

                    // We'll treat all subsequent text elements as body text,
                    // joining them together via newlines.
                    string bodyText = string.Join("\n", textElements.Skip(1).Select(t => t.Text));

                    await new MessageDialog(titleText, "Title of the message dialog").ShowAsync();
                    await new MessageDialog(bodyText, "Title of the message dialog").ShowAsync();
                }

                //                await new MessageDialog("Your message here", "Title of the message dialog").ShowAsync();

                //MessageDialog dialog = new MessageDialog("Yes or no?");
                //                dialog.Commands.Add(new UICommand("Yes", null));
                //                dialog.Commands.Add(new UICommand("No", null));
                //                dialog.DefaultCommandIndex = 0;
                //                dialog.CancelCommandIndex = 1;
                //                var cmd = await dialog.ShowAsync();

                //                if (cmd.Label == "Yes")
                //                {
                //                    // do something
                //                }

            }

            else
            {
                // Older version of Windows, no Listener
            }
        }
    }
}
