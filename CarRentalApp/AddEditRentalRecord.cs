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
    public partial class AddEditRentalRecord : Form
    {
        private readonly CarRentalEntities _carRentalEntities;
        private readonly bool _isEditMode;
        private readonly ManageRentalRecords _manageRentalRecords;

        public AddEditRentalRecord(ManageRentalRecords manageRentalRecords = null)
        {
            InitializeComponent();

            _carRentalEntities = new CarRentalEntities();
            InitializeValues();
            _isEditMode = false;
            lblTitle.Text = "Add New Rental Record";
            this.Text = "Add New Rental Record";
            _manageRentalRecords = manageRentalRecords;
        }

        public AddEditRentalRecord(CarRentalRecord carRental, ManageRentalRecords manageRentalRecords = null)
        {
            InitializeComponent();

            _carRentalEntities = new CarRentalEntities();
            InitializeValues();
            _isEditMode = true;
            lblTitle.Text = "Modify Rental Record";
            this.Text = "Modify Rental Record";
            _manageRentalRecords = manageRentalRecords;
            PopulateFields(carRental);
        }

        private void PopulateFields(CarRentalRecord carRental)
        {
            txtCustomerName.Text = carRental.CustomerName;
            dtpRentalDate.Value = carRental.DateRented.Value;
            dtpReturnDate.Value = carRental.DateReturned.Value;
            txtCost.Text = carRental.Cost.ToString();
            lblId.Text = carRental.Id.ToString();

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
                    if (_isEditMode)
                    {
                        int id = int.Parse(lblId.Text);
                        rentalRecord = _carRentalEntities.CarRentalRecords.FirstOrDefault(x => x.Id == id);
                    }

                    rentalRecord.CustomerName = name;
                    rentalRecord.DateRented = dtpRentalDate.Value;
                    rentalRecord.DateReturned = dtpReturnDate.Value;
                    rentalRecord.Cost = (decimal)cost;
                    rentalRecord.TypeOfCarId = (int)cbCar.SelectedValue;

                    if (!_isEditMode)
                        _carRentalEntities.CarRentalRecords.Add(rentalRecord);

                    _carRentalEntities.SaveChanges();
                    if (_manageRentalRecords != null)
                        _manageRentalRecords.PopulateGrid();

                    if (_isEditMode)
                        MessageBox.Show("Rental Record Modified.\n\rPlease Refresh Grid.");
                    else
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

            Close();
        }

        public bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?(\d*\.)?\d+$");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void InitializeValues()
        {
            var cars = _carRentalEntities.TypesOfCars
                .Select(d => new { Id = d.Id, Name = d.Make + " " + d.Model })
                .ToList();

            cbCar.DisplayMember = "Name";
            cbCar.ValueMember = "Id";
            cbCar.DataSource = cars;

            dtpReturnDate.Value = DateTime.Today;
            dtpRentalDate.Value = DateTime.Today;
        }
    }


}
