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
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void addRentalRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEditRentalRecord addRentalRecord = new AddEditRentalRecord();
            addRentalRecord.MdiParent = this;
            addRentalRecord.Show();
        }

        private void manageVehicleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // get a list of open forms as Form objects (of datatype Form) as they're inherited from Form
            var openForms = Application.OpenForms.Cast<Form>();
            // are any forms with the name "ManageVehicleListing" open
            var isOpen = openForms.Any(q => q.Name == "ManageVehicleListing");

            if (!isOpen)
            {
                ManageVehicleListing manageVehicleListing = new ManageVehicleListing();
                manageVehicleListing.MdiParent = this;
                manageVehicleListing.Show();
            }
        }

        private void viewArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // get a list of open forms as Form objects (of datatype Form) as they're inherited from Form
            var openForms = Application.OpenForms.Cast<Form>();
            // are any forms with the name "ManageVehicleListing" open
            var isOpen = openForms.Any(q => q.Name == "ManageRentalRecords");

            if (!isOpen)
            {
                ManageRentalRecords manageRentalRecords = new ManageRentalRecords();
                manageRentalRecords.MdiParent = this;
                manageRentalRecords.Show();

            }
        }
    }
}
