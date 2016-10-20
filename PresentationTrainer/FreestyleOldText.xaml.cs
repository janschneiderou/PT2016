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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PresentationTrainer
{
    /// <summary>
    /// Interaction logic for FreestyleOldText.xaml
    /// </summary>
    public partial class FreestyleOldText : UserControl
    {
        public FreestyleOldText()
        {
            InitializeComponent();
        }

        internal void startBanish()
        {
            
            Storyboard s;
            try
            {
                s = (Storyboard)this.FindResource("MyStoryboard") as Storyboard;
                Storyboard.SetTarget(s, myGrid);
                s.Begin();
            }
            catch (Exception e)
            {
                int x = 0;
                x++;
            }
            
           
        }
        public void vanish()
        {
            
            var animationGoOpacity = new DoubleAnimation();
            animationGoOpacity.From = 1.0;
            animationGoOpacity.To = 0;
            animationGoOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(1000));

            Storyboard.SetTarget(animationGoOpacity, this);
            Storyboard.SetTargetProperty(animationGoOpacity, new PropertyPath(UIElement.OpacityProperty));

            Storyboard animatingGo = new Storyboard();
            animatingGo.Children.Add(animationGoOpacity);
            
            animatingGo.Begin();
        }
    }
}
