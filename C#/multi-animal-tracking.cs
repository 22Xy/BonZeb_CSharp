namespace BonZeb {

	public class multiAnimalTracking {

		public double[] GetColumn(double[,] matrix, int columnNumber) {
				return Enumerable.Range(0, matrix.GetLength(0))
								.Select(x => matrix[x, columnNumber])
								.ToArray();
		}

		public double[,] cdist(double[] x, double[] y) {

			// TODO

			return new double[,];
		}

		public Tuple LinearSumAssignment(double[,] matrix) {

			// TODO
			List<double> row_ind = new double[];
			List<double> col_ind = new double[];

			return new Tuple(row_ind, col_ind);
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

		public Tuple FindIdentities(NDArray first_tracking_data_arr, NDArray second_tracking_data_arr, NDArray tail_curvature_arr, int n_fish, int n_tracking_points) {
			var cost = cdist(GetColumn(first_tracking_data_arr, 0), GetColumn(second_tracking_data_arr, 0));
			var result = LinearSumAssignment(cost);
			if (second_tracking_data_arr.Length < n_fish) {
				var missed_indices = new List<int>();
				for (int i = 0; i < first_tracking_data_arr.Length; i++) {
					if (!result.Item1.Contains(i)) {
						missed_indices.Add(i);
					}
				}
				var missed_index = missed_index[0];
				var merged_index = np.where(cost[missed_index] == np.min(cost[missed_index]))[0][0];
				second_tracking_data_arr = np.append(second_tracking_data_arr, second_tracking_data_arr[merged_index]).reshape(-1, n_tracking_points, 2);
				tail_curvature_arr = np.append(tail_curvature_arr, tail_curvature_arr[merged_index]).reshape(-1, n_tracking_points - 1);
				var temp_result = FindIdentities(first_tracking_data_arr, second_tracking_data_arr, tail_curvature_arr, n_fish, n_tracking_points);
				second_tracking_data_arr = temp_result.Item1;
				tail_curvature_arr = temp_result.Item2;
				result = temp_result.Item3;
			}

			return new Tuple(second_tracking_data_arr, tail_curvature_arr, result);
		}



		// def reorder_data_for_identities(tracking_data, tail_curvature, n_fish, n_tracking_points):
		// 	new_tracking_data = np.zeros((len(tracking_data), n_fish, n_tracking_points, 2))
		// 	new_tail_curvature = np.zeros((len(tail_curvature), n_fish, n_tracking_points - 1))
		// 	for i in range(len(tracking_data) - 1):
		// 	    if i == 0:
		// 	        first_tracking_data_arr = tracking_data[0]
		// 	        new_tracking_data[0] = tracking_data[0]
		// 	        new_tail_curvature[0] = tail_curvature[0]
		// 	    second_tracking_data_arr = tracking_data[i + 1]
		// 	    tail_curvature_arr = tail_curvature[i + 1]
		// 	    new_tracking_data_arr, new_tail_curvature_arr, new_order = find_identities(first_tracking_data_arr, second_tracking_data_arr, tail_curvature_arr, n_fish, n_tracking_points)
		// 	    new_tracking_data[i + 1] = new_tracking_data_arr[new_order[1]]
		// 	    new_tail_curvature[i + 1] = new_tail_curvature_arr[new_order[1]]
		// 	    first_tracking_data_arr = new_tracking_data[i + 1]
		// 	return [new_tracking_data, new_tail_curvature]

		public List ReorderDataForIdentities(NDArray tracking_data, NDArray tail_curvature, int n_fish, int n_tracking_points) {
			var new_tracking_data = np.zeros(tracking_data.Length, n_fish, n_tracking_points, 2);

			for (int i = 0; i < tracking_data.Length - 1; i++) {
				if (i == 0) {
					var first_tracking_data_arr = tracking_data[0];
					new_tracking_data[0] = tracking_data[0];
					new_tail_curvature[0] = tail_curvature[0];
				}
							
				var second_tracking_data_arr = tracking_data[i + 1];
				var tail_curvature_arr = tail_curvature[i + 1];
				var temp_result = find_identities(first_tracking_data_arr, second_tracking_data_arr, tail_curvature_arr, n_fish, n_tracking_points);
				second_tracking_data_arr = temp_result.Item1;
				tail_curvature_arr = temp_result.Item2;
				result = temp_result.Item3;
				new_tracking_data[i + 1] = new_tracking_data_arr[new_order[1]];
				new_tail_curvature[i + 1] = new_tail_curvature_arr[new_order[1]];
				first_tracking_data_arr = new_tracking_data[i + 1];
			}
					
			// var ret = new List();
			// ret.Add(new_tracking_data);
			// ret.Add(new_tail_curvature);
			return new List(){new_tracking_data, new_tail_curvature};
		}



	}



}

