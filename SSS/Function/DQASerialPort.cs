using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SerialPortTest_002;


namespace DQASerailPortFunction
{
    class ProcessString
    {
        private ushort CalCRC(ushort PreCRC, byte data)
        {
            int j;
            ushort to_xor;

            to_xor = (ushort)((PreCRC ^ data) & 0xff);

            for (j = 0; j < 8; j++)
            {
                if ((to_xor & 0x01) == 1)
                {
                    to_xor = (ushort)((to_xor >> 1) ^ 0xA001);
                }

                else
                    to_xor >>= 1;
            }
            PreCRC = (ushort)((PreCRC >> 8) ^ to_xor);

            return PreCRC;
        }

        public ushort CalculateCRC(int dataLen,byte[] inBuf)
        {
            int i;
            ushort CRCResult = 0xFFFF;

            for (i = 0; i <= (dataLen - 1); i++)
            {
                CRCResult = CalCRC(CRCResult, inBuf[i]);

            }
            return CRCResult;
        }
        public byte BytetoASCII(byte data)
        {
            byte tempByte = 0;
            //data = (byte)(data & 0x0F);
            if ((data >= 0x00) && (data <= 0x09))
            {
                tempByte = (byte)(data | 0x30);
            }
            else
            {
                switch (data)
                {
                    case 10:
                        tempByte = 0x41;
                        break;
                    case 11:
                        tempByte = 0x42;
                        break;
                    case 12:
                        tempByte = 0x43;
                        break;
                    case 13:
                        tempByte = 0x44;
                        break;
                    case 14:
                        tempByte = 0x45;
                        break;
                    case 15:
                        tempByte = 0x46;
                        break;
                    default:
                        tempByte = 0x00;
                        break;
                }

            }
            return tempByte;
        }
        public byte ASCIIToByte(byte data)
        {
            byte i;
            if ((data >= 0x30) && (data <= 0x39))
            {
                return ((byte)(data & 0x0F));
            }
            else
            {
                switch (data)
                {
                    case 0x41:
                    case 0x61:
                        i = 0x0A;
                        break;
                    case 0x42:
                    case 0x62:
                        i = 0x0B;
                        break;
                    case 0x43:
                    case 0x63:
                        i = 0x0C;
                        break;
                    case 0x44:
                    case 0x64:
                        i = 0x0D;
                        break;
                    case 0x45:
                    case 0x65:
                        i = 0x0E;
                        break;
                    case 0x46:
                    case 0x66:
                        i = 0x0F;
                        break;
                    default:
                        i = 0;
                        break;
                }
                return i;
            }
        }

    }
 
}
namespace DQATestCoreFun
{
    class DQACoreFun
    {
        
        public void ExecuteTask()
        {
            //int DtatRows = tempfrom.GetDataGridCount();
            //int i;
            //int CmdType;
            //string tempResult = "";
            //byte[] DataBuf = new byte [100];
            //byte[] CRCBuf = new byte[2];
            //int DelayCount = 0;
            ////if (tempfrom.FlagComPortStauts == 0)
            ////{
            ////    MessageBox.Show("Connect to Device first");
            ////}
            ////else
            //{
            //    if (DtatRows <= 1)
            //    {
            //        MessageBox.Show("No data line, finish");
            //    }
            //    else
            //    {
            //        for (i = 0; i <= (DtatRows - 1); i++)
            //        {
            //            tempfrom.GetDataGridData(0, 0, ref tempResult);
            //        }

            //    }
            //}








        }
    }

}


