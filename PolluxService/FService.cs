using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections;

namespace PolluxService
{
    public class FService
    {
        private string Path;
        public FService(string path)
        {
            if (Directory.Exists(path))
                this.Path = path;
            else
                this.Path = null;
        }
        public string getPath()
        {
            return this.Path;
        }
        public List<string> ProcessDirectory()
        {
            List<string> filesDir = new List<string>();
            string[] fileEntries = Directory.GetFiles(this.Path);
    
            return fileEntries.ToList<string>();
        }
    }
}
