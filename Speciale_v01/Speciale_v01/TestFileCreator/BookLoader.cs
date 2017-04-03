using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Speciale_v01.TestFileCreator
{
    class BookLoader
    {
        public List<string> bookLoader(string PATH)
        {
            string bookText = File.ReadAllText(PATH);
            bookText = bookText.Replace(System.Environment.NewLine, "");
            string[] words = bookText.Split(' ');
            List<string> wordList = words.ToList<string>();

            return wordList;
        }
    }
}
