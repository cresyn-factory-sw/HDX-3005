
// definitions
#define ENABLE_WIRELESS_TEST
#define AFTER_AWS_PAIR
#define xWEBCAM_TEST
#define SAVE_DUPE

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using USBInterface;

namespace threadTest01
{
    public partial class Form1 : Form
    {
        private TcpAppClient AppClient;
        private TcpClient TcpClient;

        public static int counter = 0;
        public static string command;

#if true
        public const byte USB_HOST_DATA_CMD_START_CONNECTION = 0x00;
        public const byte USB_HOST_DATA_CMD_DISCONNECT = 0x01;
        public const byte USB_HOST_DATA_CMD_GET_REMOTE_NAME = 0x02;
        public const byte USB_HOST_DATA_CMD_GET_REMOTE_BD_ADDRESS = 0x03;
        public const byte USB_HOST_DATA_CMD_GET_REMOTE_VERSION = 0x04;
        public const byte USB_HOST_DATA_CMD_SEND_AIROHA_RACE_CMD = 0x05;

        public const byte USB_DEVICE_DATA_RSP_HS_CONNECTED = 0x00;
        public const byte USB_DEVICE_DATA_RSP_HS_CONNECTING_ATTEMPT = 0x01;
        public const byte USB_DEVICE_DATA_RSP_HS_REMOTE_BD_ADDR = 0x02;
        public const byte USB_DEVICE_DATA_RSP_HS_REMOTE_NAME = 0x03;
        public const byte USB_DEVICE_DATA_RSP_HS_VERSION_INFO = 0x04;
        public const byte USB_DEVICE_DATA_RSP_HS_SPP_RX = 0x05;

        public const byte RACE_CHANNEL = 0x05;
        public const byte RACE_TYPE_RESP = 0x5B;
        public const byte RACE_TYPE_NOTI = 0x5D;

        public const int RACE_ID_CMDRELAY_PASS_TO_DST = 0x0D01;
        public const int RACE_ID_READ_FULLKEY = 0x0A00;
        public const int RACE_ID_WRITE_FULLKEY = 0x0A01;
        public const int RACE_ID_READ_BATTERY = 0x0CD6;
        public const int RACE_ID_READ_FW_INFO = 0x1C07;
        public const int RACE_ID_ENABLE_KEY_EVENT = 0x1101;
        public const int RACE_ID_READ_REGION_VP = 0x01C2;

        public static bool needProcessMoreData = false;
        public static bool needProcessDstData = false;

        public static bool requestBattery = false;
        public static bool requestFWversion = false;
        public static bool requestSerialRead = false;
        public static bool requestSerialWrite = false;
        public static bool requestFactoryReset = false;
        public static bool requestDeviceMode = false;
        public static bool requestWriteRegion = false;
        public static bool requestWriteColor = false;
        public static bool requestCheckRegion = false;
        public static bool requestColor = false;
        public static bool requestCheckRegionVP = false;
        public static bool requestWriteDeviceMode = false;
        public static bool Write_DeviceMode = false;

#endif

#if (ENABLE_WIRELESS_TEST)
        public static bool HeadsetConnected = false;
        public static bool CheckModelName = false;
        public static bool CheckBdAddress = false;
        public static bool CheckFwVersion = false;
        public static bool CheckFwRegion = false;
        public static bool CheckFactoryReset = false;
        public static bool CheckAwsInfo = false;
        public static bool WriteSerial = false;
        public static bool CheckSerial = false;
        public static bool CheckHsBatteryLevel = false;
        public static bool CheckDeviceMode = false;
        public static bool WriteRegion = false;
        public static bool CheckRegion = false;
        public static bool WriteColor = false;
        public static bool CheckColor = false;
        public static bool CheckRegionVP = false;
        public static bool tryAgain = false;
        public static bool WriteDeviceMode = false;

        static public byte dutBd01;
        static public byte dutBd02;
        static public byte dutBd03;
        static public byte dutBd04;
        static public byte dutBd05;
        static public byte dutBd06;

        static public byte[] dutModelNameByte = new Byte[30];

        static public string dutSerial;
        static public string dutBdNap;
        static public string dutBdUap;
        static public string dutBdLap;
        static public string dutFullBdAddress;
        static public string dutModelName;
        static public string dutVersionRevision;
        static public string dutFullVersion;
        static public int dutBatteryLevel;
        static public bool dutDeviceMode;
        static public string dutRegion;
        static public string dutColorCode;
        static public string dutRegionVP;


        public static DeviceScanner scanner = null;
        public static USBDevice dev = null;

#endif
        static public string gStartbutton;
        static public string gDutName;
        static public string gSwVersion;
        static public string gManufactureDate;

        static public string gDefaultBdNap;
        static public string gDefaultBdUap;
        static public string gDefaultBdLap;

        static public string gBdNap;
        static public string gBdUap;
        static public string gBdLapStart;
        static public string gBdLapEnd;

        static public string gLineInfoA;
        static public string gLineInfoB;

        static public string gFwRegion;
        static public string gFwColor;
        static public string gFwMode;
        static public int gBatteryLevelLow;
        static public int gBatteryLevelHigh;

        static public string gSerialStart;
        static public string gSerialEnd;
        static public string gSerialEndWrite;

        static public string gSNFirstLetterCheck;
        static public string tempFwRegion;

        static public string gAppName;

        static public string gSoundNg1 = System.IO.Directory.GetCurrentDirectory() + "\\" + "sound\\" + "ng_merge.wav";

        public string pathLogFile;
        public string pathDupeLogFile;

        public UInt32 totalCount = 0;
        public UInt32 passCount = 0;
        public UInt32 failCount = 0;

        public string[] ngDesc = new string[20];
        static public string typeNg = "";
        static public string usbdbg_name = "";

        static public int gCountSequence = 0;
        public string[] gtestSequence = new string[20];
        public string[] gTestValue = new string[20];
        public string[] gNgType = new string[20];

        static public int gLatestDupeRow = 0;

        string pathConfigFile;

        static public string gSerialInput = "";

        static public int failRetryCount = 0;


        public Form1()
        {
            InitializeComponent();

            AppClient = new TcpAppClient();
            AppClient.ConnectionStatusChanged += Client_ConnectionStatusChanged;

            TcpClient = new TcpClient();
            TcpClient.ConnectionStatusChanged += Client_ConnectionStatusChanged;
            TcpClient.DataReceived += TcpClient_DataReceived;
        }

        private void TcpClient_DataReceived(object sender, TcpDataReceivedEventArgs e)
        {
            AppendOutput1(e.GetString(), e.GetString(), Color.Black);
        }


        private delegate void AppendOutputDelegate(string message, string message2, Color textColor);
        private void AppendOutput1(string message, string message2, Color textColor)
        {
            if (InvokeRequired)
            {
                try
                {
                    BeginInvoke(new AppendOutputDelegate(AppendOutput1), message.Substring(74, 7), message2.Substring(96, 2), textColor);
                }
                catch
                {
                    BeginInvoke(new AppendOutputDelegate(AppendOutput1), message, message2, textColor);

                }
                return;
            }

            while (message.StartsWith("0") || message.StartsWith("1") || message.StartsWith("2") || message.StartsWith("3") || message.StartsWith("4") || message.StartsWith("5") || message.StartsWith("6") || message.StartsWith("7") || message.StartsWith("8") || message.StartsWith("9"))
            {
                textBox2.SelectionStart = textBox2.Text.Length;
                textBox2.ForeColor = textColor;
                textBox2.AppendText(message);
                textBox2.ScrollToCaret();

                string tb_message;
                tb_message = textBox2.Text.Trim().Substring(0, 7);

                textBox4.SelectionStart = textBox4.Text.Length;
                textBox4.ForeColor = textColor;
                textBox4.AppendText(message2);
                textBox4.ScrollToCaret();

                if (tb_message.Length == 7)
                {
                    this.timer.Stop();

                    if ((textBox1.Text == textBox2.Text) && (message2 == "OK"))
                    {
                        toggleSerialInputBtn(false);
                        startTest();
                    }
                    else
                    {
                        openpassworddlg();
                        clearSerial();
                        toggleSerialInputBtn(true);
                    }
                    break;
                }
                else
                {
                    openpassworddlg();
                    clearSerial();
                    toggleSerialInputBtn(true);
                }
            }

        }


        private void Client_ConnectionStatusChanged(object sender, EventArgs e)
        {
            if (TcpClient == null) return;
            if (TcpClient.Connected)
            {
                this.BeginInvoke(new MethodInvoker(ClientConnected));
            }
            else
            {
                this.BeginInvoke(new MethodInvoker(ClientDisconnected));
            }
        }

        private void ClientConnected()
        {
            textBox1.Enabled = false;

            TxtHostName.Enabled = false;
            BtConnect.Text = "DISCONNECT";
        }

        public void ClientDisconnected()
        {
            textBox2.Clear();
            textBox4.Clear();

            textBox1.Enabled = true;
            textBox1.Focus();

            TxtHostName.Enabled = true;
            BtConnect.Text = "CONNECT ▶▶▶";
        }

        private void SendCommand()
        {
            try
            {
                if (TcpClient == AppClient)
                    AppClient.ExecuteCommand(command, 2000);
                else
                {
                    command = "T2\r\n";
                    TcpClient.Write(command);
                }
            }
            catch (Exception ex)
            {
                //
            }

        }

