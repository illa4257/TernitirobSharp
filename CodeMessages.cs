namespace TirntirobSharp
{
    class CodeMessages
    {
        public static string Get(int code)
        {
            switch (code)
            {
                case 200:
                    return " OK";
            }
            return "";
        }
    }
}
