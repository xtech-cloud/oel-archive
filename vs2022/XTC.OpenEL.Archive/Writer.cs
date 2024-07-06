using System.Text;

namespace XTC.OpenEL.Archive
{
    public class Writer
    {
        protected byte[] int32ToBytes(int _value)
        {
            byte[] buffer = new byte[4];
            buffer[0] = (byte)_value;
            buffer[1] = (byte)(_value >> 8);
            buffer[2] = (byte)(_value >> 16);
            buffer[3] = (byte)(_value >> 24);
            return buffer;
        }

        protected byte[] int64ToBytes(long _value)
        {
            byte[] buffer = new byte[8];
            buffer[0] = (byte)_value;
            buffer[1] = (byte)(_value >> 8);
            buffer[2] = (byte)(_value >> 16);
            buffer[3] = (byte)(_value >> 24);
            buffer[4] = (byte)(_value >> 32);
            buffer[5] = (byte)(_value >> 40);
            buffer[6] = (byte)(_value >> 48);
            buffer[7] = (byte)(_value >> 56);
            return buffer;
        }

        protected byte[] stringToBytes(string _value)
        {
            return Encoding.UTF8.GetBytes(_value);
        }
    }
}
