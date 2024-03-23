using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Applify.Models;
using SQLite;
using System.IO;

namespace Applify
{
    public partial class MainPage : ContentPage
    {
        private SQLiteAsyncConnection conn;
        public MainPage()
        {
            InitializeComponent();
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var databasePath = Path.Combine(documentPath, "Contacts.db");
            conn = new SQLiteAsyncConnection(databasePath);

            conn.CreateTableAsync<Contacts>();
            ReadData();
        }
        private ObservableCollection<Contacts> cList;
        public ObservableCollection<Contacts> CList
        {
            get { return cList; }
            set { cList = value; }
        }
        private void Save(object sender, EventArgs e)
        {
            conn.InsertAsync(new Contacts() { Name = Name.Text, Address = Address.Text });

            ReadData();
        }
        public void ReadData()
        {

            var list = conn.Table<Contacts>().ToListAsync().Result;

            CList = new ObservableCollection<Contacts>(list);
            ContactList.ItemsSource = CList;
        }
        private void ListItem_Delete(object sender, EventArgs e)
        {
            var mi = sender as MenuItem;
            var item = mi.CommandParameter as Contacts;
            conn.DeleteAsync(item);
            ReadData();
        }
        private void Update(object sender, EventArgs e)
        {
            var mi = sender as MenuItem;
            var item = mi.CommandParameter as Contacts;
            item.Name = "Martin";
            item.Address = "New York";
            conn.UpdateAsync(item);
            ReadData();
        }
    }
}
