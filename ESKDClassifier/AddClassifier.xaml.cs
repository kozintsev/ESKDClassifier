using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

using System.IO;

namespace ESKDClassifier
{
    /// <summary>
    /// Interaction logic for AddClassifier.xaml
    /// </summary>
    public partial class AddClassifier : Window
    {
        public AddClassifier(Window win)
        {
            InitializeComponent();
            this.Owner = win;
        }

        private string filename = string.Empty;

        private ESKDClass classifier;

        public ESKDClass GetClassifier()
        {
            return classifier;
        }

        public bool Cancel = true;
        public string ErrorMsg = string.Empty;

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Cancel = true;
            this.Close();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            // Блок проперок

            classifier = new ESKDClass();
            classifier.CodESKD = this.CodeESKD.Text;
            classifier.Description = this.DescESKD.Text;

            //переименовать файл 
            // скопировать в ESKDClassifier\Files\
            string newshortname = Guid.NewGuid().ToString();
            string newfilename = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ESKDClassifier\\Files\\" + newshortname;
            classifier.PathPicture = newshortname;

            try
            {                
                File.Copy(filename, newfilename);
            }
            catch(Exception ex)
            {
                ErrorMsg = ex.Message;
                Cancel = true;
                return;
            }                     
                     
            Cancel = false;
            this.Close();
        }


        private void BtnAddPicture_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".png";
            dlg.Filter = "PNG Files (*.png)|*.png";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                filename = dlg.FileName;
                PictureESKD.Text = filename;
            }
        }
    }
}
