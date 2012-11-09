using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;

namespace Proshot.CommandServer
{
    /// <summary>
    /// The class that contains some methods and properties to manage the remote clients.
    /// </summary>
    public class ClientManager
    {
        /// <summary>
        /// Gets the IP address of connected remote client.This is 'IPAddress.None' if the client is not connected.
        /// </summary>
        public IPAddress IP
        {
            get
            {
                if ( this.socket != null)
                    return ( (IPEndPoint)this.socket.RemoteEndPoint ).Address;
                else
                    return IPAddress.None;
            }
        }
        /// <summary>
        /// Gets the port number of connected remote client.This is -1 if the client is not connected.
        /// </summary>
        public int Port
        {
            get
            {
                if ( this.socket != null)
                    return ( (IPEndPoint)this.socket.RemoteEndPoint ).Port;
                else
                    return -1;
            }
        }
        /// <summary>
        /// [Gets] The value that specifies the remote client is connected to this server or not.
        /// </summary>
        public bool Connected
        {
            get
            {
                if ( this.socket != null )
                    return this.socket.Connected;
                else
                    return false;
            }
        }

        private Socket socket;
        private string clientName;
        /// <summary>
        /// The name of remote client.
        /// </summary>
        public string ClientName
        {
            get { return this.clientName; }
            set { this.clientName = value; }
        }
        NetworkStream networkStream;
        private BackgroundWorker bwReceiver;

        #region Constructor
        /// <summary>
        /// Creates an instance of ClientManager class to comunicate with remote clients.
        /// </summary>
        /// <param name="clientSocket">The socket of ClientManager.</param>
        public ClientManager(Socket clientSocket)
        {
            this.socket = clientSocket;
            this.networkStream = new NetworkStream(this.socket);
            this.bwReceiver = new BackgroundWorker();
            this.bwReceiver.DoWork += new DoWorkEventHandler(StartReceive);
            this.bwReceiver.RunWorkerAsync();
        } 
        #endregion

        #region Private Methods
        private void StartReceive(object sender , DoWorkEventArgs e)
        {
            while ( this.socket.Connected )
            {
                //Read the command's Type.
                byte [] buffer = new byte [4];
                int readBytes = this.networkStream.Read(buffer , 0 , 4);
                if ( readBytes == 0 )
                    break;
                CommandType cmdType = (CommandType)( BitConverter.ToInt32(buffer , 0) );

                //Read the command's target size.
                string cmdTarget = "";
                buffer = new byte [4];
                readBytes = this.networkStream.Read(buffer , 0 , 4);
                if ( readBytes == 0 )
                    break;
                int ipSize = BitConverter.ToInt32(buffer , 0);
                
                //Read the command's target.
                buffer = new byte [ipSize];
                readBytes = this.networkStream.Read(buffer , 0 , ipSize);
                if ( readBytes == 0 )
                    break;
                cmdTarget = System.Text.Encoding.ASCII.GetString(buffer);

                //Read the command's MetaData size.
                string cmdMetaData = "";
                buffer = new byte [4];
                readBytes = this.networkStream.Read(buffer , 0 , 4);
                if ( readBytes == 0 )
                    break;
                int metaDataSize = BitConverter.ToInt32(buffer , 0);

                //Read the command's Meta data.
                buffer = new byte [metaDataSize];
                readBytes = this.networkStream.Read(buffer , 0 , metaDataSize);
                if ( readBytes == 0 )
                    break;
                cmdMetaData = System.Text.Encoding.Unicode.GetString(buffer);

                Command cmd = new Command(cmdType , IPAddress.Parse(cmdTarget) , cmdMetaData);
                cmd.SenderIP = this.IP;
                if ( cmd.CommandType == CommandType.ClientLoginInform )
                    cmd.SenderName = cmd.MetaData.Split(new char [] { ':' }) [1];
                else
                    cmd.SenderName = this.ClientName;
                this.OnCommandReceived(new CommandEventArgs(cmd));
            }
            this.OnDisconnected(new ClientEventArgs(this.socket));
            this.Disconnect();
        }

        private void bwSender_RunWorkerCompleted(object sender , RunWorkerCompletedEventArgs e)
        {
            if ( !e.Cancelled && e.Error == null && ( (bool)e.Result ) )
                this.OnCommandSent(new EventArgs());
            else
                this.OnCommandFailed(new EventArgs());

            ( (BackgroundWorker)sender ).Dispose();
            GC.Collect();
        }

        private void bwSender_DoWork(object sender , DoWorkEventArgs e)
        {
            Command cmd = (Command)e.Argument;
            e.Result = this.SendCommandToClient(cmd);
        }

