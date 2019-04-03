using System;
using OpenHardwareMonitor.Hardware;
using System.Threading;
using System.Net;

#region Project INFO ##### READ-ME ######

#region References, Legal deployment and contact.
/* - This project uses OpenHardWareMonitor.DLL, it's an opensource code that is availabe at: https://github.com/openhardwaremonitor/openhardwaremonitor -
 * - The dev team is aware of the use, and ANY comercial effect that this code may have, it needs to be made with the knowledge of the original devs. -
 * - This project is not authorized in ANY way to be used on comercial activities, if you do so, there are legal consequences.
 * - This code will be commentend in two languages, English as the main-one and PT-BR as the secondary one, since all members of this project are native Portuguese speakers. -
 * - For any trouble or info, please contact me at: Email: brealmeidaa@hotmail.com Git: https://github.com/Breno94re
 * */
#endregion

#region About the project
/* - This project aims for monitoring any Home-PC or Workstations/Servers via API/HTTP.
 * - The main goal is to archieve a stable and useful tool that can collect'n show info to the user, with the possibility of PDF - Reports, with graphics and usefull Statistics.
 * */
#endregion

#region Devs Info
/* - Version - 1.3 Beta -
 * - Type: Client  -
 * - Last Updated at: 03/04/2019 (DD/MM/YYYY) -
 * 
 * 
 * - ######## What's New ######### -
 *  
 *  - Fixed the problem with CPU frequency not updating
 *  - Universal code for any core quantities
 *  - Added Global Support for Intel processors
 *  - Added Global Support for Nvidia GPU's
 *  - Added GetCpuInfo Method's wich brings a string HttpAPI Ready
 * 
 * - ######## What's New ######### -
 * 
 * 
 * - ######## Bugs to fix ######### -
 * 
 * - HDD POH and PCC are not picking up correctly since last patch. Need to re-work how info is splited.
 * 
 * - ######## Bugs to fix ######### -
 * 
 * 
 * - ######## Last Changes ######### -
 * 
 *     -----------1.2b-------------
 *     
 * - Major Changes on how processor sensor info is Get, now we're using info directly from the WMI sensors, that decreases the CPU using -
 * - from 30% to 1% -
 * 
 * - Organization of the Code, now all areas have PT-BR and EN-US comments.
 * 
 * - Segmentation in chunks, on what the code is about.
 *     
 *     -----------1.2b-------------
 * 
 *     -----------1.1b-------------
 * - Added Global support for HDD's monitoring -
 *     -----------1.1b-------------
 *     
 *     -----------1.0b-------------
 * - Added Global Ram Support -
 *     -----------1.0b-------------
 *     
 *     
 * - ######## Last Changes ######### -
 * 
 * */
#endregion

#endregion



namespace HWinfoClient
{
    class Program
    {
        #region Global Var's
        public static Computer myComputer = new Computer();
        public static string[] processorinfo;
        #endregion

