using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO.Ports;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        private int algorithmCount = 0;
        private int algorithmNumber;

        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            int num;
            comboBox1.Items.Clear();
            string[] ports = SerialPort.GetPortNames().OrderBy(a => a.Length > 3 && int.TryParse(a.Substring(3), out num) ? num : 0).ToArray();
            comboBox1.Items.AddRange(ports);
        }

        private void buttonOpenPort_Click(object sender, EventArgs e)
        {
            if (!serialPort1.IsOpen)
                try
                {
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.Open();
                    buttonOpenPort.Text = "Close";
                    comboBox1.Enabled = false;
                    button1.Visible = true;
                    button2.Visible = true;
                }
                catch
                {
                    MessageBox.Show("Port " + comboBox1.Text + " is invalid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            else
            {
                serialPort1.Close();
                buttonOpenPort.Text = "Open";
                comboBox1.Enabled = true;
                button1.Visible = false;
                button2.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] b1 = new byte[1];
            b1[0] = 0xA1;
            serialPort1.Write(b1, 0, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] b1 = new byte[1];
            b1[0] = 0xB1;
            serialPort1.Write(b1, 0, 1);
        }

        private void clearAllLed()
        {
            panel1.BackColor = Color.SkyBlue;
            panel2.BackColor = Color.SkyBlue;
            panel3.BackColor = Color.SkyBlue;
            panel4.BackColor = Color.SkyBlue;
            panel5.BackColor = Color.SkyBlue;
            panel6.BackColor = Color.SkyBlue;
            panel7.BackColor = Color.SkyBlue;
            panel8.BackColor = Color.SkyBlue;
        }

        private void startTimer()
        {
            timer1.Start();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            Panel[] panels = new Panel[8] { panel1, panel2, panel3, panel4, panel5, panel6, panel7, panel8 };
            algorithmCount++;
            if (algorithmCount < 2)
            {
                if (algorithmCount % 2 == 1)
                {
                    if (algorithmNumber == 1)
                    {
                        for (int i = 0; i < panels.Length / 2; i++)
                        {
                            panels[i].BackColor = Color.Green;
                            panels[panels.Length - i - 1].BackColor = Color.Green;
                            await Task.Delay(700);
                            panels[i].BackColor = Color.SkyBlue;
                            panels[panels.Length - i - 1].BackColor = Color.SkyBlue;
                        }
                        clearAllLed();
                    }
                    else if (algorithmNumber == 2)
                    {
                        for (int i = 0; i < panels.Length; i += 2)
                        {
                            panels[i].BackColor = Color.Green;
                            await Task.Delay(700);
                            panels[i].BackColor = Color.SkyBlue;
                        }
                        for (int i = 1; i < panels.Length; i += 2)
                        {
                            panels[i].BackColor = Color.Green;
                            await Task.Delay(700);
                            panels[i].BackColor = Color.SkyBlue;
                        }
                        clearAllLed();
                    }
                }
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte commandFromArduino = (byte)serialPort1.ReadByte();
            if (commandFromArduino == 0xA1)
            {
                algorithmNumber = 1;
                algorithmCount = 0;
                this.BeginInvoke(new ThreadStart(startTimer));

            }
            if (commandFromArduino == 0xB1)
            {
                algorithmNumber = 2;
                algorithmCount = 0;
                this.BeginInvoke(new ThreadStart(startTimer));
            }
        }
    }
}
