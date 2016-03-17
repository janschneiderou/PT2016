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
    /// Interaction logic for GhostMoving.xaml
    /// </summary>
    public partial class GhostMoving : UserControl
    {
        public GhostMoving()
        {
            InitializeComponent();
        }

        public void ghostAnimation(Ghost ghostN)
        {
            double left = Canvas.GetLeft(ghostN);
            double top = Canvas.GetTop(ghostN);

           
            //  double left = 600;
            //  double top = 300;
            if (left == double.NaN)
            {
                left = 600;
            }

            Canvas.SetLeft(this, left);
            Canvas.SetTop(this, top);

            //   var uriSource = new Uri(@"/PresentationTrainer;component/Images/ic_fb_move_more.png", UriKind.Relative);
            //   ghostMoving.Source = new BitmapImage(uriSource);
           


            var animationWidth = new DoubleAnimation();
            ghostN.RenderTransformOrigin = new Point(0.5, 0.5);
            // animationWidth.From = this.animationWidth;
            animationWidth.From = 300;
            animationWidth.To = 30;
            animationWidth.Duration = new Duration(TimeSpan.FromMilliseconds(2000));

            Storyboard.SetTarget(animationWidth, this);
            Storyboard.SetTargetProperty(animationWidth, new PropertyPath(UserControl.WidthProperty));


            var animationOpacity = new DoubleAnimation();
            animationOpacity.From = 1.0;
            animationOpacity.To = 0;
            animationOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(2000));

            Storyboard.SetTarget(animationOpacity, this);
            Storyboard.SetTargetProperty(animationOpacity, new PropertyPath(UIElement.OpacityProperty));

            Storyboard animatingGhost = new Storyboard();
            // animatingGhost.Children.Add(animationTranslateLeft);
            //  animatingGhost.Children.Add(animationTranslateTop);
            animatingGhost.Children.Add(animationWidth);

            animatingGhost.Children.Add(animationOpacity);
            //  animatingGhost.b
         //   animatingGhost.Completed += animatingGhost_Completed;
            animatingGhost.Begin();

            TranslateTransform trans = new TranslateTransform();
            this.RenderTransform = trans;

            DoubleAnimation anim1;
            try
            {
                anim1 = new DoubleAnimation(0, 230 - left, TimeSpan.FromSeconds(1));
            }
            catch
            {
                anim1 = new DoubleAnimation(0, 230 - 600, TimeSpan.FromSeconds(1));
            }
            
            DoubleAnimation anim2 = new DoubleAnimation(0, -255 - top + this.Height / 2, TimeSpan.FromSeconds(1));
            trans.BeginAnimation(TranslateTransform.XProperty, anim1);
            trans.BeginAnimation(TranslateTransform.YProperty, anim2);
        }
    }
}
