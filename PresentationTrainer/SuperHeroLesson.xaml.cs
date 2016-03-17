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
    /// Interaction logic for SuperHeroLesson.xaml
    /// </summary>
    public partial class SuperHeroLesson : UserControl
    {
        int lessonNumber = 1;
        public delegate void NextEvent(object sender, int x);
        public event NextEvent nextEvent;
        public SuperHeroLesson()
        {
            InitializeComponent();
            setStrings(1);
        }
        public void setStrings(int lesson)
        {
            lessonNumber = lesson;
            switch(lesson)
            {
                case 1:
                    LabelLessonNumber.Content = "1";
                    LabelLessonName.Content = "Posture";
                    LabelLessonExplanation.Content = "In this lesson you will learn the Super Hero Posture.";
                    break;
                case 2:
                    LabelLessonNumber.Content = "2";
                    LabelLessonName.Content = "Select Super Powers";
                    LabelLessonExplanation.Content = "In this lesson you will select 3 super powers \nthat fit your personality.";
                    break;
                case 3:
                    LabelLessonNumber.Content = "3";
                    LabelLessonName.Content = "Inspiration 1";
                    LabelLessonExplanation.Content = "In this lesson you will get in touch \nwith your most inspiring personality trait. ";
                    break;
                case 4:
                    LabelLessonNumber.Content = "4";
                    LabelLessonName.Content = "Inspiration 2";
                    LabelLessonExplanation.Content = "In this lesson you will reflect on \nwhat you find inspiring \nin others.";
                    break;
                case 5:
                    LabelLessonNumber.Content = "5";
                    LabelLessonName.Content = "Impact";
                    LabelLessonExplanation.Content = "In this lesson you will reflect on \nhow as a super hero \n you would like to impact others.";
                    break;
                case 6:
                    LabelLessonNumber.Content = "6";
                    LabelLessonName.Content = "Saving the town";
                    LabelLessonExplanation.Content = "In this lesson you will use yor super strength \nto help the town";
                    break;
                case 7:
                    LabelLessonNumber.Content = "7";
                    LabelLessonName.Content = "Celebration";
                    LabelLessonExplanation.Content = "In this lesson you will celebrate your \n Super Hero Achivements";
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            nextEvent(this, lessonNumber);
            this.Visibility = Visibility.Collapsed;
        }
    }
}
