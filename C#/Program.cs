using System;
using Accord.Math;
using KalmanFilter2D;

namespace C_
{
    class Program
    {

        public static void push_test()
        {
            var rand = new Random();

            // Let's say we have the following sequence of coordinate points:
            double[] x = new double[100];
            x = x.Apply(x_i => rand.NextDouble() * 60);
            double[] y = x.Apply(x_i => 0.25 * x_i * x_i + rand.NextDouble() * 600);

            // Create a new Kalman filter
            var kf = new KF2D(60, 0.0005f, 0.1f);

            // Push the points into the filter
            for (int i = 0; i < x.Length; i++) {
                kf.Push(x[i], y[i]);

                // Estimate the points location
                double newX = kf.X;
                double newY = kf.Y;

                // Estimate the points velocity
                double velX = kf.XAxisVelocity;
                double velY = kf.YAxisVelocity;
                Console.WriteLine("Current Position: " + x[i] + ", " + y[i]);
                Console.WriteLine("Estimated Position: " + newX + ", " + newY);
                Console.WriteLine("Velocity: " + velX + ", " + velY);
                Console.WriteLine();
                // Console.WriteLine(x[i] + ", " + y[i]);
                // Console.WriteLine(newX + ", " + newY);
            }
                

        }

        static void Main(string[] args)
        {

            push_test();
        }
    }
}
