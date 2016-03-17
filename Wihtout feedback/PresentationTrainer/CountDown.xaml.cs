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
    /// Interaction logic for CountDown.xaml
    /// </summary>
    public partial class CountDown : UserControl
    {

        public delegate void CountdownFinished(object sender);
        public event CountdownFinished countdownFinished;

        public CountDown()
        {
            InitializeComponent();
        }
        public void startAnimation()
        {
            myText.Content = "  3";
            var animation3Opacity = new DoubleAnimation();
            animation3Opacity.From = 1.0;
            animation3Opacity.To = 0;
            animation3Opacity.Duration = new Duration(TimeSpan.FromMilliseconds(1000));

            Storyboard.SetTarget(animation3Opacity,myText);
            Storyboard.SetTargetProperty(animation3Opacity, new PropertyPath(UIElement.OpacityProperty));

            Storyboard animating3 = new Storyboard();
            animating3.Children.Add(animation3Opacity);
            animating3.Completed += animating3_Completed;
            animating3.Begin();
            
            
        }

        void animating3_Completed(object sender, EventArgs e)
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

        void animating2_Completed(object sender, EventArgs e)
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

        void animating1_Completed(object sender, EventArgs e)
        {
            myText.Content = " GO";
            var animationGoOpacity = new DoubleAnimation();
            animationGoOpacity.From = 1.0;
            animationGoOpacity.To = 0;
            animationGoOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(1000));

            Storyboard.SetTarget(animationGoOpacity, myText);
            Storyboard.SetTargetProperty(animationGoOpacity, new PropertyPath(UIElement.OpacityProperty));

            Storyboard animatingGo = new Storyboard();
            animatingGo.Children.Add(animationGoOpacity);
            animatingGo.Completed += animatingGo_Completed;
            animatingGo.Begin();
        }

        void animatingGo_Completed(object sender, EventArgs e)
        {
            countdownFinished(this);
        }

    }
}
