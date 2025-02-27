using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace AutoTechCenterApp
{
    public partial class ChangeStatusWindow : Window
    {
        private readonly string connectionString;

        public ChangeStatusWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["AutoTechCenter"].ConnectionString;
            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT OrderID FROM Orders";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    OrderComboBox.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке заказов: " + ex.Message);
            }
        }

        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Orders SET Status = @Status WHERE OrderID = @OrderID";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Status", NewStatusTextBox.Text);
                    command.Parameters.AddWithValue("@OrderID", OrderComboBox.SelectedValue);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Статус заказа успешно изменён!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при изменении статуса: " + ex.Message);
            }
        }
    }
}