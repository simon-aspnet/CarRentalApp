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
    public partial class ManageRentalRecords : Form
    {
        private readonly CarRentalEntities _carRentalEntities;

        public ManageRentalRecords()
        {
            InitializeComponent();
            _carRentalEntities = new CarRentalEntities();
        }

        public void PopulateGrid()
        {
            var rentals = _carRentalEntities.CarRentalRecords
                .Select(d => new
                {
                    CustomerName = d.CustomerName,
                    DateRented = d.DateRented,
                    DateReturned = d.DateReturned,
                    d.Id,
                    Cost = d.Cost,
                    Car = d.TypesOfCar.Make + " " + d.TypesOfCar.Model
                })
                .ToList();

            dgvRentalRecords.DataSource = rentals;

            dgvRentalRecords.Columns[0].HeaderText = "Customer";
            dgvRentalRecords.Columns[1].HeaderText = "Date Rented";
            dgvRentalRecords.Columns[2].HeaderText = "Date Returned";
            dgvRentalRecords.Columns[3].Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!Utils.CheckFormOpen("AddEditRentalRecord"))
            {
                AddEditRentalRecord addRentalRecord = new AddEditRentalRecord(this)
                {
                    MdiParent = this.MdiParent
                };
                addRentalRecord.Show(); 
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvRentalRecords.SelectedRows.Count > 0)
            {
                if (!Utils.CheckFormOpen("AddEditRentalRecord"))
                {
                    // get Id of selected row
                    int id = (int)dgvRentalRecords.SelectedRows[0].Cells["Id"].Value;

                    // query database for record
                    CarRentalRecord rental = _carRentalEntities.CarRentalRecords.FirstOrDefault(q => q.Id == id);

                    // launch AddEditRentalRecord with data
                    AddEditRentalRecord addEditRentalRecord = new AddEditRentalRecord(rental, this);
                    addEditRentalRecord.MdiParent = this.MdiParent;
                    addEditRentalRecord.Show(); 
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = (int)dgvRentalRecords.SelectedRows[0].Cells["Id"].Value;

            // query database for record
            CarRentalRecord rental = _carRentalEntities.CarRentalRecords.FirstOrDefault(q => q.Id == id);

            if (rental != null)
            {
                // remove car
                _carRentalEntities.CarRentalRecords.Remove(rental);
                _carRentalEntities.SaveChanges();

                PopulateGrid();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void ManageRentalRecords_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

    }
}
