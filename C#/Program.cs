using System;
using static BonZeb.multiAnimalTracking;

namespace C_
{
    class Program
    {
        static void Main(string[] args)
        {
            // Testing cDist
            // double[][] XA = new double[][]
            // {
            //     new double[] { 1, 3, 27 },
            //     new double[] { 3, 6, 8 }
            // };
            
            // var result = cDist(XA,XA);
            // for (int i = 0; i < 2; i++) {
            //     for (int j = 0; j < 2; j++) {
            //         Console.WriteLine(result[i,j]);
            //     }
            // }

            // Console.WriteLine(" ");

            // double[][] XB = new double[][]
            // {
            //     new double[] { 1, 3, 27 },
            //     new double[] { 3, 4, 6 },
            //     new double[] { 7, 6, 3 },
            //     new double[] { 3, 6, 8 }
            // };
            
            // result = cDist(XB,XB);
            // for (int i = 0; i < 4; i++) {
            //     for (int j = 0; j < 4; j++) {
            //         Console.WriteLine(result[i,j]);
            //     }
            // }

            // Testing LinearSumAssignment
            double[,] cost = new double[,] { {4, 1, 3}, {2, 0, 5}, {3, 2, 2} };
            var ret = LinearSumAssignment(cost);
            foreach (int[] ind in ret) {
                foreach (int idx in ind) {
                    Console.WriteLine(idx);
                }
                Console.WriteLine(" ");
            }
        }
    }
}
