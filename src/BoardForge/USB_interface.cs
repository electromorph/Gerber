using System;
using System.Runtime.InteropServices;
using PVOID = System.IntPtr;
using DWORD = System.UInt32;

namespace BoardForge
{
    unsafe public class USBInterface
    {
        #region  String Definitions of Pipes and VID_PID
        //string vid_pid_boot= "vid_04d8&pid_000b";    // Bootloader vid_pid ID
        string vid_pid_norm = "vid_04d8&pid_000c";

        string out_pipe = "\\MCHP_EP1"; // Define End Points
        string in_pipe = "\\MCHP_EP1";
        #endregion

        #region Imported DLL functions from mpusbapi.dll
        [DllImport("mpusbapi.dll")]
        private static extern DWORD _MPUSBGetDLLVersion();
        [DllImport("mpusbapi.dll")]
        private static extern DWORD _MPUSBGetDeviceCount(string pVID_PID);
        [DllImport("mpusbapi.dll")]
        private static extern void* _MPUSBOpen(DWORD instance, string pVID_PID, string pEP, DWORD dwDir, DWORD dwReserved);
        [DllImport("mpusbapi.dll")]
        private static extern DWORD _MPUSBRead(void* handle, void* pData, DWORD dwLen, DWORD* pLength, DWORD dwMilliseconds);
        [DllImport("mpusbapi.dll")]
        private static extern DWORD _MPUSBWrite(void* handle, void* pData, DWORD dwLen, DWORD* pLength, DWORD dwMilliseconds);
        [DllImport("mpusbapi.dll")]
        private static extern DWORD _MPUSBReadInt(void* handle, DWORD dwLen, DWORD* pLength, DWORD dwMilliseconds);
        [DllImport("mpusbapi.dll")]
        private static extern bool _MPUSBClose(void* handle);
        #endregion

        void* myOutPipe;
        void* myInPipe;

        private void OpenPipes()
        {
            DWORD selection = 0; // Selects the device to connect to, in this example it is assumed you will only have one device per vid_pid connected.

            myOutPipe = _MPUSBOpen(selection, vid_pid_norm, out_pipe, 0, 0);
            myInPipe = _MPUSBOpen(selection, vid_pid_norm, in_pipe, 1, 0);
        }
        private void ClosePipes()
        {
            _MPUSBClose(myOutPipe);
            _MPUSBClose(myInPipe);
        }

        private DWORD SendReceivePacket(byte* SendData, DWORD SendLength, byte* ReceiveData, DWORD* ReceiveLength)
        {
            uint SendDelay = 1000;
            uint ReceiveDelay = 1000;

            DWORD SentDataLength;
            DWORD ExpectedReceiveLength = *ReceiveLength;

            OpenPipes();

            if (_MPUSBWrite(myOutPipe, (void*)SendData, SendLength, &SentDataLength, SendDelay) == 1)
            {

                if (_MPUSBRead(myInPipe, (void*)ReceiveData, ExpectedReceiveLength, ReceiveLength, ReceiveDelay) == 1)
                {
                    if (*ReceiveLength == ExpectedReceiveLength)
                    {
                        ClosePipes();
                        return 1;   // Success!
                    }
                    else if (*ReceiveLength < ExpectedReceiveLength)
                    {
                        ClosePipes();
                        return 2;   // Partially failed, incorrect receive length
                    }
                }
            }
            ClosePipes();
            return 0;  // Operation Failed
        }

        public DWORD GetDLLVersion()
        {
            return _MPUSBGetDLLVersion();
        }
        public DWORD GetDeviceCount(string Vid_Pid)
        {
            return _MPUSBGetDeviceCount(Vid_Pid);
        }

        public int UpdateLED(uint led, bool State)
        {
            // The default demo firmware application has a defined application
            // level protocol.
            // To set the LED's, the host must send the UPDATE_LED
            // command which is defined as 0x32, followed by the LED to update,
            // then the state to chance the LED to.
            //
            // i.e. <UPDATE_LED><0x01><0x01>
            //
            // Would activate LED 1
            //
            // The receive buffer size must be equal to or larger than the maximum
            // endpoint size it is communicating with. In this case, it is set to 64 bytes.

            byte* send_buf = stackalloc byte[64];
            byte* receive_buf = stackalloc byte[64];

            DWORD RecvLength = 3;
            send_buf[0] = 0x32;			//Command for LED Status  
            send_buf[1] = (byte)led;
            send_buf[2] = (byte)(State ? 1 : 0);

            if (SendReceivePacket(send_buf, 3, receive_buf, &RecvLength) == 1)
            {
                if (RecvLength == 1 && receive_buf[0] == 0x32)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 1;
            }
        }

