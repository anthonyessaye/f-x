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
using F_X.Arduino_Related_Classes;
using System.Xml.Linq;
using Windows.Storage;
using F_X.InformationGathering;
using System.IO;
using F_X.InformationQueries;

namespace F_X.PersonalAssistant
{
    class Assistant
    {
        Windows.Media.SpeechRecognition.SpeechRecognizer speechRecognizer = new Windows.Media.SpeechRecognition.SpeechRecognizer();
        public string TextSaid { get; set; }
        private string WelcomingTexts = "Hello,\nYour wish is my command";

        private int NumberOfSentencesIKnow = 6;

        private SpeechSynthesizer synthesizer;
        private ResourceContext speechContext;
        private ResourceMap speechResourceMap;
        private IAsyncOperation<SpeechRecognitionResult> recognitionOperation;
        XDocument NamesXML;

       private SettingsQueries theSettings = new SettingsQueries();
        private WeatherQuery theWeatherQuery = new WeatherQuery();

        FTPDownloads pleaseDownload = new FTPDownloads();

        Controls theControls = new Controls();
        PinControlReally theArduino = new PinControlReally();
        

        public Assistant()
        {
            SetXML();   
        }
        
        public async void SetXML()
        {
            pleaseDownload.getLatestOutputs();

            NamesXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("OutputNames.xml"));
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

