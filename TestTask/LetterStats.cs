namespace TestTask
{
    /// <summary>
    /// Статистика вхождения буквы/пары букв
    /// </summary>
    public class LetterStats
    {
        /// <summary>
        /// Буква/Пара букв для учёта статистики.
        /// </summary>
        public string Letter;

        /// <summary>
        /// Кол-во вхождений буквы/пары.
        /// </summary>
        public int Count;

        /// <summary>
        /// Метод увеличивает счётчик вхождений по переданной структуре.
        /// </summary>
        /// <param name="letterStats"></param>
        public static LetterStats IncStatistic(LetterStats letterStats)
        {
            letterStats.Count++;
            return letterStats;

        }
        public static bool CompareLetter(LetterStats letterStats, string c)
        {
            if (letterStats.Letter.CompareTo(c) == 0)
                return true;
            return false;
        }
        public static LetterStats CreateLetterStats(string c)
        {
            return new LetterStats { Letter = c, Count = 1 };
        }
    }
}
