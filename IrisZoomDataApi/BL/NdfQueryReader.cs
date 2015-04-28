using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IrisZoomDataApi.BL
{
    /// <summary>
    /// HELL YEAH !
    /// </summary>
    public class NdfQueryReader
    {
        private const char[] SEGMENT_SEPARATOR = { '.' };
        private const char[] INDEX_SEPARATORS = { '[', ']' };

        public NdfQueryReader()
        {
        }

        /// <summary>
        /// Return the next step of the query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public string ParseNextStep(string query, out string rest)
        {
            rest = string.Empty;
            string[] parts = query.Split(SEGMENT_SEPARATOR);

            string next = parts[0];

            rest = query.Substring(next.Length + 1);

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