        //This Semaphor is to protect the critical section from concurrent access of sender threads.
        System.Threading.Semaphore semaphor = new System.Threading.Semaphore(1 , 1);
        private bool SendCommandToClient(Command cmd)
        {

            try
            {
                semaphor.WaitOne();
                //Type
                byte [] buffer = new byte [4];
                buffer = BitConverter.GetBytes((int)cmd.CommandType);
                this.networkStream.Write(buffer , 0 , 4);
                this.networkStream.Flush();

                //Sender IP
                byte [] senderIPBuffer = Encoding.ASCII.GetBytes(cmd.SenderIP.ToString());
                buffer = new byte [4];
                buffer = BitConverter.GetBytes(senderIPBuffer.Length);
                this.networkStream.Write(buffer , 0 , 4);
                this.networkStream.Flush();
                this.networkStream.Write(senderIPBuffer , 0 , senderIPBuffer.Length);
                this.networkStream.Flush();
                
                //Sender Name
                byte [] senderNameBuffer = Encoding.Unicode.GetBytes(cmd.SenderName.ToString());
                buffer = new byte [4];
                buffer = BitConverter.GetBytes(senderNameBuffer.Length);
                this.networkStream.Write(buffer , 0 , 4);
                this.networkStream.Flush();
                this.networkStream.Write(senderNameBuffer , 0 , senderNameBuffer.Length);
                this.networkStream.Flush();
                
                //Target
                byte [] ipBuffer = Encoding.ASCII.GetBytes(cmd.Target.ToString());
                buffer = new byte [4];
                buffer = BitConverter.GetBytes(ipBuffer.Length);
                this.networkStream.Write(buffer , 0 , 4);
                this.networkStream.Flush();
                this.networkStream.Write(ipBuffer , 0 , ipBuffer.Length);
                this.networkStream.Flush();

                //Meta Data.
                if ( cmd.MetaData == null || cmd.MetaData == "" )
                    cmd.MetaData = "\n";

                byte [] metaBuffer = Encoding.Unicode.GetBytes(cmd.MetaData);
                buffer = new byte [4];
                buffer = BitConverter.GetBytes(metaBuffer.Length);
                this.networkStream.Write(buffer , 0 , 4);
                this.networkStream.Flush();
                this.networkStream.Write(metaBuffer , 0 , metaBuffer.Length);
                this.networkStream.Flush();

                semaphor.Release();
                return true;
            }
            catch
            {
                semaphor.Release();
                return false;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sends a command to the remote client if the connection is alive.
        /// </summary>
        /// <param name="cmd">The command to send.</param>
        public void SendCommand(Command cmd)
        {
            if ( this.socket != null && this.socket.Connected )
            {
                BackgroundWorker bwSender = new BackgroundWorker();
                bwSender.DoWork += new DoWorkEventHandler(bwSender_DoWork);
                bwSender.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwSender_RunWorkerCompleted);
                bwSender.RunWorkerAsync(cmd);
            }
            else
                this.OnCommandFailed(new EventArgs());
        }

        

        /// <summary>
        /// Disconnect the current client manager from the remote client and returns true if the client had been disconnected from the server.
        /// </summary>
        /// <returns>True if the remote client had been disconnected from the server,otherwise false.</returns>
        public bool Disconnect()
        {
            if (this.socket != null && this.socket.Connected )
            {
                try
                {
                    this.socket.Shutdown(SocketShutdown.Both);
                    this.socket.Close();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
                return true;
        } 
        #endregion

        #region Events
        /// <summary>
        /// Occurs when a command received from a remote client.
        /// </summary>
        public event CommandReceivedEventHandler CommandReceived;
        /// <summary>
        /// Occurs when a command received from a remote client.
        /// </summary>
        /// <param name="e">Received command.</param>
        protected virtual void OnCommandReceived(CommandEventArgs e)
        {
            if ( CommandReceived != null )
                CommandReceived(this , e);
        }

        /// <summary>
        /// Occurs when a command had been sent to the remote client successfully.
        /// </summary>
        public event CommandSentEventHandler CommandSent;
        /// <summary>
        /// Occurs when a command had been sent to the remote client successfully.
        /// </summary>
        /// <param name="e">The sent command.</param>
        protected virtual void OnCommandSent(EventArgs e)
        {
            if ( CommandSent != null )
                CommandSent(this , e);
        }

        /// <summary>
        /// Occurs when a command sending action had been failed.This is because disconnection or sending exception.
        /// </summary>
        public event CommandSendingFailedEventHandler CommandFailed;
        /// <summary>
        /// Occurs when a command sending action had been failed.This is because disconnection or sending exception.
        /// </summary>
        /// <param name="e">The sent command.</param>
        protected virtual void OnCommandFailed(EventArgs e)
        {
            if ( CommandFailed != null )
                CommandFailed(this , e);
        }

        /// <summary>
        /// Occurs when a client disconnected from this server.
        /// </summary>
        public event DisconnectedEventHandler Disconnected;
        /// <summary>
        /// Occurs when a client disconnected from this server.
        /// </summary>
        /// <param name="e">Client information.</param>
        protected virtual void OnDisconnected(ClientEventArgs e)
        {
            if ( Disconnected != null )
                Disconnected(this , e);
        }

        #endregion
    }
}
