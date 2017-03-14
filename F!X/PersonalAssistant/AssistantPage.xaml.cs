using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace F_X.PersonalAssistant
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AssistantPage : Page
    {
        Assistant theVoice = new Assistant();


        public AssistantPage()
        {
            this.InitializeComponent();
           

            theVoice.SetUpTTS(media, SpeechText.Text);
            theVoice.CallAssitant(SpeechText);
            

            
        }

        void media_MediaEnded(object sender, RoutedEventArgs e) { } // A function to do something after media has ended its call
                                                                    // currently does nothing, but func needs to exist.



        // The three functions under this are used for the menu on the left. Assigning where each button takes you.

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
        private void Settings_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Settings));
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }
    }
}
