using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace SerialCommunication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();
                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;

                comboBoxBaudrate.SelectedIndex = comboBoxBaudrate.Items.IndexOf("115200");
            }
            catch (Exception)
            { }
        }

        private void cboPoort_DropDown(object sender, EventArgs e)
        {
            try
            {
                string selected = (string)comboBoxPoort.SelectedItem;
                string[] portNames = SerialPort.GetPortNames().Distinct().ToArray();

                comboBoxPoort.Items.Clear();
                comboBoxPoort.Items.AddRange(portNames);

                comboBoxPoort.SelectedIndex = comboBoxPoort.Items.IndexOf(selected);
            }
            catch (Exception)
            {
                if (comboBoxPoort.Items.Count > 0) comboBoxPoort.SelectedIndex = 0;
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            // abc def ghi jkl

            try
            {
                if (serialPortArduino.IsOpen)
                {
                    //Ik heb een verbinding --> gebruiker wil deze verbreken

                    serialPortArduino.Close();

                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                    labelStatus.Text = "Status: Disconnected";
                }
                else
                {
                    //Ik heb geen verbinding --> gebruiker wilt een verbinding maken

                    serialPortArduino.PortName = (string)comboBoxPoort.SelectedItem;
                    serialPortArduino.BaudRate = Int32.Parse((string)comboBoxBaudrate.SelectedItem);
                    serialPortArduino.DataBits = (int)numericUpDownDatabits.Value;

                    if (radioButtonParityEven.Checked) serialPortArduino.Parity = Parity.Even;
                    else if (radioButtonParityOdd.Checked) serialPortArduino.Parity = Parity.Odd;
                    else if (radioButtonParityNone.Checked) serialPortArduino.Parity = Parity.None;
                    else if (radioButtonParityMark.Checked) serialPortArduino.Parity = Parity.Mark;
                    else if (radioButtonParitySpace.Checked) serialPortArduino.Parity = Parity.Space;

                    if (radioButtonStopbitsNone.Checked) serialPortArduino.StopBits = StopBits.None;
                    else if (radioButtonStopbitsOne.Checked) serialPortArduino.StopBits = StopBits.One;
                    else if (radioButtonStopbitsOnePointFive.Checked) serialPortArduino.StopBits = StopBits.OnePointFive;
                    else if (radioButtonStopbitsTwo.Checked) serialPortArduino.StopBits = StopBits.Two;

                    if (radioButtonHandshakeNone.Checked) serialPortArduino.Handshake = Handshake.None;
                    else if (radioButtonHandshakeRTS.Checked) serialPortArduino.Handshake = Handshake.RequestToSend;
                    else if (radioButtonHandshakeRTSXonXoff.Checked) serialPortArduino.Handshake = Handshake.RequestToSendXOnXOff;
                    else if (radioButtonHandshakeXonXoff.Checked) serialPortArduino.Handshake = Handshake.XOnXOff;

                    serialPortArduino.RtsEnable = checkBoxRtsEnable.Checked;
                    serialPortArduino.DtrEnable = checkBoxDtrEnable.Checked;

                    serialPortArduino.Open();
                    string commando = "ping";
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();

                    if (antwoord == "pong")
                    {
                        radioButtonVerbonden.Checked = true;
                        buttonConnect.Text = "Disconnect";
                        labelStatus.Text = "Status: Connected";
                    }
                    else
                    {
                        serialPortArduino.Close();
                        labelStatus.Text = "Error: verkeerd antwoord";
                    }

                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";

            }

        }

        private void checkBoxDigital2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {

                    string commando; //set d2 high/low
                    if (checkBoxDigital2.Checked) commando = "set d2 high";
                    else commando = "set d2 low";
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";

            }
        }

        private void checkBoxDigital3_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {

                    string commando; //set d3 high/low
                    if (checkBoxDigital3.Checked) commando = "set d3 high";
                    else commando = "set d3 low";
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";

            }
        }

        private void checkBoxDigital4_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {

                    string commando; //set d4 high/low
                    if (checkBoxDigital4.Checked) commando = "set d4 high";
                    else commando = "set d4 low";
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";

            }
        }

        private void trackBarPWM9_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando = "set pwm9 " + trackBarPWM9.Value.ToString();
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void trackBarPWM10_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando = "set pwm10 " + trackBarPWM10.Value.ToString();
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void trackBarPWM11_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    string commando = "set pwm11 " + trackBarPWM11.Value.ToString();
                    serialPortArduino.WriteLine(commando);
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }


        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerOefening3.Enabled = tabControl.SelectedIndex == 3;
            timerOefening4.Enabled = tabControl.SelectedIndex == 4;
            timerOefening5.Enabled = tabControl.SelectedIndex == 5;
        }

        private void timerOefening3_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    serialPortArduino.ReadExisting();

                    string commando = "get d5";
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);
                    radioButtonDigital5.Checked = (antwoord == "1");

                    commando = "get d6";
                    serialPortArduino.WriteLine(commando);
                    antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);
                    radioButtonDigital6.Checked = (antwoord == "1");

                    commando = "get d7";
                    serialPortArduino.WriteLine(commando);
                    antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);
                    radioButtonDigital7.Checked = (antwoord == "1");

                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void timerOefening4_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    serialPortArduino.ReadExisting();

                    string commando = "get a0";
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);

                    int value = Int32.Parse(antwoord);
                    labelAnalog0.Text = value.ToString();
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }

        private void timerOefening5_Tick(object sender, EventArgs e)
        {
            try
            {
                if (serialPortArduino.IsOpen)
                {
                    serialPortArduino.ReadExisting();

                    // Read pin 0: 0..1023 → 5..45 °C (Desired temperature)
                    string commando = "get a0";
                    serialPortArduino.WriteLine(commando);
                    string antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);

                    int analogValue = Int32.Parse(antwoord);

                    double slope = 40.0 / 1023.0;
                    double offset = 5.0;
                    double temperatureDesired = analogValue * slope + offset;

                    labelGewensteTemp.Text = temperatureDesired.ToString("F1") + " °C";

                    // Read pin 1: 0..1023 → 0..500 °C (Current temperature)
                    commando = "get a1";
                    serialPortArduino.WriteLine(commando);
                    antwoord = serialPortArduino.ReadLine();
                    antwoord = antwoord.TrimEnd();
                    antwoord = antwoord.Substring(4);

                    analogValue = Int32.Parse(antwoord);

                    slope = 500.0 / 1023.0;
                    offset = 0.0;
                    double temperatureCurrent = analogValue * slope + offset;

                    labelHuidigeTemp.Text = temperatureCurrent.ToString("F1") + " °C";

                    // Compare temperatures and control digital pin 2
                    // LED turns on when current temperature < desired temperature
                    if (temperatureCurrent < temperatureDesired)
                    {
                        serialPortArduino.WriteLine("set d2 high");
                    }
                    else
                    {
                        serialPortArduino.WriteLine("set d2 low");
                    }
                }
                else
                {
                    labelStatus.Text = "Error: USB disconnected";
                    serialPortArduino.Close();
                    radioButtonVerbonden.Checked = false;
                    buttonConnect.Text = "Connect";
                }
            }
            catch (Exception exception)
            {
                labelStatus.Text = "Error: " + exception.Message;
                serialPortArduino.Close();
                radioButtonVerbonden.Checked = false;
                buttonConnect.Text = "Connect";
            }
        }
    }
}
