﻿//-----------------------------------------------------------------------
// <copyright file="Form.cs" company="Atmel">
//     Copyright (c) Atmel. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace maXTouch.ObjClinet
{
    /// <summary>
    /// FormMain
    /// </summary>
    public partial class FormMain : Form
    {
        /// <summary>
        /// Define from Windows header; needed for intercepting messages to this app.
        /// </summary>
        private const int WmCopydata = 0x4a;

        /// <summary>
        /// Wrapper for client side of client-server communications via Windows messaging. 
        /// </summary>
        private Client client;

        /// <summary>
        /// Flag: the application is shutting down.
        /// </summary>
        private bool appIsShuttingDown;

        /// <summary>
        /// Attributes
        /// </summary>
        private TouchColor touchColor;                  // Color generator
        private CollectionOfStrokes FinishedStrokes;    // Collection of finished strokes
        private CollectionOfStrokes ActiveStrokes;      // Collection of active strokes, currently being drawn by the user

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMain" /> class.
        /// </summary>
        public FormMain()
        {

            this.InitializeComponent();

            // Create data members, color generator and stroke collections.
            touchColor = new TouchColor();
            ActiveStrokes = new CollectionOfStrokes();
            FinishedStrokes = new CollectionOfStrokes();

            // Setup handlers
            Paint += new PaintEventHandler(this.OnPaintHandler);
        }

        /// <summary>
        /// Application handler for windows WM_COPYDATA messages.
        /// </summary>
        /// <param name="m">received message</param>
        /// <remarks>
        /// This is the point at which we receive all server messages. Pass them on to the
        /// client object for processing.
        /// </remarks>
        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            switch (m.Msg)
            {
                case WmCopydata:
                    // test flag: app is shutting down
                    if (!this.appIsShuttingDown)
                    {
                        this.client.handleWmCopyData(ref m);
                    }

                    break;
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Form load handler.
        /// </summary>
        /// <param name="sender">sender parameter</param>
        /// <param name="e">event parameter</param>
        /// <remarks>
        /// Allocates and configures resources.
        /// </remarks>
        private void FormMain_Load(object sender, EventArgs e)
        {
            // configure application
            this.Text = Application.ProductName;
            this.Text += " (C#) v";
            this.Text += Application.ProductVersion;

            // configure GUI
            //this.SetChipControlsEnabled(false);
            //this.SetObjectControlsEnabled(false);
            //this.SetServerControlsEnabled(false);
            //this.SetChipDebugControlsEnabled(false);

            // configure client comms handler
            this.client = new Client();
            this.client.chipAttach += new Client.chipAttachDelegate(this.Client_chipAttach);
            //this.client.chipDebugData += new Client.chipDebugDataDelegate(this.Client_chipDebugData);
            //this.client.chipDebugDataStartConfirm += new Client.chipDebugDataStartConfirmDelegate(this.Client_chipDebugDataStartConfirm);
            //this.client.chipDebugDataStopConfirm += new Client.chipDebugDataStopConfirmDelegate(this.Client_chipDebugDataStopConfirm);
            this.client.chipDetach += new Client.chipDetachDelegate(this.Client_chipDetach);
            //this.client.chipLoadConfigConfirm += new Client.chipLoadConfigConfirmDelegate(this.Client_chipLoadConfigConfirm);
            //this.client.chipObjectTable += new Client.chipObjectTableDelegate(this.Client_chipObjectTable);
            //this.client.chipSaveConfigConfirm += new Client.chipSaveConfigConfirmDelegate(this.Client_chipSaveConfigConfirm);
            this.client.objectConfig += new Client.objectConfigDelegate(this.Client_objectConfig);
            //this.client.objectInvalidMessageRegisterConfirm += new Client.objectInvalidMessageRegisterConfirmDelegate(this.Client_objectInvalidMessageRegisterConfirm);
            //this.client.objectInvalidMessageUnregisterConfirm += new Client.objectInvalidMessageUnregisterConfirmDelegate(this.Client_objectInvalidMessageUnregisterConfirm);
            this.client.objectMessage += new Client.objectMessageDelegate(this.Client_objectMessage);
            this.client.serverConnectionInfo += new Client.serverConnectionInfoDelegate(this.Client_serverConnectionInfo);
            this.client.serverInfo += new Client.serverInfoDelegate(this.Client_serverInfo);
            this.client.serverDetach += new Client.serverDetachDelegate(this.Client_serverDetach);
            this.client.serverPong += new Client.serverPongDelegate(this.Client_serverPong);

            // configure timer to send client ping messages
            this.timerClientPing.Interval = 100;
            this.timerClientPing.Enabled = true;
        }

        /// <summary>
        /// Handler for client receiving an object xxx message.
        /// </summary>
        /// <param name="data">object config information</param>
        private void Client_objectConfig(ref byte[] data)
        {
            this.textBoxInfo.AppendText("object config: type: T");
            this.textBoxInfo.AppendText(data[0].ToString());
            this.textBoxInfo.AppendText(" contents: ");
            int bufferSize = data.Length;
            for (int i = 1; i <= bufferSize - 1; i++)
            {
                this.textBoxInfo.AppendText(BitConverter.ToString(data, i, 1));
                this.textBoxInfo.AppendText(" ");
            }

            this.textBoxInfo.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// Handler for client receiving a chip attach message.
        /// </summary>
        /// <param name="buffer">received chip information</param>
        private void Client_chipAttach(ref byte[] buffer)
        {
            // user info
            this.textBoxInfo.AppendText("chip attach");
            this.textBoxInfo.AppendText(Environment.NewLine);
            this.textBoxInfo.AppendText("\t");
            this.textBoxInfo.AppendText("Family ID: 0x");
            this.textBoxInfo.AppendText(BitConverter.ToString(buffer, 0, 1));
            this.textBoxInfo.AppendText(Environment.NewLine);
            this.textBoxInfo.AppendText("\t");
            this.textBoxInfo.AppendText("Variant ID: 0x");
            this.textBoxInfo.AppendText(BitConverter.ToString(buffer, 1, 1));
            this.textBoxInfo.AppendText(Environment.NewLine);
            this.textBoxInfo.AppendText("\t");
            this.textBoxInfo.AppendText("Version: 0x");
            this.textBoxInfo.AppendText(BitConverter.ToString(buffer, 2, 1));
            this.textBoxInfo.AppendText(Environment.NewLine);
            this.textBoxInfo.AppendText("\t");
            this.textBoxInfo.AppendText("Build: 0x");
            this.textBoxInfo.AppendText(BitConverter.ToString(buffer, 3, 1));
            this.textBoxInfo.AppendText(Environment.NewLine);
            this.textBoxInfo.AppendText("\t");
            this.textBoxInfo.AppendText("Matrix X Size: ");
            this.textBoxInfo.AppendText(buffer[4].ToString());
            this.textBoxInfo.AppendText(Environment.NewLine);
            this.textBoxInfo.AppendText("\t");
            this.textBoxInfo.AppendText("Matrix Y Size: ");
            this.textBoxInfo.AppendText(buffer[5].ToString());
            this.textBoxInfo.AppendText(Environment.NewLine);
            this.textBoxInfo.AppendText("\t");
            this.textBoxInfo.AppendText("Number of Object Table Elements: ");
            this.textBoxInfo.AppendText(buffer[6].ToString());
            this.textBoxInfo.AppendText(Environment.NewLine);

            // configure GUI
            //this.SetChipControlsEnabled(true);
            //this.SetObjectControlsEnabled(true);
            //this.SetChipDebugControlsEnabled(true);
        }

        /// <summary>
        /// Handler for client receiving a chip detach message.
        /// </summary>
        private void Client_chipDetach()
        {
            // user info
            this.textBoxInfo.AppendText("chip detach");
            this.textBoxInfo.AppendText(Environment.NewLine);

            // configure GUI
            //this.SetChipControlsEnabled(false);
            //this.SetObjectControlsEnabled(false);
        }

        /// <summary>
        /// Handler for client receiving an object message message.
        /// </summary>
        /// <param name="data">object message contents</param>
        private void Client_objectMessage(ref byte[] data)
        {
            this.textBoxInfo.AppendText("object message: ");
            int bufferSize = data.Length;
            for (int i = 0; i <= bufferSize - 1; i++)
            {
                this.textBoxInfo.AppendText(BitConverter.ToString(data, i, 1));
                this.textBoxInfo.AppendText(" ");
            }

            this.textBoxInfo.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// Handler for client receiving a server connection info message.
        /// </summary>
        /// <param name="connectionInfo">information about the connection</param>
        private void Client_serverConnectionInfo(ConnectionInfo connectionInfo)
        {
            // user info
            this.textBoxInfo.AppendText("server connection info");
            this.textBoxInfo.AppendText(Environment.NewLine);
            this.textBoxInfo.AppendText("\tconnection type: ");
            switch (connectionInfo.connectionType)
            {
                case ConnectionType.CONNECTION_TYPE_NONE:
                    this.textBoxInfo.AppendText("none");
                    this.textBoxInfo.AppendText(Environment.NewLine);
                    break;

                case ConnectionType.CONNECTION_TYPE_I2C:
                    this.textBoxInfo.AppendText("I2C");
                    this.textBoxInfo.AppendText(Environment.NewLine);
                    this.textBoxInfo.AppendText("\tI2C address: 0x");
                    this.textBoxInfo.AppendText(connectionInfo.i2cAddress.ToString("X"));
                    this.textBoxInfo.AppendText(Environment.NewLine);
                    break;

                case ConnectionType.CONNECTION_TYPE_USB:
                    this.textBoxInfo.AppendText("USB");
                    this.textBoxInfo.AppendText(Environment.NewLine);
                    string msg = string.Format("\tVID: 0x{0,4:X4}", connectionInfo.usbVid);
                    this.textBoxInfo.AppendText(msg);
                    this.textBoxInfo.AppendText(Environment.NewLine);
                    msg = string.Format("\tPID: 0x{0,4:X4}", connectionInfo.usbPid);
                    this.textBoxInfo.AppendText(msg);
                    this.textBoxInfo.AppendText(Environment.NewLine);
                    break;

                case ConnectionType.CONNECTION_TYPE_BRIDGE:
                    this.textBoxInfo.AppendText("bridge");
                    this.textBoxInfo.AppendText(Environment.NewLine);
                    this.textBoxInfo.AppendText("\tname: ");
                    this.textBoxInfo.AppendText(connectionInfo.bridgeName);
                    this.textBoxInfo.AppendText(Environment.NewLine);
                    this.textBoxInfo.AppendText("\tversion: ");
                    this.textBoxInfo.AppendText(connectionInfo.bridgeVersion);
                    this.textBoxInfo.AppendText(Environment.NewLine);
                    break;

                default:
                    this.textBoxInfo.AppendText("unknown");
                    this.textBoxInfo.AppendText(Environment.NewLine);
                    break;
            }
        }

        /// <summary>
        /// Handler for client receiving a server info message.
        /// </summary>
        /// <param name="serverInfo">the message</param>
        private void Client_serverInfo(string serverInfo)
        {
            this.textBoxInfo.AppendText("server info: ");
            this.textBoxInfo.AppendText(serverInfo);
            this.textBoxInfo.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// Handler for client receiving a server detach message.
        /// </summary>
        private void Client_serverDetach()
        {
            this.textBoxInfo.AppendText("server detach");
            this.textBoxInfo.AppendText(Environment.NewLine);

            // server has detached, so start pinging again to find a new one
            this.timerClientPing.Enabled = true;

            // configure GUI
            //this.SetServerControlsEnabled(false);
            //this.SetChipDebugControlsEnabled(false);
            //this.SetChipControlsEnabled(false);
            //this.SetObjectControlsEnabled(false);
        }

        /// <summary>
        /// Handler for client receiving a server pong message.
        /// </summary>
        /// <param name="serverName">server name</param>
        /// <param name="serverVersion">server version</param>
        private void Client_serverPong(string serverName, string serverVersion)
        {
            // user info
            this.textBoxInfo.AppendText("server pong");
            this.textBoxInfo.AppendText(Environment.NewLine);
            this.textBoxInfo.AppendText("\t");
            this.textBoxInfo.AppendText("name: ");
            this.textBoxInfo.AppendText(serverName);
            this.textBoxInfo.AppendText(Environment.NewLine);
            this.textBoxInfo.AppendText("\t");
            this.textBoxInfo.AppendText("version: ");
            this.textBoxInfo.AppendText(serverVersion);
            this.textBoxInfo.AppendText(Environment.NewLine);

            // got pong response, so stop the ping timer
            this.timerClientPing.Enabled = false;

            // attach to the server
            string productName = Application.ProductName;
            string productVersion = "v" + Application.ProductVersion;
            this.client.sendClientAttach(productName, productVersion);

            // initially regist T100 object message
            this.client.sendObjectRegister((int)100);

            // configure GUI
            //this.SetServerControlsEnabled(true);
        }

        /// <summary>
        /// Handler for timeout events from the timerClientPing timer.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends a client ping message to the server. The timer is started when this application
        /// starts, or when the server detaches. It is stopped on receiving a server pong message
        /// from the server.
        /// </remarks>
        private void TimerClientPing_Tick(object sender, EventArgs e)
        {
            this.client.sendClientPing((int)Handle);
        }

        /// <summary>
        /// Form closing handler.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sets 'shutting down' flag to suppress message processing when
        /// app is closing.
        /// </remarks>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            // set flag: app is shutting down
            this.appIsShuttingDown = true;
        }

        // OnPaint event handler.
        // in:
        //      sender      object that has sent the event
        //      e           paint event arguments
        private void OnPaintHandler(object sender, PaintEventArgs e)
        {
            // Full redraw: draw complete collection of finished strokes and
            // also all the strokes that are currently in drawing.
            //FinishedStrokes.Draw(e.Graphics);
            //ActiveStrokes.Draw(e.Graphics);

        }

    }

    /// <summary>
    /// Color generator: assigns a color to the new stroke. 
    /// </summary>
    public class TouchColor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TouchColor()
        {
        }

        /// <summary>
        /// Returns color for the newly started stroke.
        /// in:
        ///      primary         boolean, whether the contact is the primary contact
        /// returns:
        ///      color of the stroke
        /// </summary>
        public Color GetColor(bool primary)
        {
            if (primary)
            {
                // The primary contact is drawn in black.
                return Color.LawnGreen;
            }
            else
            {
                // Take current secondary color.
                Color color = secondaryColors[idx];

                // Move to the next color in the array.
                idx = (idx + 1) % secondaryColors.Length;

                return color;
            }
        }

        /// <summary>
        /// Attributes
        /// </summary>
        static private Color[] secondaryColors =    // Secondary colors
        {
            Color.Red,
            Color.Black,
            Color.Blue,
            Color.Cyan,
            Color.Magenta,
            Color.Yellow
        };
        private int idx = 0;                // Rotating secondary color index
    }
}
