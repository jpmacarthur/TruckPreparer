using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Threading.Tasks;
using log4net;
using LCP.Common.Logging;
using ExcelDataReader;

namespace TruckPreparer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        string saveloc = "";
        string truckloc = "";
        string storeloc = "";
        public MainWindow()
        {
            Logger.Setup();
            DataContext = this;
            InitializeComponent();
         }

        public void ListToExcel(List<TruckItem> list, string savepath)
        {
            /* Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

             Microsoft.Office.Interop.Excel.Workbook excelworkBook;
             Microsoft.Office.Interop.Excel.Worksheet excelSheet;
             // for making Excel visible
             excel.Visible = false;
             excel.DisplayAlerts = false;

             // Creation a new Workbook
             excelworkBook = excel.Workbooks.Add(Type.Missing);

             // Workk sheet
             excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
             excelSheet.Name = "Sheet1";
             int length = list.Count;
             for (int b = 0; b < length; b++)
                 {
                 excelSheet.Cells[b.ToString(), "A"].Value2 = list[b].Name;
                 excelSheet.Cells[b.ToString(), "F"] = list[b].itemcode;
                 excelSheet.Cells[b.ToString(), "C"] = list[b].inStore.cases;
                 excelSheet.Cells[b.ToString(), "D"] = list[b].inStore.units;
                 excelSheet.Cells[b.ToString(), "E"] = list[b].inStore.unitsPerCase;
                 excelSheet.Cells[b.ToString(), "B"] = list[b].size;
             }

             excelworkBook.SaveAs(savepath + "truckitems.xls");
             excelworkBook.Close();
             excel.Quit();*/
            log.Debug("Into ListToExcel");
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            object misvalue = System.Reflection.Missing.Value;
            try
            {

                oXL = new Microsoft.Office.Interop.Excel.Application();
                if (saveloc == "")
                    oXL.Visible = true;
                else oXL.Visible = false;

                //Get a new workbook.
                oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
                oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;
                int length = list.Count;
                oSheet.Cells[1.ToString(), "A"] = "Name";
                oSheet.Cells[1.ToString(), "B"] = "Size";
                oSheet.Cells[1.ToString(), "C"] = "Store ( C)";
                oSheet.Cells[1.ToString(), "D"] = "Store (U)";
                oSheet.Cells[1.ToString(), "E"] = "Truck ( C)";
                oSheet.Cells[1.ToString(), "F"] = "U/C";
                oSheet.Cells[1.ToString(), "G"] = "Item Code";
                oSheet.Cells[1.ToString(), "I"] = "Notes";
                oSheet.Cells[1.ToString(), "H"] = "Location";

                for (int b = 0; b < length; b++)
                {
                    oSheet.Cells[(b + 2).ToString(), "A"] = list[b].Name;
                    oSheet.Cells[(b + 2).ToString(), "G"] = list[b].itemcode;
                    if (Checkboxstat == true)
                    {
                        oSheet.Cells[(b + 2).ToString(), "C"] = (Convert.ToInt32(list[b].inStore.cases) - Convert.ToInt32(list[b].onTruck.cases)).ToString();
                    }
                    else
                    {
                        oSheet.Cells[(b + 2).ToString(), "C"] = list[b].inStore.cases;
                    }
                    oSheet.Cells[(b + 2).ToString(), "D"] = list[b].inStore.units;
                    oSheet.Cells[(b + 2).ToString(), "E"] = list[b].onTruck.cases;
                   // oSheet.Cells[(b + 2).ToString(), "F"] = list[b].onTruck.units;
                    oSheet.Cells[(b + 2).ToString(), "F"] = list[b].inStore.unitsPerCase;
                    oSheet.Cells[(b + 2).ToString(), "B"] = list[b].size;
                    oSheet.Cells[(b + 2).ToString(), "H"] = list[b].location;
                    if(Convert.ToInt32(list[b].inStore.units) == 0 && Convert.ToInt32(list[b].inStore.cases) == 0)
                    {
                        oSheet.Cells[(b + 2).ToString(), "I"] = "Empty on floor";
                    }
                    if(Convert.ToDouble(list[b].fiveweek) == 0 && Convert.ToInt32(list[b].inStore.cases) == 0 && Convert.ToInt32(list[b].inStore.units) == 0) {
                        oSheet.Cells[(b + 2).ToString(), "I"] = "Possible Cut In";
                    }
                    if(list[b].location.Contains("Wine Lockbox"))
                        {
                        oSheet.Cells[(b + 2).ToString(), "I"] = "Lockbox";
                    }
                }
                oSheet.Columns.AutoFit();
                oSheet.get_Range("G:G", Type.Missing).EntireColumn.Hidden = true;
                if (savepath != "")
                {
                    oXL.Visible = false;
                    oXL.UserControl = false;
                    oWB.SaveAs(savepath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                        false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                        Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                    oWB.Close();
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); };
        }
        public void changeSheetNames(string store, string truck)
        {
            log.Debug("Into ChangeSheetNames");
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            object misvalue = System.Reflection.Missing.Value;
            try
            {

                oXL = new Microsoft.Office.Interop.Excel.Application();
                oXL.Visible = false;
                oWB = oXL.Workbooks.Open(store);
                oSheet = (Worksheet)oWB.Worksheets.Item[1];
                oSheet.Name = "Sheet1";
                oWB.Save();
                oXL.Application.Workbooks.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            try
            {

                oXL = new Microsoft.Office.Interop.Excel.Application();
                oXL.Visible = false;
                oWB = oXL.Workbooks.Open(truck);
                oSheet = (Worksheet)oWB.Worksheets.Item[1];
                oSheet.Name = "Sheet1";
                oWB.Save();
                oXL.Application.Workbooks.Close();

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private void save_loc_BT_Click(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DateTime.UtcNow.Date;
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Truck " + dateTime.ToString("dd_MM_yyyy"); // Default file name
            dlg.DefaultExt = ".xls"; // Default file extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                saveloc = dlg.FileName;
            }
        }

        private void truck_xls_BT_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if(result == true)
            {
                truckloc = openFileDialog.FileName;
            }

        }

        private void store_inv_BT_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            openFileDialog.Filter = "Excel Files (*.xls;*.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                storeloc = openFileDialog.FileName;
            }
        }

        private void prep_truck_BT_Click(object sender, RoutedEventArgs e)
        {
            log.Debug("Prep truck clicked");
            LoadGif.Visibility = Visibility.Visible;
            prep_truck_BT.IsEnabled = false;
            Task Start = Task.Factory.StartNew(() =>
            {
                try
                {
                    changeSheetNames(storeloc, truckloc);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Preparing did not work, Pat's probably an idiot");
                    MessageBox.Show(ex.ToString());
                }
                TruckParser parser = new TruckParser();
                List<TruckItem> truckItems = new List<TruckItem>();
                Dictionary<string, TruckItem> worked = new Dictionary<string, TruckItem>();
                try
                {
                    truckItems = parser.parseTruckExcelData(truckloc);
                    worked = parser.parseInv(storeloc);
                }
                catch (Exception ex)
                {
                    log.Debug("EXCEPTION: " + ex.ToString());
                    MessageBox.Show("Error: Could Not Parse Correctly");
                }
                List<TruckItem> final = new List<TruckItem>();
                List<string> nonmatch = new List<string>();
                foreach (TruckItem tr in truckItems)
                {
                    if (worked.ContainsKey(tr.itemcode))
                    {
                        tr.inStore = worked[tr.itemcode].inStore;
                        tr.fiveweek = worked[tr.itemcode].fiveweek;
                        tr.location = worked[tr.itemcode].location;
                        final.Add(tr);
                    }
                    else { nonmatch.Add(tr.Name); }
                }
                try
                {
                    ListToExcel(final, saveloc);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Pat doesn't know what he's doing, this didn't work");
                    MessageBox.Show(ex.ToString());
                }
            });
            Task Run_continue = Start.ContinueWith((antecedent) =>
            {
                LoadGif.Visibility = Visibility.Collapsed;
                prep_truck_BT.IsEnabled = true;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
