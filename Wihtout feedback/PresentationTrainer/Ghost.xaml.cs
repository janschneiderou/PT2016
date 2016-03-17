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
    /// Interaction logic for Ghost.xaml
    /// </summary>
    public partial class Ghost : UserControl
    {
        public Ghost()
        {
            InitializeComponent();
        }
        public void hideFeedback()
        {
            hunch.Visibility = Visibility.Collapsed;
            leftHand.Visibility = Visibility.Collapsed;
            rightHand.Visibility = Visibility.Collapsed;
            back.Visibility = Visibility.Collapsed;
            leftArm.Visibility = Visibility.Collapsed;
            rightArm.Visibility = Visibility.Collapsed;
        }

        public void vanish()
        {
           
            var animation1Opacity = new DoubleAnimation();
            animation1Opacity.From = 1.0;
            animation1Opacity.To = 0;
            animation1Opacity.Duration = new Duration(TimeSpan.FromMilliseconds(3000));

            Storyboard.SetTarget(animation1Opacity, this);
            Storyboard.SetTargetProperty(animation1Opacity, new PropertyPath(UIElement.OpacityProperty));

            Storyboard animating1 = new Storyboard();
            animating1.Children.Add(animation1Opacity);
           // animating1.Completed += animating1_Completed;
            animating1.Begin();
        }

    }
}
