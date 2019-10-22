using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
//using IronPython.Hosting;

namespace CarManGUI
{
    public partial class Form1 : Form
    {
        //private static Process proc = new Process(); //inicia processo no contexto global para poder manter ele rodando
        //private StreamWriter procStreamWriter = proc.StandardInput; //
        static ClassSerial serial = new ClassSerial(); //classe com a parte de comunicação serial

        public Form1()
        {
            //var engine = Python.CreateEngine();
            //dynamic py = engine.ExecuteFile(@"..\..\teste.py");
            

            InitializeComponent();
        }
        
        private void comunicaSerial(int pOp)
        {
            //--- rodando script python
            //var engine = Python.CreateEngine();
            //-----------------------
            //ICollection<string> paths = engine.GetSearchPaths();//= new List<string>();
            //paths.Add(Environment.CurrentDirectory);
            //string dir = @"/usr/local/lib/python2.7/dist-packages";
            //paths.Add(dir);
            //engine.SetSearchPaths(paths);
            //------------------------
            //dynamic py = engine.ExecuteFile(@"./teste.py");

            //dynamic test = py.Teste();
            //Console.WriteLine(test.__class__.__name__);
            //test.hello_world();
            //--- fim rodando script python

            //--- rodando script c++

            //Process process = new Process();
            //// Configure the process using the StartInfo properties.
            //process.StartInfo.FileName = "serial";
            //process.StartInfo.Arguments = "/dev/ttyACM0";
            ////process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            //process.Start();
            //process.WaitForExit();// Waits here for the process to exit.

            //ProcessStartInfo startInfo = new ProcessStartInfo() { FileName = "serial" };//{ FileName = "/bin/bash", Arguments = "/Desktop/KarManGUI/serial /dev/ttyACM0", }; //"/Desktop/KarManGUI/serial /dev/ttyACM0"
            //Process proc = new Process() { StartInfo = startInfo };
            //pOp é a opção escolhida para poder parar ou não o processo
            //MessageBox.Show("nothing");
            //ClassSerial serial = new ClassSerial();
            serial.ComunicaSerial();
            //tbVelRoda1.Text = serial.leSerial;
            //--- fim rodando script c++
        }

        private void tsmiRec_Click(object sender, EventArgs e)
        {
            try
            {
                comunicaSerial(0);
            }
            catch(Exception err)
            {
                Console.Write(err);
            }    
        }
        //Criar um botão para começar a gravar o Log e para parar de gravar, colocar algum sinal visual
        //para saber que está sendo gravado o Log. Uma ideia é deixar o botão vermelho para saber que está gravando.
    }
}
