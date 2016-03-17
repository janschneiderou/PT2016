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

        public SuperInspiration1()
        {
            InitializeComponent();
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
