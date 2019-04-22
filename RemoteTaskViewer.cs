using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WMinfo_Client
{
    class RemoteTaskViewer
    {

        public void RTVWORK()
        {

            Process process = new System.Diagnostics.Process();
            ProcessStartInfo startiNFO = new System.Diagnostics.ProcessStartInfo();
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            string name = "/C tasklist";
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = name;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            string q = "";
            string consoleouput = "";

            while (!process.HasExited)
            {
                Thread.Sleep(20);
                q = process.StandardOutput.ReadToEnd();
            }

            consoleouput = q.Replace(" ", "$").Replace("\t", "&").Replace("\r", "&").Replace(".", "[").Replace("#", "]").Replace(",", "+");

            string[] diminsh = consoleouput.Split('\n');
            consoleouput = "";
            int count = 0;
            foreach (string piece in diminsh)
            {
                count++;
                consoleouput += piece.Replace("\n", "&");
                if (count >= diminsh.Length / 5)
                {
                    try
                    {
                        HttpWebRequest request = WebRequest.Create("http://192.168.15.10:7070/" + "?" + consoleouput) as HttpWebRequest;
                        var response = (HttpWebResponse)request.GetResponse();
                        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        Console.WriteLine("Dados de Console enviados com sucesso");
                        Console.WriteLine("Resposta do servidor: {0}", responseString);
                        consoleouput = "";
                        count = 0;
                    }
                    catch (SystemException e)
                    {}
                }
            }
            q = "";


            try
            {
                Thread.Sleep(100);
                HttpWebRequest request1 = WebRequest.Create("http://192.168.15.10:7070/" + "?" + "done") as HttpWebRequest;
                var response1 = (HttpWebResponse)request1.GetResponse();
                var responseString1 = new StreamReader(response1.GetResponseStream()).ReadToEnd();
                Console.WriteLine("Dados de Console enviados com sucesso");
                Console.WriteLine("Resposta do servidor: {0}", responseString1);
            }
            catch (SystemException e)
            { }


        }

    }
}
