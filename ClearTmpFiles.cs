using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TypeSSF
{
    public class ClearTmpFile(string Filename) : IDisposable
    {
        private readonly string FileName = Filename;

        public void Dispose()
        {
            if (File.Exists(FileName + ".ssf"))
                File.Delete(FileName + ".ssf");
            if (File.Exists(FileName + ".zip"))
                File.Delete(FileName + ".zip");
            if (Directory.Exists(FileName))
                Directory.Delete(FileName, true);
        }
    }
}
