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
    /// Interaction logic for SuperHeroInspiration2.xaml
    /// </summary>
    public partial class SuperHeroInspiration2 : UserControl
    {
        public delegate void SelectionEvent(object sender);
        public event SelectionEvent nextEvent;

        public KinectCoreWindow window;

        public SuperHeroInspiration2()
        {
            InitializeComponent();
            window = KinectCoreWindow.GetForCurrentThread();
            window.PointerMoved += window_PointerMoved;
            setTexts();
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

            drop1.setText("Great", "Determination");
            drop2.setText("", "Kindess");
            drop3.setText("Great", "Creativity");
            drop4.setText("", "Honesty");
            drop5.setText("", "Courage");
            drop6.setText("Super", "Strength");
            drop7.setText("", "Discipline");
            drop8.setText("Great", "Attitude");
            drop9.setText("", "Resilience");
            drop10.setText("Super", "intelligence");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            window.PointerMoved -= window_PointerMoved;
            nextEvent(this);
        }
    }
}
