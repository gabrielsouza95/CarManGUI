namespace TempTest
{
    partial class TestTemp
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cbSerialPorts = new System.Windows.Forms.ComboBox();
            this.butInicSerial = new System.Windows.Forms.Button();
            this.rtbSerialOutput = new System.Windows.Forms.RichTextBox();
            this.tpTempGauges = new System.Windows.Forms.TabPage();
            this.lbMaxTemp4 = new System.Windows.Forms.Label();
            this.lbMinTemp4 = new System.Windows.Forms.Label();
            this.lbMLX4 = new System.Windows.Forms.Label();
            this.pbMLX4 = new System.Windows.Forms.ProgressBar();
            this.lbMaxTemp3 = new System.Windows.Forms.Label();
            this.lbMinTemp3 = new System.Windows.Forms.Label();
            this.lbMLX3 = new System.Windows.Forms.Label();
            this.pbMLX3 = new System.Windows.Forms.ProgressBar();
            this.lbMaxTemp2 = new System.Windows.Forms.Label();
            this.lbMinTemp2 = new System.Windows.Forms.Label();
            this.lbMLX2 = new System.Windows.Forms.Label();
            this.pbMLX2 = new System.Windows.Forms.ProgressBar();
            this.lbMaxTemp1 = new System.Windows.Forms.Label();
            this.lbMinTemp1 = new System.Windows.Forms.Label();
            this.lbMLX1 = new System.Windows.Forms.Label();
            this.pbMLX1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.tbMLX1 = new System.Windows.Forms.TextBox();
            this.tbMLX2 = new System.Windows.Forms.TextBox();
            this.tbMLX3 = new System.Windows.Forms.TextBox();
            this.tbMLX4 = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tpTempGauges.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tpTempGauges);
            this.tabControl1.Location = new System.Drawing.Point(0, -1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(332, 185);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.cbSerialPorts);
            this.tabPage1.Controls.Add(this.butInicSerial);
            this.tabPage1.Controls.Add(this.rtbSerialOutput);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(324, 159);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Valores recebidos";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cbSerialPorts
            // 
            this.cbSerialPorts.FormattingEnabled = true;
            this.cbSerialPorts.Location = new System.Drawing.Point(9, 106);
            this.cbSerialPorts.Name = "cbSerialPorts";
            this.cbSerialPorts.Size = new System.Drawing.Size(149, 21);
            this.cbSerialPorts.TabIndex = 2;
            this.cbSerialPorts.Text = "Selecione uma porta Serial";
            // 
            // butInicSerial
            // 
            this.butInicSerial.BackColor = System.Drawing.Color.YellowGreen;
            this.butInicSerial.Location = new System.Drawing.Point(8, 130);
            this.butInicSerial.Margin = new System.Windows.Forms.Padding(0);
            this.butInicSerial.Name = "butInicSerial";
            this.butInicSerial.Size = new System.Drawing.Size(75, 23);
            this.butInicSerial.TabIndex = 1;
            this.butInicSerial.Text = "Conecta";
            this.butInicSerial.UseVisualStyleBackColor = false;
            this.butInicSerial.Click += new System.EventHandler(this.butInicSerial_Click);
            // 
            // rtbSerialOutput
            // 
            this.rtbSerialOutput.Location = new System.Drawing.Point(6, 7);
            this.rtbSerialOutput.Name = "rtbSerialOutput";
            this.rtbSerialOutput.Size = new System.Drawing.Size(312, 94);
            this.rtbSerialOutput.TabIndex = 0;
            this.rtbSerialOutput.Text = "";
            // 
            // tpTempGauges
            // 
            this.tpTempGauges.Controls.Add(this.tbMLX4);
            this.tpTempGauges.Controls.Add(this.tbMLX3);
            this.tpTempGauges.Controls.Add(this.tbMLX2);
            this.tpTempGauges.Controls.Add(this.tbMLX1);
            this.tpTempGauges.Controls.Add(this.lbMaxTemp4);
            this.tpTempGauges.Controls.Add(this.lbMinTemp4);
            this.tpTempGauges.Controls.Add(this.lbMLX4);
            this.tpTempGauges.Controls.Add(this.pbMLX4);
            this.tpTempGauges.Controls.Add(this.lbMaxTemp3);
            this.tpTempGauges.Controls.Add(this.lbMinTemp3);
            this.tpTempGauges.Controls.Add(this.lbMLX3);
            this.tpTempGauges.Controls.Add(this.pbMLX3);
            this.tpTempGauges.Controls.Add(this.lbMaxTemp2);
            this.tpTempGauges.Controls.Add(this.lbMinTemp2);
            this.tpTempGauges.Controls.Add(this.lbMLX2);
            this.tpTempGauges.Controls.Add(this.pbMLX2);
            this.tpTempGauges.Controls.Add(this.lbMaxTemp1);
            this.tpTempGauges.Controls.Add(this.lbMinTemp1);
            this.tpTempGauges.Controls.Add(this.lbMLX1);
            this.tpTempGauges.Controls.Add(this.pbMLX1);
            this.tpTempGauges.Location = new System.Drawing.Point(4, 22);
            this.tpTempGauges.Name = "tpTempGauges";
            this.tpTempGauges.Padding = new System.Windows.Forms.Padding(3);
            this.tpTempGauges.Size = new System.Drawing.Size(324, 159);
            this.tpTempGauges.TabIndex = 1;
            this.tpTempGauges.Text = "Valores Sensores";
            this.tpTempGauges.UseVisualStyleBackColor = true;
            // 
            // lbMaxTemp4
            // 
            this.lbMaxTemp4.AutoSize = true;
            this.lbMaxTemp4.Location = new System.Drawing.Point(287, 143);
            this.lbMaxTemp4.Name = "lbMaxTemp4";
            this.lbMaxTemp4.Size = new System.Drawing.Size(33, 13);
            this.lbMaxTemp4.TabIndex = 15;
            this.lbMaxTemp4.Text = "50° C";
            // 
            // lbMinTemp4
            // 
            this.lbMinTemp4.AutoSize = true;
            this.lbMinTemp4.Location = new System.Drawing.Point(172, 143);
            this.lbMinTemp4.Name = "lbMinTemp4";
            this.lbMinTemp4.Size = new System.Drawing.Size(27, 13);
            this.lbMinTemp4.TabIndex = 14;
            this.lbMinTemp4.Text = "0° C";
            // 
            // lbMLX4
            // 
            this.lbMLX4.AutoSize = true;
            this.lbMLX4.Location = new System.Drawing.Point(173, 98);
            this.lbMLX4.Name = "lbMLX4";
            this.lbMLX4.Size = new System.Drawing.Size(38, 13);
            this.lbMLX4.TabIndex = 13;
            this.lbMLX4.Text = "MLX 4";
            // 
            // pbMLX4
            // 
            this.pbMLX4.Location = new System.Drawing.Point(172, 117);
            this.pbMLX4.MarqueeAnimationSpeed = 0;
            this.pbMLX4.Maximum = 50;
            this.pbMLX4.Name = "pbMLX4";
            this.pbMLX4.Size = new System.Drawing.Size(146, 23);
            this.pbMLX4.Step = 2;
            this.pbMLX4.TabIndex = 12;
            // 
            // lbMaxTemp3
            // 
            this.lbMaxTemp3.AutoSize = true;
            this.lbMaxTemp3.Location = new System.Drawing.Point(287, 54);
            this.lbMaxTemp3.Name = "lbMaxTemp3";
            this.lbMaxTemp3.Size = new System.Drawing.Size(33, 13);
            this.lbMaxTemp3.TabIndex = 11;
            this.lbMaxTemp3.Text = "50° C";
            // 
            // lbMinTemp3
            // 
            this.lbMinTemp3.AutoSize = true;
            this.lbMinTemp3.Location = new System.Drawing.Point(172, 54);
            this.lbMinTemp3.Name = "lbMinTemp3";
            this.lbMinTemp3.Size = new System.Drawing.Size(27, 13);
            this.lbMinTemp3.TabIndex = 10;
            this.lbMinTemp3.Text = "0° C";
            // 
            // lbMLX3
            // 
            this.lbMLX3.AutoSize = true;
            this.lbMLX3.Location = new System.Drawing.Point(173, 9);
            this.lbMLX3.Name = "lbMLX3";
            this.lbMLX3.Size = new System.Drawing.Size(38, 13);
            this.lbMLX3.TabIndex = 9;
            this.lbMLX3.Text = "MLX 3";
            // 
            // pbMLX3
            // 
            this.pbMLX3.Location = new System.Drawing.Point(172, 28);
            this.pbMLX3.MarqueeAnimationSpeed = 0;
            this.pbMLX3.Maximum = 50;
            this.pbMLX3.Name = "pbMLX3";
            this.pbMLX3.Size = new System.Drawing.Size(146, 23);
            this.pbMLX3.Step = 1;
            this.pbMLX3.TabIndex = 8;
            // 
            // lbMaxTemp2
            // 
            this.lbMaxTemp2.AutoSize = true;
            this.lbMaxTemp2.Location = new System.Drawing.Point(123, 143);
            this.lbMaxTemp2.Name = "lbMaxTemp2";
            this.lbMaxTemp2.Size = new System.Drawing.Size(33, 13);
            this.lbMaxTemp2.TabIndex = 7;
            this.lbMaxTemp2.Text = "50° C";
            // 
            // lbMinTemp2
            // 
            this.lbMinTemp2.AutoSize = true;
            this.lbMinTemp2.Location = new System.Drawing.Point(8, 143);
            this.lbMinTemp2.Name = "lbMinTemp2";
            this.lbMinTemp2.Size = new System.Drawing.Size(27, 13);
            this.lbMinTemp2.TabIndex = 6;
            this.lbMinTemp2.Text = "0° C";
            // 
            // lbMLX2
            // 
            this.lbMLX2.AutoSize = true;
            this.lbMLX2.Location = new System.Drawing.Point(9, 98);
            this.lbMLX2.Name = "lbMLX2";
            this.lbMLX2.Size = new System.Drawing.Size(38, 13);
            this.lbMLX2.TabIndex = 5;
            this.lbMLX2.Text = "MLX 2";
            // 
            // pbMLX2
            // 
            this.pbMLX2.Location = new System.Drawing.Point(8, 117);
            this.pbMLX2.MarqueeAnimationSpeed = 0;
            this.pbMLX2.Maximum = 50;
            this.pbMLX2.Name = "pbMLX2";
            this.pbMLX2.Size = new System.Drawing.Size(146, 23);
            this.pbMLX2.Step = 1;
            this.pbMLX2.TabIndex = 4;
            // 
            // lbMaxTemp1
            // 
            this.lbMaxTemp1.AutoSize = true;
            this.lbMaxTemp1.Location = new System.Drawing.Point(123, 54);
            this.lbMaxTemp1.Name = "lbMaxTemp1";
            this.lbMaxTemp1.Size = new System.Drawing.Size(33, 13);
            this.lbMaxTemp1.TabIndex = 3;
            this.lbMaxTemp1.Text = "50° C";
            // 
            // lbMinTemp1
            // 
            this.lbMinTemp1.AutoSize = true;
            this.lbMinTemp1.Location = new System.Drawing.Point(8, 54);
            this.lbMinTemp1.Name = "lbMinTemp1";
            this.lbMinTemp1.Size = new System.Drawing.Size(27, 13);
            this.lbMinTemp1.TabIndex = 2;
            this.lbMinTemp1.Text = "0° C";
            // 
            // lbMLX1
            // 
            this.lbMLX1.AutoSize = true;
            this.lbMLX1.Location = new System.Drawing.Point(9, 9);
            this.lbMLX1.Name = "lbMLX1";
            this.lbMLX1.Size = new System.Drawing.Size(38, 13);
            this.lbMLX1.TabIndex = 1;
            this.lbMLX1.Text = "MLX 1";
            // 
            // pbMLX1
            // 
            this.pbMLX1.Location = new System.Drawing.Point(8, 28);
            this.pbMLX1.MarqueeAnimationSpeed = 0;
            this.pbMLX1.Maximum = 50;
            this.pbMLX1.Name = "pbMLX1";
            this.pbMLX1.Size = new System.Drawing.Size(146, 23);
            this.pbMLX1.Step = 1;
            this.pbMLX1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Crimson;
            this.button1.Location = new System.Drawing.Point(83, 130);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Desconecta";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbMLX1
            // 
            this.tbMLX1.Location = new System.Drawing.Point(53, 6);
            this.tbMLX1.Name = "tbMLX1";
            this.tbMLX1.Size = new System.Drawing.Size(51, 20);
            this.tbMLX1.TabIndex = 16;
            // 
            // tbMLX2
            // 
            this.tbMLX2.Location = new System.Drawing.Point(53, 95);
            this.tbMLX2.Name = "tbMLX2";
            this.tbMLX2.Size = new System.Drawing.Size(51, 20);
            this.tbMLX2.TabIndex = 17;
            // 
            // tbMLX3
            // 
            this.tbMLX3.Location = new System.Drawing.Point(217, 6);
            this.tbMLX3.Name = "tbMLX3";
            this.tbMLX3.Size = new System.Drawing.Size(51, 20);
            this.tbMLX3.TabIndex = 18;
            // 
            // tbMLX4
            // 
            this.tbMLX4.Location = new System.Drawing.Point(217, 95);
            this.tbMLX4.Name = "tbMLX4";
            this.tbMLX4.Size = new System.Drawing.Size(51, 20);
            this.tbMLX4.TabIndex = 19;
            // 
            // TestTemp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 188);
            this.Controls.Add(this.tabControl1);
            this.Name = "TestTemp";
            this.Text = "Teste do sensor de Temp";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tpTempGauges.ResumeLayout(false);
            this.tpTempGauges.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tpTempGauges;
        private System.Windows.Forms.Label lbMaxTemp1;
        private System.Windows.Forms.Label lbMinTemp1;
        private System.Windows.Forms.Label lbMLX1;
        private System.Windows.Forms.ProgressBar pbMLX1;
        private System.Windows.Forms.Label lbMaxTemp4;
        private System.Windows.Forms.Label lbMinTemp4;
        private System.Windows.Forms.Label lbMLX4;
        private System.Windows.Forms.ProgressBar pbMLX4;
        private System.Windows.Forms.Label lbMaxTemp3;
        private System.Windows.Forms.Label lbMinTemp3;
        private System.Windows.Forms.Label lbMLX3;
        private System.Windows.Forms.ProgressBar pbMLX3;
        private System.Windows.Forms.Label lbMaxTemp2;
        private System.Windows.Forms.Label lbMinTemp2;
        private System.Windows.Forms.Label lbMLX2;
        private System.Windows.Forms.ProgressBar pbMLX2;
        private System.Windows.Forms.RichTextBox rtbSerialOutput;
        private System.Windows.Forms.Button butInicSerial;
        private System.Windows.Forms.ComboBox cbSerialPorts;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbMLX4;
        private System.Windows.Forms.TextBox tbMLX3;
        private System.Windows.Forms.TextBox tbMLX2;
        private System.Windows.Forms.TextBox tbMLX1;
    }
}

