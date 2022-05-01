namespace KalmanFilter2D
{
    using System;
    using Accord.Math;

    public class KF2D
    {

        double samplingRate = 1;

        double acceleration = 0.0005f;
        double accelStdDev = 0.1f;

        double[,] Q_estimate; // (location_0, location_1, vel_0, vel_1)

        double[,] A;
        double[,] B;
        double[,] C;

        double[,] Ez;
        double[,] Ex;
        double[,] P;
        double[,] K;
        double[,] Aux;

        static readonly double[,] diagonal =
        {
            { 1, 0, 0, 0 },
            { 0, 1, 0, 0 },
            { 0, 0, 1, 0 },
            { 0, 0, 0, 1 }
        };

        /// <summary>
        ///   Gets or sets the current X position of the object.
        /// </summary>
        /// 
        public double X
        {
            get { return Q_estimate[0, 0]; }
            set { Q_estimate[0, 0] = value; }
        }

        /// <summary>
        ///   Gets or sets the current Y position of the object.
        /// </summary>
        /// 
        public double Y
        {
            get { return Q_estimate[1, 0]; }
            set { Q_estimate[1, 0] = value; }
        }

        /// <summary>
        ///   Gets or sets the current object's velocity in the X axis.
        /// </summary>
        /// 
        public double XAxisVelocity
        {
            get { return Q_estimate[2, 0]; }
            set { Q_estimate[2, 0] = value; }
        }

        /// <summary>
        ///   Gets or sets the current object's velocity in the Y axis.
        /// </summary>
        /// 
        public double YAxisVelocity
        {
            get { return Q_estimate[3, 0]; }
            set { Q_estimate[3, 0] = value; }
        }

        /// <summary>
        ///   Gets or sets the observational noise 
        ///   of the current object's in the X axis.
        /// </summary>
        /// 
        public double NoiseX
        {
            get { return Ez[0, 0]; }
            set { Ez[0, 0] = value; }
        }

        /// <summary>
        ///   Gets or sets the observational noise 
        ///   of the current object's in the Y axis.
        /// </summary>
        /// 
        public double NoiseY
        {
            get { return Ez[1, 1]; }
            set { Ez[1, 1] = value; }
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="KalmanFilter2D"/> class.
        /// </summary>
        /// 
        public KF2D()
        {
            initialize();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="KalmanFilter2D"/> class.
        /// </summary>
        /// 
        /// <param name="samplingRate">The sampling rate.</param>
        /// <param name="acceleration">The acceleration.</param>
        /// <param name="accelerationStdDev">The acceleration standard deviation.</param>
        /// 
        public KF2D(double samplingRate, double acceleration, double accelerationStdDev)
        {
            this.acceleration = acceleration;
            this.accelStdDev = accelerationStdDev;
            this.samplingRate = samplingRate;

            initialize();
        }

        private void initialize()
        {
            double dt = samplingRate;

            A = new double[,]
            {
                { 1,  0, dt,  0 },
                { 0,  1,  0, dt },
                { 0,  0,  1,  0 },
                { 0,  0,  0,  1 }
            };

            B = new double[,]
            {
                { (dt * dt) / 2 },
                { (dt * dt) / 2 },
                {       dt      },
                {       dt      }
            };

            C = new double[,]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 }
            };

            Ez = new double[,] 
            {
                { 1.0, 0.0 }, 
                { 0.0, 1.0 }
            };

            double dt2 = dt * dt;
            double dt3 = dt2 * dt;
            double dt4 = dt2 * dt2;

            double aVar = accelStdDev * accelStdDev;

            Ex = new double[4, 4]
            {
                { dt4 / 4,        0,  dt3 / 2,        0 },
                { 0,        dt4 / 4,        0,  dt3 / 2 },
                { dt3 / 2,        0,      dt2,        0 },
                { 0,        dt3 / 2,        0,      dt2 }
            };

            Ex.Multiply(aVar, result: Ex);

            Q_estimate = new double[4, 1];
            P = Ex.MemberwiseClone();
        }


        /// <summary>
        ///   Registers the occurrence of a value.
        /// </summary>
        /// 
        /// <param name="value">The value to be registered.</param>
        /// 
        public void Push(double[] value)
        {
            if (value.Length != 2)
                Console.WriteLine("Dimension mismatch");

            Push(value[0], value[1]);
        }

        /// <summary>
        ///   Registers the occurrence of a value.
        /// </summary>
        /// 
        /// <param name="value">The value to be registered.</param>
        /// 
        // public void Push(DoublePoint value)
        // {
        //     Push(value.X, value.Y);
        // }

        /// <summary>
        ///   Registers the occurrence of a value.
        /// </summary>
        /// 
        /// <param name="x">The x-coordinate of the value to be registered.</param>
        /// <param name="y">The y-coordinate of the value to be registered.</param>
        /// 
        public void Push(double x, double y)
        {
            double[,] Qloc = { { x }, { y } };

            // Predict next state
            Q_estimate = Matrix.Dot(A, Q_estimate).Add(B.Multiply(acceleration));

            // Predict Covariances
            P = Matrix.Dot(A, P.DotWithTransposed(A)).Add(Ex);

            Aux = Matrix.Dot(C, P.DotWithTransposed(C)).Add(Ez).PseudoInverse();

            // Kalman Gain
            K = P.Dot(C.TransposeAndDot(Aux));
            Q_estimate = Q_estimate.Add(K.Dot(Qloc.Subtract(C.Dot(Q_estimate))));

            // Update P (Covariances)
            P = Matrix.Dot(diagonal.Subtract(Matrix.Dot(K, C)), P);
        }


        /// <summary>
        ///   Clears all measures previously computed.
        /// </summary>
        /// 
        public void Clear()
        {
            this.NoiseX = 0;
            this.NoiseY = 0;

            this.XAxisVelocity = 0;
            this.YAxisVelocity = 0;

            this.X = 0;
            this.Y = 0;
        }
    }
}