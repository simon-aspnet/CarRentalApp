using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class AddRentalRecord : Form
    {
        private readonly CarRentalEntities _carRentalEntities;
        public AddRentalRecord()
        {
            InitializeComponent();

            _carRentalEntities = new CarRentalEntities();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtCustomerName.Text;
                string car = cbCar.Text;
                string rentalDate = dtpRentalDate.Text;
                string returnDate = dtpReturnDate.Text;
                var isValid = true;
                double cost = 0d;

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(car))
                {
                    MessageBox.Show("Please fill in the form properly", "Error");
                    isValid = false;
                }

                if (dtpRentalDate.Value > dtpReturnDate.Value)
                {
                    MessageBox.Show("Return Date can't be before Rental Date", "Error");
                    isValid = false;
                }

                if (string.IsNullOrWhiteSpace(txtCost.Text) || IsNumeric(txtCost.Text) == false)
                {
                    MessageBox.Show("Enter a proper Cost", "Error");
                    isValid = false;
                }
                else
                {
                    cost = Convert.ToDouble(txtCost.Text);

                    if (cost < 0)
                    {
                        MessageBox.Show("No negative costs", "Error");
                        isValid = false;
                    }
                }


                if (isValid == true)
                {
                    var rentalRecord = new CarRentalRecord();
                    rentalRecord.CustomerName = name;
                    rentalRecord.DateRented = dtpRentalDate.Value;
                    rentalRecord.DateReturned = dtpReturnDate.Value;
                    rentalRecord.Cost = (decimal)cost;
                    rentalRecord.TypeOfCarId = (int)cbCar.SelectedValue;
                    _carRentalEntities.CarRentalRecords.Add(rentalRecord);
                    _carRentalEntities.SaveChanges();

                    MessageBox.Show($"Thank you for Renting a {car}{Environment.NewLine}" +
                        $"from {rentalDate}{Environment.NewLine}" +
                        $"to {returnDate}{Environment.NewLine}" +
                        $"at a cost of {cost}"
                        , $"{name}");
                }

            }
            catch (Exception ex)
            {
                // for any errors not caught above
                MessageBox.Show(ex.Message);
                // comment out throw to allow th program to continue running
                //throw;
            }
        }

        public bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?(\d*\.)?\d+$");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var cars=_carRentalEntities.TypesOfCars.ToList();
            cbCar.DisplayMember = "Name";
            cbCar.ValueMember = "Id";
            cbCar.DataSource = cars;

            dtpReturnDate.Value = DateTime.Today;
            dtpRentalDate.Value = DateTime.Today;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }


}
