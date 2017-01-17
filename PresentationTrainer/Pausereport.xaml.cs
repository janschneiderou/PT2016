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
    /// Interaction logic for Pausereport.xaml
    /// </summary>
    public partial class Pausereport : UserControl
    {
        string[] questions;
        int currentQuestion = 0;

        public Pausereport()
        {
            InitializeComponent();

            questions = new string[3];

            questions[0] = "Are you using your pauses with purpose?";
            questions[1] = "How can you improve your use of pauses?";
            questions[2] = "Can you add some longer pauses?";

            labelQuestion.Content = questions[currentQuestion];

            double xfactor = 800/MainWindow.presentationDuration;
            double leftPoint;
            double rightPoint;
            double speakingTime=0;
            double pausesTime = 0;
            int totalSpeaks = 0;

            totalTime.Content = (int)(MainWindow.presentationDuration / 1000);

            for(int i =0; i< MainWindow.speakTimes.Count;i++)
            {
                if(i+1==MainWindow.speakTimes.Count)
                {
                    speakingTime = speakingTime + (MainWindow.presentationFinished- (double)MainWindow.speakTimes[i]);
                    totalSpeaks++;
                    Rectangle rect = new Rectangle();
                    speakingTimeline.Children.Add(rect);
                    
                    leftPoint = ((double)MainWindow.speakTimes[i]-MainWindow.presentationStarted)*xfactor;
                    rect.Width = 800 - leftPoint;
                    rect.Height = 184;
                    rect.Fill = Brushes.Orange;
                    Canvas.SetLeft(rect, leftPoint);                   
                    break;
                }
                else
                {
                    speakingTime = speakingTime + ((double)MainWindow.speakTimes[i+1] - (double)MainWindow.speakTimes[i]);
                    if(i+2<MainWindow.speakTimes.Count)
                    {
                        pausesTime = pausesTime + ((double)MainWindow.speakTimes[i + 2] - (double)MainWindow.speakTimes[i + 1]);
                    }
                    Rectangle rect = new Rectangle();
                    speakingTimeline.Children.Add(rect);
                    leftPoint = ((double)MainWindow.speakTimes[i] - MainWindow.presentationStarted) * xfactor;
                    rightPoint = ((double)MainWindow.speakTimes[i+1] - MainWindow.presentationStarted) * xfactor;
                    rect.Width = rightPoint - leftPoint;
                    rect.Height = 184;
                    rect.Fill = Brushes.Orange;
                    Canvas.SetLeft(rect, leftPoint);
                    totalSpeaks++;
                    i++;
                }
                averageSpeak.Content = Math.Round(speakingTime / totalSpeaks / 1000, 2);
                averagePausingTime.Content = Math.Round( pausesTime / (totalSpeaks - 1) / 1000, 2);
                totalPauses.Content = (int)((MainWindow.speakTimes.Count+1)/2);

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestion < 2)
            {
                currentQuestion++;
                labelQuestion.Content = questions[currentQuestion];
                answer1.Visibility = Visibility.Visible;

                if(currentQuestion==1)
                {
                    GoMainMenu.Visibility = Visibility.Visible;
                    buttonNext.Visibility = Visibility.Collapsed;
                }
            }
            

        }
    }
}
