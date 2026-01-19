/* thermistor control MCU board IO
 * control command structure
 * data[0] = 0x04;
 * data[1] = 0xFF;
 * data[2] = 0x04; => control byte
 * data[3] = 0xF7;
 * data[4] = 0xFE;
 * A3 - high temperature simulation (low value resistor) / control byte value : 0x00
 * A4 - normal temperature simulation (normal value resistor) / control byte value : 0x02
 * A5 - low temperature simulation (high value resistor) / control byte value : 0x04
 * A6 - unassigned / control byte value : 0x06
 */


#define USE_UART
#define USE_AUTOTHMTEST

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using NationalInstruments.Visa;
using FTD2XX_NET; // wsjung.add.170517 : FT234XD gpio test
using System.IO;
using System.IO.Ports;

namespace AirohaChargingTest
{
    public partial class Form1 : Form
    {
        public MessageBasedSession mbSession;
        public string lastResourceString = null;

        string pathConfigFile;

        public string gVbatLowVoltage;
        public string gVbatHighVoltage;
        public string gVbatHighVoltageLimit; // wsjung.add.170805
        public string gVbatCurrentLimit;
        public string gChargerVoltage;
        public string gChargerCurrentLimit;
        public string gCurrentLvVbatLimitUpper;
        public string gCurrentLvVbatLimitLower;
        public string gCurrentLvChargerLimitUpper;
        public string gCurrentLvChargerLimitLower;
        public string gCurrentHvVbatLimitUpper;
        public string gCurrentHvVbatLimitLower;
        public string gCurrentHvChargerLimitUpper;
        public string gCurrentHvChargerLimitLower;
        public string gCurrentTHLimitLower;
        public string gCurrentTHLimitUpper;
        public string gCurrentTNLimitLower;
        public string gCurrentTNLimitUpper;
        public string gCurrentTLLimitLower;
        public string gCurrentTLLimitUpper;


        static public string gComPort;
        static public string gGpibAddress;

        static public bool gEnableVbusCalibration;

        public double gValCurrentLvVbatLimitUpper;
        public double gValCurrentLvVbatLimitLower;
        public double gValCurrentLvChargerLimitUpper;
        public double gValCurrentLvChargerLimitLower;
        public double gValCurrentHvVbatLimitUpper;
        public double gValCurrentHvVbatLimitLower;
        public double gValCurrentHvChargerLimitUpper;
        public double gValCurrentHvChargerLimitLower;
        public double gValCurrentTHLimitLower;
        public double gValCurrentTHLimitUpper;
        public double gValCurrentTNLimitLower;
        public double gValCurrentTNLimitUpper;
        public double gValCurrentTLLimitLower;
        public double gValCurrentTLLimitUpper;

        public string gSleepCurrentLimitLower;
        public string gSleepCurrentLimitUpper;

        public double gValSleepCurrentLimitLower;
        public double gValSleepCurrentLimitUpper;

        public int gCalStep = 0;

        // measured value
        public double gMeasValCcVbat = 0.000;
        public double gMeasValCcVbus = 0.000;
        public double gMeasValCvVbat = 0.000;
        public double gMeasValCvVbus = 0.000;
        public double gMeasResultThmHigh = 0.000;
        public double gMeasResultThmNormal = 0.000;
        public double gMeasResultThmLow = 0.000;
        public double gMeasValueSleepCurrent = 0.000;

        public string gCalValue = "";

        public string gModelName = ""; // wsjung.add.170808


        public string pathLogFile;

        //public double 

        public string readString = "";

