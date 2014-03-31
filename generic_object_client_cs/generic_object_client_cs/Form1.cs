//-----------------------------------------------------------------------
// <copyright file="Form1.cs" company="Atmel">
//     Copyright (c) Atmel. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace generic_object_client_cs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Windows.Forms;

    /// <summary>
    /// The GUI for the example client application.
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
        /// Initializes a new instance of the <see cref="FormMain" /> class.
        /// </summary>
        public FormMain()
        {
            this.InitializeComponent();
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
            this.SetChipControlsEnabled(false);
            this.SetObjectControlsEnabled(false);
            this.SetServerControlsEnabled(false);
            this.SetChipDebugControlsEnabled(false);

            // configure client comms handler
            this.client = new Client();
            this.client.chipAttach += new Client.chipAttachDelegate(this.Client_chipAttach);
            this.client.chipDebugData += new Client.chipDebugDataDelegate(this.Client_chipDebugData);
            this.client.chipDebugDataStartConfirm += new Client.chipDebugDataStartConfirmDelegate(this.Client_chipDebugDataStartConfirm);
            this.client.chipDebugDataStopConfirm += new Client.chipDebugDataStopConfirmDelegate(this.Client_chipDebugDataStopConfirm);
            this.client.chipDetach += new Client.chipDetachDelegate(this.Client_chipDetach);
            this.client.chipLoadConfigConfirm += new Client.chipLoadConfigConfirmDelegate(this.Client_chipLoadConfigConfirm);
            this.client.chipObjectTable += new Client.chipObjectTableDelegate(this.Client_chipObjectTable);
            this.client.chipSaveConfigConfirm += new Client.chipSaveConfigConfirmDelegate(this.Client_chipSaveConfigConfirm);
            this.client.objectConfig += new Client.objectConfigDelegate(this.Client_objectConfig);
            this.client.objectInvalidMessageRegisterConfirm += new Client.objectInvalidMessageRegisterConfirmDelegate(this.Client_objectInvalidMessageRegisterConfirm);
            this.client.objectInvalidMessageUnregisterConfirm += new Client.objectInvalidMessageUnregisterConfirmDelegate(this.Client_objectInvalidMessageUnregisterConfirm);
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
        /// Handler for client receiving an object invalid message unregister confirm message.
        /// </summary>
        /// <remarks >
        /// The server sends these messages in response to an object invalid message unregister
        /// command.
        /// </remarks>
        private void Client_objectInvalidMessageUnregisterConfirm()
        {
            this.textBoxInfo.AppendText("object invalid message unregister confirm");
            this.textBoxInfo.AppendText(Environment.NewLine);
        }

        /// <summary>
        /// Handler for client receiving an object invalid message register confirm message.
        /// </summary>
        /// <remarks >
        /// The server sends these messages in response to an object invalid message register
        /// command.
        /// </remarks>
        private void Client_objectInvalidMessageRegisterConfirm()
        {
            this.textBoxInfo.AppendText("object invalid message register confirm");
            this.textBoxInfo.AppendText(Environment.NewLine);
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
        /// Handler for client receiving a chip debug stop confirm message. 
        /// </summary>
        /// <param name="chipDebugStopStatus">the operation status</param>
        private void Client_chipDebugDataStopConfirm(ChipDebugStopStatus chipDebugStopStatus)
        {
            if (chipDebugStopStatus == ChipDebugStopStatus.DEBUG_STOP_OK)
            {
                this.textBoxInfo.AppendText("chip debug stop: succeeded");
                this.textBoxInfo.AppendText(Environment.NewLine);
            }
            else if (chipDebugStopStatus == ChipDebugStopStatus.DEBUG_STOP_FAILED_NOT_RUNNING)
            {
                this.textBoxInfo.AppendText("chip debug stop: failed, debug mode not running");
                this.textBoxInfo.AppendText(Environment.NewLine);
            }
            else
            {
                this.textBoxInfo.AppendText("chip debug stop: unknown return value");
                this.textBoxInfo.AppendText(Environment.NewLine);
            }
        }

        /// <summary>
        /// Handler for client receiving a chip debug start confirm message.
        /// </summary>
        /// <param name="chipDebugStartStatus">the operation status</param>
        private void Client_chipDebugDataStartConfirm(ChipDebugStartStatus chipDebugStartStatus)
        {
            if (chipDebugStartStatus == ChipDebugStartStatus.DEBUG_START_OK)
            {
                this.textBoxInfo.AppendText("chip debug start: succeeded");
                this.textBoxInfo.AppendText(Environment.NewLine);
            }
            else if (chipDebugStartStatus == ChipDebugStartStatus.DEBUG_START_FAILED_ALREADY_RUNNING)
            {
                this.textBoxInfo.AppendText("chip debug start: failed, debug mode already running");
                this.textBoxInfo.AppendText(Environment.NewLine);
            }
            else if (chipDebugStartStatus == ChipDebugStartStatus.DEBUG_START_FAILED_NOT_SUPPORTED)
            {
                this.textBoxInfo.AppendText("chip debug start: failed, debug mode not supported for this chip");
                this.textBoxInfo.AppendText(Environment.NewLine);
            }
            else if (chipDebugStartStatus == ChipDebugStartStatus.DEBUG_START_FAILED_CHIP_NOT_ATTACHED)
            {
                this.textBoxInfo.AppendText("chip debug start: failed, no chip is attached to the server");
                this.textBoxInfo.AppendText(Environment.NewLine);
            }
            else
            {
                this.textBoxInfo.AppendText("chip debug start: unknown return value");
                this.textBoxInfo.AppendText(Environment.NewLine);
            }
        }

        /// <summary>
        /// Handler for client receiving a debug data packet.
        /// </summary>
        /// <param name="buffer">the debug data packet</param>
        private void Client_chipDebugData(ref byte[] buffer)
        {
            if (this.checkBoxDisplayDebugData.Checked)
            {
                this.textBoxInfo.AppendText("debug data: ");
                int bufferSize = buffer.Length;
                for (int i = 0; i < bufferSize; i++)
                {
                    this.textBoxInfo.AppendText(BitConverter.ToString(buffer, i, 1));
                    this.textBoxInfo.AppendText(" ");
                }

                this.textBoxInfo.AppendText(Environment.NewLine);
            }
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

            // configure GUI
            this.SetServerControlsEnabled(true);
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
            this.SetServerControlsEnabled(false);
            this.SetChipDebugControlsEnabled(false);
            this.SetChipControlsEnabled(false);
            this.SetObjectControlsEnabled(false);
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
        /// Handler for client receiving a chip save config confirm message.
        /// </summary>
        /// <param name="operation_succeeded">operation status</param>
        private void Client_chipSaveConfigConfirm(bool operation_succeeded)
        {
            if (operation_succeeded == true)
            {
                this.textBoxInfo.AppendText("chip save config: succeeded");
                this.textBoxInfo.AppendText(Environment.NewLine);
            }
            else
            {
                this.textBoxInfo.AppendText("chip save config: failed");
                this.textBoxInfo.AppendText(Environment.NewLine);
            }
        }

        /// <summary>
        /// Handler for client receiving a chip object table message.
        /// </summary>
        /// <param name="operation_succeeded">flag: the operation succeeded</param>
        /// <param name="objectTable">the chip object table</param>
        private void Client_chipObjectTable(bool operation_succeeded, ref ObjectTableElement[] objectTable)
        {
            if (operation_succeeded == true)
            {
                this.textBoxInfo.AppendText("chip object table");
                this.textBoxInfo.AppendText(Environment.NewLine);

                // display object info for all objects
                for (int i = 0; i < objectTable.Length; i++)
                {
                    // display object number
                    this.textBoxInfo.AppendText("\tObject ");
                    this.textBoxInfo.AppendText(i.ToString());
                    this.textBoxInfo.AppendText(":");
                    this.textBoxInfo.AppendText(Environment.NewLine);

                    // display object info
                    this.textBoxInfo.AppendText("\t\tType ");
                    this.textBoxInfo.AppendText(objectTable[i].type.ToString());
                    this.textBoxInfo.AppendText(Environment.NewLine);
                    this.textBoxInfo.AppendText("\t\tStart position ");
                    this.textBoxInfo.AppendText(objectTable[i].start_position.ToString());
                    this.textBoxInfo.AppendText(Environment.NewLine);
                    this.textBoxInfo.AppendText("\t\tSize ");
                    this.textBoxInfo.AppendText(objectTable[i].size.ToString());
                    this.textBoxInfo.AppendText(Environment.NewLine);
                    this.textBoxInfo.AppendText("\t\tInstances ");
                    this.textBoxInfo.AppendText(objectTable[i].instances.ToString());
                    this.textBoxInfo.AppendText(Environment.NewLine);
                    this.textBoxInfo.AppendText("\t\tReport IDs per instance ");
                    this.textBoxInfo.AppendText(objectTable[i].num_report_ids_per_instance.ToString());
                    this.textBoxInfo.AppendText(Environment.NewLine);
                }
            }
            else
            {
                this.textBoxInfo.AppendText("chip get object table: failed");
                this.textBoxInfo.AppendText(Environment.NewLine);
            }
        }

        /// <summary>
        /// Handler for client receiving a chip load config confirm message.
        /// </summary>
        /// <param name="operation_succeeded">operation status</param>
        private void Client_chipLoadConfigConfirm(bool operation_succeeded)
        {
            if (operation_succeeded == true)
            {
                this.textBoxInfo.AppendText("chip load config: succeeded");
                this.textBoxInfo.AppendText(Environment.NewLine);
            }
            else
            {
                this.textBoxInfo.AppendText("chip load config: failed");
                this.textBoxInfo.AppendText(Environment.NewLine);
            }
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
            this.SetChipControlsEnabled(false);
            this.SetObjectControlsEnabled(false);
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
            this.SetChipControlsEnabled(true);
            this.SetObjectControlsEnabled(true);
            this.SetChipDebugControlsEnabled(true);
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

        /// <summary>
        /// Set the enabled state of the GUI chip controls.
        /// </summary>
        /// <param name="first">flag: controls are enabled</param>
        private void SetChipControlsEnabled(bool first)
        {
            this.groupBoxChip.Enabled = first;
        }

        /// <summary>
        /// Set the enabled state of the GUI object controls.
        /// </summary>
        /// <param name="first">flag: controls are enabled</param>
        private void SetObjectControlsEnabled(bool first)
        {
            this.groupBoxObject.Enabled = first;
        }

        /// <summary>
        /// Set the enabled state of the GUI server controls.
        /// </summary>
        /// <param name="first">flag: controls are enabled</param>
        private void SetServerControlsEnabled(bool first)
        {
            this.groupBoxServer.Enabled = first;
        }

        /// <summary>
        /// Set the enabled state of the GUI chip debug controls.
        /// </summary>
        /// <param name="first">flag: controls are enabled</param>
        private void SetChipDebugControlsEnabled(bool first)
        {
            this.groupBoxChipDebug.Enabled = first;
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
        /// Handler for user clicks on the Server|Show button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends a server show message to the server.
        /// </remarks>
        private void ButtonServerShow_Click(object sender, EventArgs e)
        {
            this.client.sendServerShow();
        }

        /// <summary>
        /// Handler for user clicks on the Server|Hide button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends a server hide message to the server.
        /// </remarks>
        private void ButtonServerHide_Click(object sender, EventArgs e)
        {
            this.client.sendServerHide();
        }

        /// <summary>
        /// Handler for user clicks on the Server|Close button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends a server close message to the server.
        /// </remarks>
        private void ButtonServerClose_Click(object sender, EventArgs e)
        {
            this.client.sendServerClose();
        }

        /// <summary>
        /// Handler for user clicks on the Server|Connection Info button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends a server get connection info message to the server.
        /// </remarks>
        private void ButtonServerConnectionInfo_Click(object sender, EventArgs e)
        {
            this.client.sendServerGetConnectionInfo();
        }

        /// <summary>
        /// Handler for user clicks on the Server|Get Info button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends a server get info message to the server.
        /// </remarks>
        private void ButtonServerGetInfo_Click(object sender, EventArgs e)
        {
            this.client.sendServerGetInfo(this.textBoxServerGetInfo.Text);
        }
        
        /// <summary>
        /// Handler for user clicks on the Chip|Backup button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends a chip backup message to the server.
        /// </remarks>
        private void ButtonChipBackup_Click(object sender, EventArgs e)
        {
            this.client.sendChipBackup();
        }

        /// <summary>
        /// Handler for user clicks on the Chip|Load Config button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Prompts the user to select a config file, then sends a chip load config message to the server.
        /// </remarks>
        private void ButtonChipLoadConfig_Click(object sender, EventArgs e)
        {
            this.openFileDialog.Filter = "Extended config files (*.xcfg)|*.xcfg|Raw config files (*.raw)|*.raw";
            this.openFileDialog.FileName = string.Empty;
            int userChoseFile = (int)this.openFileDialog.ShowDialog();
            if (userChoseFile != (int)DialogResult.Cancel)
            {
                string filename = this.openFileDialog.FileName;
                this.client.sendChipLoadConfig(ref filename);
            }
        }

        /// <summary>
        /// Handler for user clicks on the Chip|Zero Contents button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends a chip zero contents message to the server.
        /// </remarks>
        private void ButtonChipZeroContents_Click(object sender, EventArgs e)
        {
            this.client.sendChipZeroContents();
        }

        /// <summary>
        /// Handler for user clicks on the Chip|Calibrate button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends a chip calibrate message to the server.
        /// </remarks>
        private void ButtonChipCalibrate_Click(object sender, EventArgs e)
        {
            this.client.sendChipCalibrate();
        }

        /// <summary>
        /// Handler for user clicks on the Chip|Save Config button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Prompts the user to select a config file, then sends a chip save config message to the server.
        /// </remarks>
        private void ButtonChipSaveConfig_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.Filter = "Extended config files (*.xcfg)|*.xcfg|Raw config files (*.raw)|*.raw";
            this.saveFileDialog.FileName = string.Empty;
            int userChoseFile = (int)this.saveFileDialog.ShowDialog();
            if (userChoseFile != (int)DialogResult.Cancel)
            {
                string filename = this.saveFileDialog.FileName;
                this.client.sendChipSaveConfig(filename);
            }
        }

        /// <summary>
        /// Handler for user clicks on the Chip|Reset button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends a chip reset message to the server.
        /// </remarks>
        private void ButtonChipReset_Click(object sender, EventArgs e)
        {
            this.client.sendChipReset();
        }

        /// <summary>
        /// Handler for user clicks on the Chip|Object Table button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends a chip get object table message to the server.
        /// </remarks>
        private void ButtonChipObjectTable_Click(object sender, EventArgs e)
        {
            this.client.sendChipGetObjectTable();
        }

        /// <summary>
        /// Handler for clicks on the Chip Debug|Debug Start button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends a chip debug start message to the server. The server will respond
        /// with a chip debug start confirm message, and may then send chip debug
        /// data messages until sent a chip debug data stop command.
        /// </remarks>
        private void ButtonChipDebugStart_Click(object sender, EventArgs e)
        {
            this.client.sendChipDebugStart((byte)this.GetHardlimitedValue(this.maskedTextBoxDebugMode));
        }

        /// <summary>
        /// Handler for clicks on the Chip Debug|Debug Stop button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends a chip debug stop message to the server. The server will respond
        /// with a chip debug data stop message. This will stop any more debug data
        /// messages from being sent.
        /// </remarks>
        private void ButtonChipDebugStop_Click(object sender, EventArgs e)
        {
            this.client.sendChipDebugStop();
        }

        /// <summary>
        /// Gets a numeric value from a masked text box, hard-limited to the range 0..255.
        /// </summary>
        /// <param name="maskedTextBox">the masked text box from which to get the value</param>
        /// <returns>the numeric value</returns>
        private ushort GetHardlimitedValue(MaskedTextBox maskedTextBox)
        {
            ushort rtnval = 0;

            // trap empty box
            if (maskedTextBox.Text == string.Empty)
            {
                return 0;
            }

            // get input value
            int value = int.Parse(maskedTextBox.Text);

            // hardlimit return value
            if (value < 255)
            {
                rtnval = (ushort)value;
            }
            else
            {
                rtnval = 255;
            }

            return rtnval;
        }

        /// <summary>
        /// Handler for clicks on the Object|Invalid Message Register button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends an object invalid message register command to the server. This will
        /// cause the server to send any invalid messages to this client. The client
        /// can send an object invalid message unregister command to the server to
        /// stop receiving such messages.
        /// </remarks>
        private void ButtonObjectInvalidMessageRegister_Click(object sender, EventArgs e)
        {
            this.client.sendObjectInvalidMessageRegister();
        }

        /// <summary>
        /// Handler for clicks on the Object|Invalid Message Unregister button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends an object invalid message unregister command to the server. This will
        /// stop the server from sending the client invalid messages.
        /// </remarks>
        private void ButtonObjectInvalidMessageUnregister_Click(object sender, EventArgs e)
        {
            this.client.sendObjectInvalidMessageUnregister();
        }

        /// <summary>
        /// Handler for user clicks on the Object|Get button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends an object get message to the server for the specified object type.
        /// </remarks>
        private void ButtonObjectGet_Click_1(object sender, EventArgs e)
        {
            this.client.sendObjectGet((int)this.numericUpDownObjectType.Value);
        }

        /// <summary>
        /// Handler for user clicks on the Object|Set button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends an object set message to the server. The message payload
        /// consists of an example of setting some arbitrary values.
        /// </remarks>
        private void ButtonObjectSet_Click_1(object sender, EventArgs e)
        {
            // get a buffer to contain some arbitrary values
            byte[] data = new byte[3];
            data[0] = 1;
            data[1] = 2;
            data[2] = 3;

            // get object index from GUI control
            int objectType = (int)this.numericUpDownObjectType.Value;

            // set object by type
            this.client.sendObjectSet(objectType, ref data);
        }

        /// <summary>
        /// Handler for user clicks on the Object|Register button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends an object register message to the server for the specified object type.
        /// </remarks>
        private void ButtonObjectRegister_Click_1(object sender, EventArgs e)
        {
            this.client.sendObjectRegister((int)this.numericUpDownObjectType.Value);
        }

        /// <summary>
        /// Handler for user clicks on the Object|Unregister button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends an object unregister message to the server for the specified object type.
        /// </remarks>
        private void ButtonObjectUnregister_Click_1(object sender, EventArgs e)
        {
            this.client.sendObjectUnregister((int)this.numericUpDownObjectType.Value);
        }

        /// <summary>
        /// Handler for user clicks on the Object|Get by index button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends an object get by index message to the server for the specified object index.
        /// </remarks>
        private void ButtonObjectGetByIndex_Click(object sender, EventArgs e)
        {
            this.client.sendObjectGetByIndex((int)this.numericUpDownObjectIndex.Value);
        }

        /// <summary>
        /// Handler for user clicks on the Object|Set by index button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends an object set by index message to the server. The message payload
        /// consists of an example of setting some arbitrary values.
        /// </remarks>
        private void ButtonObjectSetByIndex_Click(object sender, EventArgs e)
        {
            // get a buffer to contain some arbitrary values
            byte[] data = new byte[3];
            data[0] = 4;
            data[1] = 5;
            data[2] = 6;

            // get object index from GUI control
            int objectIndex = (int)this.numericUpDownObjectIndex.Value;

            // set object by index
            this.client.sendObjectSetByIndex(objectIndex, ref data);
        }

        /// <summary>
        /// Handler for user clicks on the Object|Register by index button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends an object register by index message to the server for the specified object index.
        /// </remarks>
        private void ButtonObjectRegisterByIndex_Click(object sender, EventArgs e)
        {
            this.client.sendObjectRegisterByIndex((int)this.numericUpDownObjectIndex.Value);
        }

        /// <summary>
        /// Handler for user clicks on the Object|Unregister by index button.
        /// </summary>
        /// <param name="sender">The parameter is not used.</param>
        /// <param name="e">The parameter is not used.</param>
        /// <remarks>
        /// Sends an object unregister by index message to the server for the specified object index.
        /// </remarks>
        private void ButtonObjectUnregisterByIndex_Click(object sender, EventArgs e)
        {
            this.client.sendObjectUnregisterByIndex((int)this.numericUpDownObjectIndex.Value);
        }
    }
}
