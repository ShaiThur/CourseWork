namespace K_meansAlgorithmWebsite.Server
{
    public class ClusterService
    {
        public required double[][] RawData { get; set; }

        private int AttributesCount => RawData[0].Length;

        private int DataLength => RawData.Length;

        public int[] Cluster(int numClusters, int maxCount)
        {
            bool changed = true;
            int ct = 0;
            int[] clustering = InitClustering(numClusters, 0);
            double[][] means = Allocate(numClusters);
            double[][] centroids = Allocate(numClusters);
            UpdateMeans(clustering, means);
            UpdateCentroids(clustering, means, centroids);

            while (changed == true && ct < maxCount)
            {
                ++ct;
                changed = Assign(clustering, centroids);
                UpdateMeans(clustering, means);
                UpdateCentroids(clustering, means, centroids);
            }

            return clustering;
        }

        private int[] InitClustering(int numClusters, int randomSeed)
        {
            Random random = new Random(randomSeed);
            int[] clustering = new int[DataLength];

            for (int i = 0; i < numClusters; ++i)
                clustering[i] = i;

            for (int i = numClusters; i < clustering.Length; i++)
                clustering[i] = random.Next(0, numClusters);

            return clustering;
        }

        private double[][] Allocate(int numClusters)
        {
            double[][] result = new double[numClusters][];

            for (int k = 0; k < numClusters; ++k)
                result[k] = new double[AttributesCount];

            return result;
        }

        private void UpdateMeans(int[] clustering, double[][] means)
        {
            int numClusters = means.Length;

            for (int k = 0; k < means.Length; ++k)
                for (int j = 0; j < means[k].Length; ++j)
                    means[k][j] = 0.0;
            int[] clusterCounts = new int[numClusters];
            for (int i = 0; i < DataLength; ++i)
            {
                int cluster = clustering[i];
                ++clusterCounts[cluster];
                for (int j = 0; j < RawData[i].Length; ++j)
                    means[cluster][j] += RawData[i][j];
            }
            for (int k = 0; k < means.Length; ++k)
                for (int j = 0; j < means[k].Length; ++j)
                    means[k][j] /= clusterCounts[k];
        }

        private void UpdateCentroids(int[] clustering, double[][] means, double[][] centroids)
        {
            for (int k = 0; k < centroids.Length; ++k)
            {
                double[] centroid = ComputeCentroid(clustering, k, means);
                centroids[k] = centroid;
            }
        }

        private double[] ComputeCentroid(int[] clustering, int cluster, double[][] means)
        {
            int numAttributes = means[0].Length;
            double[] centroid = new double[numAttributes];
            double minDistance = double.MaxValue;

            for (int i = 0; i < DataLength; ++i)
            {
                int c = clustering[i];
                if (c != cluster) continue;
                double currentDistance = GetDistance(ref RawData[i], ref means[cluster]);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                    for (int j = 0; j < centroid.Length; ++j)
                        centroid[j] = RawData[i][j];
                }
            }
            return centroid;
        }

        private bool Assign(int[] clustering, double[][] centroids)
        {
            int numClusters = centroids.Length;
            bool changed = false;
            double[] distances = new double[numClusters];
            for (int i = 0; i < DataLength; ++i)
            {
                for (int k = 0; k < numClusters; ++k)
                    distances[k] = GetDistance(ref RawData[i], ref centroids[k]);
                int newCluster = GetMinIndex(ref distances);
                if (newCluster != clustering[i])
                {
                    changed = true;
                    clustering[i] = newCluster;
                }
            }
            return changed;
        }

        private static double GetDistance(ref double[] tuple, ref double[] vector)
        {
            double sumSquaredDiffs = 0.0;

            for (int j = 0; j < tuple.Length; ++j)
                sumSquaredDiffs += Math.Pow((tuple[j] - vector[j]), 2);

            return Math.Sqrt(sumSquaredDiffs);
        }

        private static int GetMinIndex(ref double[] distances)
        {
            int indexOfMin = 0;
            double minDistance = distances[0];

            for (int k = 0; k < distances.Length; ++k)
            {
                if (distances[k] < minDistance)
                {
                    minDistance = distances[k];
                    indexOfMin = k;
                }
            }
            return indexOfMin;
        }
    }
}
