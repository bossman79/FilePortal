namespace ACACTest1
{
	partial class ColumnPicker
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
			this.BTOK = new System.Windows.Forms.Button();
			this.BTCancel = new System.Windows.Forms.Button();
			this.BTAdd = new System.Windows.Forms.Button();
			this.BTRemove = new System.Windows.Forms.Button();
			this.LVIn = new System.Windows.Forms.ListView();
			this.LVAll = new System.Windows.Forms.ListView();
			this.SuspendLayout();
			// 
			// BTOK
			// 
			this.BTOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BTOK.Location = new System.Drawing.Point(445, 12);
			this.BTOK.Name = "BTOK";
			this.BTOK.Size = new System.Drawing.Size(75, 23);
			this.BTOK.TabIndex = 2;
			this.BTOK.Text = "OK";
			this.BTOK.UseVisualStyleBackColor = true;
			this.BTOK.Click += new System.EventHandler(this.BTOK_Click);
			// 
			// BTCancel
			// 
			this.BTCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BTCancel.Location = new System.Drawing.Point(445, 41);
			this.BTCancel.Name = "BTCancel";
			this.BTCancel.Size = new System.Drawing.Size(75, 23);
			this.BTCancel.TabIndex = 3;
			this.BTCancel.Text = "Cancel";
			this.BTCancel.UseVisualStyleBackColor = true;
			// 
			// BTAdd
			// 
			this.BTAdd.Location = new System.Drawing.Point(189, 99);
			this.BTAdd.Name = "BTAdd";
			this.BTAdd.Size = new System.Drawing.Size(48, 23);
			this.BTAdd.TabIndex = 4;
			this.BTAdd.Text = "<--";
			this.BTAdd.UseVisualStyleBackColor = true;
			this.BTAdd.Click += new System.EventHandler(this.BTAdd_Click);
			// 
			// BTRemove
			// 
			this.BTRemove.Location = new System.Drawing.Point(189, 144);
			this.BTRemove.Name = "BTRemove";
			this.BTRemove.Size = new System.Drawing.Size(48, 23);
			this.BTRemove.TabIndex = 5;
			this.BTRemove.Text = "-->";
			this.BTRemove.UseVisualStyleBackColor = true;
			this.BTRemove.Click += new System.EventHandler(this.BTRemove_Click);
			// 
			// LVIn
			// 
			this.LVIn.Location = new System.Drawing.Point(12, 12);
			this.LVIn.Name = "LVIn";
			this.LVIn.Size = new System.Drawing.Size(171, 367);
			this.LVIn.TabIndex = 6;
			this.LVIn.UseCompatibleStateImageBehavior = false;
			this.LVIn.View = System.Windows.Forms.View.Details;
			// 
			// LVAll
			// 
			this.LVAll.Location = new System.Drawing.Point(243, 12);
			this.LVAll.Name = "LVAll";
			this.LVAll.Size = new System.Drawing.Size(171, 367);
			this.LVAll.TabIndex = 7;
			this.LVAll.UseCompatibleStateImageBehavior = false;
			this.LVAll.View = System.Windows.Forms.View.Details;
			// 
			// ColumnPicker
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(532, 391);
			this.Controls.Add(this.LVAll);
			this.Controls.Add(this.LVIn);
			this.Controls.Add(this.BTRemove);
			this.Controls.Add(this.BTAdd);
			this.Controls.Add(this.BTCancel);
			this.Controls.Add(this.BTOK);
			this.Name = "ColumnPicker";
			this.Text = "ColumnPicker";
			this.Load += new System.EventHandler(this.ColumnPicker_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BTOK;
		private System.Windows.Forms.Button BTCancel;
		private System.Windows.Forms.Button BTAdd;
		private System.Windows.Forms.Button BTRemove;
		private System.Windows.Forms.ListView LVIn;
		private System.Windows.Forms.ListView LVAll;
	}
}