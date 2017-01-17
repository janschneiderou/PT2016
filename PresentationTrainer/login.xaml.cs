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
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : UserControl
    {
        public login()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            if(textBox.Text.Length<3)
            {
                textBox.Text = textBox.Text + "1";
            }
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length < 3)
            {
                textBox.Text = textBox.Text + "2";
            }
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length < 3)
            {
                textBox.Text = textBox.Text + "3";
            }
        }

        

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length < 3)
            {
                textBox.Text = textBox.Text + "4";
            }
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length < 3)
            {
                textBox.Text = textBox.Text + "5";
            }
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length < 3)
            {
                textBox.Text = textBox.Text + "6";
            }
        }

       

        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length < 3)
            {
                textBox.Text = textBox.Text + "7";
            }
        }

        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length < 3)
            {
                textBox.Text = textBox.Text + "8";
            }
        }

        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length < 3)
            {
                textBox.Text = textBox.Text + "9";
            }
        }

        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length < 3)
            {
                textBox.Text = textBox.Text + "0";
            }
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text.Length > 1)
            {
                textBox.Text.Remove(textBox.Text.Length - 1);
            }
        }
       
    }
}