        public int IndexRight()
        {

            byte* send_buf = stackalloc byte[64];
            byte* receive_buf = stackalloc byte[64];

            DWORD RecvLength = 3;
            send_buf[0] = 0xE5;			//Command for LED Status  
            send_buf[1] = 1;
            send_buf[2] = 1;

            if (SendReceivePacket(send_buf, 3, receive_buf, &RecvLength) == 1)
            {
                if (RecvLength == 1 && receive_buf[0] == 0x32)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 1;
            }
        }

        public int IndexLeft()
        {

            byte* send_buf = stackalloc byte[64];
            byte* receive_buf = stackalloc byte[64];

            DWORD RecvLength = 3;
            send_buf[0] = 0xE4;			//Command for LED Status  
            send_buf[1] = 1;
            send_buf[2] = 1;

            if (SendReceivePacket(send_buf, 3, receive_buf, &RecvLength) == 1)
            {
                if (RecvLength == 1 && receive_buf[0] == 0x32)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 1;
            }
        }

        public int TurnPortBOn()
        {

            byte* send_buf = stackalloc byte[64];
            byte* receive_buf = stackalloc byte[64];

            DWORD RecvLength = 3;
            send_buf[0] = 0xE8;			//Command for LED Status  
            send_buf[1] = 1;
            send_buf[2] = 1;

            if (SendReceivePacket(send_buf, 3, receive_buf, &RecvLength) == 1)
            {
                if (RecvLength == 1 && receive_buf[0] == 0x32)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 1;
            }
        }

        public int EnableMotors()
        {

            byte* send_buf = stackalloc byte[64];
            byte* receive_buf = stackalloc byte[64];

            DWORD RecvLength = 3;
            send_buf[0] = 0xE6;			//Command for Enable Motors
            send_buf[1] = 1;
            send_buf[2] = 1;

            if (SendReceivePacket(send_buf, 3, receive_buf, &RecvLength) == 1)
            {
                if (RecvLength == 1 && receive_buf[0] == 0x32)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 1;
            }
        }

        public int DisableMotors()
        {

            byte* send_buf = stackalloc byte[64];
            byte* receive_buf = stackalloc byte[64];

            DWORD RecvLength = 3;
            send_buf[0] = 0xE7;			//Command for Disable Motors  
            send_buf[1] = 1;
            send_buf[2] = 1;

            if (SendReceivePacket(send_buf, 3, receive_buf, &RecvLength) == 1)
            {
                if (RecvLength == 1 && receive_buf[0] == 0x32)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 1;
            }
        }

        public int IndexLeft(uint led, bool State)
        {

            byte* send_buf = stackalloc byte[64];
            byte* receive_buf = stackalloc byte[64];

            DWORD RecvLength = 3;
            send_buf[0] = 0xE4;  
            send_buf[1] = 1;
            send_buf[2] = 1;

            if (SendReceivePacket(send_buf, 3, receive_buf, &RecvLength) == 1)
            {
                if (RecvLength == 1 && receive_buf[0] == 0x32)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 1;
            }
        }

        public uint Add(uint num1, uint num2)
        {

            byte* send_buf = stackalloc byte[64];
            byte* receive_buf = stackalloc byte[64];

            uint Temp = 0;

            DWORD RecvLength = (byte)2;
            send_buf[0] = 0xEE;			//Command for DO_SUM  
            send_buf[1] = (byte)num1;
            send_buf[2] = (byte)num2;

            if (SendReceivePacket(send_buf, 4, receive_buf, &RecvLength) == 1)
            {
                Temp = receive_buf[1];
            }
            return Temp;
        }

        public int SendAndReceiveCommand(uint command, uint data)
        {
            byte* send_buf = stackalloc byte[64];
            byte* receive_buf = stackalloc byte[64];

            DWORD RecvLength = 3;
            send_buf[0] = 0xE2;			//Command for Send Command
            send_buf[1] = (byte)command;
            send_buf[2] = (byte)(data);

            if (SendReceivePacket(send_buf, 3, receive_buf, &RecvLength) == 1)
            {
                if (RecvLength == 1 && receive_buf[0] == 0x32)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
            else
            {
                return 1;
            }
        }
    }
}

