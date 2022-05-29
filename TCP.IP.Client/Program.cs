using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace TCP.IP.Client.Server
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DrawGraph();
            var t = GetCpuUsage();
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Client());
        }

        public static void DrawGraph()
        {
            //create a form 
            Form form = new Form();
            
            //create a viewer object 
            Microsoft.Msagl.GraphViewerGdi.GViewer viewer = new Microsoft.Msagl.GraphViewerGdi.GViewer();
            //create a graph object 
            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");
            //create the graph content 
            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;

            graph.FindNode("A").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
            graph.FindNode("B").Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;
            
            Microsoft.Msagl.Drawing.Node c = graph.FindNode("C");

            //bind the graph to the viewer 
            viewer.Graph = graph;
            //associate the viewer with the form 
            
            form.SuspendLayout();
            viewer.Dock = DockStyle.Fill;
            form.Controls.Add(viewer);
            form.ResumeLayout();
            //show the form 
            form.ShowDialog();
        }


        public static int GetCpuUsage()
        {
            PerformanceCounter cpuCounter;
            PerformanceCounter ramCounter;
            var name = Process.GetCurrentProcess().ProcessName;
            cpuCounter = new PerformanceCounter();
            cpuCounter.CategoryName = "Processor";
            cpuCounter.CounterName = "% Processor Time";
            cpuCounter.InstanceName = "_Total";
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "AsusLinkNear");// "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");

            var t = "Computer CPU Utilization rate:" + cpuCounter.NextValue() + "%";
            var tt = "The computer can use memory:" + ramCounter.NextValue() + "MB";

            while (true)
            {
                System.Threading.Thread.Sleep(1000);
                var ttt = "Computer CPU Utilization rate:" + cpuCounter.NextValue() + " %";
                var r = "The computer can use memory:" + ramCounter.NextValue() + "MB";

                if ((int)cpuCounter.NextValue() > 80)
                {
                    System.Threading.Thread.Sleep(1000 * 60);
                }
            }
        }
    }
}
