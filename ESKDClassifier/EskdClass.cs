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

        public string Description
        {
            get;
            set;
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
            get => _fullPathPictures;
            set => _fullPathPictures = value;
        }

        [NonSerialized]
        private string _hyphen;
        public string Hyphen
        {
            get
            {
                _hyphen = string.IsNullOrEmpty(Description) ? "" : " - ";
                return _hyphen;
            }
        }

    }

}
