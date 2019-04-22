using System;
using OpenHardwareMonitor.Hardware;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Management;
using WMinfo_Client;
using System.Threading.Tasks;

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
/* - Version - 1.4.1 Beta -
 * - Type: Client  -
 * - Last Updated at: 20/04/2019 (DD/MM/YYYY) -
 * 
 * 
 * - ######## What's New ######### -
 * 
 *   - Added New class "Remote Console", this class is responsible for making possible the remote console usage.
 *   the user now can execute commands and receive feedback output in real time. 
 *   Commands that are implemented and funcional: Netstat(all of variants),Chkdsk, tasklist, taskill, ipconfig and run(program).
 *   - Added New class "RemoteTaskView", this class is responsible for exibition of all processes with resources uses like task manager on windows. 
 *   The user can manipulate all of them, with possibiltity of termination of an existing process or execution of new processes. 
 *   it's in the plans to add some graphics supports aswell, with network info and processor usage.
 *   
 * - ######## What's New ######### -
 * 
 * 
 * - ######## Bugs to fix ######### -
 * 
 * 
 * - ######## Bugs to fix ######### -
 * 
 * 
 * - ######## Last Changes ######### -
 * 
 *     -----------1.4b-------------
 *     
 *  - Fixed the problem with CPU frequency not updating
 *  - Universal code for any core quantities
 *  - Added Global Support for Intel processors
 *  - Added Global Support for Nvidia GPU's
 *  - Added GetCpuInfo Method's wich brings a string HttpAPI Ready
 *  - Added GetGpuInfo Method's wich brings a string HttpAPI Ready
 *  - Added GetHDDInfo Method's wich brings a string HttpAPI Ready
 *  - Added Ping with MS returned
 *  - Fixed HDD's reading bug
 *     
 *     -----------1.4b-------------
 * 
 * 
 * 
 *     -----------1.3b-------------
 *     
 *  - Fixed the problem with CPU frequency not updating
 *  - Universal code for any core quantities
 *  - Added Global Support for Intel processors
 *  - Added Global Support for Nvidia GPU's
 *  - Added GetCpuInfo Method's wich brings a string HttpAPI Ready
 *  - Added GetGpuInfo Method's wich brings a string HttpAPI Ready
 *  - Added GetHDDInfo Method's wich brings a string HttpAPI Ready
 *  - Added Ping with MS returned
 *  - Fixed HDD's reading bug
 *     
 *     -----------1.3b-------------
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
        public static string[] globalinfo;
        public static double up;
        public static string down;
        public static bool consoleworking = false;
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
                try
                {
                    Console.Clear();
                    string info = "";
                    info = myComputer.GetReport(); // Report from DDL, here we going to pick up HDD info.
                    globalinfo = info.Split('\n'); //Split the Report in Lines, to make the reading easier.

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
                    Console.WriteLine("Getting CPU");
                    string CPUINFO = "";
                    CPUINFO = GetCpuInfo();//Calling GetCpuInfo() Method, wich return a group of information on a string format.
                    Console.WriteLine("CPU OK!");
                    #endregion

                    #region GPU GET Method()

                    //comments//
                    #region Comentários PT-BR
                    /* Esse Método está retornando um grupo de informação, dividido por '/' nessa ordem: nome,MSclock,coretemp,coreload,fan
                     * Cada grupo tem conteúdo. O conteúdo dos grupos está dividido por '&'
                     * Esse método irá tornar mais fácil trabaçhar com métodos http.
                     */
                    #endregion

                    #region EN-US Comments
                    /* This Method is returning groups of info, divided by '/' in that order: name,MSclock,coretemp,coreload,fan
                     * Each group has content, their content is divided by '&'
                     * This method will make easier to work with http methods.
                     */
                    #endregion
                    //comments//

                    string GPUINFO = "";
                    Console.WriteLine("Getting GPU");
                    GPUINFO = GetGpuInfo();  //Calling GetGpuInfo() Method, wich return a group of information on a string format.
                    Console.WriteLine("GPU OK!");
                    #endregion

                    #region HDD GET Method()

                    //comments//
                    #region Comentários PT-BR
                    /* Esse Método está retornando um grupo de informação, dividido por '/' nessa ordem: nome,drivername,driverformat,totalspace,emptyspace,worsttemp,actualtemp,power on hours(POH),power Cycle Count(PCC)
                     * Cada grupo tem conteúdo. O conteúdo dos grupos está dividido por '&'
                     * Esse método irá tornar mais fácil trabaçhar com métodos http.
                     */
                    #endregion

                    #region EN-US Comments
                    /* This Method is returning groups of info, divided by '/' in that order: name,drivername,driverformat,totalspace,emptyspace,worsttemp,actualtemp,power on hours(POH),power Cycle Count(PCC)
                     * Each group has content, their content is divided by '&'
                     * This method will make easier to work with http methods.
                     */
                    #endregion
                    //comments//

                    Console.WriteLine("Getting HDD");
                    string HDDINFO = "";
                    HDDINFO = GetHddInfo();
                    Console.WriteLine("HDD OK!");

                    #endregion

                    #region RAM

                    //comments//
                    #region Comentários PT-BR
                    /* Esse Método está retornando um grupo de informação, dividido por '/' nessa ordem: Ram Total, Ram disponível
                     * Cada grupo tem conteúdo. O conteúdo dos grupos está dividido por '&'
                     * Esse método irá tornar mais fácil trabaçhar com métodos http.
                     */
                    #endregion

                    #region EN-US Comments
                    /* This Method is returning groups of info, divided by '/' in that order: Total Ram, Availabe Ram
                     * Each group has content, their content is divided by '&'
                     * This method will make easier to work with http methods.
                     */
                    #endregion
                    //comments//
                    Console.WriteLine("Getting RAM");
                    string RAMINFO = "";
                    RAMINFO = GetRamInfo();
                    Console.WriteLine("RAM OK!");
                    #endregion

                    #region Ping Get Method()

                    //comments//
                    #region Comentários PT-BR
                    /* esse método está retornado o delay de ping em mili-segundos, baseado em um ping client-server .
                     * 
                     */
                    #endregion

                    #region EN-US Comments
                    /* this method is getting the ping delay, based on a client-server ping.
                     */
                    #endregion
                    //comments//

                    Console.WriteLine("Pinging!");
                    string PINGINFO = "";
                    PINGINFO = GetPingMs();
                    Console.WriteLine("Sucess!, Ping: {0}ms", PINGINFO);

                    #endregion

                    #region Get NetWorkStatistics Method()
                    string NETWORKINFO = "";
                    NETWORKINFO = GetNetworkInfo();
                    #endregion

                    #region MOBO Get Method()

                    string MOBOINFO = "";
                    MOBOINFO = GetMoboInfo();

                    #endregion

                    #region HTTP API CODE
                    //enviohttp



                    HttpWebRequest request = WebRequest.Create("http://192.168.15.10:8080/" + "?" + CPUINFO + "$" + GPUINFO + "$" + HDDINFO + "$" + RAMINFO + "$" + PINGINFO + "$" + NETWORKINFO + "$" + MOBOINFO) as HttpWebRequest;
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                    Console.WriteLine("Dados Enviados com sucesso");
                    Console.WriteLine("Resposta do servidor: {0}", responseString);

                    if (responseString.Contains("Console"))
                    {
                        string[] command = responseString.Split('*');
                        RemoteConsole console = new RemoteConsole();
                        console.command = command[1];
                        Task.Run (()=>console.ConsoleWork());
                    }

                    if (responseString.Contains("TaskView"))
                    {
                        RemoteTaskViewer task = new RemoteTaskViewer();
                        task.RTVWORK();
                    }

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

                #endregion

            }
            
        }
        #endregion

        #region NetWork Method's

        static string GetNetworkInfo()
        {
            System.Net.NetworkInformation.NetworkInterface[] nics;
            nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();
            PerformanceCounterCategory performanceCounterCategory = new PerformanceCounterCategory("Network Interface");
            string instance = "";


            foreach (var nic in nics)
            {
                if (nic.NetworkInterfaceType != NetworkInterfaceType.Wireless80211 && nic.NetworkInterfaceType != NetworkInterfaceType.Ethernet)
                    continue;
                if (nic.OperationalStatus.ToString().ToUpper() != "DOWN")
                {
                    Console.WriteLine("Adapter name: " + nic.Name);
                    Console.WriteLine("Link speed: " + nic.Speed);
                    Console.WriteLine("State: " + nic.OperationalStatus);
                    instance = nic.Description;
                }
            }

            while (true)
            {
                PerformanceCounter kek = new PerformanceCounter("Network Interface", "Bytes Received/sec", instance);
                PerformanceCounter kek1 = new PerformanceCounter("Network Interface", "Bytes Sent/sec", instance);
                double down = 0;
                double up = 0;
                string dspeed = "";
                string uspeed = "";
                string netspeed = "";
                kek.NextValue();
                kek1.NextValue();
                for (int i = 0; i < 10; i++)
                {
                    float number = (kek.NextValue() / 1024);
                    float number1 = (kek1.NextValue() / 1024);
                    down += Math.Round(number);
                    up += Math.Round(number1);
                    Thread.Sleep(300);
                }

                double download = down / 10;
                double upload = up / 10;

                if (download > 1000)
                {
                    download = Math.Round(download / 1000, 2);
                    dspeed = download + "-Mbs";
                }
                else
                {
                    download = Math.Round(download);
                    dspeed = download + "-Kpbs";
                }

                if (upload > 1000)
                {
                    upload = Math.Round(upload / 1000,2);
                    uspeed = upload + "-Mbs";
                }
                else
                {
                   upload = Math.Round(upload);
                   uspeed = upload + "-Kbps";
                }

                netspeed = dspeed.Replace(",", "&") + "/" + uspeed.Replace(",", "&");
                return netspeed;
                
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

        #region MOBO Method's

        private static ManagementObjectSearcher baseboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
        private static ManagementObjectSearcher motherboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_MotherboardDevice");


        static public string GetMoboInfo()
        {
            string info = "";

            info  = Manufacturer.Replace(" ", "&").Replace(".", String.Empty) + "/" + Product.Replace(" ", "&").Replace(".", String.Empty) + "/" + SerialNumber.Replace(" ", "&").Replace(".", String.Empty) + "/" + SystemName.Replace(" ", "&").Replace(".", String.Empty) + "/" + Version.Replace(" ", "&").Replace(".", String.Empty);

            return info;
        }



        static public string Manufacturer
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["Manufacturer"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }

        static public string Product
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["Product"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }


        static public string SerialNumber
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["SerialNumber"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }

        static public string SystemName
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in motherboardSearcher.Get())
                    {
                        return queryObj["SystemName"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }

        static public string Version
        {
            get
            {
                try
                {
                    foreach (ManagementObject queryObj in baseboardSearcher.Get())
                    {
                        return queryObj["Version"].ToString();
                    }
                    return "";
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }


        #endregion

        #region RAM Method's

        static string GetRamInfo()
        {
            string memtotal = "";
            string memtConverted = "";
            string memdConverted = "";
            ulong memt = GetTotalMemoryInBytes(); //Valores da RAM Total
            ulong memd = GetavailabeMemoryInBytes();//Valores da RAM Disponível
            char[] memtc = Convert.ToString(memt).ToCharArray();
            char[] memtd = Convert.ToString(memd).ToCharArray();
            if (memtc.Length == 10) //<= 9gbderam
            {
                memtConverted = Convert.ToString(memtc[0]) + "&"  //Essa variável Organiza a Quantidade de Ram em um Valor mais amigável
                    + Convert.ToString(memtc[1]) + Convert.ToString(memtc[2])
                    + Convert.ToString(memtc[3]) + "&" + Convert.ToString(memtc[4])
                    + Convert.ToString(memtc[5]) + Convert.ToString(memtc[6]);


                if (memtd.Length == 10)
                {
                    memdConverted = Convert.ToString(memtd[0]) + "&" //Essa variável Organiza a Quantidade de Ram em um Valor mais amigável
                   + Convert.ToString(memtd[1]) + Convert.ToString(memtd[2])
                   + Convert.ToString(memtd[3]) + "&" + Convert.ToString(memtd[4])
                   + Convert.ToString(memtd[5]) + Convert.ToString(memtd[6]);
                }
                if (memtd.Length == 9)
                {
                    memdConverted = Convert.ToString(memtd[0])//Essa variável Organiza a Quantidade de Ram em um Valor mais amigável
                   + Convert.ToString(memtd[1]) + Convert.ToString(memtd[2]) + "&"
                   + Convert.ToString(memtd[3]) + Convert.ToString(memtd[4])
                   + Convert.ToString(memtd[5]);
                }

                memtotal = memtConverted + "/" + memdConverted;
            }

            if (memtc.Length == 11) //<=99gbderam
            {
                memtConverted = Convert.ToString(memtc[0]) +  //Essa variável Organiza a Quantidade de Ram em um Valor mais amigável
                     Convert.ToString(memtc[1]) + "&" + Convert.ToString(memtc[2])
                    + Convert.ToString(memtc[3]) + Convert.ToString(memtc[4]) + "&"
                    + Convert.ToString(memtc[5]) + Convert.ToString(memtc[6]) + Convert.ToString(memtc[7]);
                


                if (memtd.Length == 9) //< que 1 gb de ram
                {
                    memdConverted = Convert.ToString(memtd[0])//Essa variável Organiza a Quantidade de Ram em um Valor mais amigável
                   + Convert.ToString(memtd[1]) + Convert.ToString(memtd[2]) + "&"
                   + Convert.ToString(memtd[3]) + Convert.ToString(memtd[4])
                   + Convert.ToString(memtd[5]);
                }

                if (memtd.Length == 10) //<=9gbderam
                {
                    memdConverted = Convert.ToString(memtd[0]) + "&" //Essa variável Organiza a Quantidade de Ram em um Valor mais amigável
                    + Convert.ToString(memtd[1]) + Convert.ToString(memtd[2])
                   + Convert.ToString(memtd[3]) + "&" + Convert.ToString(memtd[4])
                   + Convert.ToString(memtd[5]) + Convert.ToString(memtd[6]);
                   
                }

                if (memtd.Length == 11) //<=99gbderam
                {
                    memdConverted = Convert.ToString(memtd[0]) +  //Essa variável Organiza a Quantidade de Ram em um Valor mais amigável
                     Convert.ToString(memtd[1]) + "&" + Convert.ToString(memtd[2])
                    + Convert.ToString(memtd[3]) + Convert.ToString(memtd[4]) + "&"
                    + Convert.ToString(memtd[5]) + Convert.ToString(memtd[6]) + Convert.ToString(memtd[7]);
                    
                }

                memtotal = memtConverted + "/" + memdConverted;

            }

            return memtotal;
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
                    modelocpu += "&";
            }

            if (modelocpu.Contains("Intel") == true)//verifies brand
            {
                intel = true;
            }
            else
            {
                amd = true;
            }

            for (int i = 0; i < globalinfo.Length; i++)//Picks up the core count number.
            {
                //Nº of Cores//
                if (globalinfo[i].Contains("Number of Cores:") == true) //Contains method to find any desired info
                {
                    string[] cpucorestemp = globalinfo[i].Split(':');
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
                                temp += String.Format("{0}&", sensor.Value.HasValue ? sensor.Value.Value.ToString() : "null");
                            }
                            if (sensor.SensorType == SensorType.Load)
                            {
                                if (sensor.Value.HasValue == true)
                                    Load += String.Format("{0}&", Math.Round(Convert.ToDouble(sensor.Value)));
                                else
                                    Load += "null";
                            }
                            if (sensor.SensorType == SensorType.Clock)
                            {
                                if (sensor.Value.HasValue == true)
                                    clock += String.Format("{0}&", Math.Round(Convert.ToDouble(sensor.Value)));
                                else
                                    clock += "null";
                            }
                        }
                    }
                }
                cpupackinfo =  modelocpu + "/" + cores + "/" + clock + "/" + temp + "/" + Load ; //Each group is separated by '*' separator. Wich group content is separated by '' separator.
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

        #region GPU Method's
        static string GetGpuInfo()
        {
            #region GPU

            #region GPU Model'n Brand

            #region Comentários PT-BR
            /* Esse chunk é responsável por obter o Modelo da GPU
             */
            #endregion

            #region EN-US Comments
            /* This chunk is responsible for getting Gpu Models and Brand.
            */
            #endregion

            bool nvidia = false;
            bool amd = false;
            string models = "";
            foreach (var hardwareItem in myComputer.Hardware)
            {
                if (hardwareItem.HardwareType == HardwareType.GpuNvidia)
                {
                    if (hardwareItem.Name.ToLower().Contains("nvidia"))
                        nvidia = true;
                    else
                        amd = true;
                    models = hardwareItem.Name;

                }
            }

            char[] divisor = models.ToCharArray();

            models = "";

            for (int i = 0; i < divisor.Length; i++)//method to remove any blank char's and replace them with '%' divisor
            {
                if (divisor[i] != ' ')
                    models += divisor[i];
                else
                    models += "&";
            }
            #endregion

            #region NVIDIA

            string gpupackinfo = "";
            string name = "";
            name = models;
            string Coretemp = ""; // Core Temperatures Cº &
            string CoreLoad = ""; // Core Load &
            string MSclock = ""; // Memory and Shader Clock &
            string fan = ""; // Fan &

            if (nvidia == true)
            {
                #region Comentários PT-BR
                /* Esse chunk é responsável por obter: Informações da GPU, como temperatura, clocks, load e fan
                 */
                #endregion

                #region EN-US Comments
                /* This chunk is responsible for getting: GPU info, like: Temps, Clocks, load level and fan level.
                */
                #endregion

                

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
                                Coretemp += String.Format("{0}", sensor.Value.HasValue ? sensor.Value.Value.ToString() : "null");
                            }
                            if (sensor.SensorType == SensorType.Load)
                            {
                                if (sensor.Value.HasValue == true)
                                    CoreLoad += String.Format("{0}&", Math.Round(Convert.ToDouble(sensor.Value)));
                                else
                                    CoreLoad += "null";
                            }
                            if (sensor.SensorType == SensorType.Clock)
                            {
                                if (sensor.Value.HasValue == true)
                                    MSclock += String.Format("{0}&", Math.Round(Convert.ToDouble(sensor.Value)));
                                else
                                    MSclock += "null";
                            }
                            if (sensor.SensorType == SensorType.Fan)
                            {
                                if (sensor.Value.HasValue == true)
                                    fan += String.Format("{0}", Math.Round(Convert.ToDouble(sensor.Value)));
                                else
                                    fan += "null";
                            }
                        }

                    }
                }

               
            }

            return gpupackinfo = name + "/" + MSclock + "/" + Coretemp + "/" + CoreLoad + "/" + fan; 

            #endregion

            #endregion

        }
        #endregion

        #region HDD Method's

        static string GetHddInfo()
        {

            #region HDD

            #region HDD Gerenal Info

            //comments//
            #region Comentários PT-BR
            /* Esse chunk e responsável por pegar toda as informações do HDD
             * É necessário que o código se adapte de acordo com qualquer hardware
             * Toda informação precisa de separadores, para que facilite o trabalho da API de se comunicar com o servidor.
             */
            #endregion

            #region EN-US Comments
            /* - This chunk is responsible to get all HDD info -
             * - It's necessary that the code adapts itself under any hardware circustances -
             * - All the info need to be filtered with separators, to ease the API work on sending those to the server -
             */
            #endregion
            //comments//

            int count = 0;
            string hddtemps = "";
            string hddgeneralinfo = "";
            string hdddrivename = "";
            string hddpoh = "";
            string hddpcc = "";
            int size = 0;

            for (int i = 0; i < globalinfo.Length; i++)
            {

                #region General Info

                //comments//
                #region Comentários PT-BR
                /* Esse chunk e responsável por pegar toda as informações básicas do HDD e filtrar unidades fantasmas e partições
                 */
                #endregion

                #region EN-US Comments
                /* - This chunk is responsible to get all HDD info,filter partitions and non-physical false-positive Hdd's
                 */
                #endregion
                //comments//

                if (globalinfo[i].Contains("Logical drive name:") == true)
                {
                    string hddtest = ""; //string para armazenamento temporário
                    bool test = false; //bool que fará avaliação do hdd

                    for (int l = i; l < globalinfo.Length; l++)
                    {
                        string[] globalinfos = globalinfo[i + count].Split(':');

                        if (count == 2)
                        {
                            char[] counterino = globalinfos[1].ToCharArray();

                            if (counterino.Length < 13) //verifica se o hdd é real ou é uma partição do sistema
                                test = false;
                            if (counterino.Length >= 13) //verifica se o hdd é real ou é uma partição do sistema
                                test = true;
                        }

                        if (count == 3)
                        {
                            hddtest += globalinfos[1] + "%" + "\n&";
                            count = 0;
                            break;
                        }

                        hddtest += globalinfos[1] + "%" + "\n";
                        count++;
                    }

                    if (test == true)
                        hddgeneralinfo += hddtest;
                }
                #endregion

                #region Temperatures



                if (globalinfo[i].Contains("C2 Temperature") == true)
                {
                    size++;
                    string[] stringtemp = globalinfo[i].Replace('\n', ' ').Split(' ');

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


                #endregion

                #region Power-On Hours(POH)

                if (globalinfo[i].Contains("Power-On Hours (POH)") == true)
                {
                    string[] pohtemp = globalinfo[i].Replace('\n', ' ').Split(' ');

                    if (pohtemp.Length == 35)
                        hddpoh += pohtemp[31] + "%";

                    if (pohtemp.Length == 36)
                        hddpoh += pohtemp[32] + "%";

                    if (pohtemp.Length == 38)
                        hddpoh += pohtemp[33] + "%";

                    if (pohtemp.Length == 37)
                        hddpoh += pohtemp[33] + "%";

                }
                #endregion

                #region Power Cycle-Count (PCC)
                if (globalinfo[i].Contains("Power Cycle Count") == true)
                {
                    string[] pcctemp = globalinfo[i].Replace('\n', ' ').Split(' ');

                    if (pcctemp.Length == 39)
                        hddpcc += pcctemp[34] + "%";

                    if (pcctemp.Length == 40)
                        hddpcc += pcctemp[35] + "%";

                    if (pcctemp.Length == 41)
                        hddpcc += pcctemp[36] + "%";


                }
                #endregion

                #region FullDriveName

                if (globalinfo[i].Contains("Drive name:") == true)
                {
                    string[] splithddnames = globalinfo[i].Split(':');
                    hdddrivename += splithddnames[1] + "%" + "\n";
                }
                #endregion

            }

            #endregion

            #region Info Organization and Variables
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
            string hddfinal = "";


            //Nessa zona de código, eu escrevo todo o filtro que foi feito antes. A função Replace é usada 2 vezes em cadeia, para eliminar o line break e espaços em branco.
            if (size == 1)
            {
                string[] hdd1split = splitinfos[0].Split('%');  //divisão final da infogeral hdd1

                string[] hdd1tempsplit = splittempsfinal[0].Split('*');  //divisão final da temperatura hdd1

                #region HDD1
                string[] hdd1a = new string[9];

                hdd1a[0] = hddfinalname[0].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//HDD Name
                hdd1a[1] = hdd1split[0].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty); //DriveName
                hdd1a[2] = hdd1split[1].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//DriveFormat
                hdd1a[3] = hdd1split[2].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//TotalSpace
                hdd1a[4] = hdd1split[3].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//EmptySpace
                hdd1a[5] = hdd1tempsplit[0];//WorstTemp
                hdd1a[6] = hdd1tempsplit[1];//ActualTemp
                hdd1a[7] = splitpohfinal[0];//Power on Hours (POH)
                hdd1a[8] = splitpccfinal[0];//Power Cycle Count (PCC)

                for (int i = 0; i < hdd1a.Length; i++)
                {
                    if (i < 8)
                        hdd1 += hdd1a[i] + "&";
                    if (i == 8)
                        hdd1 += hdd1a[i];
                }
                #endregion

                hddfinal = hdd1;
            }


            if (size == 2) //2 hdds
            {
                //Nessa zona de código, eu escrevo todo o filtro que foi feito antes. A função Replace é usada 2 vezes em cadeia, para eliminar o line break e espaços em branco.

                string[] hdd1split = splitinfos[0].Split('%');  //divisão final da infogeral hdd1
                string[] hdd1split2 = splitinfos[1].Split('%'); //divisão final da infogeral hdd2

                string[] hdd1tempsplit = splittempsfinal[0].Split('*');  //divisão final da temperatura hdd1
                string[] hdd1tempsplit2 = splittempsfinal[1].Split('*'); //divisão final da temperatura hdd2

                #region HDD1
                string[] hdd1a = new string[9];
                //
                hdd1a[0] = hddfinalname[0].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//HDD Name
                hdd1a[1] = hdd1split[0].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty); //DriveName
                hdd1a[2] = hdd1split[1].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//DriveFormat
                hdd1a[3] = hdd1split[2].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//TotalSpace
                hdd1a[4] = hdd1split[3].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//EmptySpace
                hdd1a[5] = hdd1tempsplit[0];//WorstTemp
                hdd1a[6] = hdd1tempsplit[1];//ActualTemp
                hdd1a[7] = splitpohfinal[0];//Power on Hours (POH)
                hdd1a[8] = splitpccfinal[0];//Power Cycle Count (PCC)

                for (int i = 0; i < hdd1a.Length; i++)
                {
                    if (i < 8)
                        hdd1 += hdd1a[i] + "&";
                    if (i == 8)
                        hdd1 += hdd1a[i];
                }
                #endregion

                #region HDD2
                string[] hdd2a = new string[9];

                hdd2a[0] = hddfinalname[1].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//HDD Name
                hdd2a[1] = hdd1split2[0].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty); //DriveName
                hdd2a[2] = hdd1split2[1].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//DriveFormat
                hdd2a[3] = hdd1split2[2].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//TotalSpace
                hdd2a[4] = hdd1split2[3].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//EmptySpace
                hdd2a[5] = hdd1tempsplit2[0];//WorstTemp
                hdd2a[6] = hdd1tempsplit2[1];//ActualTemp
                hdd2a[7] = splitpohfinal[1];//Power on Hours (POH)
                hdd2a[8] = splitpccfinal[1];//Power Cycle Count (PCC)

                for (int i = 0; i < hdd2a.Length; i++)
                {
                    if (i < 8)
                        hdd2 += hdd2a[i] + "&";
                    if (i == 8)
                        hdd2 += hdd2a[i];
                }
                #endregion

                hddfinal = hdd1 + "/" + hdd2;
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

                #region HDD1
                string[] hdd1a = new string[9];

                hdd1a[0] = hddfinalname[0].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//HDD Name
                hdd1a[1] = hdd1split[0].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty); //DriveName
                hdd1a[2] = hdd1split[1].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//DriveFormat
                hdd1a[3] = hdd1split[2].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//TotalSpace
                hdd1a[4] = hdd1split[3].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//EmptySpace
                hdd1a[5] = hdd1tempsplit[0];//WorstTemp
                hdd1a[6] = hdd1tempsplit[1];//ActualTemp
                hdd1a[7] = splitpohfinal[0];//Power on Hours (POH)
                hdd1a[8] = splitpccfinal[0];//Power Cycle Count (PCC)

                for (int i = 0; i < hdd1a.Length; i++)
                {
                    if (i < 8)
                        hdd1 += hdd1a[i] + "&";
                    if (i == 8)
                        hdd1 += hdd1a[i];
                }
                #endregion

                #region HDD2
                string[] hdd2a = new string[9];

                hdd2a[0] = hddfinalname[1].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//HDD Name
                hdd2a[1] = hdd1split2[0].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty); //DriveName
                hdd2a[2] = hdd1split2[1].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//DriveFormat
                hdd2a[3] = hdd1split2[2].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//TotalSpace
                hdd2a[4] = hdd1split2[3].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//EmptySpace
                hdd2a[5] = hdd1tempsplit2[0];//WorstTemp
                hdd2a[6] = hdd1tempsplit2[1];//ActualTemp
                hdd2a[7] = splitpohfinal[1];//Power on Hours (POH)
                hdd2a[8] = splitpccfinal[1];//Power Cycle Count (PCC)

                for (int i = 0; i < hdd2a.Length; i++)
                {
                    if (i < 8)
                        hdd2 += hdd2a[i] + "&";
                    if (i == 8)
                        hdd2 += hdd2a[i];
                }
                #endregion

                #region HDD3
                string[] hdd3a = new string[9];

                hdd3a[0] = hddfinalname[2].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//HDD Name
                hdd3a[1] = hdd1split3[0].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty); //DriveName
                hdd3a[2] = hdd1split3[1].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//DriveFormat
                hdd3a[3] = hdd1split3[2].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//TotalSpace
                hdd3a[4] = hdd1split3[3].Replace("\r", "").Replace("\t", "").Replace("\n", "").Replace(" ", String.Empty);//EmptySpace
                hdd3a[5] = hdd1tempsplit3[0];//WorstTemp
                hdd3a[6] = hdd1tempsplit3[1];//ActualTemp
                hdd3a[7] = splitpohfinal[2];//Power on Hours (POH)
                hdd3a[8] = splitpccfinal[2];//Power Cycle Count (PCC)

                for (int i = 0; i < hdd3a.Length; i++)
                {
                    if (i < 8)
                        hdd3 += hdd3a[i] + "&";
                    if (i == 8)
                        hdd3 += hdd3a[i];
                }
                #endregion

                hddfinal = hdd1 + "/" + hdd2 + "/" + hdd3;
            }
           
            return hddfinal;
            #endregion

            #endregion

        }

        #endregion

        #region Ping Method's
        static string GetPingMs()
        {
            Ping ping = new Ping();
            string ms = "";

            try
            {
                PingReply reply = ping.Send("8.8.8.8");
                ms = reply.RoundtripTime.ToString();
            }
            catch(PingException)
            {

            }

            return ms;
        }
        #endregion


    }

    
}