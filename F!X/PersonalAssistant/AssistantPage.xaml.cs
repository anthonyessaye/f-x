using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

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
        private void Logout_Checked(object sender, RoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(LoginPage));
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }

        private void ButtonMic_Click(object sender, RoutedEventArgs e)
        {
              theVoice.SetUpTTS(media, "How can I help you?");
          //    theVoice.CallAssitant(SpeechText);
        }

        private void CreateUserConvoBox(string text)
        {
            Border ConvoBorder = new Border();
            StackPanelConvo.Children.Add(ConvoBorder);
            ConvoBorder.BorderBrush = new SolidColorBrush(Colors.Black);
            ConvoBorder.Background = new SolidColorBrush(Colors.LightBlue);
            ConvoBorder.BorderThickness = new Thickness(1, 1, 2, 1);
            ConvoBorder.Margin = new Thickness(this.ActualWidth*0.25, 5, 20, 0);
            ConvoBorder.VerticalAlignment = VerticalAlignment.Top;
            ConvoBorder.HorizontalAlignment = HorizontalAlignment.Right;
            ConvoBorder.CornerRadius = new CornerRadius(8);

            TextBlock ConvoBlock = new TextBlock();
            ConvoBorder.Child = ConvoBlock;
            ConvoBlock.Margin = new Thickness(5, 5, 5, 5);
            ConvoBlock.TextWrapping = TextWrapping.Wrap;
            ConvoBlock.Text = text;

            TextBoxConvoInput.Text = "";


            
            
        }

        private void CreateAssistantConvoBox(string text)
        {
            Border ConvoBorder = new Border();
            ConvoBorder.BorderBrush = new SolidColorBrush(Colors.Black);
            ConvoBorder.Background = new SolidColorBrush(Colors.LightGray);
            ConvoBorder.BorderThickness = new Thickness(1, 1, 2, 1);
            ConvoBorder.Margin = new Thickness(20, 5, this.ActualWidth * 0.25, 0);
            ConvoBorder.VerticalAlignment = VerticalAlignment.Top;
            ConvoBorder.HorizontalAlignment = HorizontalAlignment.Left;
            ConvoBorder.CornerRadius = new CornerRadius(8);

            TextBlock ConvoBlock = new TextBlock();
            ConvoBlock.Margin = new Thickness(5, 5, 5, 5);
            ConvoBlock.TextWrapping = TextWrapping.Wrap;
            ConvoBlock.Text = text;


            StackPanelConvo.Children.Add(ConvoBorder);
            ConvoBorder.Child = ConvoBlock;
        }

        private void TextBoxConvoInput_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
           
        }

        private void TextBoxConvoInput_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                CreateUserConvoBox(TextBoxConvoInput.Text);
            }
        }
    }
}
