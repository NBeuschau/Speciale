/*using System;
using System.IO;
using SautinSoft.Document;
using Speciale_v01.TestFileCreator;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
    class WordFileCreator
    {
        public static void CreateDocx(string PATH)
        {
            BookLoader bookLoader = new BookLoader();
            List<string> bookList = bookLoader.bookLoader(PATH + "poop.txt");
            Random rng = new Random();

            for (int p = 0; p < 500; p++)
            {




                // Working directory 
                int wordsInName = rng.Next(0, 6);
                string bookName = "";
                for (int i = 0; i < wordsInName; i++)
                {
                    bookName += bookList[rng.Next(1, bookList.Count - 1)] + " ";
                }
                bookName += bookList[rng.Next(1, bookList.Count - 1)];

                bookName = RemoveSpecialCharacters(bookName);

                string docxFilePath = Path.Combine(PATH, bookName + ".docx");

                // Let's create a simple DOCX document. 
                DocumentCore docx = new DocumentCore();
                // You may download the latest version of SDK here:   
                // http://sautinsoft.com/products/docx-document/download.php  

                // Add new section. 
                Section section = new Section(docx);
                docx.Sections.Add(section);

                // Let's set page size A4. 
                section.PageSetup.PaperType = PaperType.A4;

                // Way 2 (easy): Add 2nd paragarph using another way. 
                string paragraphString = "";

                int paragraphNum = rng.Next(2, 97);
                int stringLoops = 0;
                int stringSize = 0;
                int bookPosition = 0;
                int words = 0;
                for (int i = 0; i < paragraphNum; i++)
                {
                    paragraphString = "\n \n";
                    stringLoops = rng.Next(15, 200);
                    for (int j = 0; j < stringLoops; j++)
                    {
                        stringSize = rng.Next(15, 140);
                        bookPosition = rng.Next(1, bookList.Count - stringSize);
                        for (int k = 0; k < stringSize; k++)
                        {
                            words++;
                            paragraphString += bookList[bookPosition + k] + " ";
                        }
                        paragraphString += "\n";
                    }
                    if (i == 0)
                    {
                        string intro = bookList[rng.Next(1, bookList.Count - 1)] + " ";
                        intro += bookList[rng.Next(1, bookList.Count - 1)] + " ";
                        intro += bookList[rng.Next(1, bookList.Count - 1)] + " ";
                        docx.Content.End.Insert(intro, new CharacterFormat() { Size = 20, FontColor = Color.Black });
                    }
                    docx.Content.End.Insert("\n" + paragraphString, new CharacterFormat() { Size = 12, FontColor = Color.Black });
                }

                Console.WriteLine(words);

                // Save DOCX to a file 
                docx.Save(docxFilePath);

                // Open the result for demonstation purposes. 
                //System.Diagnostics.Process.Start(docxFilePath);

            }
        }

        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_' || c == ' ')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
} */