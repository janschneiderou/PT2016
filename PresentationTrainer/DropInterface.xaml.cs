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
    /// Interaction logic for DropInterface.xaml
    /// </summary>
    public partial class DropInterface : UserControl
    {
        public DropInterface()
        {
            InitializeComponent();
        }
        public void setText(string a, string b)
        {
            first.Content = a;
            second.Content = b;
        }

        public string getText()
        {
            return first.Content + " " + second.Content;
        }
    }
}
