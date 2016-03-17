using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for IndividualSkills.xaml
    /// </summary>
    public partial class IndividualSkills : UserControl
    {
        public PresentationAction myPresentationAction;

        public enum currentState { givingInstruction, listening };
        public currentState myCurrentState;

        PresentationAction previousAction;
        ArrayList mistakes;

        public bool isSpeaking = true;

        public int mistakeNumber;
        public double Duration = 60000;
        public double StartedTime;
        
        public MainWindow parent;
        public IndividualTracker individualTracker;
        public bool ready = false;
        public string feedbackText;

        public bool currentMistake = false;
        public bool previousMistake = false;

        public PresentationAction.MistakeType currentInstruction;

        public delegate void FinishedEvent(object sender, int x);
        public event FinishedEvent myFinishedEvent;

        Ghost ghost;
        GhostMoving ghostM;
        FreestyleTextFeedback ghostTF;
        FreestyleOldText ghostOT;

        public IndividualSkillsIntroduction introduction;
        public IndividualInstructions individualInstructions;
        public IndividualFinish finish;

        double actionTime;

        public IndividualSkills( PresentationAction pa)
        {
            myPresentationAction = pa;
            InitializeComponent();
            currentInstruction = myPresentationAction.myMistake;
            introduction = new IndividualSkillsIntroduction();
            individualInstructions = new IndividualInstructions();
            finish = new IndividualFinish();
            Loaded += IndividualSkills_Loaded;
        }

        void IndividualSkills_Loaded(object sender, RoutedEventArgs e)
        {

            loadGhosts();
            selectTypeOfIstruction();
            myCanvas.Children.Add(introduction);
            introduction.Loaded += introduction_Loaded;
           

    
           
        }

        void finish_Loaded(object sender, RoutedEventArgs e)
        {
            finish.GoToExercises.Click += GoToExercises_Click;
            finish.Visibility = Visibility.Collapsed;
        }

        void introduction_Loaded(object sender, RoutedEventArgs e)
        {
            introduction.startButton.Click += startButton_Click;
        }

        void startButton_Click(object sender, RoutedEventArgs e)
        {
            StartedTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            introduction.Visibility = Visibility.Collapsed;
            myCanvas.Children.Add(individualInstructions);
            myCanvas.Children.Add(finish);
            individualInstructions.Loaded += individualInstructions_Loaded;
            finish.Loaded += finish_Loaded;
            MainWindow.stringIndividualSkills = MainWindow.stringIndividualSkills + System.Environment.NewLine +
               "<skill>" + currentInstruction.ToString() + System.Environment.NewLine + "<time started>" +
               StartedTime + "</time started>";
        }

        void individualInstructions_Loaded(object sender, RoutedEventArgs e)
        {
            StartedTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            ready = true;
        }

        void GoToExercises_Click(object sender, RoutedEventArgs e)
        {
           StartedTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
           finish.Visibility = Visibility.Collapsed;
        
            ready = true;
 
        }

        #region loadingStuff

        public void loadGhosts()
        {
            ghost = new Ghost();
            ghostM = new GhostMoving();
            ghostTF = new FreestyleTextFeedback();
            ghostOT = new FreestyleOldText();
            myCanvas.Children.Add(ghost);
            myCanvas.Children.Add(ghostM);
            myCanvas.Children.Add(ghostTF);
            myCanvas.Children.Add(ghostOT);

            Canvas.SetLeft(ghostTF, 830);
            Canvas.SetLeft(ghostOT, 830);
            Canvas.SetLeft(ghost, 730);
            Canvas.SetTop(ghostTF, 354);
            Canvas.SetTop(ghostOT, 284);
            Canvas.SetTop(ghost, 80);

            ghost.Height = 650;
            ghost.ghostImg.Height = 450;
            ghost.CurrentFeedback.Margin = new Thickness(80, 403, 30, 191);

            setGhostInvisible();
            setGhostMovingInvisible();
            setFeedbackTextInvisible();
            setOldTextInvisible();

        }

        private void setOldTextInvisible()
        {
            ghostOT.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void setFeedbackTextInvisible()
        {
            ghostTF.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void setGhostMovingInvisible()
        {
            ghostM.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void setGhostInvisible()
        {
            ghost.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void selectTypeOfIstruction()
        {
            switch(currentInstruction)
            {
                case PresentationAction.MistakeType.ARMSCROSSED:
                case PresentationAction.MistakeType.LEFTHANDBEHINDBACK:
                case PresentationAction.MistakeType.LEFTHANDUNDERHIP:
                //   case PresentationAction.MistakeType.LEGSCROSSED:
                case PresentationAction.MistakeType.RIGHTHANDBEHINDBACK:
                case PresentationAction.MistakeType.RIGHTHANDUNDERHIP:
                case PresentationAction.MistakeType.HUNCHBACK:
                case PresentationAction.MistakeType.RIGHTLEAN:
                case PresentationAction.MistakeType.LEFTLEAN:
                    badPostureFeedback();
                    break;
                case PresentationAction.MistakeType.DANCING:
                    dancingFeedback();
                    break;
                case PresentationAction.MistakeType.HANDS_NOT_MOVING:
                    handMovementFeedback();
                    break;
                case PresentationAction.MistakeType.HANDS_MOVING_MUCH:
                    handMovementFeedback();
                    break;
                case PresentationAction.MistakeType.HIGH_VOLUME:
                    highVolumeFeedback();
                    break;
                case PresentationAction.MistakeType.LOW_VOLUME:
                    lowVolumeFeedback();
                    break;
                case PresentationAction.MistakeType.LOW_MODULATION:
                    lowModulationFeedback();
                    break;
                case PresentationAction.MistakeType.LONG_PAUSE:
                    longPauseFeedback();
                    break;
                case PresentationAction.MistakeType.LONG_TALK:
                    longTalkFeedback();
                    break;
                case PresentationAction.MistakeType.HMMMM:
                    hmmmFeedback();
                    break;
            }
        }

        private void hmmmFeedback()
        {
            introduction.skillLabel.Content = "Phonetic Pauses";
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_start_speaking.png", UriKind.Relative);
            ghost.ghostImg.Source = new BitmapImage(uriSource);
            ghostM.ghostImg.Source = new BitmapImage(uriSource);
            ghostOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            feedbackText = " Stop Hmmmms";
            ghostOT.CurrentFeedback.Content = " Stop Hmmmms";
            ghostTF.CurrentFeedback.Content = " Stop Hmmmms ";
            ghost.CurrentFeedback.Content = " Stop Hmmmms";
            individualInstructions.instructionLabel3.Content = "3.- No Noise, Just speak.";
            individualTracker = new IndividualHmmm(parent);
        }

        private void longTalkFeedback()
        {
            introduction.skillLabel.Content = "Speaking Cadence";
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_pause.png", UriKind.Relative);
            ghost.ghostImg.Source = new BitmapImage(uriSource);
            ghostM.ghostImg.Source = new BitmapImage(uriSource);
            ghostOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            
            feedbackText = " Stop Speaking";
            ghostOT.CurrentFeedback.Content = " Stop Speaking";
            ghostTF.CurrentFeedback.Content = " Stop Speaking ";
            ghost.CurrentFeedback.Content = " Stop Speaking ";
            individualInstructions.instructionLabel3.Content = "3.- Remember to Pause after a while.";
            individualTracker = new IndividualSpeakingCadence(parent);

            CadenceCounter.Visibility = Visibility.Visible;

        }

        private void longPauseFeedback()
        {
            introduction.skillLabel.Content = "Speaking Cadence";
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ghost_moduleVolume.png", UriKind.Relative);
            ghost.ghostImg.Source = new BitmapImage(uriSource);
            ghostM.ghostImg.Source = new BitmapImage(uriSource);
            ghostOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            feedbackText = " Start Speaking";
            ghostOT.CurrentFeedback.Content = " Start Speaking";
            ghostTF.CurrentFeedback.Content = " Start Speaking ";
            ghost.CurrentFeedback.Content = " Start Speaking ";
            individualInstructions.instructionLabel3.Content = "3.- Remember to Pause after a while.";
            individualTracker = new IndividualSpeakingCadence(parent);

            CadenceCounter.Visibility = Visibility.Visible;
        }

        private void lowModulationFeedback()
        {
            introduction.skillLabel.Content = "Voice Modulation";
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_module_volume.png", UriKind.Relative);
            ghost.ghostImg.Source = new BitmapImage(uriSource);
            ghostM.ghostImg.Source = new BitmapImage(uriSource);
            ghostOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            feedbackText = " Module Volume";
            ghostTF.CurrentFeedback.Content = " Module Volume ";
            individualInstructions.instructionLabel3.Content = "3.- Imagine how you will module your voice";
            ghostOT.CurrentFeedback.Content = " Module Volume";
            ghost.CurrentFeedback.Content = " Module Volume ";

            individualTracker = new IndividualVoiceVolume(parent);
        }

        private void lowVolumeFeedback()
        {
            introduction.skillLabel.Content = "Voice Volume";
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_module_volume.png", UriKind.Relative);
            ghost.ghostImg.Source = new BitmapImage(uriSource);
            ghostM.ghostImg.Source = new BitmapImage(uriSource);
            ghostOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            feedbackText = " Raise Volume";
            ghostTF.CurrentFeedback.Content = " Raise Volume ";
            individualInstructions.instructionLabel3.Content = "3.- Imagine how you will module your voice";
            ghostOT.CurrentFeedback.Content = " Raise Volume";
            ghost.CurrentFeedback.Content = " Raise Volume ";

            individualTracker = new IndividualVoiceVolume(parent);
        }

        private void highVolumeFeedback()
        {
            introduction.skillLabel.Content = "Voice Volume";
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_module_volume.png", UriKind.Relative);
            ghost.ghostImg.Source = new BitmapImage(uriSource);
            ghostM.ghostImg.Source = new BitmapImage(uriSource);
            ghostOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            ghostTF.CurrentFeedback.Content = " Lower Volume ";
            feedbackText = " Lower Volume";
            individualInstructions.instructionLabel3.Content = "3.- Imagine how you will module your voice";
            ghostOT.CurrentFeedback.Content = " Lower Volume";
            ghost.CurrentFeedback.Content = " Lower Volume ";

            individualTracker = new IndividualVoiceVolume(parent);
        }

        private void handMovementFeedback()
        {
            introduction.skillLabel.Content = "Hand Gestures";
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_move_more.png", UriKind.Relative);
            ghost.ghostImg.Source = new BitmapImage(uriSource);
            ghostM.ghostImg.Source = new BitmapImage(uriSource);
            ghostOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            feedbackText = "Move Hands";
            ghostTF.CurrentFeedback.Content = " Move Hands ";
            individualInstructions.instructionLabel3.Content = "3.- Imagine you using effective hand gestures";
            ghostOT.CurrentFeedback.Content = " Move Hands";
            ghost.CurrentFeedback.Content = " Move Hands ";

            individualTracker = new IndividualHandMovements(parent);
        }

        private void dancingFeedback()
        {
            introduction.skillLabel.Content = "Standing Still Skills";
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_move_more.png", UriKind.Relative);
            ghost.ghostImg.Source = new BitmapImage(uriSource);
            ghostM.ghostImg.Source = new BitmapImage(uriSource);
            ghostOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            feedbackText = "Stay Still";
            ghostTF.CurrentFeedback.Content = " Stay Still ";
            individualInstructions.instructionLabel3.Content = "3.- Imagine Staying grounded";
            ghostOT.CurrentFeedback.Content = " Stay Still";
            ghost.CurrentFeedback.Content = " Stay Still ";

            individualTracker = new IndividualDancing(parent);
        }

        private void badPostureFeedback()
        {
            introduction.skillLabel.Content = "Posture";
            individualTracker = new IndividualPosture(parent);
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_reset_posture.png", UriKind.Relative);
            ghost.ghostImg.Source = new BitmapImage(uriSource);
            ghostM.ghostImg.Source = new BitmapImage(uriSource);
            ghostOT.FeedbackIMG.Source = new BitmapImage(uriSource);
            feedbackText = "Reset Posture";
            ghostTF.CurrentFeedback.Content = " Reset Posture ";
            individualInstructions.instructionLabel3.Content = "3.- Imagine yourself in a good posture";
            ghostOT.CurrentFeedback.Content = " Reset Posture";
            ghost.CurrentFeedback.Content = "Reset Posture ";

            individualTracker = new IndividualPosture(parent);
        }

       
        #endregion


        public void analyze()
        {
            if(ready)
            {
                individualTracker.analyze();
                if (individualTracker.mistakeList.Count > 0)
                {
                    currentMistake = true;
                    if(currentMistake!=previousMistake)
                    {
                        MainWindow.stringIndividualFeedbacks = MainWindow.stringIndividualFeedbacks + System.Environment.NewLine+
                            "<feedback>" + currentInstruction.ToString() + "<time started>" +
                                DateTime.Now.TimeOfDay.TotalMilliseconds + "</time started>" ;
                    }
                    previousMistake = currentMistake;
                    doMistakeStuff();
                }
                else
                {
                    currentMistake = false;
                    if (currentMistake != previousMistake)
                    {
                        MainWindow.stringIndividualFeedbacks = MainWindow.stringIndividualFeedbacks + System.Environment.NewLine +
                             "<time finished>" + DateTime.Now.TimeOfDay.TotalMilliseconds + "</time finished>" +
                             System.Environment.NewLine + "</feedback>";
                    }
                    previousMistake = currentMistake;
                    setFeedbackInvisible();
                }
                checkGiveInstructions();

            }
    
        }

        private void checkGiveInstructions()
        {
            double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
            if(isSpeaking==true && 
                parent.audioHandler.audioPreAnalysis.isSpeaking==false)
            {
                
                if (currentTime - StartedTime > Duration)
                {
                    MainWindow.stringIndividualSkills = MainWindow.stringIndividualSkills + System.Environment.NewLine +
                        "<time finished>" + currentTime + "</time finished>";
                    setFinishThings();
                    ready = false;
                }
                else
                {
                    individualInstructions.Visibility = Visibility.Visible;
                    individualInstructions.stopLabel.Visibility = Visibility.Collapsed;
                    individualInstructions.startAnimation();
                    isSpeaking = false;
                    actionTime = currentTime;
                }
                
               
            }
            else if (isSpeaking == false && individualInstructions.animationFinished==true
                && parent.audioHandler.audioPreAnalysis.isSpeaking==true)
            {
                
                individualInstructions.setInvisible();
                individualInstructions.Visibility = Visibility.Collapsed;
                isSpeaking = true;
                actionTime = currentTime;
               
            }
            else if (parent.audioHandler.audioPreAnalysis.isSpeaking==true && 
                individualInstructions.animationFinished==false)
            {
              //  individualInstructions.Visibility = Visibility.Visible;
                individualInstructions.setInvisible();
                individualInstructions.showStop();
                isSpeaking = true;
            }
            else if(parent.audioHandler.audioPreAnalysis.isSpeaking==true)
            {
                
                doSpeakingFeedback();
            }
            else if (parent.audioHandler.audioPreAnalysis.isSpeaking == false)
            {
                
                doPausingFeedback();
            }
            
        }

        private void doPausingFeedback()
        {
            
            if (CadenceCounter.Visibility == Visibility.Visible)
            {
                double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
                CadenceCounter.Content = (int)(currentTime - actionTime);
                if (currentTime - actionTime < 2000)
                {
                    CadenceCounter.Foreground = Brushes.White;
                    ghost.Visibility = Visibility.Collapsed;
                }
                else if (currentTime - actionTime < 3000)
                {
                    CadenceCounter.Foreground = Brushes.Yellow;
                }
                else
                {
                    CadenceCounter.Foreground = Brushes.Red;
                    var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_start_speaking.png", UriKind.Relative);
                    ghost.ghostImg.Source = new BitmapImage(uriSource);
                    ghost.CurrentFeedback.Content = "Start Speaking";
                }
            }
           
        }

        private void doSpeakingFeedback()
        {
            
            if(CadenceCounter.Visibility == Visibility.Visible)
            {
                double currentTime = DateTime.Now.TimeOfDay.TotalMilliseconds;
                CadenceCounter.Content = (int)(currentTime - actionTime);
                if (currentTime - actionTime < 5000)
                {
                    CadenceCounter.Foreground = Brushes.White;
                    ghost.Visibility = Visibility.Collapsed;
                }
                else if (currentTime - actionTime < 9500)
                {
                    CadenceCounter.Foreground = Brushes.Yellow;
                }
                else
                {
                    CadenceCounter.Foreground = Brushes.Red;
                    var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_pause_speaking.png", UriKind.Relative);
                    ghost.ghostImg.Source = new BitmapImage(uriSource);
                    ghost.Visibility = Visibility.Visible;
                    ghost.CurrentFeedback.Content = "Stop Speaking";
                }
            }
           
        }

        private void setFeedbackInvisible()
        {
            setGhostInvisible();
            setOldTextInvisible();
            setFeedbackTextInvisible();
        }

        private void doMistakeStuff()
        {
            ghost.Visibility = Visibility.Visible;
        //    ghostTF.Visibility = Visibility.Visible;
        //    ghostTF.CurrentFeedback.Content = feedbackText;
        }

        public void setFinishThings()
        {
            finish.Visibility = Visibility.Visible;
            try
            {
                finish.pauseSound.Stop();
                finish.pauseSound.Play();
            }
            catch (Exception ex)
            {
                int x = 0;
                x++;
            }
        }
    }
}