        #region MAIN
        static void Main(string[] args)
        {
            #region DLL

            //comments//
            #region Comentários PT-BR
            /* Esse chunk é referente ao uso da DLL OpenHardWareMonitor, que facilita o uso da WMI.
            * Aqui eu posso especificar o uso de que parte do PC irei puxar infos.
            * é importantíssimo entender que a instância de classe MyComputer não deve ser declarda mais de 1x, devido ao alto uso de CPU*/
            #endregion

            #region EN-US Comments
            /* This Chunk is making use of the OpenHardwareMonitor.DLL, that helps with obtaining info from the pc hardware's.
            * Here the dll use's boolean Get() to set what hardwares that info will be needed.
            * It's very important to notice that "myComputer" object cannot be instanced more than once, this will raise CPU use and destroying the porpuses of this project.*/
            #endregion
            //comments//

            myComputer.CPUEnabled = true; //CPU info's bool
            myComputer.GPUEnabled = true; //GPU info's bool
            myComputer.HDDEnabled = true; //HDD info's bool
            myComputer.Open();
            #endregion
            
            while (1 < 2)
            {
                Thread.Sleep(1000);
                Console.Clear();
                string info = "";
                info = myComputer.GetReport(); // Report from DDL, here we going to pick up HDD and GPU info.
                processorinfo = info.Split('\n'); //Split the Report in Lines, to make the reading easier.

                #region CPU GET Method()

                //comments//
                #region Comentários PT-BR
                /* Esse Método está retornando um grupo de informação, dividido por '*' nessa ordem: nome*cores*clocks*temps*load
                 * Cada grupo tem conteúdo. O conteúdo dos grupos está dividido por '%'
                 * Esse método irá tornar mais fácil trabaçhar com métodos http.
                 */
                #endregion

                #region EN-US Comments
                /* This Method is returning groups of info, divided by '*' in that order: name*cores*clocks*temps*load
                 * Each group has content, their content is divided by '%'
                 * This method will make easier to work with http methods.
                 */
                #endregion
                //comments//


                string[] CPUchunks = GetCpuInfo().Split('*'); //Calling GetCpuInfo() Method, wich return a group of information on a string format.

                for (int i = 0; i < CPUchunks.Length; i++)
                    Console.WriteLine(CPUchunks[i]);

                #endregion

                #region GPU

                #region NVIDIA

                #region Comentários PT-BR
                /* Esse chunk é responsável por obter: Informações da GPU, como temperatura, clocks, load e fan
                 */
                #endregion

                #region EN-US Comments
                /* This chunk is responsible for getting: GPU info, like: Temps, Clocks, load level and fan level.
                */
                #endregion

                string temp1 = ""; // Temperatures Cº
                string Load1 = ""; // Load %
                string clock1 = ""; // Clock total
                string fan1 = ""; // Fan %

                foreach (var hardwareItem in myComputer.Hardware)
                {

                    if (hardwareItem.HardwareType == HardwareType.GpuNvidia)
                    {
                        hardwareItem.Update();
                        foreach (IHardware subHardware in hardwareItem.SubHardware)
                            subHardware.Update();

                        foreach (var sensor in hardwareItem.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Temperature)
                            {
                                temp1 += String.Format("{0} Temperature = {1}\r\n", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "null");
                            }
                            if (sensor.SensorType == SensorType.Load)
                            {
                                Load1 += String.Format("{0} Load = {1}\r\n", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "null");
                            }
                            if (sensor.SensorType == SensorType.Clock)
                            {
                                clock1 += String.Format("{0} Clock = {1}\r\n", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "null");
                            }
                            if (sensor.SensorType == SensorType.Fan)
                            {
                                fan1 += String.Format("{0} Fan = {1}\r\n", sensor.Name, sensor.Value.HasValue ? sensor.Value.Value.ToString() : "null");
                            }
                        }
                    }
                }

                Console.WriteLine("{0}\n{1}\n{2}\n{3}", clock1, temp1, Load1, fan1);

                #endregion

                #endregion

                #region HDD



                //HDD//
                //método para achar o HDD
                int count = 0;
                string hddtemps = "";
                string hddgeneralinfo = "";
                string hdddrivename = "";
                string hddpoh = "";
                string hddpcc = "";
                int size = 0;
                string[] splithdd = info.Split('\n'); // divide por quebras de linha

                for (int i = 0; i < splithdd.Length; i++)
                {
                    //Informações Gerais//
                    if (splithdd[i].Contains("Logical drive name:") == true)
                    {
                        string hddtest = ""; //string para armazenamento temporário
                        bool test = false; //bool que fará avaliação do hdd

                        for (int l = i; l < splithdd.Length; l++)
                        {
                            string[] splithdds = splithdd[i + count].Split(':');

                            if (count == 2)
                            {
                                char[] counterino = splithdds[1].ToCharArray();

                                if (counterino.Length < 13) //verifica se o hdd é real ou é uma partição do sistema
                                    test = false;
                                if (counterino.Length >= 13) //verifica se o hdd é real ou é uma partição do sistema
                                    test = true;
                            }

                            if (count == 3)
                            {
                                hddtest += splithdds[1] + "%" + "\n&";
                                count = 0;
                                break;
                            }

                            hddtest += splithdds[1] + "%" + "\n";
                            count++;
                        }

                        if (test == true)
                            hddgeneralinfo += hddtest;
                    }
                    //Informações Gerais//

                    //Informações de temperatura//
                    if (splithdd[i].Contains("C2 Temperature") == true)
                    {
                        size++;
                        string[] stringtemp = splithdd[i].Replace('\n', ' ').Split(' ');

                        if (stringtemp.Length == 43)
                            hddtemps += stringtemp[27] + "*" + stringtemp[36] + "%";

                        if (stringtemp.Length == 44)
                            hddtemps += stringtemp[27] + "*" + stringtemp[37] + "%";

                        if (stringtemp.Length == 45)
                            hddtemps += stringtemp[27] + "*" + stringtemp[38] + "%";

                        if (stringtemp.Length == 46)
                            hddtemps += stringtemp[27] + "*" + stringtemp[39] + "%";

                        if (stringtemp.Length == 47)
                            hddtemps += stringtemp[27] + "*" + stringtemp[40] + "%";

                        if (stringtemp.Length == 48)
                            hddtemps += stringtemp[27] + "*" + stringtemp[41] + "%";

                        if (stringtemp.Length == 49)
                            hddtemps += stringtemp[27] + "*" + stringtemp[42] + "%";

                        if (stringtemp.Length == 50)
                            hddtemps += stringtemp[27] + "*" + stringtemp[43] + "%";
                    }
                    //Informações de temperatura//

                    //informações de POH//
                    if (splithdd[i].Contains("09 Power-On Hours (POH)") == true)
                    {

                        string[] pohtemp = splithdd[i].Replace('\n', ' ').Split(' ');

                        if (pohtemp.Length == 34 || pohtemp.Length == 35 || pohtemp.Length == 36)
                        {
                            if (pohtemp[31] != String.Empty)
                                hddpoh += pohtemp[31] + "%";

                            if (pohtemp[32] != String.Empty)
                                hddpoh += pohtemp[32] + "%";

                            if (pohtemp[33] != String.Empty)
                                hddpoh += pohtemp[33] + "%";

                            if (pohtemp[34] != String.Empty)
                                hddpoh += pohtemp[34] + "%";


                        }

                        if (pohtemp.Length == 36 || pohtemp.Length == 37)
                        {
                            if (pohtemp[31] != String.Empty)
                                hddpoh += pohtemp[31] + "%";

                            if (pohtemp[32] != String.Empty)
                                hddpoh += pohtemp[32] + "%";

                            if (pohtemp[33] != String.Empty)
                                hddpoh += pohtemp[33] + "%";

                            if (pohtemp[34] != String.Empty)
                                hddpoh += pohtemp[34] + "%";

                            if (pohtemp[35] != String.Empty)
                                hddpoh += pohtemp[35] + "%";

                            if (pohtemp[36] != String.Empty)
                                hddpoh += pohtemp[36] + "%";
                        }




                    }

                    if (splithdd[i].Contains("Power Cycle Count") == true)
                    {
                        string[] pcctemp = splithdd[i].Replace('\n', ' ').Split(' ');

                        if (pcctemp.Length == 37 || pcctemp.Length == 38 || pcctemp.Length == 39 || pcctemp.Length == 40 || pcctemp.Length == 41 || pcctemp.Length == 42)
                        {
                            if (pcctemp[31] != String.Empty)
                                hddpcc += pcctemp[31] + "%";

                            if (pcctemp[32] != String.Empty)
                                hddpcc += pcctemp[32] + "%";

                            if (pcctemp[33] != String.Empty)
                                hddpcc += pcctemp[33] + "%";

                            if (pcctemp[34] != String.Empty)
                                hddpcc += pcctemp[34] + "%";

                            if (pcctemp[35] != String.Empty)
                                hddpcc += pcctemp[35] + "%";

                            if (pcctemp[36] != String.Empty)
                                hddpcc += pcctemp[36] + "%";

                            if (pcctemp[37] != String.Empty)
                                hddpcc += pcctemp[37] + "%";
                        }

                    }
                    //informações de POH//


                    //Nome específico do driver//
                    if (splithdd[i].Contains("Drive name:") == true)
                    {
                        string[] splithddnames = splithdd[i].Split(':');
                        hdddrivename += splithddnames[1] + "%" + "\n";
                    }
                    //Nome específico do driver//
                }

                string[] splitinfos = hddgeneralinfo.Split('&'); //divide as informações gerais;
                string[] hddfinalname = hdddrivename.Split('%'); //divide os nomes dos hdds;
                string[] splittempsfinal = hddtemps.Split('%'); //divide as de Temperatura;
                string[] splitpohfinal = hddpoh.Split('%'); //divide o POH;
                string[] splitpccfinal = hddpcc.Split('%'); //divide o PCC;

                string hdd1 = "";
                string hdd2 = "";
                string hdd3 = "";
                string hdd4 = "";
                string hdd5 = "";

                Console.WriteLine("");


                if (size == 1) //1 hdds
                {
                    //Nessa zona de código, eu escrevo todo o filtro que foi feito antes. A função Replace é usada 2 vezes em cadeia, para eliminar o line break e espaços em branco.

                    string[] hdd1split = splitinfos[0].Split('%');  //divisão final da infogeral hdd1

                    string[] hdd1tempsplit = splittempsfinal[0].Split('*');  //divisão final da temperatura hdd1


                    //hdd1
                    hdd1 = "Nome do HDD:" + hddfinalname[0] + "\nNome do Driver:" + hdd1split[0] + ":/" + "\nFormato do Driver: " + hdd1split[1].Replace('\n', ' ').Replace(" ", String.Empty)
                        + "\nEspaço Total: " + hdd1split[2].Replace('\n', ' ').Replace(" ", String.Empty) + "\nEspaço Livre: " + hdd1split[3].Replace('\n', ' ').Replace(" ", String.Empty)
                        + "\nPior Temperatura: " + hdd1tempsplit[0] + "ºC" + " Temperatura Atual: " + hdd1tempsplit[1] + "ºC" + "\nTempo Ligado(POH): " + splitpohfinal[0] + " Horas"
                        + "\nCiclos ON/OFF(PCC): " + splitpccfinal[0] + " Vezes";//infos
                    //hdd1

                    Console.WriteLine(hdd1);
                    Console.WriteLine("");

                }

                if (size == 2) //2 hdds
                {
                    //Nessa zona de código, eu escrevo todo o filtro que foi feito antes. A função Replace é usada 2 vezes em cadeia, para eliminar o line break e espaços em branco.

                    string[] hdd1split = splitinfos[0].Split('%');  //divisão final da infogeral hdd1
                    string[] hdd1split2 = splitinfos[1].Split('%'); //divisão final da infogeral hdd2

                    string[] hdd1tempsplit = splittempsfinal[0].Split('*');  //divisão final da temperatura hdd1
                    string[] hdd1tempsplit2 = splittempsfinal[1].Split('*'); //divisão final da temperatura hdd2


                    //hdd1
                    hdd1 = "Nome do HDD:" + hddfinalname[0] + "\nNome do Driver:" + hdd1split[0] + ":/" + "\nFormato do Driver: " + hdd1split[1].Replace('\n', ' ').Replace(" ", String.Empty)
                        + "\nEspaço Total: " + hdd1split[2].Replace('\n', ' ').Replace(" ", String.Empty) + "\nEspaço Livre: " + hdd1split[3].Replace('\n', ' ').Replace(" ", String.Empty)
                        + "\nPior Temperatura: " + hdd1tempsplit[0] + "ºC" + " Temperatura Atual: " + hdd1tempsplit[1] + "ºC" + "\nTempo Ligado(POH): " + splitpohfinal[0] + " Horas"
                        + "\nCiclos ON/OFF(PCC): " + splitpccfinal[0] + " Vezes";//infos
                    //hdd1

                    //hdd2
                    hdd2 = "Nome do HDD: " + hddfinalname[1].Replace('\n', ' ').Replace(" ", String.Empty) + "\nNome do Driver:" + hdd1split2[0] + ":/" + "\nFormato do Driver: " + hdd1split2[1].Replace('\n', ' ').Replace(" ", String.Empty)
                        + "\nEspaço Total: " + hdd1split2[2].Replace('\n', ' ').Replace(" ", String.Empty) + "\nEspaço Livre: " + hdd1split2[3].Replace('\n', ' ').Replace(" ", String.Empty)
                        + "\nPior Temperatura: " + hdd1tempsplit2[0] + "ºC" + " Temperatura Atual: " + hdd1tempsplit2[1] + "ºC" + "\nTempo Ligado(POH): " + splitpohfinal[1] + " Horas"
                        + "\nCiclos ON/OFF(PCC): " + splitpccfinal[1] + " Vezes";//infos
                                                                                 //hdd2

                    Console.WriteLine(hdd1);
                    Console.WriteLine("");
                    Console.WriteLine(hdd2);
                    Console.WriteLine("");

                }

                if (size == 3) //3 hdds
                {
                    //Nessa zona de código, eu escrevo todo o filtro que foi feito antes. A função Replace é usada 2 vezes em cadeia, para eliminar o line break e espaços em branco.

                    string[] hdd1split = splitinfos[0].Split('%');  //divisão final da infogeral hdd1
                    string[] hdd1split2 = splitinfos[1].Split('%'); //divisão final da infogeral hdd2
                    string[] hdd1split3 = splitinfos[2].Split('%'); //divisão final da infogeral hdd3

                    string[] hdd1tempsplit = splittempsfinal[0].Split('*');  //divisão final da temperatura hdd1
                    string[] hdd1tempsplit2 = splittempsfinal[1].Split('*'); //divisão final da temperatura hdd2
                    string[] hdd1tempsplit3 = splittempsfinal[2].Split('*'); //divisão final da temperatura hdd3

                    //hdd1
                    hdd1 = "Nome do HDD:" + hddfinalname[0] + "\nNome do Driver:" + hdd1split[0] + ":/" + "\nFormato do Driver: " + hdd1split[1].Replace('\n', ' ').Replace(" ", String.Empty)
                        + "\nEspaço Total: " + hdd1split[2].Replace('\n', ' ').Replace(" ", String.Empty) + "\nEspaço Livre: " + hdd1split[3].Replace('\n', ' ').Replace(" ", String.Empty)
                        + "\nPior Temperatura: " + hdd1tempsplit[0] + "ºC" + " Temperatura Atual: " + hdd1tempsplit[1] + "ºC" + "\nTempo Ligado(POH): " + splitpohfinal[0] + " Horas"
                        + "\nCiclos ON/OFF(PCC): " + splitpccfinal[0] + " Vezes";//infos
                    //hdd1

                    //hdd2
                    hdd2 = "Nome do HDD: " + hddfinalname[1].Replace('\n', ' ').Replace(" ", String.Empty) + "\nNome do Driver:" + hdd1split2[0] + ":/" + "\nFormato do Driver: " + hdd1split2[1].Replace('\n', ' ').Replace(" ", String.Empty)
                        + "\nEspaço Total: " + hdd1split2[2].Replace('\n', ' ').Replace(" ", String.Empty) + "\nEspaço Livre: " + hdd1split2[3].Replace('\n', ' ').Replace(" ", String.Empty)
                        + "\nPior Temperatura: " + hdd1tempsplit2[0] + "ºC" + " Temperatura Atual: " + hdd1tempsplit2[1] + "ºC" + "\nTempo Ligado(POH): " + splitpohfinal[1] + " Horas"
                        + "\nCiclos ON/OFF(PCC): " + splitpccfinal[1] + " Vezes";//infos
                    //hdd2

                    //hdd3
                    hdd3 = "Nome do HDD: " + hddfinalname[2].Replace('\n', ' ').Replace(" ", String.Empty) + "\nNome do Driver:" + hdd1split3[0] + ":/" + "\nFormato do Driver: " + hdd1split3[1].Replace('\n', ' ').Replace(" ", String.Empty)
                        + "\nEspaço Total: " + hdd1split3[2].Replace('\n', ' ').Replace(" ", String.Empty) + "\nEspaço Livre: " + hdd1split3[3].Replace('\n', ' ').Replace(" ", String.Empty)
                        + "\nPior Temperatura: " + hdd1tempsplit3[0] + "ºC" + " Temperatura Atual: " + hdd1tempsplit3[1] + "ºC" + "\nTempo Ligado(POH): " + splitpohfinal[2] + " Horas"
                        + "\nCiclos ON/OFF(PCC): " + splitpccfinal[2] + " Vezes";//infos

                    Console.WriteLine(hdd1);
                    Console.WriteLine("");
                    Console.WriteLine(hdd2);
                    Console.WriteLine("");
                    Console.WriteLine(hdd3);

                }


                //HDD//
                #endregion

                #region RAM

                //RAM//
                Console.WriteLine("");
                string memtConverted = "";
                string memdConverted = "";
                ulong memt = GetTotalMemoryInBytes(); //Valores da RAM Total
                ulong memd = GetavailabeMemoryInBytes();//Valores da RAM Disponível
                char[] memtc = Convert.ToString(memt).ToCharArray();
                char[] memtd = Convert.ToString(memd).ToCharArray();
                if (memtc.Length == 10) //<= 9gbderam
                {
                    memtConverted = Convert.ToString(memtc[0]) + "."  //Essa variável Organiza a Quantidade de Ram em um Valor mais amigável
                        + Convert.ToString(memtc[1]) + Convert.ToString(memtc[2])
                        + Convert.ToString(memtc[3]) + "." + Convert.ToString(memtc[4])
                        + Convert.ToString(memtc[5]) + Convert.ToString(memtc[6]);
                    Console.WriteLine("Memória Total: {0} MB", memtConverted); //Exibe os valores da RAM TOTAL

                    if (memtd.Length == 10)
                    {
                        memdConverted = Convert.ToString(memtd[0]) + "." //Essa variável Organiza a Quantidade de Ram em um Valor mais amigável
                       + Convert.ToString(memtd[1]) + Convert.ToString(memtd[2])
                       + Convert.ToString(memtd[3]) + "." + Convert.ToString(memtd[4])
                       + Convert.ToString(memtd[5]) + Convert.ToString(memtd[6]);
                    }
                    if (memtd.Length == 9)
                    {
                        memdConverted = Convert.ToString(memtd[0])//Essa variável Organiza a Quantidade de Ram em um Valor mais amigável
                       + Convert.ToString(memtd[1]) + Convert.ToString(memtd[2]) + "."
                       + Convert.ToString(memtd[3]) + Convert.ToString(memtd[4])
                       + Convert.ToString(memtd[5]);
                    }
                    Console.WriteLine("Memória Disponível: {0} MB", memdConverted);//Exibe os valores da RAM DISPONÍVEL
                }

                if (memtc.Length == 11) //<=99gbderam
                {
                    memtConverted = Convert.ToString(memtc[0]) +  //Essa variável Organiza a Quantidade de Ram em um Valor mais amigável
                         Convert.ToString(memtc[1]) + "." + Convert.ToString(memtc[2])
                        + Convert.ToString(memtc[3]) + Convert.ToString(memtc[4]) + "."
                        + Convert.ToString(memtc[5]) + Convert.ToString(memtc[6]) + Convert.ToString(memtc[7]);
                    Console.WriteLine("Memória Total: {0} MB", memtConverted); //Exibe os valores da RAM TOTAL


                    if (memtd.Length == 9) //< que 1 gb de ram
                    {
                        memdConverted = Convert.ToString(memtd[0])//Essa variável Organiza a Quantidade de Ram em um Valor mais amigável
                       + Convert.ToString(memtd[1]) + Convert.ToString(memtd[2]) + "."
                       + Convert.ToString(memtd[3]) + Convert.ToString(memtd[4])
                       + Convert.ToString(memtd[5]);
                    }

                    if (memtd.Length == 10) //<=9gbderam
                    {
                        memdConverted = Convert.ToString(memtd[0]) + "." //Essa variável Organiza a Quantidade de Ram em um Valor mais amigável
                        + Convert.ToString(memtd[1]) + Convert.ToString(memtd[2])
                       + Convert.ToString(memtd[3]) + "." + Convert.ToString(memtd[4])
                       + Convert.ToString(memtd[5]) + Convert.ToString(memtd[6]);
                        Console.WriteLine("Memória Disponível: {0} MB", memdConverted);//Exibe os valores da RAM DISPONÍVEL
                    }

                    if (memtd.Length == 11) //<=99gbderam
                    {
                        memdConverted = Convert.ToString(memtd[0]) +  //Essa variável Organiza a Quantidade de Ram em um Valor mais amigável
                         Convert.ToString(memtd[1]) + "." + Convert.ToString(memtd[2])
                        + Convert.ToString(memtd[3]) + Convert.ToString(memtd[4]) + "."
                        + Convert.ToString(memtd[5]) + Convert.ToString(memtd[6]) + Convert.ToString(memtd[7]);
                        Console.WriteLine("Memória Disponível: {0} MB", memdConverted);//Exibe os valores da RAM DISPONÍVEL
                    }



                }
                //RAM

                #endregion

                #region HTTP API CODE
                //enviohttp
                /*
                try
                {
                    HttpWebRequest request = WebRequest.Create("http://10.169.0.175:8080/"+"?"+bus+"%"+core0+"%" + core1 + "%" + core2 + "%" + core3 + "%" + memt + "%" + memd) as HttpWebRequest;
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    Console.WriteLine("Dados Enviados com sucesso");
                    //optional
                }
                catch (SystemException e)
                {
                    if (e.ToString().Contains("Unable to connect to the remote server") == true)
                    {
                        Console.WriteLine("Não foi possível conectar ao servidor, a conexão não pode ser encontrada ou foi rejeitada. \nVerificar se o Client está aberto");
                    }
                    else
                    {
                        Console.WriteLine("Não foi possível conectar ao servidor");
                    }
                    
                }
                //enviohttp
                */
                #endregion

                
            }
            
        }
        #endregion

