using ILGPU;
using ILGPU.Runtime;
using OxyPlot;
using OxyPlot.Series;
using PC.Calculation.Performance.Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCP.IP.Client
{
    public partial class PCPerformanceTestForm : Form
    {
        private const int oneSecond_Ticks = 10_000;

        public PCPerformanceTestForm()
        {
            InitializeComponent();
        }

        private async void btnGetDevices_Click(object sender, EventArgs e)
        {
            chLstBox.Items.Clear();
            btnGetDevices.Enabled = false;

            await Task.Run(() =>
            {
                using (var context = Context.CreateDefault())
                {
                    // For each available device...
                    foreach (var device in context)
                    {
                        // Create accelerator for the given device.
                        // Note that all accelerators have to be disposed before the global context is disposed
                        using var accelerator = device.CreateAccelerator(context);

                        this.Invoke((MethodInvoker)delegate
                        {
                            chLstBox.Items.Add($"Accelerator: {device.AcceleratorType}, {accelerator.Name}");
                        });

                        // PrintAcceleratorInfo(accelerator);
                        Console.WriteLine();
                    }
                }
            });
            btnGetDevices.Enabled = true;
        }

        private async void btnRunTest_Click(object sender, EventArgs e)
        {
            btnRunTest.Enabled = false;

            int.TryParse(txtBox_runMaxTime.Text, out int iterationRunTimeMlSec);

            await Task.Run(() =>
            {
                BenchmarkContext benchmarkContext = new BenchmarkContext();
                var data = benchmarkContext.Run(iterationRunTimeMlSec);

                var myModel = new PlotModel { Title = "Benchmark experiment run" };

                foreach (var iteration in data)
                {
                    var buildLine = new LineSeries()
                    {
                        Color = OxyColor.Parse(iteration.GraphicColor),
                        SeriesGroupName = iteration.CurrentRunName,
                        Title = iteration.CurrentRunName,
                        LegendKey = iteration.CurrentRunName,
                        LineLegendPosition = LineLegendPosition.End,
                    };
                    foreach (var item in iteration.iteration)
                    {
                        buildLine.Points.Add(new DataPoint(item.Key, item.Value / oneSecond_Ticks));
                    }
                    myModel.Series.Add(buildLine);
                }

                this.Invoke((MethodInvoker)delegate
                {
                    this.plotViewIterations.Model = myModel;
                    btnRunTest.Enabled = true;
                });

                // txtBoxMeasurementsInfo.Text += benchmarkResults;
            });
        }

        private void btnRunTest_Click(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!char.IsDigit(ch) && ch != 8 && ch != 46)
            {
                e.Handled = true;
            }
        }

        private void txtBox_runMaxTime_TextChanged(object sender, EventArgs e)
        {
            int.TryParse(txtBox_runMaxTime.Text, out int result);
            txtBox_iterationMaxRunTimeInSec.Text = (result / 1_000).ToString();
        }

        private void PCPerformanceTestForm_Load(object sender, EventArgs e)
        {
            txtBox_runMaxTime_TextChanged(sender, e);
        }

        //static void PrintAcceleratorInfo(Accelerator accelerator)
        //{
        //    Console.WriteLine($"Name: {accelerator.Name}");
        //    Console.WriteLine($"MemorySize: {accelerator.MemorySize}");
        //    Console.WriteLine($"MaxThreadsPerGroup: {accelerator.MaxNumThreadsPerGroup}");
        //    Console.WriteLine($"MaxSharedMemoryPerGroup: {accelerator.MaxSharedMemoryPerGroup}");
        //    Console.WriteLine($"MaxGridSize: {accelerator.MaxGridSize}");
        //    Console.WriteLine($"MaxConstantMemory: {accelerator.MaxConstantMemory}");
        //    Console.WriteLine($"WarpSize: {accelerator.WarpSize}");
        //    Console.WriteLine($"NumMultiprocessors: {accelerator.NumMultiprocessors}");
        //}
    }
}
