namespace K_meansAlgorithmWebsite.Server.Models
{
    public class DataRequest
    {
        public required double[][] RawData { get; set; }
        
        public required int ClustersCount { get; set; }
    }
}
