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
    public partial class AddEditVehicle : Form
    {
        private readonly bool _isEditMode;
        private readonly CarRentalEntities _carRentalEntities;

        public AddEditVehicle()
        {
            InitializeComponent();
            _carRentalEntities = new CarRentalEntities();
            _isEditMode = false;
            lblCar.Text = "Add Car";
        }

        public AddEditVehicle(TypesOfCar carToEdit)
        {
            InitializeComponent();
            _carRentalEntities = new CarRentalEntities();
            _isEditMode = true;
            lblCar.Text = "Edit Car";
            PopuateFields(carToEdit);
        }

        private void PopuateFields(TypesOfCar car)
        {
            txtMake.Text= car.Make;
            txtModel.Text=car.Model;
            txtVIN.Text=car.VIN;
            txtYear.Text = car.Year.ToString();
            txtLicense.Text = car.LicensePlateNumber;
            lblId.Text = car.Id.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_isEditMode)
                {
                    int id = int.Parse(lblId.Text);
                    var car = _carRentalEntities.TypesOfCars.FirstOrDefault(x => x.Id == id);
                    car.Make = txtMake.Text;
                    car.Model = txtModel.Text;
                    car.VIN = txtVIN.Text;
                    if (IsNumeric(txtYear.Text))
                    {
                        car.Year = int.Parse(txtYear.Text);
                    }
                    car.LicensePlateNumber = txtLicense.Text;

                    _carRentalEntities.SaveChanges();

                    MessageBox.Show("Car Modified.\n\rPlease Refresh Grid.");
                }
                else
                {
                    TypesOfCar car = new TypesOfCar();
                    car.Make = txtMake.Text;
                    car.Model = txtModel.Text;
                    car.VIN = txtVIN.Text;
                    if (IsNumeric(txtYear.Text))
                    {
                        car.Year = int.Parse(txtYear.Text);
                    }
                    car.LicensePlateNumber = txtLicense.Text;

                    _carRentalEntities.TypesOfCars.Add(car);
                    _carRentalEntities.SaveChanges();

                    MessageBox.Show("Car Added.\n\rPlease Refresh Grid.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Error");
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?(\d*\.)?\d+$");
        }


    }
}
