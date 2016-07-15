using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.IO;
using Newtonsoft.Json;

namespace ESKDClassifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly string _pathFileJson = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\\ESKDClassifier\\ESKDClassifier.json";
        private readonly string _dirFromFiles = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\\ESKDClassifier\\Files\\";
        private readonly List<EskdClass> _classifier;
        private readonly List<EskdClass> _classList;

        private EskdClass _eskdClass;
        private TreeViewItem _selectedItem;

        public MainWindow()
        {
            InitializeComponent();
            _classifier = new List<EskdClass>();
            _classList = new List<EskdClass>();
            _classList.Clear();
           
            //ESKDClass root = new ESKDClass();
            //root.CodESKD = "42";
            //root.Description = "устройства и системы контроля и регулирования парамметрами технолоогического процесса";

            //ESKDClass child = new ESKDClass();
            //child.CodESKD = "421111";
            //child.Description = "устройства и системы контроля и регулирования параметров технологических процессов электрические, преобразователи пневмоэлектрические, аналоговые, постоянного тока ";

            //root.eskdViews.Add(child);

            //Classifier.Add(root);
            //Classifier.Add(new ESKDClass() { CodESKD = "75", Description = "детали-тела вращения" });
            //Classifier.Add(new ESKDClass() { CodESKD = "74", Description = "детали-не тела вращения" });
           
            ////создаём файл сериализации
            //FileStream fsout = new FileStream(pathFileXML, FileMode.Create, FileAccess.Write);
            //XmlSerializer serializerout = new XmlSerializer(typeof(List<ESKDClass>), new Type[] { typeof(ESKDClass) });
            //serializerout.Serialize(fsout, Classifier);
            //fsout.Close();

            //загрузка данных
            //FileStream fsin = new FileStream(pathFileXML, FileMode.Open, FileAccess.Read);
            //XmlSerializer serializerin = new XmlSerializer(typeof(List<ESKDClass>), new Type[] { typeof(ESKDClass) });
            //Classifier = (List<ESKDClass>)serializerin.Deserialize(fsin);
            
            //fsin.Close();

            if (!File.Exists(_pathFileJson))
                return;
            
            using (var file = File.OpenText(_pathFileJson))
            {
                var serializer = new JsonSerializer();
                _classifier = (List<EskdClass>)serializer.Deserialize(file, typeof(List<EskdClass>));
            }

            ESKDTree.ItemsSource = _classifier;
            ESKDListView.ItemsSource = _classList;

            var view = (CollectionView)CollectionViewSource.GetDefaultView(ESKDListView.ItemsSource);
            view.Filter = UserFilter;
            CollectionViewSource.GetDefaultView(ESKDListView.ItemsSource).Refresh();
        }

        private void Serialization()
        {
            File.WriteAllText(_pathFileJson, JsonConvert.SerializeObject(_classifier));
            using (var file = File.CreateText(_pathFileJson))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, _classifier);
            }
        }

        private bool UserFilter(object item)
        {
            //751112
            //|| ((item as ESKDClass).CodESKD.IndexOf(FindToList.Text, StringComparison.OrdinalIgnoreCase) >= 0)
            if (string.IsNullOrEmpty(FindToList.Text))
                return true;
            var eskdClass = item as EskdClass;
            return eskdClass != null && (eskdClass.CodEskd.IndexOf(FindToList.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void AddClass_Click(object sender, RoutedEventArgs e)
        {
            var addClass = new AddClassifier(this);            
            addClass.ShowDialog();

            _eskdClass = addClass.GetClassifier();
            if (addClass.Cancel)
                return;

            if (_selectedItem == null)
            {
                _classifier.Add(_eskdClass);
            }
            else
            {
                var parentclass = _selectedItem.DataContext as EskdClass;
                if (parentclass != null) 
                    parentclass.EskdViews.Add(_eskdClass);
            }
            if (_selectedItem != null)
                _selectedItem.Items.Refresh();
            Serialization();
        }

        private void DelClass_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ESKDTree_Selected_Item(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as TreeViewItem;
            if (item == null) return;
            _classList.Clear();
            var selectedClass = item.DataContext as EskdClass;
            if (selectedClass != null)
            {
                var childClasses = selectedClass.EskdViews;
                txtBxCode.Text = selectedClass.CodEskd;
                foreach (var eskdClass in childClasses)
                {
                    eskdClass.FullPathPictures = _dirFromFiles + eskdClass.PathPicture;
                    _classList.Add(eskdClass);
                }
            }
            ESKDListView.Items.Refresh();
            CollectionViewSource.GetDefaultView(ESKDListView.ItemsSource).Refresh();
            _selectedItem = item;
        }

        private void ESKDClassifier_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Serialization();
        }


        private void ESKDListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lv = e.OriginalSource as ListView;
            if (lv == null) return;
            var lvi = lv.SelectedItem as EskdClass;
            if (lvi != null)
                txtBxCode.Text = lvi.CodEskd;
        }

        private void FindTree_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void FindToList_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ESKDListView.ItemsSource).Refresh();
        }

    }
}
