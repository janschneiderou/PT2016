using Microsoft.Kinect.Input;
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
    /// Interaction logic for SuperHeroPowers.xaml
    /// </summary>
    public partial class SuperHeroPowers : UserControl
    {

        public delegate void SelectionEvent(object sender);
        public event SelectionEvent nextEvent;

        public KinectCoreWindow window;

        public SuperHeroPowers()
        {
            InitializeComponent();
            setTexts();
            window = KinectCoreWindow.GetForCurrentThread();
            window.PointerMoved += window_PointerMoved;
        }


        private void window_PointerMoved(object sender, KinectPointerEventArgs e)
        {
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/Button_start.png", UriKind.Relative);

            if ((!BodyFramePreAnalysis.rightHandUp && e.CurrentPoint.Properties.HandType == HandType.LEFT) ||
             (BodyFramePreAnalysis.rightHandUp && e.CurrentPoint.Properties.HandType == HandType.RIGHT))
            {
                if (e.CurrentPoint.Position.X * this.ActualWidth > Canvas.GetLeft(nextButton) && e.CurrentPoint.Position.X * this.ActualWidth < Canvas.GetLeft(nextButton) + nextButton.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Canvas.GetTop(nextButton) && e.CurrentPoint.Position.Y * this.ActualHeight < Canvas.GetTop(nextButton) + nextButton.Height)
                {
                    uriSource = new Uri(@"/PresentationTrainer;component/Images/Button_next_hover.png", UriKind.Relative);
                    imgButton.Source = new BitmapImage(uriSource);
                }
                else
                {
                    uriSource = new Uri(@"/PresentationTrainer;component/Images/Button_next.png", UriKind.Relative);
                    imgButton.Source = new BitmapImage(uriSource);
                }

            }
        }

        private void setTexts()
        {
           drop1.setText("Super", "Confidence");
            drop2.setText("Super", "Willpower");
            drop3.setText("Super", "Intelligence");
            drop4.setText("Super", "Creativity");
            drop5.setText("Super", "Empathy");
            drop6.setText("Super", "Concentration");
            drop7.setText("Super", "Presence");
            drop8.setText("Super", "Charisma");
            drop9.setText("Super", "Attention");
            drop10.setText("Super", "Assertiveness");
       
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            window.PointerMoved -= window_PointerMoved;
            nextEvent(this);

        }
    }
}
