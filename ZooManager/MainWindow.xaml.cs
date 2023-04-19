using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ZooManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();
            string conn = ConfigurationManager.ConnectionStrings["ZooManager.Properties.Settings.panjutorialDBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(conn);
            shooZoos();
            showAllAnimal();
        }
        private void showAllAnimal()
            
        {
            try
            {
                string query = "select * from Animal";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                using (sqlDataAdapter)
                {
                    DataTable animalTable = new DataTable();
                    sqlDataAdapter.Fill(animalTable);
                    allAnimal.DisplayMemberPath = "name";
                    allAnimal.SelectedValuePath = "Id";
                    allAnimal.ItemsSource = animalTable.DefaultView;
                }
            }catch(Exception e) {
              MessageBox.Show(e.Message);
            }
        }
        private void shooZoos()
        {
            try
            {
                string query = "select * from Zoo";
                SqlDataAdapter adapter = new SqlDataAdapter(query, sqlConnection);

                using (adapter)
                {
                    DataTable zooTable = new DataTable();
                    adapter.Fill(zooTable);
                    listZoos.DisplayMemberPath = "Location";
                    listZoos.SelectedValuePath = "Id";
                    listZoos.ItemsSource = zooTable.DefaultView;


                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
                
            }
        private void showSelectedAnimal()
        {
            try
            {
                string query = "select name from Animal where id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@AnimalId", allAnimal.SelectedValue);
                    DataTable animalTable = new DataTable();
                    adapter.Fill(animalTable);
                    MyTextBox.Text = animalTable.Rows[0]["name"].ToString();
                }


            }
            catch (Exception)
            {

                
            }
        }
        private void showSelectedZoo()
        {
            try
            {
                string query = "select location from zoo where id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                    DataTable zooTable = new DataTable();
                    adapter.Fill(zooTable);
                    MyTextBox.Text = zooTable.Rows[0]["location"].ToString();

                }
                             


            }catch (Exception ex) { 
            }
        }
        private void shooAssociated()
        {
            try
            {
                string query = "select * from Animal a inner join ZooAnimal za on a.Id = za.AnimalId " +
                    "where za.ZooId = @ZooId";
               SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);    

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);

                using (adapter)
                {
                    sqlCommand.Parameters.AddWithValue("@ZooId",listZoos.SelectedValue);
                    DataTable animalTable = new DataTable();
                    adapter.Fill(animalTable);
                    associated.DisplayMemberPath = "name";
                    associated.SelectedValuePath = "Id";
                    associated.ItemsSource = animalTable.DefaultView;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


        private void listZoos_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(listZoos.SelectedValue.ToString());
            shooAssociated();
            showSelectedZoo();
        }

    
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Zoo where id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.ExecuteScalar();
            }catch (Exception ex) {
                MessageBox.Show(ex.ToString());
            }
            finally { sqlConnection.Close();
                shooZoos();
            }
        }

        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "insert into Zoo values (@Location)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@Location", MyTextBox.Text);

                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                shooZoos();
            }

        }

        private void addAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string query = "insert into ZooAnimal values (@ZooId,@AnimalId)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", allAnimal.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);


                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                shooAssociated();
            }

        }

        private void deleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "delete from Animal where id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", allAnimal.SelectedValue);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
            finally
            {
                sqlConnection.Close();
                showAllAnimal();
            }
        }

        private void addAnimal_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string query = "insert into Animal values (@name)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@name", MyTextBox.Text);

                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                showAllAnimal();
            }
        }

        private void updateZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "update Zoo Set Location = @Location where id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand( query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Location", MyTextBox.Text);

                sqlCommand.ExecuteScalar();
            }
            catch (Exception)
            {


            }
            finally
            {
                sqlConnection.Close();
                shooZoos();
            }
        }

        private void updateAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "update Animal Set name = @name where id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@name", MyTextBox.Text);

                sqlCommand.ExecuteScalar();
            }
            catch (Exception)
            {


            }
            finally
            {
                sqlConnection.Close();
                showAllAnimal();
            }

        }

        private void allAnimal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showSelectedAnimal();
        }
    }
}
