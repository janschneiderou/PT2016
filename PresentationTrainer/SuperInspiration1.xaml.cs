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
    /// Interaction logic for SuperInspiration1.xaml
    /// </summary>
    public partial class SuperInspiration1 : UserControl
    {
        public delegate void SelectionEvent(object sender, string x);
        public event SelectionEvent selectionEvent;
        public string value;
        public KinectCoreWindow window;

        public SuperInspiration1()
        {
            InitializeComponent();
            window = KinectCoreWindow.GetForCurrentThread();
            window.PointerMoved += window_PointerMoved;

        }


        private void window_PointerMoved(object sender, KinectPointerEventArgs e)
        {
            var uriSource = new Uri(@"/PresentationTrainer;component/Images/Button_start.png", UriKind.Relative);

            if ((!BodyFramePreAnalysis.rightHandUp && e.CurrentPoint.Properties.HandType == HandType.LEFT) ||
             (BodyFramePreAnalysis.rightHandUp && e.CurrentPoint.Properties.HandType == HandType.RIGHT))
            {
                if (e.CurrentPoint.Position.X * this.ActualWidth > Confidence.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Confidence.Margin.Left + Confidence.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Confidence.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Confidence.Margin.Top + Confidence.Height)
                {
                   
                    Confidence.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Confidence.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }

                if (e.CurrentPoint.Position.X * this.ActualWidth > Kind.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Kind.Margin.Left + Kind.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Kind.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Kind.Margin.Top + Kind.Height)
                {
                   
                    Kind.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Kind.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }

                if (e.CurrentPoint.Position.X * this.ActualWidth > Friendly.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Friendly.Margin.Left + Friendly.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Friendly.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Friendly.Margin.Top + Friendly.Height)
                {
                   
                    Friendly.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Friendly.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }
                if (e.CurrentPoint.Position.X * this.ActualWidth > Fun.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Fun.Margin.Left + Fun.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Fun.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Fun.Margin.Top + Fun.Height)
                {
                   
                    Fun.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Fun.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }
                if (e.CurrentPoint.Position.X * this.ActualWidth > Disciplined.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Disciplined.Margin.Left + Disciplined.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Disciplined.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Disciplined.Margin.Top + Disciplined.Height)
                {
                   
                    Disciplined.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Disciplined.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }        
                if (e.CurrentPoint.Position.X * this.ActualWidth > Attractive.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Attractive.Margin.Left + Attractive.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Attractive.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Attractive.Margin.Top + Attractive.Height)
                {
                   
                    Attractive.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Attractive.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }             
                if (e.CurrentPoint.Position.X * this.ActualWidth > Balanced.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Balanced.Margin.Left + Balanced.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Balanced.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Balanced.Margin.Top + Balanced.Height)
                {
                   
                    Balanced.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Balanced.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }              
                if (e.CurrentPoint.Position.X * this.ActualWidth > Intelligent.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Intelligent.Margin.Left + Intelligent.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Intelligent.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Intelligent.Margin.Top + Intelligent.Height)
                {
                   
                    Intelligent.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Intelligent.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }                
                if (e.CurrentPoint.Position.X * this.ActualWidth > Brave.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Brave.Margin.Left + Brave.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Brave.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Brave.Margin.Top + Brave.Height)
                {
                   
                    Brave.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Brave.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }                   
                 if (e.CurrentPoint.Position.X * this.ActualWidth > Brave.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Creative.Margin.Left + Creative.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Brave.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Creative.Margin.Top + Creative.Height)
                {
                   
                    Creative.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Creative.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }                       
                 if (e.CurrentPoint.Position.X * this.ActualWidth > Adaptable.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Adaptable.Margin.Left + Adaptable.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Adaptable.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Adaptable.Margin.Top + Adaptable.Height)
                {
                   
                    Adaptable.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Adaptable.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }                 
                 if (e.CurrentPoint.Position.X * this.ActualWidth > Charming.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Charming.Margin.Left + Charming.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Charming.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Charming.Margin.Top + Charming.Height)
                {
                   
                    Charming.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Charming.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }                         
                 if (e.CurrentPoint.Position.X * this.ActualWidth > Bright.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Bright.Margin.Left + Bright.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Bright.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Bright.Margin.Top + Bright.Height)
                {
                   
                    Bright.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Bright.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }                           
                 if (e.CurrentPoint.Position.X * this.ActualWidth > Loyal.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Loyal.Margin.Left + Loyal.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Loyal.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Loyal.Margin.Top + Loyal.Height)
                {
                   
                    Loyal.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Loyal.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }                              
                 if (e.CurrentPoint.Position.X * this.ActualWidth > Honest.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Honest.Margin.Left + Honest.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Honest.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Honest.Margin.Top + Honest.Height)
                {
                   
                    Honest.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Honest.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }                               
                 if (e.CurrentPoint.Position.X * this.ActualWidth > Modest.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Modest.Margin.Left + Modest.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Modest.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Modest.Margin.Top + Modest.Height)
                {
                   
                    Modest.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Modest.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }                                   
                 if (e.CurrentPoint.Position.X * this.ActualWidth > Reliable.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Reliable.Margin.Left + Reliable.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Reliable.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Reliable.Margin.Top + Reliable.Height)
                {
                   
                    Reliable.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Reliable.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }
                 if (e.CurrentPoint.Position.X * this.ActualWidth > Witty.Margin.Left && e.CurrentPoint.Position.X * this.ActualWidth < Witty.Margin.Left + Witty.Width
               && e.CurrentPoint.Position.Y * this.ActualHeight > Witty.Margin.Top && e.CurrentPoint.Position.Y * this.ActualHeight < Witty.Margin.Top + Witty.Height)
                {

                    Witty.Background = new BrushConverter().ConvertFromString("#9bff43") as SolidColorBrush;
                        
                }
                else
                {
                    Witty.Background = new BrushConverter().ConvertFromString("#262262") as SolidColorBrush;
                }                                         
                                                    
                                                        
                                                        

            }
        }

        private void GoTo_Click(object sender, RoutedEventArgs e)
        {
            value = ((Button)sender).Content.ToString();
            try
            {
                selectionEvent(this, value);
            }
            catch
            {

            }

        }
    }
}
