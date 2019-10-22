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

//using IronPython.Hosting;

namespace CarManGUI
{
    public partial class Form1 : Form
    {
        //inicia processo através da classe no contexto global para poder manter ele rodando e atualizar a janela
        //static ClassSerial serial = new ClassSerial(); //classe com a parte de comunicação serial
        static SerialPort serialPort1 = new SerialPort();
        static int read = 0;
        static string s;

        public Form1()
        {
            InitializeComponent(); // inicializa os componentes da janela
            pegaNomesPortasSerial();
            tsmiStopSerial.Enabled = false;
            rtbSerialOutput.Text = "Iniciado";
            backgroundWorkerSerial = new BackgroundWorker();
            backgroundWorkerSerial. = true;
            backgroundWorkerSerial.DoWork += new DoWorkEventHandler(backgroundWorkerSerial_DoWork);
            backgroundWorkerSerial.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerSerial_RunWorkerCompleted);
            tsmiStartSerial.Click += new EventHandler(tsmiStartSerial_Click);
        }
        
        private void comunicaSerial(int pOp)
        {
            //--- rodando script c++
            //pOp é a opção escolhida para poder parar ou não o processo
            //serial.ComunicaSerial();
            //tbVelRoda1.Text = serial.leSerial;
            //--- fim rodando script c++
            //rtbSerialOutput.Text = serialPort1.ReadLine(); comando pra ler uma vez por vez clicada
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
                //comunicaSerial(0);    
            }
            catch(Exception err)
            {
                Console.Write(err);
            }    
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
                    tsmiStopSerial.Enabled = true;

                    backgroundWorkerSerial.RunWorkerAsync();
                }
            } catch (UnauthorizedAccessException)
            {
                rtbSerialOutput.Text = "***--- Exception: Unauthorized Acess ---***";
            }
        }

        private void tsmiStopSerial_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            tsmiStopSerial.Enabled = false;
            tsmiStartSerial.Enabled = true;
        }

        //--------------------------------------------------------------------------------------------------
        // Lida com o backgroundWorker
        //
        // This event handler is where the time-consuming work is done.
        private void backgroundWorkerSerial_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            int i = 1;

            if (worker.CancellationPending == true)
            {
                e.Cancel = true;
            }
            else
                for(;;)
                    worker.ReportProgress(i,serialPort1.ReadLine());
        }

        // This event handler updates the progress.
        private void backgroundWorkerSerial_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            rtbSerialOutput.Text = e.ToString();
        }

        // This event handler deals with the results of the background operation.
        private void backgroundWorkerSerial_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                rtbSerialOutput.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                rtbSerialOutput.Text = "Error: " + e.Error.Message;
            }
            else
            {
                rtbSerialOutput.Text = "Done!";
            }
        }
        //Criar um botão para começar a gravar o Log e para parar de gravar, colocar algum sinal visual
        //para saber que está sendo gravado o Log. Uma ideia é deixar o botão vermelho para saber que está gravando.
    }
}
