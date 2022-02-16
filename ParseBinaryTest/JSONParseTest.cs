using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ParseBinary;

namespace ParseBinaryTest
{
    [TestFixture]
    public class JSONParseTest
    {

        [Test]
        public void TestJson()
        {
            string Filename = "C:\\Users\\ryan.anderson\\Desktop\\ParseBinary\\ParseBinary\\FourthRun\\run3.json";

            ParseJSON pj = new ParseJSON();
            pj.JSONParse(Filename);
        }
    }
}
