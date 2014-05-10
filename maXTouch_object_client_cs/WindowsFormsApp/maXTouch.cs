//-----------------------------------------------------------------------
// <copyright file="maXTouch.cs" company="Atmel">
//     Copyright (c) Atmel. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;

namespace maXTouch.ObjClinet
{
    /// <summary>
    /// touch event
    /// </summary>
    public enum EventCodes
    {
        /// <summary>
        /// no specific event has occurred
        /// </summary>
        TOUCH_NOEVENT = 0,

        /// <summary>
        /// touch position has changed
        /// </summary>
        TOUCH_MOVE = 1,

        /// <summary>
        /// touch has just come within range of the sensor
        /// </summary>
        TOUCH_DOWN = 4,

        /// <summary>
        /// touch has just left the range of the sensor
        /// </summary>
        TOUCH_UP = 5,
    }

    /// <summary>
    /// event types
    /// </summary>
    public enum EventTypes
    { 
        /// <summary>
        /// reserved
        /// </summary>
        EVENT_RESERVED = 0,

        /// <summary>
        /// touch is considered to be a finger that is contacting the screen
        /// </summary>
        EVENT_FINGER = 1,

        /// <summary>
        /// touch is a passive stylus
        /// </summary>
        EVENT_PASSIVE_STYLUS = 2,

        /// <summary>
        /// touch is reporting and present within the range of the sensor
        /// </summary>
        EVENT_DETECT = 8,
    }
    
    /// <summary>
    /// 
    /// </summary>
    public class Touch
    {
        /// <summary>
        /// construction
        /// </summary>
        public Touch()
        {
           //data = default(TouchData);

        }

        /// <summary>
        /// touch messages queue
        /// </summary>
        public struct TouchData
        {
            /// <summary>
            /// 
            /// </summary>
            public byte id;
            
            /// <summary>
            /// 
            /// </summary>
            public byte touchevent;

            /// <summary>
            /// 
            /// </summary>
            public byte xLoByte;
            
            /// <summary>
            /// 
            /// </summary>
            public byte xHiByte;

            /// <summary>
            /// 
            /// </summary>
            public byte yLoByte;
            
            /// <summary>
            /// 
            /// </summary>
            public byte yHiByte;

            /// <summary>
            /// 
            /// </summary>
            public byte[] objmsg;
            /// <summary>
            /// 
            /// </summary>
            public int msg_size;
        }

        /// <summary>
        /// touch message queue
        /// </summary>
        public TouchData data;

        /// <summary>
        /// skip id
        /// </summary>
        public byte skip_id = 39;
        /// <summary>
        /// valid id
        /// </summary>
        public byte valid_id = 40; 

        /// <summary>
        /// get touch message id
        /// </summary>
        /// <returns></returns>
        public byte get_id
        {
            get { return data.objmsg[0]; }
        }

        /// <summary>
        /// get touch event up, down, movement
        /// </summary>
        /// <returns></returns>
        public byte get_event
        {
            get { return (byte)(data.objmsg[1] & 0x0f); }
        }
        
        /// <summary>
        /// get touch detection and type
        /// </summary>
        /// <returns></returns>
        public byte get_type
        {
            get { return (byte)((data.objmsg[1] >> 4) & (byte)(EventTypes.EVENT_DETECT | EventTypes.EVENT_FINGER)); }            
        }
        
        /// <summary>
        /// real touch x cordination
        /// </summary>
        /// <returns></returns>
        public short get_x
        {
            get { return (short)((data.objmsg[3] << 8) | data.objmsg[2]); }
        }

        /// <summary>
        /// real touch y cordination
        /// </summary>
        /// <returns></returns>
        public short get_y
        {
            get { return (short)((data.objmsg[5] << 8) | data.objmsg[4]); }
        }

        public byte get_t9_event
        {
            get { return (byte)(data.objmsg[1] & 0x7f); }
        }

        public short get_t9_x
        {

            get { return (short)((data.objmsg[2] << 4) | (data.objmsg[4] >> 4)); }
        }

        public short get_t9_y
        {
            get { return (short)((data.objmsg[3]) << 2 | (data.objmsg[4] & 0x0c) >> 2); }
        }

        /// <summary>
        /// 
        /// </summary>
        public void filldata(ref byte[] objmsg)
        {
            int bufferSize = objmsg.Length;
            int i;

            data.msg_size = bufferSize - 3;
            data.objmsg = new byte[data.msg_size];
            for (i = 0; i < data.msg_size; i++) 
            {
                data.objmsg[i] = objmsg[i];
            }
        }

    }
}