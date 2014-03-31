namespace generic_object_client_cs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.groupBoxServer = new System.Windows.Forms.GroupBox();
            this.textBoxServerGetInfo = new System.Windows.Forms.TextBox();
            this.buttonServerGetInfo = new System.Windows.Forms.Button();
            this.buttonServerConnectionInfo = new System.Windows.Forms.Button();
            this.buttonServerClose = new System.Windows.Forms.Button();
            this.buttonServerHide = new System.Windows.Forms.Button();
            this.buttonServerShow = new System.Windows.Forms.Button();
            this.groupBoxChip = new System.Windows.Forms.GroupBox();
            this.buttonChipObjectTable = new System.Windows.Forms.Button();
            this.buttonChipReset = new System.Windows.Forms.Button();
            this.buttonChipZeroContents = new System.Windows.Forms.Button();
            this.buttonChipSaveConfig = new System.Windows.Forms.Button();
            this.buttonChipLoadConfig = new System.Windows.Forms.Button();
            this.buttonChipCalibrate = new System.Windows.Forms.Button();
            this.buttonChipBackup = new System.Windows.Forms.Button();
            this.groupBoxObject = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numericUpDownObjectIndex = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonObjectUnregisterByIndex = new System.Windows.Forms.Button();
            this.buttonObjectRegisterByIndex = new System.Windows.Forms.Button();
            this.buttonObjectSetByIndex = new System.Windows.Forms.Button();
            this.buttonObjectGetByIndex = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numericUpDownObjectType = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonObjectUnregister = new System.Windows.Forms.Button();
            this.buttonObjectRegister = new System.Windows.Forms.Button();
            this.buttonObjectSet = new System.Windows.Forms.Button();
            this.buttonObjectGet = new System.Windows.Forms.Button();
            this.buttonObjectInvalidMessageUnregister = new System.Windows.Forms.Button();
            this.buttonObjectInvalidMessageRegister = new System.Windows.Forms.Button();
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.timerClientPing = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.groupBoxChipDebug = new System.Windows.Forms.GroupBox();
            this.checkBoxDisplayDebugData = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.maskedTextBoxDebugMode = new System.Windows.Forms.MaskedTextBox();
            this.buttonChipDebugStop = new System.Windows.Forms.Button();
            this.buttonChipDebugStart = new System.Windows.Forms.Button();
            this.groupBoxServer.SuspendLayout();
            this.groupBoxChip.SuspendLayout();
            this.groupBoxObject.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownObjectIndex)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownObjectType)).BeginInit();
            this.groupBoxChipDebug.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxServer
            // 
            this.groupBoxServer.Controls.Add(this.textBoxServerGetInfo);
            this.groupBoxServer.Controls.Add(this.buttonServerGetInfo);
            this.groupBoxServer.Controls.Add(this.buttonServerConnectionInfo);
            this.groupBoxServer.Controls.Add(this.buttonServerClose);
            this.groupBoxServer.Controls.Add(this.buttonServerHide);
            this.groupBoxServer.Controls.Add(this.buttonServerShow);
            this.groupBoxServer.Location = new System.Drawing.Point(280, 8);
            this.groupBoxServer.Name = "groupBoxServer";
            this.groupBoxServer.Size = new System.Drawing.Size(327, 119);
            this.groupBoxServer.TabIndex = 0;
            this.groupBoxServer.TabStop = false;
            this.groupBoxServer.Text = " Server ";
            // 
            // textBoxServerGetInfo
            // 
            this.textBoxServerGetInfo.Location = new System.Drawing.Point(173, 87);
            this.textBoxServerGetInfo.Name = "textBoxServerGetInfo";
            this.textBoxServerGetInfo.Size = new System.Drawing.Size(96, 20);
            this.textBoxServerGetInfo.TabIndex = 5;
            this.textBoxServerGetInfo.Text = "test";
            // 
            // buttonServerGetInfo
            // 
            this.buttonServerGetInfo.Location = new System.Drawing.Point(57, 84);
            this.buttonServerGetInfo.Name = "buttonServerGetInfo";
            this.buttonServerGetInfo.Size = new System.Drawing.Size(96, 25);
            this.buttonServerGetInfo.TabIndex = 4;
            this.buttonServerGetInfo.Text = "Get Info";
            this.buttonServerGetInfo.UseVisualStyleBackColor = true;
            this.buttonServerGetInfo.Click += new System.EventHandler(this.ButtonServerGetInfo_Click);
            // 
            // buttonServerConnectionInfo
            // 
            this.buttonServerConnectionInfo.Location = new System.Drawing.Point(173, 53);
            this.buttonServerConnectionInfo.Name = "buttonServerConnectionInfo";
            this.buttonServerConnectionInfo.Size = new System.Drawing.Size(96, 23);
            this.buttonServerConnectionInfo.TabIndex = 3;
            this.buttonServerConnectionInfo.Text = "Connection Info";
            this.buttonServerConnectionInfo.UseVisualStyleBackColor = true;
            this.buttonServerConnectionInfo.Click += new System.EventHandler(this.ButtonServerConnectionInfo_Click);
            // 
            // buttonServerClose
            // 
            this.buttonServerClose.Location = new System.Drawing.Point(173, 22);
            this.buttonServerClose.Name = "buttonServerClose";
            this.buttonServerClose.Size = new System.Drawing.Size(96, 25);
            this.buttonServerClose.TabIndex = 2;
            this.buttonServerClose.Text = "Close";
            this.buttonServerClose.UseVisualStyleBackColor = true;
            this.buttonServerClose.Click += new System.EventHandler(this.ButtonServerClose_Click);
            // 
            // buttonServerHide
            // 
            this.buttonServerHide.Location = new System.Drawing.Point(57, 53);
            this.buttonServerHide.Name = "buttonServerHide";
            this.buttonServerHide.Size = new System.Drawing.Size(96, 25);
            this.buttonServerHide.TabIndex = 1;
            this.buttonServerHide.Text = "Hide";
            this.buttonServerHide.UseVisualStyleBackColor = true;
            this.buttonServerHide.Click += new System.EventHandler(this.ButtonServerHide_Click);
            // 
            // buttonServerShow
            // 
            this.buttonServerShow.Location = new System.Drawing.Point(57, 22);
            this.buttonServerShow.Name = "buttonServerShow";
            this.buttonServerShow.Size = new System.Drawing.Size(96, 25);
            this.buttonServerShow.TabIndex = 0;
            this.buttonServerShow.Text = "Show";
            this.buttonServerShow.UseVisualStyleBackColor = true;
            this.buttonServerShow.Click += new System.EventHandler(this.ButtonServerShow_Click);
            // 
            // groupBoxChip
            // 
            this.groupBoxChip.Controls.Add(this.buttonChipObjectTable);
            this.groupBoxChip.Controls.Add(this.buttonChipReset);
            this.groupBoxChip.Controls.Add(this.buttonChipZeroContents);
            this.groupBoxChip.Controls.Add(this.buttonChipSaveConfig);
            this.groupBoxChip.Controls.Add(this.buttonChipLoadConfig);
            this.groupBoxChip.Controls.Add(this.buttonChipCalibrate);
            this.groupBoxChip.Controls.Add(this.buttonChipBackup);
            this.groupBoxChip.Location = new System.Drawing.Point(280, 133);
            this.groupBoxChip.Name = "groupBoxChip";
            this.groupBoxChip.Size = new System.Drawing.Size(327, 121);
            this.groupBoxChip.TabIndex = 1;
            this.groupBoxChip.TabStop = false;
            this.groupBoxChip.Text = " Chip ";
            // 
            // buttonChipObjectTable
            // 
            this.buttonChipObjectTable.Location = new System.Drawing.Point(37, 81);
            this.buttonChipObjectTable.Name = "buttonChipObjectTable";
            this.buttonChipObjectTable.Size = new System.Drawing.Size(76, 25);
            this.buttonChipObjectTable.TabIndex = 7;
            this.buttonChipObjectTable.Text = "Object Table";
            this.buttonChipObjectTable.UseVisualStyleBackColor = true;
            this.buttonChipObjectTable.Click += new System.EventHandler(this.ButtonChipObjectTable_Click);
            // 
            // buttonChipReset
            // 
            this.buttonChipReset.Location = new System.Drawing.Point(213, 50);
            this.buttonChipReset.Name = "buttonChipReset";
            this.buttonChipReset.Size = new System.Drawing.Size(76, 25);
            this.buttonChipReset.TabIndex = 6;
            this.buttonChipReset.Text = "Reset";
            this.buttonChipReset.UseVisualStyleBackColor = true;
            this.buttonChipReset.Click += new System.EventHandler(this.ButtonChipReset_Click);
            // 
            // buttonChipZeroContents
            // 
            this.buttonChipZeroContents.Location = new System.Drawing.Point(213, 16);
            this.buttonChipZeroContents.Name = "buttonChipZeroContents";
            this.buttonChipZeroContents.Size = new System.Drawing.Size(76, 25);
            this.buttonChipZeroContents.TabIndex = 5;
            this.buttonChipZeroContents.Text = "Zero Contents";
            this.buttonChipZeroContents.UseVisualStyleBackColor = true;
            this.buttonChipZeroContents.Click += new System.EventHandler(this.ButtonChipZeroContents_Click);
            // 
            // buttonChipSaveConfig
            // 
            this.buttonChipSaveConfig.Location = new System.Drawing.Point(125, 50);
            this.buttonChipSaveConfig.Name = "buttonChipSaveConfig";
            this.buttonChipSaveConfig.Size = new System.Drawing.Size(76, 25);
            this.buttonChipSaveConfig.TabIndex = 4;
            this.buttonChipSaveConfig.Text = "Save Config";
            this.buttonChipSaveConfig.UseVisualStyleBackColor = true;
            this.buttonChipSaveConfig.Click += new System.EventHandler(this.ButtonChipSaveConfig_Click);
            // 
            // buttonChipLoadConfig
            // 
            this.buttonChipLoadConfig.Location = new System.Drawing.Point(125, 16);
            this.buttonChipLoadConfig.Name = "buttonChipLoadConfig";
            this.buttonChipLoadConfig.Size = new System.Drawing.Size(76, 25);
            this.buttonChipLoadConfig.TabIndex = 3;
            this.buttonChipLoadConfig.Text = "Load Config";
            this.buttonChipLoadConfig.UseVisualStyleBackColor = true;
            this.buttonChipLoadConfig.Click += new System.EventHandler(this.ButtonChipLoadConfig_Click);
            // 
            // buttonChipCalibrate
            // 
            this.buttonChipCalibrate.Location = new System.Drawing.Point(37, 50);
            this.buttonChipCalibrate.Name = "buttonChipCalibrate";
            this.buttonChipCalibrate.Size = new System.Drawing.Size(76, 25);
            this.buttonChipCalibrate.TabIndex = 2;
            this.buttonChipCalibrate.Text = "Calibrate";
            this.buttonChipCalibrate.UseVisualStyleBackColor = true;
            this.buttonChipCalibrate.Click += new System.EventHandler(this.ButtonChipCalibrate_Click);
            // 
            // buttonChipBackup
            // 
            this.buttonChipBackup.Location = new System.Drawing.Point(37, 16);
            this.buttonChipBackup.Name = "buttonChipBackup";
            this.buttonChipBackup.Size = new System.Drawing.Size(76, 25);
            this.buttonChipBackup.TabIndex = 1;
            this.buttonChipBackup.Text = "Backup";
            this.buttonChipBackup.UseVisualStyleBackColor = true;
            this.buttonChipBackup.Click += new System.EventHandler(this.ButtonChipBackup_Click);
            // 
            // groupBoxObject
            // 
            this.groupBoxObject.Controls.Add(this.groupBox2);
            this.groupBoxObject.Controls.Add(this.groupBox1);
            this.groupBoxObject.Controls.Add(this.buttonObjectInvalidMessageUnregister);
            this.groupBoxObject.Controls.Add(this.buttonObjectInvalidMessageRegister);
            this.groupBoxObject.Location = new System.Drawing.Point(280, 349);
            this.groupBoxObject.Name = "groupBoxObject";
            this.groupBoxObject.Size = new System.Drawing.Size(327, 241);
            this.groupBoxObject.TabIndex = 2;
            this.groupBoxObject.TabStop = false;
            this.groupBoxObject.Text = " Object ";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numericUpDownObjectIndex);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.buttonObjectUnregisterByIndex);
            this.groupBox2.Controls.Add(this.buttonObjectRegisterByIndex);
            this.groupBox2.Controls.Add(this.buttonObjectSetByIndex);
            this.groupBox2.Controls.Add(this.buttonObjectGetByIndex);
            this.groupBox2.Location = new System.Drawing.Point(7, 143);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(314, 89);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " By Index ";
            // 
            // numericUpDownObjectIndex
            // 
            this.numericUpDownObjectIndex.Location = new System.Drawing.Point(51, 37);
            this.numericUpDownObjectIndex.Name = "numericUpDownObjectIndex";
            this.numericUpDownObjectIndex.Size = new System.Drawing.Size(57, 20);
            this.numericUpDownObjectIndex.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Index";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonObjectUnregisterByIndex
            // 
            this.buttonObjectUnregisterByIndex.Location = new System.Drawing.Point(201, 50);
            this.buttonObjectUnregisterByIndex.Name = "buttonObjectUnregisterByIndex";
            this.buttonObjectUnregisterByIndex.Size = new System.Drawing.Size(76, 25);
            this.buttonObjectUnregisterByIndex.TabIndex = 7;
            this.buttonObjectUnregisterByIndex.Text = "Unregister";
            this.buttonObjectUnregisterByIndex.UseVisualStyleBackColor = true;
            this.buttonObjectUnregisterByIndex.Click += new System.EventHandler(this.ButtonObjectUnregisterByIndex_Click);
            // 
            // buttonObjectRegisterByIndex
            // 
            this.buttonObjectRegisterByIndex.Location = new System.Drawing.Point(200, 19);
            this.buttonObjectRegisterByIndex.Name = "buttonObjectRegisterByIndex";
            this.buttonObjectRegisterByIndex.Size = new System.Drawing.Size(76, 25);
            this.buttonObjectRegisterByIndex.TabIndex = 6;
            this.buttonObjectRegisterByIndex.Text = "Register";
            this.buttonObjectRegisterByIndex.UseVisualStyleBackColor = true;
            this.buttonObjectRegisterByIndex.Click += new System.EventHandler(this.ButtonObjectRegisterByIndex_Click);
            // 
            // buttonObjectSetByIndex
            // 
            this.buttonObjectSetByIndex.Location = new System.Drawing.Point(119, 50);
            this.buttonObjectSetByIndex.Name = "buttonObjectSetByIndex";
            this.buttonObjectSetByIndex.Size = new System.Drawing.Size(76, 25);
            this.buttonObjectSetByIndex.TabIndex = 5;
            this.buttonObjectSetByIndex.Text = "Set";
            this.buttonObjectSetByIndex.UseVisualStyleBackColor = true;
            this.buttonObjectSetByIndex.Click += new System.EventHandler(this.ButtonObjectSetByIndex_Click);
            // 
            // buttonObjectGetByIndex
            // 
            this.buttonObjectGetByIndex.Location = new System.Drawing.Point(119, 19);
            this.buttonObjectGetByIndex.Name = "buttonObjectGetByIndex";
            this.buttonObjectGetByIndex.Size = new System.Drawing.Size(76, 25);
            this.buttonObjectGetByIndex.TabIndex = 4;
            this.buttonObjectGetByIndex.Text = "Get";
            this.buttonObjectGetByIndex.UseVisualStyleBackColor = true;
            this.buttonObjectGetByIndex.Click += new System.EventHandler(this.ButtonObjectGetByIndex_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numericUpDownObjectType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.buttonObjectUnregister);
            this.groupBox1.Controls.Add(this.buttonObjectRegister);
            this.groupBox1.Controls.Add(this.buttonObjectSet);
            this.groupBox1.Controls.Add(this.buttonObjectGet);
            this.groupBox1.Location = new System.Drawing.Point(6, 48);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(314, 89);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " By Type ";
            // 
            // numericUpDownObjectType
            // 
            this.numericUpDownObjectType.Location = new System.Drawing.Point(51, 37);
            this.numericUpDownObjectType.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownObjectType.Name = "numericUpDownObjectType";
            this.numericUpDownObjectType.Size = new System.Drawing.Size(57, 20);
            this.numericUpDownObjectType.TabIndex = 10;
            this.numericUpDownObjectType.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Type";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonObjectUnregister
            // 
            this.buttonObjectUnregister.Location = new System.Drawing.Point(201, 50);
            this.buttonObjectUnregister.Name = "buttonObjectUnregister";
            this.buttonObjectUnregister.Size = new System.Drawing.Size(76, 25);
            this.buttonObjectUnregister.TabIndex = 7;
            this.buttonObjectUnregister.Text = "Unregister";
            this.buttonObjectUnregister.UseVisualStyleBackColor = true;
            this.buttonObjectUnregister.Click += new System.EventHandler(this.ButtonObjectUnregister_Click_1);
            // 
            // buttonObjectRegister
            // 
            this.buttonObjectRegister.Location = new System.Drawing.Point(200, 19);
            this.buttonObjectRegister.Name = "buttonObjectRegister";
            this.buttonObjectRegister.Size = new System.Drawing.Size(76, 25);
            this.buttonObjectRegister.TabIndex = 6;
            this.buttonObjectRegister.Text = "Register";
            this.buttonObjectRegister.UseVisualStyleBackColor = true;
            this.buttonObjectRegister.Click += new System.EventHandler(this.ButtonObjectRegister_Click_1);
            // 
            // buttonObjectSet
            // 
            this.buttonObjectSet.Location = new System.Drawing.Point(119, 50);
            this.buttonObjectSet.Name = "buttonObjectSet";
            this.buttonObjectSet.Size = new System.Drawing.Size(76, 25);
            this.buttonObjectSet.TabIndex = 5;
            this.buttonObjectSet.Text = "Set";
            this.buttonObjectSet.UseVisualStyleBackColor = true;
            this.buttonObjectSet.Click += new System.EventHandler(this.ButtonObjectSet_Click_1);
            // 
            // buttonObjectGet
            // 
            this.buttonObjectGet.Location = new System.Drawing.Point(119, 19);
            this.buttonObjectGet.Name = "buttonObjectGet";
            this.buttonObjectGet.Size = new System.Drawing.Size(76, 25);
            this.buttonObjectGet.TabIndex = 4;
            this.buttonObjectGet.Text = "Get";
            this.buttonObjectGet.UseVisualStyleBackColor = true;
            this.buttonObjectGet.Click += new System.EventHandler(this.ButtonObjectGet_Click_1);
            // 
            // buttonObjectInvalidMessageUnregister
            // 
            this.buttonObjectInvalidMessageUnregister.Location = new System.Drawing.Point(171, 19);
            this.buttonObjectInvalidMessageUnregister.Name = "buttonObjectInvalidMessageUnregister";
            this.buttonObjectInvalidMessageUnregister.Size = new System.Drawing.Size(150, 23);
            this.buttonObjectInvalidMessageUnregister.TabIndex = 10;
            this.buttonObjectInvalidMessageUnregister.Text = "Invalid Message Unregister";
            this.buttonObjectInvalidMessageUnregister.UseVisualStyleBackColor = true;
            this.buttonObjectInvalidMessageUnregister.Click += new System.EventHandler(this.ButtonObjectInvalidMessageUnregister_Click);
            // 
            // buttonObjectInvalidMessageRegister
            // 
            this.buttonObjectInvalidMessageRegister.Location = new System.Drawing.Point(6, 19);
            this.buttonObjectInvalidMessageRegister.Name = "buttonObjectInvalidMessageRegister";
            this.buttonObjectInvalidMessageRegister.Size = new System.Drawing.Size(150, 23);
            this.buttonObjectInvalidMessageRegister.TabIndex = 9;
            this.buttonObjectInvalidMessageRegister.Text = "Invalid Message Register";
            this.buttonObjectInvalidMessageRegister.UseVisualStyleBackColor = true;
            this.buttonObjectInvalidMessageRegister.Click += new System.EventHandler(this.ButtonObjectInvalidMessageRegister_Click);
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.Location = new System.Drawing.Point(8, 8);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxInfo.Size = new System.Drawing.Size(265, 582);
            this.textBoxInfo.TabIndex = 3;
            // 
            // timerClientPing
            // 
            this.timerClientPing.Tick += new System.EventHandler(this.TimerClientPing_Tick);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // groupBoxChipDebug
            // 
            this.groupBoxChipDebug.Controls.Add(this.checkBoxDisplayDebugData);
            this.groupBoxChipDebug.Controls.Add(this.label2);
            this.groupBoxChipDebug.Controls.Add(this.maskedTextBoxDebugMode);
            this.groupBoxChipDebug.Controls.Add(this.buttonChipDebugStop);
            this.groupBoxChipDebug.Controls.Add(this.buttonChipDebugStart);
            this.groupBoxChipDebug.Location = new System.Drawing.Point(280, 260);
            this.groupBoxChipDebug.Name = "groupBoxChipDebug";
            this.groupBoxChipDebug.Size = new System.Drawing.Size(327, 83);
            this.groupBoxChipDebug.TabIndex = 4;
            this.groupBoxChipDebug.TabStop = false;
            this.groupBoxChipDebug.Text = " Chip Debug ";
            // 
            // checkBoxDisplayDebugData
            // 
            this.checkBoxDisplayDebugData.AutoSize = true;
            this.checkBoxDisplayDebugData.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxDisplayDebugData.Location = new System.Drawing.Point(45, 52);
            this.checkBoxDisplayDebugData.Name = "checkBoxDisplayDebugData";
            this.checkBoxDisplayDebugData.Size = new System.Drawing.Size(117, 17);
            this.checkBoxDisplayDebugData.TabIndex = 13;
            this.checkBoxDisplayDebugData.Text = "Display debug data";
            this.checkBoxDisplayDebugData.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Debug mode";
            // 
            // maskedTextBoxDebugMode
            // 
            this.maskedTextBoxDebugMode.Location = new System.Drawing.Point(148, 21);
            this.maskedTextBoxDebugMode.Mask = "0##";
            this.maskedTextBoxDebugMode.Name = "maskedTextBoxDebugMode";
            this.maskedTextBoxDebugMode.PromptChar = ' ';
            this.maskedTextBoxDebugMode.Size = new System.Drawing.Size(53, 20);
            this.maskedTextBoxDebugMode.TabIndex = 11;
            this.maskedTextBoxDebugMode.Text = "8";
            // 
            // buttonChipDebugStop
            // 
            this.buttonChipDebugStop.Location = new System.Drawing.Point(207, 48);
            this.buttonChipDebugStop.Name = "buttonChipDebugStop";
            this.buttonChipDebugStop.Size = new System.Drawing.Size(75, 23);
            this.buttonChipDebugStop.TabIndex = 10;
            this.buttonChipDebugStop.Text = "Debug Stop";
            this.buttonChipDebugStop.UseVisualStyleBackColor = true;
            this.buttonChipDebugStop.Click += new System.EventHandler(this.ButtonChipDebugStop_Click);
            // 
            // buttonChipDebugStart
            // 
            this.buttonChipDebugStart.Location = new System.Drawing.Point(207, 19);
            this.buttonChipDebugStart.Name = "buttonChipDebugStart";
            this.buttonChipDebugStart.Size = new System.Drawing.Size(75, 23);
            this.buttonChipDebugStart.TabIndex = 9;
            this.buttonChipDebugStart.Text = "Debug Start";
            this.buttonChipDebugStart.UseVisualStyleBackColor = true;
            this.buttonChipDebugStart.Click += new System.EventHandler(this.ButtonChipDebugStart_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 597);
            this.Controls.Add(this.groupBoxChipDebug);
            this.Controls.Add(this.textBoxInfo);
            this.Controls.Add(this.groupBoxObject);
            this.Controls.Add(this.groupBoxChip);
            this.Controls.Add(this.groupBoxServer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.groupBoxServer.ResumeLayout(false);
            this.groupBoxServer.PerformLayout();
            this.groupBoxChip.ResumeLayout(false);
            this.groupBoxObject.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownObjectIndex)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownObjectType)).EndInit();
            this.groupBoxChipDebug.ResumeLayout(false);
            this.groupBoxChipDebug.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxServer;
        private System.Windows.Forms.GroupBox groupBoxChip;
        private System.Windows.Forms.GroupBox groupBoxObject;
        private System.Windows.Forms.TextBox textBoxInfo;
        private System.Windows.Forms.Button buttonServerClose;
        private System.Windows.Forms.Button buttonServerHide;
        private System.Windows.Forms.Button buttonServerShow;
        private System.Windows.Forms.Button buttonChipReset;
        private System.Windows.Forms.Button buttonChipZeroContents;
        private System.Windows.Forms.Button buttonChipSaveConfig;
        private System.Windows.Forms.Button buttonChipLoadConfig;
        private System.Windows.Forms.Button buttonChipCalibrate;
        private System.Windows.Forms.Button buttonChipBackup;
        private System.Windows.Forms.Timer timerClientPing;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.Button buttonChipObjectTable;
        private System.Windows.Forms.Button buttonServerConnectionInfo;
        private System.Windows.Forms.GroupBox groupBoxChipDebug;
        private System.Windows.Forms.Button buttonChipDebugStop;
        private System.Windows.Forms.Button buttonChipDebugStart;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxDebugMode;
        private System.Windows.Forms.CheckBox checkBoxDisplayDebugData;
        private System.Windows.Forms.Button buttonServerGetInfo;
        private System.Windows.Forms.TextBox textBoxServerGetInfo;
        private System.Windows.Forms.Button buttonObjectInvalidMessageUnregister;
        private System.Windows.Forms.Button buttonObjectInvalidMessageRegister;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonObjectGet;
        private System.Windows.Forms.NumericUpDown numericUpDownObjectType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonObjectUnregister;
        private System.Windows.Forms.Button buttonObjectRegister;
        private System.Windows.Forms.Button buttonObjectSet;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numericUpDownObjectIndex;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonObjectUnregisterByIndex;
        private System.Windows.Forms.Button buttonObjectRegisterByIndex;
        private System.Windows.Forms.Button buttonObjectSetByIndex;
        private System.Windows.Forms.Button buttonObjectGetByIndex;
    }
}
