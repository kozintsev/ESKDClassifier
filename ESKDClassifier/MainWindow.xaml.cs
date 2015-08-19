using System;
using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

using System.Data;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Collections.ObjectModel;

using Newtonsoft.Json;

namespace ESKDClassifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string pathFileXML = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\\ESKDClassifier\\ESKDClassifier.xml";
        public string pathFileJson = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\\ESKDClassifier\\ESKDClassifier.json";
        public string DirFromFiles = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\\ESKDClassifier\\Files\\";


        ESKDClass eskdClass;
        List<ESKDClass> Classifier;
        List<ESKDClass> classList;

        private void Serialization()
        {
            ////создаём файл сериализации
            //FileStream fsout = new FileStream(pathFileXML, FileMode.Create, FileAccess.Write);
            //XmlSerializer serializerout = new XmlSerializer(typeof(List<ESKDClass>), new Type[] { typeof(ESKDClass) });
            //serializerout.Serialize(fsout, Classifier);
            //fsout.Close();

            //string json = JsonConvert.SerializeObject(Classifier);

            File.WriteAllText(pathFileJson, JsonConvert.SerializeObject(Classifier));
            using (StreamWriter file = File.CreateText(pathFileJson))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, Classifier);
            }

        }

        //delegate void MyTaskList();
        //private MyTaskList myDelegateFunc;

        public MainWindow()
        {
            InitializeComponent();

            //myDelegateFunc = new MyTaskList(MyTask);

            Classifier = new List<ESKDClass>();
            classList = new List<ESKDClass>();
            classList.Clear();

           
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

            if (!File.Exists(pathFileJson))
            {
                
                return;
            }
            

            using (StreamReader file = File.OpenText(pathFileJson))
            {
                JsonSerializer serializer = new JsonSerializer();
                Classifier = (List<ESKDClass>)serializer.Deserialize(file, typeof(List<ESKDClass>));
            }


            
            ESKDTree.ItemsSource = Classifier;
            ESKDListView.ItemsSource = classList;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ESKDListView.ItemsSource);
            view.Filter = UserFilter;
            CollectionViewSource.GetDefaultView(ESKDListView.ItemsSource).Refresh();

        }

        private TreeViewItem selectedItem = null;

        private bool UserFilter(object item)
        {
            //751112
            //|| ((item as ESKDClass).CodESKD.IndexOf(FindToList.Text, StringComparison.OrdinalIgnoreCase) >= 0)
            if (String.IsNullOrEmpty(FindToList.Text))
                return true;
            else
                return ((item as ESKDClass).CodESKD.IndexOf(FindToList.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void AddClass_Click(object sender, RoutedEventArgs e)
        {
            AddClassifier addClass = new AddClassifier(this);            
            addClass.ShowDialog();

            eskdClass = addClass.GetClassifier();
            if (addClass.Cancel)
                return;

            if (selectedItem == null)
            {
                Classifier.Add(eskdClass);
            }
            else
            {
                ESKDClass parentclass = selectedItem.DataContext as ESKDClass;
                parentclass.eskdViews.Add(eskdClass);
            }
            //ESKDTree.Items.Refresh();
            if (selectedItem != null)
                selectedItem.Items.Refresh();
            //selectedItem.IsExpanded = true;

            Serialization();

            
        }

        private void DelClass_Click(object sender, RoutedEventArgs e)
        {

        }


        //void MyTask()
        //{
        //    for (int i = 0; i < childClasses.Count; i++)
        //    {
        //        childClasses[i].FullPathPictures = DirFromFiles + childClasses[i].PathPicture;
        //        classList.Add(childClasses[i]);
        //    }
        //    ESKDListView.Items.Refresh();  
        //}

        //void MyTask()
        //{
        //    ESKDListView.Items.Refresh();
        //}

        //void MyTask2()
        //{
        //    ESKDListView.Dispatcher.BeginInvoke(myDelegateFunc);
        //}



        private void ESKDTree_Selected_Item(object sender, RoutedEventArgs e)
        {
            var item = e.OriginalSource as TreeViewItem;
            if (item != null)
            {
                classList.Clear();
                ESKDClass selectedClass = item.DataContext as ESKDClass;
                ObservableCollection<ESKDClass> childClasses = selectedClass.eskdViews;

                if (selectedClass != null)
                    txtBxCode.Text = selectedClass.CodESKD;

                //Task task = new Task(MyTask2);

                for (int i = 0; i < childClasses.Count; i++)
                {
                    childClasses[i].FullPathPictures = DirFromFiles + childClasses[i].PathPicture;
                    classList.Add(childClasses[i]);
                }

                //task.Start();

                //ESKDListView.Dispatcher.BeginInvoke(myDelegateFunc);
                ESKDListView.Items.Refresh();
                CollectionViewSource.GetDefaultView(ESKDListView.ItemsSource).Refresh();
                selectedItem = item;
            }
        }

        private void ESKDClassifier_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Serialization();
        }


        private void ESKDListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView lv = e.OriginalSource as ListView;
            if (lv != null)
            {
                ESKDClass lvi = lv.SelectedItem as ESKDClass;
                if (lvi != null)
                    txtBxCode.Text = lvi.CodESKD;
            }
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
