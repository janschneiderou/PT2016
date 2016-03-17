using System;
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

    public partial class ReportFeedback : UserControl
    {
        public enum InterruptionType { LongPause, BadPosture, reportButton, interruption };
        public InterruptionType myInterruption;
        public string myMistake;
        public ReportFeedback()
        {
            InitializeComponent();
            myMistake = "25";
            myInterruption = InterruptionType.reportButton;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
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
            switch (myInterruption)
            {
                case InterruptionType.LongPause:
                    introLabel.Content = PresentationTrainer.Properties.Resources.StringPausingLongIntro;
                    mistakeLabel.Content = myMistake;
                    descriptionLabel.Content = PresentationTrainer.Properties.Resources.StringPausingLongMiddle;
                    outroLabel.Content = PresentationTrainer.Properties.Resources.StringGoBackOrExercise;
                    uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_startSpeaking.png", UriKind.Relative);                    
                    break;
                case InterruptionType.BadPosture:
                    break;
                case InterruptionType.reportButton:
                    introLabel.Content =  "Number of mistakes:";
                    mistakeLabel.Content = ""+RulesAnalyzer.nrOfMistakes;
                    descriptionLabel.Content = "Volume Mistakes: " + RulesAnalyzer.nrOfVolumeMistakes + "\n"
                        + "Cadence Mistakes: " + RulesAnalyzer.nrOfCadenceMistakes + "\n"
                        + "Posture Mistakes: " + RulesAnalyzer.nrOfPostureMistakes + "\n"
                        + "Hand Movement Mistakes: " + RulesAnalyzer.nrOfHandMovementMistakes + "\n"
                        + "Short Mistakes: " + RulesAnalyzer.nrOfShortMistakes + "\n"
                        + "Average Points of your Mistakes: " + RulesAnalyzer.averageMistakePoints + "\n"
                        + "Standard deviation of your mistakes: " + RulesAnalyzer.standardDeviationOfMistakePoints + "\n"
                        + "Your biggest Mistake: " + Mistake.getStringOfSubType(RulesAnalyzer.biggestOfAllMistakes.subType) + "\n"
                        + "Your most repeated Mistake: "+ Mistake.getStringOfSubType(RulesAnalyzer.mostRepeatedMistake);
                    outroLabel.Content = "You can continue by clicking the left bottom button or "+"\n"+
                        "you can go back to main menu by clicking the right bottom button";
                    uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_startSpeaking.png", UriKind.Relative);                                        
                    break;
            }

        }

        private void GoToMainMenuButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

