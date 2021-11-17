using System;
using static BonZeb.multiAnimalTracking;
using LinearAssignment;

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

            double[][] XC = new double[][] {
                new double[] {35.0456, -85.2672}, 
                new double[] {35.1674, -89.98811}, 
                new double[] {35.95678, -83.94352},
                new double[] {36.16347, -86.7834435}
            };

            double[][] XD = new double[][] {
                new double[] {35.0456, -85.2672}, 
                new double[] {35.1174, -89.9711},
                new double[] {36.1667, -86.7833},
                new double[] {35.9728, -83.9322}
            };
            
            
            var cost = cDist(XC,XD);
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    Console.Write(cost[i,j] + ", ");
                }
            }

// [0, 4.704447943170383, 1.8855833102782755, 1.6253999015626954], [4.722480965350737, 0.0528142035819881, 3.3569938376619124, 6.109231955663505], [1.6069778264805001, 6.0857439513012705, 2.8475282008787803, 0.01961588132101616], [1.8837801591991161, 3.3549092993540457, 0.0032331860834191707, 2.8576116855150744]

            // Testing LinearSumAssignment
            // var cost = new double[,] { {4, 1, 3, 4}, 
            //                            {2, 0, 5, 4}, 
            //                            {3, 12, 123, 2},
            //                            {3, 123, 2, 2},
            //                            {12, 2, 3, 2}};
            // var cost = new double[,] { {4, 1, 3}, 
            //                            {2, 0, 5}, 
            //                            {3, 2, 2},
            //                            {12, 3, 1}};
            // double[,] cost = new double[,] { {35.0456, -85.2672}, 
            //                                  {35.1174, -89.9711}, 
            //                                  {35.9728, -83.9422},
            //                                  {36.1667, -86.7833} };
            var ret = LinearSumAssignment(cost);
            // var ret = Solver.Solve(cost);
            foreach (int[] ind in ret) {
                foreach (int idx in ind) {
                    Console.Write(idx + " ");
                }
                Console.WriteLine(" ");
            }
            // foreach (var ind in ret.RowAssignment) {
            //     Console.Write(ind + " ");
            // }
            // Console.WriteLine("\n");
            // foreach (var ind in ret.ColumnAssignment) {
            //     Console.Write(ind + " ");
            // }
            // Console.WriteLine(ret.ColumnAssignment);
        }
    }
}
