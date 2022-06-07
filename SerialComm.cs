using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace Spectrometer
{

    public static class PKT_STATE
    {
        public const int HEADER_1 = 0;
        public const int HEADER_2 = 1;
        public const int HEADER_3 = 2;
        public const int DATA = 3;
        public const int ENDER_1= 4;
        public const int ENDER_2= 5;
        public const int ENDER_3= 6;
    }
    public static class PKT_INDEX
    {
        public const int HEADER_1 = 0;
        public const int HEADER_2 = 1;
        public const int HEADER_3 = 2;
        public const int DATA = 3;
        public const int ENDER_1 = 4;
        public const int ENDER_2 = 5;
    }

    public static class DEFINE
    {
        public const byte HEADER_1 = 0xFF;
        public const byte HEADER_2 = 0xFE;
        public const byte HEADER_3 = 0xFD;

        public const byte ENDER_1 = 0xEF;
        public const byte ENDER_2 = 0xEE;

        public const int DATA_BUFFER = 3688;

    }
    public static class INSTRUCTION
    {
        //public const byte SET_ANGLE = 0x01;
        //public const byte SET_RPM = 0x02;
        //public const byte SET_DIRECTION = 0x03;
    }


    public class PKT
    {
        private int length;
        private int inst;
        private int[] data = new int[DEFINE.DATA_BUFFER];

        public int Length { get => length; set => length = value; }
        public int Inst { get => inst; set => inst = value; }
        public int[] Data { get => data; set => data = value; }
    }

    class SerialComm
    {
        public PKT rxPKT = new PKT();
        private int state = PKT_STATE.HEADER_1;

        private string printData;
        public string PrintData { get => printData; set => printData = value; }

        public bool checksumFlag;

        DateTime preTime = DateTime.Now;

        byte[] rxbuf = new byte[DEFINE.DATA_BUFFER + 3+2];//DATA 3688 + HEADER 3 + ENDER 2
        Queue<byte> rxMessage = new Queue<byte>();

        public bool msgParse(byte rx_data, int timeout)
        {
            checksumFlag = false;

            if ((DateTime.Now - preTime).TotalMilliseconds > timeout)
            {
                state = PKT_STATE.HEADER_1;
                preTime = DateTime.Now;
            }
            /////Add : Time out 걸어서 header state로 이동
            // 상태머신으로 parsing
            switch (state)
            {
                case PKT_STATE.HEADER_1:
                    if (rx_data == 0xFF)
                    {
                        rxMessage.Clear();
                        rxMessage.Enqueue(rx_data);
                        state = PKT_STATE.HEADER_2;
                    }
                    break;

                case PKT_STATE.HEADER_2:
                    if (rx_data == 0xFE)
                    {
                        rxMessage.Enqueue(rx_data);
                        state = PKT_STATE.HEADER_3;
                    }
                    else 
                    {state = PKT_STATE.HEADER_1; rxMessage.Clear();}
                    break;

                case PKT_STATE.HEADER_3:
                    if (rx_data == 0xFD)
                    {
                        rxMessage.Enqueue(rx_data);
                        state = PKT_STATE.DATA;
                    }
                    else
                    {state = PKT_STATE.HEADER_1; rxMessage.Clear();}
                    break;

                case PKT_STATE.DATA:
                    rxMessage.Enqueue(rx_data);
                    state = PKT_STATE.ENDER_1;
                    break;

                case PKT_STATE.ENDER_1:
                    rxMessage.Enqueue(rx_data);
                    state = PKT_STATE.ENDER_2;
                    break;

                case PKT_STATE.ENDER_2:
                    rxMessage.Enqueue(rx_data);
                    int i = 0;
                    while (rxMessage.Count > 0)
                    { rxbuf[i++] = rxMessage.Dequeue(); }
                    state = PKT_STATE.HEADER_1;
                    break;
            }

            PrintData = BitConverter.ToString(rxbuf);
            PrintData = PrintData.Replace('-', ' ');

            return checksumFlag;
/*           *************Log Printf************
 *           if (checksumFlag == true & rxLogFlag == true)
            {
                string hexString = BitConverter.ToString(rxbuf);
                hexString = hexString.Replace('-', ' ');
                return hexString;
*//*              txtRead.Text += hexString;
                txtRead.Text += Environment.NewLine + "------------------" + Environment.NewLine;
                txtRead.Text += "Length : " + rxPKT.length.ToString() + Environment.NewLine;
                txtRead.Text += "Instruction : " + rxPKT.inst.ToString() + Environment.NewLine;
                txtRead.Text += "Parameter :" + rxPKT.param.ToString() + Environment.NewLine;*//*
            }
            else { 
            return "";
            }*/
        }

        private byte ChecksumByte(byte[] txData)
        {
            uint sum = 0;
            for (int i = 0; i < txData.Length - 1; i++)
            {
                sum += Convert.ToUInt32(txData[i]);
            }
            sum = sum & 0xFF;
            sum = (~sum + 1) & 0xFF;

            return ((byte)sum);
            //            total += sum;
            //            return total;
        }
        private bool ChecksumPAK(byte[] txData, byte Checkbyte)
        {
            bool ret = false;
            uint sum = 0, total = 0;
            for (int i = 0; i < txData.Length - 1; i++)
            {
                sum += Convert.ToUInt32(txData[i]);
            }
            total = sum;
            total = total & 0xFF;
            total = (~total + 1) & 0xFF;
            total += sum;
            total = total & 0xFF;
            if (total == 0)
            {
                return ret = true;
            }
            return ret;
        }

    }
}
