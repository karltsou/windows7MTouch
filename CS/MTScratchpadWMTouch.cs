// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.

// MTScratchpadWMTouch application.
// Description:
//  Inside the application window, user can draw using multiple fingers
//  at the same time. The trace of each finger is drawn using different
//  color. The primary finger trace is always drawn in black, and the
//  remaining traces are drawn by rotating through the following colors:
//  red, blue, green, magenta, cyan and yellow.
//
// Purpose:
//  This sample demonstrates handling of the multi-touch input inside
//  a C# application using WM_TOUCH window message:
//  - Registering a window for multi-touch using RegisterTouchWindow,
//    IsTouchWindow.
//  - Handling WM_TOUCH messages and unpacking their parameters using
//    GetTouchInputInfo and CloseTouchInputHandle; reading touch contact
//    data from the TOUCHINPUT structure.
//  In addition, the sample also shows how to store and draw strokes
//  entered by user, using the helper classes Stroke and
//  CollectionOfStrokes.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Microsoft.Samples.Touch.MTScratchpadWMTouch
{
    // Main app, WMTouchForm-derived, multi-touch aware form.
    public partial class MTScratchpadWMTouchForm : WMTouchForm
    {
        // Constructor
        public MTScratchpadWMTouchForm()
        {
            InitializeComponent();

            // Create data members, color generator and stroke collections.
            touchColor = new TouchColor ();
            ActiveStrokes = new CollectionOfStrokes ();
            FinishedStrokes = new CollectionOfStrokes ();

            // Setup handlers
            Touchdown += OnTouchDownHandler;
            Touchup += OnTouchUpHandler;
            TouchMove += OnTouchMoveHandler;
            Paint += new PaintEventHandler(this.OnPaintHandler);

            // Set window background color
            this.BackColor = SystemColors.Window;
        }

        // Touch down event handler.
        // Starts a new stroke and assigns a color to it. 
        // in:
        //      sender      object that has sent the event
        //      e           touch event arguments
        private void OnTouchDownHandler(object sender, WMTouchEventArgs e)
        {
            // We have just started a new stroke, which must have an ID value unique
            // among all the strokes currently being drawn. Check if there is a stroke
            // with the same ID in the collection of the strokes in drawing.
            Debug.Assert(ActiveStrokes.Get(e.Id) == null);

            // Create new stroke, add point and assign a color to it.
            Stroke newStroke = new Stroke ();
            newStroke.Color = touchColor.GetColor(e.IsPrimaryContact);
            newStroke.Id = e.Id;

            // Add new stroke to the collection of strokes in drawing.
            ActiveStrokes.Add(newStroke);

#if (DEBUG_XY)
            label5.Text = "TOUCH DOWN";
            richTextBox1.AppendText("x: " + e.LocationX.ToString() + " ");
            richTextBox1.AppendText("y: " + e.LocationY.ToString() + "\n");
#endif
            FinishedStrokes.tdTopLeftX = e.LocationX;
            FinishedStrokes.tdTopLeftY = e.LocationY;
            FinishedStrokes.tdTopRightX = e.LocationX;
            FinishedStrokes.tdTopRightY = e.LocationY;
            FinishedStrokes.tdBottomLeftX = e.LocationX;
            FinishedStrokes.tdBottomLeftY = e.LocationY;
            FinishedStrokes.tdBottomRightX = e.LocationX;
            FinishedStrokes.tdBottomRightY = e.LocationY;
        }

        // Touch up event handler.
        // Finishes the stroke and moves it to the collection of finished strokes.
        // in:
        //      sender      object that has sent the event
        //      e           touch event arguments
        private void OnTouchUpHandler(object sender, WMTouchEventArgs e)
        {
            // Find the stroke in the collection of the strokes in drawing
            // and remove it from this collection.
            Stroke stroke = ActiveStrokes.Remove(e.Id);
            Debug.Assert(stroke != null);

            // Add this stroke to the collection of finished strokes.
            FinishedStrokes.Add(stroke);
#if (DEBUG_XY)
            label5.Text = "TOUCH UP";
            richTextBox1.AppendText("x: " + e.LocationX.ToString() + " ");
            richTextBox1.AppendText("y: " + e.LocationY.ToString() + "\n");
#endif
            FinishedStrokes.tuTopLeftX = e.LocationX;
            FinishedStrokes.tuTopLeftY = e.LocationY;
            FinishedStrokes.tuTopRightX = e.LocationX;
            FinishedStrokes.tuTopRightY = e.LocationY;
            FinishedStrokes.tuBottomLeftX = e.LocationX;
            FinishedStrokes.tuBottomLeftY = e.LocationY;
            FinishedStrokes.tuBottomRightX = e.LocationX;
            FinishedStrokes.tuBottomRightY = e.LocationY;

            // Request full redraw.
            Invalidate();
        }

        // Touch move event handler.
        // Adds a point to the active stroke and draws new stroke segment.
        // in:
        //      sender      object that has sent the event
        //      e           touch event arguments
        private void OnTouchMoveHandler(object sender, WMTouchEventArgs e)
        {
            // Find the stroke in the collection of the strokes in drawing.
            Stroke stroke = ActiveStrokes.Get(e.Id);
            Debug.Assert(stroke != null);

            // Add contact point to the stroke
            stroke.Add(new Point(e.LocationX, e.LocationY));

#if (DEBUG_XY)
            label5.Text = "TOUCH MOVING";
            richTextBox1.AppendText("x: " + e.LocationX.ToString() + " ");
            richTextBox1.AppendText("y: " + e.LocationY.ToString() + "\n");
#endif
            // Partial redraw: only the last line segment
            Graphics g = this.CreateGraphics();
            stroke.DrawLast(g);
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

                // Top,left to Bottom,Right
                if ((FinishedStrokes.tuBottomRightX - FinishedStrokes.tdTopLeftX >= 1300)
                    &&(FinishedStrokes.tuBottomRightY - FinishedStrokes.tdTopLeftY >= 600))
                {

                    FinishedStrokes.Draw(e.Graphics);
                    label5.Text = "Drawing Path 1 to 3 Passed";
                    richTextBox1.AppendText("Path 1 to 3 Passed\n");

                }

                // top,right to bottom,left
                else if ((FinishedStrokes.tdTopRightX - FinishedStrokes.tuBottomLeftX >= 1300)
                         &&(FinishedStrokes.tuBottomLeftY - FinishedStrokes.tdTopRightY >= 600))
                {

                    FinishedStrokes.Draw(e.Graphics);
                    label5.Text = "Drawing Path 2 to 4 Passed";
                    richTextBox1.AppendText("Path 2 to 4 Passed\n");

                }

                // Top,left to Top,right
                else if ((FinishedStrokes.tuTopRightX - FinishedStrokes.tdTopLeftX >= 1300)
                         && (FinishedStrokes.tuTopRightY <= 45))
                {

                    FinishedStrokes.Draw(e.Graphics);
                    label5.Text = "Drawing Path 1 to 2 Passed";
                    richTextBox1.AppendText("Path 1 to 2 Passed\n");

                }

                // Top,right to Bottom,right
                else if ((FinishedStrokes.tuBottomRightY - FinishedStrokes.tdTopRightY >= 600)
                         && (FinishedStrokes.tuTopRightX >= 1330))
                {

                    FinishedStrokes.Draw(e.Graphics);
                    label5.Text = "Drawing Path 2 to 3 Passed";
                    richTextBox1.AppendText("Path 2 to 3 Passed\n");

                }

                // Bottom,right to Bottom,left
                else if ((FinishedStrokes.tdBottomRightX - FinishedStrokes.tuBottomLeftX >= 1300)
                         && (FinishedStrokes.tuBottomLeftY >= 600))
                {

                    FinishedStrokes.Draw(e.Graphics);
                    label5.Text = "Drawing Path 3 to 4 Passed";
                    richTextBox1.AppendText("Path 3 to 4 Passed\n");

                }

                // Top,left to Bottom,left
                else if ((FinishedStrokes.tuBottomLeftY - FinishedStrokes.tdTopLeftY >= 600)
                         && (FinishedStrokes.tuBottomLeftX <= 45))
                {

                    FinishedStrokes.Draw(e.Graphics);
                    label5.Text = "Drawing Path 1 to 4 Passed";
                    richTextBox1.AppendText("Path 1 to 4 Passed\n");

                }

                else
                {

                    FinishedStrokes.Clear();
                    label5.Text = "Drawing Path Failed";
                    richTextBox1.AppendText("Path Failed\n");

                }

        }

        // Attributes
        private TouchColor touchColor;                  // Color generator
        private CollectionOfStrokes FinishedStrokes;    // Collection of finished strokes
        private CollectionOfStrokes ActiveStrokes;      // Collection of active strokes, currently being drawn by the user

        // Clean Drawing Surface
        // in:
        //      sender      object that has sent the event
        //      e           event arguments
        private void button1_Click(object sender, EventArgs e)
        {
            // Clears the entire drawing surface
            // and fills it with the specified background color.
            //Graphics g = this.CreateGraphics();
            //g.Clear(Color.White);

            // Clears richtext
            richTextBox1.Clear();

            // Remove strokes both finish and active
            FinishedStrokes.Clear();
            //ActiveStrokes.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK
                && saveFileDialog1.FileName.Length > 0)
            {

                richTextBox1.SaveFile(saveFileDialog1.FileName,
                    RichTextBoxStreamType.PlainText);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            // Add action here
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // Add action here
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Add action here
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Add action here
        }

        private void label5_Click(object sender, EventArgs e)
        {
            // Add action here
        }

    }

    // Color generator: assigns a color to the new stroke.
    public class TouchColor
    {
        // Constructor
        public TouchColor()
        {
        }

        // Returns color for the newly started stroke.
        // in:
        //      primary         boolean, whether the contact is the primary contact
        // returns:
        //      color of the stroke
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

        // Attributes
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