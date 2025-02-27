using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Windows.Controls;

namespace AutoTechCenterApp
{
    public partial class MainWindow : Window
    {
        private readonly string connectionString;
        private DataTable ordersTable;
        private DataTable clientsTable;
        private DataTable carsTable;

        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["AutoTechCenter"].ConnectionString;
            LoadData();
        }

        private void LoadData()
        {
            LoadOrders();
            LoadClients();
            LoadCars();
        }

        private void LoadOrders()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Orders";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    ordersTable = new DataTable();
                    adapter.Fill(ordersTable);
                    OrdersDataGrid.ItemsSource = ordersTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке заказов: " + ex.Message);
            }
        }

        private void LoadClients()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Clients";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    clientsTable = new DataTable();
                    adapter.Fill(clientsTable);
                    ClientsDataGrid.ItemsSource = clientsTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке клиентов: " + ex.Message);
            }
        }

        private void LoadCars()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Cars";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    carsTable = new DataTable();
                    adapter.Fill(carsTable);
                    CarsDataGrid.ItemsSource = carsTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке автомобилей: " + ex.Message);
            }
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ordersTable == null) return;

            var view = ordersTable.DefaultView;
            switch (SortComboBox.SelectedIndex)
            {
                case 0:
                    view.Sort = "OrderDate ASC"; 
                    break;
                case 1:
                    view.Sort = "OrderDate DESC"; 
                    break;
                case 2:
                    view.Sort = "Status ASC"; 
                    break;
            }
        }

        private void FilterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ordersTable == null) return;

            string filterText = FilterTextBox.Text.ToLower();
            ordersTable.DefaultView.RowFilter = $"Status LIKE '%{filterText}%'"; 
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ordersTable == null) return;

            string searchText = SearchTextBox.Text.ToLower();
            ordersTable.DefaultView.RowFilter = $"ClientID IN (SELECT ClientID FROM Clients WHERE LastName LIKE '%{searchText}%')"; 
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadData(); 
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            AddOrderWindow addOrderWindow = new AddOrderWindow();
            addOrderWindow.ShowDialog();
            LoadOrders();
        }

        private void ChangeStatusButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusWindow changeStatusWindow = new ChangeStatusWindow();
            changeStatusWindow.ShowDialog();
            LoadOrders();
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow addClientWindow = new AddClientWindow();
            addClientWindow.ShowDialog();
            LoadClients();
        }

        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            AddCarWindow addCarWindow = new AddCarWindow();
            addCarWindow.ShowDialog();
            LoadCars();
        }
    }
}