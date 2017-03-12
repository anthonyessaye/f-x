using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Media.SpeechRecognition;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace F_X.PersonalAssistant
{
    class Assistant
    {
        Windows.Media.SpeechRecognition.SpeechRecognizer speechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();
        public string TextSaid { get; set; }

        private SpeechSynthesizer synthesizer;
        private ResourceContext speechContext;
        private ResourceMap speechResourceMap;
        private IAsyncOperation<SpeechRecognitionResult> recognitionOperation;

        public Assistant() {
            
        }
        

        // This function is called to configure text-to-speech recognition
        // It also need a string to start the conversation
        public async void SetUpTTS(MediaElement aMediaElement, string StartingText)
        {

            synthesizer = new SpeechSynthesizer();

            speechContext = ResourceContext.GetForCurrentView();
            speechContext.Languages = new string[] { SpeechSynthesizer.DefaultVoice.Language };
            speechResourceMap = ResourceManager.Current.MainResourceMap.GetSubtree("LocalizationTTSResources");

            try

            {

                // Create a stream from the text. This will be played using a media element.

                SpeechSynthesisStream synthesisStream = await synthesizer.SynthesizeTextToStreamAsync(StartingText);

                // Set the source and start playing the synthesized audio stream.

                aMediaElement.AutoPlay = true;
                aMediaElement.SetSource(synthesisStream, synthesisStream.ContentType);
                aMediaElement.Play();

            }

            catch (System.IO.FileNotFoundException)

            {

                // If media player components are unavailable, (eg, using a N SKU of windows), we won't

                // be able to start media playback. Handle this gracefully


                var messageDialog = new Windows.UI.Popups.MessageDialog("Media player components unavailable");

                await messageDialog.ShowAsync();

            }

            catch (Exception)

            {

                // If the text is unable to be synthesized, throw an error message to the user.

                var messageDialog = new Windows.UI.Popups.MessageDialog("Unable to synthesize text");

                await messageDialog.ShowAsync();

            }
        }


        public async void CallAssitant(TextBlock speechText)
        {
            // Compile the dictation grammar by default.
            await speechRecognizer.CompileConstraintsAsync();

            //recognitionOperation = speechRecognizer.RecognizeAsync();
            //SpeechRecognitionResult speechRecognitionResult = await recognitionOperation;

            //// Start recognition.

            //if (speechRecognitionResult.Status == SpeechRecognitionResultStatus.Success)
            //{
            //    TextSaid = "\n" + speechRecognitionResult.Text;
            //}


            Windows.Media.SpeechRecognition.SpeechRecognitionResult speechRecognitionResult = await speechRecognizer.RecognizeWithUIAsync();
            TextSaid = "\n" + speechRecognitionResult.Text;
            speechText.Text = speechText.Text + TextSaid;
            //This code is commented out because i am trying to live without a dialogue box

        }

    }

       
}
