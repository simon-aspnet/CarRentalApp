using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ManageVehicleListing : Form
    {
        private readonly CarRentalEntities _carRentalEntities;
        public ManageVehicleListing()
        {
            InitializeComponent();
            _carRentalEntities = new CarRentalEntities();
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            // This is equivalent to "select * from TypesOfCars
            //var cars = _carRentalEntities.TypesOfCars.ToList();

            // The d below in the lamda expression is like the alias you have in an SQL expresion.  It's the same as
            // select d.Id as CarId,d.Name as CarName from TypesOfCars d
            // We are creating a new object on the fly "new {ID=d.Id,CarName=d.Name}" CarId and CarName are also aliases
            // When making a LINQ query it is easiest/best to do a ToList()
            //var cars = _carRentalEntities.TypesOfCars
            //    .Select(d => new { CarId = d.Id, CarName = d.Name })
            //    .ToList();

            //var cars = _carRentalEntities.TypesOfCars
            //    .Select(d => new { Make = d.Make, Model = d.Model, VIN = d.VIN, Year = d.Year, LicensePlate = d.LicensePlateNumber, d.Id })
            //    .ToList();

            //dgvVehicleList.DataSource = cars;

            //dgvVehicleList.Columns[4].HeaderText = "License Plate";
            //dgvVehicleList.Columns[5].Visible = false;

            PopulateGrid();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!Utils.CheckFormOpen("AddEditVehicle"))
            {
                AddEditVehicle addEditVehicle = new AddEditVehicle(this);
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show(); 
            }

        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvVehicleList.SelectedRows.Count > 0)
            {
                if(!Utils.CheckFormOpen("AddEditVehicle"))
                {
                    // get Id of selected row
                    int id = (int)dgvVehicleList.SelectedRows[0].Cells["Id"].Value;

                    // query database for record
                    TypesOfCar car = _carRentalEntities.TypesOfCars.FirstOrDefault(q => q.Id == id);

                    // launch addEditVehicle with data
                    AddEditVehicle addEditVehicle = new AddEditVehicle(car, this);
                    addEditVehicle.MdiParent = this.MdiParent;
                    addEditVehicle.Show();


                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = (int)dgvVehicleList.SelectedRows[0].Cells["Id"].Value;

            // query database for record
            TypesOfCar car = _carRentalEntities.TypesOfCars.FirstOrDefault(q => q.Id == id);

            if (car != null)
            {
                DialogResult dr = MessageBox.Show("Are you sure you want to delete this record?",
                    "Delete",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    // remove car
                    try
                    {
                        _carRentalEntities.TypesOfCars.Remove(car);
                        _carRentalEntities.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,"Error");
                    }
                }

                PopulateGrid();
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            var cars = _carRentalEntities.TypesOfCars
                .Select(d => new
                {
                    Make = d.Make,
                    Model = d.Model,
                    VIN = d.VIN,
                    Year = d.Year,
                    LicensePlate = d.LicensePlateNumber,
                    d.Id
                })
                .ToList();

            dgvVehicleList.DataSource = cars;

            dgvVehicleList.Columns[4].HeaderText = "License Plate";
            dgvVehicleList.Columns[5].Visible = false;
        }

    }
}
