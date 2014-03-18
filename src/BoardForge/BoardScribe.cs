using System;
using System.IO.Ports;

namespace BoardForge
{
    public class Forge
    {
        readonly SerialPort mySerialPort = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);

        public void MoveRight() {SendCommand("M2R");}

        public void MoveLeft() {SendCommand("M2L");}

        public void MoveForward() { SendCommand("M1R"); }

        public void MoveBackwards() { SendCommand("M1L"); }

        public void LightTop(bool On) {SendCommand("L" + (On ? "1" : "0") + "1");}

        public void LightBottom(bool On) {SendCommand("L" + (On ? "1" : "0") + "2");}

        public void MoveFarLeft()
        {
            throw new NotImplementedException();
        }

        public void SendCommand(string command)
        {
            try
            {
                mySerialPort.Open();
                mySerialPort.Write(command);
                mySerialPort.Close();
            }
            catch
            {
                //MessageBox.Show("Could not connect to the BoardForge");
            }
        }
    }
}
