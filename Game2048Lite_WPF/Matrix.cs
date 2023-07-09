using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2048Lite_WPF
{
    public class Matrix<T>
    {
        public T[,] matrix;
        public int Size { get; set; }
        public Matrix(int size = 0)
        {
            matrix = new T[size, size];
            Size = size;
            this.SetDelfaultValues();
        }
        public Matrix(T[,] matrix, int size)
        {
            this.matrix = matrix;
            Size = size;
        }

        public void SetDelfaultValues()
        {
            System.Reflection.ConstructorInfo constructor = (typeof(T)).GetConstructor(System.Type.EmptyTypes);
            T obj = (ReferenceEquals(constructor, null)) ? default(T) : (T)constructor.Invoke(new object[0]);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    this.matrix[i, j] = obj;
                }
            }
        }
        public void TransposeMain()
        {
            T[,] temp = new T[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    temp[j, i] = matrix[i, j];
                }
            }
            matrix = temp;
        }
        public void HorizontalFlip()
        {
            T[,] temp = new T[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j1 = 0, j2 = Size - 1; j1 < Size; j1++, j2 --)
                {
                    temp[i, j2] = matrix[i, j1];
                }
            }
            matrix = temp;
        }
        public void VerticalFlip()
        {
            T[,] temp = new T[Size, Size];
            for (int i1 = 0, i2 = Size - 1; i1 < Size; i1++, i2--)
            {
                for (int j = 0; j < Size; j++)
                {
                    temp[i2, j] = matrix[i1, j];
                }
            }
            matrix = temp;
        }
    }
}
