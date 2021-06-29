﻿using System.IO;

namespace XTC.oelArchive
{
    public class FileReader : StreamReader
    {
        public void Open(string _filepath)
        {
            if(!File.Exists(_filepath))
            {
                return;
            }

            stream_ = File.OpenRead(_filepath);
            parseHeader();
        }

        public void Open(byte[] _data)
        {
            if(null == _data | _data.Length == 0)
            {
                return;
            }

            stream_ = new MemoryStream(_data);
            parseHeader();
        }
    }
}

