namespace TCP.IP.Client.Benchmarking
{
    public enum AcceleratorType
    {
        /// <summary>
        /// Represents a CPU accelerator.
        /// </summary>
        CPU,

        /// <summary>
        /// Represents a Cuda accelerator.
        /// </summary>
        Cuda,

        /// <summary>
        /// Represents an OpenCL accelerator (CPU/GPU via OpenCL).
        /// </summary>
        OpenCL,
    }
}
