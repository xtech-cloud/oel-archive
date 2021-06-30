
using System;
using System.Text;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using XTC.oelMVCS;

namespace oel.archive.Proto
{
public class Field
    {
        public enum Tag
        {
            NULL = 0,
            StringValue = 1,
            IntValue = 2,
            LongValue = 3,
            FloatValue = 4,
            DoubleValue = 5,
            BoolValue = 6,
            StringAryValue = 11,
            IntAryValue = 12,
            LongAryValue = 13,
            FloatAryValue = 14,
            DoubleAryValue = 15,
            BoolAryValue = 16,
            Bytes = 17,
        }

        private string value_ = "";
        private Tag tag_ = Tag.NULL;

        public Field()
        {
        }

        public static Field New(Tag _tag)
        {
            Field field = new Field();
            field.tag_ = _tag;
            return field;
        }


        public static Field FromString(string _value)
        {
            Field field = new Field();
            field.tag_ = Tag.StringValue;
            field.value_ = _value;
            return field;
        }

        public static Field FromFloat(float _value)
        {
            Field field = new Field();
            field.tag_ = Tag.FloatValue;
            field.value_ = _value.ToString();
            return field;
        }

        public static Field FromDouble(double _value)
        {
            Field field = new Field();
            field.tag_ = Tag.DoubleValue;
            field.value_ = _value.ToString();
            return field;
        }

        public static Field FromBool(bool _value)
        {
            Field field = new Field();
            field.tag_ = Tag.BoolValue;
            field.value_ = _value.ToString();
            return field;
        }

        public static Field FromInt(int _value)
        {
            Field field = new Field();
            field.tag_ = Tag.IntValue;
            field.value_ = _value.ToString();
            return field;
        }

        public static Field FromLong(long _value)
        {
            Field field = new Field();
            field.tag_ = Tag.LongValue;
            field.value_ = _value.ToString();
            return field;
        }
        public static Field FromStringAry(string[] _value)
        {
            Field field= new Field();
            field.tag_ = Tag.StringAryValue;
            string ary = "";
            foreach(string v in _value)
            {
                ary += string.Format("{0},", v);
            }
            if(!string.IsNullOrEmpty(ary))
            {
                ary = ary.Remove(ary.Length - 1, 1);
            }
            field.value_ = string.Format("[{0}]", ary);
            return field;
        }

        public static Field FromFloatAry(float[] _value)
        {
            Field field = new Field();
            field.tag_ = Tag.FloatAryValue;
            string ary = "";
            foreach (float v in _value)
            {
                ary += string.Format("{0},", v);
            }
            if (!string.IsNullOrEmpty(ary))
            {
                ary = ary.Remove(ary.Length - 1, 1);
            }
            field.value_ = string.Format("[{0}]", ary);
            return field;
        }

        public static Field FromDoubleAry(double[] _value)
        {
            Field field = new Field();
            field.tag_ = Tag.DoubleAryValue;
            string ary = "";
            foreach (double v in _value)
            {
                ary += string.Format("{0},", v);
            }
            if (!string.IsNullOrEmpty(ary))
            {
                ary = ary.Remove(ary.Length - 1, 1);
            }
            field.value_ = string.Format("[{0}]", ary);
            return field;
        }

        public static Field FromBoolAry(bool[] _value)
        {
            Field field = new Field();
            field.tag_ = Tag.BoolAryValue;
            string ary = "";
            foreach (bool v in _value)
            {
                ary += string.Format("{0},", v);
            }
            if (!string.IsNullOrEmpty(ary))
            {
                ary = ary.Remove(ary.Length - 1, 1);
            }
            field.value_ = string.Format("[{0}]", ary);
            return field;
        }

        public static Field FromIntAry(int[] _value)
        {
            Field field = new Field();
            field.tag_ = Tag.IntAryValue;
            string ary = "";
            foreach (int v in _value)
            {
                ary += string.Format("{0},", v);
            }
            if (!string.IsNullOrEmpty(ary))
            {
                ary = ary.Remove(ary.Length - 1, 1);
            }
            field.value_ = string.Format("[{0}]", ary);
            return field;
        }

        public static Field FromLongAry(long[] _value)
        {
            Field field = new Field();
            field.tag_ = Tag.LongAryValue;
            string ary = "";
            foreach (long v in _value)
            {
                ary += string.Format("{0},", v);
            }
            if (!string.IsNullOrEmpty(ary))
            {
                ary = ary.Remove(ary.Length - 1, 1);
            }
            field.value_ = string.Format("[{0}]", ary);
            return field;
        }

        public static Field FromBytes(byte[] _value)
        {
            Field field = new Field();
            field.tag_ = Tag.Bytes;
            field.value_ = Convert.ToBase64String(_value);
            return field;
        }

        public bool IsNull()
        {
            return tag_ == Tag.NULL;
        }

        public bool IsString()
        {
            return tag_ == Tag.StringValue;
        }

        public bool IsInt()
        {
            return tag_ == Tag.IntValue;
        }

        public bool IsLong()
        {
            return tag_ == Tag.LongValue;
        }

        public bool IsFloat()
        {
            return tag_ == Tag.FloatValue;
        }

        public bool IsDouble()
        {
            return tag_ == Tag.DoubleValue;
        }

        public bool IsBool()
        {
            return tag_ == Tag.BoolValue;
        }

        public bool IsStringAry()
        {
            return tag_ == Tag.StringAryValue;
        }

        public bool IsIntAry()
        {
            return tag_ == Tag.IntAryValue;
        }

