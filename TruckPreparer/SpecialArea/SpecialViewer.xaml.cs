﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TruckPreparer.SpecialArea
{
    /// <summary>
    /// Interaction logic for SpecialViewer.xaml
    /// </summary>
    public partial class SpecialViewer : UserControl
    {
        Window parentwindow = new Window();
        public AreasList source;
        private ObservableCollection<AreasJSON> ObservableSource = new ObservableCollection<AreasJSON>();
        public SpecialViewer()
        {
            InitializeComponent();
            this.Loaded += SpecialViewer_Loaded;
            Special_LB.ItemsSource = ObservableSource;
          
        }

        private void SpecialViewer_Loaded(object sender, RoutedEventArgs e)
        {
            Window parentwindow = Window.GetWindow(this);
            parentwindow.Closed += Parentwindow_Closed;
        }

        private void Parentwindow_Closed(object sender, EventArgs e)
        {
            source.Save();
        }

        public void Reload(AreasList source)
        {
            this.source = source;
            if(source.Areas == null)
            {
                source.Areas = new List<AreasJSON>();
                foreach(AreasJSON item in source.Areas.ToList())
                {
                    item.Items.Items = new List<LTSItem>();
                }
            }
            foreach (AreasJSON item in source.Areas.ToList())
            {
                if(DateTime.Now > item.Until && item.CheckedAlready == false)
                {
                    MessageBoxResult mbr = MessageBox.Show(item.Name + " has ended. Would you like to remove this grouping?", "Confirmation" ,MessageBoxButton.YesNo);
                    if(mbr == MessageBoxResult.Yes)
                    {
                        source.Areas.Remove(item);
                    }
                    else { item.CheckedAlready = true; }
                }
            }
            foreach (AreasJSON item in source.Areas)
            {
                ObservableSource.Add(item);
            }
            source.Save();
        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            Window temp = new Window();
            AddArea add = new AddArea();
            temp.Content = add;
            temp.Title = "Area";
            temp.Show();
            
            add.AddItem += Add_AddItem;
        }

        private void Add_AddItem(object sender, AddAreaEventArgs args)
        {
            ((sender as AddArea).Parent as Window).Close();
            ObservableSource.Add(new AreasJSON { Name = args.Name, Items = new LTS { Items = LTSViewer.ParseToHighlyRated(args.Filelocation) },Start = args.Start, Until = args.End, CheckedAlready = false });
            source.Areas.Add(new AreasJSON { Name = args.Name, Items = new LTS { Items = LTSViewer.ParseToHighlyRated(args.Filelocation) }, Start = args.Start, Until = args.End, CheckedAlready = false });
        }

        private void Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            LTS its = (Special_LB.SelectedItem as AreasJSON).Items;
            Window wind = new Window();
            LTSViewer ltsv = new LTSViewer();
            ltsv.Reload(its);
            wind.Content = ltsv;
            wind.Show();
        }

        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            var sel = Special_LB.SelectedItem as AreasJSON;
            ObservableSource.Remove(sel);
            source.Areas.Remove(sel);
            source.Save();
        }
    }
}
