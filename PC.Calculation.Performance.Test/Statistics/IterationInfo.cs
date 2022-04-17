namespace TCP.IP.Client
{
    public class IterationInfo
    {
        public Dictionary<int, long> iteration { get; } = new Dictionary<int, long>();
        public string CurrentRunName { get; set; }

        public string GraphicColor { get; } = "255,255,255,255,50";

        public int MaxMatrixSize { get; }

        public IterationInfo() { }

        public IterationInfo(string currentRunName, Dictionary<int, long> currentIteration, string graphicColor, int maxMatrixSize)
        {
            CurrentRunName = currentRunName;
            iteration = currentIteration;
            GraphicColor = graphicColor;
            MaxMatrixSize = maxMatrixSize;
        }
    }
}