        public Form1()
        {
            InitializeComponent();
        }

#if (USE_UART)
        private SerialPort _Port;
        private SerialPort Port
        {
            get
            {
                if (_Port == null)
                {
                    _Port = new SerialPort();
                    _Port.PortName = "COM47";
                    _Port.BaudRate = 115200;
                    _Port.DataBits = 8;
                    _Port.Parity = Parity.None;
                    _Port.Handshake = Handshake.None;
                    _Port.StopBits = StopBits.One;
                    _Port.DataReceived += Port_DataReceived;
                }
                return _Port;
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(200);

            byte[] bArray = new byte[50];

            int xbb = Port.BytesToRead;
            //MessageBox.Show(xbb.ToString());

            string msg = "";
            for (int i = 0; i < xbb; i++)
            {
                bArray[i] = Convert.ToByte(Port.ReadByte());
                msg += String.Format("0x{0:X2} ", bArray[i]);
            }
        }

        private Boolean IsOpen
        {
            get { return Port.IsOpen; }
            set
            {
                if (value)
                {
                    //
                }
                else
                {
                    //
                }
            }
        }

        private bool openPort()
        {
            bool flag_ng = false;

            if (!Port.IsOpen)
            {
                Port.PortName = gComPort;

                try
                {
                    // 연결
                    Port.Open();
                }
                catch // (Exception ex)
                {
                    flag_ng = true;
                }
            }
            else
            {
                // 현재 시리얼이 연결 상태이면 연결 해제
                Port.Close();
            }

            // 상태 변경
            IsOpen = Port.IsOpen;

            if (flag_ng) { return false; }
            else { return true; }
        }

        private void sendByteMsg(byte[] data)
        {
            // 보낼 메시지가 없으면 종료
            String text = gComPort;
            if (String.IsNullOrEmpty(text)) return;

            try
            {
                // 메시지 전송
                //Port.WriteLine(text);
                Port.Write(data, 0, data.Length);

                string sendData = "";
                for (int i = 0; i < data.Length; i++)
                {
                    sendData += String.Format("0x{0:X2} ", data[i]);
                }
            }
            catch //(Exception ex)
            {
                //
            }
        }
#endif // (USE_UART)

        private void Form1_Load(object sender, EventArgs e)
        {
            // load config
            loadConfig();

#if (USE_UART)
            if (!openPort())
            {
                MessageBox.Show("Check UART connection!!! - NG");
                Close();
            }
#endif // #if (USE_UART)

            checkLogFile();


            this.Text = gModelName + " PBA Charging Test";

            // check battery simulator
            if (checkSimulator())
            {
                // set some display(limit)
                lb_lv_current_vbat_lower.Text = gCurrentLvVbatLimitLower;
                lb_lv_current_vbat_upper.Text = gCurrentLvVbatLimitUpper;
                lb_lv_current_vbus_lower.Text = gCurrentLvChargerLimitLower;
                lb_lv_current_vbus_upper.Text = gCurrentLvChargerLimitUpper;

                lb_hv_current_vbat_lower.Text = gCurrentHvVbatLimitLower;
                lb_hv_current_vbat_upper.Text = gCurrentHvVbatLimitUpper;
                lb_hv_current_vbus_lower.Text = gCurrentHvChargerLimitLower;
                lb_hv_current_vbus_upper.Text = gCurrentHvChargerLimitUpper;

                lb_sleep_current_lower.Text = gSleepCurrentLimitLower;
                lb_sleep_current_upper.Text = gSleepCurrentLimitUpper;


                lb_hightemp_lower.Text = gCurrentTHLimitLower;
                lb_hightemp_upper.Text = gCurrentTHLimitUpper;
                lb_normaltemp_lower.Text = gCurrentTNLimitLower;
                lb_normaltemp_upper.Text = gCurrentTNLimitUpper;
                lb_lowtemp_lower.Text = gCurrentTLLimitLower;
                lb_lowtemp_upper.Text = gCurrentTLLimitUpper;


                // convert limit text to double value
                gValCurrentLvVbatLimitLower = Convert.ToDouble(gCurrentLvVbatLimitLower);
                gValCurrentLvVbatLimitUpper = Convert.ToDouble(gCurrentLvVbatLimitUpper);

                gValCurrentLvChargerLimitLower = Convert.ToDouble(gCurrentLvChargerLimitLower);
                gValCurrentLvChargerLimitUpper = Convert.ToDouble(gCurrentLvChargerLimitUpper);

                gValCurrentHvVbatLimitLower = Convert.ToDouble(gCurrentHvVbatLimitLower);
                gValCurrentHvVbatLimitUpper = Convert.ToDouble(gCurrentHvVbatLimitUpper);

                gValCurrentHvChargerLimitLower = Convert.ToDouble(gCurrentHvChargerLimitLower);
                gValCurrentHvChargerLimitUpper = Convert.ToDouble(gCurrentHvChargerLimitUpper);

                gValCurrentTHLimitLower = Convert.ToDouble(gCurrentTHLimitLower);
                gValCurrentTHLimitUpper = Convert.ToDouble(gCurrentTHLimitUpper);
                gValCurrentTNLimitLower = Convert.ToDouble(gCurrentTNLimitLower);
                gValCurrentTNLimitUpper = Convert.ToDouble(gCurrentTNLimitUpper);
                gValCurrentTLLimitLower = Convert.ToDouble(gCurrentTLLimitLower);
                gValCurrentTLLimitUpper = Convert.ToDouble(gCurrentTLLimitUpper);

                gValSleepCurrentLimitLower = Convert.ToDouble(gSleepCurrentLimitLower);
                gValSleepCurrentLimitUpper = Convert.ToDouble(gSleepCurrentLimitUpper);

                Thread.Sleep(500);

                // set basic parameter
                gpib_write("*rst");
                Thread.Sleep(200);

                gpib_write("DISP:CHAN 1");
                gpib_write("VOLT 3.700");
                gpib_write("CURR 550e-3");
                gpib_write("outp off");
                gpib_write("DISP:CHAN 2");
                gpib_write("SOUR2:VOLT 5.000");
                gpib_write("SOUR2:CURR 550e-3");
                gpib_write("outp2 off");

            }
            else
            {
                //btnStart.Enabled = false;
                Close();
            }

        }

        private void loadConfig()
        {
            // using ini style config
            pathConfigFile = System.IO.Directory.GetCurrentDirectory() + "\\" + "config\\" + "config.ini";
            string tempString;

            IniReadWrite IniReader = new IniReadWrite();

            tempString = IniReader.IniReadValue("CONFIG", "VBAT_LOW_VOLTAGE", pathConfigFile);
            gVbatLowVoltage = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "VBAT_HIGH_VOLTAGE", pathConfigFile);
            gVbatHighVoltage = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "VBAT_HIGH_VOLTAGE_LIMIT", pathConfigFile);
            gVbatHighVoltageLimit = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "VBAT_CURRENT_LIMIT", pathConfigFile);
            gVbatCurrentLimit = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CHARGER_VOLTAGE", pathConfigFile);
            gChargerVoltage = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CHARGER_CURRENT_LIMIT", pathConfigFile);
            gChargerCurrentLimit = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_SLEEP_LIMIT_LOWER", pathConfigFile);
            gSleepCurrentLimitLower = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_SLEEP_LIMIT_UPPER", pathConfigFile);
            gSleepCurrentLimitUpper = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_LV_VBAT_LIMIT_UPPER", pathConfigFile);
            gCurrentLvVbatLimitUpper = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_LV_VBAT_LIMIT_LOWER", pathConfigFile);
            gCurrentLvVbatLimitLower = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_LV_CHARGER_LIMIT_UPPER", pathConfigFile);
            gCurrentLvChargerLimitUpper = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_LV_CHARGER_LIMIT_LOWER", pathConfigFile);
            gCurrentLvChargerLimitLower = tempString;


            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_HV_VBAT_LIMIT_UPPER", pathConfigFile);
            gCurrentHvVbatLimitUpper = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_HV_VBAT_LIMIT_LOWER", pathConfigFile);
            gCurrentHvVbatLimitLower = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_HV_CHARGER_LIMIT_UPPER", pathConfigFile);
            gCurrentHvChargerLimitUpper = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_HV_CHARGER_LIMIT_LOWER", pathConfigFile);
            gCurrentHvChargerLimitLower = tempString;


            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_TH_LIMIT_LOWER", pathConfigFile);
            gCurrentTHLimitLower = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_TH_LIMIT_UPPER", pathConfigFile);
            gCurrentTHLimitUpper = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_TN_LIMIT_LOWER", pathConfigFile);
            gCurrentTNLimitLower = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_TN_LIMIT_UPPER", pathConfigFile);
            gCurrentTNLimitUpper = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_TL_LIMIT_LOWER", pathConfigFile);
            gCurrentTLLimitLower = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CURRENT_TL_LIMIT_UPPER", pathConfigFile);
            gCurrentTLLimitUpper = tempString;


