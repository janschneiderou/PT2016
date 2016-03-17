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
    /// Interaction logic for SuperHeroInspiration2.xaml
    /// </summary>
    public partial class SuperHeroInspiration2 : UserControl
    {
        public delegate void SelectionEvent(object sender);
        public event SelectionEvent nextEvent;

        public SuperHeroInspiration2()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            nextEvent(this);
        }
    }
}
