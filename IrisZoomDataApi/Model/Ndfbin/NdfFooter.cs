using System;
using System.Collections.Generic;
using System.Text;

namespace IrisZoomDataApi.Model.Ndfbin
{
    /// <summary>
    /// typedef struct ndfbinFooterPart {
    ///    char partName[4];
    ///    DWORD junk1;
    ///    DWORD offset;
    ///    DWORD junk2;
    ///    DWORD length;
    ///    DWORD junk3;
    /// } footerPart;
    ///
    /// struct ndfbinFooter {
    ///    char header[8];
    ///    footerPart entries[9];
    /// };
    /// </summary>
    public class NdfFooter
    {
        private readonly List<NdfFooterEntry> _entries = new List<NdfFooterEntry>();

        private string _header;

        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
            }
        }

        public void AddEntry(string name, long offset, long size)
        {
            Entries.Add(new NdfFooterEntry() { Name = name.ToUpper(), Offset = offset, Size = size });
        }

        public byte[] GetBytes()
        {
            var data = new List<byte>();

            var seperator = new byte[4];

            data.AddRange(Encoding.ASCII.GetBytes("TOC0"));
            data.AddRange(BitConverter.GetBytes(Entries.Count));

            foreach (var entry in Entries)
            {
                data.AddRange(Encoding.ASCII.GetBytes(entry.Name));
                data.AddRange(seperator);
                data.AddRange(BitConverter.GetBytes(entry.Offset));
                data.AddRange(BitConverter.GetBytes(entry.Size));
            }

            return data.ToArray();
        }

        public List<NdfFooterEntry> Entries
        {
            get { return _entries; }
        }
    }
}