        private void initDataGridView()
        {
            dataGridView1.ColumnCount = 5;
            dataGridView1.ColumnHeadersVisible = true;

            DataGridViewCellStyle columnHeaderStyle = new DataGridViewCellStyle();

            columnHeaderStyle.BackColor = Color.Blue;
            columnHeaderStyle.Font = new Font("Arial", 7, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle = columnHeaderStyle;

            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 300;
            dataGridView1.Columns[2].Width = 200;
            dataGridView1.Columns[3].Width = 300;
            dataGridView1.Columns[4].Width = 100;

            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView1.Columns[0].Name = "No.";
            dataGridView1.Columns[1].Name = "Item";
            dataGridView1.Columns[2].Name = "Config";
            dataGridView1.Columns[3].Name = "Value";
            dataGridView1.Columns[4].Name = "Result";
        }

        private void initDataGridView2()
        {
            dataGridView2.ColumnCount = 5;
            dataGridView2.ColumnHeadersVisible = true;

            DataGridViewCellStyle columnHeaderStyle2 = new DataGridViewCellStyle();

            columnHeaderStyle2.BackColor = Color.Blue;
            columnHeaderStyle2.Font = new Font("Arial", 8, FontStyle.Bold);
            dataGridView2.ColumnHeadersDefaultCellStyle = columnHeaderStyle2;

            dataGridView2.Columns[0].Width = 50;
            dataGridView2.Columns[1].Width = 250;
            dataGridView2.Columns[2].Width = 250;
            dataGridView2.Columns[2].Width = 60;
            dataGridView2.Columns[3].Width = 60;

            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView2.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dataGridView2.Columns[0].Name = "No.";
            dataGridView2.Columns[1].Name = "BD";
            dataGridView2.Columns[2].Name = "SN";
            dataGridView2.Columns[3].Name = "Result";
            dataGridView2.Columns[4].Name = "Dupe";
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            counter++;
            this.timer.Equals(this.timer);
            this.timer.Tick += timer_Tick;

            int flag_load_fail = 0;

            initDataGridView();
            initDataGridView2();

            loadConfig();

            // set mainform name
            this.Text = gDutName + " Wireless BD Check";
            this.ActiveControl = textBox1;

            checkLogFile();
            checkDupeLogFile();

            // update count
            tb_totalCount.Text = totalCount.ToString();
            tb_passCount.Text = passCount.ToString();

            failCount = totalCount - passCount;
            tb_failCount.Text = (totalCount - passCount).ToString();

            // show region
            lbRegion.Text = gFwRegion;

            // show configured color
            lbColor.Text = gFwColor;

            switch (gFwColor)
            {
                case "BLACK":
                    lbColor.BackColor = Color.Gray;
                    break;
                case "BLUE":
                    lbColor.BackColor = Color.Blue;
                    break;
                case "BEIGE":
                    lbColor.BackColor = Color.Beige;
                    break;
                case "WHITE":
                    lbColor.BackColor = Color.White;
                    break;
                case "PINK":
                    lbColor.BackColor = Color.Pink;
                    break;
                case "YELLOW":
                    lbColor.BackColor = Color.Yellow;
                    break;
                default:
                    break;
            }


            if (gStartbutton == "Enable")
            {
                btnStart.Enabled = true;
                this.ActiveControl = btnStart;

                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox4.Enabled = false;
                BtConnect.Enabled = false;

            }
            else if (gStartbutton == "Disable")
            {
                btnStart.Enabled = false;

                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox4.Enabled = true;
                BtConnect.Enabled = true;

                this.ActiveControl = textBox1;

            }
            else
            {
                flag_load_fail = 1;
                MessageBox.Show("NG plz Check config :: START_BUTTON_ENABLE ");
                Close();
            }

            if (flag_load_fail != 1)
            {
                if (!checkConfigSetting())
                {
                    flag_load_fail = 1;
                    MessageBox.Show("Check Color OR Battery OR FW Type setting");
                    Close();
                }
            }

            if (flag_load_fail != 1)
            {
                if (!initUsbHid())
                {
                    flag_load_fail = 1;
                    MessageBox.Show("Cannot load USB Audio Dongle");

                    Close();
                }
            }

        }


        private void clearDisp()
        {
            for (int row = 0; row < Form1.gCountSequence; row++)
            {
                dataGridView1.Rows[row].Cells[3].Value = "-";
                dataGridView1.Rows[row].Cells[4].Value = "-";
                dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.Empty;
            }
            dataGridView2.Rows[gLatestDupeRow].DefaultCellStyle.BackColor = Color.Empty;

            lbResult.Text = "--";
            lbResult.ForeColor = Color.Empty;
            lbResult.BackColor = Color.Empty;
        }

        private void clearFlag()
        {
            requestFWversion = requestBattery =
            requestSerialRead = requestSerialWrite = requestFactoryReset =
            requestDeviceMode = requestCheckRegion = requestCheckRegionVP = false;

            requestWriteRegion = false;

            WriteDeviceMode = false;
            HeadsetConnected = false;
            CheckModelName = false;
            CheckBdAddress = false;
            CheckFwVersion = false;
            CheckFwRegion = false;
            CheckFactoryReset = false;
            CheckAwsInfo = false;
            CheckHsBatteryLevel = false;
            WriteSerial = false;
            CheckSerial = false;
            CheckDeviceMode = false;
            WriteRegion = false;
            CheckRegion = false;
            CheckColor = false;
            CheckRegionVP = false;

            // clear variables
            dutSerial = "";
            dutBdNap = "";
            dutBdUap = "";
            dutBdLap = "";
            dutFullBdAddress = "";
            dutModelName = "";
            dutVersionRevision = "";
            dutFullVersion = "";
            dutDeviceMode = false;
            dutRegion = "";
            dutColorCode = "";
            dutRegion = "";
            dutRegionVP = "";

            dutBatteryLevel = 0;


            for (int i = 0; i < 30; i++)
            {
                dutModelNameByte[i] = 0x00;
            }

        }

        private void NgSound()
        {
            SoundPlayer sound1 = new SoundPlayer(gSoundNg1);
            //SoundPlayer sound2 = new SoundPlayer(gSoundNg2);

            sound1.Play();
            //sound2.PlaySync();
        }

        private void startTest()
        {
            clearDisp();
            clearFlag();

            backWork2 bg = new backWork2(this);
            Thread workerThread = new Thread(bg.DoWork);
            workerThread.Start();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            startTest();
            //label6.Text = "";
        }

#if (ENABLE_WIRELESS_TEST)
        public bool initUsbHid()
        {
            int flag_ng = 0;
            String portsStr_S;

            // setup a scanner before hand
            scanner = new DeviceScanner(0x0a12, 0x1004);
            scanner.DeviceArrived += enter;
            scanner.DeviceRemoved += exit;
            scanner.StartAsyncScan();
            Console.WriteLine("OK");
            scanner.StopAsyncScan();

            portsStr_S = scanner.ToString();
            usbdbg_name = portsStr_S;

            try
            {
                // this can all happen inside a using(...) statement
                dev = new USBDevice(0x0a12, 0x1004, 0x0001, 0xFFA0, null, false, 121);

                // do not use below for windows 10 compatability...
                //Console.WriteLine(dev.Description());

                dev.SetNonBlock();

                // add handle for data read
                dev.InputReportArrivedEvent += handle;
                // after adding the handle start reading
                dev.StartAsyncRead();
            }
            catch
            {
                flag_ng = 1;
                usbdbg_name = "(Not Connected)";
            }

            if (flag_ng == 1) { return false; }
            else { return true; }
        }

        public static void handle(object s, USBInterface.ReportEventArgs a)
        {
            if (a.Data[0] == 0x02 && a.Data[1] == 0x0b && a.Data[2] == USB_DEVICE_DATA_RSP_HS_CONNECTED) // check connected
            {
                if (!HeadsetConnected)
                    HeadsetConnected = true;
            }
#if false
            else if (a.Data[0] == 0x02 && a.Data[1] == 0x0b && a.Data[2] == 0x03 && a.Data[3] == 0x01) // check disconnected
            {
                if (HeadsetConnected)
                    HeadsetConnected = false;
            }
#endif
            else if (a.Data[0] == 0x02 && a.Data[1] == 0x0b && a.Data[2] == USB_DEVICE_DATA_RSP_HS_REMOTE_BD_ADDR) // check remote bd address
            {
                if (!CheckBdAddress)
                {
                    dutBd06 = a.Data[3];
                    dutBd05 = a.Data[4];
                    dutBd04 = a.Data[5];
                    dutBd03 = a.Data[6];
                    dutBd02 = a.Data[7];
                    dutBd01 = a.Data[8];

                    dutBdNap = dutBd01.ToString("X2") + dutBd02.ToString("X2");
                    dutBdUap = dutBd03.ToString("X2");
                    dutBdLap = dutBd04.ToString("X2") + dutBd05.ToString("X2") + dutBd06.ToString("X2");

                    dutFullBdAddress = dutBdNap + dutBdUap + dutBdLap;

                    CheckBdAddress = true;
                }

            }
            else if (a.Data[0] == 0x02 && a.Data[1] == 0x0b && a.Data[2] == USB_DEVICE_DATA_RSP_HS_REMOTE_NAME) // check remote name
            {
                if (!CheckModelName)
                {
                    int x = 0;
                    for (int i = 0; i < 30; i++)
                    {
                        dutModelNameByte[i] = a.Data[i + 3];
                        if (a.Data[i + 3] == '\0') { x = i; break; }
                    }
                    //dutModelName = dutModelNameByte.ToString();
                    dutModelName = Encoding.UTF8.GetString(dutModelNameByte, 0, x);
                    CheckModelName = true;
                }

            }
#if true
            else if (a.Data[0] == 0x02 && a.Data[1] == 0x0b && a.Data[2] == USB_DEVICE_DATA_RSP_HS_SPP_RX) // check factory reset
            {
                Console.WriteLine("_____[kimgh] SPP RX Received!!!!");
                if (a.Data[3] != 0)
                {
                    int size = a.Data[3];
                    for (int i = 0; i < size; i++)
                    {
                        Console.Write("{0:X2}", a.Data[i + 4]);
                        Console.Write(" ");
                    }
                    Console.WriteLine("");

                    byte[] dataForProc = new byte[size];
                    //Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));
                    Buffer.BlockCopy(a.Data, 4, dataForProc, 0, size);
                    procRaceCmdData(dataForProc);
                }
            }
#endif
        }

#if true
        public static void procRaceCmdData(byte[] race_data)
        {
            byte RaceChannel = 0;
            byte RaceType = 0;
            int RaceId = 0;
            int NvkeyReadLength = 0;
            int RespLength = 0;
            int DataLength = 0;
            int size = 0;

            RaceChannel = race_data[0];
            RaceType = race_data[1];

            RaceId |= ((int)(race_data[4] << 0));
            RaceId |= ((int)(race_data[5] << 8));

            RespLength = race_data[2] + (race_data[3] * 255);

            DataLength = Buffer.ByteLength(race_data);

            needProcessDstData = false;

            if (RaceChannel == RACE_CHANNEL)
            {
                if (DataLength == RespLength + 4)
                {
                    needProcessMoreData = false;
                }
                else if (DataLength > RespLength + 4)
                {
                    needProcessMoreData = true;
                }

                switch (RaceType)
                {
                    case RACE_TYPE_RESP: //
                        switch (RaceId)
                        {
                            case RACE_ID_READ_FULLKEY:
                                NvkeyReadLength = race_data[6] + (race_data[7] * 255);
                                Console.WriteLine("_____[kimgh] NvkeyReadLength = {0}", NvkeyReadLength);
                                if (NvkeyReadLength > 0)
                                {
                                    if (requestSerialRead)
                                    {
                                        if (NvkeyReadLength == 7)
                                        {
                                            Console.WriteLine("_____[kimgh] got Agent SN : {0} {1} {2} {3} {4} {5} {6}",
                                                race_data[8].ToString("X2"), race_data[9].ToString("X2"), race_data[10].ToString("X2"), race_data[11].ToString("X2"), race_data[12].ToString("X2"), race_data[13].ToString("X2"), race_data[14].ToString("X2"));

                                            byte[] temp_dutSerialAgent = new byte[7];
                                            for (int i = 0; i < 7; i++)
                                            {
                                                temp_dutSerialAgent[i] = race_data[i + 8];
                                            }
                                            dutSerial = Encoding.ASCII.GetString(temp_dutSerialAgent).Trim('\0');

                                            CheckSerial = true;
                                            requestSerialRead = false;

                                        }
                                        else
                                        {
                                            tryAgain = true;
                                        }
                                    }
                                    else if (requestCheckRegion)
                                    {
                                        if (NvkeyReadLength == 1)
                                        {
                                            Console.WriteLine("_____[kimgh] got Region : Region ({0})", (race_data[8]).ToString());

                                            switch (race_data[8])
                                            {
                                                case 0x01: // UC
                                                    dutRegion = "UC";
                                                    break;
                                                case 0x02:
                                                    dutRegion = "J";
                                                    break;
                                                case 0x03:
                                                    dutRegion = "CE7";
                                                    break;
                                                case 0x04:
                                                    dutRegion = "E";
                                                    break;
                                                case 0x05:
                                                    dutRegion = "IN";
                                                    break;
                                                case 0x06:
                                                    dutRegion = "CN";
                                                    break;
                                                default:
                                                    dutRegion = "NONE";
                                                    break;
                                            }

                                            CheckRegion = true;
                                            requestCheckRegion = false;
                                            failRetryCount = 0;
                                        }
                                        else
                                        {
                                            tryAgain = true;
                                        }
                                    }
                                    else if (requestColor)
                                    {
                                        if (NvkeyReadLength == 1)
                                        {
                                            Console.WriteLine("_____[kimgh] got Color : color ({0})", (race_data[8]).ToString());

                                            switch (race_data[8])
                                            {
                                                case 0x01: // Black
                                                    dutColorCode = "BLACK";
                                                    break;
                                                //case 0x02:
                                                //    dutColorCode = "WHITE";
                                                //    break;
                                                //case 0x05:
                                                //    dutColorCode = "BLUE";
                                                //    break;
                                                //case 0x06:
                                                //    dutColorCode = "PINK";
                                                //    break;
                                                default:
                                                    dutColorCode = "NONE";
                                                    break;
                                            }

                                            CheckColor = true;
                                            requestColor = false;
                                            failRetryCount = 0;
                                        }
                                        else
                                        {
                                            tryAgain = true;
                                        }
                                    }
                                    else if (requestDeviceMode)
                                    {
                                        byte lMode = 0x00;

                                        if (NvkeyReadLength == 64)
                                        {
                                            switch (Form1.gFwMode)
                                            {
                                                case "USER":
                                                    lMode = 0x00;
                                                    break;
                                                case "FACTORY":
                                                    lMode = 0x01;
                                                    break;
                                                default:
                                                    break;
                                            }

                                            if (race_data[8] == lMode)
                                            {
                                                dutDeviceMode = true;
                                                CheckDeviceMode = true;
                                            }
                                            else
                                            {
                                                dutDeviceMode = false;
                                                CheckDeviceMode = false;
                                            }

                                            requestDeviceMode = false;
                                        }
                                        else
                                        {
                                            tryAgain = true;
                                        }
                                    }
                                }
                                else if (NvkeyReadLength == 0)
                                {
                                    tryAgain = true;
                                }
                                break;
                            case RACE_ID_WRITE_FULLKEY:
                                if (race_data[6] == 0x00) // 0x00 mean write OK
                                {
                                    if (requestSerialWrite)
                                    {
                                        Console.WriteLine("_____[kimgh] got Serial Write Success");
                                        WriteSerial = true;
                                        requestSerialWrite = false;
                                    }
                                    else if (requestWriteRegion)
                                    {
                                        Console.WriteLine("_____[kimgh] got Region Write Success");
                                        WriteRegion = true;
                                        requestWriteRegion = false;
                                    }
                                    else if (requestWriteColor)
                                    {
                                        Console.WriteLine("_____[kimgh] got Color Write Success");
                                        WriteColor = true;
                                        requestWriteColor = false;
                                    }
                                    else if (requestWriteDeviceMode)
                                    {
                                        Console.WriteLine("_____[kimgh] got Write Mode Success");
                                        Write_DeviceMode = true;
                                        WriteDeviceMode = true;
                                        requestWriteDeviceMode = false;
                                    }
                                }
                                else
                                {
                                    tryAgain = true;
                                }
                                break;
                            case RACE_ID_ENABLE_KEY_EVENT:
                                if (race_data[6] == 0x00)
                                {
                                    if (requestFactoryReset)
                                    {
                                        Console.WriteLine("_____[kimgh] got Factory Reset Success");
                                        CheckFactoryReset = true;
                                        requestFactoryReset = false;
                                    }
                                }
                                break;

                            case RACE_ID_READ_REGION_VP:
                                if (requestCheckRegionVP)
                                {
                                    if (race_data[6] == 0x01) //EN
                                    {
                                        dutRegionVP = "EN";
                                    }
                                    else if (race_data[6] == 0x0B) //J
                                    {
                                        dutRegionVP = "J";
                                    }
                                    else if (race_data[6] == 0xF0) //CN
                                    {
                                        dutRegionVP = "CN";
                                    }
                                    else
                                    {
                                        dutRegionVP = "UNKNOWN";
                                    }

                                    CheckRegionVP = true;
                                    requestCheckRegionVP = false;
                                }
                                else
                                {
                                    tryAgain = true;
                                }
                                break;

                        } // switch(RaceId)
                        break;
                    case RACE_TYPE_NOTI:
                        switch (RaceId)
                        {
                            case RACE_ID_READ_FULLKEY:
                                break;
                            case RACE_ID_READ_BATTERY:
                                if (race_data[6] == 0x00) // status : OK
                                {
                                    if (requestBattery)
                                    {
                                        Console.WriteLine("_____[kimgh] got battery level : {0}%", race_data[8]);
                                        dutBatteryLevel = race_data[8];
                                        CheckHsBatteryLevel = true;
                                    }

                                    requestBattery = false;
                                }
                                break;
                            case RACE_ID_READ_FW_INFO:
                                if (race_data[6] == 0x00) // status : OK
                                {
                                    if (requestFWversion)
                                    {
                                        byte[] temp_dutFullVersion = new byte[5];
                                        for (int i = 0; i < 5; i++)
                                        {
                                            temp_dutFullVersion[i] = race_data[i + 9];
                                        }
                                        dutFullVersion = System.Text.Encoding.ASCII.GetString(temp_dutFullVersion).Trim('\0');

                                        Console.WriteLine("_____[kimgh] got FW Version : {0}", dutFullVersion);
                                        CheckFwVersion = true;
                                    }
                                    requestFWversion = false;
                                }
                                break;
                            case RACE_ID_CMDRELAY_PASS_TO_DST: // get this by Notification mean,  send command to  to get  information
                                if (RespLength + 4 == DataLength)
                                {
                                    if (race_data[6] == 0x05 && race_data[8] == 0x05 && (race_data[9] == 0x5D || race_data[9] == 0x5B))
                                    {
                                        needProcessDstData = true;
                                    }
                                }
                                break;
                        }
                        break;
                } // switch(RaceType)

                // processing more data
                if (needProcessMoreData)
                {
                    size = DataLength - (RespLength + 4);
                    byte[] dataForProcMoreData = new byte[size];
                    //Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));
                    Buffer.BlockCopy(race_data, (RespLength + 4), dataForProcMoreData, 0, size);
                    procRaceCmdData(dataForProcMoreData);
                }

                if (needProcessDstData)
                {
                    size = DataLength - 8;
                    byte[] dataForProcDstData = new byte[size];
                    Buffer.BlockCopy(race_data, 8, dataForProcDstData, 0, size);
                    procRaceCmdData(dataForProcDstData);
                }
            }
        }
#endif

        public static void enter(object s, EventArgs a)
        {
            Console.WriteLine("device arrived");
        }
        public static void exit(object s, EventArgs a)
        {
            Console.WriteLine("device removed");
        }
#endif

        #region delegates
        delegate void writeDupeDataDelegate(string bd, string sn); // dupe bd write
        public void writeDupeData(string bd, string sn)
        {
            if (InvokeRequired)
            {
                writeDupeDataDelegate writeDupeDel = new writeDupeDataDelegate(writeDupeData);
                Invoke(writeDupeDel, bd, sn);
            }
            else
            {
                // add dupe data to datagrid2
                string[] rowrow = { passCount.ToString(), bd, sn, "OK", "0" };
                dataGridView2.Rows.Add(rowrow);

                // add dupe data to log
                writeDupeLog(passCount.ToString(), bd, sn, "OK", "0");
            }
        }

        delegate bool findDupeDataDelegate(string bd, string sn); // dupe bd find
        public bool findDupeData(string bd, string sn)
        {
            if (InvokeRequired)
            {
                findDupeDataDelegate findDupeDel = new findDupeDataDelegate(findDupeData);
                return (bool)Invoke(findDupeDel, bd, sn);
            }
            else
            {
                int count = 0;
                int row = 0;
                bool isDupe = false;

                count = dataGridView2.Rows.Count - 1;

                if (count != 0)
                {
                    for (row = 0; row < count; row++)
                    {
                        string ttt = dataGridView2.Rows[row].Cells[1].Value.ToString();
                        if (bd == dataGridView2.Rows[row].Cells[1].Value.ToString())
                        {
                            // focus?
                            dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows[row].Index;

                            // change dupe count
                            string strTempCount = dataGridView2.Rows[row].Cells[4].Value.ToString();
                            dataGridView2.Rows[row].Cells[4].Value = (Convert.ToInt32(strTempCount) + 1).ToString();

                            dataGridView2.Rows[row].DefaultCellStyle.BackColor = Color.Red;
                            gLatestDupeRow = row;
                            isDupe = true;
                            break;
                        }
                    }
                }

                if (!isDupe) { return false; }
                else { return true; }
            }

        }

        delegate bool findSNDupeDataDelegate(string sn); // dupe bd find
        public bool findSNDupeData(string sn)
        {
            if (InvokeRequired)
            {
                try
                {
                    findSNDupeDataDelegate findSNDupeDel = new findSNDupeDataDelegate(findSNDupeData);
                    return (bool)Invoke(findSNDupeDel, sn);
                }
                catch { return true; }
            }
            else
            {
                int count = 0;
                int row = 0;
                bool isSNDupe = false;

                count = dataGridView2.Rows.Count - 1;

                if (count != 0)
                {
                    for (row = 0; row < count; row++)
                    {
                        string ttt = dataGridView2.Rows[row].Cells[2].Value.ToString();
                        if ((sn == dataGridView2.Rows[row].Cells[2].Value.ToString()) || (sn == dataGridView2.Rows[row].Cells[3].Value.ToString()))
                        {
                            // focus?
                            dataGridView2.FirstDisplayedScrollingRowIndex = dataGridView2.Rows[row].Index;

                            // change dupe count
                            string strTempCount = dataGridView2.Rows[row].Cells[5].Value.ToString();
                            dataGridView2.Rows[row].Cells[5].Value = (Convert.ToInt32(strTempCount) + 1).ToString();

                            dataGridView2.Rows[row].DefaultCellStyle.BackColor = Color.Red;
                            gLatestDupeRow = row;
                            isSNDupe = true;
                            break;
                        }
                    }
                }

                if (!isSNDupe) { return false; }
                else { return true; }
            }

        }

        delegate void showResultDelegate(int result, string bd);
        public void showResult(int result, string bd)
        {
            IniReadWrite IniWriter = new IniReadWrite();

            if (InvokeRequired)
            {
                showResultDelegate resultDel = new showResultDelegate(showResult);
                Invoke(resultDel, result, bd);
            }
            else
            {
                string log_dutversion = dutFullVersion;
                string log_dutRegion = dutRegion;
                string log_color = dutColorCode;
                string log_sn = dutSerial;
                string log_Battery = dutBatteryLevel + "%";

                if (result == 1)
                {
                    // write log                 
                    writeLog(bd, "FAIL", dutModelName, log_Battery, log_dutversion, log_dutRegion, log_color, log_sn, typeNg);
                    totalCount++;
                    failCount++;

                    tb_totalCount.Text = totalCount.ToString();
                    tb_failCount.Text = failCount.ToString();

                    lbResult.Text = "FAIL";
                    lbResult.BackColor = Color.Red;

                    openpassworddlg();

                    NgSound();
                }
                else
                {
                    // write log                   
                    writeLog(bd, "PASS", dutModelName, log_Battery, log_dutversion, log_dutRegion, log_color, log_sn, "-");
                    totalCount++;
                    passCount++;

                    IniWriter.IniWriteValue("CONFIG", "WRITE_SERIAL_END", gSerialInput, pathConfigFile);

                    tb_totalCount.Text = totalCount.ToString();
                    tb_passCount.Text = passCount.ToString();

                    lbResult.Text = "PASS";
                    lbResult.BackColor = Color.Blue;
                }
            }
        }

        delegate void toggleBtnDelegate(Boolean x); // start button control
        public void toggleBtn(Boolean x)
        {
            if (InvokeRequired)
            {
                toggleBtnDelegate btnDel = new toggleBtnDelegate(toggleBtn);
                Invoke(btnDel, x);
            }
            else
            {
                if (x == true) { BtConnect.Enabled = false; btnStart.Enabled = true; btnStart.Focus(); }
                else { btnStart.Enabled = false; }
            }
        }

        delegate void toggleSerialInputDelegate(Boolean x);
        public void toggleSerialInputBtn(Boolean x)
        {
            if (InvokeRequired)
            {
                toggleSerialInputDelegate serialInputbtnDel = new toggleSerialInputDelegate(toggleSerialInputBtn);
                Invoke(serialInputbtnDel, x);
            }
            else
            {
                if (x == true)
                {
                    textBox1.Enabled = true;
                    textBox2.Enabled = true;
                    textBox4.Enabled = true;
                    BtConnect.Enabled = true;

                    textBox1.BackColor = Color.Yellow;
                    textBox2.BackColor = Color.White;
                    textBox4.BackColor = Color.White;

                    BtConnect.PerformClick();

                    textBox1.Focus();
                }
                else
                {
                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox4.Enabled = false;
                    BtConnect.Enabled = false;
                }
            }
        }

        delegate void ShowErrorDelegates(string s); // temp
        public void showError(string s)
        {
            if (InvokeRequired)
            {
                ShowErrorDelegates eDel = new ShowErrorDelegates(showError);
                Invoke(eDel, s);
            }
            else
            {
                this.label1.Text = s;
                this.label1.ForeColor = Color.Red;
            }
        }

        delegate void ShowDelegate2(int row, string result); // show progress and result
        public void ShowProgress2(int row, string result)
        {
            if (InvokeRequired)
            {
                ShowDelegate2 del = new ShowDelegate2(ShowProgress2);
                //또는 ShowDelegate del = p => ShowProgress(p);
                Invoke(del, row, result);
            }
            else
            {
                //progressBar1.Value = pct;
                dataGridView1.Rows[row].Cells[3].Value = gTestValue[row];
                dataGridView1.Rows[row].Cells[4].Value = result;

                if (result == "FAIL")
                {
                    dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.Red;
                    typeNg = gNgType[row];
                }
                else if (result == "PASS")
                {
                    dataGridView1.Rows[row].DefaultCellStyle.BackColor = Color.SkyBlue;
                }
            }
        }

#if true
        delegate void ShowProgressIndicatorDelegate(int row, bool x);
        public void ShowProgressIndicator(int row, bool x)
        {
            if (InvokeRequired)
            {
                ShowProgressIndicatorDelegate del = new ShowProgressIndicatorDelegate(ShowProgressIndicator);
                Invoke(del, row, x);
            }
            else
            {
                if (x == true)
                {
                    dataGridView1.Rows[row].Cells[3].Value = dataGridView1.Rows[row].Cells[3].Value + ">";
                }
                else
                {
                    dataGridView1.Rows[row].Cells[3].Value = "";
                }

            }
        }
#endif
        delegate void clearSerialDelegate();
        public void clearSerial()
        {
            if (InvokeRequired)
            {
                clearSerialDelegate clear = new clearSerialDelegate(clearSerial);
                Invoke(clear);
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox4.Text = "";

            }
        }
        #endregion

        #region config_relative
        private bool checkConfigSetting()
        {
            bool flag_ng = false;

            if (!flag_ng)
            {
                switch (gFwRegion)
                {
                    case "UC":
                    case "CE7":
                    case "E":
                    case "IN":
                    case "J":
                    case "CN":
                        break;
                    default:
                        flag_ng = true;
                        break;
                }
            }

            if (!flag_ng)
            {
                switch (gFwColor)
                {
                    case "BLACK":
                        //case "BLUE":
                        //case "BEIGE":
                        //case "WHITE":
                        //case "PINK":
                        //case "YELLOW":
                        break;
                    default:
                        flag_ng = true;
                        break;
                }
            }

            if (!flag_ng)
            {
                if (gBatteryLevelLow == 255) flag_ng = true;
                if (gBatteryLevelHigh == 255) flag_ng = true;
            }

            if (flag_ng)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void loadConfig()
        {
            // using ini style config
            pathConfigFile = System.IO.Directory.GetCurrentDirectory() + "\\" + "config\\" + "config.ini";
            string tempString;

            IniReadWrite IniReader = new IniReadWrite();

            tempString = IniReader.IniReadValue("CONFIG", "START_BUTTON_ENABLE", pathConfigFile);
            gStartbutton = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "MODEL_NAME", pathConfigFile);
            gDutName = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "SW_VERSION", pathConfigFile);
            gSwVersion = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "MANUFACTURE_DATE", pathConfigFile);
            gManufactureDate = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "DEFAULT_BD_NAP", pathConfigFile);
            gDefaultBdNap = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "DEFAULT_BD_UAP", pathConfigFile);
            gDefaultBdUap = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "DEFAULT_BD_LAP", pathConfigFile);
            gDefaultBdLap = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "BD_NAP", pathConfigFile);
            gBdNap = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "BD_UAP", pathConfigFile);
            gBdUap = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "BD_LAP_START", pathConfigFile);
            gBdLapStart = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "BD_LAP_END", pathConfigFile);
            gBdLapEnd = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "SERIAL_START", pathConfigFile);
            gSerialStart = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "SERIAL_END", pathConfigFile);
            gSerialEnd = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "WRITE_SERIAL_END", pathConfigFile);
            gSerialEndWrite = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "APPNAME", pathConfigFile);
            gAppName = tempString;
            lb_AppName.Text = gAppName;
            lb_AppName.BackColor = Color.LightYellow;
            lb_AppName.ForeColor = Color.Green;

            tempString = IniReader.IniReadValue("CONFIG", "FW_REGION", pathConfigFile);
            gFwRegion = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "FW_COLOR", pathConfigFile);
            gFwColor = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "FW_MODE", pathConfigFile);
            gFwMode = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "SN_FIRST_LETTER_CHECK", pathConfigFile);
            gSNFirstLetterCheck = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "BATTERY_LEVEL_LOW", pathConfigFile);
            if (!int.TryParse(tempString, out gBatteryLevelLow))
            {
                gBatteryLevelLow = 255;
            }
            if (gBatteryLevelLow > 100) { gBatteryLevelLow = 255; }

            tempString = IniReader.IniReadValue("CONFIG", "BATTERY_LEVEL_HIGH", pathConfigFile);
            if (!int.TryParse(tempString, out gBatteryLevelHigh))
            {
                gBatteryLevelHigh = 255;
            }
            if (gBatteryLevelHigh > 100) { gBatteryLevelHigh = 255; }

            // setup test list
            // max item of sequence is 20
            string tempProc = "";
            for (int i = 0; i < 20; i++)
            {
                tempProc = String.Format("TEST_{0}", i.ToString("D2"));
                //updateStatus(String.Format("High Temp(): {0}", count.ToString()));
                tempString = IniReader.IniReadValue("SEQUENCE", tempProc, pathConfigFile);
                if (tempString == "END")
                {
                    gCountSequence = i;
                    break;
                }
                else
                {
                    gtestSequence[i] = tempString;
                }
            }

            // display test list
            for (int j = 0; j < gCountSequence; j++)
            {
                if (gtestSequence[j] == "CHECK_MODEL_NAME")
                {
                    string[] rowInfo = { j.ToString(), gtestSequence[j], gDutName, "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
                else if (gtestSequence[j] == "CHECK_FW_VERSION")
                {
                    string[] rowInfo = { j.ToString(), gtestSequence[j], gSwVersion, "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
                else if (gtestSequence[j] == "WRITE_DEVICE_MODE")
                {
                    string[] rowInfo = { j.ToString(), gtestSequence[j], gFwMode, "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
                else if (gtestSequence[j] == "CHECK_DEVICE_MODE")
                {
                    string[] rowInfo = { j.ToString(), gtestSequence[j], gFwMode, "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
                else if (gtestSequence[j] == "CHECK_BD_DEFAULT_FOR_CHECK")
                {
                    string[] rowInfo = { j.ToString(), gtestSequence[j], String.Format(gDefaultBdNap + gDefaultBdUap + gDefaultBdLap), "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
                else if (gtestSequence[j] == "CHECK_BD_RANGE")
                {
                    string[] rowInfo = { j.ToString(), gtestSequence[j], String.Format(gBdNap + gBdUap + "({0} ~ {1})", gBdLapStart, gBdLapEnd), "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
                else if (gtestSequence[j] == "WRITE_COLOR")
                {
                    string[] rowInfo = { j.ToString(), gtestSequence[j], String.Format("{0}", gFwColor), "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
                else if (gtestSequence[j] == "CHECK_COLOR")
                {
                    string[] rowInfo = { j.ToString(), gtestSequence[j], String.Format("{0}", gFwColor), "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
                else if (gtestSequence[j] == "WRITE_REGION")
                {
                    string[] rowInfo = { j.ToString(), gtestSequence[j], String.Format("{0}", gFwRegion), "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
                else if (gtestSequence[j] == "CHECK_REGION")
                {
                    string[] rowInfo = { j.ToString(), gtestSequence[j], String.Format("{0}", gFwRegion), "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
                else if (gtestSequence[j] == "CHECK_REGION_VP")
                {
                    if (Form1.gFwRegion == "UC") Form1.tempFwRegion = "EN";
                    else if (Form1.gFwRegion == "CE7") Form1.tempFwRegion = "EN";
                    else if (Form1.gFwRegion == "E") Form1.tempFwRegion = "EN";
                    else if (Form1.gFwRegion == "IN") Form1.tempFwRegion = "EN";
                    else if (Form1.gFwRegion == "J") Form1.tempFwRegion = "J";
                    else if (Form1.gFwRegion == "CN") Form1.tempFwRegion = "CN";
                    else Form1.tempFwRegion = "UNKNOWN";

                    string[] rowInfo = { j.ToString(), gtestSequence[j], tempFwRegion, "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
                else if (gtestSequence[j] == "CHECK_BATTERY_LEVEL")
                {
                    string[] rowInfo = { j.ToString(), gtestSequence[j], String.Format("{0}%" + " ≤ Level ≤ " + "{1}%", gBatteryLevelLow, gBatteryLevelHigh), "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
                else if (gtestSequence[j] == "WRITE_SERIAL_RESULT")
                {
                    string[] rowInfo = { j.ToString(), gtestSequence[j], "( Last No :  " + gSerialEndWrite + " )", "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
                else if (gtestSequence[j] == "CHECK_SERIAL_RESULT")
                {
                    string[] rowInfo = { j.ToString(), gtestSequence[j], String.Format("{0} ~ {1}", gSerialStart, gSerialEnd), "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
                else
                {
                    string[] rowInfo = { j.ToString(), gtestSequence[j], "-", "-", "-" };
                    dataGridView1.Rows.Add(rowInfo);
                }
            }

            // check bd setting range
            // check bd range
            UInt32 tempConfigLapStart = Convert.ToUInt32(Form1.gBdLapStart, 16);
            UInt32 tempConfigLapEnd = Convert.ToUInt32(Form1.gBdLapEnd, 16);

            if (tempConfigLapEnd <= tempConfigLapStart)
            {
                MessageBox.Show("Check BD Address Range Config");
                Close();
            }
        }
        #endregion

        #region log_relative
        private void writeLog(string bd, string result, string model_name, string batteryLV, string fwver, string region, string color, string sn, string faildesc)
        {
            string date = DateTime.Now.ToString("yy-MM-dd HH:mm:ss");

            // open stream
            StreamWriter sw = new StreamWriter(pathLogFile, true, Encoding.Unicode);

            // write measured data
            sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}\t", date, bd, result, model_name, batteryLV, fwver, region, color, sn, faildesc);
            // close stream
            sw.Close();
        }

        private bool checkLogFile()
        {
            DateTime today = DateTime.Now;
            string strDatePrefix = today.ToString("yy-MM-dd");
            string strLogFile = strDatePrefix + "_" + gDutName + "-log.csv";

            // set file path
            pathLogFile = System.IO.Directory.GetCurrentDirectory() + "\\" + "log\\" + strLogFile;

            if (File.Exists(pathLogFile))
            {
                // re-set test Cound
                StreamReader sr = new StreamReader(pathLogFile);
                string line;
                UInt32 lineCount = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    lineCount++;
                }
                sr.Close();
                totalCount = lineCount - 1;

                return true;
            }
            else
            {
                //make new log file
                StreamWriter sw = new StreamWriter(pathLogFile, true, Encoding.Unicode);

                // write basic data (index)
                sw.WriteLine("Date\t BDA\t Result\t Mode_Name\t Battery_LV\t FW_Ver\t Region\t Color\t S/N\t NG_Description");

                sw.Close();
                totalCount = 0;

                return false;
            }
        }

        private void writeDupeLog(string num, string bd, string sn, string result, string dupeCount)
        {
            string date = DateTime.Now.ToString("yy-MM-dd-HH-mm-ss");

            // open stream
            StreamWriter swd = new StreamWriter(pathDupeLogFile, true, Encoding.Unicode);


            // write measured data
            swd.WriteLine("{0},{1},{2},{3},{4}", num, bd, sn, result, dupeCount);
            // close stream
            swd.Close();
        }

        private bool checkDupeLogFile()
        {
            DateTime today = DateTime.Now;
            string strDatePrefix = today.ToString("yy-MM-dd");
            string strLogFile = strDatePrefix + "_" + gDutName + "-dupelog.dat";

            // set file path
            pathDupeLogFile = System.IO.Directory.GetCurrentDirectory() + "\\" + "log\\" + strLogFile;

            if (File.Exists(pathDupeLogFile))
            {
                // re-set test Cound
                StreamReader srd = new StreamReader(pathDupeLogFile);
                string line;
                UInt32 lineCount = 0;
                while ((line = srd.ReadLine()) != null)
                {
                    lineCount++;
                    string[] rowrow = line.Split(',');
                    dataGridView2.Rows.Add(rowrow);
                }
                srd.Close();

                passCount = lineCount;

                return true;
            }
            else
            {
                //make new log file
                StreamWriter swd = new StreamWriter(pathDupeLogFile, true, Encoding.Unicode);

                // write basic data (index)
                //swd.WriteLine("date" + "," + "bd" + "," + "result" + "," + "ng description");
                swd.Close();

                return false;
            }
        }
        #endregion

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (dev != null)
                dev.Dispose();

            try
            {
                TcpClient = null;
            }
            catch
            {
                TcpClient.Dispose();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dev != null)
                dev.Dispose();

            try
            {
                TcpClient = null;
            }
            catch
            {
                TcpClient.Dispose();
            }
        }


        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int flag_ng = 0;

                if (textBox1.Text != "")
                {
                    if (textBox1.Text.Length == 11)
                    {
                        if (textBox1.Text.Substring(0, 3) == "S01")
                        {
                            string SNnumber;
                            SNnumber = textBox1.Text.Substring(3, 7);

                            if (gSNFirstLetterCheck == "NO")
                            {
                                if (SNnumber.Length == 7)
                                {
                                    //label6.Text = SNnumber;
                                    gSerialInput = SNnumber.ToUpper();
                                    textBox1.Text = gSerialInput;
                                }
                                else
                                {
                                    flag_ng = 1;
                                    textBox1.Text = "";
                                }
                            }
                            else if (gSNFirstLetterCheck == "YES")
                            {
                                string firstletter = textBox1.Text.Substring(3, 1); ;

                                switch (firstletter)
                                {
                                    case "4":
                                        if (gFwRegion == "UC") { }
                                        else
                                        {
                                            flag_ng = 1;
                                            MessageBox.Show("Error : plz Check BarcodeLabel and Config file of FW_Region");
                                        }
                                        break;

                                    case "5":
                                        if (gFwRegion == "J") { }
                                        else
                                        {
                                            flag_ng = 1;
                                            MessageBox.Show("Error : plz Check BarcodeLabel and Config file of FW_Region");
                                        }
                                        break;
                                    case "6":
                                        if (gFwRegion == "CE7") { }
                                        else
                                        {
                                            flag_ng = 1;
                                            MessageBox.Show("Error : plz Check BarcodeLabel and Config file of FW_Region");
                                        }
                                        break;
                                    case "7":
                                        if (gFwRegion == "E") { }
                                        else
                                        {
                                            flag_ng = 1;
                                            MessageBox.Show("Error : plz Check BarcodeLabel and Config file of FW_Region");
                                        }
                                        break;
                                    case "8":
                                        if (gFwRegion == "CN") { }
                                        else
                                        {
                                            flag_ng = 1;
                                            MessageBox.Show("Error : plz Check BarcodeLabel and Config file of FW_Region");
                                        }
                                        break;
                                    case "9":
                                        if (gFwRegion == "IN") { }
                                        else
                                        {
                                            flag_ng = 1;
                                            MessageBox.Show("Error : plz Check BarcodeLabel and Config file of FW_Region");
                                        }
                                        break;
                                    default:
                                        MessageBox.Show("Error : plz Check BarcodeLabel and Config file of FW_Region");
                                        flag_ng = 1;
                                        break;
                                }

                                if (flag_ng != 1)
                                {
                                    if (SNnumber.Length == 7)
                                    {
                                        //label6.Text = SNnumber;
                                        gSerialInput = SNnumber.ToUpper();
                                        textBox1.Text = gSerialInput;
                                    }
                                    else
                                    {
                                        flag_ng = 1;
                                    }
                                }
                            }
                            else
                            {
                                flag_ng = 1;
                            }
                        }
                        else
                        {
                            flag_ng = 1;
                        }
                    }
                    else
                    {
                        flag_ng = 1;
                    }
                }
                else
                {
                    flag_ng = 1;
                }

                if (textBox1.Text != "")
                {
                    if (flag_ng == 0)
                    {
                        textBox1.BackColor = Color.White;
                        textBox2.BackColor = Color.Yellow;
                        textBox4.BackColor = Color.Yellow;

                        textBox2.Focus();

                        BtConnect.PerformClick();
                    }
                    else
                    {
                        openpassworddlg();
                        textBox1.Text = "";
                    }
                }
            }
        }

        private void openpassworddlg()
        {
            passwordbox password = new passwordbox();
            password.ShowDialog();
        }

        private void BtConnect_Click(object sender, EventArgs e)
        {
            if (TcpClient.Connected)
            {
                TcpClient.Disconnect();
            }
            else
            {
                this.timer.Enabled = true;
                this.timer.Interval = 2000;
                this.timer.Start();

                TcpClient.HostName = TxtHostName.Text;
                TcpClient.Port = 8500;
                TcpClient.Connect();

                command = "OF,01\r\n";
                TcpClient.Write(command);

            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (TcpClient.Connected)
            {
                SendCommand();
            }
        }
    }


    class backWork2
    {
        Form1 mainForm;

        byte[] bdAddress = new Byte[6];

        string tempNap;
        string tempUap;
        string tempLap;
        string tempSerial;

        public backWork2(Form1 frm)
        {
            mainForm = frm;
        }

        public void DoWork() // sequence base
        {
            int flag_ng = 0;

            //mainForm.toggleBtn(false);

            Form1.failRetryCount = 0;

            // start sequence
            for (int i = 0; i < Form1.gCountSequence; i++)
            {
                if (procTestSequence(mainForm.gtestSequence[i], i) == false)
                {
                    // (3 more by 500ms interval)
                    if (mainForm.gtestSequence[i] == "WRITE_SERIAL_RESULT")
                    {
                        if (Form1.failRetryCount < 3)
                        {
                            i--;
                            Form1.WriteSerial = false;
                            Form1.failRetryCount++;
                            Thread.Sleep(500);
                        }
                        else
                        {
                            flag_ng = 1;
                            mainForm.ShowProgress2(i, "FAIL");
                            break;
                        }
                    }
                    else if ((mainForm.gtestSequence[i] == "CHECK_SERIAL_CHECK" || mainForm.gtestSequence[i] == "CHECK_REGION" || mainForm.gtestSequence[i] == "CHECK_COLOR") && Form1.tryAgain)
                    {
                        Form1.tryAgain = false;

                        if (Form1.failRetryCount < 3)
                        {
                            i--;
                            Form1.CheckSerial = false;
                            Form1.CheckRegion = false;
                            Form1.CheckColor = false;
                            Form1.failRetryCount++;
                            Thread.Sleep(500);
                        }
                        else
                        {
                            flag_ng = 1;
                            mainForm.ShowProgress2(i, "FAIL");
                            break;
                        }
                    }
                    else
                    {
                        flag_ng = 1;
                        mainForm.ShowProgress2(i, "FAIL");
                        break;
                    }

                }
                else
                {
                    mainForm.ShowProgress2(i, "PASS");
                }
            }

            // show final result
            mainForm.showResult(flag_ng, Form1.dutFullBdAddress);

            // write dupe data (pass only)
            if (flag_ng == 0)
            {
                mainForm.writeDupeData(Form1.dutFullBdAddress, Form1.dutSerial);
            }

            Thread.Sleep(500);
            funcPowerOff();
            Thread.Sleep(500);

            // disconnect only
            Form1.dev.StartAsyncRead();
            byte[] data = new byte[19];

            data[0] = 0x02;
            data[1] = 0x0b;
            data[2] = Form1.USB_HOST_DATA_CMD_DISCONNECT;

            Form1.dev.Write(data);


            if (Form1.gStartbutton == "Enable")
            {
                mainForm.toggleBtn(true);

            }
            else if (Form1.gStartbutton == "Disable")
            {
                mainForm.clearSerial();
                mainForm.toggleSerialInputBtn(true);
            }

        }

        private bool validateBd(string xnap, string xuap, string xlap, string type)
        {
            int flag_ng = 0;

            switch (type)
            {
                case "check_default_for_check":
                    {
                        if ((Form1.gDefaultBdNap == xnap) && (Form1.gDefaultBdUap == xuap) && (Form1.gDefaultBdLap == xlap)) { flag_ng = 1; }
                    }
                    break;
                case "check_default_for_write":
                    {
                        if ((Form1.gDefaultBdNap != xnap) || (Form1.gDefaultBdUap != xuap) || (Form1.gDefaultBdLap != xlap)) { flag_ng = 1; }
                    }
                    break;
                case "check_range":
                    {
                        // convert
                        UInt16 tempConfigNap = Convert.ToUInt16(Form1.gBdNap, 16);
                        Byte tempConfigUap = Convert.ToByte(Form1.gBdUap, 16);
                        UInt32 tempConfigLapStart = Convert.ToUInt32(Form1.gBdLapStart, 16);
                        UInt32 tempConfigLapEnd = Convert.ToUInt32(Form1.gBdLapEnd, 16);

                        UInt16 tempNap = Convert.ToUInt16(xnap, 16);
                        Byte tempUap = Convert.ToByte(xuap, 16);
                        UInt32 tempLap = Convert.ToUInt32(xlap, 16);

                        if ((tempLap < tempConfigLapStart) || (tempLap > tempConfigLapEnd) || (tempNap != tempConfigNap) || (tempUap != tempConfigUap)) { flag_ng = 1; }
                    }
                    break;
                default:
                    {

                    }
                    break;
            }

            if (flag_ng == 1) { return false; }
            else { return true; }
        }

        private bool validateSerial(string serial)
        {
            int flag_ng = 0;

            // convert
            int tempConfigSerialStart = Convert.ToInt32(Form1.gSerialStart);
            int tempConfigSerialEnd = Convert.ToInt32(Form1.gSerialEnd);

            int tempSerial = Convert.ToInt32(serial);

            if ((tempSerial < tempConfigSerialStart) || (tempSerial > tempConfigSerialEnd)) { flag_ng = 1; }

            if (flag_ng == 1) { return false; }
            else { return true; }
        }

        private bool checkName(string name)
        {
            if (name == Form1.gDutName) { return true; }
            else { return false; }
        }

        private bool checkSwVer(string swVersion)
        {
            if (swVersion == Form1.gSwVersion) { return true; }
            else { return false; }
        }

        private bool procTestSequence(string seqName, int index)
        {
            bool retVal = false;

            if (seqName == "TEST_CONNECT") { retVal = procTestOpenPort(index); }
            else if (seqName == "CHECK_BD_DEFAULT_FOR_CHECK") { retVal = procTestCheckBdDefaultForCheck(index); }
            else if (seqName == "CHECK_BD_RANGE") { retVal = procTestCheckBdRange(index); }
            else if (seqName == "CHECK_MODEL_NAME") { retVal = procTestCheckModelName(index); }
            else if (seqName == "CHECK_FW_VERSION") { retVal = procTestCheckFwVersion(index); }
            else if (seqName == "CHECK_BD_DUPE") { retVal = procTestCheckDupe(index); }
            else if (seqName == "CHECK_FACTORY_RESET") { retVal = procTestCheckFactoryReset(index); }
            else if (seqName == "CHECK_BATTERY_LEVEL") { retVal = procTestCheckHsBatteryLevel(index); }
            else if (seqName == "WRITE_SERIAL_RESULT") { retVal = procTestWriteSerial(index); }
            else if (seqName == "CHECK_SERIAL_RESULT") { retVal = procTestCheckSerial(index); }
            else if (seqName == "CHECK_REGION") { retVal = procTestCheckRegion(index); }
            else if (seqName == "CHECK_COLOR") { retVal = procTestCheckColor(index); }
            else if (seqName == "WRITE_COLOR") { retVal = procTestWriteColor(index); }
            else if (seqName == "WRITE_REGION") { retVal = procTestWriteRegion(index); }
            else if (seqName == "WRITE_DEVICE_MODE") { retVal = procTestWriteDeviceMode(index); }
            else if (seqName == "CHECK_DEVICE_MODE") { retVal = procTestCheckDeviceMode(index); }
            else if (seqName == "CHECK_REGION_VP") { retVal = procTestCheckRegionVP(index); }
            return retVal;
        }

        private bool funcPowerOff()
        {
            int flag_ng = 0;

            try
            {
                Form1.dev.StartAsyncRead();

                byte[] data = new byte[50];
                byte[] cmdData = new byte[] { 0x05, 0x5A, 0x03, 0x00, 0x11, 0x11, 0x01 };

                data[0] = 0x02;
                data[1] = 0x0b;
                data[2] = Form1.USB_HOST_DATA_CMD_SEND_AIROHA_RACE_CMD;
                data[3] = (byte)Buffer.ByteLength(cmdData);
                Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));

                Form1.dev.Write(data);
            }

            catch
            {
                flag_ng = 1;
            }
            finally
            {

            }

            if (flag_ng == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool procTestCheckDupe(int index) // check bd dupe
        {
            int flag_ng = 0;

            try
            {
                // set test value (nothing)
                mainForm.gTestValue[index] = "-";

                // 01. bd check (default bd for write)
            }
            catch
            {
                flag_ng = 1;
            }
            finally
            {
                if (mainForm.findDupeData(Form1.dutFullBdAddress, Form1.dutSerial)) { flag_ng = 1; }
            }

            if (flag_ng == 1)
            {
                mainForm.gTestValue[index] = String.Format("{0}", Form1.dutFullBdAddress);
                mainForm.gNgType[index] = "BD Address Dupe";
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool procTestOpenPort(int index) // check connection
        {

            int flag_ng = 0;
            int flag_temp = 0;
            int Count = 20;

            if (mainForm.findSNDupeData(Form1.gSerialInput))
            {
                flag_temp = 1;
                MessageBox.Show("Dupe SN");

            }

            if (flag_temp != 1)
            {
                try
                {
                    Form1.dev.StartAsyncRead();

                    byte[] data = new byte[19];

                    data[0] = 0x02;
                    data[1] = 0x0b;
                    data[2] = 0x00;

                    Form1.dev.Write(data);

                    while (Count > 0)
                    {
                        mainForm.ShowProgressIndicator(index, true);

                        Form1.dev.StartAsyncRead();
                        Thread.Sleep(1000);
                        Count--;

                        if (Form1.HeadsetConnected)
                        {
                            Console.WriteLine("Headset Connected");
                            break;
                        }
                    }
                }

                catch
                {
                    flag_ng = 1;
                }
                finally
                {
                    if (Count == 0 || !Form1.HeadsetConnected)
                    {
                        flag_ng = 1;
                    }
                }

                mainForm.ShowProgressIndicator(index, false);

                if (flag_ng == 1)
                {
                    mainForm.gNgType[index] = "Connection Fail";
                    mainForm.gTestValue[index] = "(Not Connected)";
                    return false;
                }
                else
                {
                    mainForm.gTestValue[index] = String.Format("( {0} )", Form1.usbdbg_name);
                    return true;
                }
            }

            if (flag_temp == 1)
            {
                mainForm.gNgType[index] = "SN Dupe : " + Form1.gSerialInput;
                return false;
            }
            else
            {
                return true;
            }

        }


        private bool procTestCheckBdDefaultForCheck(int index) // check default bd (check)
        {
            int flag_ng = 0;
            int Count = 20;

            try
            {
                Form1.dev.StartAsyncRead();

                byte[] data = new byte[19];

                data[0] = 0x02;
                data[1] = 0x0b;
                data[2] = Form1.USB_HOST_DATA_CMD_GET_REMOTE_BD_ADDRESS;

                Form1.dev.Write(data);

                while (Count > 0)
                {
                    mainForm.ShowProgressIndicator(index, true);

                    Form1.dev.StartAsyncRead();
                    Thread.Sleep(1000);
                    Count--;

                    if (Form1.CheckBdAddress)
                    {
                        Console.WriteLine("CheckBdAddress");
                        break;
                    }
                }
            }
            catch
            {
                flag_ng = 1;
            }
            finally
            {
                if (Count == 0 || !Form1.CheckBdAddress)
                {
                    flag_ng = 1;
                }

                if (flag_ng != 1)
                {
                    tempNap = Form1.dutBdNap;
                    tempUap = Form1.dutBdUap;
                    tempLap = Form1.dutBdLap;

                    // set test value (nothing)
                    mainForm.gTestValue[index] = Form1.dutFullBdAddress;

                    if (!validateBd(tempNap, tempUap, tempLap, "check_default_for_check")) { flag_ng = 1; }
                }
            }

            mainForm.ShowProgressIndicator(index, false);

            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "Check Default BD(check) Fail";
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool procTestCheckBdRange(int index) // check bd range (against config)
        {
            int flag_ng = 0;

            try
            {
                // split bd string
                tempNap = Form1.dutBdNap;
                tempUap = Form1.dutBdUap;
                tempLap = Form1.dutBdLap;

                // set test value (nothing)
                mainForm.gTestValue[index] = Form1.dutFullBdAddress;
            }
            catch
            {
                flag_ng = 1;
            }
            finally
            {
                if (!validateBd(tempNap, tempUap, tempLap, "check_range")) { flag_ng = 1; }
            }

            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "Check BD Range Fail";
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool procTestCheckModelName(int index) // check model name
        {
            int flag_ng = 0;
            int Count = 20;

            try
            {
                Form1.dev.StartAsyncRead();

                byte[] data = new byte[19];

                data[0] = 0x02;
                data[1] = 0x0b;
                data[2] = Form1.USB_HOST_DATA_CMD_GET_REMOTE_NAME;

                Form1.dev.Write(data);

                while (Count > 0)
                {
                    mainForm.ShowProgressIndicator(index, true);

                    Form1.dev.StartAsyncRead();
                    Thread.Sleep(1000);
                    Count--;

                    if (Form1.CheckModelName)
                    {
                        Console.WriteLine("CheckModelName");
                        break;
                    }
                }
            }

            catch
            {
                flag_ng = 1;
            }
            finally
            {
                if (Count == 0 || !Form1.CheckModelName)
                {
                    flag_ng = 1;
                }

                if (flag_ng != 1)
                {
                    mainForm.gTestValue[index] = Form1.dutModelName;
                    if (!checkName(Form1.dutModelName)) { flag_ng = 1; }
                }
            }

            mainForm.ShowProgressIndicator(index, false);

            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "Model name fail!";
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool procTestCheckHsBatteryLevel(int index)
        {
            int flag_ng = 0;
            int Count = 20;

            if (flag_ng != 1)
            {
                try
                {
                    Form1.requestBattery = true;

                    Form1.dev.StartAsyncRead();

                    byte[] data = new byte[50];
                    byte[] cmdData = new byte[] { 0x05, 0x5A, 0x03, 0x00, 0xD6, 0x0C, 0x00 };

                    data[0] = 0x02;
                    data[1] = 0x0b;
                    data[2] = Form1.USB_HOST_DATA_CMD_SEND_AIROHA_RACE_CMD;
                    data[3] = (byte)Buffer.ByteLength(cmdData);
                    Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));

                    Form1.dev.Write(data);

                    while (Count > 0)
                    {
                        mainForm.ShowProgressIndicator(index, true);

                        Form1.dev.StartAsyncRead();
                        Thread.Sleep(200);
                        Count--;

                        if (Form1.CheckHsBatteryLevel)
                        {
                            Console.WriteLine("CheckHsBatteryLevel");
                            break;
                        }
                    }
                }

                catch
                {
                    flag_ng = 1;
                }
                finally
                {
                    if (Count == 0 || !Form1.CheckHsBatteryLevel)
                    {
                        flag_ng = 1;
                    }

                    if (flag_ng != 1)
                    {
                        mainForm.ShowProgressIndicator(index, false);

                        mainForm.gTestValue[index] = String.Format("{0}%", Form1.dutBatteryLevel);
                    }
                }
            }

            if (Form1.gBatteryLevelLow > Form1.dutBatteryLevel) { flag_ng = 1; }
            if (Form1.gBatteryLevelHigh < Form1.dutBatteryLevel) { flag_ng = 1; }

            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "Battery Level fail!";
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool procTestCheckFactoryReset(int index)
        {
            int flag_ng = 0;
            int Count = 20;

            if (flag_ng != 1)
            {
                Form1.CheckFactoryReset = false;

                try
                {
                    Form1.requestFactoryReset = true;

                    Form1.dev.StartAsyncRead();

                    byte[] data = new byte[50];
                    byte[] cmdData = new byte[] { 0x05, 0x5A, 0x04, 0x00, 0x01, 0x11, 0x95, 0x00 };

                    data[0] = 0x02;
                    data[1] = 0x0b;
                    data[2] = Form1.USB_HOST_DATA_CMD_SEND_AIROHA_RACE_CMD;
                    data[3] = (byte)Buffer.ByteLength(cmdData);
                    Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));

                    Form1.dev.Write(data);

                    Count = 50;
                    while (Count > 0)
                    {
                        mainForm.ShowProgressIndicator(index, true);

                        Form1.dev.StartAsyncRead();
                        Thread.Sleep(300);
                        Count--;

                        if (Form1.CheckFactoryReset)
                        {
                            Console.WriteLine("CheckFactoryReset");
                            break;
                        }

                        if (Form1.tryAgain)
                        {
                            Console.WriteLine("CheckFactoryReset / tryAgain");
                            break;
                        }
                    }
                }

                catch
                {
                    flag_ng = 1;
                }
                finally
                {
                    if (Count == 0 || !Form1.CheckFactoryReset)
                    {
                        flag_ng = 1;
                        mainForm.gTestValue[index] = "NG";
                    }

                    if (flag_ng != 1)
                    {
                        mainForm.gTestValue[index] = "OK(USER MODE)";
                    }
                }
            }

            mainForm.ShowProgressIndicator(index, false);

            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "Check factory reset fail!";
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool procTestWriteSerial(int index)
        {
            int flag_ng = 0;
            int Count = 20;

            byte[] lSerial = new byte[7];
            int cnt = 0;
            foreach (char character in Form1.gSerialInput)
            {
                byte x = (byte)Char.GetNumericValue(character);
                lSerial[cnt] = x;
                cnt++;
            }

            string strSerial1 = lSerial[0].ToString();
            string strSerial2 = lSerial[1].ToString();
            string strSerial3 = lSerial[2].ToString();
            string strSerial4 = lSerial[3].ToString();
            string strSerial5 = lSerial[4].ToString();
            string strSerial6 = lSerial[5].ToString();
            string strSerial7 = lSerial[6].ToString();
            byte[] arraySerial1 = System.Text.Encoding.ASCII.GetBytes(strSerial1);
            byte[] arraySerial2 = System.Text.Encoding.ASCII.GetBytes(strSerial2);
            byte[] arraySerial3 = System.Text.Encoding.ASCII.GetBytes(strSerial3);
            byte[] arraySerial4 = System.Text.Encoding.ASCII.GetBytes(strSerial4);
            byte[] arraySerial5 = System.Text.Encoding.ASCII.GetBytes(strSerial5);
            byte[] arraySerial6 = System.Text.Encoding.ASCII.GetBytes(strSerial6);
            byte[] arraySerial7 = System.Text.Encoding.ASCII.GetBytes(strSerial7);
            lSerial[0] = arraySerial1[0];
            lSerial[1] = arraySerial2[0];
            lSerial[2] = arraySerial3[0];
            lSerial[3] = arraySerial4[0];
            lSerial[4] = arraySerial5[0];
            lSerial[5] = arraySerial6[0];
            lSerial[6] = arraySerial7[0];

            if (flag_ng != 1)
            {
                try
                {
                    Form1.requestSerialWrite = true;

                    Form1.dev.StartAsyncRead();

                    byte[] data = new byte[100];
                    byte[] cmdData = new byte[] { 0x05, 0x5A, 0x0B, 0x00, 0x01, 0x0A, 0x00, 0xF4, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

                    cmdData[8] = lSerial[0];
                    cmdData[9] = lSerial[1];
                    cmdData[10] = lSerial[2];
                    cmdData[11] = lSerial[3];
                    cmdData[12] = lSerial[4];
                    cmdData[13] = lSerial[5];
                    cmdData[14] = lSerial[6];

                    data[0] = 0x02;
                    data[1] = 0x0b;
                    data[2] = Form1.USB_HOST_DATA_CMD_SEND_AIROHA_RACE_CMD;
                    data[3] = (byte)Buffer.ByteLength(cmdData);
                    Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));

                    Form1.dev.Write(data);

                    while (Count > 0)
                    {
                        mainForm.ShowProgressIndicator(index, true);

                        Form1.dev.StartAsyncRead();
                        Thread.Sleep(300);
                        Count--;

                        if (Form1.WriteSerial)
                        {
                            Console.WriteLine("WriteSerial");
                            break;
                        }
                    }
                }

                catch
                {
                    flag_ng = 1;
                }
                finally
                {
                    if (Count == 0 || !Form1.WriteSerial)
                    {
                        flag_ng = 1;
                    }

                    if (flag_ng != 1)
                    {
                        mainForm.gTestValue[index] = "OK";
                    }
                }
            }

            mainForm.ShowProgressIndicator(index, false);

            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "Write Serial fail!";
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool procTestCheckSerial(int index)
        {
            int flag_ng = 0;
            int Count = 20;

            if (flag_ng != 1)
            {
                try
                {
                    Form1.requestSerialRead = true;

                    Form1.dev.StartAsyncRead();

                    byte[] data = new byte[50];
                    byte[] cmdData = new byte[] { 0x05, 0x5A, 0x06, 0x00, 0x00, 0x0A, 0x00, 0xF4, 0x07, 0x00 };

                    data[0] = 0x02;
                    data[1] = 0x0b;
                    data[2] = Form1.USB_HOST_DATA_CMD_SEND_AIROHA_RACE_CMD;
                    data[3] = (byte)Buffer.ByteLength(cmdData);
                    Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));

                    Form1.dev.Write(data);

                    while (Count > 0)
                    {
                        mainForm.ShowProgressIndicator(index, true);

                        Form1.dev.StartAsyncRead();
                        Thread.Sleep(300);
                        Count--;

                        if (Form1.CheckSerial)
                        {
                            Console.WriteLine("CheckSerial");
                            break;
                        }

                        if (Form1.tryAgain)
                        {
                            Console.WriteLine("CheckSerial / tryAgain");
                            break;
                        }
                    }
                }

                catch
                {
                    flag_ng = 1;
                }
                finally
                {
                    if (Count == 0 || !Form1.CheckSerial)
                    {
                        flag_ng = 1;
                    }

                    if (flag_ng != 1)
                    {
                        tempSerial = Form1.dutSerial;

                        // set test value (nothing)
                        mainForm.gTestValue[index] = Form1.dutSerial;

                        if (!validateSerial(tempSerial)) { flag_ng = 1; }

                    }
                }
            }

            mainForm.ShowProgressIndicator(index, false);

            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "Check Serial fail!";
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool procTestWriteRegion(int index)
        {
            int flag_ng = 0;
            int Count = 20;

            byte IRegion = 0x00;

            switch (Form1.gFwRegion)
            {
                case "UC":
                    IRegion = 0x01;
                    break;
                case "J":
                    IRegion = 0x02;
                    break;
                case "CE7":
                    IRegion = 0x03;
                    break;
                case "E":
                    IRegion = 0x04;
                    break;
                case "IN":
                    IRegion = 0x05;
                    break;
                case "CN":
                    IRegion = 0x06;
                    break;
                default:
                    break;
            }
            if (IRegion == 0x00) { flag_ng = 1; }

            if (flag_ng != 1)
            {
                try
                {
                    Form1.requestWriteRegion = true;

                    Form1.dev.StartAsyncRead();

                    byte[] data = new byte[100];
                    byte[] cmdData = new byte[] { 0x05, 0x5A, 0x05, 0x00, 0x01, 0x0A, 0x03, 0xF4, 0x00 };
                    cmdData[8] = IRegion;

                    data[0] = 0x02;
                    data[1] = 0x0b;
                    data[2] = Form1.USB_HOST_DATA_CMD_SEND_AIROHA_RACE_CMD;
                    data[3] = (byte)Buffer.ByteLength(cmdData);
                    Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));

                    Form1.dev.Write(data);

                    while (Count > 0)
                    {
                        mainForm.ShowProgressIndicator(index, true);

                        Form1.dev.StartAsyncRead();
                        Thread.Sleep(300);
                        Count--;

                        if (Form1.WriteRegion)
                        {
                            Console.WriteLine("WriteRegion");
                            break;
                        }
                    }
                }

                catch
                {
                    flag_ng = 1;
                }
                finally
                {
                    if (Count == 0 || !Form1.WriteRegion)
                    {
                        flag_ng = 1;
                    }

                    if (flag_ng != 1)
                    {
                        mainForm.gTestValue[index] = "OK";
                    }
                }
            }

            mainForm.ShowProgressIndicator(index, false);

            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "Write Region fail!";
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool procTestCheckRegion(int index)
        {
            int flag_ng = 0;
            int Count = 20;

            if (flag_ng != 1)
            {
                try
                {
                    Form1.requestCheckRegion = true;

                    Form1.dev.StartAsyncRead();

                    byte[] data = new byte[50];
                    byte[] cmdData = new byte[] { 0x05, 0x5A, 0x06, 0x00, 0x00, 0x0A, 0x03, 0xF4, 0x01, 0x00 };

                    data[0] = 0x02;
                    data[1] = 0x0b;
                    data[2] = Form1.USB_HOST_DATA_CMD_SEND_AIROHA_RACE_CMD;
                    data[3] = (byte)Buffer.ByteLength(cmdData);
                    Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));

                    Form1.dev.Write(data);

                    while (Count > 0)
                    {
                        mainForm.ShowProgressIndicator(index, true);

                        Form1.dev.StartAsyncRead();
                        Thread.Sleep(300);
                        Count--;

                        if (Form1.CheckRegion)
                        {
                            Console.WriteLine("CheckRegion");
                            break;
                        }

                        if (Form1.tryAgain)
                        {
                            Console.WriteLine("CheckRegion / tryAgain");
                            break;
                        }
                    }
                }

                catch
                {
                    flag_ng = 1;
                }
                finally
                {
                    if (Count == 0 || !Form1.CheckRegion)
                    {
                        flag_ng = 1;
                    }

                    if (flag_ng != 1)
                    {
                        //
                    }
                }
            }

            mainForm.ShowProgressIndicator(index, false);

#if (AFTER_AWS_PAIR)
            if ((Form1.gFwRegion != Form1.dutRegion))
            {
                flag_ng = 1;
            }

            mainForm.gTestValue[index] = String.Format("Region({0})", Form1.dutRegion);
#endif // #if (AFTER_AWS_PAIR)
            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "Check Region fail!";
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool procTestWriteColor(int index)
        {
            int flag_ng = 0;
            int Count = 20;

            byte lColor = 0x00;

            switch (Form1.gFwColor)
            {
                case "BLACK":
                    lColor = 0x01;
                    break;
                //case "BLUE":
                //    lColor = 0x02;
                //    break;
                //case "BEIGE":
                //    lColor = 0x03;
                //    break;
                //case "WHITE":
                //    lColor = 0x04;
                //    break;
                //case "PINK":
                //    lColor = 0x05;
                //    break;
                //case "YELLOW":
                //    lColor = 0x06;
                //    break;
                default:
                    break;
            }

            if (lColor == 0x00) { flag_ng = 1; }

            if (flag_ng != 1)
            {
                try
                {
                    Form1.requestWriteColor = true;

                    Form1.dev.StartAsyncRead();

                    byte[] data = new byte[100];
                    byte[] cmdData = new byte[] { 0x05, 0x5A, 0x05, 0x00, 0x01, 0x0A, 0x01, 0xF4, 0x00 };
                    cmdData[8] = lColor;

                    data[0] = 0x02;
                    data[1] = 0x0b;
                    data[2] = Form1.USB_HOST_DATA_CMD_SEND_AIROHA_RACE_CMD;
                    data[3] = (byte)Buffer.ByteLength(cmdData);
                    Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));

                    Form1.dev.Write(data);

                    while (Count > 0)
                    {
                        mainForm.ShowProgressIndicator(index, true);

                        Form1.dev.StartAsyncRead();
                        Thread.Sleep(300);
                        Count--;

                        if (Form1.WriteColor)
                        {
                            Console.WriteLine("WriteColor");
                            break;
                        }
                    }
                }

                catch
                {
                    flag_ng = 1;
                }
                finally
                {
                    if (Count == 0 || !Form1.WriteColor)
                    {
                        flag_ng = 1;
                    }

                    if (flag_ng != 1)
                    {
                        mainForm.gTestValue[index] = "OK";
                    }
                }
            }

            mainForm.ShowProgressIndicator(index, false);

            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "Write Color fail!";
                return false;
            }
            else
            {
                return true;
            }
        }


        private bool procTestCheckColor(int index)
        {
            int flag_ng = 0;
            int Count = 20;

            if (flag_ng != 1)
            {
                try
                {
                    Form1.requestColor = true;

                    Form1.dev.StartAsyncRead();

                    byte[] data = new byte[50];
                    byte[] cmdData = new byte[] { 0x05, 0x5A, 0x06, 0x00, 0x00, 0x0A, 0x01, 0xF4, 0x01, 0x00 };

                    data[0] = 0x02;
                    data[1] = 0x0b;
                    data[2] = Form1.USB_HOST_DATA_CMD_SEND_AIROHA_RACE_CMD;
                    data[3] = (byte)Buffer.ByteLength(cmdData);
                    Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));

                    Form1.dev.Write(data);

                    while (Count > 0)
                    {
                        mainForm.ShowProgressIndicator(index, true);

                        Form1.dev.StartAsyncRead();
                        Thread.Sleep(300);
                        Count--;

                        if (Form1.CheckColor)
                        {
                            Console.WriteLine("CheckColor");
                            break;
                        }

                        if (Form1.tryAgain)
                        {
                            Console.WriteLine("CheckColor /  / tryAgain");
                            break;
                        }
                    }
                }

                catch
                {
                    flag_ng = 1;
                }
                finally
                {
                    if (Count == 0 || !Form1.CheckColor)
                    {
                        flag_ng = 1;
                    }

                    if (flag_ng != 1)
                    {
                        //
                    }
                }
            }

            mainForm.ShowProgressIndicator(index, false);

            if (Form1.gFwColor != Form1.dutColorCode)
            {
                flag_ng = 1;
            }

            mainForm.gTestValue[index] = String.Format("Color({0})", Form1.dutColorCode);

            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "Check color fail!";
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool procTestCheckRegionVP(int index)
        {
            int flag_ng = 0;
            int Count = 20;

            Form1.requestCheckRegionVP = true;

            if (flag_ng != 1)
            {
                try
                {
                    Form1.dev.StartAsyncRead();

                    byte[] data = new byte[50];
                    byte[] cmdData = new byte[] { 0x05, 0x5A, 0x02, 0x00, 0xC2, 0x01 };

                    data[0] = 0x02;
                    data[1] = 0x0b;
                    data[2] = Form1.USB_HOST_DATA_CMD_SEND_AIROHA_RACE_CMD;
                    data[3] = (byte)Buffer.ByteLength(cmdData);
                    Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));

                    Form1.dev.Write(data);

                    while (Count > 0)
                    {
                        mainForm.ShowProgressIndicator(index, true);

                        Form1.dev.StartAsyncRead();
                        Thread.Sleep(300);
                        Count--;

                        if (Form1.CheckRegionVP)
                        {
                            if (Form1.gFwRegion == "UC") Form1.tempFwRegion = "EN";
                            else if (Form1.gFwRegion == "CE7") Form1.tempFwRegion = "EN";
                            else if (Form1.gFwRegion == "E") Form1.tempFwRegion = "EN";
                            else if (Form1.gFwRegion == "IN") Form1.tempFwRegion = "EN";
                            else if (Form1.gFwRegion == "J") Form1.tempFwRegion = "J";
                            else if (Form1.gFwRegion == "CN") Form1.tempFwRegion = "CN";
                            else Form1.tempFwRegion = "UNKNOWN";

                            mainForm.gTestValue[index] = String.Format("{0}", Form1.dutRegionVP);

                            Console.WriteLine("CheckRegionVP_");
                            break;
                        }

                    }
                }

                catch
                {
                    flag_ng = 1;
                }
                finally
                {
                    if (Form1.tempFwRegion != Form1.dutRegionVP)
                    {
                        flag_ng = 1;
                    }
                }
            }

            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "Check Region and VP fail!";
                return false;
            }
            else
            {
                return true;
            }
        }


        private bool procTestWriteDeviceMode(int index)
        {
            int flag_ng = 0;
            int Count = 20;

            byte lMode = 0x00;

            switch (Form1.gFwMode)
            {
                case "USER":
                    lMode = 0x00;
                    break;
                case "FACTORY":
                    lMode = 0x01;
                    break;
                default:
                    break;
            }

            if (flag_ng != 1)
            {
                try
                {
                    Form1.requestWriteDeviceMode = true;

                    Form1.dev.StartAsyncRead();

                    byte[] data = new byte[100];
                    byte[] cmdData = new byte[] { 0x05, 0x5A, 0x05, 0x00, 0x01, 0x0A, 0x00, 0xF4,
                                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                                                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                    cmdData[8] = lMode;

                    data[0] = 0x02;
                    data[1] = 0x0b;
                    data[2] = Form1.USB_HOST_DATA_CMD_SEND_AIROHA_RACE_CMD;
                    data[3] = (byte)Buffer.ByteLength(cmdData);
                    Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));

                    Form1.dev.Write(data);

                    while (Count > 0)
                    {
                        mainForm.ShowProgressIndicator(index, true);

                        Form1.dev.StartAsyncRead();
                        Thread.Sleep(300);
                        Count--;

                        if (Form1.WriteDeviceMode)
                        {
                            Console.WriteLine("WriteDeviceMode");
                            break;
                        }

                        if (Form1.tryAgain)
                        {
                            Console.WriteLine("WriteDeviceMode /  / tryAgain");
                            break;
                        }
                    }
                }
                catch
                {
                    flag_ng = 1;
                }
                finally
                {
                    if (Count == 0 || !Form1.WriteDeviceMode)
                    {
                        flag_ng = 1;
                    }

                    if (flag_ng != 1)
                    {
                        //
                    }
                }
            }

            mainForm.ShowProgressIndicator(index, false);

            mainForm.gTestValue[index] = Form1.Write_DeviceMode ? "OK" : "NG";

            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "Write device mode fail!";
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool procTestCheckDeviceMode(int index)
        {
            int flag_ng = 0;
            int Count = 20;

            Form1.CheckDeviceMode = false;

            Form1.requestDeviceMode = true;

            if (flag_ng != 1)
            {
                try
                {
                    Form1.dev.StartAsyncRead();

                    byte[] data = new byte[50];
                    byte[] cmdData = new byte[] { 0x05, 0x5A, 0x06, 0x00, 0x00, 0x0A, 0x00, 0xF4, 0xE8, 0x03 };

                    data[0] = 0x02;
                    data[1] = 0x0b;
                    data[2] = Form1.USB_HOST_DATA_CMD_SEND_AIROHA_RACE_CMD;
                    data[3] = (byte)Buffer.ByteLength(cmdData);
                    Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));

                    Form1.dev.Write(data);

                    while (Count > 0)
                    {
                        mainForm.ShowProgressIndicator(index, true);

                        Form1.dev.StartAsyncRead();
                        Thread.Sleep(300);
                        Count--;

                        if (Form1.CheckDeviceMode)
                        {
                            Console.WriteLine("CheckDeviceMode");
                            break;
                        }

                        if (Form1.tryAgain)
                        {
                            Console.WriteLine("CheckDeviceMode /  / tryAgain");
                            break;
                        }
                    }
                }

                catch
                {
                    flag_ng = 1;
                }
                finally
                {
                    if (Count == 0 || !Form1.CheckDeviceMode)
                    {
                        flag_ng = 1;
                    }

                    if (flag_ng != 1)
                    {
                        //
                    }
                }
            }

            mainForm.ShowProgressIndicator(index, false);

            mainForm.gTestValue[index] = Form1.dutDeviceMode ? "OK" + "(" + Form1.gFwMode + ")" : "NG";

            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "Check device mode fail!";
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool procTestCheckFwVersion(int index) // check fw version
        {
            int flag_ng = 0;
            int Count = 20;

            if (flag_ng != 1)
            {
                try
                {
                    Form1.requestFWversion = true;

                    Form1.dev.StartAsyncRead();

                    byte[] data = new byte[50];
                    byte[] cmdData = new byte[] { 0x05, 0x5A, 0x03, 0x00, 0x07, 0x1C, 0x00 };

                    data[0] = 0x02;
                    data[1] = 0x0b;
                    data[2] = Form1.USB_HOST_DATA_CMD_SEND_AIROHA_RACE_CMD;
                    data[3] = (byte)Buffer.ByteLength(cmdData);
                    Buffer.BlockCopy(cmdData, 0, data, 4, Buffer.ByteLength(cmdData));

                    Form1.dev.Write(data);

                    while (Count > 0)
                    {
                        mainForm.ShowProgressIndicator(index, true);

                        Form1.dev.StartAsyncRead();
                        Thread.Sleep(300);
                        Count--;

                        if (Form1.CheckFwVersion)
                        {
                            Console.WriteLine("CheckFWVersion");
                            break;
                        }
                    }
                }
                catch
                {
                    flag_ng = 1;
                }
                finally
                {
                    if (Count == 0 || !Form1.CheckFwVersion)
                    {
                        flag_ng = 1;
                    }

                    if (flag_ng != 1)
                    {
                        //
                    }
                }
            }

            if (flag_ng != 1)
            {

                mainForm.gTestValue[index] = String.Format("{0}", Form1.dutFullVersion);
                if (!checkSwVer(Form1.dutFullVersion))
                { flag_ng = 1; }

            }

            mainForm.ShowProgressIndicator(index, false);

            if (flag_ng == 1)
            {
                mainForm.gNgType[index] = "FW Version fail!";
                return false;
            }
            else
            {
                return true;
            }
        }

        private void openpassworddlg()
        {
            passwordbox password = new passwordbox();
            password.ShowDialog();

        }

    }


    public class TcpAppClient : TcpClient
    {
        private bool Initialized = false;

        public event EventHandler<TcpAppClientEventArgs> CommandSend;
        public event EventHandler<TcpAppClientEventArgs> ResponseReceived;

        public List<string> Commands { get; private set; } = new List<string>();

        private TcpAppCommandResult ExecuteTcpAppCommand(string command, int timeout = 1000)
        {
            TcpAppCommandResult result = new TcpAppCommandResult();
            try
            {
                string tcpCommand = command + "\r";
                CommandSend?.Invoke(this, new TcpAppClientEventArgs(command));
                FlushInputBuffer();
                Write(tcpCommand);

                DateTime startTime = DateTime.Now;
                while ((DateTime.Now - startTime).TotalMilliseconds < timeout)
                {
                    string response = ReadString();
                    ResponseReceived?.Invoke(this, new TcpAppClientEventArgs(response));
                    if (!string.IsNullOrEmpty(response))
                    {
                        string[] resultParams = response.Split(' ');

                        //BUSY and Queue statue handle by application
                        result.Status = (TcpAppCommandStatus)Enum.Parse(typeof(TcpAppCommandStatus), resultParams[0]);
                        if (resultParams.Length > 1) result.ReturnMessage = string.Join(" ", resultParams.Skip(1)).Trim(); //Remove trailing CRLF
                        return result;
                    }
                    Thread.Sleep(100); //Wait 100ms, retry.
                }//while
                throw new TcpAppClientException("TIMEOUT: No response received from server!");
            }
            catch (TcpAppClientException) { throw; }
            catch (Exception ex)
            {
                throw new TcpAppClientException("Exception raised!", ex);
            }
            finally
            {
                SuspendDataReceivedEvent = false;
            }
        }

        public TcpAppCommandResult ExecuteCommand(string command, int timeout = 1000)
        {
            if (!Initialized) throw new TcpAppClientException("TcpApp not initialized, execute Connect() first!");
            if (!Connected) Connect();
            return ExecuteTcpAppCommand(command, timeout);
        }


    }

    public class TcpClient : IDisposable
    {
        private System.Net.Sockets.TcpClient Client = new System.Net.Sockets.TcpClient();
        private NetworkStream TcpStream;
        private byte[] FixedBuffer;
        private int BufferSize;
        private bool ConnectState = false;
        private readonly object LockHandler = new object();

        private bool MonitoringThreadActive = true;
        private Thread IncomingDataMonitoring = null;
        private Thread ConnectionMonitoring = null;

        public bool SuspendDataReceivedEvent { get; set; } = false;

        public event EventHandler<TcpDataReceivedEventArgs> DataReceived;
        public event EventHandler ConnectionStatusChanged;
        public string HostName { get; set; }
        public int Port { get; set; }


        public bool Connected
        {
            get
            {
                bool state = Client.IsConnected();
                if (state == ConnectState) return ConnectState;
                ConnectState = state;
                if (!ConnectState) TerminateThreadsAndTCPStream();
                ConnectionStatusChanged?.Invoke(this, null);
                return state;
            }

            private set
            {
                if (value == ConnectState) return;
                ConnectState = value;
                ConnectionStatusChanged?.Invoke(this, null);
            }
        }

        private bool ConnectionAccepted { get; set; }

        public virtual void Connect()
        {
            ConnectToServer();
            ConnectionAccepted = true;
        }

        private void ConnectToServer()
        {
            try
            {
                DisconnectFromServer();
                Client.Connect(HostName, Port);
                Thread.Sleep(100);
                if (Client.IsConnected())
                {
                    TcpStream = Client.GetStream();
                    Connected = true;
                    BufferSize = Client.ReceiveBufferSize;
                    FixedBuffer = new byte[BufferSize];

                    MonitoringThreadActive = true;
                    IncomingDataMonitoring = new Thread(MonitorIncomingData);
                    IncomingDataMonitoring.Name = "TCP Client Data Monitoring @ " + Port.ToString();
                    IncomingDataMonitoring.Start();

                    ConnectionMonitoring = new Thread(MonitorConnection);
                    ConnectionMonitoring.Name = "TCP Client Connection Monitoring @ " + Port.ToString();
                    ConnectionMonitoring.Start();
                    ConnectionStatusChanged?.Invoke(this, null);
                }
                else throw new TcpClientException("Connection rejected by server!");
            }
            catch (Exception ex)
            {

            }

        }

        private void Reconnect()
        {
            if (!ConnectionAccepted) throw new TcpClientException("Connection to server not established!");
            ConnectToServer();
        }

        public virtual void Disconnect()
        {
            ConnectionAccepted = false;
            DisconnectFromServer();
        }

        private void DisconnectFromServer()
        {
            TerminateThreadsAndTCPStream();
            Connected = false;
            Client.Close();

            Client = new System.Net.Sockets.TcpClient();
        }

        private void TerminateThreadsAndTCPStream()
        {
            MonitoringThreadActive = false;
            IncomingDataMonitoring = null;
            ConnectionMonitoring = null;

            TcpStream?.Close();
            TcpStream = null;
        }

        public void Write(string message)
        {
            if (!Connected) Reconnect();
            byte[] outputBuffer = Encoding.ASCII.GetBytes(message);
            Write(outputBuffer);
        }

        public void Write(byte[] dataBytes)
        {
            if (dataBytes == null) throw new ArgumentNullException(nameof(dataBytes));

            //if (!Connected) Reconnect();

            if (Connected)
            {
                TcpStream.Write(dataBytes, 0, dataBytes.Length);
                TcpStream.Flush();
            }
            else
            {
                MessageBox.Show("Check IPv4 address");
            }

        }

        public void FlushInputBuffer()
        {
            while (TcpStream.DataAvailable) { TcpStream.Read(FixedBuffer, 0, BufferSize); }
        }

        public byte[] ReadBytes()
        {
            lock (LockHandler)
            {
                if (!Connected) Reconnect();
                return ReadRawBytes();
            }
        }

        private byte[] ReadRawBytes()
        {
            TcpStream.ReadTimeout = 1000;
            List<byte> ByteBuffer = new List<byte>();
            while (true)
            {
                try
                {
                    int readByte = TcpStream.Read(FixedBuffer, 0, BufferSize);
                    if (readByte == 0) break;
                    else if (readByte == BufferSize)
                    {
                        ByteBuffer.AddRange(FixedBuffer);
                    }
                    else
                    {
                        byte[] data = new byte[readByte];
                        Array.Copy(FixedBuffer, data, readByte);
                        ByteBuffer.AddRange(data);
                    }

                    if (!TcpStream.DataAvailable) break;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("WARNING: Exception raise from readRawByte(): " + ex);
                    return ByteBuffer.ToArray(); //Empty Array
                }
            }
            return ByteBuffer.ToArray();
        }

        public string ReadString()
        {
            if (!Connected) Reconnect();
            return ASCIIEncoding.ASCII.GetString(ReadBytes());
        }

        private void MonitorIncomingData()
        {
            EventHandler<TcpDataReceivedEventArgs> DataReceivedHandler;
            while (MonitoringThreadActive) //Loop forever
            {
                DataReceivedHandler = DataReceived;
                if (DataReceivedHandler != null)
                {
                    bool dataAvailable = TcpStream.DataAvailable;
                    if (dataAvailable)
                    {
                        lock (LockHandler)
                        {
                            byte[] data = ReadRawBytes();
                            if (data.Length > 0) DataReceivedHandler.Invoke(this, new TcpDataReceivedEventArgs() { Data = data });
                        }
                    }
                }
                Thread.Sleep(50);
            }
        }

        private void MonitorConnection()
        {
            while (MonitoringThreadActive)
            {
                if (ConnectState)
                {
                    if (!Connected) ConnectionStatusChanged?.Invoke(this, null);
                }
                Thread.Sleep(50);
            }
        }

        #region [ IDisposable Support ]
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    TerminateThreadsAndTCPStream();
                    Client.Close();
                    Client = null;
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

    [Serializable]
    public class TcpClientException : Exception
    {
        public TcpClientException(string message) : base(message) { }

    }

    public class TcpDataReceivedEventArgs : EventArgs
    {
        public byte[] Data { get; internal set; }

        public string GetString() { return Encoding.ASCII.GetString(Data); }
    }
    public class TcpAppClientEventArgs : EventArgs
    {
        public TcpAppClientEventArgs(string message) { Message = message; }

        public string Message { get; private set; }
    }

    public class TcpAppClientException : Exception
    {
        public TcpAppClientException(string message) : base(message) { }
        public TcpAppClientException(string message, Exception innerException) : base(message, innerException) { }

    }

    public class TcpAppCommand : ICloneable
    {

        private readonly List<TcpAppParameter> Params = new List<TcpAppParameter>();
        public IList<TcpAppParameter> Parameters { get { return Params.AsReadOnly(); } }

        public void AddParameter(TcpAppParameter parameter)
        {
            if (parameter == null) throw new ArgumentNullException(nameof(parameter));

            //Sanity Check
            if (Params.FirstOrDefault(x => x.Name.Equals(parameter.Name, StringComparison.InvariantCultureIgnoreCase)) != null)
            {
                //Duplicated parameter, throw exception.
                throw new ArgumentException("Unable to add parameter " + parameter.Name + ", alredy exist!");
            }
            Params.Add(parameter);
        }

        public object Clone()
        {
            TcpAppCommand result = new TcpAppCommand();

            foreach (TcpAppParameter p in Parameters) result.AddParameter(p);
            return result;
        }
    }

    public class TcpAppCommandResult
    {
        public string ReturnMessage { get; set; }

        public TcpAppCommandStatus Status { get; set; } = TcpAppCommandStatus.ERR;

    }

    public enum TcpAppCommandStatus
    {
        ERR = -1,
        OK = 0,
        QUEUED = 1,
        BUSY = 2
    };

    public class TcpAppParameter
    {
        public string Name { get; private set; }

    }

}
