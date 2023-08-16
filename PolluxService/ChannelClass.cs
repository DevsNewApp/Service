using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolluxService
{
    internal class ChannelClass
    {
       public string pathSourceChannel { get; set; }
       public string pathDestinationChannel { get; set; }
       public string typeChannel { get; set; }
       public string deleteFiles { get; set; }
       public string backupFiles { get; set; }
       public ChannelClass() { }
       public void Init(string pathSourceChannel, string pathDestinationChannel, string typeChannel, string deleteFiles, string backupFiles)
        {
            this.pathSourceChannel = pathSourceChannel;
            this.pathDestinationChannel = pathDestinationChannel;
            this.typeChannel = typeChannel;
            this.deleteFiles = deleteFiles;
            this.backupFiles = backupFiles;
        }

    }
}
