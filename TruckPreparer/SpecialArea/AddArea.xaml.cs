using Microsoft.Win32;
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

namespace TruckPreparer.SpecialArea
{
    /// <summary>
    /// Interaction logic for AddArea.xaml
    /// </summary>
    public partial class AddArea : UserControl
    {
        public delegate void NewReportEventHandler(object sender, AddAreaEventArgs args);
        public event NewReportEventHandler AddItem;
        AddAreaProp ap = new AddAreaProp();
        public AddArea()
        {
            InitializeComponent();
            DataContext = ap;
            ap.StartDateTime = DateTime.Now;
            ap.EndDateTime = DateTime.Now;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.AddItem != null)
            {
                this.AddItem(this, new AddAreaEventArgs(ap.Name, ap.FileLocation, ap.StartDateTime, ap.EndDateTime));
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                ap.FileLocation = dialog.FileName;
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            BindingExpression be = Area_TB.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();
        }
    }
}
