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
    public partial class ResetPassword : Form
    {
        private readonly CarRentalEntities _db;
        private readonly User _user;

        public ResetPassword(User user)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _user = user;
        }

        private void btnChange_Click(object sender, EventArgs e)
        {
            try
            {
                string password = txtPassword.Text.Trim();
                string confirm = txtConfirm.Text.Trim();

                if (password != confirm)
                {
                    MessageBox.Show("Password do not match.  Please try again.");
                }
                else
                {
                    var user = _db.Users.FirstOrDefault(x => x.Id == _user.Id);

                    user.password = Utils.getHashedPassword(password);
                    _db.SaveChanges();

                    MessageBox.Show("Password changed","Success");

                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error.\n\rPlease try again.","Problem");
            }

        }

    }
}
