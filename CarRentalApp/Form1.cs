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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            dtpReturnDate.Value = DateTime.Today;
            dtpRentalDate.Value = DateTime.Today;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            string name=txtCustomerName.Text;
            string car=cbCar.Text;
            string rentalDate=dtpRentalDate.Text;
            string returnDate=dtpReturnDate.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(car))
            {
                MessageBox.Show("Please fill in the form properly","Error");
                return;
            }

            if (dtpRentalDate.Value > dtpReturnDate.Value)
            {
                MessageBox.Show("Return Date can't be before Rental Date","Error");
                return ;
            }

            if (string.IsNullOrWhiteSpace(txtCost.Text)||IsNumeric(txtCost.Text) == false)
            {
                MessageBox.Show("Enter a proper Cost","Error");
                return;
            }

            double cost = Convert.ToDouble(txtCost.Text);

            if (cost < 0)
            {
                MessageBox.Show("No negative costs","Error");
                    return;
            }

            MessageBox.Show($"Thank you for Renting a {car}{Environment.NewLine}" +
                $"from {rentalDate}{Environment.NewLine}" +
                $"to {returnDate}{Environment.NewLine}" +
                $"at a cost of {cost}"
                ,$"{name}");
        }

        public bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?(\d*\.)?\d+$");
        }
    }


}
