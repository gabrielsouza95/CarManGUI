using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO.Ports;
using System.IO;

//using IronPython.Hosting;

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


        //--- Thread related
        // Declare our worker thread
        //private Thread workerThread = null;

        // Boolean flag used to stop the 
        private bool stopProcess = false;

        // Declare a delegate used to communicate with the UI thread
        private delegate void UpdateStatusDelegate();
        private UpdateStatusDelegate updateStatusDelegate = null;
        private event EventHandler NewSensorDataReceived;
        //-- Thread related


        //--- Data received related
        //lida com a string recebida para converte-la
        private static String[] sepearator = { "_", ":", "=" }; //marcadores para diferenciar as informações vindas do Arduino
        private static String[] strlist = new String[30]; //strings para receber as partes da msg enviada pelo Arduino
        private static int strNr = 30, //quantidade de strings para passar a função Split 
                           count = 0, //indica qual das informações está sendo passada para a função que atualiza a GUI
                           valor = 0; //variável para guardar o valor convertido da String
        private static double valorConvertido = 0.0; //variável que guarda o valor que foi convertido do valor bruto do sensor para um valor entendível
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

        private void UpdateFields(string pStr)
        {
            switch (count) //lê na ordem que é mandada pelo programa no arduino
            {
                //MPU 6050 1
                case 0: //gyro x mpu 1
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpGyros"])
                    {
                        valor = Int32.Parse(pStr);
                        valorConvertido = valor / 131;//16384.0
                        tbGyrX1.Text = pStr;//valorConvertido.ToString();
                    }
                    break;
                case 1: //gyro y mpu 1
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpGyros"])
                    {
                        valor = Int32.Parse(pStr);
                        valorConvertido = valor / 131;//16384.0
                        tbGyrY1.Text = pStr;//valorConvertido.ToString();
                    }
                    break;
                case 2: //gyro z mpu 1
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpGyros"])
                    {
                        valor = Int32.Parse(pStr);
                        valorConvertido = valor / 131;//16384.0
                        tbGyrZ1.Text = pStr;//valorConvertido.ToString();
                    }
                    break;
                case 3: //accel x mpu 1
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpAccels"])
                    {
                        valor = Int32.Parse(pStr);
                        valorConvertido = valor / 16384.0;
                        tbAcelX1.Text = pStr;//valorConvertido.ToString();
                    }
                    break;
                case 4: //accel y mpu 1
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpAccels"])
                    {
                        valor = Int32.Parse(pStr);
                        valorConvertido = valor / 16384.0;
                        tbAcelY1.Text = pStr;//valorConvertido.ToString();
                    }
                    break;
                case 5: //accel z mpu 1
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpAccels"])
                    {
                        valor = Int32.Parse(pStr);
                        valorConvertido = valor / 16384.0;
                        tbAcelZ1.Text = pStr;//valorConvertido.ToString();
                    }
                    break;
                //MPU 6050 2
                case 6: //gyro x mpu 2
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpGyros"])
                    {
                        tbGyrX2.Text = pStr;
                    }
                    break;
                case 7: //gyro y mpu 2
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpGyros"])
                    {
                        tbGyrY2.Text = pStr;
                    }
                    break;
                case 8: //gyro z mpu 2
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpGyros"])
                    {
                        tbGyrZ2.Text = pStr;
                    }
                    break;
                case 9: //accel x mpu 2
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpAccels"])
                    {
                        tbAcelX2.Text = pStr;
                    }
                    break;
                case 10: //accel y mpu 2
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpAccels"])
                    {
                        tbAcelY2.Text = pStr;
                    }
                    break;
                case 11: //accel z mpu 2
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpAccels"])
                    {
                        tbAcelZ2.Text = pStr;
                    }
                    break;
                //MPU 6050 3
                case 12: //gyro x mpu 3
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpGyros"])
                    {
                        tbGyrX3.Text = pStr;
                    }
                    break;
                case 13: //gyro y mpu 3
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpGyros"])
                    {
                        tbGyrY3.Text = pStr;
                    }
                    break;
                case 14: //gyro z mpu 3
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpGyros"])
                    {
                        tbGyrZ3.Text = pStr;
                    }
                    break;
                case 15: //accel x mpu 3
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpAccels"])
                    {
                        tbAcelX3.Text = pStr;
                    }
                    break;
                case 16: //accel y mpu 3
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpAccels"])
                    {
                        tbAcelY3.Text = pStr;
                    }
                    break;
                case 17: //accel z mpu 3
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpAccels"])
                    {
                        tbAcelZ3.Text = pStr;
                    }
                    break;
                //MLX 90614 (all temp disc)
                case 18: //mlx 1 FR
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpTempDiscs"])
                    {
                        tbTempDiscFR.Text = pStr + " Kelvin";
                        tkbTempDiscFR.Value = Int32.Parse(pStr);
                    }
                    break;
                case 19: //mlx 2 FL
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpTempDiscs"])
                    {
                        tbTempDiscFL.Text = pStr + " Kelvin";
                        tkbTempDiscFL.Value = Int32.Parse(pStr);
                    }
                    break;
                case 20: //mlx 3 RR
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpTempDiscs"])
                    {
                        tbTempDiscRR.Text = pStr + " Kelvin";
                        tkbTempDiscRR.Value = Int32.Parse(pStr);
                    }
                    break;
                case 21: //mlx 4 RL
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpTempDiscs"])
                    {
                        tbTempDiscRL.Text = pStr + " Kelvin";
                        tkbTempDiscRL.Value = Int32.Parse(pStr);
                    }
                    break;
                //potenciometros suspensão(todos)
                case 22: //potenciometro FR
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpSuspPosition"])
                    {
                        tbSuspFR.Text = pStr;
                        pbSuspFR.Value = Int32.Parse(pStr);
                    }
                    break;
                case 23: //potenciometro FL
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpSuspPosition"])
                    {
                        tbSuspFL.Text = pStr;
                        pbSuspFL.Value = Int32.Parse(pStr);
                    }
                    break;
                case 24: //potenciometro RR
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpSuspPosition"])
                    {
                        tbSuspRR.Text = pStr;
                        pbSuspRR.Value = Int32.Parse(pStr);
                    }
                    break;
                case 25: //potenciometro RL
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpSuspPosition"])
                    {
                        tbSuspRL.Text = pStr;
                        pbSuspRL.Value = Int32.Parse(pStr);
                    }
                    break;
                //ky003 all vel rodas
                case 26: //vel roda FR
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpVelRodas"])
                    {
                        tbVelRodaFR.Text = pStr;
                        pbVelRodaFR.Value = Int32.Parse(pStr);
                    }
                    break;
                case 27: //vel roda FL
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpVelRodas"])
                    {
                        tbVelRodaFL.Text = pStr;
                        pbVelRodaFL.Value = Int32.Parse(pStr);
                    }
                    break;
                case 28: //vel roda RR
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpVelRodas"])
                    {
                        tbVelRodaRR.Text = pStr;
                        pbVelRodaRR.Value = Int32.Parse(pStr);
                    }
                    break;
                case 29: //vel roda RL
                    if (tcTelas.SelectedTab == tcTelas.TabPages["tpVelRodas"])
                    {
                        tbVelRodaRL.Text = pStr;
                        pbVelRodaRL.Value = Int32.Parse(pStr);
                    }
                    break;
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

                    strlist = sModified.Split(sepearator, strNr,
                    StringSplitOptions.RemoveEmptyEntries);
                    
                    double valorLog, valorConvertidoLog = 0.0;

                    foreach (String str in strlist)
                    {
                        try
                        {
                            switch (countLine) //lê na ordem que é mandada pelo programa no arduino
                            {
                                //MPU 6050 1
                                case 0: //gyro mpu 1
                                case 1:
                                case 2:
                                case 6: //gyro mpu 2
                                case 7:
                                case 8:
                                case 12: //gyro mpu 3
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

        /*private void comunicaSerial()
        {
            //--- rodando script c++
            //pOp é a opção escolhida para poder parar ou não o processo
            //serial.ComunicaSerial();
            //tbVelRoda1.Text = serial.leSerial;
            //--- fim rodando script c++
            try
            {
                for (;;)
                {
                    // Check if Stop button was clicked
                    if (!this.stopProcess)
                    {
                        // Show progress
                        this.Invoke(this.updateStatusDelegate);
                        s = serialPort1.ReadLine();
                    }
                    else
                    {
                        // Stop thread
                        this.workerThread.Abort();
                    }
                }
            }
            catch (System.Exception e)
            {
                Console.Write(e);
            }
        }*/

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

                    this.stopProcess = false;

                    //manda info para arduino começar a mandar info
                    serialPort1.Write("1#");

                    // Initialise and start worker thread
                    //this.workerThread = new Thread(new ThreadStart(this.comunicaSerial));
                    //this.workerThread.Start();
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
            this.stopProcess = true;

            serialPort1.Write("0#");
            //serialPort1.Close();
        }

        //Criar um botão para começar a gravar o Log e para parar de gravar, colocar algum sinal visual
        //para saber que está sendo gravado o Log. Uma ideia é deixar o botão vermelho para saber que está gravando.
    }
}
