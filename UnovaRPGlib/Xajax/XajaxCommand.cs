using System;
using System.Collections.Specialized;

namespace UnovaRPGlib.Xajax
{
    internal struct XajaxCommand
    {
        public XajaxValue Value;
        public NameValueCollection Attributes;

        public string Command => Attributes?["cmd"];
    }
}
