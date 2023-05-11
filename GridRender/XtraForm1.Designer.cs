namespace GridRender
{
    partial class XtraForm1
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
        public DevExpress.XtraEditors.TrackBarControl GetTrackBar1()
        {
            return trackBarControl1;
        }
        public DevExpress.XtraEditors.TrackBarControl GetTrackBar2()
        {
            return trackBarControl2;
        }
        public DevExpress.XtraEditors.TrackBarControl GetTrackBar3()
        {
            return trackBarControl3;
        }
        public DevExpress.XtraEditors.CheckEdit GetCheck1()
        {
            return checkEdit1;
        }
        public DevExpress.XtraEditors.CheckEdit GetCheck2()
        {
            return checkEdit2;
        }
        public DevExpress.XtraEditors.CheckEdit GetCheck3()
        {
            return checkEdit3;
        }
        public DevExpress.XtraEditors.CheckEdit GetFullCheck()
        {
            return checkEdit4;
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.simpleButton3 = new DevExpress.XtraEditors.SimpleButton();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.trackBarControl3 = new DevExpress.XtraEditors.TrackBarControl();
            this.trackBarControl2 = new DevExpress.XtraEditors.TrackBarControl();
            this.trackBarControl1 = new DevExpress.XtraEditors.TrackBarControl();
            this.checkEdit4 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit3 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit2 = new DevExpress.XtraEditors.CheckEdit();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit4.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.simpleButton3);
            this.panelControl1.Controls.Add(this.simpleButton1);
            this.panelControl1.Controls.Add(this.labelControl3);
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.trackBarControl3);
            this.panelControl1.Controls.Add(this.trackBarControl2);
            this.panelControl1.Controls.Add(this.trackBarControl1);
            this.panelControl1.Controls.Add(this.checkEdit4);
            this.panelControl1.Controls.Add(this.checkEdit3);
            this.panelControl1.Controls.Add(this.checkEdit2);
            this.panelControl1.Controls.Add(this.checkEdit1);
            this.panelControl1.Location = new System.Drawing.Point(-2, -14);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(6);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(308, 789);
            this.panelControl1.TabIndex = 0;
            this.panelControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.panelControl1_Paint);
            // 
            // simpleButton3
            // 
            this.simpleButton3.AllowFocus = false;
            this.simpleButton3.Location = new System.Drawing.Point(22, 290);
            this.simpleButton3.Name = "simpleButton3";
            this.simpleButton3.Size = new System.Drawing.Size(53, 36);
            this.simpleButton3.TabIndex = 9;
            this.simpleButton3.Text = "<";
            this.simpleButton3.Click += new System.EventHandler(this.simpleButton3_Click);
            // 
            // simpleButton1
            // 
            this.simpleButton1.AllowFocus = false;
            this.simpleButton1.Location = new System.Drawing.Point(229, 290);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(53, 36);
            this.simpleButton1.TabIndex = 1;
            this.simpleButton1.Text = ">";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(99, 300);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(89, 16);
            this.labelControl3.TabIndex = 7;
            this.labelControl3.Text = "Imported_PPRS";
            this.labelControl3.Click += new System.EventHandler(this.labelControl3_Click);
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(9, 366);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(210, 176);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Controls: \r\nW - forward\r\nS - backward\r\nA - left\r\nD - right\r\nSpace - up\r\nShift - d" +
    "own\r\nF - focus/unfocus on grid\r\nRight mouse button - control camera\r\n\r\n\r\n";
            this.labelControl2.Click += new System.EventHandler(this.labelControl2_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(0, 0);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(75, 16);
            this.labelControl1.TabIndex = 6;
            this.labelControl1.Text = "labelControl1";
            // 
            // trackBarControl3
            // 
            this.trackBarControl3.EditValue = null;
            this.trackBarControl3.Location = new System.Drawing.Point(80, 231);
            this.trackBarControl3.Margin = new System.Windows.Forms.Padding(10);
            this.trackBarControl3.Name = "trackBarControl3";
            this.trackBarControl3.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.trackBarControl3.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.trackBarControl3.Size = new System.Drawing.Size(201, 56);
            this.trackBarControl3.TabIndex = 5;
            // 
            // trackBarControl2
            // 
            this.trackBarControl2.EditValue = null;
            this.trackBarControl2.Location = new System.Drawing.Point(80, 150);
            this.trackBarControl2.Margin = new System.Windows.Forms.Padding(8);
            this.trackBarControl2.Name = "trackBarControl2";
            this.trackBarControl2.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.trackBarControl2.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.trackBarControl2.Size = new System.Drawing.Size(202, 56);
            this.trackBarControl2.TabIndex = 4;
            this.trackBarControl2.EditValueChanged += new System.EventHandler(this.trackBarControl2_EditValueChanged);
            // 
            // trackBarControl1
            // 
            this.trackBarControl1.EditValue = null;
            this.trackBarControl1.Location = new System.Drawing.Point(80, 62);
            this.trackBarControl1.Margin = new System.Windows.Forms.Padding(6);
            this.trackBarControl1.Name = "trackBarControl1";
            this.trackBarControl1.Properties.LabelAppearance.Options.UseTextOptions = true;
            this.trackBarControl1.Properties.LabelAppearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.trackBarControl1.Size = new System.Drawing.Size(202, 56);
            this.trackBarControl1.TabIndex = 1;
            this.trackBarControl1.EditValueChanged += new System.EventHandler(this.trackBarControl1_EditValueChanged);
            // 
            // checkEdit4
            // 
            this.checkEdit4.EditValue = true;
            this.checkEdit4.Location = new System.Drawing.Point(9, 24);
            this.checkEdit4.Margin = new System.Windows.Forms.Padding(8);
            this.checkEdit4.Name = "checkEdit4";
            this.checkEdit4.Properties.Caption = "Full";
            this.checkEdit4.Size = new System.Drawing.Size(119, 24);
            this.checkEdit4.TabIndex = 3;
            // 
            // checkEdit3
            // 
            this.checkEdit3.EditValue = true;
            this.checkEdit3.Location = new System.Drawing.Point(9, 227);
            this.checkEdit3.Margin = new System.Windows.Forms.Padding(8);
            this.checkEdit3.Name = "checkEdit3";
            this.checkEdit3.Properties.Caption = "Z";
            this.checkEdit3.Size = new System.Drawing.Size(35, 24);
            this.checkEdit3.TabIndex = 2;
            // 
            // checkEdit2
            // 
            this.checkEdit2.EditValue = true;
            this.checkEdit2.Location = new System.Drawing.Point(9, 146);
            this.checkEdit2.Margin = new System.Windows.Forms.Padding(6);
            this.checkEdit2.Name = "checkEdit2";
            this.checkEdit2.Properties.Caption = "Y";
            this.checkEdit2.Size = new System.Drawing.Size(52, 24);
            this.checkEdit2.TabIndex = 1;
            this.checkEdit2.CheckedChanged += new System.EventHandler(this.checkEdit2_CheckedChanged);
            // 
            // checkEdit1
            // 
            this.checkEdit1.EditValue = true;
            this.checkEdit1.Location = new System.Drawing.Point(9, 58);
            this.checkEdit1.Margin = new System.Windows.Forms.Padding(6);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "X";
            this.checkEdit1.Size = new System.Drawing.Size(51, 24);
            this.checkEdit1.TabIndex = 1;
            this.checkEdit1.CheckedChanged += new System.EventHandler(this.checkEdit1_CheckedChanged);
            // 
            // XtraForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 748);
            this.Controls.Add(this.panelControl1);
            this.KeyPreview = true;
            this.Name = "XtraForm1";
            this.Load += new System.EventHandler(this.XtraForm1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit4.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit3.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit2.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.CheckEdit checkEdit3;
        private DevExpress.XtraEditors.CheckEdit checkEdit2;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
        private DevExpress.XtraEditors.CheckEdit checkEdit4;
        private DevExpress.XtraEditors.TrackBarControl trackBarControl3;
        private DevExpress.XtraEditors.TrackBarControl trackBarControl2;
        private DevExpress.XtraEditors.TrackBarControl trackBarControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton simpleButton3;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.LabelControl labelControl3;
    }
}