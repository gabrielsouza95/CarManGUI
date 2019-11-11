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

        static string s = ""; //recebe os dados da serial
        //--- Serial related


        //--- Thread related
        // Declare our worker thread
        private Thread workerThread = null;

        // Boolean flag used to stop the 
        private bool stopProcess = false;

        // Declare a delegate used to communicate with the UI thread
        private delegate void UpdateStatusDelegate();
        private UpdateStatusDelegate updateStatusDelegate = null;
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
        static StreamWriter sw;
        static bool writeFile = false;
        static bool rewriteFile = false;
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
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpGyros"])
                    {
                        valor = Int32.Parse(pStr);
                        valorConvertido = valor / 131;//16384.0
                        this.tbGyrX1.Text = pStr;//valorConvertido.ToString();
                    }
                    break;
                case 1: //gyro y mpu 1
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpGyros"])
                    {
                        valor = Int32.Parse(pStr);
                        valorConvertido = valor / 131;//16384.0
                        this.tbGyrY1.Text = pStr;//valorConvertido.ToString();
                    }
                    break;
                case 2: //gyro z mpu 1
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpGyros"])
                    {
                        valor = Int32.Parse(pStr);
                        valorConvertido = valor / 131;//16384.0
                        this.tbGyrZ1.Text = pStr;//valorConvertido.ToString();
                    }
                    break;
                case 3: //accel x mpu 1
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpAccels"])
                    {
                        valor = Int32.Parse(pStr);
                        valorConvertido = valor / 16384.0;
                        this.tbAcelX1.Text = pStr;//valorConvertido.ToString();
                    }
                    break;
                case 4: //accel y mpu 1
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpAccels"])
                    {
                        valor = Int32.Parse(pStr);
                        valorConvertido = valor / 16384.0;
                        this.tbAcelY1.Text = pStr;//valorConvertido.ToString();
                    }
                    break;
                case 5: //accel z mpu 1
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpAccels"])
                    {
                        valor = Int32.Parse(pStr);
                        valorConvertido = valor / 16384.0;
                        this.tbAcelZ1.Text = pStr;//valorConvertido.ToString();
                    }
                    break;
                //MPU 6050 2
                case 6: //gyro x mpu 2
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpGyros"])
                    {
                        this.tbGyrX2.Text = pStr;
                    }
                    break;
                case 7: //gyro y mpu 2
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpGyros"])
                    {
                        this.tbGyrY2.Text = pStr;
                    }
                    break;
                case 8: //gyro z mpu 2
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpGyros"])
                    {
                        this.tbGyrZ2.Text = pStr;
                    }
                    break;
                case 9: //accel x mpu 2
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpAccels"])
                    {
                        this.tbAcelX2.Text = pStr;
                    }
                    break;
                case 10: //accel y mpu 2
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpAccels"])
                    {
                        this.tbAcelY2.Text = pStr;
                    }
                    break;
                case 11: //accel z mpu 2
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpAccels"])
                    {
                        this.tbAcelZ2.Text = pStr;
                    }
                    break;
                //MPU 6050 3
                case 12: //gyro x mpu 3
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpGyros"])
                    {
                        this.tbGyrX3.Text = pStr;
                    }
                    break;
                case 13: //gyro y mpu 3
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpGyros"])
                    {
                        this.tbGyrY3.Text = pStr;
                    }
                    break;
                case 14: //gyro z mpu 3
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpGyros"])
                    {
                        this.tbGyrZ3.Text = pStr;
                    }
                    break;
                case 15: //accel x mpu 3
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpAccels"])
                    {
                        this.tbAcelX3.Text = pStr;
                    }
                    break;
                case 16: //accel y mpu 3
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpAccels"])
                    {
                        this.tbAcelY3.Text = pStr;
                    }
                    break;
                case 17: //accel z mpu 3
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpAccels"])
                    {
                        this.tbAcelZ3.Text = pStr;
                    }
                    break;
                //MLX 90614 (all temp disc)
                case 18: //mlx 1 FR
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpTempDiscs"])
                    {
                        this.tbTempDiscFR.Text = pStr + " Kelvin";
                        this.tkbTempDiscFR.Value = Int32.Parse(pStr);
                    }
                    break;
                case 19: //mlx 2 FL
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpTempDiscs"])
                    {
                        this.tbTempDiscFL.Text = pStr + " Kelvin";
                        this.tkbTempDiscFL.Value = Int32.Parse(pStr);
                    }
                    break;
                case 20: //mlx 3 RR
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpTempDiscs"])
                    {
                        this.tbTempDiscRR.Text = pStr + " Kelvin";
                        this.tkbTempDiscRR.Value = Int32.Parse(pStr);
                    }
                    break;
                case 21: //mlx 4 RL
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpTempDiscs"])
                    {
                        this.tbTempDiscRL.Text = pStr + " Kelvin";
                        this.tkbTempDiscRL.Value = Int32.Parse(pStr);
                    }
                    break;
                //potenciometros suspensão(todos)
                case 22: //potenciometro FR
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpSuspPosition"])
                    {
                        this.tbSuspFR.Text = pStr;
                        this.pbSuspFR.Value = Int32.Parse(pStr);
                    }
                    break;
                case 23: //potenciometro FL
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpSuspPosition"])
                    {
                        this.tbSuspFL.Text = pStr;
                        this.pbSuspFL.Value = Int32.Parse(pStr);
                    }
                    break;
                case 24: //potenciometro RR
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpSuspPosition"])
                    {
                        this.tbSuspRR.Text = pStr;
                        this.pbSuspRR.Value = Int32.Parse(pStr);
                    }
                    break;
                case 25: //potenciometro RL
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpSuspPosition"])
                    {
                        this.tbSuspRL.Text = pStr;
                        this.pbSuspRL.Value = Int32.Parse(pStr);
                    }
                    break;
                //ky003 all vel rodas
                case 26: //vel roda FR
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpVelRodas"])
                    {
                        this.tbVelRodaFR.Text = pStr;
                        this.pbVelRodaFR.Value = Int32.Parse(pStr);
                    }
                    break;
                case 27: //vel roda FL
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpVelRodas"])
                    {
                        this.tbVelRodaFL.Text = pStr;
                        this.pbVelRodaFL.Value = Int32.Parse(pStr);
                    }
                    break;
                case 28: //vel roda RR
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpVelRodas"])
                    {
                        this.tbVelRodaRR.Text = pStr;
                        this.pbVelRodaRR.Value = Int32.Parse(pStr);
                    }
                    break;
                case 29: //vel roda RL
                    if (this.tcTelas.SelectedTab == this.tcTelas.TabPages["tpVelRodas"])
                    {
                        this.tbVelRodaRL.Text = pStr;
                        this.pbVelRodaRL.Value = Int32.Parse(pStr);
                    }
                    break;
            }
        }

        private void UpdateFile()
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
                        sw.WriteLine(Convert.ToString(System.DateTime.Now)+" "+s);
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
                        sw.WriteLine(Convert.ToString(System.DateTime.Now) + " " + s);
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
        }

        private void UpdateGUI_Log()
        {
            //--- Updating Rec button/File
            if (writeFile)
            {
                tsmiRec.Text = "Rec*";
                UpdateFile();
            }
            else
            {
                tsmiRec.Text = "Rec ";
                if (sw != null)
                    sw.Close();
            }
            //--- Updating Rec button/File

            //--- Updating Data fields
            this.rtbSerialOutput.Text = s;
            this.rtbTesteNros.Text = "";
            count = 0;

            // using the method 
            strlist = s.Split(sepearator, strNr,
                   StringSplitOptions.RemoveEmptyEntries);

            foreach (String str in strlist)
            {
                this.rtbTesteNros.Text += count.ToString();
                this.rtbTesteNros.Text += " - ";
                this.rtbTesteNros.Text += str;
                this.rtbTesteNros.Text += "\n";
                this.UpdateFields(str);
                count++;
            }
        }

        private void comunicaSerial()
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

        private void tsmiStartSerial_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbSerialPorts.Text != "")
                {
                    serialPort1.PortName = cbSerialPorts.Text;
                    serialPort1.BaudRate = 115200;
                    serialPort1.Open();

                    tsmiStartSerial.Enabled = false;
                    tsmiRec.Enabled = true;
                    tsmiResetLog.Enabled = true;
                    //tsmiStopSerial.Enabled = true;

                    this.stopProcess = false;

                    // Initialise and start worker thread
                    this.workerThread = new Thread(new ThreadStart(this.comunicaSerial));
                    this.workerThread.Start();
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
            serialPort1.Close();
        }

        //Criar um botão para começar a gravar o Log e para parar de gravar, colocar algum sinal visual
        //para saber que está sendo gravado o Log. Uma ideia é deixar o botão vermelho para saber que está gravando.
    }
}
