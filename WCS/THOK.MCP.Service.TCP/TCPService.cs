using System;
using System.Collections.Generic;
using System.Text;
using THOK.MCP;
using THOK.TCP;
using System.Net.Sockets;
using System.Net;

namespace THOK.MCP.Service.TCP
{
    public class TCPService:THOK.MCP.AbstractService 
    {
        private System.Timers.Timer time1;
        private DateTime dtTime;
        private Server server = null;
        private Client client = null;
        private string ip = "127.0.0.1";
        private int port = 6000;
        private IProtocolParse protocol = null;
        string lastSeqNo = "";
        int i = 0;

        public override void Initialize(string file)
        {
            Config.Configuration config = new Config.Configuration(file);
            protocol = (IProtocolParse)ObjectFactory.CreateInstance(config.Type);

            ip = config.IP;
            port = config.Port;
            server = new Server();
            server.OnReceive += new ReceiveEventHandler(server_OnReceive);
            server.OnConnect += new SocketEventHandler(server_OnConnect);
            server.OnDisconnect += new SocketEventHandler(server_OnDisconnect);
            time1 = new System.Timers.Timer();
            time1.Interval = 20000;
            dtTime = DateTime.Now;
            //time1.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);

            time1.Elapsed += new System.Timers.ElapsedEventHandler(time1_Elapsed);

            time1.Start();
            
        }

        void time1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            TimeSpan midTime = DateTime.Now - dtTime;
            if (midTime.TotalSeconds >= 45)
            {

                //context.ProcessDispatcher.WriteToService("Crane", "DUM", "<00000CRAN30THOK01DUM0000000>");
                //DispatchState("DUU", "");
                this.Write("DUM", "<00000CRAN30THOK01DUM0000000>");
                dtTime = DateTime.Now;
            }
        }
        //protected void OnTimer(Object source, System.Timers.ElapsedEventArgs e)
        //{
        //    System.Threading.Thread thdInStock = new System.Threading.Thread(new System.Threading.ThreadStart(AutoInStock));
        //    thdInStock.IsBackground = true;
        //    thdInStock.Start();
        //}

        private void AutoInStock()
        {
            TimeSpan midTime = DateTime.Now - dtTime;
            if (midTime.TotalSeconds >= 55)
            {
                //context.ProcessDispatcher.WriteToService("Crane", "DUM", "<00000CRAN30THOK01DUM0000000>");
                DispatchState("DUM", "<00000CRAN30THOK01DUM0000000>");
                dtTime = DateTime.Now;
            }
        }

        private void server_OnReceive(object sender, ReceiveEventArgs e)
        {
            //object mes = null;
            
            //lock (message)
            //{
                try
                {
                    Message message = null;
                    string text = "";
                    text = string.Format("recv: <--- {0}", e.Read());
                    WriteToLog(text);
                    if (null != protocol)
                        message = protocol.Parse(e.Read());
                    else
                    {
                        message = new Message(e.Read());
                    }

                    //过滤收到重复的报文,流水号0000除外
                    //if (message.Parameters["SeqNo"] != "0000")
                    //{
                    //    if (lastSeqNo != message.Parameters["SeqNo"])
                    //    {
                            //text = string.Format("recv: <--- {0}", e.Read());
                            //WriteToLog(text);

                            if (message.Parsed)
                                DispatchState(message.Command, message.Parameters);
                        //}
                        //lastSeqNo = message.Parameters["SeqNo"];
                    //}
                    //else
                    //{
                    //    //text = string.Format("recv: <--- {0}", e.Read());
                    //    //WriteToLog(text);

                    //    if (message.Parsed)
                    //        DispatchState(message.Command, message.Parameters);
                    //}

                    dtTime = DateTime.Now;
                }

                catch (Exception ex)
                {
                    Logger.Debug("server_OnReceive Error " + ex.Message.ToString());
                }
            //}
        }
        private void server_OnConnect(object sender, SocketEventArgs e)
        {
            string ClientIP = e.RemoteAddress;
            string text = string.Format("recv: <--- {0}", "Connection established");
            WriteToLog(text);
            DispatchState("Connect", true);
            Logger.Info("Crane BOX：Connected");
        }
        private void server_OnDisconnect(object sender, SocketEventArgs e)
        {
            string ClientIP = e.RemoteAddress;
            string text = string.Format("recv: <--- {0}", "Connection lost");
            WriteToLog(text);
            Logger.Info("Crane BOX：Disconnected");
            DispatchState("Disconnect", false);
        }
        public override void Release()
        {
            server.StopListen();
        }

        public override void Start()
        {
            server.StartListen(ip, port);
        }

        public override void Stop()
        {
            server.StopListen();
        }

        public override object Read(string itemName)
        {
            throw new Exception("TCPService未实现Read方法，请用System.Net.Sockets.TCPClient类发送TCP消息。");
        }

        public override bool Write(string itemName, object state)
        {
            string text = string.Format("send: ---> {0}", (string)state);
            WriteToLog(text);

            //if(server.OnlineCount)

            server.Write(ip + ":" + port.ToString(), (string)state);
            return true;
            //throw new Exception("TCPService未实现Write方法，请用System.Net.Sockets.TCPClient类发送TCP消息。");
            //TcpClient tcpClient = new TcpClient();
            //tcpClient.Connect(IPAddress.Parse(ip), port);

            //NetworkStream ns = tcpClient.GetStream();


            //if (ns.CanWrite)
            //{
            //    Byte[] sendBytes = Encoding.UTF8.GetBytes(state.ToString());
            //    ns.Write(sendBytes, 0, sendBytes.Length);
            //}           

            //ns.Close();
            //tcpClient.Close();

        }

        private void WriteToLog(string Message)
        {
            if (!System.IO.Directory.Exists("Crane"))
                System.IO.Directory.CreateDirectory("Crane");
            string path = "Crane/" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            System.IO.File.AppendAllText(path, string.Format("{0} :  {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm"), Message + "\r\n"));
        }
    }
}
