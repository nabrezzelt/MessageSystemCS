using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageSystemCSDesktopApp
{
    class LocalClientData
    {
        public string uid;
        public string publicKey;

        public LocalClientData(string uid, string publicKey)
        {
            this.uid = uid;
            this.publicKey = publicKey;
        }

        public override string ToString()
        {
            return uid + ";" + publicKey;
        }
    }
}
