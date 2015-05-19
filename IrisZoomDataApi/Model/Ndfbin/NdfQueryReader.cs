using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisZoomDataApi.Model.Ndfbin
{
    /// <summary>
    /// Read string
    /// </summary>
    public class NdfQueryReader
    {
        private static char[] SEGMENT_SEPARATOR = { '.' };
        private static char[] INDEX_SEPARATORS = { '[', ']' };

        /// <summary>
        /// Return the next step of the query. Sounds good.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="rest"></param>
        /// <returns>The string representing the next step inthe query, can be a property name, an empty string or an item index. </returns>
        public static string ParseNextStep(string query, out string rest)
        {
            rest = string.Empty;
            string[] parts = query.Split(SEGMENT_SEPARATOR, System.StringSplitOptions.RemoveEmptyEntries);
            string next = string.Empty;
            if (parts.Length > 0)
            {
                next = parts[0];
                rest = query.Substring(next.Length + 1);
            }

            string[] listPart = next.Split(INDEX_SEPARATORS);
            if(listPart.Length > 1)
            {
                next = listPart[0];
                rest = listPart[1] + "." + rest;
            }
            return next;
        }



    }
}