            tempString = IniReader.IniReadValue("CONFIG", "PORT", pathConfigFile);
            gComPort = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "GPIB_ADDRESS", pathConfigFile);
            gGpibAddress = tempString;

            tempString = IniReader.IniReadValue("CONFIG", "CAL_STEP", pathConfigFile);
            gCalStep = Convert.ToInt32(tempString);

            tempString = IniReader.IniReadValue("CONFIG", "ENABLE_VBUS_CURRENT_CAL", pathConfigFile);
            if (tempString == "YES") { gEnableVbusCalibration = true; }
            else if (tempString == "NO") { gEnableVbusCalibration = false; }

            tempString = IniReader.IniReadValue("CONFIG", "MODEL_NAME", pathConfigFile); // wsjung.add.170808
            gModelName = tempString;
        }

        private void clearDisp()
        {
            tb_HvCvCurrentAtVbat.Text = "--";
            tb_HvCvCurrentAtVbus.Text = "--";
            tb_LvCcCurrentAtVbat.Text = "--";
            tb_LvCcCurrentAtVbus.Text = "--";

            tb_SleepCurrentAtVbat.Text = "--";

            tb_resultThmHigh.Text = "--";
            tb_resultThmLow.Text = "--";
            tb_resultThmNormal.Text = "--";

            tb_HvCvCurrentAtVbat.BackColor = Color.Empty;
            tb_HvCvCurrentAtVbus.BackColor = Color.Empty;
            tb_LvCcCurrentAtVbat.BackColor = Color.Empty;
            tb_LvCcCurrentAtVbus.BackColor = Color.Empty;

            tb_SleepCurrentAtVbat.BackColor = Color.Empty;

            tb_resultThmHigh.BackColor = Color.Empty;
            tb_resultThmLow.BackColor = Color.Empty;
            tb_resultThmNormal.BackColor = Color.Empty;

            lbResult.Text = "--";
            lbResult.ForeColor = Color.Empty;

            lbStatus.Text = "--";
            lbStatus.ForeColor = Color.Empty;


            gMeasValCcVbat = 0.000;
            gMeasValCcVbus = 0.000;
            gMeasValCvVbat = 0.000;
            gMeasValCvVbus = 0.000;
            gMeasResultThmHigh = 0.000;
            gMeasResultThmNormal = 0.000;
            gMeasResultThmLow = 0.000;

            gCalValue = "";
        }


        private void setResistorDefault()
        {
            thmMcuA4On();
        }

        private bool checkSimulator()
        {
            {
                int flag_ng = 0;

                //lastResourceString = sr.ResourceName;
                //Cursor.Current = Cursors.WaitCursor;

                using (var rmSession = new ResourceManager())
                {
                    //string abcd = "GPIB0::11::INSTR";
                    string abcd = "GPIB0::" + gGpibAddress + "::INSTR";
                    //MessageBox.Show(abcd);

                    try
                    {
                        mbSession = (MessageBasedSession)rmSession.Open(abcd);
                    }
                    catch (InvalidCastException)
                    {
                        flag_ng = 1;
                        MessageBox.Show("Resource selected must be a message-based session");
                    }
                    catch //(Exception exp)
                    {
                        flag_ng = 1;
                        //MessageBox.Show(exp.Message);
                    }
                    finally
                    {
                        //...
                    }

                    // read instrument information
                    if (flag_ng != 1)
                    {
                        readString = "";
                        gpib_query("*IDN?");
                        if (readString == "")
                        {
                            flag_ng = 1;
                        }
                    }

                    if (flag_ng == 1)
                    {
                        MessageBox.Show("Battery simulator was not found - NG");
                        return false;
                    }
                    else
                    {
                        MessageBox.Show("Battery simulator was found - OK");
                        return true;
                    }
                }
            }
        }

        private bool gpib_query(string arg)
        {
            int flag_ng = 0;
            //string readValue = "";

            try
            {
                mbSession.RawIO.Write(arg);
                readString = InsertCommonEscapeSequences(mbSession.RawIO.ReadString());
            }
            catch (Exception exp)
            {
                flag_ng = 1;
                MessageBox.Show(exp.Message);
            }
            finally
            {
                //Cursor.Current = Cursors.Default;
            }

            if (flag_ng == 1)
            {
                return false;
            }
            else
            {
                //MessageBox.Show(readString);
                return true;
            }
        }

        private bool gpib_write(string arg)
        {
            int flag_ng = 0;

            try
            {
                mbSession.RawIO.Write(arg);
            }
            catch (Exception exp)
            {
                flag_ng = 1;
                MessageBox.Show(exp.Message);
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


        private string InsertCommonEscapeSequences(string s)
        {
            return s.Replace("\n", "\\n").Replace("\r", "\\r");
        }

        private string ReplaceCommonEscapeSequences(string s)
        {
            return s.Replace("\\n", "\n").Replace("\\r", "\r");
        }


        #region simulator_control_functions
        public void setSimulatorCh1Off()
        {
            gpib_write("DISP:CHAN 1");
            gpib_write("outp off");
        }

        public void setSimulatorCh1On()
        {
            gpib_write("DISP:CHAN 1");
            gpib_write("outp on");
        }

        public void setSimulatorCh2Off()
        {
            gpib_write("DISP:CHAN 2");
            gpib_write("outp2 off");
        }

        public void setSimulatorCh2On()
        {
            gpib_write("DISP:CHAN 2");
            gpib_write("outp2 on");
        }

        public void setSimulatorCh1CurrentRangeMax()
        {
            gpib_write("SENS:CURR:RANG MAX");
        }
        public void setSimulatorCh1CurrentRangeMin()
        {
            gpib_write("SENS:CURR:RANG MIN");
        }

        public void setSimulatorCh1Voltage(string ch1Voltage)
        {
            string temp = "VOLT " + ch1Voltage;
            gpib_write("DISP:CHAN 1");
            gpib_write(temp);
        }

        public void setSimulatorCh2Voltage(string ch2Voltage)
        {
            string temp = "SOUR2:VOLT " + ch2Voltage;
            gpib_write("DISP:CHAN 2");
            gpib_write(temp);
        }

        public void setSimulatorCh1Current(string ch1CurrentLimit)
        {
            string temp = "CURR " + ch1CurrentLimit;
            gpib_write("DISP:CHAN 1");
            gpib_write(temp);
        }

        public void setSimulatorCh2Current(string ch2CurrentLimit)
        {
            string temp = "SOUR2:CURR " + ch2CurrentLimit;
            gpib_write("DISP:CHAN 2");
            gpib_write(temp);
        }

        public double getSimulatorCh1Current()
        {
            double sum = 0.000;

            gpib_write("DISP:CHAN 1");
            gpib_write("SENS:FUNC 'CURR'");
            Thread.Sleep(200);

            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(500);

                //gpib_write("SENS:FUNC 'CURR'");
                gpib_query("READ?");

                readString = readString.Replace("\\n", string.Empty);
                sum += double.Parse(readString);
            }

            return (double)(sum / 3);
        }

        public double getSimulatorCh2Current()
        {
            double sum = 0.000;

            gpib_write("DISP:CHAN 2");
            gpib_write("SENS2:FUNC 'CURR'");
            Thread.Sleep(200);

            for (int i = 0; i < 3; i++)
            {
                Thread.Sleep(500);

                //gpib_write("SENS2:FUNC 'CURR'");
                gpib_query("READ2?");

                readString = readString.Replace("\\n", string.Empty);
                sum += double.Parse(readString);
            }

            return (double)(sum / 3);
        }

        public void setSimulatorCh1ToReadCurr()
        {
            gpib_write("DISP:CHAN 1");
            gpib_write("SENS:FUNC 'CURR'");
            Thread.Sleep(500);
        }

        public double getSimulatorCh1CurrentOneTime()
        {
            double sum = 0.000;

            gpib_query("READ?");

            readString = readString.Replace("\\n", string.Empty);
            sum += double.Parse(readString);

            return sum;
        }
        #endregion

        #region delegates for update UI
        delegate void updateStatusDelegate(string str);
        public void updateStatus(string str)
        {
            if (InvokeRequired)
            {
                updateStatusDelegate showStatusDel = new updateStatusDelegate(updateStatus);
                Invoke(showStatusDel, str);
            }
            else
            {
                lbStatus.Text = str;
            }
        }

        delegate void updateThmStatusDelegate(double measValue, int sel, bool result);
        public void updateThmStatus(double measValue, int sel, bool result)
        {
            if (InvokeRequired)
            {
                updateThmStatusDelegate showThmStatusDel = new updateThmStatusDelegate(updateThmStatus);
                Invoke(showThmStatusDel, measValue, sel, result);
            }
            else
            {
                switch (sel)
                {
                    case 1:
                        tb_resultThmHigh.Text = measValue.ToString("F4");
                        if (result == true) { tb_resultThmHigh.BackColor = Color.SkyBlue; }
                        else { tb_resultThmHigh.BackColor = Color.Red; }

                        break;
                    case 2:
                        tb_resultThmNormal.Text = measValue.ToString("F4");
                        if (result == true) { tb_resultThmNormal.BackColor = Color.SkyBlue; }
                        else { tb_resultThmNormal.BackColor = Color.Red; }
                        break;
                    case 3:
                        tb_resultThmLow.Text = measValue.ToString("F4");
                        if (result == true) { tb_resultThmLow.BackColor = Color.SkyBlue; }
                        else { tb_resultThmLow.BackColor = Color.Red; }
                        break;
                    case 4:
                        if (result == true)
                        {
                            writeLog(gCalValue, gMeasValueSleepCurrent, gMeasValCcVbat, gMeasValCcVbus, gMeasValCvVbat, gMeasValCvVbus, gMeasResultThmHigh, gMeasResultThmNormal, gMeasResultThmLow, "PASS");
                            lbResult.Text = "PASS";
                            lbResult.ForeColor = Color.Blue;
                        }
                        else
                        {
                            writeLog(gCalValue, gMeasValueSleepCurrent, gMeasValCcVbat, gMeasValCcVbus, gMeasValCvVbat, gMeasValCvVbus, gMeasResultThmHigh, gMeasResultThmNormal, gMeasResultThmLow, "FAIL");
                            lbResult.Text = "FAIL";
                            lbResult.ForeColor = Color.Red;
                        }
                        break;
                }
            }
        }

        delegate void showCcCurrentResultDelegate(double measValue, int type, bool result);
        public void showCcCurrentResult(double measValue, int type, bool result)
        {
            if (InvokeRequired)
            {
                showCcCurrentResultDelegate resultCcCurrentDel = new showCcCurrentResultDelegate(showCcCurrentResult);
                Invoke(resultCcCurrentDel, measValue, type, result);
            }
            else
            {
                switch (type)
                {
                    case 0: // Low voltage, Vbat Current
                        //tb_LvCcCurrentAtVbat.Text = Convert.ToString(measValue);
                        tb_LvCcCurrentAtVbat.Text = measValue.ToString("F4");
                        if (result == false) { tb_LvCcCurrentAtVbat.BackColor = Color.Red; }
                        else { tb_LvCcCurrentAtVbat.BackColor = Color.SkyBlue; }
                        break;
                    case 1: // Low voltage, Vbus Current
                            //tb_LvCcCurrentAtVbus.Text = Convert.ToString(measValue);
                        tb_LvCcCurrentAtVbus.Text = measValue.ToString("F4");
                        if (result == false) { tb_LvCcCurrentAtVbus.BackColor = Color.Red; }
                        else { tb_LvCcCurrentAtVbus.BackColor = Color.SkyBlue; }
                        break;
                    case 2: // High voltage, Vbat Current
                            //tb_HvCvCurrentAtVbat.Text = Convert.ToString(measValue);
                        tb_HvCvCurrentAtVbat.Text = measValue.ToString("F4");
                        if (result == false) { tb_HvCvCurrentAtVbat.BackColor = Color.Red; }
                        else { tb_HvCvCurrentAtVbat.BackColor = Color.SkyBlue; }
                        break;
                    case 3: // High voltage, Vbus Current
                        //tb_HvCvCurrentAtVbus.Text = Convert.ToString(measValue);
                        tb_HvCvCurrentAtVbus.Text = measValue.ToString("F4");
                        if (result == false) { tb_HvCvCurrentAtVbus.BackColor = Color.Red; }
                        else { tb_HvCvCurrentAtVbus.BackColor = Color.SkyBlue; }
                        break;
                    case 4:
                        tb_SleepCurrentAtVbat.Text = measValue.ToString("F2");
                        if (result == false) { tb_SleepCurrentAtVbat.BackColor = Color.Red; }
                        else { tb_SleepCurrentAtVbat.BackColor = Color.SkyBlue; }
                        break;
                }

                //tb_LvCcCurrentAtVbat.Text = Convert.ToString(measValue);
            }
        }

        delegate void toggleBtnDelegate(Boolean x);
        public void toggleBtn(Boolean x)
        {
            if (InvokeRequired)
            {
                toggleBtnDelegate btnDel = new toggleBtnDelegate(toggleBtn);
                Invoke(btnDel, x);
            }
            else
            {
                if (x == true) { btnStart.Enabled = true; btnStart.Focus(); }
                else { btnStart.Enabled = false; btnStart.Focus(); }
            }
        }
        #endregion

        private void btnStart_Click(object sender, EventArgs e)
        {
            clearDisp();

            thmMcuAllOff();
            Thread.Sleep(100);

            thmMcuA4On();

            backWork bg = new backWork(this);
            Thread workerThread = new Thread(bg.DoWork2);
            workerThread.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (mbSession != null)
            {
                mbSession.Dispose();
            }
        }

        private bool checkLogFile()
        {
            DateTime today = DateTime.Now;
            //string strDatePrefix = string.Format("{0:YY-MM-DD}", today);
            string strDatePrefix = today.ToString("yy-MM-dd");
            string strLogFile = strDatePrefix + "_" + gModelName + "_Charging" + "-log.csv";

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

                return true;
            }
            else
            {
                //make new log file
                StreamWriter sw = new StreamWriter(pathLogFile, true, Encoding.Unicode);

                // write basic data (index)
                sw.WriteLine("date\t" + "sleep current\t" + "cc vbat\t" + "cc vbus\t" + "cv vbat\t" + "cv vbus\t" + "thm high\t" + "thm normal\t" + "thm low\t" + "result");
                sw.Close();

                return false;
            }
        }

        private void writeLog(string day, double sleepCurrent, double ccVbat, double ccVbus, double cvVbat, double cvVbus, double thmHigh, double thmNormal, double thmLow, string result)
        {
            string date = DateTime.Now.ToString("yy-MM-dd");

            string strCcVbat = ccVbat.ToString("F4");
            string strCcVbus = ccVbus.ToString("F4");
            string strCvVbat = cvVbat.ToString("F4");
            string strCvVbus = cvVbus.ToString("F4");
            string strSleepCurrent = sleepCurrent.ToString("F3");

            // open stream
            StreamWriter sw = new StreamWriter(pathLogFile, true, Encoding.Unicode);

            // write measured data
            sw.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t{9}", date, strSleepCurrent, strCcVbat, strCcVbus, strCvVbat, strCvVbus, thmHigh, thmNormal, thmLow, result);
            // close stream
            sw.Close();
        }

        public void thmMcuA3On() // high temperature simulation (low value resistor)
        {
            byte[] data = new byte[5];

            data[0] = 0x04;
            data[1] = 0xFF;
            data[2] = 0x00;
            data[3] = 0xF7;
            data[4] = 0xFE;

            sendByteMsg(data);
        }

        public void thmMcuA4On() // normal temperature simulation (normal value resistor)
        {
            byte[] data = new byte[5];

            data[0] = 0x04;
            data[1] = 0xFF;
            data[2] = 0x02;
            data[3] = 0xF7;
            data[4] = 0xFE;

            sendByteMsg(data);
        }

        public void thmMcuA5On() // low temperature simulation (high value resistor)
        {
            byte[] data = new byte[5];

            data[0] = 0x04;
            data[1] = 0xFF;
            data[2] = 0x04;
            data[3] = 0xF7;
            data[4] = 0xFE;

            sendByteMsg(data);
        }

        public void thmMcuA6On() // unsssigned
        {
            byte[] data = new byte[5];

            data[0] = 0x04;
            data[1] = 0xFF;
            data[2] = 0x06;
            data[3] = 0xF7;
            data[4] = 0xFE;

            sendByteMsg(data);
        }

        public void thmMcuAllOff()
        {
            byte[] data = new byte[5];

            data[0] = 0x04;
            data[1] = 0xFF;
            data[2] = 0x01;
            data[3] = 0xF7;
            data[4] = 0xFE;

            sendByteMsg(data);
        }

    }


    class backWork
    {
        Form1 mainForm;

        public backWork(Form1 frm)
        {
            mainForm = frm;
        }

        public void DoWork2()
        {
            mainForm.toggleBtn(false);
            Thread.Sleep(500);

            int flag_ng = 0;

            //string tempString = "";

            double measLvCcVbatCurrent = 0.000;
            double measLvCcVbusCurrent = 0.000;
            double measHvCvVbatCurrent = 0.000;
            double measHvCvVbusCurrent = 0.000;

            double measSleepCurrent = 0.000;

            if (flag_ng != 1)
            {
                // set simulator ch1 current range to max
                mainForm.setSimulatorCh1CurrentRangeMax();

                mainForm.setSimulatorCh1On();
                Thread.Sleep(1000);
                mainForm.setSimulatorCh2On();
                Thread.Sleep(3000);

                // set simulator ch1 current range to min (5mA)
                mainForm.setSimulatorCh1CurrentRangeMin();

                measSleepCurrent = mainForm.getSimulatorCh1Current();
                measSleepCurrent = measSleepCurrent * Math.Pow(10, 6);

                if ((measSleepCurrent < mainForm.gValSleepCurrentLimitUpper) && (measSleepCurrent > mainForm.gValSleepCurrentLimitLower))
                {
                    flag_ng = 0;

                    // update UI
                    mainForm.gMeasValueSleepCurrent = measSleepCurrent;
                    mainForm.showCcCurrentResult(measSleepCurrent, 4, true);
                }
                else
                {
                    if (measSleepCurrent > mainForm.gValSleepCurrentLimitUpper)
                    {
                        mainForm.setSimulatorCh1Off();
                        mainForm.setSimulatorCh1On();

                        while (measSleepCurrent > mainForm.gValSleepCurrentLimitUpper)
                        {
                            measSleepCurrent = mainForm.getSimulatorCh1Current();
                            measSleepCurrent = measSleepCurrent * Math.Pow(10, 6);

                            if (measSleepCurrent < mainForm.gValSleepCurrentLimitUpper)
                            {
                                break;
                            }
                        }

                        if ((measSleepCurrent < mainForm.gValSleepCurrentLimitUpper) && (measSleepCurrent > mainForm.gValSleepCurrentLimitLower))
                        {
                            flag_ng = 0;

                            // update UI
                            mainForm.gMeasValueSleepCurrent = measSleepCurrent;
                            mainForm.showCcCurrentResult(measSleepCurrent, 4, true);
                        }
                        else
                        {
                            flag_ng = 1;

                            // update UI
                            mainForm.gMeasValueSleepCurrent = measSleepCurrent;
                            mainForm.showCcCurrentResult(measSleepCurrent, 4, false);
                        }
                    }
                    else if (measSleepCurrent < mainForm.gValSleepCurrentLimitUpper)
                    {
                        mainForm.setSimulatorCh1Off();
                        mainForm.setSimulatorCh1On();

                        measSleepCurrent = mainForm.getSimulatorCh1Current();
                        measSleepCurrent = measSleepCurrent * Math.Pow(10, 6);

                        if ((measSleepCurrent < mainForm.gValSleepCurrentLimitUpper) && (measSleepCurrent > mainForm.gValSleepCurrentLimitLower))
                        {
                            flag_ng = 0;

                            // update UI
                            mainForm.gMeasValueSleepCurrent = measSleepCurrent;
                            mainForm.showCcCurrentResult(measSleepCurrent, 4, true);
                        }
                        else
                        {
                            flag_ng = 1;

                            // update UI
                            mainForm.gMeasValueSleepCurrent = measSleepCurrent;
                            mainForm.showCcCurrentResult(measSleepCurrent, 4, false);
                        }
                    }

                }
            }

            mainForm.setSimulatorCh2Off();
            Thread.Sleep(200);
            mainForm.setSimulatorCh1Off();
            Thread.Sleep(200);

            mainForm.setSimulatorCh1CurrentRangeMax();

            mainForm.setSimulatorCh1Voltage(mainForm.gVbatLowVoltage);
            //mainForm.setSimulatorCh1Current(mainForm.gVbatCurrentLimit);
            Thread.Sleep(1000);

            mainForm.setSimulatorCh1On();
            Thread.Sleep(1000);
            mainForm.setSimulatorCh2On();
            Thread.Sleep(3000);


            // 01. get vbus current in low voltage
            if (flag_ng != 1)
            {
                measLvCcVbusCurrent = mainForm.getSimulatorCh2Current();

                if (((measLvCcVbusCurrent < mainForm.gValCurrentLvChargerLimitUpper) && (measLvCcVbusCurrent > mainForm.gValCurrentLvChargerLimitLower)))
                {
                    flag_ng = 0;

                    // update UI
                    mainForm.gMeasValCcVbus = measLvCcVbusCurrent;
                    mainForm.showCcCurrentResult(measLvCcVbusCurrent, 1, true);
                }
                else
                {
                    flag_ng = 1;

                    // update UI
                    mainForm.gMeasValCcVbus = measLvCcVbusCurrent;
                    mainForm.showCcCurrentResult(measLvCcVbusCurrent, 1, false);
                }
            }

            // 02. get vbat current in low voltage
            if (flag_ng != 1)
            {
                measLvCcVbatCurrent = mainForm.getSimulatorCh1Current();

                if ((measLvCcVbatCurrent < mainForm.gValCurrentLvVbatLimitUpper) && (measLvCcVbatCurrent > mainForm.gValCurrentLvVbatLimitLower))
                {
                    flag_ng = 0;

                    // update UI
                    mainForm.gMeasValCcVbat = measLvCcVbatCurrent;
                    mainForm.showCcCurrentResult(measLvCcVbatCurrent, 0, true);
                }
                else
                {
                    flag_ng = 1;

                    // update UI
                    mainForm.gMeasValCcVbat = measLvCcVbatCurrent;
                    mainForm.showCcCurrentResult(measLvCcVbatCurrent, 0, false);
                }
            }

            // 03. change battery voltage to high
            if (flag_ng != 1)
            {
                mainForm.setSimulatorCh2Off();
                Thread.Sleep(200);
                mainForm.setSimulatorCh1Off();
                Thread.Sleep(200);

                mainForm.setSimulatorCh1Voltage(mainForm.gVbatHighVoltage);
                Thread.Sleep(500);

                // turn on again
                mainForm.setSimulatorCh1On();
                Thread.Sleep(1000);
                mainForm.setSimulatorCh2On();
                Thread.Sleep(3000);
            }

            // 04. get vbus current in high voltage
            if (flag_ng != 1)
            {
                measHvCvVbusCurrent = mainForm.getSimulatorCh2Current();

                if ((measHvCvVbusCurrent < mainForm.gValCurrentHvChargerLimitUpper) && (measHvCvVbusCurrent > mainForm.gValCurrentHvChargerLimitLower))
                {
                    flag_ng = 0;

                    // update UI
                    mainForm.gMeasValCvVbus = measHvCvVbusCurrent;
                    mainForm.showCcCurrentResult(measHvCvVbusCurrent, 3, true);
                }
                else
                {
                    // try adjust full vbat battery (more high) until reach vbat high limit (ex: 4.25V)
                    flag_ng = 1;

                    double vbatStart = Convert.ToDouble(mainForm.gVbatHighVoltage) + 0.01;
                    double vbatLimit = Convert.ToDouble(mainForm.gVbatHighVoltageLimit);

                    while (vbatStart <= vbatLimit)
                    {
                        mainForm.setSimulatorCh1Voltage(Convert.ToString(vbatStart)); // change vbat
                        Thread.Sleep(250);

                        measHvCvVbusCurrent = mainForm.getSimulatorCh2Current();

                        if ((measHvCvVbusCurrent < mainForm.gValCurrentHvChargerLimitUpper) && (measHvCvVbusCurrent > mainForm.gValCurrentHvChargerLimitLower))
                        {
                            flag_ng = 0;
                            break;
                        }

                        vbatStart = vbatStart + 0.01; // increase
                    }

                    if (flag_ng == 0)
                    {
                        // update UI
                        mainForm.gMeasValCvVbus = measHvCvVbusCurrent;
                        mainForm.showCcCurrentResult(measHvCvVbusCurrent, 3, true);
                    }
                    else
                    {
                        flag_ng = 1;

                        // update UI
                        mainForm.gMeasValCvVbus = measHvCvVbusCurrent;
                        mainForm.showCcCurrentResult(measHvCvVbusCurrent, 3, false);
                    }

                }
            }

            Thread.Sleep(2000);

            // 05. get vbat current in high voltage
            if (flag_ng != 1)
            {
                measHvCvVbatCurrent = mainForm.getSimulatorCh1Current();

                if ((measHvCvVbatCurrent < mainForm.gValCurrentHvVbatLimitUpper) && (measHvCvVbatCurrent > mainForm.gValCurrentHvVbatLimitLower))
                {
                    flag_ng = 0;

                    // update UI
                    mainForm.gMeasValCvVbat = measHvCvVbatCurrent;
                    mainForm.showCcCurrentResult(measHvCvVbatCurrent, 2, true);
                }
                else
                {
                    flag_ng = 1;

                    // update UI
                    mainForm.gMeasValCvVbat = measHvCvVbatCurrent;
                    mainForm.showCcCurrentResult(measHvCvVbatCurrent, 2, false);
                }
            }


            mainForm.setSimulatorCh1Off();
            Thread.Sleep(200);
            mainForm.setSimulatorCh2Off();
            Thread.Sleep(200);

            if (flag_ng != 1)
            {
                mainForm.setSimulatorCh1Voltage(mainForm.gVbatLowVoltage);

                // turn on again
                mainForm.setSimulatorCh1On();
                Thread.Sleep(1000);
                mainForm.setSimulatorCh2On();
                Thread.Sleep(2000);

                double thermistorCurrent = 0.000;
                int count = 10;
                int flag_pass_thm = 0;

                // set simulator to read ch1 current
                mainForm.setSimulatorCh1ToReadCurr();
                // 01. test high temperature cut-off
                mainForm.updateStatus(String.Format("High Temp(36KΩ): {0}", count.ToString()));

                // set resistor to High
                //mainForm.thmMcuAllOff();
                mainForm.thmMcuA3On();
                Thread.Sleep(1000);// 

                // testing loop : high temperature cut-off
                for (int i = 0; i < 10; i++)
                //for (int i = 0; i < 40; i++) 
                {
                    for (int j = 0; j < 2; j++)
                    {
                        thermistorCurrent = mainForm.getSimulatorCh1CurrentOneTime();

                        if ((thermistorCurrent < mainForm.gValCurrentTHLimitUpper) && (thermistorCurrent > mainForm.gValCurrentTHLimitLower)) // roll back
                        //if ((thermistorCurrent > (mainForm.gValCurrentHvVbatLimitUpper * -1.0)) && (thermistorCurrent < mainForm.gValCurrentHvVbatLimitLower)) 
                        {
                            flag_pass_thm = 1; break;
                        }

                        Thread.Sleep(500);
                        //Thread.Sleep(250); 
                    }

                    if (flag_pass_thm == 0)
                    {
                        // decrease count
                        count -= 1;
                        mainForm.updateStatus(String.Format("High Temp(36KΩ): {0}", count.ToString()));
                    }
                    else
                    {
                        break;
                    }
                }

                if (flag_pass_thm == 0)
                {
                    mainForm.gMeasResultThmHigh = thermistorCurrent;
                    flag_ng = 1;

                    mainForm.updateThmStatus(thermistorCurrent, 1, false);
                }
                else
                {
                    mainForm.gMeasResultThmHigh = thermistorCurrent;
                    mainForm.updateThmStatus(thermistorCurrent, 1, true);
                }

                //if (flag_ng != 1)
                //{
                //    // 02. test normal temperature
                //    count = 10;
                //    flag_pass_thm = 0;
                //    mainForm.updateStatus(String.Format("Normal Temp(100KΩ): {0}", count.ToString()));


                //    // set resistor to normal
                //    mainForm.thmMcuA4On();
                //    Thread.Sleep(1000);

                //    // turn off and turn on again
                //    mainForm.setSimulatorCh2Off();
                //    Thread.Sleep(200);
                //    mainForm.setSimulatorCh1Off();
                //    Thread.Sleep(200);

                //    //mainForm.setSimulatorCh2On();
                //    //Thread.Sleep(1000);// 3000
                //    mainForm.setSimulatorCh1On();
                //    Thread.Sleep(1000);

                //    // testing loop : Normal temperature cut-off
                //    for (int i = 0; i < 10; i++)
                //    {
                //        for (int j = 0; j < 2; j++)
                //        {
                //            thermistorCurrent = mainForm.getSimulatorCh1CurrentOneTime();

                //            if ((thermistorCurrent < mainForm.gValCurrentTNLimitUpper) && (thermistorCurrent > mainForm.gValCurrentTNLimitLower))
                //            {
                //                flag_pass_thm = 1; break;
                //            }

                //            Thread.Sleep(250);
                //        }

                //        if (flag_pass_thm == 0)
                //        {
                //            // decrease count
                //            count -= 1;
                //            mainForm.updateStatus(String.Format("Normal Temp(100KΩ): {0}", count.ToString()));
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }

                //    if (flag_pass_thm == 0)
                //    {
                //        mainForm.gMeasResultThmNormal = thermistorCurrent;
                //        flag_ng = 1; mainForm.updateThmStatus(thermistorCurrent, 2, false);
                //    }
                //    else
                //    {
                //        mainForm.gMeasResultThmNormal = thermistorCurrent;
                //        mainForm.updateThmStatus(thermistorCurrent, 2, true);
                //    }
                //}



                //if (flag_ng != 1)
                //{
                //    // 03. test low temperature cut-off
                //    count = 10;
                //    flag_pass_thm = 0;
                //    mainForm.updateStatus(String.Format("Low Temp(160KΩ): {0}", count.ToString()));


                //    // set resistor to low
                //    mainForm.thmMcuA5On();
                //    Thread.Sleep(1000); 


                //    // turn off and turn on again
                //    mainForm.setSimulatorCh2Off();
                //    Thread.Sleep(200);
                //    mainForm.setSimulatorCh1Off();
                //    Thread.Sleep(200);

                //    //mainForm.setSimulatorCh2On();
                //    //Thread.Sleep(1000);
                //    mainForm.setSimulatorCh1On();
                //    Thread.Sleep(2000);

                //    // testing loop : Low temperature cut-off
                //    for (int i = 0; i < 10; i++)
                //    {
                //        for (int j = 0; j < 2; j++)
                //        {
                //            thermistorCurrent = mainForm.getSimulatorCh1CurrentOneTime();

                //            if ((thermistorCurrent < mainForm.gValCurrentTLLimitUpper) && (thermistorCurrent > mainForm.gValCurrentTLLimitLower)) // roll back
                //            //if ((thermistorCurrent > (mainForm.gValCurrentHvVbatLimitUpper * -1.0)) && (thermistorCurrent < mainForm.gValCurrentHvVbatLimitLower)) 
                //            { flag_pass_thm = 1; break; }

                //            Thread.Sleep(250);
                //        }

                //        if (flag_pass_thm == 0)
                //        {
                //            // decrease count
                //            count -= 1;
                //            mainForm.updateStatus(String.Format("Low Temp(160KΩ): {0}", count.ToString()));
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }

                //    if (flag_pass_thm == 0)
                //    {
                //        mainForm.gMeasResultThmLow = thermistorCurrent;
                //        flag_ng = 1; mainForm.updateThmStatus(thermistorCurrent, 3, false);
                //    }
                //    else
                //    {
                //        mainForm.gMeasResultThmLow = thermistorCurrent;
                //        mainForm.updateThmStatus(thermistorCurrent, 3, true);
                //    }
                //}

            }

            mainForm.setSimulatorCh2Off();
            Thread.Sleep(200);
            mainForm.setSimulatorCh1Off();
            Thread.Sleep(200);

            double V = 0;

            if (flag_ng == 1)
            {
                // display result
                mainForm.updateThmStatus(V, 4, false);
            }
            else
            {
                mainForm.updateThmStatus(V, 4, true);
            }

            mainForm.toggleBtn(true);

        }

    }
}