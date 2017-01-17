/**
 * ****************************************************************************
 * Copyright (C) 2016 Open Universiteit Nederland
 * <p/>
 * This library is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * <p/>
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 * <p/>
 * You should have received a copy of the GNU Lesser General Public License
 * along with this library.  If not, see <http://www.gnu.org/licenses/>.
 * <p/>
 * Contributors: Jan Schneider
 * ****************************************************************************
 */

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
    /// Interaction logic for GestureReport.xaml
    /// </summary>
    public partial class GestureReport : UserControl
    {

        string[] questions;
        int currentQuestion = 0;

        public GestureReport()
        {
            InitializeComponent();
            questions = new string[3];
         
            questions[0] = "Is there a meaning behind your gestures?";
            questions[1] = "What gestures can you add to support the communication\nof your message?";
            questions[2] = "Are your gestures helping you to communicate your message?";

            labelQuestion.Content = questions[currentQuestion];
            double xfactor = 800 / MainWindow.presentationDuration;
            double leftPoint;
            double rightPoint;
          

            for (int i = 0; i < MainWindow.gestureTimes.Count; i++)
            {
                if (i + 1 == MainWindow.gestureTimes.Count)
                {
                    
                    
                    Rectangle rect = new Rectangle();
                    speakingTimeline.Children.Add(rect);

                    leftPoint = ((double)MainWindow.gestureTimes[i] - MainWindow.presentationStarted) * xfactor;
                    rect.Width = 800 - leftPoint;
                    rect.Height = 75;
                    rect.Fill = Brushes.Orange;
                    Canvas.SetLeft(rect, leftPoint);
                    break;
                }
                else
                {
                   
                   
                    Rectangle rect = new Rectangle();
                    speakingTimeline.Children.Add(rect);
                    leftPoint = ((double)MainWindow.gestureTimes[i] - MainWindow.presentationStarted) * xfactor;
                    rightPoint = ((double)MainWindow.gestureTimes[i + 1] - MainWindow.presentationStarted) * xfactor;
                    rect.Width = rightPoint - leftPoint;
                    rect.Height = 75;
                    rect.Fill = Brushes.Orange;
                    Canvas.SetLeft(rect, leftPoint);
                   
                    i++;
                }

                gesture1.Source = MainWindow.gestureImages[0];
                gesture2.Source = MainWindow.gestureImages[1];
                gesture3.Source = MainWindow.gestureImages[2];

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestion < 2)
            {
                currentQuestion++;
                labelQuestion.Content = questions[currentQuestion];

                if (currentQuestion == 1)
                {
                    answerQuestion.Visibility = Visibility.Visible;
                    GoMainMenu.Visibility = Visibility.Visible;
                    buttonNext.Visibility = Visibility.Collapsed;
                }
            }
            
        }
    }
}
