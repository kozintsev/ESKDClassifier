using System;
using System.Windows;

using System.IO;

namespace ESKDClassifier
{
    /// <summary>
    /// Interaction logic for AddClassifier.xaml
    /// </summary>
    public partial class AddClassifier
    {
        public AddClassifier(Window win)
        {
            InitializeComponent();
            Owner = win;
        }

        private string _filename = string.Empty;

        private EskdClass _classifier;

        public EskdClass GetClassifier()
        {
            return _classifier;
        }

        public bool Cancel = true;
        public string ErrorMsg = string.Empty;

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Cancel = true;
            Close();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            // Блок проперок

            _classifier = new EskdClass
            {
                CodEskd = CodeESKD.Text,
                Description = DescESKD.Text
            };

            //переименовать файл 
            // скопировать в ESKDClassifier\Files\
            var newShortName = Guid.NewGuid().ToString();
            var newFileName = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\ESKDClassifier\\Files\\" + newShortName;
            _classifier.PathPicture = newShortName;

            try
            {                
                File.Copy(_filename, newFileName);
            }
            catch(Exception ex)
            {
                ErrorMsg = ex.Message;
                Cancel = true;
                return;
            }                     
                     
            Cancel = false;
            Close();
        }


        private void BtnAddPicture_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".png",
                Filter = "PNG Files (*.png)|*.png"
            };
            var result = dlg.ShowDialog();
            if (result != true) return;
            _filename = dlg.FileName;
            PictureESKD.Text = _filename;
        }
    }
}
