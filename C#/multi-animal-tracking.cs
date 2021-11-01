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
			/	An mB by n array of mB original observations in an n-dimensional space. Inputs are converted to double type.

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

		public static List<int[]> LinearSumAssignment(double[,] cost) {

			var nr = cost.GetLength(0);
            var nc = cost.GetLength(1);

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

                        if (shortestPathCosts[jl] < lowest || shortestPathCosts[jl] == lowest && y[jl] == -1)
                        {
                            lowest = shortestPathCosts[jl];
                            indexLowest = jk;
                        }
                    }

                    minVal = lowest;
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
			ret.Add(x);
			ret.Add(y);
			return ret;
		}

		// def find_identities(first_tracking_data_arr, second_tracking_data_arr, tail_curvature_arr, n_fish, n_tracking_points):
		//     cost = cdist(first_tracking_data_arr[:, 0], second_tracking_data_arr[:, 0])
		//     result = linear_sum_assignment(cost)
		//     if second_tracking_data_arr.shape[0] < n_fish:
		//         missed_index = [i for i in range(len(first_tracking_data_arr)) if i not in result[0]][0]
		//         merged_index = np.where(cost[missed_index] == np.min(cost[missed_index]))[0][0]
		//         second_tracking_data_arr = np.append(second_tracking_data_arr, second_tracking_data_arr[merged_index]).reshape(-1, n_tracking_points, 2)
		//         tail_curvature_arr = np.append(tail_curvature_arr, tail_curvature_arr[merged_index]).reshape(-1, n_tracking_points - 1)
		//         second_tracking_data_arr, tail_curvature_arr, result = find_identities(first_tracking_data_arr, second_tracking_data_arr, tail_curvature_arr, n_fish, n_tracking_points)
		//     return second_tracking_data_arr, tail_curvature_arr, result

		// public Tuple FindIdentities(NDArray first_tracking_data_arr, NDArray second_tracking_data_arr, NDArray tail_curvature_arr, int n_fish, int n_tracking_points) {
		// 	var cost = cdist(GetColumn(first_tracking_data_arr, 0), GetColumn(second_tracking_data_arr, 0));
		// 	var result = LinearSumAssignment(cost);
		// 	if (second_tracking_data_arr.Length < n_fish) {
		// 		var missed_indices = new List<int>();
		// 		for (int i = 0; i < first_tracking_data_arr.Length; i++) {
		// 			if (!result.Item1.Contains(i)) {
		// 				missed_indices.Add(i);
		// 			}
		// 		}
		// 		var missed_index = missed_index[0];
		// 		var merged_index = np.where(cost[missed_index] == np.min(cost[missed_index]))[0][0];
		// 		second_tracking_data_arr = np.append(second_tracking_data_arr, second_tracking_data_arr[merged_index]).reshape(-1, n_tracking_points, 2);
		// 		tail_curvature_arr = np.append(tail_curvature_arr, tail_curvature_arr[merged_index]).reshape(-1, n_tracking_points - 1);
		// 		var temp_result = FindIdentities(first_tracking_data_arr, second_tracking_data_arr, tail_curvature_arr, n_fish, n_tracking_points);
		// 		second_tracking_data_arr = temp_result.Item1;
		// 		tail_curvature_arr = temp_result.Item2;
		// 		result = temp_result.Item3;
		// 	}

		// 	return new Tuple(second_tracking_data_arr, tail_curvature_arr, result);
		// }



		// // def reorder_data_for_identities(tracking_data, tail_curvature, n_fish, n_tracking_points):
		// // 	new_tracking_data = np.zeros((len(tracking_data), n_fish, n_tracking_points, 2))
		// // 	new_tail_curvature = np.zeros((len(tail_curvature), n_fish, n_tracking_points - 1))
		// // 	for i in range(len(tracking_data) - 1):
		// // 	    if i == 0:
		// // 	        first_tracking_data_arr = tracking_data[0]
		// // 	        new_tracking_data[0] = tracking_data[0]
		// // 	        new_tail_curvature[0] = tail_curvature[0]
		// // 	    second_tracking_data_arr = tracking_data[i + 1]
		// // 	    tail_curvature_arr = tail_curvature[i + 1]
		// // 	    new_tracking_data_arr, new_tail_curvature_arr, new_order = find_identities(first_tracking_data_arr, second_tracking_data_arr, tail_curvature_arr, n_fish, n_tracking_points)
		// // 	    new_tracking_data[i + 1] = new_tracking_data_arr[new_order[1]]
		// // 	    new_tail_curvature[i + 1] = new_tail_curvature_arr[new_order[1]]
		// // 	    first_tracking_data_arr = new_tracking_data[i + 1]
		// // 	return [new_tracking_data, new_tail_curvature]

		// public List ReorderDataForIdentities(NDArray tracking_data, NDArray tail_curvature, int n_fish, int n_tracking_points) {
		// 	var new_tracking_data = np.zeros(tracking_data.Length, n_fish, n_tracking_points, 2);

		// 	for (int i = 0; i < tracking_data.Length - 1; i++) {
		// 		if (i == 0) {
		// 			var first_tracking_data_arr = tracking_data[0];
		// 			new_tracking_data[0] = tracking_data[0];
		// 			new_tail_curvature[0] = tail_curvature[0];
		// 		}
							
		// 		var second_tracking_data_arr = tracking_data[i + 1];
		// 		var tail_curvature_arr = tail_curvature[i + 1];
		// 		var temp_result = find_identities(first_tracking_data_arr, second_tracking_data_arr, tail_curvature_arr, n_fish, n_tracking_points);
		// 		second_tracking_data_arr = temp_result.Item1;
		// 		tail_curvature_arr = temp_result.Item2;
		// 		result = temp_result.Item3;
		// 		new_tracking_data[i + 1] = new_tracking_data_arr[new_order[1]];
		// 		new_tail_curvature[i + 1] = new_tail_curvature_arr[new_order[1]];
		// 		first_tracking_data_arr = new_tracking_data[i + 1];
		// 	}
					
		// 	// var ret = new List();
		// 	// ret.Add(new_tracking_data);
		// 	// ret.Add(new_tail_curvature);
		// 	return new List(){new_tracking_data, new_tail_curvature};
		// }
	}


}

