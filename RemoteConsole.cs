using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WMinfo_Client
{
    
    class RemoteConsole
    {
        public string command = "";

        public void ConsoleWork()
        {
            Process process = new System.Diagnostics.Process();
            ProcessStartInfo startiNFO = new System.Diagnostics.ProcessStartInfo();
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            string name = "/C " +command;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = name;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.Start();
            string q = "";
            string consoleouput = "";

            if (name.Contains("chkdsk"))
            {
                q = "";
                while (!process.HasExited)
                {
                    Thread.Sleep(10);
                    q = process.StandardOutput.ReadLine() + "&";
                    consoleouput = q.Replace(".", "-").Replace(" ", "*").Replace(":", "$").Replace(";", "[").Replace("%", "]");
                    HttpWebRequest request = WebRequest.Create("http://192.168.15.10:9090/" + "?" + consoleouput) as HttpWebRequest;
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    Console.WriteLine("Dados de Console enviados com sucesso");
                    Console.WriteLine("Resposta do servidor: {0}", responseString);
                    q = "";
                    consoleouput = "";
                }
            }

            if (name.Contains("ipconfig"))
            {
                q = "";
                while (!process.HasExited)
                {
                    Thread.Sleep(20);
                    q = process.StandardOutput.ReadToEnd();
                }

                consoleouput = q.Replace(".", "$").Replace(" ", "[").Replace(":", "]").Replace("\n", "&").Replace("\t", "&").Replace("\r", "&").Replace("%", "+");
                HttpWebRequest request = WebRequest.Create("http://192.168.15.10:9090/" + "?" + consoleouput) as HttpWebRequest;
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Console.WriteLine("Dados de Console enviados com sucesso");
                Console.WriteLine("Resposta do servidor: {0}", responseString);
                q = "";
                consoleouput = "";
            }

            if (name.Contains("taskkill"))
            {
                q = "";
                while (!process.HasExited)
                {
                    q = "";
                    while (!process.HasExited)
                    {
                        Thread.Sleep(20);
                        q = process.StandardOutput.ReadToEnd();
                    }

                    consoleouput = "Sucess[&The&process&is&dead&R-I-P";
                    HttpWebRequest request = WebRequest.Create("http://192.168.15.10:9090/" + "?" + consoleouput) as HttpWebRequest;
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    Console.WriteLine("Dados de Console enviados com sucesso");
                    Console.WriteLine("Resposta do servidor: {0}", responseString);
                    q = "";
                    consoleouput = "";
                }
            }

            if (name.Contains("netstat") || name.Contains("netstat -a")|| name.Contains("netstat -b") || name.Contains("netstat -f") || name.Contains("netstat -o") || name.Contains("netstat -t") || name.Contains("netstat -y"))
            {
                q = "";
                while (!process.HasExited)
                {
                    Thread.Sleep(10);
                    q = process.StandardOutput.ReadLine() + "&";
                    consoleouput = q.Replace(".", "$").Replace(":", "+").Replace(" ", "/");
                    HttpWebRequest request = WebRequest.Create("http://192.168.15.10:9090/" + "?" + consoleouput) as HttpWebRequest;
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                    Console.WriteLine("Dados de Console enviados com sucesso");
                    Console.WriteLine("Resposta do servidor: {0}", responseString);
                    q = "";
                    consoleouput = "";
                }
            }

            if (name.Contains("netstat -e") || name.Contains("netstat -s") || name.Contains("netstat -e -s") || name.Contains("netstat -n"))
            {
                q = "";
                while (!process.HasExited)
                {
                    Thread.Sleep(20);
                    q = process.StandardOutput.ReadToEnd();
                }

                consoleouput = q.Replace(" ", "$").Replace("\n", "&").Replace("\t", "&").Replace("\r", "&");
                HttpWebRequest request = WebRequest.Create("http://192.168.15.10:9090/" + "?" + consoleouput) as HttpWebRequest;
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Console.WriteLine("Dados de Console enviados com sucesso");
                Console.WriteLine("Resposta do servidor: {0}", responseString);
                q = "";
                consoleouput = "";


            }

            if (name.Contains("netstat -r"))
            {
                q = "";
                while (!process.HasExited)
                {
                    Thread.Sleep(20);
                    q = process.StandardOutput.ReadToEnd();
                }

                consoleouput = q.Replace(" ", "$").Replace("\n", "&").Replace("\t", "&").Replace("\r", "&").Replace("#", "+").Replace(".", "_");
                HttpWebRequest request = WebRequest.Create("http://192.168.15.10:9090/" + "?" + consoleouput) as HttpWebRequest;
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Console.WriteLine("Dados de Console enviados com sucesso");
                Console.WriteLine("Resposta do servidor: {0}", responseString);
                q = "";
                consoleouput = "";


            }

            if (name.Contains("tasklist"))
            {
                q = "";
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
                    if (count >= diminsh.Length/5)
                    {
                        HttpWebRequest request = WebRequest.Create("http://192.168.15.10:9090/" + "?" + consoleouput) as HttpWebRequest;
                        var response = (HttpWebResponse)request.GetResponse();
                        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        Console.WriteLine("Dados de Console enviados com sucesso");
                        Console.WriteLine("Resposta do servidor: {0}", responseString);
                        consoleouput = "";
                        count = 0;
                    }
                }
                q = "";


            }



            Thread.Sleep(200);
            HttpWebRequest request1 = WebRequest.Create("http://192.168.15.10:9090/" + "?" + "done") as HttpWebRequest;
            var response1 = (HttpWebResponse)request1.GetResponse();
            var responseString1 = new StreamReader(response1.GetResponseStream()).ReadToEnd();
            Console.WriteLine("Dados de Console enviados com sucesso");
            Console.WriteLine("Resposta do servidor: {0}", responseString1);

        }


    }
}
