using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2048Lite_WPF
{
    public static class Extentions
    {
        public static bool BiasArr<T>(this T[] arr)
        {
            bool total = false;
            for (int i = 0, j = 0; i < arr.Length; i++)
            {
                if (!arr[i].Equals(default(T)))
                {
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                    j++;
                    total = true;
                }
            }
            return total;
        }
        public static T[] GetColumn<T>(this T[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0)).Select(x => matrix[x, columnNumber]).ToArray();
        }
        public static T[] GetRow<T>(this T[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1)).Select(x => matrix[rowNumber, x]).ToArray();
        }
    }
}
