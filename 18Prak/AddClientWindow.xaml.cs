using System;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace AutoTechCenterApp
{
    public partial class AddClientWindow : Window
    {
        private readonly string connectionString;

        public AddClientWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["AutoTechCenter"].ConnectionString;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Clients (FirstName, LastName, Phone, Email, Address) VALUES (@FirstName, @LastName, @Phone, @Email, @Address)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FirstName", FirstNameTextBox.Text);
                    command.Parameters.AddWithValue("@LastName", LastNameTextBox.Text);
                    command.Parameters.AddWithValue("@Phone", PhoneTextBox.Text);
                    command.Parameters.AddWithValue("@Email", EmailTextBox.Text);
                    command.Parameters.AddWithValue("@Address", AddressTextBox.Text);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Клиент успешно добавлен!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении клиента: " + ex.Message);
            }
        }
    }
}