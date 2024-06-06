using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HammingNetwork
{
    delegate double ActiveFunction(double x);
    public class HammingNetwork
    {
        //число входных параметров
        public static int M;
        //число эталонов, которые умеем распознавать
        public static int K;
        // = M/2
        public static double T;
        // меньше 1/K
        public static double E;
        //до сколько обучаться
        public static double Emin;

        //первый слой
        double[,] ws;
        //второй слой
        double[,] es;

        //активационная функция
        ActiveFunction f;

        public HammingNetwork(double[,] xs)
        {
            ws = new double[K, M];
            es = new double[K, K];

            for (int i = 0; i < K; i++) {
                for (int j = 0; j < M; j++)
                {
                    ws[i,j] = xs[i, j] / 2;
                }
            }

            f = (x) =>
            {
                if (x <= 0) return 0;
                else if (x <= T) return x;
                return T;
            };


            for (int i = 0; i < K; i++)
            {
                for (int j = 0; j < K; j++)
                {
                    if (i == j) es[i, j] = 1;
                    else es[i, j] = -E;
                }
            }
        }

        public double[] Find(double[] x)
        {
            //выход первого слоя
            double[] s = new double[K];

            //умножение матрицы весов на вектор входных параметров
            for (int i = 0; i < K; i ++)
            {
                for (int j = 0; j < M; j++)
                {
                    s[i] += ws[i,j] * x[j];
                }

                //прибавление вектора T = {T, ..., T}
                s[i] += T;
            }

            //выход второго слоя на первом шаге это выход первого слоя
            double[] y = new double[K];

            for (int i = 0; i < K; i++)
            {
                y[i] = s[i];
            }

            //вспомогательный массив
            double[] z;

            while (true)
            {
                z = new double[K];
                //выход второго слоя до применения активационной функции
                s = new double[K];

                //умножение матрицы второго слоя на выход второго слоя
                for (int i = 0; i < K; i++)
                {
                    for (int j = 0; j < K; j++)
                    {
                        s[i] += es[i, j] * y[j];
                    }
                }

                //применяем активационную функцию
                double d = 0;
                for (int i = 0; i < K; i++)
                {
                    z[i] = f(s[i]);

                    //считаем расхождение между выходами
                    d += (z[i] - y[i]) * (z[i] - y[i]);
                }


                //если расхождение минимально - конец
                if (d < Emin)
                {
                    break;
                }

                //иначе продолжаем
                y = z;
            }

            return z;
        }
    }
}
