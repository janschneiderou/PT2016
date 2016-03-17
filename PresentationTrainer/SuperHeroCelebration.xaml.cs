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
    /// Interaction logic for SuperHeroCelebration.xaml
    /// </summary>
    public partial class SuperHeroCelebration : UserControl
    {
        public delegate void SelectionEvent(object sender);
        public event SelectionEvent nextEvent;

        public SuperHeroCelebration()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            nextEvent(this);
        }
    }
}
