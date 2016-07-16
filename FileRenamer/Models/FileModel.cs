using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FileRenamer
{
    public class FileModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Boolean isChecked = false;
        private string filename = string.Empty;
        private string renameTo = null;
        private string path = null;

        private void NotifyPropertyChanged([CallerMemberName]String propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }



        [ReadOnly(true)]
        public Boolean IsChecked
        {
            get
            {
                return this.isChecked;
            }

            set
            {
                if (value != this.isChecked)
                {
                    this.isChecked = value;
                    NotifyPropertyChanged();
                }
            }
        }


        [ReadOnly(true)]
        public string Filename
        {
            get
            {
                return path.Substring(path.LastIndexOf(@"\"),
                    path.Length - path.LastIndexOf(@"\")).Remove(0, 1);
            }
        }
        public string RenameTo
        {
            get
            {
                if (!string.IsNullOrEmpty(renameTo))
                    return renameTo.Substring(renameTo.LastIndexOf(@"\"),
                    renameTo.Length - renameTo.LastIndexOf(@"\")).Remove(0, 1);
                else
                    return this.renameTo;
            }
            set
            {
                if (value != this.renameTo)
                {
                    this.renameTo = value;
                    NotifyPropertyChanged();
                }
            }
        }
        [ReadOnly(true)]
        public string Path
        {
            get
            {
                return this.path;
            }
            set
            {
                if (value != this.path)
                {
                    this.path = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
