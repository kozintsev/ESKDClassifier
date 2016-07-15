using System;
using System.Collections.ObjectModel;

namespace ESKDClassifier
    //Класс:
    //42 - устройства и системы контроля и регулирования парамметрами технолоогического процесса
    //75 - детали-тела вращения
    //74 - детали-не тела вращения
{
    [Serializable]
    public class EskdClass
    {
        public EskdClass() 
        {
            EskdViews = new ObservableCollection<EskdClass>();
        }

        public ObservableCollection<EskdClass> EskdViews { get; set; }

        public string CodEskd
        {
            get;
            set;
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set {_description = value; }
        }

        public string PathPicture
        {
            get;
            set;
        }
        [NonSerialized]
        private string _fullPathPictures;
        public string FullPathPictures
        {
            get { return _fullPathPictures; }
            set { _fullPathPictures = value; }
        }

        [NonSerialized]
        private string _hyphen;
        public string Hyphen
        {
            get
            {
                _hyphen = string.IsNullOrEmpty(_description) ? "" : " - ";
                return _hyphen;
            }
        }

    }
    // под класс
    //public class SubClass : Classifier
    //{
    //    public string Name
    //    {
    //        get;
    //        set;
    //    }
    //}
    //Группа


    // Подгруппа

    //Вид
    public class EskdView
    {
        public string CodEskd
        {
            get;
            set;
        }
    }
}
