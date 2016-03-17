using Microsoft.Kinect;
using System;
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
    /// Interaction logic for FreestyleMode.xaml
    /// </summary>
    public partial class FreestyleMode : UserControl
    {
        public MainWindow parent;
        public FreestyleMode()
        {
            InitializeComponent();
        }
        public void loaded()
        {
            backgroundImg.Width = parent.ActualWidth;
            backgroundImg.Height = parent.ActualHeight;
            Canvas.SetLeft(backgroundImg, 0);
            Canvas.SetTop(backgroundImg, 0);
            myBody.initialize(parent);
            myAudio.initialize(parent);
            mySkeleton.initialize(parent);
            parent.rulesAnalyzer.hmmmEvent += rulesAnalyzer_hmmmEvent;
            parent.rulesAnalyzer.tooLoudEvent += rulesAnalyzer_tooLoudEvent;
            parent.rulesAnalyzer.badPostureEvent += rulesAnalyzer_badPostureEvent;
            parent.rulesAnalyzer.noMistakeEvent += rulesAnalyzer_noMistakeEvent;
            parent.rulesAnalyzer.periodicMovementsEvent += rulesAnalyzer_periodicMovementsEvent;

            parent.rulesAnalyzer.feedBackEvent += rulesAnalyzer_feedBackEvent;
            parent.rulesAnalyzer.volumeMistakeEvent += rulesAnalyzer_volumeMistakeEvent;
            parent.rulesAnalyzer.pauseMistakeEvent += rulesAnalyzer_pauseMistakeEvent;
            parent.rulesAnalyzer.postureMistakeEvent += rulesAnalyzer_postureMistakeEvent;
            parent.rulesAnalyzer.handMovementMistakeEvent += rulesAnalyzer_handMovementMistakeEvent;
        }

        void rulesAnalyzer_periodicMovementsEvent(object sender, string e)
        {
            textFeedback.CurrentFeedback.Content = "Stop Dancing "+ e;
        }

        void rulesAnalyzer_noMistakeEvent(object sender, EventArgs e)
        {
            ghost.Visibility = Visibility.Collapsed;
          //  textFeedback.CurrentFeedback.Content = ":)";
        }

        void rulesAnalyzer_badPostureEvent(object sender, EventArgs e)
        {
            float factor=490;
            float displacement= 378;
            float xHead= parent.bodyFrameHandler.bodyFramePreAnalysis.body.Joints[JointType.Head].Position.X;

            Canvas.SetLeft(ghost, factor * xHead + displacement);
            ghost.Visibility = Visibility.Visible;
            
            textFeedback.CurrentFeedback.Content = "Reset Posture " ;
        }

        void rulesAnalyzer_hmmmEvent(object sender, EventArgs e)
        {
            textFeedback.CurrentFeedback.Content = "HMMMMMMM :(";
        }
        void rulesAnalyzer_tooLoudEvent(object sender, EventArgs e)
        {
            textFeedback.CurrentFeedback.Content = "LOWER YOUR VOICE!!!!!!!!!";
        }

        void rulesAnalyzer_feedBackEvent(object sender, Mistake x)
        {
            if (x.hasEnded)
            {
                textFeedback.CurrentFeedback.Content = ":-D";
            }
            else
            {
                textFeedback.CurrentFeedback.Content = x.getString();
            }
        }

        void rulesAnalyzer_volumeMistakeEvent(object sender, Mistake x)
        {
            textFeedback.CurrentFeedback.Content = x.getString();
        }

        void rulesAnalyzer_pauseMistakeEvent(object sender, Mistake x)
        {
            textFeedback.CurrentFeedback.Content = x.getString();
        }

        void rulesAnalyzer_postureMistakeEvent(object sender, Mistake x)
        {
            textFeedback.CurrentFeedback.Content = x.getString();
        }

        void rulesAnalyzer_handMovementMistakeEvent(object sender, Mistake x)
        {
            textFeedback.CurrentFeedback.Content = x.getString();
        }
    }
}
