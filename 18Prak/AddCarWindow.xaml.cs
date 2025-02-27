using System;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace AutoTechCenterApp
{
    public partial class AddCarWindow : Window
    {
        private readonly string connectionString;

        public AddCarWindow()
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
                    string query = "INSERT INTO Cars (Brand, Model, Year, LicensePlate) VALUES (@Brand, @Model, @Year, @LicensePlate)";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Brand", BrandTextBox.Text);
                    command.Parameters.AddWithValue("@Model", ModelTextBox.Text);
                    command.Parameters.AddWithValue("@Year", int.Parse(YearTextBox.Text));
                    command.Parameters.AddWithValue("@LicensePlate", LicensePlateTextBox.Text);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Автомобиль успешно добавлен!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении автомобиля: " + ex.Message);
            }
        }
    }
}