using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner.Helpers
{
    public interface IRunner
    {
        void Execute(string runon,string filename,string parameters);
    }
}
