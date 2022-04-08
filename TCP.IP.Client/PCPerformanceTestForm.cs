﻿using ILGPU;
using ILGPU.Runtime;
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
                        chLstBox.Items.Add($"Accelerator: {device.AcceleratorType}, {accelerator.Name}");
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
            await Task.Run(() =>
            {
                var benchmarkResults = MatrixMultiplyBenchmark.MatrixMultiplyBenchmarkRun();
                txtBoxMeasurementsInfo.Text += benchmarkResults;
                btnRunTest.Enabled = true;
            });
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