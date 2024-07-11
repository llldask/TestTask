using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestTask
{
    public class Program
    {

        /// <summary>
        /// Программа принимает на входе 2 пути до файлов.
        /// Анализирует в первом файле кол-во вхождений каждой буквы (регистрозависимо). Например А, б, Б, Г и т.д.
        /// Анализирует во втором файле кол-во вхождений парных букв (не регистрозависимо). Например АА, Оо, еЕ, тт и т.д.
        /// По окончанию работы - выводит данную статистику на экран.
        /// </summary>
        /// <param name="args">Первый параметр - путь до первого файла.
        /// Второй параметр - путь до второго файла.</param>
        static void Main(string[] args)
        {
            IReadOnlyStream inputStream1 = GetInputStream(args[0]);
            IReadOnlyStream inputStream2 = GetInputStream(args[1]);

            IList<LetterStats> singleLetterStats = FillSingleLetterStats(inputStream1);
            IList<LetterStats> doubleLetterStats = FillDoubleLetterStats(inputStream2);

            RemoveCharStatsByType(singleLetterStats, CharType.Vowel);
            RemoveCharStatsByType(doubleLetterStats, CharType.Consonants);

            PrintStatistic(singleLetterStats);
            Console.WriteLine();
            PrintStatistic(doubleLetterStats);

            Console.WriteLine("Нажмите на любую клавишу");
            Console.ReadKey();
        }

        /// <summary>
        /// Ф-ция возвращает экземпляр потока с уже загруженным файлом для последующего посимвольного чтения.
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        /// <returns>Поток для последующего чтения.</returns>
        private static IReadOnlyStream GetInputStream(string fileFullPath)
        {
            return new ReadOnlyStream(fileFullPath);
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения каждой буквы.
        /// Статистика РЕГИСТРОЗАВИСИМАЯ!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillSingleLetterStats(IReadOnlyStream stream)
        {
            IList<LetterStats> result = new List<LetterStats>();
            stream.ResetPositionToStart();
            while (!stream.IsEof)
            {
                bool flExistLetter = false;
                char c = '0';
                try
                {
                    c = stream.ReadNextChar();
                }
                catch (EndOfStreamException)
                {
                    break;
                }
                for (int i = 0; i < result.Count; i++)
                {
                    if (LetterStats.CompareLetter(result[i], c.ToString()))
                    {
                        result[i] = LetterStats.IncStatistic(result[i]);
                        flExistLetter = true;
                        break;
                    }
                }
                if (flExistLetter == false)
                    result.Add(LetterStats.CreateLetterStats(c.ToString()));
            }
            return result;
        }

        /// <summary>
        /// Ф-ция считывающая из входящего потока все буквы, и возвращающая коллекцию статистик вхождения парных букв.
        /// В статистику должны попадать только пары из одинаковых букв, например АА, СС, УУ, ЕЕ и т.д.
        /// Статистика - НЕ регистрозависимая!
        /// </summary>
        /// <param name="stream">Стрим для считывания символов для последующего анализа</param>
        /// <returns>Коллекция статистик по каждой букве, что была прочитана из стрима.</returns>
        private static IList<LetterStats> FillDoubleLetterStats(IReadOnlyStream stream)
        {
            IList<LetterStats> result = new List<LetterStats>();
            stream.ResetPositionToStart();
            char c = '0';
            try
            {
                c = stream.ReadNextChar();
            }
            catch (EndOfStreamException)
            {
                return result;
            }
            string str = c.ToString();
            while (!stream.IsEof)
            {
                bool flExistLetter = false;
                try
                {
                    c = stream.ReadNextChar();
                }
                catch (EndOfStreamException)
                {
                    break;
                }
                str += c;
                str = str.ToLower();
                for (int i = 0; i < result.Count; i++)
                {
                    if (LetterStats.CompareLetter(result[i], str))
                    {
                        result[i] = LetterStats.IncStatistic(result[i]);
                        flExistLetter = true;
                        break;
                    }
                }
                if (flExistLetter == false)
                    result.Add(LetterStats.CreateLetterStats(str));
                str = c.ToString();
            }
            return result;
        }

        /// <summary>
        /// Ф-ция перебирает все найденные буквы/парные буквы, содержащие в себе только гласные или согласные буквы.
        /// (Тип букв для перебора определяется параметром charType)
        /// Все найденные буквы/пары соответствующие параметру поиска - удаляются из переданной коллекции статистик.
        /// </summary>
        /// <param name="letters">Коллекция со статистиками вхождения букв/пар</param>
        /// <param name="charType">Тип букв для анализа</param>
        private static void RemoveCharStatsByType(IList<LetterStats> letters, CharType charType)
        {
            var listRemove = new List<int>();
            for (int i = 0; i < letters.Count; i++)
            {
                if (CoincidenceTypeOfLetter(letters[i].Letter, charType))
                {
                    listRemove.Add(i);
                }
            }
            for (int i = listRemove.Count - 1; i >= 0; i--)
            {
                letters.RemoveAt(listRemove[i]);
            }
        }

        /// <summary>
        /// Ф-ция выводит на экран полученную статистику в формате "{Буква} : {Кол-во}"
        /// Каждая буква - с новой строки.
        /// Выводить на экран необходимо предварительно отсортировав набор по алфавиту.
        /// В конце отдельная строчка с ИТОГО, содержащая в себе общее кол-во найденных букв/пар
        /// </summary>
        /// <param name="letters">Коллекция со статистикой</param>
        private static void PrintStatistic(IEnumerable<LetterStats> letters)
        {
            letters = letters.OrderBy(x => x.Letter);
            foreach (LetterStats letter in letters)
            {
                Console.WriteLine(letter.Letter + " : " + letter.Count);
            }
        }


        public static bool CoincidenceTypeOfLetter(string str, CharType type)
        {
            int count = 0;
            foreach (var ch in str)
            {
                if (Array.IndexOf(VowelConsonantsTypeLetter.Dict[type], ch) != -1)
                    count++;
            }
            if (count == str.Length)
                return true;
            return false;
        }
    }
}
