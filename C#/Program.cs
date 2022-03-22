using System;
using Accord.Math;
using KalmanFilter2D;

namespace C_
{
    class Program
    {

        public static async void push_test()
        {
            var rand = new Random();

            // Let's say we have the following sequence of coordinate points:
            // double[] x = new double[100];
            double[] x = Vector.Range(0.0, 50.0);
            // x = x.Apply(x_i => rand.NextDouble() * 5);
            double[] y = x.Apply(x_i => 0.07 * x_i * x_i);

            // Create a new Kalman filter
            var kf = new KF2D(1, 0.1, 0.1f);
            // var kf = new KF2D();

            Console.WriteLine("Actual Position: ");

            for (int i = 0; i < x.Length; i++) {
                Console.WriteLine(x[i] + ", " + y[i]);
            }

            Console.WriteLine();
            Console.WriteLine("Estimated Position: ");

            // Push the points into the filter
            for (int i = 1; i < x.Length; i++) {
                kf.Push(x[i-1], y[i-1]);

                kf.Push(kf.X, kf.Y);

                // Estimate the points location
                double newX = kf.X;
                double newY = kf.Y;

                // Estimate the points velocity
                double velX = kf.XAxisVelocity;
                double velY = kf.YAxisVelocity;
                // Console.WriteLine("Actual Position: " + x[i] + ", " + y[i]);
                // Console.WriteLine("Estimated Position: " + newX + ", " + newY);
                // Console.WriteLine("Estimated Velocity: " + velX + ", " + velY);
                // Console.WriteLine();
                // Console.WriteLine(x[i] + ", " + y[i]);
                Console.WriteLine(newX + ", " + newY);
            }
                

        }

        static void Main(string[] args)
        {
            push_test();
        }
    }
}
