using System;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Threading;

namespace ExitGames.Client.Photon
{
    internal class SocketUdp : IPhotonSocket
    {
        private Socket sock;

        private readonly object syncer = new object();

        public SocketUdp(PeerBase npeer)
            : base(npeer)
        {
            if (ReportDebugOfLevel(DebugLevel.ALL))
            {
                base.Listener.DebugReturn(DebugLevel.ALL, "CSharpSocket: UDP, Unity3d.");
            }
            base.Protocol = ConnectionProtocol.Udp;
            PollReceive = false;
        }

        public override bool Connect()
        {
            //Discarded unreachable code: IL_0059
            lock (syncer)
            {
                if (!base.Connect())
                {
                    return false;
                }
                base.State = PhotonSocketState.Connecting;
                Thread thread = new Thread(DnsAndConnect)
                {
                    Name = "photon dns thread",
                    IsBackground = true
                };
                thread.Start();
                return true;
            }
        }

        public override bool Disconnect()
        {
            if (ReportDebugOfLevel(DebugLevel.INFO))
            {
                EnqueueDebugReturn(DebugLevel.INFO, "CSharpSocket.Disconnect()");
            }
            base.State = PhotonSocketState.Disconnecting;
            lock (syncer)
            {
                if (sock != null)
                {
                    try
                    {
                        sock.Close();
                        sock = null;
                    }
                    catch (Exception arg)
                    {
                        EnqueueDebugReturn(DebugLevel.INFO, "Exception in Disconnect(): " + arg);
                    }
                }
            }
            base.State = PhotonSocketState.Disconnected;
            return true;
        }

        public override PhotonSocketError Send(byte[] data, int length)
        {
            //Discarded unreachable code: IL_0041
            lock (syncer)
            {
                if (!sock.Connected)
                {
                    return PhotonSocketError.Skipped;
                }
                try
                {
                    sock.Send(data, 0, length, SocketFlags.None);
                }
                catch
                {
                    return PhotonSocketError.Exception;
                }
            }
            return PhotonSocketError.Success;
        }

        public override PhotonSocketError Receive(out byte[] data)
        {
            data = null;
            return PhotonSocketError.NoData;
        }

        internal void DnsAndConnect()
        {
            //Discarded unreachable code: IL_008b, IL_00c9
            try
            {
                lock (syncer)
                {
                    sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    IPAddress ipAddress = IPhotonSocket.GetIpAddress(base.ServerAddress);
                    sock.Connect(ipAddress, base.ServerPort);
                    base.State = PhotonSocketState.Connected;
                }
            }
            catch (SecurityException ex)
            {
                if (ReportDebugOfLevel(DebugLevel.ERROR))
                {
                    base.Listener.DebugReturn(DebugLevel.ERROR, "Connect() failed: " + ex.ToString());
                }
                HandleException(StatusCode.SecurityExceptionOnConnect);
                return;
            }
            catch (Exception ex2)
            {
                if (ReportDebugOfLevel(DebugLevel.ERROR))
                {
                    base.Listener.DebugReturn(DebugLevel.ERROR, "Connect() failed: " + ex2.ToString());
                }
                HandleException(StatusCode.ExceptionOnConnect);
                return;
            }
            Thread thread = new Thread(ReceiveLoop)
            {
                Name = "photon receive thread",
                IsBackground = true
            };
            thread.Start();
        }

        public void ReceiveLoop()
        {
            byte[] array = new byte[base.MTU];
            while (base.State == PhotonSocketState.Connected)
            {
                try
                {
                    int length = sock.Receive(array);
                    HandleReceivedDatagram(array, length, willBeReused: true);
                }
                catch (Exception ex)
                {
                    if (base.State != PhotonSocketState.Disconnecting && base.State != 0)
                    {
                        if (ReportDebugOfLevel(DebugLevel.ERROR))
                        {
                            EnqueueDebugReturn(DebugLevel.ERROR, "Receive issue. State: " + base.State + " Exception: " + ex);
                        }
                        HandleException(StatusCode.ExceptionOnReceive);
                    }
                }
            }
            Disconnect();
        }
    }
}
