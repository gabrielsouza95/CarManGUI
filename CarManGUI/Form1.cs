using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace CarManGUI
{
    public partial class Form1 : Form
    {
        //--- Serial related 
        //inicia processo através da classe no contexto global para poder manter ele rodando e atualizar a janela
        //static ClassSerial serial = new ClassSerial(); //classe com a parte de comunicação serial
        static SerialPort serialPort1 = new SerialPort();

        static string s = "" ; //recebe os dados da serial
        //--- Serial related


        //--- Serial Event Thread related

        // Declare a delegate used to communicate with the UI thread from the Serial Event
        private delegate void UpdateStatusDelegate();
        private UpdateStatusDelegate updateStatusDelegate = null;
        private event EventHandler NewSensorDataReceived;
        //-- Serial Event Thread related


        //--- Data received related
        //lida com a string recebida para converte-la
        private static String[] sepearator = { "_", ":", "=" }; //marcadores para diferenciar as informações vindas do Arduino
        private static String[] strlist = new String[30]; //strings para receber as partes da msg enviada pelo Arduino
        private static int strNr = 30, //quantidade de strings para passar a função Split 
                           count = 0;//, //indica qual das informações está sendo passada para a função que atualiza a GUI
                           //valor = 0; //variável para guardar o valor convertido da String
        //private static double valorConvertido = 0.0; //variável que guarda o valor que foi convertido do valor bruto do sensor para um valor entendível
        //--- valores MPU
        private long[] mpuax = { 0, 0, 0 },
                       mpuay = { 0, 0, 0 },
                       mpuaz = { 0, 0, 0 },
                       mpugx = { 0, 0, 0 },
                       mpugy = { 0, 0, 0 },
                       mpugz = { 0, 0, 0 };
        private static short MPU1 = 0,
                             MPU2 = 1,
                             MPU3 = 2;

        //--- valores MPU
        //--- valores MLX
        private double[] temp = { 0, 0, 0, 0 };
        private static short MLX1 = 0,
                             MLX2 = 1,
                             MLX3 = 2,
                             MLX4 = 4;
        //--- valores MLX
        //--- valores potenc susp
        private int[] susp = { 0, 0, 0, 0 };
        private static short SUSP1 = 0,
                             SUSP2 = 1,
                             SUSP3 = 2,
                             SUSP4 = 3;
        //--- valores potenc susp
        //--- valores vel rodas
        private int[] vel = { 0, 0, 0, 0 };
        private static short RODA1 = 0,
                             RODA2 = 1,
                             RODA3 = 2,
                             RODA4 = 4;
        //--- valores vel rodas
        //--- Data received related

        //--- File related
        static StreamWriter sw = null;
        static bool writeFile = false;
        static bool rewriteFile = false;
        static bool recHasChanged = false;
        //--- File related

        public Form1()
        {
            InitializeComponent(); // inicializa os componentes da janela
            pegaNomesPortasSerial();
            tsmiStopSerial.Enabled = false;
            rtbSerialOutput.Text = "Iniciado";
            // Initialise the delegate
            this.updateStatusDelegate = new UpdateStatusDelegate(this.UpdateGUI_Log);
        }

        //

        private void UpdateFields(string pStr)
        {
            switch (count) //lê na ordem que é mandada pelo programa no arduino
            {
                //MPU 6050 1
                case 0: mpuax[MPU1] = Int32.Parse(pStr); break;//accel x mpu 1
                case 1: mpuay[MPU1] = Int32.Parse(pStr); break;//accel y mpu 1
                case 2: mpuaz[MPU1] = Int32.Parse(pStr); break;//accel z mpu 1
                case 3: mpugx[MPU1] = Int32.Parse(pStr); break;//gyro x mpu 1
                case 4: mpugy[MPU1] = Int32.Parse(pStr); break;//gyro y mpu 1
                case 5: mpugz[MPU1] = Int32.Parse(pStr); break;//gyro z mpu 1
                //MPU 6050 2
                case 6: mpuax[MPU2] = Int32.Parse(pStr); break;//accel x mpu 2
                case 7: mpuay[MPU2] = Int32.Parse(pStr); break;//accel y mpu 2
                case 8: mpuaz[MPU2] = Int32.Parse(pStr); break;//accel z mpu 2
                case 9: mpugx[MPU2] = Int32.Parse(pStr); break;//gyro x mpu 2
                case 10: mpugy[MPU2] = Int32.Parse(pStr); break;//gyro y mpu 2
                case 11: mpugz[MPU2] = Int32.Parse(pStr); break;//gyro z mpu 2
                //MPU 6050 3
                case 12: mpuax[MPU3] = Int32.Parse(pStr); break;//accel x mpu 3
                case 13: mpuay[MPU3] = Int32.Parse(pStr); break;//accel y mpu 3
                case 14: mpuaz[MPU3] = Int32.Parse(pStr); break;//accel z mpu 3
                case 15: mpugx[MPU3] = Int32.Parse(pStr); break;//gyro x mpu 3
                case 16: mpugy[MPU3] = Int32.Parse(pStr); break;//gyro y mpu 3
                case 17: mpugz[MPU3] = Int32.Parse(pStr); break;//gyro z mpu 3
                //MLX 90614 (all temp disc)
                case 18: temp[MLX1] = float.Parse(pStr); break;//mlx 1 FR
                case 19: temp[MLX2] = float.Parse(pStr); break;//mlx 2 FL
                case 20: temp[MLX3] = float.Parse(pStr); break;//mlx 3 RR
                case 21: temp[MLX4] = float.Parse(pStr); break;//mlx 4 RL
                //potenciometros suspensão(todos)
                case 22: susp[SUSP1] = Int32.Parse(pStr); break;//potenciometro FR
                case 23: susp[SUSP2] = Int32.Parse(pStr); break;//potenciometro FL
                case 24: susp[SUSP3] = Int32.Parse(pStr); break;//potenciometro RR
                case 25: susp[SUSP4] = Int32.Parse(pStr); break;//potenciometro RL
                //ky003 all vel rodas
                case 26: vel[RODA1] = Int32.Parse(pStr); break;//vel roda FR
                case 27: vel[RODA2] = Int32.Parse(pStr); break;//vel roda FL
                case 28: vel[RODA3] = Int32.Parse(pStr); break;//vel roda RR
                case 29: vel[RODA4] = Int32.Parse(pStr); break;//vel roda RL
            }
            if (tcTelas.SelectedTab == tcTelas.TabPages["tpAccels"])
            {//valorConvertido = valor / convertAccel;// valorConvertido.ToString("F");
                tbAccelX1.Text = mpuax[MPU1].ToString();
                tbAccelY1.Text = mpuay[MPU1].ToString();
                tbAccelZ1.Text = mpuaz[MPU1].ToString();
                tbAccelX2.Text = mpuax[MPU2].ToString();
                tbAccelY2.Text = mpuay[MPU2].ToString();
                tbAccelZ2.Text = mpuaz[MPU2].ToString();
                tbAccelX3.Text = mpuax[MPU3].ToString();
                tbAccelY3.Text = mpuay[MPU3].ToString();
                tbAccelZ3.Text = mpuaz[MPU3].ToString();
            }
            if (tcTelas.SelectedTab == tcTelas.TabPages["tpGyros"])
            {
                tbGyroX1.Text = mpugx[MPU1].ToString();
                tbGyroY1.Text = mpugy[MPU1].ToString();
                tbGyroZ1.Text = mpugz[MPU1].ToString();
                tbGyroX2.Text = mpugx[MPU2].ToString();
                tbGyroY2.Text = mpugy[MPU2].ToString();
                tbGyroZ2.Text = mpugz[MPU2].ToString();
                tbGyroX3.Text = mpugx[MPU3].ToString();
                tbGyroY3.Text = mpugy[MPU3].ToString();
                tbGyroZ3.Text = mpugz[MPU3].ToString();
            }
            if (tcTelas.SelectedTab == tcTelas.TabPages["tpTempDiscs"])
            {
                tbTempDiscFL.Text = temp[MLX1].ToString("F") + " ºC";
                tbTempDiscFR.Text = temp[MLX2].ToString("F") + " ºC";
                tbTempDiscRL.Text = temp[MLX3].ToString("F") + " ºC";
                tbTempDiscRR.Text = temp[MLX4].ToString("F") + " ºC";
                tkbTempDiscFR.Value = Convert.ToInt32(temp[MLX1]);
                tkbTempDiscFL.Value = Convert.ToInt32(temp[MLX2]);
                tkbTempDiscRR.Value = Convert.ToInt32(temp[MLX3]);
                tkbTempDiscRL.Value = Convert.ToInt32(temp[MLX4]);
            }
            if (tcTelas.SelectedTab == tcTelas.TabPages["tpSuspPosition"])
            {
                tbSuspFL.Text = susp[SUSP1].ToString();
                tbSuspFR.Text = susp[SUSP2].ToString();
                tbSuspRL.Text = susp[SUSP3].ToString();
                tbSuspRR.Text = susp[SUSP4].ToString();
                pbSuspFL.Value = susp[SUSP1];
                pbSuspFR.Value = susp[SUSP2];
                pbSuspRL.Value = susp[SUSP3];
                pbSuspRR.Value = susp[SUSP4];
            }
            if (tcTelas.SelectedTab == tcTelas.TabPages["tpVelRodas"])
            {
                tbVelRodaFR.Text = vel[RODA1].ToString();
                tbVelRodaFR.Text = vel[RODA2].ToString();
                tbVelRodaRL.Text = vel[RODA3].ToString();
                tbVelRodaRR.Text = vel[RODA4].ToString();
                pbVelRodaFL.Value = vel[RODA1];
                pbVelRodaFR.Value = vel[RODA2];
                pbVelRodaRL.Value = vel[RODA3];
                pbVelRodaRR.Value = vel[RODA4];
            }
        }

        private void Convert_Log()
        {
            string line;
            int countLine = 0;
            try
            {
                // Read the file line. 
                StreamReader file = new StreamReader("./log.txt");
                while ((line = file.ReadLine()) != null)
                {
                    // Open the file if dont exist or reset the file.
                    if (!File.Exists("./log_converted.txt"))
                    {
                        // Create a file and write the log.
                        //StreamWriter 
                        sw = File.CreateText("./log_converted.txt");
                    }
                    if (File.Exists("./log_converted.txt"))
                    {
                        // Append the next info to the log file.
                        //StreamWriter 
                        sw = File.AppendText("./log_converted.txt");
                    }

                    string sModified = line.Substring(20);

                    strlist = sModified.Split(sepearator, strNr,//gyro
                    StringSplitOptions.RemoveEmptyEntries);
                    
                    double valorLog, valorConvertidoLog = 0.0;

                    foreach (String str in strlist)
                    {
                        try
                        {
                            switch (countLine) //lê na ordem que é mandada pelo programa no arduino
                            {
                                //MPU 6050 1
                                case 0: //accel mpu 1
                                case 1:
                                case 2:
                                case 6: //accel mpu 2
                                case 7:
                                case 8:
                                case 12: //accel mpu 3
                                case 13:
                                case 14:
                                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpGyros"])
                                    {
                                        valorLog = Int32.Parse(str);
                                        valorConvertidoLog = valorLog / 131;//16384.0
                                        sModified = valorConvertidoLog.ToString();//valorConvertido.ToString();
                                        sModified += " gyro mpu";
                                    }
                                    break;
                                case 3: //accel mpu 1
                                case 4:
                                case 5:
                                case 9: //accel mpu 2
                                case 10:
                                case 11:
                                case 15: //accel mpu 3
                                case 16:
                                case 17:
                                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpAccels"])
                                    {
                                        valorLog = Int32.Parse(str);
                                        valorConvertidoLog = valorLog / 16384.0;
                                        sModified = valorConvertidoLog.ToString();//valorConvertido.ToString();
                                        sModified += " accel mpu";
                                    }
                                    break;
                                //MLX 90614 (all temp disc)
                                case 18: //mlx 1 FR
                                case 19:
                                case 20:
                                case 21:
                                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpTempDiscs"])
                                    {
                                        sModified = "mlx";
                                    }
                                    break;
                                //potenciometros suspensão(todos)
                                case 22: //potenciometro FR
                                case 23:
                                case 24:
                                case 25:
                                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpSuspPosition"])
                                    {
                                        sModified = "potenciometro";
                                    }
                                    break;
                                //ky003 all vel rodas
                                case 26: //vel roda FR
                                case 27:
                                case 28:
                                case 29:
                                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpVelRodas"])
                                    {
                                        sModified = "ky003";
                                    }
                                    break;
                            }
                            sw.WriteLine(sModified);
                            //sw.WriteLine(s);
                        }
                        finally
                        {
                            if (sw != null)
                                sw.Close();
                        }

                       countLine++;
                    }


                }

                file.Close();
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            
        }

        private void UpdateFile()
        {
            try
            {
                if (writeFile)
                {
                    // Open the file if dont exist or reset the file.
                    if (!File.Exists("./log.txt") || rewriteFile)
                    {
                        if (rewriteFile)
                            rewriteFile = false;
                        // Create a file and write the log.
                        //StreamWriter 
                        sw = File.CreateText("./log.txt");
                        try
                        {
                            string sModified = s;
                            sModified = new string((from c in sModified
                                                    where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c) || c == ':' || c == '-'
                                                    select c
                                        ).ToArray());
                            sw.WriteLine(Convert.ToString(System.DateTime.Now) + " " + sModified);
                            //sw.WriteLine(s);
                        }
                        finally
                        {
                            if (sw != null)
                                sw.Close();
                        }
                    }
                    if (File.Exists("./log.txt"))
                    {
                        // Append the next info to the log file.
                        //StreamWriter 
                        sw = File.AppendText("./log.txt");
                        try
                        {
                            string sModified = s;
                            sModified = new string((from c in sModified
                                                    where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c) || c == ':' || c == '-'
                                                    select c
                                        ).ToArray());
                            sw.WriteLine(Convert.ToString(System.DateTime.Now) + " " + sModified);
                            //sw.WriteLine(s);
                        }
                        finally
                        {
                            if (sw != null)
                                sw.Close();
                        }
                    }
                }
                else
                {
                    if (sw != null)
                        sw.Close();
                }
            } catch(System.Exception e)
            {
                Console.Write(e);
            }
        }
        
        private void UpdateGUI_Log()
        {
            //--- Updating Rec button/File
            if (writeFile)
            {
                if (recHasChanged)
                {
                    recHasChanged = false;
                    tsmiRec.Text = "Rec*";
                }
                UpdateFile();
            }
            else
            {
                if (recHasChanged)
                {
                    recHasChanged = false;
                    tsmiRec.Text = "Rec ";
                    if (sw != null)
                        sw.Close();
                }
            }
            //--- Updating Rec button/File

            //--- Updating Data fields
            rtbSerialOutput.Text = s;
            rtbTesteNros.Text = "";
            count = 0;

            // using the method 
            strlist = s.Split(sepearator, strNr,
                   StringSplitOptions.RemoveEmptyEntries);
            strlist[0] = "";
            foreach (String str in strlist)
            {
                rtbTesteNros.Text += count.ToString();
                rtbTesteNros.Text += " - ";
                rtbTesteNros.Text += str;
                rtbTesteNros.Text += "\n";
                UpdateFields(str);
                
                count++;
            }
        }

        private void arduinoBoard_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(this.updateStatusDelegate);
            s = serialPort1.ReadLine();

            if (NewSensorDataReceived != null)//If there is someone waiting for this event to be fired
            {
                NewSensorDataReceived(this, new EventArgs()); //Fire the event, indicating that new WeatherData was added to the list.
            }
        }

        private void pegaNomesPortasSerial() //carrega as portas seriais disponíveis
        {
            String[] ports = SerialPort.GetPortNames(); //escolher a ACM0
            cbSerialPorts.Items.AddRange(ports);
        }

        //private void ViewChart1()
        //{
        //    Chart1 lChart = new Chart1(this);
        //    tpGyros.Controls.Add (lChart);
        //}

        //

        //private void tpGyros_Enter(object sender, EventArgs e)
        //{
        //    ViewChart1();
        //}

        private void tsmiRec_Click(object sender, EventArgs e)
        {
            try
            {
                recHasChanged = true;
                if (writeFile == false)
                    writeFile = true;
                else
                    writeFile = false;
            }
            catch(Exception err)
            {
                Console.Write(err);
            }    
        }

        private void tsmiResetLog_Click(object sender, EventArgs e)
        {
            if (!rewriteFile)
                rewriteFile = true;
        }

        private void tsmiConvertLog_Click(object sender, EventArgs e)
        {
            Convert_Log();
        }

        private void tsmiStartSerial_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbSerialPorts.Text != "" )
                {
                    if (!serialPort1.IsOpen)
                    {
                        serialPort1.DataReceived += arduinoBoard_DataReceived;
                        serialPort1.PortName = cbSerialPorts.Text;
                        serialPort1.BaudRate = 9600;
                        serialPort1.Open();
                    }

                    tsmiStartSerial.Enabled = false;
                    tsmiRec.Enabled = true;
                    tsmiResetLog.Enabled = true;
                    tsmiStopSerial.Enabled = true;

                    //manda info para arduino começar a mandar info
                    serialPort1.Write("1#");

                }
            } catch (UnauthorizedAccessException)
            {
                rtbSerialOutput.Text = "***--- Exception: Unauthorized Acess ---***";
            }
        }

        private void tsmiStopSerial_Click(object sender, EventArgs e)
        {
            tsmiStopSerial.Enabled = false;
            tsmiStartSerial.Enabled = true;
            if (!serialPort1.IsOpen)
            {
                serialPort1.DataReceived += arduinoBoard_DataReceived;
                serialPort1.PortName = cbSerialPorts.Text;
                serialPort1.BaudRate = 9600;
                serialPort1.Open();
            }
            serialPort1.Write("0#");
        }

        //Criar um botão para começar a gravar o Log e para parar de gravar, colocar algum sinal visual
        //para saber que está sendo gravado o Log. Uma ideia é deixar o botão vermelho para saber que está gravando.
    }
}
