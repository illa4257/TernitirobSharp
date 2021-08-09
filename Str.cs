namespace TirntirobSharp
{
    class Str
    {
        public static int SubStrCount(string str, char subchar) => SubStrCount(str, subchar.ToString());

        public static int SubStrCount(string str, string substr)
        {
            int c = 0;
            while (str.Contains(substr))
            {
                str = str[(str.IndexOf(substr) + 1)..];
                c++;
            }
            return c;
        }
    }
}
