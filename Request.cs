namespace TirntirobSharp
{
    class Request
    {
        public string Method;
        public string URI;
        public string HTTPv;
        public string RemoteIP;
        public string LocalIP;
        public ListStrStr Headers = new ListStrStr();
    }
}
