using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Chapter27_AsynchronousOperations_Cache
{
    internal class Program
    {
        [StructLayout(LayoutKind.Explicit)]
        private class Data
        {
            // Два соседних поля.скорее всего.расположены в одной строке кэша
            [FieldOffset(0)]
            public Int32 field1;

            [FieldOffset(64)]
            public Int32 field2;
        }

        private const Int32 iterations = 100000000; // 100 миллионов
        private static Int32 s_operations = 2;
        private static Stopwatch s_startTime;

        static void Main(string[] args)
        {
            // Выделяем объект и записываем начальное время
            Data data = new Data();
            s_startTime = Stopwatch.StartNew();
            // Два потока имеют доступ к своим полям внутри структуры
            ThreadPool.QueueUserWorkItem(o => AccessData(data, 0));
            ThreadPool.QueueUserWorkItem(o => AccessData(data, 1));
            // Для целей тестирования заблокируем поток Main
            Console.ReadKey();
        }

        private static void AccessData(Data data, Int32 field)
        {
            // Каждый поток имеет доступ к своим полям в объекте Data
            for (Int32 x = 0; x < iterations; x++)
                if (field == 0) data.field1++; else data.field2++;
            // Последний завершенный поток показывает время работы
            if (Interlocked.Decrement(ref s_operations) == 0)
                Console.WriteLine("Access time: {0:N} ", s_startTime.ElapsedMilliseconds);
            // 2300мс без сдвига
            // 463мс со сдвигом
        }
    }
}