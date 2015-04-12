namespace IrisZoomDataApi.Model.Ndfbin
{
    /// <summary>
    /// struct cndfHeader {
    ///	char header[12];
    ///	DWORD always128;
    ///	DWORD blockSizef;
    ///	DWORD chunk1;
    ///	DWORD len4;
    ///	DWORD chunk2;
    ///	DWORD blockSizePlusE0;
    ///	DWORD chunk3;
    /// only if compressed
    ///	DWORD blockSizePlusE0MinusLen4;
    ///};
    /// </summary>
    public class NdfHeader
    {
        private bool _isCompressedBody;
        private int _blockSize;
        private int _blockSizeE0;

        // Only if compressed = true;

        private int _blockSizeWithoutHeader;

        public int FileSizeUncompressed
        {
            get { return _blockSize; }
            set { _blockSize = value; }
        }

        public int FileSizeUncompressedMinusE0
        {
            get { return _blockSizeE0; }
            set { _blockSizeE0 = value; }
        }

        public int UncompressedContentSize
        {
            get { return _blockSizeWithoutHeader; }
            set { _blockSizeWithoutHeader = value; }
        }

        public bool IsCompressedBody
        {
            get { return _isCompressedBody; }
            set { _isCompressedBody = value; }
        }
    }
}
