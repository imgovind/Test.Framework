using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Framework.Handlers.Compression
{
    public class DeflateCompressor : Compressor
    {
        private const string DeflateEncoding = "deflate";

        public override string EncodingType
        {
            get { return DeflateEncoding; }
        }

        public override Stream CreateCompressionStream(Stream output)
        {
            return new DeflateStream(output, CompressionMode.Compress, leaveOpen: true);
        }

        public override Stream CreateDecompressionStream(Stream input)
        {
            return new DeflateStream(input, CompressionMode.Decompress, leaveOpen: true);
        }
    }
}
