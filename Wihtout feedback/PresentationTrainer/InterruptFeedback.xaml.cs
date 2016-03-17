using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationTrainer
{
    /// <summary>
    /// Interaction logic for InterruptFeedback.xaml
    /// </summary>
    /// 

    public partial class InterruptFeedback : UserControl
    {
        public enum InterruptionType  {LongPause, BadPosture, reportButton, interruption};
        public InterruptionType myInterruption;
        public string myMistake;
        public PresentationAction[] mistakes;
        public string errorType = "";

        public InterruptFeedback()
        {
            InitializeComponent();
            myMistake = "25";
            myInterruption = InterruptionType.reportButton;
       
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            IntroGrid.Visibility = Visibility.Visible;
            try
            {
                pauseSound.Stop();
                pauseSound.Play();
            }
            catch (Exception ex)
            {
                int x = 0;
                x++;
            }
             var uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_startSpeaking.png", UriKind.Relative);

            switch (mistakes[0].myMistake)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:
                    crossArms();
                    errorType = "Posture";
                    break;
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                    handsBehindBack();
                    errorType = "Posture";
                    break;
                case PresentationAction.MistakeType.LEGSCROSSED:
                    legsCrossed();
                    errorType = "Posture";
                    break;

                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                    handsUnderHips();
                    errorType = "Posture";
                    break;
                case PresentationAction.MistakeType.HUNCHBACK:
                    hunchPosture();
                    errorType = "Posture";
                    break;
                case PresentationAction.MistakeType.RIGHTLEAN:
                case PresentationAction.MistakeType.LEFTLEAN:
                    leaningPosture();
                    errorType = "Posture";
                    break;
                case PresentationAction.MistakeType.DANCING:
                    PeriodicMovements();
                    errorType = "Sanding Still";
                    break;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    handsNotMoving();
                    errorType = "Hand Movements";
                    break;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:
                    handsMovingMuch();
                    errorType = "Hand Movements";
                    break;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                    highVolume();
                    errorType = "Voice Volume";
                    break;
                case PresentationAction.MistakeType.LOW_VOLUME:
                    lowVolume();
                    errorType = "Voice Volume";
                    break;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    noModulation();
                    errorType = "Voice Volume";
                    break;
                case PresentationAction.MistakeType.LONG_PAUSE:
                    longPauses();
                    errorType = "Timing";
                    break;
                case PresentationAction.MistakeType.LONG_TALK:
                    longTalks();
                    errorType = "Timing";
                    break;
                case PresentationAction.MistakeType.HMMMM:
                    hmmmms();
                    errorType = "Phonetic Pauses";
                    break;
                case PresentationAction.MistakeType.SERIOUS:
                    smile();
                    errorType = "Serious";
                    break;
            }

            outroLabel.Content = "You might want to go back to Freestyle mode \nto train multiple aspects of your presentation" +
                "\nor to the Special Skills Mode \nwhere you can focus only on " + errorType;
            //switch(myInterruption)
            //{
            //    case InterruptionType.LongPause:
            //        introLabel.Content =  PresentationTrainer.Properties.Resources.StringPausingLongIntro;
            //        mistakeLabel.Content = myMistake;
            //        descriptionLabel.Content = PresentationTrainer.Properties.Resources.StringPausingLongMiddle;
            //        outroLabel.Content = PresentationTrainer.Properties.Resources.StringGoBackOrExercise;
            //        uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_startSpeaking.png", UriKind.Relative);
            //        iconImage.Source = new BitmapImage(uriSource);
            //        break;
            //    case InterruptionType.BadPosture:
            //        break;
            //    case InterruptionType.reportButton:
            //        if (RulesAnalyzer.isRepetitionInterruption)
            //        {
            //            introLabel.Content = "Number of repetitions of this mistake:";
            //            mistakeLabel.Content = "" + RulesAnalyzer.REPETITION_THRESHOLD;
            //        }
            //        else
            //        {
            //            introLabel.Content = "Size of this mistake: ";
            //            mistakeLabel.Content = "" + RulesAnalyzer.pointsOfBiggestOfAllMistakes;
            //        }
                    
            //        descriptionLabel.Content = "Triggered mistake: " + Mistake.getStringOfSubType(RulesAnalyzer.triggered) + "\n"
            //            + Mistake.getMistakeFeedBack(RulesAnalyzer.triggered);
            //            //+ "Biggest Mistake: "+ Mistake.getStringOfSubType(RulesAnalyzer.biggestOfAllMistakes.subType) + ". " 
            //            //+ Mistake.getMistakeFeedBack(RulesAnalyzer.biggestOfAllMistakes.subType) + "\n"
            //            //+ "Most Repeated Mistake: " + Mistake.getStringOfSubType(RulesAnalyzer.mostRepeatedMistake) + ". " 
            //            //+ Mistake.getMistakeFeedBack(RulesAnalyzer.mostRepeatedMistake);

            //        outroLabel.Content = "You can continue by clicking the left bottom button or " + "\n" +
            //            "you can go back to main menu by clicking the right bottom button";
            //        switch (RulesAnalyzer.triggered)
            //        {
            //            case Mistake.SubType.loud:
            //                uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_lower_volume.png", UriKind.Relative);
            //                break;
            //            case Mistake.SubType.soft:
            //                uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_raiseVolume.png", UriKind.Relative);
            //                break;
            //            case Mistake.SubType.longSpeakingTime:
            //                uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_pause_speaking.png", UriKind.Relative);
            //                break;
            //            case Mistake.SubType.longPause:
            //                uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_startSpeaking.png", UriKind.Relative);
            //                break;
            //            case Mistake.SubType.shortSpeakingTime:
            //                uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_startSpeaking.png", UriKind.Relative);
            //                break;
            //            case Mistake.SubType.shortPause:
            //                uriSource = new Uri(@"/PresentationTrainer;component/Images/Images/ic_fb_pause_speaking.png", UriKind.Relative);
            //                break;
            //            case Mistake.SubType.badPosture:
            //                uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_reset_posture.png", UriKind.Relative);
            //                break;
            //            case Mistake.SubType.noHandMovement:
            //                uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_move_more.png", UriKind.Relative);
            //                break;
            //            case Mistake.SubType.tooMuchHandMovement:
            //                uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_move_more.png", UriKind.Relative);
            //                break;
            //            case Mistake.SubType.moduleVolume:
            //                uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_moduleVolume.png", UriKind.Relative);
            //                break;
            //        }
            //        //uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_startSpeaking.png", UriKind.Relative);
            //        iconImage.Source = new BitmapImage(uriSource);
            //        break;
            //}

            

        }

        private void crossArms()
        {

            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = "for a very long time";
            }
            else
            {
                mistakeNumber = x + "times";
            }

            introLabel.Content = "I interrupted because \nyou crossed your arms " + mistakeNumber;
       
            descriptionLabel.Content = "If you do not know in which posture to stand,\n you can always go to the Reset Posture:\n" +
                "Standing Straight, Hands in front of your Body,\nabove your hips and slightly touching";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/postureExample.png", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            int imgtoshow = findEvidenceImage();
            evidence.EvidenceImg3.Source = mistakes[imgtoshow].firstImage;
           evidence.EvidenceLabel2.Content = "";
           evidence.EvidenceLabel1.Content = "";
            

           // myImage.Source = parent.videoHandler.kinectImage.Source;
        }

   

        private void handsBehindBack()
        {

            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = "for a very long time";
            }
            else
            {
                mistakeNumber = x + "times";
            }

            introLabel.Content = "I interrupted because \nyou had your hands behind your back \n" + mistakeNumber;
         
           descriptionLabel.Content =  "If you do not know in which posture to stand,\n you can always go to the Reset Posture:\n" +
               "Standing Straight, Hands in front of your Body,\nabove your hips and slightly touching";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/postureExample.png", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            int imgtoshow = findEvidenceImage();
            evidence.EvidenceImg3.Source = mistakes[imgtoshow].firstImage;
            evidence.EvidenceLabel2.Content = "";
            evidence.EvidenceLabel1.Content = "";

        }

        private void legsCrossed()
        {
            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = "for a very long time";
            }
            else
            {
                mistakeNumber = x + "";
            }

            introLabel.Content = "I interrupted because \nyou hand your legs crossed " + mistakeNumber
                        + " times";
            
            descriptionLabel.Content = "If you do not know in which posture to stand,\n you can always go to the Reset Posture:\n" +
               "Standing Straight, Hands in front of your Body,\nabove your hips and slightly touching";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/postureExample.png", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            int imgtoshow = findEvidenceImage();
            evidence.EvidenceImg3.Source = mistakes[imgtoshow].firstImage;
            evidence.EvidenceLabel2.Content = "";
            evidence.EvidenceLabel1.Content = "";
        }

        private void handsUnderHips()
        {
            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = "for a very long time";
            }
            else
            {
                mistakeNumber = x + "times";
            }

            introLabel.Content = "I interrupted because \nyou hand your hands under your hips \n" + mistakeNumber;
            
             descriptionLabel.Content = "If you do not know in which posture to stand,\n you can always go to the Reset Posture:\n" +
                "Standing Straight, Hands in front of your Body,\nabove your hips and slightly touching";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/postureExample.png", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            int imgtoshow = findEvidenceImage();
            evidence.EvidenceImg3.Source = mistakes[imgtoshow].firstImage;
            evidence.EvidenceLabel2.Content = "";
            evidence.EvidenceLabel1.Content = "";
        }

        private void hunchPosture()
        {

            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = "for a very long time";
            }
            else
            {
                mistakeNumber = x + "times";
            }

            introLabel.Content = "I interrupted because \nyou were not standing straight \n" + mistakeNumber;
          
           descriptionLabel.Content = "If you do not know in which posture to stand,\n you can always go to the Reset Posture:\n" +
              "Standing Straight, Hands in front of your Body,\nabove your hips and slightly touching";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/postureExample.png", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            int imgtoshow = findEvidenceImage();
            evidence.EvidenceImg3.Source = mistakes[imgtoshow].firstImage;
            evidence.EvidenceLabel2.Content = "";
            evidence.EvidenceLabel1.Content = "";
        }

        private void leaningPosture()
        {

            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = "for a very long time";
            }
            else
            {
                mistakeNumber = x + "times";
            }

            introLabel.Content = "I interrupted because \nyou were leaning to one side \n" + mistakeNumber;
            
            descriptionLabel.Content = "If you do not know in which posture to stand,\n you can always go to the Reset Posture:\n" +
               "Standing Straight, Hands in front of your Body,\nabove your hips and slightly touching";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/postureExample.png", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            int imgtoshow = findEvidenceImage();
            evidence.EvidenceImg3.Source = mistakes[imgtoshow].firstImage;
            evidence.EvidenceLabel2.Content = "";
            evidence.EvidenceLabel1.Content = "";
        }

        private void PeriodicMovements()
        {
            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = "for a very long time";
            }
            else
            {
                mistakeNumber = x + "";
            }

            introLabel.Content = "I interrupted because \nyou were moving repetitively from side to side ";
           
            descriptionLabel.Content = "If you are not walking \nyou should stay still to show confidence";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/standStillExample.png", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            int imgtoshow = findEvidenceImage();
            float duration = (float)(mistakes[imgtoshow].timeFinished - mistakes[imgtoshow].timeStarted) / 10;
            int durationInt = (int)duration;
            duration = durationInt / 100;
            //evidence.EvidenceImg1.Source = mistakes[imgtoshow].firstImage;
            //evidence.EvidenceImg2.Source = mistakes[imgtoshow].lastImage;
            //evidence.EvidenceLabel2.Content = "" + duration + " secs later";
            evidence.Visibility = Visibility.Collapsed;
        }

        private void handsNotMoving()
        {
            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = " a very long time";
            }
            else
            {
                mistakeNumber = x + "times";
            }

            introLabel.Content = "I interrupted because \nyour arms did not move enough \n" + mistakeNumber;
            
            descriptionLabel.Content = "You should move your arms more \nto emphasise your points";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/moveMoreExample.jpg", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            int imgtoshow = findEvidenceImage();
            float duration = (float)(mistakes[imgtoshow].timeFinished - mistakes[imgtoshow].timeStarted)/10;
            int durationInt = (int)duration;
            duration = durationInt/100;
            evidence.EvidenceImg1.Source = mistakes[imgtoshow].firstImage;
            evidence.EvidenceImg2.Source = mistakes[imgtoshow].lastImage;
            evidence.EvidenceLabel2.Content = "" + duration + " secs later";
        }

        private void handsMovingMuch()
        {
            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = "for a very long time";
            }
            else
            {
                mistakeNumber = x + "times";
            }

            introLabel.Content = "I interrupted because \nyour arms too much \n" + mistakeNumber;
           
            descriptionLabel.Content = "Try to be more relax and move\n your arms only to emphasise \nimportant points";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/moveMoreExample.jpg", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            int imgtoshow = findEvidenceImage();
            float duration = (float)(mistakes[imgtoshow].timeFinished - mistakes[imgtoshow].timeStarted) / 10;
            int durationInt = (int)duration;
            duration = durationInt / 100;
            evidence.EvidenceImg1.Source = mistakes[imgtoshow].firstImage;
            evidence.EvidenceImg2.Source = mistakes[imgtoshow].lastImage;
            evidence.EvidenceLabel2.Content = "" + duration + " secs later";
        }

        private void highVolume()
        {
            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = "for a very long time";
            }
            else
            {
                mistakeNumber = x + "times";
            }

            introLabel.Content = "I interrupted because \nyou were speaking too loud \n" + mistakeNumber
                        + " times";
          
            descriptionLabel.Content = "Relax and speak a bit softer";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/lowerVolumeExample.jpg", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            evidence.Visibility = Visibility.Collapsed;
        }

        private void lowVolume()
        {

            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = "for a very long time";
            }
            else
            {
                mistakeNumber = x + "times";
            }
            introLabel.Content = "I interrupted because \nyou were speaking too soft \n" + mistakeNumber;
           
            descriptionLabel.Content = "Relax and speak a bit Louder";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/speakLouderExample.jpg", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            evidence.Visibility = Visibility.Collapsed;
        }

        private void noModulation()
        {
            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = " a very long time";
            }
            else
            {
                mistakeNumber = x + "times";
            }

            introLabel.Content = "I interrupted because \nyou were speaking very monotone \n" + mistakeNumber;
           
            descriptionLabel.Content = "Try to modulate your voice a bit more";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ModulateVoiceExample.jpg", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            evidence.Visibility = Visibility.Collapsed;
        }

        private void longPauses()
        {
            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x<4)
            {
                mistakeNumber = " for a very long time";
            }
            else
            {
                mistakeNumber = x + "times";
            }
            introLabel.Content = "I interrupted because \nyou pause for too long time \n" + mistakeNumber;
            
            descriptionLabel.Content = "Try to shorten your pauses";
 
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/dontpauseExample.jpg", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            evidence.Visibility = Visibility.Collapsed;
        }

        private void longTalks()
        {

            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = "for a very long time";
            }
            else
            {
                mistakeNumber = x + "times";
            }

            introLabel.Content = "I interrupted because \nyou talked for too long without pauses \n" + mistakeNumber;
            
            descriptionLabel.Content = "Try to take a breath after each sentence\nlet the audience catch up with you";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/takeabreathExample.jpg", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            evidence.Visibility = Visibility.Collapsed;
        }

        private void hmmmms()
        {

            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = " for a very long time";
            }
            else
            {
                mistakeNumber = x + " times";
            }

            introLabel.Content = "I interrupted because \nyou were using phonetic pauses \n" + mistakeNumber;
            
            descriptionLabel.Content = "Before each sentence,\nYou were doing a lot of ahhhh or hmmms\nbetter take a deep breath\nand start speaking after it";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/takeabreathExample.jpg", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            evidence.Visibility = Visibility.Collapsed;
        }
        private void smile()
        {
            int x = getMistakeNumbers();
            string mistakeNumber = "";
            if (x < 4)
            {
                mistakeNumber = " for a very long time";
            }
            else
            {
                mistakeNumber = x + " times";
            }

            introLabel.Content = "I interrupted because \nyou had a blank face \n" + mistakeNumber;

            descriptionLabel.Content = "Remember to smile and Show emotions,\nEnthusiasm is contagious\nBe enthusiastic about what you speech.\n";

            var uriSource = new Uri(@"/PresentationTrainer;component/Images/smiley.jpg", UriKind.Relative);
            iconImage.Source = new BitmapImage(uriSource);
            int imgtoshow = findEvidenceImage();
            evidence.EvidenceImg3.Source = mistakes[imgtoshow].firstImage;
            evidence.EvidenceLabel2.Content = "";
            evidence.EvidenceLabel1.Content = "";

            GoToExercises.IsEnabled = false;
        }

        private int findEvidenceImage()
        {
            int x = 0;
            for (int i = 0; i < 4; i++)
            {
                if (mistakes[i] != null)
                {
                    x = i;

                }
            }
            return x;
        }

        int getMistakeNumbers()
        {
            int x=0;
            for(int i=0;i<4;i++)
            {
                if(mistakes[i]!=null)
                {
                    x++;
                }
            }
            
            return x;
        }
        private double calcAverageTime()
        {
            
            double average = 0;
            foreach (PresentationAction pa in mistakes)
            {
                if(pa!=null)
                {
                    average = average + pa.timeFinished - pa.timeStarted;
                }
                
                
            }
            average = average / getMistakeNumbers();
            return average/1000;
        }

        private void GoToMainMenuButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GoToExercises_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IntroButtonNext_Click(object sender, RoutedEventArgs e)
        {
            IntroGrid.Visibility = Visibility.Collapsed;
            InstructGrid.Visibility = Visibility.Visible;
        }

        private void InstructButtonNext_Click(object sender, RoutedEventArgs e)
        {
            InstructGrid.Visibility = Visibility.Collapsed;
        }
    }
}
