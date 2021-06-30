using System.IO;

namespace XTC.oelArchive
{
    public class MemoryWriter : StreamWriter
    {
        public void Open()
        {
            stream_ = new MemoryStream();
            prepare();
        }

        public byte[] GetBytes()
        {
            MemoryStream ms = stream_ as MemoryStream;
            return ms.ToArray();
        }
    }
}

