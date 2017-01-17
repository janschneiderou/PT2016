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
    /// Interaction logic for PostureReport.xaml
    /// </summary>
    public partial class PostureReport : UserControl
    {
        string[] questions;
        int currentQuestion = 0;

        public PostureReport()
        {
            InitializeComponent();

            questions = new string[3];

            questions[0] = "The attitude reflected in your posture,\nis the same attitude that you want to convey?";
            questions[1] = "What would you improve from your posture?";
            questions[2] = "Is your posture helping you to engage with the audience?";

            labelQuestion.Content = questions[currentQuestion];

            posture1.Source = MainWindow.postureImages[0];
            posture2.Source = MainWindow.postureImages[1];
            posture3.Source = MainWindow.postureImages[2];
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
