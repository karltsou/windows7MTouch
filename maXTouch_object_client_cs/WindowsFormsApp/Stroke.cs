// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Diagnostics;

namespace maXTouch.ObjClinet
{
    /// <summary>
    /// Stroke object represents a single stroke, trajectory of the finger from
    /// touch-down to touch-up. Object has two properties: color of the stroke,
    /// and ID used to distinguish strokes coming from different fingers.
    /// </summary>
    public class Stroke
    {
        /// <summary>
        /// Stroke constructor.
        /// </summary>
        public Stroke()
        {
            points = new ArrayList();
        }

        /// <summary>
        /// Draws the complete stroke.
        /// in:
        ///      graphics        the drawing surface
        /// </summary>
        public void Draw(Graphics graphics)
        {
            if ((points.Count < 2) || (graphics == null))
            {
                return;
            }

            Pen pen = new Pen(color, penWidth);
            graphics.DrawLines(pen, (Point[]) points.ToArray(typeof(Point)));
        }

        /// <summary>
        /// Draws only last segment of the stroke.
        /// in:
        ///      graphics        the drawing surface
        /// </summary>
        public void DrawLast(Graphics graphics)
        {
            if ((points.Count < 2) || (graphics == null))
            {
                return;
            }

            Pen pen = new Pen(color, penWidth);
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            graphics.DrawLine(pen, (Point)points[points.Count - 2], (Point)points[points.Count - 1]);
        }

        /// <summary>
        /// Access to the property stroke color
        /// </summary>
        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// Access to the property stroke ID
        /// </summary>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Adds a point to the stroke.
        /// in:
        ///      pt          point to be added to the stroke
        /// </summary>
        public void Add(Point pt)
        {
            points.Add(pt);
        }

        // Attributes
        private ArrayList points;               // the array of stroke points
        private Color color;                    // the color of the stroke
        private int id;                         // stroke ID

        private const float penWidth = 50.0f;   // pen width for drawing the stroke
    }

    /// <summary>
    /// CollectionOfStrokes object represents a collection of the strokes.
    /// It supports add/remove stroke operations and finding a stroke by ID.
    /// </summary>
    public class CollectionOfStrokes
    {
        /// <summary>
        /// CollectionOfStrokes constructor.
        /// </summary>
        public CollectionOfStrokes()
        {
            strokes = new ArrayList();
        }

        /// <summary>
        /// Draw the collection of the strokes.
        /// in:
        ///      graphics        the drawing surface
        /// </summary>
        public void Draw(Graphics graphics)
        {
            foreach (Stroke stroke in strokes)
            {
                stroke.Draw(graphics);
            }
        }

        /// <summary>
        /// Adds the stroke to the collection.
        /// in:
        ///      stroke          stroke to be added
        /// </summary>
        public void Add(Stroke stroke)
        {
            strokes.Add(stroke);
        }

        /// <summary>
        /// Returns the stroke with the given ID.
        /// in:
        ///      id              stroke ID
        /// returns:
        ///      stroke, if found, or null
        /// </summary>
        public Stroke Get(int id)
        {
            int i = _IndexFromId(id);
            if (i == -1)
            {
                return null;
            }
            return (Stroke)strokes[i];
        }

        /// <summary>
        /// Remove the stroke from the collection.
        /// in:
        ///      id              stroke ID
        /// returns:
        ///      stroke, if found, or null
        /// </summary>
        public Stroke Remove(int id)
        {
            int i = _IndexFromId(id);
            if (i == -1)
            {
                return null;
            }
            Stroke s = (Stroke)strokes[i];
            strokes.RemoveAt(i);
            return s;
        }

        /// <summary>
        /// Clear the stroke from the collection.
        /// </summary>
        public void Clear()
        {
            // Removes all the strokes from the collection.
            strokes.Clear();
        }

        /// <summary>
        /// Search the collection for given ID.
        /// in:
        ///      id      stroke ID
        /// returns:
        ///      stroke index in the array
        /// </summary>
        private int _IndexFromId(int id)
        {
            for (int i = 0; i < strokes.Count; ++i)
            {
                Stroke stroke = (Stroke)strokes[i];
                if (id == stroke.Id)
                {
                    return i;
                }
            }
            return -1;
        }

        // Attributes
        private ArrayList strokes;          // the array of strokes
    }
}
