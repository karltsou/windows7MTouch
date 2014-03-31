//-----------------------------------------------------------------------
// <copyright file="Client.cs" company="Atmel">
//     Copyright (c) Atmel. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace generic_object_client_cs
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;

    /// <summary>
    /// Represents connection type info returned from the server.
    /// </summary>
    public enum ConnectionType
    {
        /// <summary>
        /// no device is attached to the server
        /// </summary>
        CONNECTION_TYPE_NONE,

        /// <summary>
        /// server is connected to device via I2C
        /// </summary>
        CONNECTION_TYPE_I2C,

        /// <summary>
        /// server is connected to device via USB
        /// </summary>
        CONNECTION_TYPE_USB,

        /// <summary>
        /// server is connected to device via a bridge client
        /// </summary>
        CONNECTION_TYPE_BRIDGE
    }

    /// <summary>
    /// Represents the status of a debug start command sent to the server.
    /// </summary>
    public enum ChipDebugStartStatus
    {
        /// <summary>
        /// the operation succeeded
        /// </summary>
        DEBUG_START_OK,

        /// <summary>
        /// the operation failed because the server is already streaming debug data
        /// </summary>
        DEBUG_START_FAILED_ALREADY_RUNNING,

        /// <summary>
        /// the operation failed because the server does not support streaming debug
        /// data for this chip
        /// </summary>
        DEBUG_START_FAILED_NOT_SUPPORTED,

        /// <summary>
        /// the operation failed because no chip is attached to the server 
        /// </summary>
        DEBUG_START_FAILED_CHIP_NOT_ATTACHED
    }

    /// <summary>
    /// Represents the status of a debug stop command sent to the server.
    /// </summary>
    public enum ChipDebugStopStatus
    {
        /// <summary>
        /// the operation succeeded
        /// </summary>
        DEBUG_STOP_OK,

        /// <summary>
        /// the operation failed because the server is not streaming debug data
        /// </summary>
        DEBUG_STOP_FAILED_NOT_RUNNING
    }

    /// <summary>
    /// Wraps up the client side of client-server communications via Windows messaging.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// define from windows header file; needed for intercepting messages sent to this app
        /// </summary>
        private const int WmCopydata = 0x4a;

        // message enumerations
        private const int ChipAttach = 449184888;
        private const int ChipBackup = 443745401;
        private const int ChipCalibrate = 707986858;
        private const int ChipDetach = 445449324;
        private const int ChipDebugData = 807405059;
        private const int ChipDebugStart = 924255895;
        private const int ChipDebugStartConfirm = 2053114340;
        private const int ChipDebugStop = 815400495;
        private const int ChipDebugStopConfirm = 1889732988;
        private const int ChipLoadConfig = 921044600;
        private const int ChipLoadConfigConfirm = 2033650117;
        private const int ChipReset = 377291814;
        private const int ChipSaveConfig = 929826439;
        private const int ChipSaveConfigConfirm = 2050296276;
        private const int ChipZeroContents = 1189283728;
        private const int ChipGetObjectTable = 1568868480;
        private const int ChipObjectTable = 1039206113;
        private const int ClientAttach = 625870163;
        private const int ClientDetach = 622134599;
        private const int ClientPing = 455410828;
        private const int ObjectConfig = 621741388;
        private const int ObjectGet = 375391254;
        private const int ObjectGetByIndex = 1296631751;
        private const uint ObjectInvalidMessageRegister = 3408661701;
        private const uint ObjectInvalidMessageRegisterConfirm = 1072959506;
        private const uint ObjectInvalidMessageUnregister = 3864792488;
        private const uint ObjectInvalidMessageUnregisterConfirm = 1648103669;
        private const int ObjectMessage = 721356219;
        private const int ObjectRegister = 826869307;
        private const int ObjectRegisterByIndex = 2071923180;
        private const int ObjectSet = 377750562;
        private const int ObjectSetByIndex = 1306068947;
        private const int ObjectUnregister = 1063585566;
        private const uint ObjectUnregisterByIndex = 2442529487;
        private const int ServerClose = 554435852;
        private const int ServerConnectionInfo = 1782581553;
        private const int ServerDetach = 641533279;
        private const uint ServerGetConnectionInfo = 2457537232;
        private const int ServerGetInfo = 846071361;
        private const int ServerHide = 468124816;
        private const int ServerInfo = 470287522;
        private const int ServerPong = 472843434;
        private const int ServerShow = 473433271;

        /// <summary>
        /// server application window handle
        /// </summary>
        private static IntPtr serverHandle = IntPtr.Zero;

        /// <summary>
        /// client application window handle
        /// </summary>
        private int clientHandle;

        /// <summary>
        /// flag: we're attached to a server
        /// </summary>
        private bool attachedToServer;

        /// <summary>
        /// Finalizes an instance of the <see cref="Client" /> class.
        /// </summary>
        /// <remarks>
        /// If currently attached to a server, sends it a client detach message.
        /// </remarks>
        ~Client()
        {
            if (this.attachedToServer == true)
            {
                this.sendClientDetach();
            }
        }

        /// <summary>
        /// delegate for chip attach events
        /// </summary>
        /// <param name="buffer">received chip information</param>
        public delegate void chipAttachDelegate(ref byte[] buffer);

        /// <summary>
        /// delegate for chip debug data event
        /// </summary>
        /// <param name="buffer">the chip debug data</param>
        public delegate void chipDebugDataDelegate(ref byte[] buffer);

        /// <summary>
        /// delegate for chip debug start confirm events
        /// </summary>
        /// <param name="chipDebugStartStatus">the operation status</param>
        public delegate void chipDebugDataStartConfirmDelegate(ChipDebugStartStatus chipDebugStartStatus);

        /// <summary>
        /// delegate for chip debug stop confirm events
        /// </summary>
        /// <param name="chipDebugStopStatus">the operation status</param>
        public delegate void chipDebugDataStopConfirmDelegate(ChipDebugStopStatus chipDebugStopStatus);

        /// <summary>
        /// delegate for chip detach events
        /// </summary>
        public delegate void chipDetachDelegate();

        /// <summary>
        /// delegate for chip load config confirm events
        /// </summary>
        /// <param name="operationSucceeded">flag: the operation succeeded</param>
        public delegate void chipLoadConfigConfirmDelegate(bool operationSucceeded);

        /// <summary>
        /// delegate for chip save config confirm events
        /// </summary>
        /// <param name="operationSucceeded">flag: the operation succeeded</param>
        public delegate void chipSaveConfigConfirmDelegate(bool operationSucceeded);

        /// <summary>
        /// delegate for chip object table events
        /// </summary>
        /// <param name="operationSucceeded">flag: the operation succeeded</param>
        /// <param name="objectTable">the chip object table</param>
        public delegate void chipObjectTableDelegate(bool operationSucceeded, ref ObjectTableElement[] objectTable);

        /// <summary>
        /// delegate for object configuration events
        /// </summary>
        /// <param name="data">the object configuration</param>
        public delegate void objectConfigDelegate(ref byte[] data);

        /// <summary>
        /// delegate for object invalid message register confirm events
        /// </summary>
        public delegate void objectInvalidMessageRegisterConfirmDelegate();

        /// <summary>
        /// delegate for object invalid message unregister confirm events
        /// </summary>
        public delegate void objectInvalidMessageUnregisterConfirmDelegate();

        /// <summary>
        /// delegate for object message events
        /// </summary>
        /// <param name="data">the object message</param>
        public delegate void objectMessageDelegate(ref byte[] data);

        /// <summary>
        /// delegate for server connection information events
        /// </summary>
        /// <param name="connectionInfo">the server connection information</param>
        public delegate void serverConnectionInfoDelegate(ConnectionInfo connectionInfo);

        /// <summary>
        /// delegate for server info events
        /// </summary>
        /// <param name="serverInfo">the server info</param>
        public delegate void serverInfoDelegate(string serverInfo);

        /// <summary>
        /// delegate for server detach events
        /// </summary>
        public delegate void serverDetachDelegate();

        /// <summary>
        /// delegate for server pong events
        /// </summary>
        /// <param name="serverName">the server name</param>
        /// <param name="serverVersion">the server version</param>
        public delegate void serverPongDelegate(string serverName, string serverVersion);

        /// <summary>
        /// delegate for calls to enumerate windows
        /// </summary>
        /// <param name="hWnd">window handle</param>
        /// <param name="dummy">dummy argument</param>
        /// <returns>true if the handle is for the server, false otherwise</returns>
        private delegate bool EnumWindowsProc(IntPtr hWnd, int dummy);

        /// <summary>
        /// chip attach event
        /// </summary>
        public event chipAttachDelegate chipAttach;

        /// <summary>
        /// chip debug data event
        /// </summary>
        public event chipDebugDataDelegate chipDebugData;

        /// <summary>
        /// chip debug data start confirm event
        /// </summary>
        public event chipDebugDataStartConfirmDelegate chipDebugDataStartConfirm;

        /// <summary>
        /// chip debug data stop confirm event
        /// </summary>
        public event chipDebugDataStopConfirmDelegate chipDebugDataStopConfirm;

        /// <summary>
        /// chip detach event
        /// </summary>
        public event chipDetachDelegate chipDetach;

        /// <summary>
        /// chip load config confirmation event
        /// </summary>
        public event chipLoadConfigConfirmDelegate chipLoadConfigConfirm;

        /// <summary>
        /// chip save config confirmation event
        /// </summary>
        public event chipSaveConfigConfirmDelegate chipSaveConfigConfirm;

        /// <summary>
        /// chip object table event
        /// </summary>
        public event chipObjectTableDelegate chipObjectTable;

        /// <summary>
        /// object configuration event
        /// </summary>
        public event objectConfigDelegate objectConfig;

        /// <summary>
        /// object invalid message register confirm event
        /// </summary>
        public event objectInvalidMessageRegisterConfirmDelegate objectInvalidMessageRegisterConfirm;

        /// <summary>
        /// object invalid message unregister confirm event
        /// </summary>
        public event objectInvalidMessageUnregisterConfirmDelegate objectInvalidMessageUnregisterConfirm;

        /// <summary>
        /// object message event
        /// </summary>
        public event objectMessageDelegate objectMessage;

        /// <summary>
        /// server connection information event
        /// </summary>
        public event serverConnectionInfoDelegate serverConnectionInfo;

        /// <summary>
        /// server info event
        /// </summary>
        public event serverInfoDelegate serverInfo;

        /// <summary>
        /// server detach event
        /// </summary>
        public event serverDetachDelegate serverDetach;

        /// <summary>
        /// server pong event
        /// </summary>
        public event serverPongDelegate serverPong;

        /// <summary>
        /// Callback function used to test if the server has been found.
        /// </summary>
        /// <param name="hWnd">handle of a top-level window</param>
        /// <param name="dummy">unused parameter</param>
        /// <returns>true if the handle is for the server, false otherwise</returns>
        /// <remarks>
        /// We test for the server by checking if the window title contains appropriate text.
        /// </remarks>
        public static bool EnumerateWindowsProc(IntPtr hWnd, int dummy)
        {
            // the caption of the app we're trying to find (the object server)
            string partialServerCaption = "Object-Based Server";

            // default return value: carry on enumerating   
            bool rtnval = true;

            // get the caption for the window we've been given the handle for
            int length = GetWindowTextLength(hWnd);
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(hWnd, sb, sb.Capacity);

            // see if it's the object server
            string windowText = sb.ToString();
            int partialServerCaptionIndex = windowText.IndexOf(partialServerCaption);
            if (partialServerCaptionIndex != -1)
            {
                // if it is, store server application window handle
                serverHandle = hWnd;

                // return false to stop enumerating windows
                rtnval = false;
            }

            return rtnval;
        }

        /// <summary>
        /// Query: are we attached to the server?
        /// </summary>
        /// <returns>flag: currently attached to server</returns>
        public bool isAttachedToServer()
        {
            return this.attachedToServer;
        }

        /// <summary>
        /// Send a chip backup message to the server.
        /// </summary>
        public void sendChipBackup()
        {
            // debug info
            OutputDebugString("sendChipBackup");

            this.SendWmCopyDataMessage(ChipBackup);
        }

        /// <summary>
        /// Send a chip calibrate message to the server.
        /// </summary>
        public void sendChipCalibrate()
        {
            // debug info
            OutputDebugString("sendChipCalibrate");

            this.SendWmCopyDataMessage(ChipCalibrate);
        }

        /// <summary>
        /// Send a chip debug start message to the server. 
        /// </summary>
        /// <param name="debugMode">the mode to use when generating debug data</param>
        public void sendChipDebugStart(byte debugMode)
        {
            // debug info
            OutputDebugString("sendChipDebugStart");

            // create buffer to hold message payload
            byte[] buffer = new byte[1];
            buffer[0] = (byte)debugMode;

            this.SendWmCopyDataMessage(ChipDebugStart, ref buffer);
        }

        /// <summary>
        /// Send a chip debug stop message to the server.
        /// </summary>
        public void sendChipDebugStop()
        {
            // debug info
            OutputDebugString("sendChipDebugStop");

            this.SendWmCopyDataMessage(ChipDebugStop);
        }

        /// <summary>
        /// Send a chip load config message to the server.
        /// </summary>
        /// <param name="name">name of config file to load</param>
        public void sendChipLoadConfig(ref string name)
        {
            // debug info
            OutputDebugString("sendChipLoadConfig");

            // convert file name to buffer for transmission
            byte[] buffer = null;
            buffer = this.GetBufferFromString(ref name);

            // send message
            this.SendWmCopyDataMessage(ChipLoadConfig, ref buffer);
        }

        /// <summary>
        /// Send a chip reset message to the server.
        /// </summary>
        public void sendChipReset()
        {
            // debug info
            OutputDebugString("sendChipReset");

            this.SendWmCopyDataMessage(ChipReset);
        }

        /// <summary>
        /// Send a chip save config message to the server.
        /// </summary>
        /// <param name="name">name of config file to save</param>
        public void sendChipSaveConfig(string name)
        {
            // debug info
            OutputDebugString("sendChipSaveConfig");

            // convert file name to buffer for transmission
            byte[] buffer = null;
            buffer = this.GetBufferFromString(ref name);

            // send message
            this.SendWmCopyDataMessage(ChipSaveConfig, ref buffer);
        }

        /// <summary>
        /// Send a chip zero contents message to the server.
        /// </summary>
        public void sendChipZeroContents()
        {
            // debug info
            OutputDebugString("sendChipZeroContents");

            this.SendWmCopyDataMessage(ChipZeroContents);
        }

        /// <summary>
        /// Send a chip get object table message to the server.
        /// </summary>
        public void sendChipGetObjectTable()
        {
            // debug info
            OutputDebugString("sendChipGetObjectTable");

            this.SendWmCopyDataMessage(ChipGetObjectTable);
        }

        /// <summary>
        /// Send a client attach message to the server.
        /// </summary>
        /// <param name="name">client name</param>
        /// <param name="version">client version</param>
        public void sendClientAttach(string name, string version)
        {
            // debug info
            OutputDebugString("sendClientAttach");

            // get size of buffer required to hold client name and version info
            int bufferSize = name.Length + version.Length + 2;

            // create buffer to hold strings
            byte[] buffer = new byte[bufferSize];

            // write client name size to buffer
            buffer[0] = (byte)name.Length;

            // write client name to buffer
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            encoding.GetBytes(name, 0, name.Length, buffer, 1);

            // write client version size to buffer
            buffer[name.Length + 1] = (byte)version.Length;

            // write client version to buffer
            encoding.GetBytes(version, 0, version.Length, buffer, name.Length + 2);

            // send message
            this.SendWmCopyDataMessage(ClientAttach, ref buffer);

            // set flag: we're attached to a server
            this.attachedToServer = true;
        }

        /// <summary>
        /// Send a client detach message to the server.
        /// </summary>
        public void sendClientDetach()
        {
            // debug info
            OutputDebugString("sendClientDetach");

            // send message
            this.SendWmCopyDataMessage(ClientDetach);
        }

        /// <summary>
        /// Broadcast a client ping message.
        /// </summary>
        /// <param name="handle">handle of current application</param>
        /// <remarks>
        /// This message is sent to all applications. If a server is running it will respond 
        /// with a server ping message.
        /// </remarks>
        public void sendClientPing(int handle)
        {
            // debug info
            OutputDebugString("sendClientPing");

            // store client application window handle
            this.clientHandle = handle;

            // clear server application window handle; will be filled out by
            // enumWindowsProc() if server is found
            serverHandle = IntPtr.Zero;

            // enumerate all top-level windows, passing a handle to each to
            // enumWindowsProc()
            EnumWindows(EnumerateWindowsProc, 0);

            // if we found the server, send a ping message to it
            if (serverHandle != IntPtr.Zero)
            {
                this.SendWmCopyDataMessage(ClientPing);
            }
        }

        /// <summary>
        /// Send an object get message to the server.
        /// </summary>
        /// <param name="object_type">object type to get</param>
        public void sendObjectGet(int object_type)
        {
            // debug info
            OutputDebugString("sendObjectGet");

            // create buffer to hold message payload
            byte[] buffer = new byte[1];
            buffer[0] = (byte)object_type;

            // send message
            this.SendWmCopyDataMessage(ObjectGet, ref buffer);
        }

        /// <summary>
        /// Send an object get by index message to the server.
        /// </summary>
        /// <param name="index">the object index in the chip object table</param>
        public void sendObjectGetByIndex(int index)
        {
            // debug info
            OutputDebugString("sendObjectGetByIndex");

            // create buffer to hold message payload
            byte[] buffer = new byte[1];
            buffer[0] = (byte)index;

            // send message
            this.SendWmCopyDataMessage(ObjectGetByIndex, ref buffer);
        }

        /// <summary>
        /// Send an object invalid message register message to the server.
        /// </summary>
        public void sendObjectInvalidMessageRegister()
        {
            // debug info
            OutputDebugString("sendObjectInvalidMessageRegister");

            // send message
            this.SendWmCopyDataMessage(ObjectInvalidMessageRegister);
        }

        /// <summary>
        /// Send an object invalid message unregister message to the server.
        /// </summary>
        public void sendObjectInvalidMessageUnregister()
        {
            // debug info
            OutputDebugString("sendObjectInvalidMessageUnregister");

            // send message
            this.SendWmCopyDataMessage(ObjectInvalidMessageUnregister);
        }

        /// <summary>
        /// Send an object register message to the server.
        /// </summary>
        /// <param name="object_type">object type to register</param>
        public void sendObjectRegister(int object_type)
        {
            // debug info
            OutputDebugString("sendObjectRegister");

            // create buffer to hold message payload
            byte[] buffer = new byte[1];
            buffer[0] = (byte)object_type;

            // send message
            this.SendWmCopyDataMessage(ObjectRegister, ref buffer);
        }

        /// <summary>
        /// Send an object register by index message to the server.
        /// </summary>
        /// <param name="object_index">object index to register</param>
        public void sendObjectRegisterByIndex(int object_index)
        {
            // debug info
            OutputDebugString("sendObjectRegisterByIndex");

            // create buffer to hold message payload
            byte[] buffer = new byte[1];
            buffer[0] = (byte)object_index;

            // send message
            this.SendWmCopyDataMessage(ObjectRegisterByIndex, ref buffer);
        }

        /// <summary>
        /// Send an object set message to the server.
        /// </summary>
        /// <param name="object_type">object type to register</param>
        /// <param name="object_data">new object contents</param>
        public void sendObjectSet(int object_type, ref byte[] object_data)
        {
            // debug info
            OutputDebugString("sendObjectSet");

            // create buffer to hold message payload
            byte[] buffer = new byte[object_data.Length + 1];

            // byte[0] = object type
            buffer[0] = (byte)object_type;

            // remaining bytes = object contents
            for (int i = 0; i <= object_data.Length - 1; i++)
            {
                buffer[i + 1] = object_data[i];
            }

            // send message
            this.SendWmCopyDataMessage(ObjectSet, ref buffer);
        }

        /// <summary>
        /// Send an object set by index message to the server.
        /// </summary>
        /// <param name="index">the object index in the chip object table</param>
        /// <param name="object_data">the object data to set</param>
        public void sendObjectSetByIndex(int index, ref byte[] object_data)
        {
            // debug info
            OutputDebugString("sendObjectSetByIndex");

            // create buffer to hold message payload
            byte[] buffer = new byte[object_data.Length + 1];

            // byte[0] = object index
            buffer[0] = (byte)index;

            // remaining bytes = object contents
            for (int i = 0; i <= object_data.Length - 1; i++)
            {
                buffer[i + 1] = object_data[i];
            }

            // send message
            this.SendWmCopyDataMessage(ObjectSetByIndex, ref buffer);
        }

        /// <summary>
        /// Send an object unregister message to the server.
        /// </summary>
        /// <param name="object_type">object type to unregister</param>
        public void sendObjectUnregister(int object_type)
        {
            // debug info
            OutputDebugString("sendObjectUnregister");

            // create buffer to hold message payload
            byte[] buffer = new byte[1];
            buffer[0] = (byte)object_type;

            // send message
            this.SendWmCopyDataMessage(ObjectUnregister, ref buffer);
        }

        /// <summary>
        /// Send an object unregister by index message to the server.
        /// </summary>
        /// <param name="object_index">object index to unregister</param>
        public void sendObjectUnregisterByIndex(int object_index)
        {
            // debug info
            OutputDebugString("sendObjectUnregisterByIndex");

            // create buffer to hold message payload
            byte[] buffer = new byte[1];
            buffer[0] = (byte)object_index;

            // send message
            this.SendWmCopyDataMessage(ObjectUnregisterByIndex, ref buffer);
        }

        /// <summary>
        /// Send a server close message to the server.
        /// </summary>
        public void sendServerClose()
        {
            // debug info
            OutputDebugString("sendServerClose");

            this.SendWmCopyDataMessage(ServerClose);
        }

        /// <summary>
        /// Send a server get connection info message to the server.
        /// </summary>
        public void sendServerGetConnectionInfo()
        {
            // debug info
            OutputDebugString("sendServerGetConnectionInfo");

            this.SendWmCopyDataMessage(ServerGetConnectionInfo);
        }

        /// <summary>
        /// Send a server get info message to the server.
        /// </summary>
        /// <param name="info">the requested info</param>
        public void sendServerGetInfo(string info)
        {
            // debug info
            OutputDebugString("sendServerGetInfo");

            // sanity check          
            Debug.Assert(info.Length < 256, "sendServerGetInfo: info length is too long");

            // create buffer to hold string
            byte[] buffer = new byte[info.Length + 1];

            // write string size to buffer
            buffer[0] = (byte)info.Length;

            // write string to buffer
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            encoding.GetBytes(info, 0, info.Length, buffer, 1);

            this.SendWmCopyDataMessage(ServerGetInfo, ref buffer);
        }

        /// <summary>
        /// Send a server hide message to the server.
        /// </summary>
        public void sendServerHide()
        {
            // debug info
            OutputDebugString("sendServerHide");

            this.SendWmCopyDataMessage(ServerHide);
        }

        /// <summary>
        /// Send a server show message to the server.
        /// </summary>
        public void sendServerShow()
        {
            // debug info
            OutputDebugString("sendServerShow");

            this.SendWmCopyDataMessage(ServerShow);
        }

        /// <summary>
        /// Handle a WM_COPYDATA message sent to the application.
        /// </summary>
        /// <param name="m">received message</param>
        public void handleWmCopyData(ref System.Windows.Forms.Message m)
        {
            CopyData copyData = default(CopyData);
            int handle = 0;

            // get message contents
            copyData = (CopyData)m.GetLParam(typeof(CopyData));
            handle = (int)m.WParam;

            // handle known messages
            // NB: messy handling to convert the received 32-bit value into
            // an unsigned value - a proper C# programmer could probably do
            // this more gracefully.
            long event_id;
            int event_id_integer = (int)copyData.dwData;
            if (event_id_integer < 0)
            {
                event_id = event_id_integer + 4294967296;
            }
            else
            {
                event_id = event_id_integer;
            }

            switch (event_id)
            {
                case ChipAttach:
                    this.ProcessChipAttach(ref copyData);
                    break;
                case ChipDetach:
                    this.ProcessChipDetach();
                    break;
                case ChipDebugData:
                    this.ProcessChipDebugData(ref copyData);
                    break;
                case ChipDebugStartConfirm:
                    this.ProcessChipDebugStartConfirm(ref copyData);
                    break;
                case ChipDebugStopConfirm:
                    this.ProcessChipDebugStopConfirm(ref copyData);
                    break;
                case ChipLoadConfigConfirm:
                    this.ProcessChipLoadConfigConfirm(ref copyData);
                    break;
                case ChipSaveConfigConfirm:
                    this.ProcessChipSaveConfigConfirm(ref copyData);
                    break;
                case ChipObjectTable:
                    this.ProcessChipObjectTable(ref copyData);
                    break;
                case ObjectConfig:
                    this.ProcessObjectConfig(ref copyData);
                    break;
                case ObjectInvalidMessageRegisterConfirm:
                    this.ProcessObjectInvalidMessageRegisterConfirm();
                    break;
                case ObjectInvalidMessageUnregisterConfirm:
                    this.ProcessObjectInvalidMessageUnregisterConfirm();
                    break;
                case ObjectMessage:
                    this.ProcessObjectMessage(ref copyData);
                    break;
                case ServerConnectionInfo:
                    this.ProcessServerConnectionInfo(ref copyData);
                    break;
                case ServerInfo:
                    this.ProcessServerInfo(ref copyData);
                    break;
                case ServerPong:
                    this.ProcessServerPong((IntPtr)handle, ref copyData);
                    break;
                case ServerDetach:
                    this.ProcessServerDetach();
                    break;
            }
        }

        /// <summary>
        /// Send a WM_COPYDATA message to the server.
        /// </summary>
        /// <param name="eventType">event type info</param>
        /// <remarks>
        /// This is a convenience function; it just attaches a dummy buffer to
        /// the event type and calls the function which actually sends the
        /// message.
        /// </remarks>
        protected void SendWmCopyDataMessage(uint eventType)
        {
            byte[] buffer = new byte[1];

            this.SendWmCopyDataMessage(eventType, ref buffer);
        }

        /// <summary>
        /// Send a WM_COPYDATA message to the server.
        /// </summary>
        /// <param name="eventType">event type info</param>
        /// <param name="data">data to send to server; passed via message lpData field</param>
        /// <remarks>
        /// In the message:
        /// dwData is the message ID
        /// cbData is the size of the message contents
        /// lpData points to the message contents
        /// As SendMessage blocks until handled by the receiver, we can use the
        /// input data buffer to hold the message contents.
        /// </remarks>
        protected void SendWmCopyDataMessage(uint eventType, ref byte[] data)
        {
            // get pointer to buffer contents
            int bufferSize = data.Length;
            IntPtr ptrData = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(bufferSize);
            System.Runtime.InteropServices.Marshal.Copy(data, 0, ptrData, bufferSize);

            // fill out WM_COPYDATA message to send to server
            CopyData copyData = default(CopyData);
            copyData.dwData = (UIntPtr)eventType;
            copyData.cbData = data.Length;
            copyData.lpData = ptrData;

            SendMessage((IntPtr)serverHandle, WmCopydata, this.clientHandle, ref copyData);
        }

        /// <summary>
        /// Get the payload from a Windows WM_COPYDATA message.
        /// </summary>
        /// <param name="copyData">Windows WM_COPYDATA message</param>
        /// <returns>The message contents as a Byte array.</returns>
        protected byte[] GetBufferFromCopyData(ref CopyData copyData)
        {
            // the structure cbData field contains the number of bytes in the message data
            int bufferSize = copyData.cbData;

            // get a buffer containing the message data
            byte[] buffer = new byte[bufferSize];
            System.Runtime.InteropServices.Marshal.Copy(copyData.lpData, buffer, 0, bufferSize);

            return buffer;
        }

        /// <summary>
        /// Get a string from a byte buffer.
        /// </summary>
        /// <param name="buffer">the input byte buffer</param>
        /// <param name="index">the index from which to get the string</param>
        /// <returns>the specified string</returns>
        /// <remarks>
        /// Strings are stored as a 1-byte length field, followed by the string contents.
        /// The index is increased to allow calls to be chained together.
        /// </remarks>
        protected string GetString(ref byte[] buffer, ref int index)
        {
            // get return value
            int stringSize = buffer[index];
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            string rtnval = ascii.GetString(buffer, index + 1, stringSize);

            // increase buffer index for next call to this or similar helper function
            index += rtnval.Length + 1;

            return rtnval;
        }

        // utility function for printing debug info
        [DllImport("kernel32", EntryPoint = "OutputDebugStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern void OutputDebugString(string lpOutputString);

        // enumerate all top-level windows on the screen by passing the handle to each
        // window, in turn, to specified callback function
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, int dummy);

        // copy the text of the specified window's title bar into a buffer
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        // retrieves the length, in characters, of the specified window's title bar text
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        // send a Windows message to specified recipient
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hwnd, int wMsg, int wParam, ref CopyData lParam);

        /// <summary>
        /// Get the byte array representation of a string.
        /// </summary>
        /// <param name="msg">string to be converted</param>
        /// <returns>The byte array representation of the input string.</returns>
        /// <remarks>
        /// In the array representation, the first byte is the number of following bytes.
        /// The remaining bytes consist of the string contents.
        /// </remarks>
        private byte[] GetBufferFromString(ref string msg)
        {
            // get size of buffer required to hold file name
            int bufferSize = msg.Length + 1;

            // create buffer to hold string
            byte[] buffer = new byte[bufferSize];

            // write file name size to buffer
            buffer[0] = (byte)msg.Length;

            // write file name to buffer
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            encoding.GetBytes(msg, 0, msg.Length, buffer, 1);

            return buffer;
        }

        /// <summary>
        /// handle chip attach events
        /// </summary>
        /// <param name="buffer">the chip attach information</param>
        private void NotifyChipAttach(ref byte[] buffer)
        {
            if (this.chipAttach != null)
            {
                this.chipAttach(ref buffer);
            }
        }

        /// <summary>
        /// handle chip debug data events
        /// </summary>
        /// <param name="buffer">the chip debug data</param>
        private void NotifyChipDebugData(ref byte[] buffer)
        {
            if (this.chipDebugData != null)
            {
                this.chipDebugData(ref buffer);
            }
        }

        /// <summary>
        /// handle chip debug data start confirm events
        /// </summary>
        /// <param name="chipDebugStartStatus">the operation status</param>
        private void NotifyChipDebugDataStartConfirm(ChipDebugStartStatus chipDebugStartStatus)
        {
            if (this.chipDebugDataStartConfirm != null)
            {
                this.chipDebugDataStartConfirm(chipDebugStartStatus);
            }
        }

        /// <summary>
        /// handle chip debug data stop confirm events
        /// </summary>
        /// <param name="chipDebugStopStatus">the operation status</param>
        private void NotifyChipDebugDataStopConfirm(ChipDebugStopStatus chipDebugStopStatus)
        {
            if (this.chipDebugDataStopConfirm != null)
            {
                this.chipDebugDataStopConfirm(chipDebugStopStatus);
            }
        }

        /// <summary>
        /// handle chip detach events
        /// </summary>
        private void NotifyChipDetach()
        {
            if (this.chipDetach != null)
            {
                this.chipDetach();
            }
        }

        /// <summary>
        /// handle chip load config confirmation events
        /// </summary>
        /// <param name="operationSucceeded">flag: the operation succeeded</param>
        private void NotifyChipLoadConfigConfirm(bool operationSucceeded)
        {
            if (this.chipLoadConfigConfirm != null)
            {
                this.chipLoadConfigConfirm(operationSucceeded);
            }
        }

        /// <summary>
        /// handle chip save config confirmation events
        /// </summary>
        /// <param name="operationSucceeded">flag: the operation succeeded</param>
        private void NotifyChipSaveConfigConfirm(bool operationSucceeded)
        {
            if (this.chipSaveConfigConfirm != null)
            {
                this.chipSaveConfigConfirm(operationSucceeded);
            }
        }

        /// <summary>
        /// handle chip object table events
        /// </summary>
        /// <param name="operationSucceeded">flag: the operation succeeded</param>
        /// <param name="objectTable">the chip object table</param>
        private void NotifyChipObjectTable(bool operationSucceeded, ref ObjectTableElement[] objectTable)
        {
            if (this.chipObjectTable != null)
            {
                this.chipObjectTable(operationSucceeded, ref objectTable);
            }
        }

        /// <summary>
        /// handle object configuration events
        /// </summary>
        /// <param name="data">the object configuration</param>
        private void NotifyObjectConfig(ref byte[] data)
        {
            if (this.objectConfig != null)
            {
                this.objectConfig(ref data);
            }
        }

        /// <summary>
        /// handle object invalid message register confirm events
        /// </summary>
        private void NotifyObjectInvalidMessageRegisterConfirm()
        {
            if (this.objectInvalidMessageRegisterConfirm != null)
            {
                this.objectInvalidMessageRegisterConfirm();
            }
        }

        /// <summary>
        /// handle object invalid message unregister confirm events
        /// </summary>
        private void NotifyObjectInvalidMessageUnregisterConfirm()
        {
            if (this.objectInvalidMessageUnregisterConfirm != null)
            {
                this.objectInvalidMessageUnregisterConfirm();
            }
        }

        /// <summary>
        /// handle object message events
        /// </summary>
        /// <param name="data">the object message</param>
        private void NotifyObjectMessage(ref byte[] data)
        {
            if (this.objectMessage != null)
            {
                this.objectMessage(ref data);
            }
        }

        /// <summary>
        /// handle server connection information events
        /// </summary>
        /// <param name="connectionInfo">the server connection information</param>
        private void NotifyServerConnectionInfo(ConnectionInfo connectionInfo)
        {
            if (this.serverConnectionInfo != null)
            {
                this.serverConnectionInfo(connectionInfo);
            }
        }

        /// <summary>
        /// handle server info events
        /// </summary>
        /// <param name="serverInfo">the server info</param>
        private void NotifyServerInfo(string serverInfo)
        {
            if (this.serverInfo != null)
            {
                this.serverInfo(serverInfo);
            }
        }

        /// <summary>
        /// handle server detach events
        /// </summary>
        private void NotifyServerDetach()
        {
            if (this.serverDetach != null)
            {
                this.serverDetach();
            }
        }

        /// <summary>
        /// handle server pong events
        /// </summary>
        /// <param name="serverName">the server name</param>
        /// <param name="serverVersion">the server version</param>
        private void NotifyServerPong(string serverName, string serverVersion)
        {
            if (this.serverPong != null)
            {
                this.serverPong(serverName, serverVersion);
            }
        }

        /// <summary>
        /// Process a chip attach message from the server.
        /// </summary>
        /// <param name="copyData">received message</param>
        /// <remarks>
        /// Returns the message contents to the client application.
        /// </remarks>
        private void ProcessChipAttach(ref CopyData copyData)
        {
            // debug info
            OutputDebugString("processChipAttach");

            // get message contents
            byte[] buffer = null;
            buffer = this.GetBufferFromCopyData(ref copyData);

            // expected data = family_id etc from chip info block

            // trigger chip attach event
            this.NotifyChipAttach(ref buffer);
        }

        /// <summary>
        /// Process a chip debug data message from the server.
        /// </summary>
        /// <param name="copyData">received message</param>
        private void ProcessChipDebugData(ref CopyData copyData)
        {
            // debug info
            OutputDebugString("ProcessChipDebugData");

            byte[] buffer = null;
            buffer = this.GetBufferFromCopyData(ref copyData);

            // trigger chip debug data event
            this.NotifyChipDebugData(ref buffer);
        }

        /// <summary>
        /// Process a chip debug start confirm message from the server.
        /// </summary>
        /// <param name="copyData">received message</param>
        private void ProcessChipDebugStartConfirm(ref CopyData copyData)
        {
            // debug info
            OutputDebugString("ProcessChipDebugStartConfirm");

            // message parsing variables
            byte[] buffer = null;
            buffer = this.GetBufferFromCopyData(ref copyData);
            int index = 0;

            // message format:
            // byte: debug start status

            // parse message
            ChipDebugStartStatus chipDebugStartStatus = (ChipDebugStartStatus)this.GetByte(ref buffer, ref index);

            // trigger chip debug start confirm event
            this.NotifyChipDebugDataStartConfirm(chipDebugStartStatus);
        }

        /// <summary>
        /// Process a chip debug stop confirm message from the server.
        /// </summary>
        /// <param name="copyData">received message</param>
        private void ProcessChipDebugStopConfirm(ref CopyData copyData)
        {
            // debug info
            OutputDebugString("ProcessChipDebugStopConfirm");

            // message parsing variables
            byte[] buffer = null;
            buffer = this.GetBufferFromCopyData(ref copyData);
            int index = 0;

            // message format:
            // byte: debug stop status

            // parse message
            ChipDebugStopStatus chipDebugStopStatus = (ChipDebugStopStatus)this.GetByte(ref buffer, ref index);

            // trigger chip debug sopt confirm event
            this.NotifyChipDebugDataStopConfirm(chipDebugStopStatus);
        }

        /// <summary>
        /// Process a chip detach message from the server.
        /// </summary>
        /// <remarks>
        /// Informs the client application of the message.
        /// </remarks>
        private void ProcessChipDetach()
        {
            // debug info
            OutputDebugString("processChipDetach");

            // trigger chip detach event
            this.NotifyChipDetach();
        }

        /// <summary>
        /// Process a chip load config confirm message from the server.
        /// </summary>
        /// <param name="copyData">received message</param>
        /// <remarks>
        /// Returns the message contents to the client application.
        /// </remarks>
        private void ProcessChipLoadConfigConfirm(ref CopyData copyData)
        {
            // debug info
            OutputDebugString("processChipLoadConfigConfirm");

            // message parsing variables
            byte[] buffer = null;
            buffer = this.GetBufferFromCopyData(ref copyData);
            int index = 0;

            // message format:
            // byte: flag: operation succeeded

            // parse message
            byte operationSucceeded = this.GetByte(ref buffer, ref index);
            
            // parse operation success/failure from message payload
            bool operationSucceededFlag = false;
            if (operationSucceeded == 0)
            {
                operationSucceededFlag = true;
            }

            // trigger chip load config confirm event
            this.NotifyChipLoadConfigConfirm(operationSucceededFlag);
        }

        /// <summary>
        /// Process a chip save config confirm message from the server.
        /// </summary>
        /// <param name="copyData">received message</param>
        /// <remarks>
        /// Returns the message contents to the client application.
        /// </remarks>
        private void ProcessChipSaveConfigConfirm(ref CopyData copyData)
        {
            // debug info
            OutputDebugString("processChipSaveConfigConfirm");

            // message parsing variables
            byte[] buffer = null;
            buffer = this.GetBufferFromCopyData(ref copyData);
            int index = 0;

            // message format:
            // byte: flag: operation succeeded

            // parse message
            byte operationSucceeded = this.GetByte(ref buffer, ref index);

            // parse operation success/failure from message payload
            bool operationSucceededFlag = false;
            if (operationSucceeded == 0)
            {
                operationSucceededFlag = true;
            }

            // trigger chip save config confirm event
            this.NotifyChipSaveConfigConfirm(operationSucceededFlag);
        }

        /// <summary>
        /// Process a chip object table message from the server.
        /// </summary>
        /// <param name="copyData">received message</param>
        /// <remarks>
        /// Returns the message contents to the client application by callback.
        /// </remarks>
        private void ProcessChipObjectTable(ref CopyData copyData)
        {
            // debug info
            OutputDebugString("processChipObjectTable");

            // message parsing variables
            byte[] buffer = null;
            buffer = this.GetBufferFromCopyData(ref copyData);
            int index = 0;

            // message format:
            // byte: flag: operation succeeded
            // then: multiple instances of the following:
            //       ushort: object type
            //       ushort: object start position
            //       ushort: object size
            //       ushort: number of object instances
            //       ushort: number of report IDs per instance

            // parse message
            byte operationSucceeded = this.GetByte(ref buffer, ref index);

            // parse operation success/failure from message payload
            bool operationSucceededFlag = false;
            if (operationSucceeded == 0)
            {
                operationSucceededFlag = true;
            }

            // calculate number of object table elements in the received data
            int bufferSize = buffer.Length;
            int numObjectTableElements = (bufferSize - 1) / ObjectTableElement.OBJECT_TABLE_ELEMENT_SIZE;

            // create object table
            ObjectTableElement[] objectTable = new ObjectTableElement[numObjectTableElements];

            // parse object table from message
            for (int i = 0; i < numObjectTableElements; i++)
            {
                // build object table element
                ObjectTableElement objectTableElement = new ObjectTableElement();
                objectTableElement.type = this.GetUshort(ref buffer, ref index);
                objectTableElement.start_position = this.GetUshort(ref buffer, ref index);
                objectTableElement.size = this.GetUshort(ref buffer, ref index);
                objectTableElement.instances = this.GetUshort(ref buffer, ref index);
                objectTableElement.num_report_ids_per_instance = this.GetUshort(ref buffer, ref index);

                // save object table element
                objectTable[i] = objectTableElement;
            }

            // trigger chip object table event
            this.NotifyChipObjectTable(operationSucceededFlag, ref objectTable);
        }

        /// <summary>
        /// Process an object config message from the server.
        /// </summary>
        /// <param name="copyData">received message</param>
        /// <remarks>
        /// Returns the message contents to the client application.
        /// </remarks>
        private void ProcessObjectConfig(ref CopyData copyData)
        {
            // debug info
            OutputDebugString("processObjectConfig");

            // get message contents
            byte[] buffer = null;
            buffer = this.GetBufferFromCopyData(ref copyData);

            // trigger object config event
            this.NotifyObjectConfig(ref buffer);
        }

        /// <summary>
        /// Process an object invalid message register confirm message from the server.
        /// </summary>
        private void ProcessObjectInvalidMessageRegisterConfirm()
        {
            // debug info
            OutputDebugString("ProcessObjectInvalidMessageRegisterConfirm");

            // trigger object invalid message register confirm event
            this.NotifyObjectInvalidMessageRegisterConfirm();
        }

        /// <summary>
        /// Process an object invalid message unregister confirm message from the server.
        /// </summary>
        private void ProcessObjectInvalidMessageUnregisterConfirm()
        {
            // debug info
            OutputDebugString("ProcessObjectInvalidMessageUnregisterConfirm");

            // trigger object invalid message unregister confirm event
            this.NotifyObjectInvalidMessageUnregisterConfirm();
        }

        /// <summary>
        /// Process an object message message from the server.
        /// </summary>
        /// <param name="copyData">received message</param>
        /// <remarks>
        /// Returns the message contents to the client application.
        /// </remarks>
        private void ProcessObjectMessage(ref CopyData copyData)
        {
            // debug info
            OutputDebugString("processObjectMessage");

            // get message contents
            byte[] buffer = null;
            buffer = this.GetBufferFromCopyData(ref copyData);

            // trigger object message event
            this.NotifyObjectMessage(ref buffer);
        }

        /// <summary>
        /// Process a server connection info message from the server.
        /// </summary>
        /// <param name="copyData">received message</param>
        /// <remarks>
        /// Returns the message contents to the client application by callback.
        /// </remarks>
        private void ProcessServerConnectionInfo(ref CopyData copyData)
        {
            // debug info
            OutputDebugString("processServerConnectionInfo");

            // message parsing variables
            byte[] buffer = null;
            buffer = this.GetBufferFromCopyData(ref copyData);
            int index = 0;

            // message format:
            // byte: connection type
            // byte: I2C address (for I2C connections)
            // UInt16: VID (for USB connections)
            // UInt16: PID (for USB connections)
            // string: bridge client name (for bridge connections)
            // string: bridge client version (for bridge connections)

            // parse message
            ConnectionInfo connectionInfo = new ConnectionInfo();
            byte connectionType = this.GetByte(ref buffer, ref index);
            connectionInfo.connectionType = (ConnectionType)connectionType;
            connectionInfo.i2cAddress = this.GetByte(ref buffer, ref index);

            // the original version of this message only returned the I2C address, so
            // don't try to parse the buffer if it's too short
            if (buffer.Length > 2)
            {
                connectionInfo.usbVid = this.GetUshort(ref buffer, ref index);
                connectionInfo.usbPid = this.GetUshort(ref buffer, ref index);
                connectionInfo.bridgeName = this.GetString(ref buffer, ref index);
                connectionInfo.bridgeVersion = this.GetString(ref buffer, ref index);
            }
            else
            {
                connectionInfo.usbVid = 0;
                connectionInfo.usbPid = 0;
                connectionInfo.bridgeName = string.Empty;
                connectionInfo.bridgeVersion = string.Empty;
            }

            // trigger server connection info event
            this.NotifyServerConnectionInfo(connectionInfo);
        }

        /// <summary>
        /// Process a server info message from the server.
        /// </summary>
        /// <param name="copyData">received message</param>
        private void ProcessServerInfo(ref CopyData copyData)
        {
            // debug info
            OutputDebugString("ProcessServerInfo");

            // message parsing variables
            byte[] buffer = null;
            buffer = this.GetBufferFromCopyData(ref copyData);
            int index = 0;

            // message format:
            // string: server info

            // parse message
            string msg = this.GetString(ref buffer, ref index);

            // trigger server info event
            this.NotifyServerInfo(msg);
        }

        /// <summary>
        /// Get a byte from a byte buffer.
        /// </summary>
        /// <param name="buffer">the input byte buffer</param>
        /// <param name="index">the index from which to get the byte</param>
        /// <returns>the specified byte</returns>
        /// <remarks>
        /// The index is increased to allow calls to be chained together.
        /// </remarks>
        private byte GetByte(ref byte[] buffer, ref int index)
        {
            // get return value
            byte rtnval = buffer[index];

            // increase buffer index for next call to this or similar helper function
            index++;

            return rtnval;
        }

        /// <summary>
        /// Get a 16-bit value from a byte buffer.
        /// </summary>
        /// <param name="buffer">the input byte buffer</param>
        /// <param name="index">the index from which to get the 16-bit value</param>
        /// <returns>the specified 16-bit value</returns>
        /// <remarks>
        /// The index is increased to allow calls to be chained together.
        /// </remarks>
        private ushort GetUshort(ref byte[] buffer, ref int index)
        {
            // get return value
            ushort rtnval = (ushort)((256 * buffer[index + 1]) + buffer[index]);

            // increase buffer index for next call to this or similar helper function
            index += 2;

            return rtnval;
        }

        /// <summary>
        /// Process a server detach message from the server.
        /// </summary>
        /// <remarks>
        /// Informs the client application of the message.
        /// </remarks>
        private void ProcessServerDetach()
        {
            // debug info
            OutputDebugString("processServerDetach");

            // clear server application window handle
            serverHandle = IntPtr.Zero;

            // trigger server detach event
            this.NotifyServerDetach();
        }

        /// <summary>
        /// Process a server pong message from the server.
        /// </summary>
        /// <param name="handle">server windows handle</param>
        /// <param name="copyData">received message</param>
        /// <remarks>
        /// Returns the message contents to the client application.
        /// </remarks>
        private void ProcessServerPong(IntPtr handle, ref CopyData copyData)
        {
            // debug info
            OutputDebugString("processServerPong");

            // store server application window handle
            serverHandle = handle;

            // message parsing variables
            byte[] buffer = null;
            buffer = this.GetBufferFromCopyData(ref copyData);
            int index = 0;

            // message format:
            // string: server name
            // string: server version

            // parse message
            string serverName = this.GetString(ref buffer, ref index);
            string serverVersion = this.GetString(ref buffer, ref index);
            
            // trigger server pong event
            this.NotifyServerPong(serverName, serverVersion);
        }

        /// <summary>
        /// C# representation of a Windows WM_COPYDATA message.
        /// </summary>
        /// <remarks>
        /// This structure is used for parsing received messages, and for building messages to send.
        /// </remarks>
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct CopyData
        {
            /// <summary>
            /// the message type
            /// </summary>
            public UIntPtr dwData;

            /// <summary>
            /// count of message size, in bytes
            /// </summary>
            public int cbData;

            /// <summary>
            /// pointer to the message contents
            /// </summary>
            public IntPtr lpData;
        }
    }

    /// <summary>
    /// Represents information about a connection between the server and a maXTouch device.
    /// </summary>
    public class ConnectionInfo
    {
        /// <summary>
        /// the connection type
        /// </summary>
        public ConnectionType connectionType;

        /// <summary>
        /// the device I2C address (for I2C connections)
        /// </summary>
        public byte i2cAddress;

        /// <summary>
        /// the device VID (for USB connections)
        /// </summary>
        public ushort usbVid;

        /// <summary>
        /// the device PID (for USB connections)
        /// </summary>
        public ushort usbPid;

        /// <summary>
        /// the bridge name (for bridge client connections)
        /// </summary>
        public string bridgeName;

        /// <summary>
        /// the bridge version (for bridge client connections)
        /// </summary>
        public string bridgeVersion;
    }

    /// <summary>
    /// Represents an entry in the object table for a chip.
    /// </summary>
    public class ObjectTableElement
    {
        /// <summary>
        /// the size of a received object table element, in bytes
        /// </summary>
        public const int OBJECT_TABLE_ELEMENT_SIZE = 10;

        /// <summary>
        /// the object type
        /// </summary>
        public uint type;

        /// <summary>
        /// the object's start position in the chip memory
        /// </summary>
        public uint start_position;

        /// <summary>
        /// the size of each instance of the object
        /// NB: chip reports size - 1, we actually store size
        /// </summary>
        public uint size;

        /// <summary>
        /// the number of instances of the object
        /// NB: chip reports instances - 1, we actually store instances
        /// </summary>
        public uint instances;

        /// <summary>
        /// the number of report IDs associated with each instance of the object
        /// </summary>
        public uint num_report_ids_per_instance;
    }
}
