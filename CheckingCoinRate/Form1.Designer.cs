namespace CheckingCoinRate
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.curBalVND = new System.Windows.Forms.TextBox();
            this.allBalUSD = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.allBalVND = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.curBalUSD = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtp2pRate = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(811, 299);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(11, 317);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowTemplate.Height = 25;
            this.dataGridView2.Size = new System.Drawing.Size(812, 135);
            this.dataGridView2.TabIndex = 1;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            this.dataGridView2.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView2_DataBindingComplete);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(313, 466);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Current Balance (VND)";
            // 
            // curBalVND
            // 
            this.curBalVND.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.curBalVND.Location = new System.Drawing.Point(313, 484);
            this.curBalVND.Name = "curBalVND";
            this.curBalVND.ReadOnly = true;
            this.curBalVND.Size = new System.Drawing.Size(160, 23);
            this.curBalVND.TabIndex = 3;
            // 
            // allBalUSD
            // 
            this.allBalUSD.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.allBalUSD.Location = new System.Drawing.Point(488, 484);
            this.allBalUSD.Name = "allBalUSD";
            this.allBalUSD.ReadOnly = true;
            this.allBalUSD.Size = new System.Drawing.Size(160, 23);
            this.allBalUSD.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(488, 466);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "All Balance";
            // 
            // allBalVND
            // 
            this.allBalVND.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.allBalVND.Location = new System.Drawing.Point(663, 484);
            this.allBalVND.Name = "allBalVND";
            this.allBalVND.ReadOnly = true;
            this.allBalVND.Size = new System.Drawing.Size(160, 23);
            this.allBalVND.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(663, 466);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 15);
            this.label3.TabIndex = 6;
            this.label3.Text = "All Balance (VND)";
            // 
            // curBalUSD
            // 
            this.curBalUSD.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.curBalUSD.Location = new System.Drawing.Point(138, 484);
            this.curBalUSD.Name = "curBalUSD";
            this.curBalUSD.ReadOnly = true;
            this.curBalUSD.Size = new System.Drawing.Size(160, 23);
            this.curBalUSD.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 466);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "Current Balance";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 466);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 15);
            this.label5.TabIndex = 10;
            this.label5.Text = "VND P2P Rate";
            // 
            // txtp2pRate
            // 
            this.txtp2pRate.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtp2pRate.Location = new System.Drawing.Point(12, 484);
            this.txtp2pRate.Name = "txtp2pRate";
            this.txtp2pRate.ReadOnly = true;
            this.txtp2pRate.Size = new System.Drawing.Size(111, 23);
            this.txtp2pRate.TabIndex = 11;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 515);
            this.Controls.Add(this.txtp2pRate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.curBalUSD);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.allBalVND);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.allBalUSD);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.curBalVND);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Checking Coin Rate";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DataGridView dataGridView1;
        private DataGridView dataGridView2;
        private Label label1;
        private TextBox curBalVND;
        private TextBox allBalUSD;
        private Label label2;
        private TextBox allBalVND;
        private Label label3;
        private TextBox curBalUSD;
        private Label label4;
        private Label label5;
        private TextBox txtp2pRate;
    }
}