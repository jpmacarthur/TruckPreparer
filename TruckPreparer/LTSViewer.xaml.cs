using ExcelDataReader;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
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
using LCP.Common.Json;
using TruckPreparer.SpecialArea;

namespace TruckPreparer
{
    /// <summary>
    /// Interaction logic for LTSViewer.xaml
    /// </summary>
    public partial class LTSViewer : UserControl
    {
        public LTS source;
        ObservableCollection<LTSItem> ltssource = new ObservableCollection<LTSItem>();
        LTS src;
        public LTSViewer()
        {
            InitializeComponent();
            LTS_LB.ItemsSource = ltssource;
        }
        public void Reload(LTS src)
        {
            this.src = src;
            ltssource.Clear();
            foreach(LTSItem item in src.Items)
            {
                ltssource.Add(item);
            }
        }
        public ObservableCollection<LTSItem> ToObservable(LTS source)
        {
            ObservableCollection<LTSItem> ltsitems = new ObservableCollection<LTSItem>();
            foreach(LTSItem item in source.Items)
            {
                ltsitems.Add(item);
            }
            return ltsitems;
        }

        public void ToObservable(LTS source, ObservableCollection<LTSItem> items)
        {
            items.Clear();
            foreach (LTSItem item in source.Items)
            {
                items.Add(item);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            string path = "";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                path = openFileDialog.FileName;
            }
            if (path != "")
            {
                src.Items = ParseToHighlyRated(path);
                ToObservable(src, ltssource);
                src.Save();
            }

        }
        static public List<LTSItem> ParseToHighlyRated(string path)
        {
            DataSet result = new DataSet();
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {

                // Auto-detect format, supports:
                //  - Binary Excel files (2.0-2003 format; *.xls)
                //  - OpenXml Excel files (2007 format; *.xlsx)
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {

                    // Choose one of either 1 or 2:

                    // 1. Use the reader methods
                    do
                    {
                        while (reader.Read())
                        {
                            // reader.GetDouble(0);
                        }
                    } while (reader.NextResult());

                    // 2. Use the AsDataSet extension method
                    result = reader.AsDataSet();

                    // The result of each spreadsheet is in result.Tables
                }
            }
            DataTable data = result.Tables[0];

            Dictionary<string, TruckItem> final = new Dictionary<string, TruckItem>();
            List<string> names = new List<string>();

            List<LTSItem> items = new List<LTSItem>();

            int num = data.Rows.Count;
            for (int i = 5; i <= num - 1; i++)
            {
                LTSItem item = new LTSItem();
                item.Name = data.Rows[i].ItemArray[1].ToString();
                item.Size = data.Rows[i].ItemArray[2].ToString();
                item.Itemcode = data.Rows[i].ItemArray[0].ToString();
                items.Add(item);
            }
            return items;
        }
        private LTS FromCollectionToLTS(ObservableCollection<LTSItem> items)
        {
            LTS temp = new LTS();
            foreach(LTSItem item in items)
            {
                temp.Items.Add(item);
            }
            return new LTS();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            List<LTSItem> pos = new List<LTSItem>();
            foreach(LTSItem item in LTS_LB.SelectedItems)
            {
                pos.Add(item);
            }
            foreach(LTSItem positives in pos)
            {
                ltssource.Remove(positives);
                src.Items.Remove(positives);
            }
            src.Save();
        }
    }
}
