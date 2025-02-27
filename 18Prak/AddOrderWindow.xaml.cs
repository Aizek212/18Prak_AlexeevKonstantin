using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace AutoTechCenterApp
{
    public partial class AddOrderWindow : Window
    {
        private readonly string connectionString;

        public AddOrderWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["AutoTechCenter"].ConnectionString;
            LoadClients();
            LoadCars();
        }

        private void LoadClients()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT ClientID, LastName FROM Clients";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    ClientComboBox.ItemsSource = dataTable.DefaultView;
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
                    string query = "SELECT CarID, LicensePlate FROM Cars";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    CarComboBox.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке автомобилей: " + ex.Message);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Orders (ClientID, CarID, OrderDate, CompletionDate, Status) VALUES (@ClientID, @CarID, @OrderDate, @CompletionDate, @Status)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@ClientID", ClientComboBox.SelectedValue);
                    command.Parameters.AddWithValue("@CarID", CarComboBox.SelectedValue);
                    command.Parameters.AddWithValue("@OrderDate", OrderDatePicker.SelectedDate);
                    command.Parameters.AddWithValue("@CompletionDate", CompletionDatePicker.SelectedDate);
                    command.Parameters.AddWithValue("@Status", StatusTextBox.Text);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Заказ успешно добавлен!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении заказа: " + ex.Message);
            }
        }
    }
}