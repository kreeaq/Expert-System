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

namespace SEiED_1.View
{
    /// <summary>
    /// Interaction logic for PickUpFileView.xaml
    /// </summary>
    public partial class PickUpFileView : UserControl
    {
        public PickUpFileView()
        {
            InitializeComponent();
        }

        private void StackPanel_DragOver(object sender, DragEventArgs e)
        {
            //TODO: Make kind of grayin out the backgroung while dragging over the file
        }
    }
}
