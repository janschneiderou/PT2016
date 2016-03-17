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
    /// Interaction logic for HeroMode.xaml
    /// </summary>
    public partial class HeroMode : UserControl
    {
        public delegate void ExitEvent(object sender);
        public event ExitEvent exitEvent;

        public MainWindow parent;
        public string value;
        public string InstructionHeroPosture = "Lets start by standing in the Superhero posture.";
        public string InstructionValue = "Select a value that you identify with.";
        public bool isLoaded = false;

        public HeroMode()
        {
            InitializeComponent();
        }

        public void loaded()
        {
            myCanvas.Height = parent.ActualWidth;
            myCanvas.Width = parent.ActualHeight;
            backgroundImg.Width = parent.ActualWidth;
            backgroundImg.Height = parent.ActualHeight;
            Canvas.SetLeft(backgroundImg, 0);
            Canvas.SetTop(backgroundImg, 0);
            myBody.initializeB(parent);
            myAudio.initialize(parent);
            mySkeleton.initialize(parent);
            parent.heroAnalysis.loadStuff();
            values.Visibility = Visibility.Collapsed;
            heroPower.Visibility = Visibility.Collapsed;
            HeroAffirmation.Visibility = Visibility.Collapsed;
            inspiration1.Visibility = Visibility.Collapsed;
            heroInspiration2.Visibility = System.Windows.Visibility.Collapsed;
            heroImpact.Visibility = System.Windows.Visibility.Collapsed;
            heroCelebration.Visibility = Visibility.Collapsed;
            heroExit.Visibility = Visibility.Collapsed;
            heroExit.nextButton.Click += nextButton_Click;


            isLoaded = true;
        }

        void nextButton_Click(object sender, RoutedEventArgs e)
        {
            music.Stop();
            exitEvent(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            exitEvent(this);
        }
    }
}
