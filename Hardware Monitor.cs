using System;
using System.Management;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Linq;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Net.NetworkInformation;
using Microsoft.Win32;

namespace Windows_Hardware_Monitor
{
    public partial class hardwaremonitor : Form
    {

        public hardwaremonitor()
        {
            InitializeComponent();
            SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;
            this.components = new System.ComponentModel.Container();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.contextMenu1.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] { this.menuItem1 });
            this.menuItem1.Index = 0;
            this.menuItem1.Text = "E&xit";
            this.menuItem1.Click += new System.EventHandler(this.menuItem1_Click);
            this.Size = new System.Drawing.Size(923, 400);
            notifyIcon1.ContextMenu = this.contextMenu1;
            notifyIcon1.Visible = true;
        }

        #region//PUBLIC
        private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		public string path1 = Directory.GetCurrentDirectory();
		public StreamReader sr;
		public StreamWriter sw;
        public int onetimesaver;
		public int batterycapacity1;
		public int Gethardwarenamescounter;
		public int batteryhealthtext;
		public string checkexit;
		public string checktheme;
		public string settingpath;
		public string savebatterytimechecker;
        public string savenetworkchecker;
        float averagebatteryusage;
		public int colorindex = 0;
		public int colorindex2 = 0;
		public int bgch = 0;
		public int counterfulltimemin;
		public int counter;
        public int showcounter;
		public float avedc;
		public int bcapa;
		public int counterfulltime;
		public int batterycapa;
		public string svbatterych;
		public string batteryname;
		public string cputemp1;
		public int corecounter;
		public int checkbatterystatus;
		public int ramSizeInfo;
        public string memorytype;
        public float chargewatt;

        public int networkhover1 = 0;
        public int networkhover2 = 0;

        public int diskcounter;
		public string disk1;
		public string disk2;
		public string disk3;
		public string disk4;
		public string disk5;
		public string disk6;
		public string disk7;

		public int networkadaptercounter;
		public string adapter1name;
		public string adapter2name;

		public string datetime = DateTime.Now.ToString();
        public string bdatapath;

		public string Rdisk1data; public string Wdisk1data;
		public string Rdisk2data; public string Wdisk2data;
		public string Rdisk3data; public string Wdisk3data;
		public string Rdisk4data; public string Wdisk4data;

		public long Rdisk1int; public long Wdisk1int;
		public long Rdisk2int; public long Wdisk2int;
		public long Rdisk3int; public long Wdisk3int;
		public long Rdisk4int; public long Wdisk4int;

		public string Radapter1data; public string Sadapter1data;
		public string Radapter2data; public string Sadapter2data;
		public int Radapter1value = 0; public int Radapter1oldvalue;
		public int Sadapter1value = 0; public int Sadapter1oldvalue;
		public int Radapter2value = 0; public int Radapter2oldvalue;
		public int Sadapter2value = 0; public int Sadapter2oldvalue;

        public string pathimage = null;
        public int remaincapa = 0;
        public int avfunc;
        public int avenetworktime;
        public float ndownload1;
        public float nupload1;
        public float ndownload2;
        public float nupload2;
        public float ndownload1data;
        public float nupload1data;
        public float ndownload2data;
        public float nupload2data;

        public int dcline = 0;
		public int lfcline = 0;
		public int linecounter = 0;
		public string dcnumber = null;
        #endregion//PUBLIC


        #region//Performance Counter
        PerformanceCounter perfSysCounter = new PerformanceCounter("System", "System Up Time");
        PerformanceCounter perfCPUCounter = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
        PerformanceCounter perfMemCounter = new PerformanceCounter("Memory", "Available MBytes");

        PerformanceCounter perfCHARGE = new PerformanceCounter();
        PerformanceCounter perfDCHARGE = new PerformanceCounter();
        PerformanceCounter perfRCAPA = new PerformanceCounter();
        PerformanceCounter perfVOLT = new PerformanceCounter();

        PerformanceCounter Rdisk1 = new PerformanceCounter("LogicalDisk", "Disk Read Bytes/sec"); PerformanceCounter Wdisk1 = new PerformanceCounter("LogicalDisk", "Disk Write Bytes/sec");
        PerformanceCounter Rdisk2 = new PerformanceCounter("LogicalDisk", "Disk Read Bytes/sec"); PerformanceCounter Wdisk2 = new PerformanceCounter("LogicalDisk", "Disk Write Bytes/sec");
        PerformanceCounter Rdisk3 = new PerformanceCounter("LogicalDisk", "Disk Read Bytes/sec"); PerformanceCounter Wdisk3 = new PerformanceCounter("LogicalDisk", "Disk Write Bytes/sec");
        PerformanceCounter Rdisk4 = new PerformanceCounter("LogicalDisk", "Disk Read Bytes/sec"); PerformanceCounter Wdisk4 = new PerformanceCounter("LogicalDisk", "Disk Write Bytes/sec");

        PerformanceCounter FPdisk1 = new PerformanceCounter("LogicalDisk", "% Free Space"); PerformanceCounter Fdisk1 = new PerformanceCounter("LogicalDisk", "Free Megabytes");
        PerformanceCounter FPdisk2 = new PerformanceCounter("LogicalDisk", "% Free Space"); PerformanceCounter Fdisk2 = new PerformanceCounter("LogicalDisk", "Free Megabytes");
        PerformanceCounter FPdisk3 = new PerformanceCounter("LogicalDisk", "% Free Space"); PerformanceCounter Fdisk3 = new PerformanceCounter("LogicalDisk", "Free Megabytes");
        PerformanceCounter FPdisk4 = new PerformanceCounter("LogicalDisk", "% Free Space"); PerformanceCounter Fdisk4 = new PerformanceCounter("LogicalDisk", "Free Megabytes");

        PerformanceCounter core0 = new PerformanceCounter("Processor Information", "% Processor Time", "0,0");
        PerformanceCounter core1 = new PerformanceCounter("Processor Information", "% Processor Time", "0,1");
        PerformanceCounter core2 = new PerformanceCounter("Processor Information", "% Processor Time", "0,2");
        PerformanceCounter core3 = new PerformanceCounter("Processor Information", "% Processor Time", "0,3");
        PerformanceCounter core4 = new PerformanceCounter("Processor Information", "% Processor Time", "0,4");
        PerformanceCounter core5 = new PerformanceCounter("Processor Information", "% Processor Time", "0,5");
        PerformanceCounter core6 = new PerformanceCounter("Processor Information", "% Processor Time", "0,6");
        PerformanceCounter core7 = new PerformanceCounter("Processor Information", "% Processor Time", "0,7");
        PerformanceCounter core8 = new PerformanceCounter("Processor Information", "% Processor Time", "0,8");
        PerformanceCounter core9 = new PerformanceCounter("Processor Information", "% Processor Time", "0,9");
        PerformanceCounter core10 = new PerformanceCounter("Processor Information", "% Processor Time", "0,10");
        PerformanceCounter core11 = new PerformanceCounter("Processor Information", "% Processor Time", "0,11");
        PerformanceCounter core12 = new PerformanceCounter("Processor Information", "% Processor Time", "0,12");
        PerformanceCounter core13 = new PerformanceCounter("Processor Information", "% Processor Time", "0,13");
        PerformanceCounter core14 = new PerformanceCounter("Processor Information", "% Processor Time", "0,14");
        PerformanceCounter core15 = new PerformanceCounter("Processor Information", "% Processor Time", "0,15");

        PerformanceCounter Radapter1 = new PerformanceCounter("Network Interface", "Bytes Received/sec"); PerformanceCounter Sadapter1 = new PerformanceCounter("Network Interface", "Bytes Sent/sec");
        PerformanceCounter Radapter2 = new PerformanceCounter("Network Interface", "Bytes Received/sec"); PerformanceCounter Sadapter2 = new PerformanceCounter("Network Interface", "Bytes Sent/sec");
        #endregion//Performance Counter
        private void Batterycapacity()
        {
            PowerStatus status = SystemInformation.PowerStatus;
            batterycapacity1 = (int)(status.BatteryLifePercent * 100);
        }
        

    private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                File.Copy(Path.Combine(Application.StartupPath, "customimage1.jpg"), Path.Combine(Application.StartupPath, "customimage.jpg"), true);
            }
            catch { }
            textcolor.SelectedIndex = 5;
            backgroundcolor.SelectedIndex = 4;
            #region//Read Settings
            settingpath = path1 + "/Settings.cfg";
            StreamReader sr = new StreamReader(settingpath);
            checkexit = sr.ReadLine();
            checktheme = sr.ReadLine();
            savebatterytimechecker = sr.ReadLine();
            savenetworkchecker = sr.ReadLine();
            memoryunit.SelectedItem = sr.ReadLine().Substring(14);
            bvoltageunit.SelectedItem = sr.ReadLine().Substring(23);
            bchargerateunit.SelectedItem = sr.ReadLine().Substring(31);
            cputemperature.SelectedItem = sr.ReadLine().Substring(23);
            networkunit.SelectedItem = sr.ReadLine().Substring(15);
            batterycapacityunit.SelectedItem = sr.ReadLine().Substring(24);
            batterytimeunit.SelectedItem = sr.ReadLine().Substring(20);
            systemuptimeunit.SelectedItem = sr.ReadLine().Substring(22);
            pyhsicaldiskcapacityunit.SelectedItem = sr.ReadLine().Substring(25);
            physicaldiskreadunit.SelectedItem = sr.ReadLine().Substring(27);
            if (checktheme == "Auto Save Theme = 0")
            {
                textcolor.SelectedItem = sr.ReadLine().Substring(13);
                backgroundcolor.SelectedItem = sr.ReadLine().Substring(19);
                backgroundimg.SelectedItem = sr.ReadLine().Substring(19);
            }
            sr.Close();
            #endregion//Read Settings

            //powercfg();
            onetimesaver = 0;
            #region//CheckBoxState
            if (checkexit == "Minimize to taskbar with close(X) button = 0"){mtaskbar.Checked = true;}
            else{mtaskbar.Checked = false;}
            if (checktheme == "Auto Save Theme = 0"){savetheme.Checked = true;}
            else{savetheme.Checked = false;}
            if (savebatterytimechecker == "Save Battery Charging Time = 0"){savebatterytime.Checked = true;}
            else{savebatterytime.Checked = false;}
            if (savenetworkchecker == "Save Average Network Usage And Speed Data = 0"){savenetwork.Checked = true;}
            else {savenetwork.Checked = false;}
            #endregion
            counter = 0;
            showcounter = 0;
            avedc = 0;
            checkbatterystatus = 0;
            Gethardwarenamescounter = 0;
            Gethardwarenames();
            GetRamSize();
            Gethardwarenamescounter = 1;
            counterfulltime = 0;
            batterycapa = 0;
            avenetworktime = 0;
            ndownload1 = 0;
            nupload1 = 0;
            ndownload2 = 0;
            nupload2 = 0;
        }
        
        private void Timer1_Tick(object sender, EventArgs e)
        {
            datetime = DateTime.Now.ToString();
            Cpufrequency();
            #region//Write Settings
            using (StreamWriter sw = new StreamWriter(settingpath))
            {
                sw.WriteLine(checkexit);
                sw.WriteLine(checktheme);
                sw.WriteLine(savebatterytimechecker);
                sw.WriteLine(savenetworkchecker);
                sw.WriteLine("Memory Unit = " + memoryunit.SelectedItem);
                sw.WriteLine("Battery Voltage Unit = " + bvoltageunit.SelectedItem);
                sw.WriteLine("Battery Dis/Charge Rate Unit = " + bchargerateunit.SelectedItem);
                sw.WriteLine("Cpu Temperature Unit = " + cputemperature.SelectedItem);
                sw.WriteLine("Network Unit = " + networkunit.SelectedItem);
                sw.WriteLine("Battery Capacity Unit = " + batterycapacityunit.SelectedItem);
                sw.WriteLine("Battery Time Unit = " + batterytimeunit.SelectedItem);
                sw.WriteLine("System Up Time Unit = " + systemuptimeunit.SelectedItem);
                sw.WriteLine("Physical Disk Capacity = " + pyhsicaldiskcapacityunit.SelectedItem);
                sw.WriteLine("Physical Disk Read/Write = " + physicaldiskreadunit.SelectedItem);
                sw.WriteLine("Text Color = " + textcolor.SelectedItem);
                sw.WriteLine("Background Color = " + backgroundcolor.SelectedItem);
                sw.WriteLine("Background Image = " + backgroundimg.SelectedItem);
                
            }
            #endregion//Write Settings

            #region//Start Function
            Gethardwarenames();
            Batterycapacity();
            CPUCounter();
            LogicalDisk();
            #endregion//Start Function

            #region//Battery Health
            if (File.Exists(@"C:\arda.txt") == true)
            {
                batteryhealthtext += 1;
                if (batteryhealthtext == 1)
                {
                    batteryhealth();
                }
            }
            #endregion//Battery Health

            #region//Network Adapter Counter
            if (networkadaptercounter == 1)
            {
                Networkadapter();
                adapter2.Visible = false;
                adapter2data.Visible = false;
            }
            else { Networkadapter();}
            #endregion//Network Adapter Counter

            #region//Thermal Zone Information
            if (cputemp1 != null) { Thermalzoneinformation(); } else { cputemp.Visible = false; }
            #endregion//Thermal Zone Information

            #region//Check Battery 
            if (checkbatterystatus == 1)
            {
                perfCHARGE.CategoryName = "BatteryStatus"; perfCHARGE.CounterName = "ChargeRate";
                perfDCHARGE.CategoryName = "BatteryStatus"; perfDCHARGE.CounterName = "DischargeRate";
                perfRCAPA.CategoryName = "BatteryStatus"; perfRCAPA.CounterName = "RemainingCapacity";
                perfVOLT.CategoryName = "BatteryStatus"; perfVOLT.CounterName = "Voltage";
                Batterystatus();
            }
            else if (checkbatterystatus == 2)
            {
                perfCHARGE.CategoryName = "Battery Status"; perfCHARGE.CounterName = "Charge Rate";
                perfDCHARGE.CategoryName = "Battery Status"; perfDCHARGE.CounterName = "Discharge Rate";
                perfRCAPA.CategoryName = "Battery Status"; perfRCAPA.CounterName = "Remaining Capacity";
                perfVOLT.CategoryName = "Battery Status"; perfVOLT.CounterName = "Voltage";
                Batterystatus();
            }
            else { groupBox2.Visible = false; groupBox6.Visible = false; groupBox9.Visible = false; groupBox11.Visible = false; this.Width = 641 ; groupBox4.Width = 343; }
            #endregion//Check Battery 

            #region//CPU-RAM
            cputotal.Text = "CPU Total: " + Math.Round(perfCPUCounter.NextValue(), 0).ToString() + " %";
            label2.Text = "Available Memory: " + Math.Round(Datachanger(perfMemCounter.NextValue(), "Mb", memoryunit.SelectedItem.ToString(), "megabyte"), 3) + " " + Datanamechanger(memoryunit.SelectedItem.ToString());
            label3.Text = "Memory In Use: " + Math.Round(Datachanger((ramSizeInfo - perfMemCounter.NextValue()), "Mb", memoryunit.SelectedItem.ToString(), "megabyte"), 3) + " " + Datanamechanger(memoryunit.SelectedItem.ToString());
            #endregion//CPU-RAM

            #region//SYSTEM UP TİME
            int syscounter1 = (int)perfSysCounter.NextValue();
            label4.Text = "System up time: " + Timechanger(syscounter1, systemuptimeunit.SelectedItem.ToString()) ;
            #endregion//SYSTEM UP TİME
        }

        public void Batterystatus()
        {
            perfCHARGE.InstanceName = batteryname;
            perfDCHARGE.InstanceName = batteryname;
            perfRCAPA.InstanceName = batteryname;
            perfVOLT.InstanceName = batteryname;
            if (remaincapa != 0){remaincapa = (int)perfRCAPA.NextSample().RawValue;}
            else{remaincapa = (int)perfRCAPA.NextSample().RawValue;}
            
            if (perfCHARGE.NextValue() != 0)
            {
                if (batterycapacity1 != 100)
                onetimesaver = 0;

                float charge1 = (float)perfCHARGE.NextValue() / 1000;
                charge.Text = "Charge Rate: " + Datachanger(charge1, "W", bchargerateunit.SelectedItem.ToString(), "watt").ToString("0.00") + " " + Datanamechanger(bchargerateunit.SelectedItem.ToString());
                counterfulltime += 1;
                chargewatt += charge1;
                chargewatt = chargewatt / 2;
                counterfulltimemin = (counterfulltime - (counterfulltime % 60)) / 60;
                if (batterycapa == 0)
                {
                    batterycapa = batterycapacity1;
                }
                if (counterfulltime >= 60)
                {
                    bfulltime.Text = batterycapa.ToString() + " % to " +  rcapacity.Text + " = " + counterfulltimemin + " min " + counterfulltime % 60 + " sec";
                }
                else
                {
                    bfulltime.Text = batterycapa.ToString() + " % to " +  rcapacity.Text + " = " + counterfulltime + " sec";
                }
            }
            else if (perfDCHARGE.NextValue() != 0)
            {
                onetimesaver = 1;
                charge.Text = "Discharge Rate: " + Datachanger(perfDCHARGE.NextValue(), "W", bchargerateunit.SelectedItem.ToString(), "miliwatt").ToString("0.00") + " " + Datanamechanger(bchargerateunit.SelectedItem.ToString());

                if (counterfulltime != 0)
                {
                    if (counterfulltime <= 60)
                    {
                        bfulltime.Text = batterycapa.ToString() + "% to " + batterycapacity1 + "% = " + counterfulltime + " sec";
                    }
                    else
                    {
                        bfulltime.Text = batterycapa.ToString() + "% to " + batterycapacity1 + "% = " + counterfulltimemin + " min " + counterfulltime % 60 + " sec";
                    }
                    svbatterych = bfulltime.Text;
                    batterycapa = 0;
                    counterfulltime = 0;
                }
            }

            else
            {
                if (batterycapacity1 >= 100) { charge.Text = "Battery Fully Charged"; }
                else { charge.Text = "Conservation Mode"; }
            }
            //Battery Capacity
            if (perfDCHARGE.NextValue() != 0)
            {
                //rcapa.Text = SystemInformation.PowerStatus.PowerLineStatus.ToString();
                rcapa.Text = "Reamining Capacity: " + Datachanger(remaincapa, "Wh", batterycapacityunit.SelectedItem.ToString(), "miliwatthours") + " " + Datanamechanger(batterycapacityunit.SelectedItem.ToString());
                rcapacity.Text = batterycapacity1.ToString() + " %";
                groupBox11.Text = "Remaining Capacity";
                progressBar1.Value = batterycapacity1;
            }
            else
            {
                rcapa.Text = "Filled Capacity: " + Datachanger(remaincapa, "Wh", batterycapacityunit.SelectedItem.ToString(), "miliwatthours") + " " + Datanamechanger(batterycapacityunit.SelectedItem.ToString());
                rcapacity.Text = batterycapacity1.ToString() + " %";
                groupBox11.Text = "Filled Capacity";
                progressBar1.Value = batterycapacity1;
            }
            if (perfDCHARGE.NextValue() != 0 || batterycapacity1 == 100)
            {
                if (savebatterytimechecker == "Save Battery Charging Time = 0")
                {
                    if (onetimesaver == 0)
                    {
                        Savebatterydata();
                        onetimesaver = 1;
                    }
                }
            }
            //Battery Voltage
            volt.Text = "Voltage: " + Datachanger(perfVOLT.NextSample().RawValue, "V", bvoltageunit.SelectedItem.ToString(), "Milivolt").ToString("0.00") + " " +Datanamechanger(bvoltageunit.SelectedItem.ToString());

            //Battery Remaining Time
            if (perfDCHARGE.NextValue() != 0)
            {
                int hours = (int)(remaincapa / perfDCHARGE.NextValue() * 60 * 60);
                batterytime.Text = "Remaining Time: " + Timechanger(hours, batterytimeunit.SelectedItem.ToString());
            }
            else if (perfCHARGE.NextValue() == 0)
            {
                if (batterycapacity1 >= 100) { batterytime.Text = "Battery Fully Charged"; }
                else { batterytime.Text = "Conservation Mode"; }
            }
            if (perfDCHARGE.NextValue() != 0)
            {
                int abtime1 = (int)(remaincapa / (averagebatteryusage * 1000) * 60 * 60);
                abtime.Text = "Remaining Time: " + Timechanger(abtime1, batterytimeunit.SelectedItem.ToString());
            }
            else if (perfCHARGE.NextValue() == 0)
            {
                if (batterycapacity1 >= 100) { abtime.Text = "Battery Fully Charged"; }
                else { abtime.Text = "Conservation Mode"; }
            }
            else { abtime.Text = "Battery is charging"; }
            //Battery Full Charge Time
            if (perfCHARGE.NextValue() != 0)
            {
                bcapa = (int)((remaincapa / batterycapacity1) * 100);
                int chargetime1 = (int)(((bcapa - remaincapa) / perfCHARGE.NextValue()) * 60 * 60);
                if (batterycapacity1 >= 100) { batterytime.Text = "Battery Fully Charged"; }
                else
                {
                    batterytime.Text = "Full Charge Time: " + Timechanger(chargetime1, batterytimeunit.SelectedItem.ToString());
                }
            }
            if (perfDCHARGE.NextValue().ToString() != "0")
            {   
                counter += 1;
                showcounter++;
                if (counter == 360)
                {
                    Bcountercomp();
                }
                totaltime.Text = "Total time: " + Timechanger(showcounter, batterytimeunit.SelectedItem.ToString());
                
                avedc += (perfDCHARGE.NextValue() / 1000);
                avebattusa.Text = "Average Battery Usage: " + Datachanger(avedc / counter, "W", bchargerateunit.SelectedItem.ToString(), "watt").ToString("0.00") + " " + Datanamechanger(bchargerateunit.SelectedItem.ToString());
            }
            else
            {   
                if (batterycapacity1 != 100)
                {
                    if (perfCHARGE.NextValue() == 0 && perfDCHARGE.NextValue() == 0)
                    {
                    totaltime.Text = "Conservation Mode";
                    avebattusa.Text = "Conservation Mode";
                    }
                    else
                    {
                    totaltime.Text = "Battery is charging";
                    avebattusa.Text = "Battery is charging";
                    }
                }
                else
                {
                    totaltime.Text = "Battery Fully Charged";
                    avebattusa.Text = "Battery Fully Charged";
                }
            }
                
            
            averagebatteryusage = avedc / counter;
        }

        public void GetRamSize()
        {
            ManagementObjectSearcher ramSearcher = new ManagementObjectSearcher("Select * From Win32_ComputerSystem");
            foreach (ManagementObject mObject in ramSearcher.Get())
            {
                double Ram_Bytes = (Convert.ToDouble(mObject["TotalPhysicalMemory"]));
                double ramgb = Ram_Bytes / 1048576;
                ramSizeInfo = (int)ramgb;
            }
        }

        public void Savebatterydata()
        {
            bdatapath = path1 + "/Battery Charge Time Data.txt";
            using (StreamWriter sw = File.AppendText(bdatapath)) {
                if (onetimesaver == 0)
                {
                    if (chargewatt != 0)
                    {
                        sw.WriteLine("");
                        sw.WriteLine("[" + datetime + "]");
                        sw.WriteLine(bfulltime.Text);
                        sw.WriteLine("Average Watt: " + chargewatt.ToString("0.00") + "W/s");
                    }
                }
            }
            onetimesaver = 1;
            chargewatt = 0;
        }

        public void SaveNetworkstat()
        {
            bdatapath = path1 + "/Average Network Speed.txt";
            using (StreamWriter sw = File.AppendText(bdatapath))
            {
                sw.WriteLine("");
                sw.WriteLine("[" + datetime + "]");
                sw.WriteLine("Average Network Download Speed: " + ndownload1data.ToString("0.00") + "Kb/s");
                sw.WriteLine("Average Network Upload Speed: " + nupload1data.ToString("0.00") + "Kb/s");
                sw.WriteLine("Total Downloaded Data: " + (ndownload1 / 1024.0).ToString("0.00" + "Mb"));
                sw.WriteLine("Total Uploaded Data: " + (nupload1 / 1024.0).ToString("0.00") + "Mb");
            }
        }

        public void Gethardwarenames()
        {

            var categories = PerformanceCounterCategory.GetCategories();
            foreach (var cat in categories)
            {
                if (cat.CategoryName.ToString() == "BatteryStatus") { checkbatterystatus = 1; }
                else if (cat.CategoryName.ToString() == "Battery Status") { checkbatterystatus = 2; }
            }

            //Processor Information
            PerformanceCounterCategory cpu1 = new PerformanceCounterCategory("Processor Information");
            String[] instancenamecpu = cpu1.GetInstanceNames();
            corecounter = 0;
            foreach (string name in instancenamecpu)
            {
                if (name != "_Total")
                {
                    if (name != "0,_Total")
                    {
                        corecounter += 1;
                    }
                }
            }
            //Processor Information
            //BatteryStatus
            if (checkbatterystatus == 1)
            {
                PerformanceCounterCategory battery1 = new PerformanceCounterCategory("BatteryStatus");
                String[] instancenamebattery = battery1.GetInstanceNames();

                foreach (string name in instancenamebattery)
                {
                    if (batteryname != name) { batteryname = name; }
                }
            }
            else if (checkbatterystatus == 2)
            {
                PerformanceCounterCategory battery2 = new PerformanceCounterCategory("Battery Status");
                String[] instancenamebattery2 = battery2.GetInstanceNames();

                foreach (string name in instancenamebattery2)
                {
                    if (batteryname != name) { batteryname = name; }
                }
            }
            //BatteryStatus
            //LogicalDisk
            PerformanceCounterCategory LogicalDisk1 = new PerformanceCounterCategory("LogicalDisk");
            String[] instancenamedisk = LogicalDisk1.GetInstanceNames();
            diskcounter = 0;
            foreach (string name in instancenamedisk)
            {
                if (name != "_Total")
                {
                    if (name.Length < 3)
                    {
                        if (name != null) { diskcounter += 1; }
                        if (diskcounter == 1) { disk1 = name; }
                        if (diskcounter == 2) { disk2 = name; }
                        else if (diskcounter < 2) { disk2 = null; }
                        if (diskcounter == 3) { disk3 = name; }
                        else if (diskcounter < 3) { disk3 = null; }
                        if (diskcounter == 4) { disk4 = name; }
                        else if (diskcounter < 4) { disk4 = null; }
                    }
                }
            }
            if (diskcounter == 1) { disk2 = null; disk3 = null; disk4 = null; }
            else if (diskcounter == 2){ disk3 = null; disk4 = null; }
            else if (diskcounter == 3) { disk4 = null; }
            //LogicalDisk

            //Network Interface
            PerformanceCounterCategory net1 = new PerformanceCounterCategory("Network Interface");
            String[] instancenamenet = net1.GetInstanceNames();
            networkadaptercounter = 0;
            foreach (string name in instancenamenet)
            {
                networkadaptercounter += 1;
                if (networkadaptercounter == 1) { adapter1name = name; }
                if (networkadaptercounter == 2) { adapter2name = name; }
            }
            //Network Interface

            //Thermal Zone Information
            PerformanceCounterCategory thermal1 = new PerformanceCounterCategory("Thermal Zone Information");
            String[] instancenamethermal = thermal1.GetInstanceNames();

            foreach (string name in instancenamethermal)
            {
                if (cputemp1 != name) { cputemp1 = name; }
            }
            //Thermal Zone Information
        }

        public void Thermalzoneinformation()
        {
            PerformanceCounter perfcputemp = new PerformanceCounter("Thermal Zone Information", "Temperature", cputemp1);
            cputemp.Text = "CPU Temperature: " + Datachanger(perfcputemp.NextValue(), "Temp", cputemperature.SelectedItem.ToString(), "kelvin") + " " +Datanamechanger(cputemperature.SelectedItem.ToString());
        }
        public void Cpufrequency()
        {
            PerformanceCounter perfcpufreq = new PerformanceCounter("Processor Information", "Processor Frequency", "_Total");
            float cpufreq = perfcpufreq.NextValue() / 100;
            PerformanceCounter perfcpufreq2 = new PerformanceCounter("Processor Information", "% Processor performance", "_Total");
            label1.Text = "CPU Frequency: " + ((cpufreq * (perfcpufreq2.NextValue() + perfcpufreq2.NextValue())) / 1000).ToString("0.000") + " GHz";
        }

        public void LogicalDisk()
        {
            if (diskcounter >= 1) { ldisk1.Visible = true; disk1gb.Visible = true; } else { ldisk1.Visible = false; disk1gb.Visible = false; }
            if (diskcounter >= 2) { ldisk2.Visible = true; disk2gb.Visible = true; } else { ldisk2.Visible = false; disk2gb.Visible = false; }
            if (diskcounter >= 3) { ldisk3.Visible = true; disk3gb.Visible = true; } else { ldisk3.Visible = false; disk3gb.Visible = false; }
            if (diskcounter >= 4) { ldisk4.Visible = true; disk4gb.Visible = true; } else { ldisk4.Visible = false; disk4gb.Visible = false; }

            Rdisk1.InstanceName = disk1; Wdisk1.InstanceName = disk1; FPdisk1.InstanceName = disk1; Fdisk1.InstanceName = disk1;
            Rdisk2.InstanceName = disk2; Wdisk2.InstanceName = disk2; FPdisk2.InstanceName = disk2; Fdisk2.InstanceName = disk2;
            Rdisk3.InstanceName = disk3; Wdisk3.InstanceName = disk3; FPdisk3.InstanceName = disk3; Fdisk3.InstanceName = disk3;
            Rdisk4.InstanceName = disk4; Wdisk4.InstanceName = disk4; FPdisk4.InstanceName = disk4; Fdisk4.InstanceName = disk4;

            if (diskcounter >= 1) { Rdisk1int = Convert.ToInt64(Rdisk1.NextValue()); Wdisk1int = Convert.ToInt64(Wdisk1.NextValue()); }
            if (diskcounter >= 2) { Rdisk2int = Convert.ToInt64(Rdisk2.NextValue()); Wdisk2int = Convert.ToInt64(Wdisk2.NextValue()); }
            if (diskcounter >= 3) { Rdisk3int = Convert.ToInt64(Rdisk3.NextValue()); Wdisk3int = Convert.ToInt64(Wdisk3.NextValue()); }
            if (diskcounter >= 4) { Rdisk4int = Convert.ToInt64(Rdisk4.NextValue()); Wdisk4int = Convert.ToInt64(Wdisk4.NextValue()); }

            if (diskcounter >= 1) { disk1gb.Text = disk1[0] + " Total: " + Datachanger(Fdisk1.NextValue() / FPdisk1.NextValue() * 100, "Mb", pyhsicaldiskcapacityunit.SelectedItem.ToString(), "megabyte").ToString("0.0") + " " + Datanamechanger(pyhsicaldiskcapacityunit.SelectedItem.ToString()) + " Free: " + Datachanger(Fdisk1.NextValue(), "Mb", pyhsicaldiskcapacityunit.SelectedItem.ToString(), "megabyte").ToString("0.0") + " " + Datanamechanger(pyhsicaldiskcapacityunit.SelectedItem.ToString()); }
            if (diskcounter >= 2) { disk2gb.Text = disk2[0] + " Total: " + Datachanger(Fdisk2.NextValue() / FPdisk2.NextValue() * 100, "Mb", pyhsicaldiskcapacityunit.SelectedItem.ToString(), "megabyte").ToString("0.0") + " " + Datanamechanger(pyhsicaldiskcapacityunit.SelectedItem.ToString()) + " Free: " + Datachanger(Fdisk2.NextValue(), "Mb", pyhsicaldiskcapacityunit.SelectedItem.ToString(), "megabyte").ToString("0.0") + " " + Datanamechanger(pyhsicaldiskcapacityunit.SelectedItem.ToString()); }
            if (diskcounter >= 3) { disk3gb.Text = disk3[0] + " Total: " + Datachanger(Fdisk3.NextValue() / FPdisk3.NextValue() * 100, "Mb", pyhsicaldiskcapacityunit.SelectedItem.ToString(), "megabyte").ToString("0.0") + " " + Datanamechanger(pyhsicaldiskcapacityunit.SelectedItem.ToString()) + " Free: " + Datachanger(Fdisk3.NextValue(), "Mb", pyhsicaldiskcapacityunit.SelectedItem.ToString(), "megabyte").ToString("0.0") + " " + Datanamechanger(pyhsicaldiskcapacityunit.SelectedItem.ToString()); }
            if (diskcounter >= 4) { disk4gb.Text = disk4[0] + " Total: " + Datachanger(Fdisk4.NextValue() / FPdisk4.NextValue() * 100, "Mb", pyhsicaldiskcapacityunit.SelectedItem.ToString(), "megabyte").ToString("0.0") + " " + Datanamechanger(pyhsicaldiskcapacityunit.SelectedItem.ToString()) + " Free: " + Datachanger(Fdisk4.NextValue(), "Mb", pyhsicaldiskcapacityunit.SelectedItem.ToString(), "megabyte").ToString("0.0") + " " + Datanamechanger(pyhsicaldiskcapacityunit.SelectedItem.ToString()); }

            if (diskcounter >= 1) { Rdisk1data = Datachanger(Rdisk1int, "Mb", physicaldiskreadunit.SelectedItem.ToString(), "byte").ToString("0.0") + " " + Datanamechanger(physicaldiskreadunit.SelectedItem.ToString()); }
            if (diskcounter >= 2) { Rdisk2data = Datachanger(Rdisk2int, "Mb", physicaldiskreadunit.SelectedItem.ToString(), "byte").ToString("0.0") + " " + Datanamechanger(physicaldiskreadunit.SelectedItem.ToString()); }
            if (diskcounter >= 3) { Rdisk3data = Datachanger(Rdisk3int, "Mb", physicaldiskreadunit.SelectedItem.ToString(), "byte").ToString("0.0") + " " + Datanamechanger(physicaldiskreadunit.SelectedItem.ToString()); }
            if (diskcounter >= 4) { Rdisk4data = Datachanger(Rdisk4int, "Mb", physicaldiskreadunit.SelectedItem.ToString(), "byte").ToString("0.0") + " " + Datanamechanger(physicaldiskreadunit.SelectedItem.ToString()); }

            if (diskcounter >= 1) { Wdisk1data = Datachanger(Wdisk1int, "Mb", physicaldiskreadunit.SelectedItem.ToString(), "byte").ToString("0.0") + " " + Datanamechanger(physicaldiskreadunit.SelectedItem.ToString()); }
            if (diskcounter >= 2) { Wdisk2data = Datachanger(Wdisk2int, "Mb", physicaldiskreadunit.SelectedItem.ToString(), "byte").ToString("0.0") + " " + Datanamechanger(physicaldiskreadunit.SelectedItem.ToString()); }
            if (diskcounter >= 3) { Wdisk3data = Datachanger(Wdisk3int, "Mb", physicaldiskreadunit.SelectedItem.ToString(), "byte").ToString("0.0") + " " + Datanamechanger(physicaldiskreadunit.SelectedItem.ToString()); }
            if (diskcounter >= 4) { Wdisk4data = Datachanger(Wdisk4int, "Mb", physicaldiskreadunit.SelectedItem.ToString(), "byte").ToString("0.0") + " " + Datanamechanger(physicaldiskreadunit.SelectedItem.ToString()); }


            if (diskcounter >= 1) { ldisk1.Text = disk1[0].ToString() + " Read: " + Rdisk1data + "    " + " Write: " + Wdisk1data; }
            if (diskcounter >= 2) { ldisk2.Text = disk2[0].ToString() + " Read: " + Rdisk2data + "    " + " Write: " + Wdisk2data; }
            if (diskcounter >= 3) { ldisk3.Text = disk3[0].ToString() + " Read: " + Rdisk3data + "    " + " Write: " + Wdisk3data; }
            if (diskcounter >= 4) { ldisk4.Text = disk4[0].ToString() + " Read: " + Rdisk4data + "    " + " Write: " + Wdisk4data; }
        }

        public void CPUCounter()
        {
            if (corecounter >= 4)
            {
                lcore0.Text = "Core 0: " + Math.Round(core0.NextValue(), 0).ToString() + " %";
                lcore1.Text = "Core 1: " + Math.Round(core1.NextValue(), 0).ToString() + " %";
                lcore2.Text = "Core 2: " + Math.Round(core2.NextValue(), 0).ToString() + " %";
                lcore3.Text = "Core 3: " + Math.Round(core3.NextValue(), 0).ToString() + " %";
            }
            if (corecounter >= 6)
            {
                lcore4.Text = "Core 4: " + Math.Round(core4.NextValue(), 0).ToString() + " %";
                lcore5.Text = "Core 5: " + Math.Round(core5.NextValue(), 0).ToString() + " %";
                lcore4.Visible = true; lcore5.Visible = true;
            }
            else { lcore4.Visible = false; lcore5.Visible = false; }
            if (corecounter >= 8)
            {
                lcore6.Text = "Core 6: " + Math.Round(core6.NextValue(), 0).ToString() + " %";
                lcore7.Text = "Core 7: " + Math.Round(core7.NextValue(), 0).ToString() + " %";
                lcore6.Visible = true; lcore7.Visible = true;
            }
            else { lcore6.Visible = false; lcore7.Visible = false; }
            if (corecounter >= 10)
            {
                lcore8.Text = "Core 8: " + Math.Round(core8.NextValue(), 0).ToString() + " %";
                lcore9.Text = "Core 9: " + Math.Round(core9.NextValue(), 0).ToString() + " %";
                lcore8.Visible = true; lcore9.Visible = true;
            }
            else { lcore8.Visible = false; lcore9.Visible = false; }
            if (corecounter >= 12)
            {
                lcore10.Text = "Core 10: " + Math.Round(core10.NextValue(), 0).ToString() + " %";
                lcore11.Text = "Core 11: " + Math.Round(core11.NextValue(), 0).ToString() + " %";
                lcore10.Visible = true; lcore11.Visible = true;
            }
            else { lcore10.Visible = false; lcore11.Visible = false; }
            if (corecounter >= 16)
            {
                lcore12.Text = "Core 12: " + Math.Round(core12.NextValue(), 0).ToString() + " %";
                lcore13.Text = "Core 13: " + Math.Round(core13.NextValue(), 0).ToString() + " %";
                lcore14.Text = "Core 14: " + Math.Round(core14.NextValue(), 0).ToString() + " %";
                lcore15.Text = "Core 15: " + Math.Round(core15.NextValue(), 0).ToString() + " %";
                lcore12.Visible = true; lcore13.Visible = true; lcore14.Visible = true; lcore15.Visible = true;
            }
            else { lcore12.Visible = false; lcore13.Visible = false; lcore14.Visible = false; lcore15.Visible = false; }
        }
        
        public void Networkadapter()
        {
            Radapter1.InstanceName = adapter1name; Sadapter1.InstanceName = adapter1name;
            Radapter2.InstanceName = adapter2name; Sadapter2.InstanceName = adapter2name;

            if (networkadaptercounter >= 1)
            {
                adapter1.Text = adapter1name;

                Radapter1value = (int)Radapter1.NextSample().RawValue - Radapter1oldvalue;
                Radapter1oldvalue = (int)Radapter1.NextSample().RawValue;
                Sadapter1value = (int)Sadapter1.NextSample().RawValue - Sadapter1oldvalue;
                Sadapter1oldvalue = (int)Sadapter1.NextSample().RawValue;

                Radapter1data = Datachanger(Radapter1value, "Mb", networkunit.SelectedItem.ToString(), "byte").ToString("0.0") + " " + Datanamechanger(networkunit.SelectedItem.ToString());
                Sadapter1data = Datachanger(Sadapter1value, "Mb", networkunit.SelectedItem.ToString(), "byte").ToString("0.0") + " " + Datanamechanger(networkunit.SelectedItem.ToString());
                if (networkhover1 == 0)
                {
                    adapter1data.Text = "Received: " + Radapter1data + " -- Sent: " + Sadapter1data;
                }
                else
                {
                    adapter1data.Text = "Total Down Data: " + (ndownload1 / 1024).ToString("0") + "Mb -- Up Data: " + (nupload1 / 1024).ToString("0") + " Mb";
                }
                
            }
            if (networkadaptercounter >= 2)
            {
                adapter2.Text = adapter2name;

                Radapter2value = (int)Radapter2.NextSample().RawValue - Radapter2oldvalue;
                Radapter2oldvalue = (int)Radapter2.NextSample().RawValue;
                Sadapter2value = (int)Sadapter2.NextSample().RawValue - Sadapter2oldvalue;
                Sadapter2oldvalue = (int)Sadapter2.NextSample().RawValue;

                Radapter2data = Datachanger(Radapter2value, "Mb", networkunit.SelectedItem.ToString(), "byte").ToString("0.0") + " " + Datanamechanger(networkunit.SelectedItem.ToString());
                Sadapter2data = Datachanger(Sadapter2value, "Mb", networkunit.SelectedItem.ToString(), "byte").ToString("0.0") + " " + Datanamechanger(networkunit.SelectedItem.ToString());

                if (networkhover2 == 0)
                {
                    adapter2data.Text = "Received: " + Radapter2data + " -- Sent: " + Sadapter2data;
                }
                else
                {
                    adapter2data.Text = "Total Down Data: " + (ndownload2 / 1024).ToString("0") + "Mb -- Up Data: " + (nupload2 / 1024).ToString("0") + " Mb";
                }
            }
            if(savenetworkchecker == "Save Average Network Usage And Speed Data = 0")
            {
                Averagenetwork();
                avfunc = 1;
            }
        }

        private void Averagenetwork()
        {
            if (avfunc == 1)
            {
                avenetworktime += 1;
                if (networkadaptercounter >= 1)
                {
                    ndownload1 += (float)(Radapter1value / 1024.0);
                    nupload1 += (float)(Sadapter1value / 1024.0);
                    ndownload1data = (ndownload1 / avenetworktime);
                    nupload1data = (nupload1 / avenetworktime);
                }
                if (networkadaptercounter >= 2)
                {
                    ndownload2 += (float)(Radapter2value / 1048576.0);
                    nupload2 += (float)(Sadapter2value / 1048576.0);
                    ndownload2data = (ndownload2 / avenetworktime);
                    nupload2data = (nupload2 / avenetworktime);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            colorindex = textcolor.SelectedIndex;
            System.Drawing.Color color = System.Drawing.Color.AliceBlue;
            if (colorindex == 0) {color = System.Drawing.Color.Red;}
            else if (colorindex == 1) { color = System.Drawing.Color.Yellow; }
            else if (colorindex == 2) { color = System.Drawing.Color.Orange; }
            else if (colorindex == 3) { color = System.Drawing.Color.Green; }
            else if (colorindex == 4) { color = System.Drawing.Color.Black; }
            else if (colorindex == 5) { color = System.Drawing.Color.White; }
            else if (colorindex == 6) { color = System.Drawing.Color.Blue; }
            groupBox1.ForeColor = color;
            groupBox2.ForeColor = color;
            groupBox3.ForeColor = color;
            groupBox4.ForeColor = color;
            groupBox5.ForeColor = color;
            groupBox6.ForeColor = color;
            groupBox7.ForeColor = color;
            groupBox8.ForeColor = color;
            groupBox9.ForeColor = color;
            groupBox11.ForeColor = color;
            groupBox12.ForeColor = color;
            bvoltagebox.ForeColor = color;
            networkbox.ForeColor = color;
            backgroundimgbox.ForeColor = color;
            backgroundcolorbox.ForeColor = color;
            textcolorbox.ForeColor = color;
            systemuptimebox.ForeColor = color;
            memorybox.ForeColor = color;
            batterycapacitybox.ForeColor = color;
            bchargeratebox.ForeColor = color;
            batterytimebox.ForeColor = color;
            cputemperaturebox.ForeColor = color;
            button2.ForeColor = color;
            if (color == System.Drawing.Color.Black) { button1.BackColor = System.Drawing.Color.White; settingsbtn.BackColor = System.Drawing.Color.White; }
            else { button1.BackColor = System.Drawing.Color.Black; settingsbtn.BackColor = System.Drawing.Color.Black; }
            button1.ForeColor = color;
            settingsbtn.ForeColor = color;
            progressBar1.ForeColor = color;
            if (color == System.Drawing.Color.Black) { progressBar1.BackColor = System.Drawing.Color.White; }
            else { progressBar1.BackColor = System.Drawing.Color.Black; }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.BackgroundImage = null;
            colorindex2 = backgroundcolor.SelectedIndex;
            System.Drawing.Color color = System.Drawing.Color.AliceBlue;
            if (colorindex2 == 0) { color = System.Drawing.Color.Red; }
            else if (colorindex2 == 1) { color = System.Drawing.Color.Yellow; }
            else if (colorindex2 == 2) { color = System.Drawing.Color.Orange; }
            else if (colorindex2 == 3) { color = System.Drawing.Color.Green; }
            else if (colorindex2 == 4) { color = System.Drawing.Color.Black; }
            else if (colorindex2 == 5) { color = System.Drawing.Color.White; }
            else if (colorindex2 == 6) { color = System.Drawing.Color.Blue; }
            this.BackColor = color;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            bgch = backgroundimg.SelectedIndex;
            groupBox1.BackColor = System.Drawing.Color.Transparent;
            groupBox2.BackColor = System.Drawing.Color.Transparent;
            groupBox3.BackColor = System.Drawing.Color.Transparent;
            groupBox4.BackColor = System.Drawing.Color.Transparent;
            groupBox5.BackColor = System.Drawing.Color.Transparent;
            groupBox6.BackColor = System.Drawing.Color.Transparent;
            groupBox7.BackColor = System.Drawing.Color.Transparent;
            groupBox8.BackColor = System.Drawing.Color.Transparent;
            groupBox9.BackColor = System.Drawing.Color.Transparent;
            groupBox11.BackColor = System.Drawing.Color.Transparent;
            groupBox12.BackColor = System.Drawing.Color.Transparent;
            bvoltagebox.BackColor = System.Drawing.Color.Transparent;
            networkbox.BackColor = System.Drawing.Color.Transparent;
            backgroundimgbox.BackColor = System.Drawing.Color.Transparent;
            backgroundcolorbox.BackColor = System.Drawing.Color.Transparent;
            textcolorbox.BackColor = System.Drawing.Color.Transparent;
            systemuptimebox.BackColor = System.Drawing.Color.Transparent;
            memorybox.BackColor = System.Drawing.Color.Transparent;
            batterycapacitybox.BackColor = System.Drawing.Color.Transparent;
            bchargeratebox.BackColor = System.Drawing.Color.Transparent;
            batterytimebox.BackColor = System.Drawing.Color.Transparent;
            cputemperaturebox.BackColor = System.Drawing.Color.Transparent;
            if (bgch == 0)
            {

            }
            else if (bgch == 1)
            {
                this.BackgroundImage = Windows_Hardware_Monitor.Properties.Resources.Msı_Red;
                textcolor.SelectedIndex = 5; 
            }
            else if (bgch == 2)
            {
                this.BackgroundImage = Windows_Hardware_Monitor.Properties.Resources.Msi_Cyan;
                textcolor.SelectedIndex = 5;
            }
            else if (bgch == 3)
            {
                this.BackgroundImage = Windows_Hardware_Monitor.Properties.Resources.Msi_Green;
                textcolor.SelectedIndex = 5;
            }
            else if (bgch == 4)
            {
                this.BackgroundImage = Windows_Hardware_Monitor.Properties.Resources.Msi_Grey;
                textcolor.SelectedIndex = 5;
            }
            else if (bgch == 5)
            {
                this.BackgroundImage = Windows_Hardware_Monitor.Properties.Resources.Msi_Orange;
                textcolor.SelectedIndex = 5;
            }
            else if (bgch == 6)
            {
                this.BackgroundImage = Windows_Hardware_Monitor.Properties.Resources.Background_3001;
                textcolor.SelectedIndex = 4;
            }
            else if (bgch == 7)
            {
                try
                {
                    File.Copy(Path.Combine(Application.StartupPath, "customimage1.jpg"), Path.Combine(Application.StartupPath, "customimage.jpg"), true);
                }
                catch { }
                this.BackgroundImage = Image.FromFile(Application.StartupPath + "\\customimage.jpg");
            }
            
        }
        
        private void batteryhealth()
        {
            dcline = 0;
            lfcline = 0;
            linecounter = 0;
            foreach (string line in System.IO.File.ReadLines(@"c:\arda.txt"))
            {
                linecounter += 1;
                if (line == "<td><span class=\"detail-name\">Tasarım Kapasitesi</span></td>")
                {
                    dcline = linecounter;
                }
                else if (line == "<td><span class=\"detail-name\">Son Tam Dolum</span></td>")
                {
                    lfcline = linecounter;
                    break;
                }
            }
            
            string[] lines = System.IO.File.ReadAllLines(@"C:\arda.txt");
            if (dcline != 0)
            {
                string dcsvalue = lines[dcline + 1].Substring(4, 5);
                string lfcsvalue = lines[lfcline + 1].Substring(4, 5);
                int health = int.Parse(lfcsvalue) * 100 / int.Parse(dcsvalue);
                label7.Text = "Battery Health: %" + health.ToString();              
            }
        }

        private void notifyIcon1_DoubleClick(object Sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.Activate();
                this.ShowInTaskbar = true;
            }
        }
        
        private void menuItem1_Click(object Sender, EventArgs e)
        {
            checkexit = "Minimize to taskbar with close(X) button = 1";
            notifyIcon1.Dispose();
            Application.Exit();
        }

        private void hardwaremonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveNetworkstat();
            if (onetimesaver == 0)
            {
                Savebatterydata();
            }
            if (checkexit == "Minimize to taskbar with close(X) button = 0")
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }
            else
            {
                notifyIcon1.Dispose();
            }
        }
        
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
            if (mtaskbar.Checked == true)
            {
                checkexit = "Minimize to taskbar with close(X) button = 0";
            }
            else
            {
                checkexit = "Minimize to taskbar with close(X) button = 1";
            }
        }

        private void savetheme_CheckedChanged(object sender, EventArgs e)
        {
            if (savetheme.Checked == true)
            {
                checktheme = "Auto Save Theme = 0";
            }
            else
            {
                checktheme = "Auto Save Theme = 1";
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (savebatterytime.Checked == true)
            {
                savebatterytimechecker = "Save Battery Charging Time = 0";
            }
            else
            {
                savebatterytimechecker = "Save Battery Charging Time = 1";
            }
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            if (savenetwork.Checked == true)
            {
                savenetworkchecker = "Save Average Network Usage And Speed Data = 0";
            }
            else
            {
                savenetworkchecker = "Save Average Network Usage And Speed Data = 1";
            }
        }

        private void hardwaremonitor_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(Application.ExecutablePath));
        }

        private void Bcountercomp()
        {
            avedc = (avedc / counter) * 60;
            counter = 60;
        }

        public static float Datachanger(float data, string type, string outputdatatype, string inputdatatype)
        {
            if (type == "Mb") { 
                if (inputdatatype == "byte"){}
                else if (inputdatatype == "kilobyte") { data = data * (int)Math.Pow(2, 10); }
                else if (inputdatatype == "megabyte") { data = data * (int)Math.Pow(2, 20); }
                else if (inputdatatype == "gigabyte") { data = data * (int)Math.Pow(2, 30); }
                else if (inputdatatype == "terabyte") { data = data * (int)Math.Pow(2, 40); }
            }
            else if (type == "Mbps")
            {
                if (inputdatatype == "kilobit") { }
                else if (inputdatatype == "megabit") { data = data * (int)Math.Pow(10, 3); }
                else if (inputdatatype == "gigabit") { data = data * (int)Math.Pow(10, 6); }
            }
            else if (type == "W")
            {
                if (inputdatatype == "miliwatt") { }
                else if (inputdatatype == "watt") { data = data * (int)Math.Pow(10, 3); }
            }
            else if(type == "V") {
                if (inputdatatype == "milivolt") { }
                else if (inputdatatype == "volt") { data = data * (int)Math.Pow(10, 3); }
            }
            else if (type == "Wh")
            {
                if (inputdatatype == "miliwatthours") { }
                else if (inputdatatype == "watthours") { data = data * (int)Math.Pow(10, 3); }
            }
            else if (type == "Temp")
            {
                if (inputdatatype == "celsius") { }
                else if (inputdatatype == "kelvin") { data = data - 273; }
                else if (inputdatatype == "fahrenheit") { data = (data - 32) * 5 / 9; }
            }
            //-------------------------------------------------------------------------
                if (type == "Mb")
            {
                if (outputdatatype == "Byte") { return data; }
                else if (outputdatatype == "Kilobyte") { return (float)data / (int)Math.Pow(2, 10); }
                else if (outputdatatype == "Megabyte") { return (float)data / (int)Math.Pow(2, 20); }
                else if (outputdatatype == "Gigabyte") { return (float)data / (int)Math.Pow(2, 30); }
                else if (outputdatatype == "Terabyte") { return (float)data / (long)Math.Pow(2, 40); }
                else if (outputdatatype == "Kilobit") { return (float)data / (int)Math.Pow(2, 10) * 8; }
                else if (outputdatatype == "Megabit") { return (float)data / (int)Math.Pow(2, 20) * 8; }
                else if (outputdatatype == "Gigabit") { return (float)data / (int)Math.Pow(2, 30) * 8; }
            }
                else if (type == "Mbps")
            {
                if (outputdatatype == "Kilobit") { return data; }
                else if (outputdatatype == "Megabit") { return (float)data / (int)Math.Pow(10, 3); }
                else if (outputdatatype == "Migabit") { return (float)data / (int)Math.Pow(10, 6); }
                else if (outputdatatype == "Byte") { return (float)data / 8 * (int)Math.Pow(2, 10); }
                else if (outputdatatype == "Kilobyte") { return (float)data / 8; }
                else if (outputdatatype == "Megabyte") { return (float)data / 8 / (int)Math.Pow(2, 10); }
                else if (outputdatatype == "Gigabyte") { return (float)data / 8 / (int)Math.Pow(2, 20); }
                else if (outputdatatype == "Terabyte") { return (float)data / 8 / (int)Math.Pow(2, 30); }
            }

                else if (type == "W")
            {
                if (outputdatatype == "Miliwatt") { return data; }
                else if (outputdatatype == "Watt") { return (float)data / (int)Math.Pow(10, 3); }
            }
                else if (type == "V")
            {
                if (outputdatatype == "Milivolt") { return data; }
                else if (outputdatatype == "Volt") { return (float)data / (int)Math.Pow(10, 3); }
            }
                else if (type == "Wh")
            {
                if (outputdatatype == "Miliwatt hours") { return data; }
                else if (outputdatatype == "Watt hours") { return (float)data / (int)Math.Pow(10, 3); }
            }

                else if (type == "Temp")
            {
                if (outputdatatype == "Celsius") { return data; }
                else if (outputdatatype == "Kelvin") { return data + 273; }
                else if (outputdatatype == "Fahrenheit") { return (data * 9 / 5 ) + 32; }
            }
            return data;
            }
        public static string Timechanger(int time, string outputtype)
        {
            TimeSpan t = TimeSpan.FromSeconds(time);
            if (outputtype == "Second") { return time.ToString() + " sec"; }
            else if (outputtype == "Minutes") { return time / 60 + " min " + time % 60 + " sec"; }
            else if (outputtype == "Hours") { return time / 60 / 60 + " hrs " + (time / 60) % 60 + " min " + (time % 60) + " sec"; }
            else if (outputtype == "Day") { return string.Format("{0:D2} day {1:D2} hrs {2:D2} min {3:D2} sec", t.Days, t.Hours, t.Minutes, t.Seconds); }
            return time.ToString();
        }
        public static string Datanamechanger(string dataname)
        {
                if (dataname == "Byte") { return "B"; }
                else if (dataname == "Kilobyte") { return "Kb"; }
                else if (dataname == "Megabyte") { return "Mb"; }
                else if (dataname == "Gigabyte") { return "Gb"; }
                else if (dataname == "Terabyte") { return "Tb"; }
                else if (dataname == "Kilobit") { return "Kbps"; }
                else if (dataname == "Megabit") { return "Mbps"; }
                else if (dataname == "Gigabit") { return "Gbps"; }
                else if (dataname == "Miliwatt") { return "mW"; }
                else if (dataname == "Watt") { return "W"; }
                else if (dataname == "Milivolt") { return "mV"; }
                else if (dataname == "Volt") { return "V"; }
                else if (dataname == "Miliwatt hours") { return "mWh"; }
                else if (dataname == "Watt hours") { return "Wh"; }
                else if (dataname == "Second") { return "Sec"; }
                else if (dataname == "Minutes") { return "Min"; }
                else if (dataname == "Hours") { return "Hrs"; }
                else if (dataname == "Day") { return "Day"; }
                else if (dataname == "Celsius") { return "°C"; }
                else if (dataname == "Kelvin") { return "K"; }
                else if (dataname == "Fahrenheit") { return "°F"; }
                return dataname;
        }

        private void Settingsbtn_Click(object sender, EventArgs e)
        {
            if (this.Size.Height == 636 )
            {
                this.Height = 400;
            }
            else
            {
                this.Height = 636;
            }
        }

        private void adapter1data_MouseHover(object sender, EventArgs e)
        {
            networkhover1 = 1;
        }

        private void adapter1data_MouseLeave(object sender, EventArgs e)
        {
            networkhover1 = 0;
        }

        private void adapter2data_MouseHover(object sender, EventArgs e)
        {
            networkhover2 = 1;
        }

        private void adapter2data_MouseLeave(object sender, EventArgs e)
        {
            networkhover2 = 0;
        }
        public string filePath;
        public string destinationPath;
        private void button2_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                 filePath = openFileDialog1.FileName;
                string fileName = Path.GetFileName(filePath);
                string newFileName = "customimage1.jpg"; // yeni dosya adı ve uzantısı

                string appPath = Application.StartupPath;

                destinationPath = Path.Combine(appPath, newFileName);
                
                this.BackgroundImage = Image.FromFile(filePath);
                backgroundimg.SelectedIndex = 7;
                File.Copy(filePath, destinationPath, true);


            }
        }
        private void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Suspend:
                    // Bilgisayar uyku moduna geçtiğinde yapılacak işlemler
                    MessageBox.Show("Bilgisayar uyku moduna geçti.");
                    break;
                case PowerModes.Resume:
                    // Bilgisayar uykudan uyandığında yapılacak işlemler
                    MessageBox.Show("Bilgisayar uykudan uyandı.");
                    break;
            }
        }

    }
    }
