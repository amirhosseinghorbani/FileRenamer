using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRenamer
{
    public interface IFilter
    {
        string doFilter(string filterPattern);
    }
}
