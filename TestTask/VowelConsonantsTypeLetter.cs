using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask
{
    public class VowelConsonantsTypeLetter
    {
        public static Dictionary<CharType, char[]> Dict = new Dictionary<CharType, char[]>()
        {
            { CharType.Vowel, new char[]{ 'a', 'e', 'i', 'o', 'u', 'y' } },
            { CharType.Consonants, new char[]{ 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'y', 'z' } }

        };
    }
}
