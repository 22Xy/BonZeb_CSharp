using System;
using System.Collections.Generic;
using System.Linq;

namespace BonZeb {

	public class multiAnimalTracking {

		// public double[] GetColumn(double[,] matrix, int columnNumber) {
		// 		return Enumerable.Range(0, matrix.GetLength(0))
		// 						.Select(x => matrix[x, columnNumber])
		// 						.ToArray();
		// }

		public static double distance(double[] A, double[] B) {
			int n = A.Length;
			if (n != B.Length) return -1;
			double ret = 0;
			for (int i = 0; i < n; i++) {
				var temp = A[i] - B[i];
				ret += Math.Pow(temp, 2);
			}
			return Math.Sqrt(ret);
		}
	
	
		public static double[,] cDist(double[][] XA, double[][] XB) {

			/* Parameters
				XA: array_like
					An mA by n array of mA original observations in an n-dimensional space. Inputs are converted to double type.

				XB: array_like
					An mB by n array of mB original observations in an n-dimensional space. Inputs are converted to double type.

			Returns
				Y: ndarray
				A mA by mB distance matrix is returned. For each i and j, the metric dist(u=XA[i], v=XB[j]) is computed and stored 
				in the ij th entry.
			*/
			int mA = XA.Length, mB = XB.Length;

			double[,] result = new double[mA, mB];

			for (int i = 0; i < mA; i++) {
				for (int j = 0; j < mB; j++) {
					result[i,j] = distance(XA[i], XB[j]);
				}
			}

			return result;
		}
		private static bool Transpose<T>(ref T[,] cost)
        {
            // In our solution, we will assume that nr <= nc. If this isn't the case,
            // we transpose the entire matrix and make sure to fix up the results at
            // the end of the day.
            var nr = cost.GetLength(0);
            var nc = cost.GetLength(1);
            if (nr <= nc) return false;
            var tmpCost = new T[nc, nr];
            for (var i = 0; i < nc; i++)
            for (var j = 0; j < nr; j++)
                tmpCost[i, j] = cost[j, i];
            cost = tmpCost;
            return true;
        }

		public static List<int[]> LinearSumAssignment(double[,] cost) {
			/*
				Parameters
				----------
				cost_matrix : array
					The cost matrix of the bipartite graph.

				Returns
				-------
				row_ind, col_ind : array
					An array of row indices and one of corresponding column indices giving
					the optimal assignment. The cost of the assignment can be computed
					as ``cost_matrix[row_ind, col_ind].sum()``. The row indices will be
					sorted; in the case of a square cost matrix they will be equal to
					``numpy.arange(cost_matrix.shape[0])``.
			*/

			var nr = cost.GetLength(0);
            var nc = cost.GetLength(1);

			if (nr >= nc) {
				var tmpCost = new double[nc, nr];
				for (var i = 0; i < nc; i++)
				for (var j = 0; j < nr; j++)
					tmpCost[i, j] = cost[j, i];
				cost = tmpCost;
				nr = cost.GetLength(0);
            	nc = cost.GetLength(1);
			}

			// Initialize working arrays
			var u = new double[nr];
			var v = new double[nc];
			var shortestPathCosts = new double[nc];
			var path = new int[nc];
			var x = new int[nr];
			var y = new int[nc];
			var sr = new bool[nr];
			var sc = new bool[nc];
			Array.Fill(path, -1);
			Array.Fill(x, -1);
			Array.Fill(y, -1);

			// Find a matching one vertex at a time
			for (var curRow = 0; curRow < nr; curRow++) {
				double minVal = 0;
				var i = curRow;
				// Reset working arrays
				var remaining = new int[nc].ToList();
				var numRemaining = nc;
				for (var jp = 0; jp < nc; jp++) {
					remaining[jp] = jp;
					shortestPathCosts[jp] = double.PositiveInfinity;
				}
				Array.Clear(sr, 0, sr.Length);
				Array.Clear(sc, 0, sc.Length);

				// Start finding augmenting path
				var sink = -1;
				while (sink == -1) {
					sr[i] = true;
					var indexLowest = -1;
					var lowest = double.PositiveInfinity;
					for (var jk = 0; jk < numRemaining; jk++) {
						var jl = remaining[jk];
						// Note that this is the main bottleneck of this method; looking up the cost array
						// is costly. Some obvious attempts to improve performance include swapping rows and
						// columns, and disabling CLR bounds checking by using pointers to access the elements
						// instead. We do not seem to get any significant improvements over the simpler
						// approach below though.
						var r = minVal + cost[i, jl] - u[i] - v[jl];
						if (r < shortestPathCosts[jl])
						{
							path[jl] = i;
							shortestPathCosts[jl] = r;
						}
						// Console.WriteLine(lowest + " " + shortestPathCosts[jl]);

						if (shortestPathCosts[jl] < lowest || shortestPathCosts[jl] == lowest && y[jl] == -1)
						{
							lowest = shortestPathCosts[jl];
							indexLowest = jk;
						}
					}
					minVal = lowest;
					// Console.WriteLine(indexLowest);
					var jp = remaining[indexLowest];
					if (double.IsPositiveInfinity(minVal))
						throw new InvalidOperationException("No feasible solution.");
					if (y[jp] == -1)
						sink = jp;
					else
						i = y[jp];

					sc[jp] = true;
					remaining[indexLowest] = remaining[--numRemaining];
					remaining.RemoveAt(numRemaining);
				}
				if (sink < 0)
					throw new InvalidOperationException("No feasible solution.");

				// Update dual variables
				u[curRow] += minVal;
				for (var ip = 0; ip < nr; ip++)
					if (sr[ip] && ip != curRow)
						u[ip] += minVal - shortestPathCosts[x[ip]];

				for (var jp = 0; jp < nc; jp++)
					if (sc[jp])
						v[jp] -= minVal - shortestPathCosts[jp];

				// Augment previous solution
				var j = sink;
				while (true) {
					var ip = path[j];
					y[j] = ip;
					(j, x[ip]) = (x[ip], j);
					if (ip == curRow)
						break;
				}
			}
			var ret = new List<int[]>();
			Array.Sort(x);
			ret.Add(x);
			ret.Add(y);
			return ret;
		}
	}

}

