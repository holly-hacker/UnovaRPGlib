using System;
using System.Diagnostics;

namespace UnovaRPGlib.Xajax
{
    internal struct XajaxValue
    {
        public DataType Type;
        public string Text;

        public XajaxValue(object o)
        {
            switch (o)
            {
                case XajaxValue x:
                    Type = x.Type;
                    break;
                case string _:
                    Type = DataType.String;
                    break;
                case bool _:
                    Type = DataType.Boolean;
                    break;
                case sbyte _:
                case byte _:
                case short _:
                case ushort _:
                case int _:
                case uint _:
                case long _:
                case ulong _:
                    Type = DataType.Number;
                    break;
                default:
                    if (o == null) {
                        Type = DataType.Null;
                    }
                    else {
                        Debug.Fail("Unknown type"); //only fails on debug build
                        Type = DataType.String;
                    }
                    break;
            }

            Text = o?.ToString();
        }

        public override string ToString() => (char) Type + Text;

        public static XajaxValue Parse(string str)
        {
            return new XajaxValue {
                Type = (DataType)str[0],
                Text = str.Substring(1)
            };
        }
    }
}
