using F_X.PersonalAssistant;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Syndication;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace F_X.Extras
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ExtrasPage : Page
    {
        public ExtrasPage()
        {
            this.InitializeComponent();
            ExtrasText.TextAlignment = TextAlignment.Center;
            
        }



        private async void load(ItemsControl list, Uri uri)
        {
            SyndicationClient client = new SyndicationClient();
            SyndicationFeed feed = await client.RetrieveFeedAsync(uri);
            if (feed != null)
            {
                foreach (SyndicationItem item in feed.Items)
                {
                    list.Items.Add(item);
                }
            }
        }

        public void Go(ref ItemsControl list, string value)
        {

            try
            {
                load(list, new Uri(value));
            }
            catch
            {

            }
            list.Focus(FocusState.Keyboard);

        }
    

    private void feedClick_Click(object sender, RoutedEventArgs e)
    {
        Go(ref Display, Value.Text);

    }




    public Frame AppFrame { get { return Content; } }

        private void Home_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        private void Assistant_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AssistantPage));
        }
        private void Controls_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Controls));

        }

        private void Extras_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ExtrasPage));
        }
        private void Settings_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Settings));
        }
        private void Logout_Checked(object sender, RoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(LoginPage));
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }

        private void onClick(object sender, RoutedEventArgs e)
        {
            MediaPlayer.Play();
        }
    }
}
