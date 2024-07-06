using System.Text;

namespace XTC.OpenEL.Archive
{
    public class Reader
    {
        protected int bytesToInt32(byte[] _value)
        {
            int value = 0;
            value |= (int)_value[0];
            value |= ((int)_value[1]) << 8;
            value |= ((int)_value[2]) << 16;
            value |= ((int)_value[3]) << 24;
            return value;
        }

        protected long bytesToInt64(byte[] _value)
        {
            long value = 0;
            value |= (long)_value[0];
            value |= ((long)_value[1]) << 8;
            value |= ((long)_value[2]) << 16;
            value |= ((long)_value[3]) << 24;
            value |= ((long)_value[4]) << 32;
            value |= ((long)_value[5]) << 40;
            value |= ((long)_value[6]) << 48;
            value |= ((long)_value[7]) << 56;
            return value;
        }

        protected string bytesToString(byte[] _value)
        {
            return Encoding.UTF8.GetString(_value);
        }
    }
}