        public bool IsLongAry()
        {
            return tag_ == Tag.LongAryValue;
        }

        public bool IsFloatAry()
        {
            return tag_ == Tag.FloatAryValue;
        }

        public bool IsDoubleAry()
        {
            return tag_ == Tag.DoubleAryValue;
        }

        public bool IsBoolAry()
        {
            return tag_ == Tag.BoolAryValue;
        }

        public bool IsBytes()
        {
            return tag_ == Tag.Bytes;
        }



        public string AsString()
        {
            return value_;
        }

        public int AsInt()
        {
            int value = 0;
            int.TryParse(value_, out value);
            return value;
        }

        public long AsLong()
        {
            long value = 0;
            long.TryParse(value_, out value);
            return value;
        }

        public float AsFloat()
        {
            float value = 0;
            float.TryParse(value_, out value);
            return value;
        }

        public double AsDouble()
        {
            double value = 0;
            double.TryParse(value_, out value);
            return value;
        }

        public bool AsBool()
        {
            bool value = false;
            bool.TryParse(value_, out value);
            return value;
        }

        public string[] AsStringAry()
        {
            List<string> v = new List<string>();
            if (value_.StartsWith("[") && value_.EndsWith("]"))
            {
                string ary = value_.Remove(0, 1);
                ary = ary.Remove(ary.Length - 1, 1);
                foreach (string e in ary.Split(","))
                {
                    string value = e.Trim();
                    v.Add(value);
                }
            }
            return v.ToArray();
        }

        public int[] AsIntAry()
        {
            List<int> v = new List<int>();
            if (value_.StartsWith("[") && value_.EndsWith("]"))
            {
                string ary = value_.Remove(0, 1);
                ary = ary.Remove(ary.Length - 1, 1);
                foreach (string e in ary.Split(","))
                {
                    int value;
                    if(int.TryParse(e.Trim(), out value))
                        v.Add(value);
                    else
                        v.Add(0);
                }
            }
            return v.ToArray();
        }

        public long[] AsLongAry()
        {
            List<long> v = new List<long>();
            if (value_.StartsWith("[") && value_.EndsWith("]"))
            {
                string ary = value_.Remove(0, 1);
                ary = ary.Remove(ary.Length - 1, 1);
                foreach (string e in ary.Split(","))
                {
                    long value;
                    if (long.TryParse(e.Trim(), out value))
                        v.Add(value);
                    else
                        v.Add(0);
                }
            }
            return v.ToArray();
        }

        public float[] AsFloatAry()
        {
            List<float> v = new List<float>();
            if (value_.StartsWith("[") && value_.EndsWith("]"))
            {
                string ary = value_.Remove(0, 1);
                ary = ary.Remove(ary.Length - 1, 1);
                foreach (string e in ary.Split(","))
                {
                    float value;
                    if (float.TryParse(e.Trim(), out value))
                        v.Add(value);
                    else
                        v.Add(0);
                }
            }
            return v.ToArray();
        }

        public double[] AsDoubleAry()
        {
            List<double> v = new List<double>();
            if (value_.StartsWith("[") && value_.EndsWith("]"))
            {
                string ary = value_.Remove(0, 1);
                ary = ary.Remove(ary.Length - 1, 1);
                foreach (string e in ary.Split(","))
                {
                    double value;
                    if (double.TryParse(e.Trim(), out value))
                        v.Add(value);
                    else
                        v.Add(0);
                }
            }
            return v.ToArray();
        }

        public bool[] AsBoolAry()
        {
            List<bool> v = new List<bool>();
            if (value_.StartsWith("[") && value_.EndsWith("]"))
            {
                string ary = value_.Remove(0, 1);
                ary = ary.Remove(ary.Length - 1, 1);
                foreach (string e in ary.Split(","))
                {
                    bool value;
                    if (bool.TryParse(e.Trim(), out value))
                        v.Add(value);
                    else
                        v.Add(false);
                }
            }
            return v.ToArray();
        }

        public byte[] AsBytes()
        {
            return Convert.FromBase64String(value_);
        }

        public Any AsAny()
        {
            if(IsString())
                return Any.FromString(AsString());
            if(IsInt())
                return Any.FromInt32(AsInt());
            if(IsLong())
                return Any.FromInt64(AsLong());
            if(IsFloat())
                return Any.FromFloat32(AsFloat());
            if(IsDouble())
                return Any.FromFloat64(AsDouble());
            if(IsBool())
                return Any.FromBool(AsBool());
            if (IsStringAry())
                return Any.FromStringAry(AsStringAry());
            if (IsIntAry())
                return Any.FromInt32Ary(AsIntAry());
            if (IsLongAry())
                return Any.FromInt64Ary(AsLongAry());
            if (IsFloatAry())
                return Any.FromFloat32Ary(AsFloatAry());
            if (IsDoubleAry())
                return Any.FromFloat64Ary(AsDoubleAry());
            if (IsBoolAry())
                return Any.FromBoolAry(AsBoolAry());
            if (IsBytes())
                return Any.FromBytes(AsBytes());
            return new Any();
        }
    }//class

        public class Status
        {
            public Status()
            {
                _code = new Field();
                _message = new Field();

            }
            [JsonPropertyName("code")]
            public Field _code {get;set;}
            [JsonPropertyName("message")]
            public Field _message {get;set;}

        }
    
        public class BlankRequest
        {
            public BlankRequest()
            {

            }

        }
    
        public class BlankResponse
        {
            public BlankResponse()
            {
                _status = new Status();

            }
            [JsonPropertyName("status")]
            public Status _status {get;set;}

        }
    
}
