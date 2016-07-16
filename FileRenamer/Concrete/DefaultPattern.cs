using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRenamer
{
    public class DefaultPattern : IPattern
    {
        // All the characters that are legal to use in a filename is useable
        // RNADOM : use a random string , Example:
        // RNADOM10 => a string of characters with lenght between 1 to 10
        // INDEX : use a number to set index , Example:
        // INDEX => add 1,2,3... to files
        // FILENAME : use the default name as a substring for new filename , Example:
        // EXTENSION => use default file extension
        // DATE : use current Date in the filename
        // TIME : use current Time in the filename

        public string Patterner(object pattern)
        {
            //For the default pattern we prefer to use default rules of renamer
            //So just return the pattern that we got , but we can customize it.
            return (string)pattern;
        }
    }
}
