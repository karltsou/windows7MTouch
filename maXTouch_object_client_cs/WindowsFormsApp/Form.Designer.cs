//-----------------------------------------------------------------------
// <copyright file="Form.Designer.cs" company="Atmel">
//     Copyright (c) Atmel. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace maXTouch.ObjClinet
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.timerClientPing = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.Location = new System.Drawing.Point(300, 21);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInfo.Size = new System.Drawing.Size(329, 134);
            this.textBoxInfo.TabIndex = 3;
            // 
            // timerClientPing
            // 
            this.timerClientPing.Tick += new System.EventHandler(this.TimerClientPing_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::maXTouch.ObjClinet.Properties.Resources.wall2;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(909, 554);
            this.Controls.Add(this.textBoxInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMain";
            this.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.Text = "maXTouch";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxInfo;
        private System.Windows.Forms.Timer timerClientPing;
    }
}

