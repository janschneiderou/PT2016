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
    /// Interaction logic for SuperHeroAffirmation.xaml
    /// </summary>
    /// 

    public partial class SuperHeroAffirmation : UserControl
    {

        int lessonNumber = 1;
        public delegate void NextEvent(object sender, int x);
        public event NextEvent nextEvent;
        public string selections;
        public string inspiringValue;

        public KinectCoreWindow window;

        public SuperHeroAffirmation()
        {


            InitializeComponent();
            setStrings(1);
            window = KinectCoreWindow.GetForCurrentThread();
            window.PointerMoved += window_PointerMoved;
        }

        private void window_PointerMoved(object sender, KinectPointerEventArgs e)
        {
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/Button_start.png", UriKind.Relative);

             if ((!BodyFramePreAnalysis.rightHandUp && e.CurrentPoint.Properties.HandType == HandType.LEFT) ||
              (BodyFramePreAnalysis.rightHandUp && e.CurrentPoint.Properties.HandType == HandType.RIGHT))
            {
                if (e.CurrentPoint.Position.X * this.ActualWidth > button1.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < button1.Margin.Left + button1.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > button1.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < button1.Margin.Top + button1.Height)
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
         public void setStrings(int lesson)
        {
            lessonNumber = lesson;
            switch(lesson)
            {
                case 1:
                    LabelLessonNumber.Content = "1";
                    LabelLessonName.Content = "Posture";
                    LabelLessonExplanation.Content = "That's the Posture";
                    break;
                case 2:
                    LabelLessonNumber.Content = "2";
                    LabelLessonName.Content = "Select Super Powers";
                    LabelLessonExplanation.Content = "Imagine for a moment how would you \nMove, Stand and Talk now that you have: \n" + selections;
                    break;
                case 3:
                    LabelLessonNumber.Content = "3";
                    LabelLessonName.Content = "Inspiration 1";
                    LabelLessonExplanation.Content ="Being " + inspiringValue+ ": \nIsn't that inspiring?";
                    break;
                case 4:
                    LabelLessonNumber.Content = "4";
                    LabelLessonName.Content = "Inspiration 2";
                    LabelLessonExplanation.Content = "How do you feel\nin the Presence of: \n" + selections;
                    break;
                case 5:
                    LabelLessonNumber.Content = "5";
                    LabelLessonName.Content = "Impact";
                    LabelLessonExplanation.Content = "Imagine for a moment how would the world be\nwith more: \n" + selections;
                    break;
                case 6:
                    LabelLessonNumber.Content = "6";
                    LabelLessonName.Content = "Impact";
                    LabelLessonExplanation.Content = "Imagine for a moment how would the world be\nwith more: \n" + selections;
                    break;
                case 7:
                    LabelLessonNumber.Content = "6";
                    LabelLessonName.Content = "Celebration";
                    LabelLessonExplanation.Content = "Don't fake it till you make it, fake it til you become it.";
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            nextEvent(this, lessonNumber);
            window.PointerMoved -= window_PointerMoved;
            this.Visibility = Visibility.Collapsed;
        }
    }
  
}
