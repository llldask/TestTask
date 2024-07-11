using System;
using System.IO;
using System.Text;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {
        private Stream _localStream;

        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            _localStream = File.OpenRead(fileFullPath);
            CheckLenghtStream();
        }
        private void CheckLenghtStream()
        {
            if (_localStream == null || _localStream.Length == 0)
                IsEof = true;
            else
                IsEof = false;
        }
        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof
        {
            get;
            private set;
        }

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        private int sizeBuffer = 1;
        public char ReadNextChar()
        {
            byte[] b = new byte[1];
            UTF8Encoding temp = new UTF8Encoding(true);
            if (_localStream.Read(b, 0, sizeBuffer) > 0)
                return char.Parse(temp.GetString(b, 0, sizeBuffer));
            else
            {
                IsEof = true;
                throw new EndOfStreamException();
            }

        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            if (_localStream == null)
            {
                CheckLenghtStream();
                return;
            }

            _localStream.Position = 0;
            IsEof = false;
        }

        public void Close()
        {
            _localStream.Close();
        }
        ~ReadOnlyStream()
        {
            Close();
        }
    }
}
