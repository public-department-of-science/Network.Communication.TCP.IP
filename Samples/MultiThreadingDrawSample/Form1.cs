using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiThreadingDrawSample
{
    public partial class Form1 : Form
    {
        Random random;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnThread1_Click(object sender, EventArgs e)
        {
            int pixelSize = 10;
            Thread thread = new Thread(t =>
            {
                for (int i = 0; i < 100; i++)
                {
                    int width = random.Next(0, this.Width);
                    int height = random.Next(0, this.Height);
                    this.CreateGraphics().DrawEllipse(new Pen(Brushes.Red, 1), new Rectangle(width, height, pixelSize, pixelSize));
                    Thread.Sleep(100);
                }
            })
            { IsBackground = true };

            thread.Start();
        }

        private void btnThread2_Click(object sender, EventArgs e)
        {
            int pixelSize = 10;
            Thread thread = new Thread(t =>
            {
                for (int i = 0; i < 100; i++)
                {
                    int width = random.Next(0, this.Width);
                    int height = random.Next(0, this.Height);
                    this.CreateGraphics().DrawEllipse(new Pen(Brushes.Blue, 1), new Rectangle(width, height, pixelSize, pixelSize));
                    Thread.Sleep(100);
                }
            })
            { IsBackground = true };

            thread.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            random = new Random();
        }
    }
}
