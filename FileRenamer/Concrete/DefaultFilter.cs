using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileRenamer
{
    public class DefaultFilter : IFilter
    {
        const int MAX_LENGHT = 255;
        public DefaultFilter()
        {
            illegalStatements = new List<string>()
            {
                "<",">",":","\"","/","|","\\","?","*",
                "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
                "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9",
            };
        }
        public DefaultFilter(List<string> illegals)
        {
            illegalStatements.AddRange(illegals);
        }
        public List<string> illegalStatements { get; set; }
        public string doFilter(string FilterPattern)
        {
            if (File.Exists(FilterPattern) == true)
                throw new Exception($"{FilterPattern} is already exist.");
            foreach (string str in illegalStatements)
            {//Automaticly remove illegal strings
                FilterPattern = FilterPattern.Replace(str, "");
            }
            if (FilterPattern.Length >= MAX_LENGHT)
                throw new Exception("File Length is greater than what it should be!");
            return FilterPattern;
        }
    }
}
