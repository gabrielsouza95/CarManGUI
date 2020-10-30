using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace CarManGUI
{
    public class ClassSerial
    {   
        static Process proc = new Process(); //inicia processo no contexto global para poder manter ele rodando
        static string buffer = "";

        public ClassSerial()
        {
            proc.StartInfo.FileName = "serial";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;

            proc.Start();

            RecebeEnviaSerial();
        }

        public void ComunicaSerial()
        {
            Process[] pname = Process.GetProcessesByName("serial"); //verifica se está rodando o processo
            if (pname.Length == 0) //caso o processo tenha parado de rodar
            {
                proc.StartInfo.FileName = "serial";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;

                proc.Start();
            }
            
        }

        public void RecebeEnviaSerial()
        {
            StreamWriter procStreamWriter = proc.StandardInput;
            StreamReader procStreamReader = proc.StandardOutput;
            try 
            {
                int i = 0;

                while (i < 7)
                {
                    buffer += procStreamReader.ReadLine(); //le o que o script escreveu
                    procStreamWriter.WriteLine("\n"); //manda um end of line para continuar o script
                    i++;
                    buffer += "&"; //adiciona separador de strings recebidas
                }

                procStreamWriter.WriteLine("fecha");
            }
            catch (Exception e)
            {
                Console.Write(e);   
            }
        }

        public string leSerial
        {
            get
            {
                string sAux = buffer;
                buffer = ""; //esvazia buffer
                return sAux;
            }
        }
    }
}
