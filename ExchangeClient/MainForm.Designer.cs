namespace ExchangeClient
{
    partial class MainForm
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
            this.pgSetting = new System.Windows.Forms.PropertyGrid();
            this.btnLogin = new System.Windows.Forms.Button();
            this.pgOrder = new System.Windows.Forms.PropertyGrid();
            this.btnPlaceOrder = new System.Windows.Forms.Button();
            this.bnCancelOrder = new System.Windows.Forms.Button();
            this.txtTrade = new System.Windows.Forms.TextBox();
            this.txtOrderbook = new System.Windows.Forms.TextBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblTrade = new System.Windows.Forms.Label();
            this.lblOrderbook = new System.Windows.Forms.Label();
            this.lblLog = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pgSetting
            // 
            this.pgSetting.Location = new System.Drawing.Point(3, 3);
            this.pgSetting.Name = "pgSetting";
            this.pgSetting.Size = new System.Drawing.Size(205, 222);
            this.pgSetting.TabIndex = 0;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(3, 231);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(205, 28);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            // 
            // pgOrder
            // 
            this.pgOrder.Location = new System.Drawing.Point(3, 280);
            this.pgOrder.Name = "pgOrder";
            this.pgOrder.Size = new System.Drawing.Size(205, 225);
            this.pgOrder.TabIndex = 2;
            // 
            // btnPlaceOrder
            // 
            this.btnPlaceOrder.Location = new System.Drawing.Point(3, 511);
            this.btnPlaceOrder.Name = "btnPlaceOrder";
            this.btnPlaceOrder.Size = new System.Drawing.Size(205, 32);
            this.btnPlaceOrder.TabIndex = 3;
            this.btnPlaceOrder.Text = "Place/Update";
            this.btnPlaceOrder.UseVisualStyleBackColor = true;
            // 
            // bnCancelOrder
            // 
            this.bnCancelOrder.Location = new System.Drawing.Point(3, 549);
            this.bnCancelOrder.Name = "bnCancelOrder";
            this.bnCancelOrder.Size = new System.Drawing.Size(205, 33);
            this.bnCancelOrder.TabIndex = 4;
            this.bnCancelOrder.Text = "Cancel";
            this.bnCancelOrder.UseVisualStyleBackColor = true;
            // 
            // txtTrade
            // 
            this.txtTrade.Location = new System.Drawing.Point(226, 30);
            this.txtTrade.Multiline = true;
            this.txtTrade.Name = "txtTrade";
            this.txtTrade.ReadOnly = true;
            this.txtTrade.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTrade.Size = new System.Drawing.Size(664, 229);
            this.txtTrade.TabIndex = 5;
            this.txtTrade.WordWrap = false;
            // 
            // txtOrderbook
            // 
            this.txtOrderbook.Location = new System.Drawing.Point(226, 280);
            this.txtOrderbook.Multiline = true;
            this.txtOrderbook.Name = "txtOrderbook";
            this.txtOrderbook.ReadOnly = true;
            this.txtOrderbook.Size = new System.Drawing.Size(664, 300);
            this.txtOrderbook.TabIndex = 6;
            this.txtOrderbook.WordWrap = false;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(919, 30);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(418, 550);
            this.txtLog.TabIndex = 7;
            // 
            // lblTrade
            // 
            this.lblTrade.AutoSize = true;
            this.lblTrade.Location = new System.Drawing.Point(223, 9);
            this.lblTrade.Name = "lblTrade";
            this.lblTrade.Size = new System.Drawing.Size(40, 13);
            this.lblTrade.TabIndex = 8;
            this.lblTrade.Text = "Trades";
            // 
            // lblOrderbook
            // 
            this.lblOrderbook.AutoSize = true;
            this.lblOrderbook.Location = new System.Drawing.Point(226, 264);
            this.lblOrderbook.Name = "lblOrderbook";
            this.lblOrderbook.Size = new System.Drawing.Size(95, 13);
            this.lblOrderbook.TabIndex = 9;
            this.lblOrderbook.Text = "Level 2 Orderbook";
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(919, 8);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(61, 13);
            this.lblLog.TabIndex = 10;
            this.lblLog.Text = "Private Log";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1340, 585);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.lblOrderbook);
            this.Controls.Add(this.lblTrade);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.txtOrderbook);
            this.Controls.Add(this.txtTrade);
            this.Controls.Add(this.bnCancelOrder);
            this.Controls.Add(this.btnPlaceOrder);
            this.Controls.Add(this.pgOrder);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.pgSetting);
            this.Name = "MainForm";
            this.Text = "ExchangeClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgSetting;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.PropertyGrid pgOrder;
        private System.Windows.Forms.Button btnPlaceOrder;
        private System.Windows.Forms.Button bnCancelOrder;
        private System.Windows.Forms.TextBox txtTrade;
        private System.Windows.Forms.TextBox txtOrderbook;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblTrade;
        private System.Windows.Forms.Label lblOrderbook;
        private System.Windows.Forms.Label lblLog;
    }
}

