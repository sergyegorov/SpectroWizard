using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace SpectroWizard.dev.interf
{
    public abstract class CONInterf
    {
        abstract public string Open();
        abstract public void Close();
        abstract public bool IsConnected();
    }

    public class IPInterf : CONInterf
    {
        const string MLSConst = "IPInterf";

        private Socket Socket = null;
        EndPoint Destination;
        public override string Open()
        {
            /*string addr = Common.Conf.IP1 + "." +
                    Common.Conf.IP2 + "." +
                    Common.Conf.IP3 + "." +
                    Common.Conf.IP4;*/
            int port = Common.Conf.Port;
            if (Socket != null)
                return null;
            Socket s = null;
            Socket = null;
            try
            {
                //IPHostEntry ent = Dns.GetHostEntry(addr);
                byte[] bt = {Common.Conf.IP1,Common.Conf.IP2,Common.Conf.IP3,Common.Conf.IP4};
                IPAddress ipAddress = new IPAddress(bt);// ent.AddressList[0];
                EndPoint destination = new IPEndPoint(ipAddress, port);
                s = new Socket(destination.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
                s.Bind(new IPEndPoint(IPAddress.Any, port));
                s.Connect(destination);
                s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 500000);
                s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendBuffer, 500000);
                Destination = destination;//*/

                IpPacket p = IpPacket.GetCommand_PowerOnReset();
                Send(p, 1000, s);

                SpectroWizard.gui.MainForm.SetupConnectionIndicator(true);
            }
            catch (Exception e)
            {
                if(s != null)
                try
                {
                    SpectroWizard.gui.MainForm.SetupConnectionIndicator(false);
                    s.Shutdown(SocketShutdown.Both);
                }
                finally
                {
                    try
                    {
                        s.Close();
                    }
                    finally
                    {
                    }
                }
                Common.LogNoMsg(e);
                return e.Message;
            }
            Socket = s;
            return null;
        }

        public override void Close()
        {
            if (Socket != null && Socket.Connected)
            {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
            }
            Socket = null;
        }

        public override bool IsConnected()
        {
            if (Socket == null)
                return false;
            return Socket.Connected;
        }

        static public int mToInt(byte b1, byte b2, byte b3, byte b4)
        {
            int rez = (sbyte)b4;
            rez <<= 8;
            rez |= b3;
            rez <<= 8;
            rez |= b2;
            rez <<= 8;
            rez |= b1;
            return rez;
        }

        byte[] TmpBuffer = new byte[16000];
        public IpPacket Send(IpPacket data, int time_out_mls)
        {
            return Send(data, time_out_mls, Socket);
        }

        public IpPacket Send(IpPacket data, int time_out_mls,Socket s)
        {
            while (s.Available != 0)
                s.Receive(TmpBuffer);

            if (s.SendTo(data.Buffer, Destination) != data.Buffer.Length)
                throw new Exception("Send data error...");
            //--------------------------
            long tick_from = DateTime.Now.Ticks;
            //gui.MainForm.MForm.SetupTimeOut(time_out_mls / 1000F);
            while (s.Available == 0 &&
                (DateTime.Now.Ticks - tick_from) / 10000 < time_out_mls)
                Thread.Sleep(10);
            //gui.MainForm.MForm.SetupTimeOut(0);
            if (s.Available == 0)
                throw new Exception("No reply");
            //--------------------------
            int l = s.Receive(TmpBuffer);
            IpPacket p = new IpPacket(TmpBuffer, l);
            if (p.PacketSize + 8 != l)
                throw new Exception("Wrong size of packet");
            if(data.IsGoodReply(p) == false)
                throw new Exception("Wrong reply: Command1="+p.Command1+" Command2="+p.Command2);
            return p;
        }

        static short mToShort(byte b1, byte b2)
        {
            int rez = (sbyte)b2;
            rez <<= 8;
            rez |= b1;
            return (short)rez;
        }

        /*private static byte[] mReadReply()
        {
            byte[] ret;
            short scn;
            do
            {
                ret = clsCommon.mHardwareInterface.mReceiveData();
                scn = mToShort(ret[16], ret[17]);
            } while (-scn < mCycleCommandNumber && clsCommon.mHardwareInterface.mHasData());
            if (-scn != mCycleCommandNumber)
                throw new Exception("Packet mis order...");
            byte[] proc = new byte[ret.Length - mBodyBase];
            for (int j = 0; j < proc.Length; j++)
                proc[j] = ret[j + mBodyBase];
            return proc;
        }*/


        public short[][] ReadData(int time_out_mls, byte[] start_packet)
        {
            short[][] bb,be;
            return ReadData(time_out_mls, start_packet,out bb,out be);
        }

        //public short[][] ReadData(int time_out_mls, byte[] start_packet)
        public short[][] ReadData(int time_out_mls,byte[] start_packet,out short[][] blank_before,out short[][] blank_after)
        {
            time_out_mls += 1000;
            long tick_from = DateTime.Now.Ticks;
            //gui.MainForm.MForm.SetupTimeOut(time_out_mls/1000F);
            while (Socket.Available == 0 &&
                (DateTime.Now.Ticks - tick_from) / 10000 < time_out_mls)
                Thread.Sleep(10);
            //gui.MainForm.MForm.SetupTimeOut(0);
            
            if (Socket.Available == 0)
                throw new Exception(Common.MLS.Get(MLSConst,"No reply..."));

            List<byte> data = new List<byte>();

            int l;
            if (start_packet != null)
            {
                for (int i = 18; i < start_packet.Length; i++)
                    data.Add(start_packet[i]);
            }
            else
            {
                l = Socket.Receive(TmpBuffer);
                for (int i = 18; i < l; i++)
                    data.Add(TmpBuffer[i]);
            }

            int ver = mToShort(data[2], data[3]);
            int n = mToShort(data[4], data[5]);
            int size = mToInt(data[6], data[7], data[8], data[9]);
            int exp_n;
            if (ver == 1)
                exp_n = 8;
            else
                exp_n = 16;
            int[] mult = new int[exp_n];
            int ind;
            for (int i = 0; i < exp_n; i++)
            {
                ind = 10 + exp_n * 4 + i * 4;
                mult[i] = mToInt(data[ind], data[ind + 1], data[ind + 2], data[ind + 3]);
            }
            ind = 10 + exp_n * 2 * 4;

            int size_must_to_be = ind + n * size;
            do
            {
                l = Socket.Receive(TmpBuffer);
                for (int i = 16; i < l; i++)
                    data.Add(TmpBuffer[i]);
                for (int i = 0; i < 20 && Socket.Available == 0 && data.Count < size_must_to_be;i++ )
                    Thread.Sleep(1);
            } while (Socket.Available != 0);

            short[][] ret = new short[n][];
            blank_before = new short[n][];
            blank_after = new short[n][];
            for (int s = 0; s < n; s++)
            {
                short[] tmp;
                if (ver == 1)
                {
                    //ret[s] 
                    tmp = new short[size / 2];
                    for (int i = 0; i < tmp.Length; i++, ind += 2)
                        //ret[s][i] 
                        tmp[i] = (short)(mToInt(data[ind], data[ind + 1], 0, 0));
                }
                else
                {
                    //ret[s]
                    tmp = new short[size / 4];
                    for (int i = 0; i < tmp.Length; i++, ind += 4)
                        //ret[s][i]
                        tmp[i] = (short)(mToInt(data[ind], data[ind + 1],
                            data[ind + 2], data[ind + 3]) / mult[s]);
                }
                if (Common.Conf.BlakPixelEnd == 0 && Common.Conf.BlakPixelStart == 0)
                {
                    ret[s] = tmp;
                    blank_after[s] = null;
                    blank_before[s] = null;
                }
                else
                {
                    int start, end;
                    if ((s & 1) != 0)
                    {
                        start = Common.Conf.BlakPixelStart;
                        end = Common.Conf.BlakPixelEnd;
                    }
                    else
                    {
                        end = Common.Conf.BlakPixelStart;
                        start = Common.Conf.BlakPixelEnd;
                    }
                    double[] blank = new double[end + start];
                    blank_before[s] = new short[start];
                    blank_after[s] = new short[end];
                    for (int i = 0; i < start; i++)
                    {
                        blank[i] = tmp[i];
                        blank_before[s][i] = tmp[i];
                    }
                    int from = tmp.Length - 1 - end;
                    for (int i = 0; i < end; i++)
                    {
                        blank[i + start] = tmp[from + i];
                        blank_after[s][i] = tmp[from + i];
                    }

                    /*short ever;
                    if (Common.Conf.BlankSub == true)
                        ever = (short)SpectroWizard.analit.Stat.GetEver(blank);
                    else
                        ever = 0;*/

                    short[] rtmp = new short[tmp.Length - end - start];
                    for (int i = 0; i < rtmp.Length; i++)
                        rtmp[i] = (short)(tmp[i + start]);

                    ret[s] = rtmp;
                }
            }
            
			return ret;
        }

        public class IpPacket
        {
            public byte[] Buffer = null;
            public int BufferLen = 0;
            //---- Packet level ---------------------
            static int CycleNumLast = 0;
            public int CycleNum
            {
                get
                {
                    return (((int)Buffer[0])&0xFF) |
                        (((int)(Buffer[1]) & 0xFF) << 8) |
                        (((int)(Buffer[2]) & 0xFF) << 16) |
                        (((int)(Buffer[3]) & 0xFF) << 24);
                }
                set
                {
                    Buffer[0] = (byte)(value & 0xFF);
                    Buffer[1] = (byte)((value >> 8) & 0xFF);
                    Buffer[2] = (byte)((value >> 16) & 0xFF);
                    Buffer[3] = (byte)((value >> 24) & 0xFF);
                }
            }
            public int PacketSize
            {
                get
                {
                    return (((int)Buffer[4]) & 0xFF) |
                        (((int)(Buffer[5]) & 0xFF) << 8) |
                        (((int)(Buffer[6]) & 0xFF) << 16) |
                        (((int)(Buffer[7]) & 0xFF) << 24);
                }
                set
                {
                    Buffer[4] = (byte)(value & 0xFF);
                    Buffer[5] = (byte)((value >> 8) & 0xFF);
                    Buffer[6] = (byte)((value >> 16) & 0xFF);
                    Buffer[7] = (byte)((value >> 24) & 0xFF);
                }
            }
            public short DSN
            {
                get
                {
                    return (short)((((short)Buffer[8]) & 0xFF) |
                                  (((short)(Buffer[9]) & 0xFF) << 8));
                }
                set
                {
                    Buffer[8] = (byte)(value & 0xFF);
                    Buffer[9] = (byte)((value >> 8) & 0xFF);
                }
            }
            public byte PaketType
            {
                get
                {
                    return Buffer[10];
                }
                set
                {
                    Buffer[10] = value;
                }
            }
            public byte Reserv;
            public int PaketPointer
            {
                get
                {
                    return (((int)Buffer[12]) & 0xFF) |
                        (((int)(Buffer[13]) & 0xFF) << 8) |
                        (((int)(Buffer[14]) & 0xFF) << 16) |
                        (((int)(Buffer[15]) & 0xFF) << 24);
                }
                set
                {
                    Buffer[12] = (byte)(value & 0xFF);
                    Buffer[13] = (byte)((value >> 8) & 0xFF);
                    Buffer[14] = (byte)((value >> 16) & 0xFF);
                    Buffer[15] = (byte)((value >> 24) & 0xFF);
                }
            }
            //---- Command level --------------------
            static short CycleCommandLast = 0;
            public short CycleCommandNum
            {
                get
                {
                    return (short)((((ushort)Buffer[16]) & 0xFF) |
                                  (((ushort)(Buffer[17]) & 0xFF) << 8));
                }
                set
                {
                    Buffer[16] = (byte)(value & 0xFF);
                    Buffer[17] = (byte)((value >> 8) & 0xFF);
                }
            }
            public byte Command1
            {
                get
                {
                    return Buffer[18];
                }
                set
                {
                    Buffer[18] = value;
                }
            }
            public byte Command2
            {
                get
                {
                    return Buffer[19];
                }
                set
                {
                    Buffer[19] = value;
                }
            }
            public byte[] ExtraCommand = null;

            //---- Extra data------
            public short ConfigVersion
            {
                get { return (short)(Buffer[20] | (Buffer[21] << 8));  }
            }
            public byte ConfigHType
            {
                get { return Buffer[22];  }
            }
            public byte ConfigHVer
            {
                get { return Buffer[23]; }
            }
            public short ConfigLineCount
            {
                get { return (short)(Buffer[24] | (Buffer[25] << 8)); }
            }
            public short ConfigSensorSize
            {
                get { return (short)(Buffer[26] | (Buffer[27] << 8)); }
            }
            public short ConfigSDir
            {
                get { return (short)(Buffer[28] | (Buffer[29] << 8)); }
            }
            byte ReplyCommand1, ReplyCommand2;
            public bool IsGoodReply(IpPacket reply)
            {
                if (reply.Command1 == ReplyCommand1 &&
                    reply.Command2 == ReplyCommand2)
                    return true;
                return false;
            }

            public IpPacket(byte[] buff, int len)
            {
                Buffer = new byte[len];
                for (int i = 0; i < len; i++)
                    Buffer[i] = buff[i];
                BufferLen = len;
            }

            private IpPacket()
            {
            }

            void InitSize()
            {
                int size;
                if (ExtraCommand != null)
                    size = 12 + ExtraCommand.Length;
                else
                    size = 12;
                Buffer = new byte[size+8];
                BufferLen = size+8;
                PacketSize = size;
                InitExtraInfo();
            }

            void InitExtraInfo()
            {
                if (ExtraCommand == null)
                    return;
                for (int i = 0; i < ExtraCommand.Length; i++)
                    Buffer[12 + 8 + i] = ExtraCommand[i];
            }

            static public IpPacket GetCommand_PowerOnReset()
            {
                IpPacket p = new IpPacket();

                p.InitSize();

                p.CycleNum = CycleNumLast++;
                p.CycleCommandNum = CycleCommandLast++;
                p.DSN = 15;
                p.PaketType = 1;
                p.Command1 = 1;
                p.Command2 = 1;
                p.ReplyCommand1 = 254;
                p.ReplyCommand2 = 2;
                return p;
            }

            static public IpPacket GetCommand_CheckOnline()
            {
                IpPacket p = new IpPacket();

                p.InitSize();

                p.CycleNum = CycleNumLast++;
                p.CycleCommandNum = CycleCommandLast++;
                p.DSN = 15;
                p.PaketType = 1;
                p.Command1 = 1;
                p.Command2 = 4;
                p.ReplyCommand1 = 254;
                p.ReplyCommand2 = 2;
                return p;
            }

            static public IpPacket GetCommand_StartMeasuring()
            {
                IpPacket p = new IpPacket();

                p.InitSize();

                p.CycleNum = CycleNumLast++;
                p.CycleCommandNum = CycleCommandLast++;
                p.DSN = 15;
                p.PaketType = 1;
                p.Command1 = 5;
                p.Command2 = 1;
                p.ReplyCommand1 = 254;
                p.ReplyCommand2 = 3;
                return p;
            }

            static public IpPacket GetCommand_ReSendLastData()
            {
                IpPacket p = new IpPacket();

                p.InitSize();

                p.CycleNum = CycleNumLast++;
                p.CycleCommandNum = CycleCommandLast++;
                p.DSN = 15;
                p.PaketType = 1;
                p.Command1 = 6;
                p.Command2 = 1;
                p.ReplyCommand1 = 251;
                p.ReplyCommand2 = 1;
                return p;
            }

            static public IpPacket GetCommand_SetDivider(byte val)
            {
                IpPacket p = new IpPacket();
                
                p.ExtraCommand = new byte[4];
                p.ExtraCommand[0] = 1;
                p.ExtraCommand[1] = 0;
                p.ExtraCommand[2] = val;
                p.ExtraCommand[3] = (byte)(val >> 8);

                p.InitSize();

                p.CycleNum = CycleNumLast++;
                p.CycleCommandNum = CycleCommandLast++;
                p.DSN = 15;
                p.PaketType = 1;
                p.Command1 = 3;
                p.Command2 = 3;

                p.ReplyCommand1 = 254;
                p.ReplyCommand2 = 2;
                return p;
            }

            static public IpPacket GetCommand_SetExpositions(int common_time,
                int[] exps,int ver)
            {
                IpPacket p = new IpPacket();

                int len;
                byte comm;
                if (ver == 4)
                {
                    len = 16;
                    comm = 2;
                }
                else
                {
                    if (ver == 2)
                    {
                        len = 8;
                        comm = 1;
                    }
                    else
                        throw new Exception("Exposition error: unsuported version of the device: "+ver);
                }
                p.ExtraCommand = new byte[2 + 4 + 4 * len];
                p.ExtraCommand[0] = comm;
                p.ExtraCommand[1] = 0;
                int[] tmp = new int[exps.Length + 1];
                tmp[0] = common_time;
                for (int i = 0; i < exps.Length; i++)
                    tmp[1 + i] = exps[i];
                for (int i = 0; i < tmp.Length; i++)
                {
                    p.ExtraCommand[2 + i * 4] = (byte)tmp[i];
                    p.ExtraCommand[3 + i * 4] = (byte)(tmp[i] >> 8);
                    p.ExtraCommand[4 + i * 4] = (byte)(tmp[i] >> 16);
                    p.ExtraCommand[5 + i * 4] = (byte)(tmp[i] >> 24);
                }

                p.InitSize();

                p.CycleNum = CycleNumLast++;
                p.CycleCommandNum = CycleCommandLast++;
                p.DSN = 15;
                p.PaketType = 1;
                p.Command1 = 4;
                p.Command2 = 1;
                p.ReplyCommand1 = 254;
                p.ReplyCommand2 = 2;
                return p;
            }

            static public IpPacket GetCommand_SetGenStatus(bool status)
            {
                IpPacket p = new IpPacket();

                p.InitSize();

                p.CycleNum = CycleNumLast++;
                p.CycleCommandNum = CycleCommandLast++;
                p.DSN = 15;
                p.PaketType = 1;
                p.Command1 = 108;
                if(status)
                    p.Command2 = 2;
                else
                    p.Command2 = 3;

                p.ReplyCommand1 = 254;
                p.ReplyCommand2 = 2;
                return p;
            }

            static public IpPacket GetCommand_SetGenByte(byte data)
            {
                IpPacket p = new IpPacket();

                p.ExtraCommand = new byte[1];
                p.ExtraCommand[0] = data;

                p.InitSize();

                p.CycleNum = CycleNumLast++;
                p.CycleCommandNum = CycleCommandLast++;
                p.DSN = 15;
                p.PaketType = 1;
                p.Command1 = 108;
                p.Command2 = 10;

                p.ReplyCommand1 = 254;
                p.ReplyCommand2 = 2;
                return p;
            }

            static public IpPacket GetCommand_GetConfig()
            {
                IpPacket p = new IpPacket();

                p.InitSize();

                p.CycleNum = CycleNumLast++;
                p.CycleCommandNum = CycleCommandLast++;
                p.DSN = 15;
                p.PaketType = 1;
                p.Command1 = 3;
                p.Command2 = 1;

                p.ReplyCommand1 = 253;
                p.ReplyCommand2 = 1;
                return p;
            }

            static public IpPacket GetCommand_GetDivider()
            {
                IpPacket p = new IpPacket();

                p.InitSize();

                p.CycleNum = CycleNumLast++;
                p.CycleCommandNum = CycleCommandLast++;
                p.DSN = 15;
                p.PaketType = 1;
                p.Command1 = 3;
                p.Command2 = 2;

                p.ReplyCommand1 = 253;
                p.ReplyCommand2 = 2;
                return p;
            }

            static public IpPacket GetCommand_Reset()
            {
                IpPacket p = new IpPacket();

                p.InitSize();

                p.CycleNum = CycleNumLast++;
                p.CycleCommandNum = CycleCommandLast++;
                p.DSN = 15;
                p.PaketType = 1;
                p.Command1 = 1;
                p.Command2 = 3;

                p.ReplyCommand1 = 254;
                p.ReplyCommand2 = 2;
                return p;
            }
        }
    }
}
