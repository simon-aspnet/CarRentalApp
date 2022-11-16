using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public static class Utils
    {
        public static string getHashedPassword(string password)
        {
            SHA256 sha = SHA256.Create();
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder stringBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string
            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static string getDefaultHashedPassword()
        {
            SHA256 sha = SHA256.Create();
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes("Password@123"));

            StringBuilder stringBuilder = new StringBuilder();

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string
            for (int i = 0; i < data.Length; i++)
            {
                stringBuilder.Append(data[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static bool CheckFormOpen(string formName)
        {
            // get a list of open forms as Form objects (of datatype Form) as they're inherited from Form
            var openForms = Application.OpenForms.Cast<Form>();
            // are any forms with the name <formName> open
            var isOpen = openForms.Any(q => q.Name == formName);
            return isOpen;
        }

    }
}
