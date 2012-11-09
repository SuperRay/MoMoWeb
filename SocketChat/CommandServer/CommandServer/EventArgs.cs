using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Proshot.CommandServer
{
    /// <summary>
    /// Occurs when a command received from a client.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">The received command object.</param>
    public delegate void CommandReceivedEventHandler(object sender,CommandEventArgs e);

    /// <summary>
    /// Occurs when a command had been sent to the remote client successfully.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">EventArgs.</param>
    public delegate void CommandSentEventHandler(object sender , EventArgs e);

    /// <summary>
    /// Occurs when a command sending action had been failed.This is because disconnection or sending exception.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">EventArgs.</param>
    public delegate void CommandSendingFailedEventHandler(object sender , EventArgs e);

    /// <summary>
    /// The class that contains information about received command.
    /// </summary>
    public class CommandEventArgs : EventArgs
    {
        private Command command;
        /// <summary>
        /// The received command.
        /// </summary>
        public Command Command
        {
            get { return command; }
        }

        /// <summary>
        /// Creates an instance of CommandEventArgs class.
        /// </summary>
        /// <param name="cmd">The received command.</param>
        public CommandEventArgs(Command cmd)
        {
            this.command = cmd;
        }
    }
    /// <summary>
    /// Occurs when a remote client had been disconnected from the server.
    /// </summary>
    /// <param name="sender">Sender.</param>
    /// <param name="e">The client information.</param>
    public delegate void DisconnectedEventHandler(object sender,ClientEventArgs e);
    /// <summary>
    /// Client event args.
    /// </summary>
    public class ClientEventArgs : EventArgs
    {
        private Socket socket;
        /// <summary>
        /// The ip address of remote client.
        /// </summary>
        public IPAddress IP
        {
            get { return ( (IPEndPoint)this.socket.RemoteEndPoint ).Address; }
        }
        /// <summary>
        /// The port of remote client.
        /// </summary>
        public int Port
        {
            get{return ((IPEndPoint)this.socket.RemoteEndPoint).Port;}
        }
        /// <summary>
        /// Creates an instance of ClientEventArgs class.
        /// </summary>
        /// <param name="clientManagerSocket">The socket of server side socket that comunicates with the remote client.</param>
        public ClientEventArgs(Socket clientManagerSocket)
        {
            this.socket = clientManagerSocket;
        }
    }
}
