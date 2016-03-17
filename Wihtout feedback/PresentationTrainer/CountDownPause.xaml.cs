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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationTrainer
{
    /// <summary>
    /// Interaction logic for CountDownPause.xaml
    /// </summary>
    public partial class CountDownPause : UserControl
    {
        public delegate void CountdownFinished(object sender);
        public event CountdownFinished countdownFinished;
        public bool animationStarted=false;

        public CountDownPause()
        {
            InitializeComponent();
        }

        public void startAnimation()
        {
            animationStarted = true;
            myText.Visibility = Visibility.Visible;
            myText.Content = "  3";
            var animation3Opacity = new DoubleAnimation();
            animation3Opacity.From = 1.0;
            animation3Opacity.To = 0;
            animation3Opacity.Duration = new Duration(TimeSpan.FromMilliseconds(1000));

            Storyboard.SetTarget(animation3Opacity, myText);
            Storyboard.SetTargetProperty(animation3Opacity, new PropertyPath(UIElement.OpacityProperty));

            Storyboard animating3 = new Storyboard();
            animating3.Children.Add(animation3Opacity);
            animating3.Completed += animating3_Completed;
            animating3.Begin();


        }

        void animating3_Completed(object sender, EventArgs e)
        {
            if(MainWindow.stopGesture)
            {
                myText.Content = "  2";
                var animation2Opacity = new DoubleAnimation();
                animation2Opacity.From = 1.0;
                animation2Opacity.To = 0;
                animation2Opacity.Duration = new Duration(TimeSpan.FromMilliseconds(1000));

                Storyboard.SetTarget(animation2Opacity, myText);
                Storyboard.SetTargetProperty(animation2Opacity, new PropertyPath(UIElement.OpacityProperty));

                Storyboard animating2 = new Storyboard();
                animating2.Children.Add(animation2Opacity);
                animating2.Completed += animating2_Completed;
                animating2.Begin();
            }
            else
            {
                myText.Visibility = Visibility.Collapsed;
                animationStarted = false;
            }

            

        }

        void animating2_Completed(object sender, EventArgs e)
        {
            if(MainWindow.stopGesture)
            {
                myText.Content = "  1";
                var animation1Opacity = new DoubleAnimation();
                animation1Opacity.From = 1.0;
                animation1Opacity.To = 0;
                animation1Opacity.Duration = new Duration(TimeSpan.FromMilliseconds(1000));

                Storyboard.SetTarget(animation1Opacity, myText);
                Storyboard.SetTargetProperty(animation1Opacity, new PropertyPath(UIElement.OpacityProperty));

                Storyboard animating1 = new Storyboard();
                animating1.Children.Add(animation1Opacity);
                animating1.Completed += animating1_Completed;
                animating1.Begin();
            }
            else 
            {
                myText.Visibility = Visibility.Collapsed;
                animationStarted = false;
            }
            
        }

        void animating1_Completed(object sender, EventArgs e)
        {
            if(MainWindow.stopGesture)
            {
                countdownFinished(this);
                animationStarted = false;
            }
            else
            {
                myText.Visibility = Visibility.Collapsed;
                animationStarted = false;
            }
            
        }

        void animatingGo_Completed(object sender, EventArgs e)
        {
   
        }
    }
}
