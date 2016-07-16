using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FileRenamer;

namespace FileRenamer
{
    //************************DEFUALT PATTERN RULES************************
    // All the characters that are legal to use in a filename is useable
    // RNADOM : use a random string , Example:
    // RNADOM10 => a string of characters with lenght between 1 to 10
    // INDEX : use a number to set index , Example:
    // INDEX => add 1,2,3... to files
    // FILENAME : use the default name as a substring for new filename , Example:
    // EXTENSION => use default file extension
    // DATE : use current Date in the filename
    // TIME : use current Time in the filename
    //*********************************************************************
    public class Renamer
    {
        IFilter _filter;
        IPattern _pattern;
        public IFilter Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
            }
        }
        public IPattern Pattern
        {
            get
            {
                return _pattern;
            }
            set
            {
                _pattern = value;
            }
        }
        public Renamer(IPattern pattern, IFilter filter)
        {
            this.Filter = filter;
            this.Pattern = pattern;
        }
        public Renamer(IPattern pattern)
        {
            this.Pattern = pattern;
            this.Filter = new DefaultFilter();
        }
        public Renamer(IFilter filter)
        {
            this.Filter = filter;
            this.Pattern = new DefaultPattern();
        }
        public Renamer()
        {
            this.Pattern = new DefaultPattern();
            this.Filter = new DefaultFilter();
        }

        private bool IsThereParameter(string key, ref IDictionary<string, object> dictionary)
        {
            return dictionary.Any(p => (p.Value != null && p.Key == key));
        }

        private void MakingNewFilename(ref string pattern, IDictionary<string, object> parameters)
        {
            //params
            string filename = null;
            string extension = null;
            if (IsThereParameter("filename", ref parameters))
                filename = (string)parameters["filename"];
            if (IsThereParameter("extension", ref parameters))
                extension = (string)parameters["extension"];
            int index = -1, random = -1;
            DateTime? date = null, time = null;
            if (IsThereParameter("index", ref parameters))
                index = (int)parameters["index"];
            if (IsThereParameter("random", ref parameters))
                random = (int)parameters["random"];
            if (IsThereParameter("date", ref parameters))
                date = (DateTime?)parameters["date"];
            if (IsThereParameter("time", ref parameters))
                time = (DateTime?)parameters["time"];
            //FILENAME
            int filenameIndex = pattern.IndexOf("FILENAME");
            if (filenameIndex != -1 && !string.IsNullOrEmpty(filename))
                pattern = pattern.Replace("FILENAME", filename);
            //DATE
            int dateIndex = pattern.IndexOf("DATE");
            if (dateIndex != -1 && date.HasValue)
                pattern = pattern.Replace("DATE", date.Value.ToString("hh-mm-ss"));
            //TIME
            int timeIndex = pattern.IndexOf("TIME");
            if (timeIndex != -1 && time.HasValue)
                pattern = pattern.Replace("TIME", time.Value.ToString("MMddyyyy"));
            //INDEX
            int iIndex = pattern.IndexOf("INDEX");
            if (iIndex != -1 && index > -1)
            {
                pattern = pattern.Replace("INDEX", index.ToString(index.ToString().Length == 1 ? "00" : ""));
            }
            //RANDOM
            int randomIndex = pattern.IndexOf("RANDOM");
            if (randomIndex != -1 && random > -1)
            {
                string extra = pattern.Substring((randomIndex + "RANDOM".Length),
                    pattern.IndexOf(" ", randomIndex) - (randomIndex + "RANDOM".Length));
                if (extra.Length > 0)
                {
                    int len;
                    bool tester = int.TryParse(extra, out len);
                    if (tester)
                    {
                        pattern = pattern.Replace($"RANDOM{len}", RandomString(len));
                    }
                    else
                    {
                        throw new Exception("The random should attached to a number to generate a random string.");
                    }
                }
            }
            //EXTENSION
            int extensionIndex = pattern.IndexOf("EXTENSION");
            if (extensionIndex != -1 && !string.IsNullOrEmpty(extension))
                pattern = pattern.Replace("EXTENSION", extension);
        }
        private string getFileName(string path)
        {
            return path.Substring(path.LastIndexOf(@"\"), path.Length - path.LastIndexOf(@"\")).Remove(0, 1);
        }
        private string getPathName(string path)
        {
            int len = path.LastIndexOf(@"\");
            string pth = path.Substring(0, len);
            return pth + @"\";
        }
        public void RenameFile(FileModel model, string newPathName)
        {
            string newName = Filter.doFilter(getFileName(newPathName));
            if (newName.Length > 255)
                throw new Exception($"The filename: {model.Filename} length is illegal , the length should be less than 255.");
            newName = $"{getPathName(model.Path)}{newName}";
            if (File.Exists(newName))
            {
                string newNameRND = newName.Insert(newName.LastIndexOf('.'), RandomString(5));
                File.Move(model.Path, newNameRND);
            }
            else
            {
                File.Move(model.Path, newName);
            }
        }
        public void RenameFileWithPattern(FileModel model, string pattern, int index = -1)
        {
            Dictionary<string, object> paramerters = new Dictionary<string, object>();
            pattern = Pattern.Patterner(pattern);
            FileInfo info = new FileInfo(model.Path);
            int randomIndex = pattern.IndexOf("RANDOM");
            if (randomIndex != -1)
            {
                string extra = pattern.Substring((randomIndex + "RANDOM".Length),
                                        pattern.IndexOf(" ", randomIndex) - (randomIndex + "RANDOM".Length));
                if (extra.Length > 0)
                {
                    int len;
                    bool tester = int.TryParse(extra, out len);
                    if (tester)
                        paramerters.Add("random", (int?)len);
                    else
                        paramerters.Add("random", (int?)null);
                }
            }
            if (pattern.IndexOf("INDEX") != -1)
                paramerters.Add("index", (int?)index);
            if (pattern.IndexOf("FILENAME") != -1)
                paramerters.Add("filename", model.Filename);
            if (pattern.IndexOf("DATE") != -1)
                paramerters.Add("date", (DateTime?)DateTime.Now);
            if (pattern.IndexOf("TIME") != -1)
                paramerters.Add("time", (DateTime?)DateTime.Now);
            if (pattern.IndexOf("EXTENSION") != -1)
                paramerters.Add("extension", info.Extension);
            MakingNewFilename(ref pattern, paramerters);
            string newPathName = model.Path + @"\" + pattern;
            RenameFile(model, newPathName);
        }

        public void RenameFilesWithPattern(IEnumerable<FileModel> files, int indexStart = 1)
        {
            if (files.Count() > 0)
            {
                int i = indexStart;
                foreach (FileModel model in files)
                {
                    RenameFileWithPattern(model, model.RenameTo, i);
                    i++;
                }
            }
        }
        private Random rnd = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }
    }
}
