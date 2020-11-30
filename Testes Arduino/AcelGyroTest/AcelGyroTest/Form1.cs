using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace AcelGyroTest
{
    public partial class TestAcelGyro : Form
    {
        static SerialPort serialPort1 = new SerialPort();
        static bool First = true;

        static string s = ""; //recebe os dados da serial
        private static String[] sepearator = { "_", ":", "=" }; //marcadores para diferenciar as informações vindas do Arduino
        private static String[] strlist = new String[18]; //strings para receber as partes da msg enviada pelo Arduino
        private static int strNr = 18; //quantidade de strings para passar a função Split 
        
        private delegate void UpdateStatusDelegate();
        private UpdateStatusDelegate updateStatusDelegate = null;
        private event EventHandler NewSensorDataReceived;

        long[] mpuax = { 0, 0, 0 },
               mpuay = { 0, 0, 0 },
               mpuaz = { 0, 0, 0 },
               mpugx = { 0, 0, 0 },
               mpugy = { 0, 0, 0 },
               mpugz = { 0, 0, 0 };

        public TestAcelGyro()
        {
            InitializeComponent();
            pegaNomesPortasSerial();
            rtbSerialOutput.Text = "Iniciado";
            this.updateStatusDelegate = new UpdateStatusDelegate(this.UpdateGUI_Log);
        }

        private void UpdateFields(string pStr, int pCount)
        {
            try { 
                switch (pCount)
                {
                    case 0: // A1 MPU 1
                        mpuax[0] = int.Parse(pStr, System.Globalization.NumberStyles.AllowLeadingSign);
                        break;
                    case 1: // A2 MPU 1
                        mpuay[0] = int.Parse(pStr, System.Globalization.NumberStyles.AllowLeadingSign);
                        break;
                    case 2: // A3 MPU 1
                        mpuaz[0] = int.Parse(pStr, System.Globalization.NumberStyles.AllowLeadingSign);
                        break;
                    case 3: // G1 MPU 1
                        mpugx[0] = int.Parse(pStr, System.Globalization.NumberStyles.AllowLeadingSign);
                        break;
                    case 4: // G2 MPU 1
                        mpugy[0] = int.Parse(pStr, System.Globalization.NumberStyles.AllowLeadingSign);
                        break;
                    case 5: // G3 MPU 1
                        mpugz[0] = int.Parse(pStr, System.Globalization.NumberStyles.AllowLeadingSign);
                        break;
                    case 6: // A1 MPU 2
                        mpuax[1] = int.Parse(pStr, System.Globalization.NumberStyles.AllowLeadingSign);
                        break;
                    case 7: // A2 MPU 2
                        mpuay[1] = int.Parse(pStr, System.Globalization.NumberStyles.AllowLeadingSign);
                        break;
                    case 8: // A3 MPU 2
                        mpuaz[1] = int.Parse(pStr, System.Globalization.NumberStyles.AllowLeadingSign);
                        break;
                    case 9: // G1 MPU 2
                        mpugx[1] = int.Parse(pStr, System.Globalization.NumberStyles.AllowLeadingSign);
                        break;
                    case 10: // G2 MPU 2
                        mpugy[1] = int.Parse(pStr, System.Globalization.NumberStyles.AllowLeadingSign);
                        break;
                    case 11: // G3 MPU 2
                        mpugz[1] = int.Parse(pStr, System.Globalization.NumberStyles.AllowLeadingSign);
                        break;
                }
                
                if (tcTabs.SelectedTab == tcTabs.TabPages["tpMPU1Acel"])
                {
                    chartAcel1MPU1.Series["MPU1_Acel1"].Points.Clear();
                    chartAcel2MPU1.Series["MPU1_Acel2"].Points.Clear();
                    chartAcel3MPU1.Series["MPU1_Acel3"].Points.Clear();
                    chartAcel1MPU1.Series["MPU1_Acel1"].Points.AddXY(mpuax[0], mpuay[0]);
                    chartAcel2MPU1.Series["MPU1_Acel2"].Points.AddXY(mpuay[0], mpuaz[0]);
                    chartAcel3MPU1.Series["MPU1_Acel3"].Points.AddXY(mpuax[0], mpuaz[0]);
                }
                if (tcTabs.SelectedTab == tcTabs.TabPages["tpMPU2Acel"])
                {
                    chartAcel1MPU2.Series["MPU2_Acel1"].Points.Clear();
                    chartAcel2MPU2.Series["MPU2_Acel2"].Points.Clear();
                    chartAcel3MPU2.Series["MPU2_Acel3"].Points.Clear();
                    chartAcel1MPU2.Series["MPU2_Acel1"].Points.AddXY(mpuax[1], mpuay[1]);
                    chartAcel2MPU2.Series["MPU2_Acel2"].Points.AddXY(mpuay[1], mpuaz[1]);
                    chartAcel3MPU2.Series["MPU2_Acel3"].Points.AddXY(mpuax[1], mpuaz[1]);
                }
                if (tcTabs.SelectedTab == tcTabs.TabPages["tpMPU3Acel"])
                {
                    chartAcel1MPU3.Series["MPU3_Acel1"].Points.Clear();
                    chartAcel2MPU3.Series["MPU3_Acel2"].Points.Clear();
                    chartAcel3MPU3.Series["MPU3_Acel3"].Points.Clear();
                    chartGyro1MPU3.Series["MPU3_Acel1"].Points.AddXY(mpuax[2], mpuay[2]);
                    chartGyro2MPU3.Series["MPU3_Acel2"].Points.AddXY(mpuay[2], mpuaz[2]);
                    chartGyro3MPU3.Series["MPU3_Acel3"].Points.AddXY(mpuax[2], mpuaz[2]);
                }
                if (tcTabs.SelectedTab == tcTabs.TabPages["tpMPU1Gyro"])
                {
                    chartGyro1MPU1.Series["MPU1_Gyro1"].Points.Clear();
                    chartGyro2MPU1.Series["MPU1_Gyro2"].Points.Clear();
                    chartGyro3MPU1.Series["MPU1_Gyro3"].Points.Clear();
                    chartGyro1MPU1.Series["MPU1_Gyro1"].Points.AddXY(mpugx[0], mpugy[0]);
                    chartGyro2MPU1.Series["MPU1_Gyro2"].Points.AddXY(mpugy[0], mpugz[0]);
                    chartGyro3MPU1.Series["MPU1_Gyro3"].Points.AddXY(mpugx[0], mpugz[0]);
                }
                if (tcTabs.SelectedTab == tcTabs.TabPages["tpMPU2Gyro"])
                {
                    chartGyro1MPU2.Series["MPU2_Gyro1"].Points.Clear();
                    chartGyro2MPU2.Series["MPU2_Gyro2"].Points.Clear();
                    chartGyro3MPU2.Series["MPU2_Gyro3"].Points.Clear();
                    chartGyro1MPU2.Series["MPU2_Gyro1"].Points.AddXY(mpugx[1], mpugy[1]);
                    chartGyro2MPU2.Series["MPU2_Gyro2"].Points.AddXY(mpugy[1], mpugz[1]);
                    chartGyro3MPU2.Series["MPU2_Gyro3"].Points.AddXY(mpugx[1], mpugz[1]);
                }
                if (tcTabs.SelectedTab == tcTabs.TabPages["tpMPU3Gyro"])
                {
                    chartGyro1MPU3.Series["MPU3_Gyro1"].Points.Clear();
                    chartGyro2MPU3.Series["MPU3_Gyro2"].Points.Clear();
                    chartGyro3MPU3.Series["MPU3_Gyro3"].Points.Clear();
                    chartGyro1MPU3.Series["MPU3_Gyro1"].Points.AddXY(mpugx[2], mpugy[2]);
                    chartGyro2MPU3.Series["MPU3_Gyro2"].Points.AddXY(mpugy[2], mpugz[2]);
                    chartGyro3MPU3.Series["MPU3_Gyro3"].Points.AddXY(mpugx[2], mpugz[2]);
                }
            } catch (Exception e)
            {
                rtbSerialOutput.Text = e.ToString();
            }
        }

        private void UpdateGUI_Log()
        {
            if (!First)
            {
                //--- Updating Data fields
                rtbSerialOutput.Text = s;
                rtbTesteNros.Text = "";
                int count = 0;

                // using the method 
                strlist = s.Split(sepearator, strNr,
                       StringSplitOptions.RemoveEmptyEntries);

                foreach (String str in strlist)
                {
                    rtbTesteNros.Text += count.ToString();
                    rtbTesteNros.Text += " - ";
                    rtbTesteNros.Text += str;
                    rtbTesteNros.Text += "\n";
                    UpdateFields(str, count);

                    count++;
                }
            }
            else
                First = false;
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

        private void butTermSerial_Click(object sender, EventArgs e)
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