        #region Memory Method (Visual Basic)
        static ulong GetavailabeMemoryInBytes() //método que retorna memória disponível
        {
            return new Microsoft.VisualBasic.Devices.ComputerInfo().AvailablePhysicalMemory;
        }
        static ulong GetTotalMemoryInBytes() //método que retorna memória Total
        {
            return new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory;
        }
        #endregion

        #region CPU Method's
        static string GetCpuInfo()
        {
            #region CPU
            //comments//
            #region Comentários PT-BR
            /* Esse chunk e responsável por pegar toda as informações da CPU
             * É necessário que o código se adapte de acordo com qualquer hardware
             * Toda informação precisa de separadores, para que facilite o trabalho da API de se comunicar com o servidor.
             */
            #endregion

            #region EN-US Comments
            /* - This chunk is responsible to get all CPU info -
             * - It's necessary that the code adapts itself under any hardware circustances -
             * - All the info need to be filtered with separators, to ease the API work on sending those to the server -
             */
            #endregion
            //comments//

            string cpupackinfo = "";

            #region Model and Core's Count
            //comments//
            #region Comentários PT-BR
            /* Esse chunk e responsável por pegar: Marca/modelo e a quantidade de cores
             */
            #endregion

            #region EN-US Comments
            /* - This chunk is responsible to get: Brand/model and the number of cores
             */
            #endregion
            //comments//

            string modelocpu = ""; //model info
            int cores = 0; //Core info
            bool intel = false; //Intel Brand
            bool amd = false; //AMD Brand

            foreach (var hardwareItem in myComputer.Hardware) //picking up name/brand
            {
                if (hardwareItem.HardwareType == HardwareType.CPU)
                {
                    modelocpu = hardwareItem.Name;
                }
            }

            char[] divisor = modelocpu.ToCharArray(); 

            modelocpu = "";

            for(int i =0;i<divisor.Length;i++)//method to remove any blank char's and replace them with '%' divisor
            {
                if (divisor[i] != ' ')
                    modelocpu += divisor[i];
                else
                    modelocpu += "%";
            }

            if (modelocpu.Contains("Intel") == true)//verifies brand
            {
                intel = true;
            }
            else
            {
                amd = true;
            }

            for (int i = 0; i < processorinfo.Length; i++)//Picks up the core count number.
            {
                //Nº of Cores//
                if (processorinfo[i].Contains("Number of Cores:") == true) //Contains method to find any desired info
                {
                    string[] cpucorestemp = processorinfo[i].Split(':');
                    cores = Convert.ToInt32(cpucorestemp[1].Replace(" ", String.Empty));
                }
                //Nº of Cores//
            }
            #endregion

            #region Intel CPU's

            if (intel == true)
            {

                #region CPU Load, Real Time Clock and Temperatures

                //comments//
                #region Comentários PT-BR
                /* - Esse chunk e responsável por pegar: Load da CPU, Clocks em Tempo-Real e Temperaturas 
                 * - Esse chunk já é global, e funciona com qualquer processador intel
                 */
                #endregion

                #region EN-US Comments
                /* - This chunk is responsible to get: CPU Load, Real Time Clock and Temperatures 
                 * - This chunk is already global, it works with any intel processors
                 */
                #endregion
                //comments//

                string temp = ""; // Temperatures
                string Load = ""; // Load
                string clock = ""; // Clock total

                foreach (var hardwareItem in myComputer.Hardware)
                {

                    if (hardwareItem.HardwareType == HardwareType.CPU)
                    {
                        hardwareItem.Update();
                        foreach (IHardware subHardware in hardwareItem.SubHardware)
                            subHardware.Update();

                        foreach (var sensor in hardwareItem.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Temperature)
                            {
                                temp += String.Format("{0}%", sensor.Value.HasValue ? sensor.Value.Value.ToString() : "null");
                            }
                            if (sensor.SensorType == SensorType.Load)
                            {
                                if (sensor.Value.HasValue == true)
                                    Load += String.Format("{0}%", Math.Round(Convert.ToDouble(sensor.Value)));
                                else
                                    Load += "null";
                            }
                            if (sensor.SensorType == SensorType.Clock)
                            {
                                if (sensor.Value.HasValue == true)
                                    clock += String.Format("{0}%", Math.Round(Convert.ToDouble(sensor.Value)));
                                else
                                    clock += "null";
                            }
                        }
                    }
                }
                cpupackinfo =  modelocpu + "*" + cores + "*" + clock + "*" + temp + "*" + Load ; //Each group is separated by '*' separator. Wich group content is separated by '' separator.
                #endregion

            }

            #endregion

            #region AMD CPU's
            if(amd ==true)
            {


            }
            #endregion

            return cpupackinfo;
            #endregion
        }
        #endregion

    }
}