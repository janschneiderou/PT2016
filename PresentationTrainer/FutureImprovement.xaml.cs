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
    /// Interaction logic for FutureImprovement.xaml
    /// </summary>
    public partial class FutureImprovement : UserControl
    {
        public delegate void SelectionEvent();
        public event SelectionEvent selectionEvent;

        string nextImprovement="";

        public FutureImprovement()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch(((Button)sender).Name)
            {
                case "buttonPosture":
                    nextImprovement = "Next Posture";
                    MainWindow.focusedString = MainWindow.focusedPosture;
                    break;
                case "buttonVolume":
                    nextImprovement = "Next Volume";
                    MainWindow.focusedString = "";
                    break;
                case "buttonGesture":
                    nextImprovement = "Next Gesture";
                    MainWindow.focusedString = MainWindow.focusedGestures;
                    break;
                case "buttonPauses":
                    nextImprovement = "Next Pauses";
                    MainWindow.focusedString = MainWindow.focusedPauses;
                    break;
                case "buttonFace":
                    nextImprovement = "Next Face";
                    MainWindow.focusedString = "";
                    break;
                    
            }
            

            MainWindow.logString = MainWindow.logString + System.Environment.NewLine + nextImprovement;
            selectionEvent();
        }
    }
}
