using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Xml;

namespace UnovaRPGlib.Xajax
{
    internal static class XajaxParser
    {
        public static IEnumerable<XajaxCommand> Parse(string str)
        {
            var xml = new XmlDocument();
            xml.LoadXml(str);

            Debug.Assert(xml.DocumentElement?.Name == "xjx");
            XmlElement xjx = xml.DocumentElement;

            //convert the child nodes to XajaxCommand instances
            foreach (XmlElement e in xjx.ChildNodes)
            {
                Debug.Assert(e.Name == "cmd");
                //Debug.Assert(!e.HasChildNodes);

                var cmd = new XajaxCommand {
                    Value = XajaxValue.Parse(e.InnerText),
                    Attributes = new NameValueCollection()
                };

                foreach (XmlAttribute attr in e.Attributes) {
                    cmd.Attributes.Add(attr.Name, attr.Value);
                }

                yield return cmd;
            }
        }
    }
}
