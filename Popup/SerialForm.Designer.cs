
namespace Spectrometer
{
    partial class SerialForm
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
            this.txtPortname = new System.Windows.Forms.ComboBox();
            this.btnSerial = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.serialPortIn = new System.IO.Ports.SerialPort(this.components);
            this.SuspendLayout();
            // 
            // txtPortname
            // 
            this.txtPortname.FormattingEnabled = true;
            this.txtPortname.Location = new System.Drawing.Point(15, 35);
            this.txtPortname.Name = "txtPortname";
            this.txtPortname.Size = new System.Drawing.Size(283, 23);
            this.txtPortname.TabIndex = 0;
            this.txtPortname.DropDown += new System.EventHandler(this.txtPortname_DropDown);
            // 
            // btnSerial
            // 
            this.btnSerial.Location = new System.Drawing.Point(223, 64);
            this.btnSerial.Name = "btnSerial";
            this.btnSerial.Size = new System.Drawing.Size(75, 23);
            this.btnSerial.TabIndex = 1;
            this.btnSerial.Text = "Connect";
            this.btnSerial.UseVisualStyleBackColor = true;
            this.btnSerial.Click += new System.EventHandler(this.btnSerial_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Please select device:";
            // 
            // SerialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 98);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSerial);
            this.Controls.Add(this.txtPortname);
            this.Name = "SerialForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HY spectrometer";
            this.Load += new System.EventHandler(this.SerialForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox txtPortname;
        private System.Windows.Forms.Button btnSerial;
        private System.Windows.Forms.Label label1;
        private System.IO.Ports.SerialPort serialPortIn;
    }
}