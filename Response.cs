namespace TirntirobSharp
{
    class Response
    {
        public bool ForceClose = false;
        public bool HTTPResponse = true;
        public int Code;
        public string HTTPv;
        public string ContentType;
        public byte[] Result;
    }
}
