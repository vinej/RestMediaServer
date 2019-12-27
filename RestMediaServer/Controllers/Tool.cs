using System;

namespace RestMediaServer.Controllers
{
    public static class Tool
    {
        public static void GetTwoInteger(string value, out long id1, out long id2)
        {
            char[] sep = { '|',',' };
            string[] list = value.Split(sep,2, StringSplitOptions.RemoveEmptyEntries);
            id1 = long.Parse(list[0]);
            id2 = long.Parse(list[1]);
        }
    }
}