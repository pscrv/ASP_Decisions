using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ASP_Decisions.Formatters
{
    public static class Formatter
    {
        public static string FormatCaseNumber(string cn)
        {
            string search = cn.Trim().ToUpper();
            if (search == "")
                return cn;  // nothing there, send it back


            Regex re = new Regex(@"(.*)([DGJRTW]) *(\d*)/(\d*)(.*)");
            Match found = re.Match(search);
            if (!found.Success)
                return cn;

            if (found.Groups[1].Value != "" || found.Groups[5].Value != "")
                return cn;

            StringBuilder builder = new StringBuilder();
            builder.Append(found.Groups[2].Value);
            builder.Append(' ');
            builder.Append(found.Groups[3].Value.PadLeft(4, '0'));
            builder.Append('/');
            builder.Append(found.Groups[4].Value);

            return builder.ToString();
        }


    }
}