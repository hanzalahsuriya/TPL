using System;
using System.Linq;

namespace TPL.Extensions
{
    public static class StringExtentions
    {
        public static string GenerateRandom(int lenght)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, lenght).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}