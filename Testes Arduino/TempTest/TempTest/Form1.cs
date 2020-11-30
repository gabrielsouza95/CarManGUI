using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace TempTest
{
    public partial class TestTemp : Form
    {
        static SerialPort serialPort1 = new SerialPort();

        static string s = ""; //recebe os dados da serial
        private static String[] sepearator = { "_", ":", "=" }; //marcadores para diferenciar as informações vindas do Arduino
        private static String[] strlist = new String[30]; //strings para receber as partes da msg enviada pelo Arduino
        private static int strNr = 4; //quantidade de strings para passar a função Split 


        private delegate void UpdateStatusDelegate();
        private UpdateStatusDelegate updateStatusDelegate = null;
        private event EventHandler NewSensorDataReceived;

        //private static ProgressBar[] = new ProgressBar;
            //{pbMLX1,pbMLX2,pbMLX3,pbMLX4};

        public TestTemp()
        {
            InitializeComponent();
            pegaNomesPortasSerial();
            rtbSerialOutput.Text = "Iniciado";
            this.updateStatusDelegate = new UpdateStatusDelegate(this.UpdateGUI_Log);
        }

        private void UpdateFields(string pStr, int pCount)
        {
            if (tabControl1.SelectedTab == tabControl1.TabPages["tpTempGauges"])
                switch (pCount)
                {
                    case 0:
                        tbMLX1.Text = (Math.Round(float.Parse(pStr),1)).ToString() + "°C";
                        pbMLX1.Value = (int)float.Parse(pStr);
                        break;
                    case 1:
                        tbMLX2.Text = (Math.Round(float.Parse(pStr), 1)).ToString() + "°C";
                        pbMLX2.Value = (int)float.Parse(pStr);
                        break;
                    case 2:
                        tbMLX3.Text = (Math.Round(float.Parse(pStr), 1)).ToString() + "°C";
                        pbMLX3.Value = (int)float.Parse(pStr);
                        break;
                    case 3:
                        tbMLX4.Text = (Math.Round(float.Parse(pStr), 1)).ToString() + "°C";
                        pbMLX4.Value = (int)float.Parse(pStr);
                        break;
                }
        }

        private void UpdateGUI_Log()
        {
            //--- Updating Data fields
            rtbSerialOutput.Text = s;
            //rtbTesteNros.Text = "";
            int count = 0;

            // using the method 
            strlist = s.Split(sepearator, strNr,
                   StringSplitOptions.RemoveEmptyEntries);

            foreach (String str in strlist)
            {
                UpdateFields(str, count);

                count++;
            }

        }

        private void arduinoBoard_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort1.IsOpen && serialPort1 != null)
            {
                this.Invoke(this.updateStatusDelegate);
                s = serialPort1.ReadLine();

                NewSensorDataReceived?.Invoke(this, new EventArgs()); //Fire the event, indicating that new WeatherData was added to the list.
            }
        }

        private void pegaNomesPortasSerial() //carrega as portas seriais disponíveis
        {
            String[] ports = SerialPort.GetPortNames(); //escolher a ACM0
            cbSerialPorts.Items.AddRange(ports);
        }

        private void butInicSerial_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbSerialPorts.Text != "")
                {
                    if (serialPort1 == null)
                    {
                        serialPort1 = new SerialPort();
                    }
                    if (!serialPort1.IsOpen && serialPort1 != null)
                    {
                        serialPort1.DataReceived += arduinoBoard_DataReceived;
                        serialPort1.PortName = cbSerialPorts.Text;
                        serialPort1.BaudRate = 9600;
                        serialPort1.Open();
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                rtbSerialOutput.Text = "***--- Exception: Unauthorized Acess ---***";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                }
            }
            catch (UnauthorizedAccessException)
            {
                rtbSerialOutput.Text = "***--- Exception: Unauthorized Acess ---***";
            }
        }
    }
}
