using System;

namespace TCP.IP.Client
{
    internal class ClientScoreConfiguration
    {
        public int CPU_Score { get; set; }
        public int GPU_Score { get; set; }
        public int CPU_GPU_BuiltIn_Score { get; set; }

        public string TaskType { get; set; }
        public DateTime ExecutionTimeMark { get; set; }
    }
}
