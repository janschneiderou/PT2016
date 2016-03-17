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
using Microsoft.Kinect;
using Microsoft.Kinect.Wpf.Controls;

namespace PresentationTrainer
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();

            
            KinectRegion.SetKinectRegion(this, kinectRegion);


            App app = ((App)Application.Current);
            app.KinectRegion = kinectRegion;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FreestyleButton.Content = "Pressed";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