        public String HelloText()
        {
           // DateTime DateSeed = new DateTime();
            Random aRandomNumber = new Random(DateTime.Now.Ticks.GetHashCode());

            if ((aRandomNumber.Next()) % NumberOfSentencesIKnow == 0)
                WelcomingTexts = "Hello, How can i help you?";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 1)
                WelcomingTexts = "Hope your day is fine.\nWhat can i do for you?";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 2)
                WelcomingTexts = "Yes, I'm awake, What is it you need?";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 3)
                WelcomingTexts = "JARVIS? No, it's just me. \nWhat can i help you with?";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 4)
                WelcomingTexts = "Tell me my son, you can trust me";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 5)
                WelcomingTexts = "No shame commanding your home\nCome on do it.";
            

            return WelcomingTexts;
        }

        public String IAmStuuuupid()
        {
            Random aRandomNumber = new Random(DateTime.Now.Ticks.GetHashCode());

            if ((aRandomNumber.Next()) % NumberOfSentencesIKnow == 0)
                WelcomingTexts = "Nope I don't get that\nTry typing help";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 1)
                WelcomingTexts = "My knowledge is limited.\nSay help and I will";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 2)
                WelcomingTexts = "It is help you need.\nAsk me to help you.";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 3)
                WelcomingTexts = "You think I'm that smart?\nNope, try help";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 4)
                WelcomingTexts = "Help. Help. Help. Maybe I will";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 5)
                WelcomingTexts = "Try saying that in binary.";


            return WelcomingTexts;
        }
        public String WhatIsThat()
        {

            String WhatThe = "Focus on the name. Genius.";
            Random aRandomNumber = new Random(DateTime.Now.Ticks.GetHashCode());

            if ((aRandomNumber.Next()) % NumberOfSentencesIKnow == 0)
                WhatThe = "What the hell is that?";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 1)
                WhatThe = "I know you don't have\nsuch a device";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 2)
                WhatThe = "I may not be smart, but\nI know that nothing is called that";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 3)
                WhatThe = "Auto correct?";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 4)
                WhatThe = "I bet you that name is wrong";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 5)
                WhatThe = "Oh well, If only i knew what that is!";


            return WhatThe;
        }
        public String TurnOnOffText(String OnOff)
        {
            String OnOffText = "Okay. " + OnOff;
            Random aRandomNumber = new Random(DateTime.Now.Ticks.GetHashCode());

            if ((aRandomNumber.Next()) % NumberOfSentencesIKnow == 0)
                OnOffText = "Yes sir I will turn that " + OnOff;
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 1)
                OnOffText = "I bet it's " + OnOff + " now!";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 2)
                OnOffText = "Count on me to turn that " + OnOff;
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 3)
                OnOffText = "You have to trust me that\n this is " + OnOff;
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 4)
                OnOffText = "Some people get paid for that\nTurning this " + OnOff + " for FREE!";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 5)
                OnOffText = OnOff.ToUpper() + " it is";


            return OnOffText;
        }
        public String AlreadyOnOff(String AlreadyOnOff)
        {
            String OnOffText = "This is already on";
            Random aRandomNumber = new Random(DateTime.Now.Ticks.GetHashCode());

            if ((aRandomNumber.Next()) % NumberOfSentencesIKnow == 0)
                OnOffText = "But,but.. This is already " + AlreadyOnOff;
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 1)
                OnOffText = "I think it's " + AlreadyOnOff;
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 2)
                OnOffText = "You don't need my help. It was " + AlreadyOnOff;
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 3)
                OnOffText = "Guess you didn't realise\n It is already " + AlreadyOnOff;
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 4)
                OnOffText = "No need for that. Already " + AlreadyOnOff;
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 5)
                OnOffText = AlreadyOnOff.ToUpper() + " " + AlreadyOnOff.ToUpper() + " " + AlreadyOnOff.ToUpper()
                            + ". That's how it is";


            return OnOffText;
        }

        private int TryToUnderStand(String splitText)
        {



            if (splitText.Contains("Hey") || splitText.Contains("Hello") ||
                    splitText.Contains("hello") || splitText.Contains("hey") ||
                        splitText.Contains("Hi") || splitText.Contains("hi"))
            {
                return 0;
            }

            else if (splitText.Contains("turn") || splitText.Contains("switch")
                        || splitText.Contains("Turn") || splitText.Contains("Switch")
                         || splitText.Contains("Power") || splitText.Contains("power"))
            {
                return 1;
            }

            else if (splitText.Contains("Help") || splitText.Contains("help"))
            {
                return 2;
            }


            else if (splitText.Contains("Weather") || splitText.Contains("weather") || splitText.Contains("temperature")
                        || splitText.Contains("Temperature"))
            {
                return 3;
            }

            else if (splitText.Contains(theSettings.getAssistantQuery()))
            {
                return 4;
            }


            else
                return 5;



        }

        //iShouldTurnSomething() is a function i created to receive a text that contains
        // some predefined words such as "turn" "power" etc... it then goes on to check
        // what status it should move on.

        //iCanControlOutputs is of type OutputQueries(), a class that can read XML values and manipulate them
        private String iShouldTurnSomething(String somethingIShouldDo)
        {
           
            int ToggleIndex;
            ToggleIndex = theArduino.FindPinName(somethingIShouldDo,ref NamesXML);

            if (somethingIShouldDo.Contains("On") || somethingIShouldDo.Contains("on"))
            {
                if (somethingIShouldDo.Contains("all") || somethingIShouldDo.Contains("All") ||
                       somethingIShouldDo.Contains("everything") || somethingIShouldDo.Contains("Everything"))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        theArduino.SetPin(i, "On", ref NamesXML);
                    }
                    return "Everthing is on now";
                }
                else if (theArduino.FindPinName(somethingIShouldDo, ref NamesXML) == -1)
                    return WhatIsThat();
                else if(theArduino.ControlIsON(ToggleIndex, ref NamesXML) == true)
                    return AlreadyOnOff("on");
                else if (theArduino.ControlIsON(ToggleIndex, ref NamesXML) == false)
                {
                    theArduino.SetPin(ToggleIndex, "On", ref NamesXML);
                    return TurnOnOffText("on");

                }
                 
              
            }

            if (somethingIShouldDo.Contains("Off") || somethingIShouldDo.Contains("off"))
            {
                if (somethingIShouldDo.Contains("all") || somethingIShouldDo.Contains("All") ||
                      somethingIShouldDo.Contains("everything") || somethingIShouldDo.Contains("Everything"))
                {
                    for (int i = 0; i < 4; i++)
                    {
                        theArduino.SetPin(i, "Off",ref NamesXML);
                    }
                    return "Everthing is off now";
                }
                else if(theArduino.FindPinName(somethingIShouldDo, ref NamesXML) == -1)
                    return WhatIsThat();

                else if(theArduino.ControlIsON(ToggleIndex, ref NamesXML) == false)
                    return AlreadyOnOff("off");
                else if (theArduino.ControlIsON(ToggleIndex, ref NamesXML) ==true)
                {
                    theArduino.SetPin(ToggleIndex,"Off",ref NamesXML);
                    return TurnOnOffText("off");
                }
                

                
            }




            return "I dont get what i should change";

        }

        private string SayMyName()
        {
            String thatIsMyName = "Yes?";
            Random aRandomNumber = new Random(DateTime.Now.Ticks.GetHashCode());

            if ((aRandomNumber.Next()) % NumberOfSentencesIKnow == 0)
                thatIsMyName = "Say my name say my name. Sing along!";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 1)
                thatIsMyName = "That is me. Yes?";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 2)
                thatIsMyName = "What kind of name is that right?";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 3)
                thatIsMyName = "Please change that name. I don't like it!";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 4)
                thatIsMyName = "At your service";
            if (aRandomNumber.Next() % NumberOfSentencesIKnow == 5)
                thatIsMyName = "Just so you know. I do things for free!";



            return thatIsMyName;
        }


        public string AssistantBrainWorking(string incomingText)
        {
            string reply="Hunh?";
            switch(TryToUnderStand(incomingText))
            {
                case 0:
                    {
                        reply = HelloText();
                        break;
                    }

                case 1:
                    {
                        reply = iShouldTurnSomething(incomingText);
                        break;
                    }
                case 2:
                    {
                        reply = "So far i can only welcome you\n and turn things on and off";
                        break;
                    }


                case 3:
                    {
                        string UnitTemperatureString = "";
                        if (theSettings.isUnitTemperatureC())
                            UnitTemperatureString = "°C";
                        else if (!theSettings.isUnitTemperatureC())
                            UnitTemperatureString = "°F";

                        reply = "Weather Forecast for " + theSettings.getWeatherQuery() + ":\nMin: " + theWeatherQuery.getMinTemp() +
                                                    UnitTemperatureString + "\tMax: " + theWeatherQuery.getMaxTemp() + UnitTemperatureString + "\nHumidity: " +
                                                      theWeatherQuery.getHumidity() + "%";
                        break;
                    }
                    
                case 4:
                    {
                        reply = SayMyName();
                        break;
                    }


                case 5:
                    {
                        reply = IAmStuuuupid();
                        break;
                    }
            }
            
            return reply;
        }

    }

       
}
