namespace AP_Lists
{
	partial class ContentsEdit
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
            this.LVIn = new System.Windows.Forms.ListView();
            this.LVOut = new System.Windows.Forms.ListView();
            this.BTAdd = new System.Windows.Forms.Button();
            this.BTRemove = new System.Windows.Forms.Button();
            this.BTEdit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LBLAvailableMessage = new System.Windows.Forms.Label();
            this.BTClose = new System.Windows.Forms.Button();
            this.TVOut = new System.Windows.Forms.TreeView();
            this.BTRemoveAll = new System.Windows.Forms.Button();
            this.BTUp = new System.Windows.Forms.Button();
            this.BTDown = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // LVIn
            // 
            this.LVIn.FullRowSelect = true;
            this.LVIn.HideSelection = false;
            this.LVIn.Location = new System.Drawing.Point(12, 66);
            this.LVIn.Name = "LVIn";
            this.LVIn.Size = new System.Drawing.Size(283, 397);
            this.LVIn.TabIndex = 0;
            this.LVIn.UseCompatibleStateImageBehavior = false;
            this.LVIn.View = System.Windows.Forms.View.Details;
            // 
            // LVOut
            // 
            this.LVOut.FullRowSelect = true;
            this.LVOut.HideSelection = false;
            this.LVOut.Location = new System.Drawing.Point(483, 66);
            this.LVOut.Name = "LVOut";
            this.LVOut.Size = new System.Drawing.Size(303, 397);
            this.LVOut.TabIndex = 1;
            this.LVOut.UseCompatibleStateImageBehavior = false;
            this.LVOut.View = System.Windows.Forms.View.Details;
            // 
            // BTAdd
            // 
            this.BTAdd.Location = new System.Drawing.Point(358, 152);
            this.BTAdd.Name = "BTAdd";
            this.BTAdd.Size = new System.Drawing.Size(75, 23);
            this.BTAdd.TabIndex = 2;
            this.BTAdd.Text = "<--";
            this.BTAdd.UseVisualStyleBackColor = true;
            this.BTAdd.Click += new System.EventHandler(this.BTAdd_Click);
            // 
            // BTRemove
            // 
            this.BTRemove.Location = new System.Drawing.Point(358, 225);
            this.BTRemove.Name = "BTRemove";
            this.BTRemove.Size = new System.Drawing.Size(75, 23);
            this.BTRemove.TabIndex = 3;
            this.BTRemove.Text = "-->";
            this.BTRemove.UseVisualStyleBackColor = true;
            this.BTRemove.Click += new System.EventHandler(this.BTRemove_Click);
            // 
            // BTEdit
            // 
            this.BTEdit.Location = new System.Drawing.Point(220, 34);
            this.BTEdit.Name = "BTEdit";
            this.BTEdit.Size = new System.Drawing.Size(75, 23);
            this.BTEdit.TabIndex = 4;
            this.BTEdit.Text = "Edit";
            this.BTEdit.UseVisualStyleBackColor = true;
            this.BTEdit.Click += new System.EventHandler(this.BTEdit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Contents in List";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(480, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Available to add";
            // 
            // LBLAvailableMessage
            // 
            this.LBLAvailableMessage.AutoSize = true;
            this.LBLAvailableMessage.Location = new System.Drawing.Point(480, 44);
            this.LBLAvailableMessage.Name = "LBLAvailableMessage";
            this.LBLAvailableMessage.Size = new System.Drawing.Size(16, 13);
            this.LBLAvailableMessage.TabIndex = 7;
            this.LBLAvailableMessage.Text = "...";
            // 
            // BTClose
            // 
            this.BTClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BTClose.Location = new System.Drawing.Point(711, 9);
            this.BTClose.Name = "BTClose";
            this.BTClose.Size = new System.Drawing.Size(75, 23);
            this.BTClose.TabIndex = 8;
            this.BTClose.Text = "Close";
            this.BTClose.UseVisualStyleBackColor = true;
            // 
            // TVOut
            // 
            this.TVOut.Location = new System.Drawing.Point(465, 66);
            this.TVOut.Name = "TVOut";
            this.TVOut.Size = new System.Drawing.Size(304, 397);
            this.TVOut.TabIndex = 9;
            this.TVOut.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.TVOut_AfterExpand);
            // 
            // BTRemoveAll
            // 
            this.BTRemoveAll.Location = new System.Drawing.Point(358, 264);
            this.BTRemoveAll.Name = "BTRemoveAll";
            this.BTRemoveAll.Size = new System.Drawing.Size(75, 23);
            this.BTRemoveAll.TabIndex = 10;
            this.BTRemoveAll.Text = "-->>";
            this.BTRemoveAll.UseVisualStyleBackColor = true;
            this.BTRemoveAll.Click += new System.EventHandler(this.BTRemoveAll_Click);
            // 
            // BTUp
            // 
            this.BTUp.Location = new System.Drawing.Point(301, 83);
            this.BTUp.Name = "BTUp";
            this.BTUp.Size = new System.Drawing.Size(45, 23);
            this.BTUp.TabIndex = 11;
            this.BTUp.Text = "Up";
            this.BTUp.UseVisualStyleBackColor = true;
            this.BTUp.Click += new System.EventHandler(this.BTUp_Click);
            // 
            // BTDown
            // 
            this.BTDown.Location = new System.Drawing.Point(301, 112);
            this.BTDown.Name = "BTDown";
            this.BTDown.Size = new System.Drawing.Size(45, 23);
            this.BTDown.TabIndex = 12;
            this.BTDown.Text = "Down";
            this.BTDown.UseVisualStyleBackColor = true;
            this.BTDown.Click += new System.EventHandler(this.BTDown_Click);
            // 
            // ContentsEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 475);
            this.Controls.Add(this.BTDown);
            this.Controls.Add(this.BTUp);
            this.Controls.Add(this.BTRemoveAll);
            this.Controls.Add(this.TVOut);
            this.Controls.Add(this.BTClose);
            this.Controls.Add(this.LBLAvailableMessage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BTEdit);
            this.Controls.Add(this.BTRemove);
            this.Controls.Add(this.BTAdd);
            this.Controls.Add(this.LVOut);
            this.Controls.Add(this.LVIn);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "ContentsEdit";
            this.Text = "ContentsEdit";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ContentsEdit_FormClosing);
            this.Load += new System.EventHandler(this.ContentsEdit_Load);
            this.Resize += new System.EventHandler(this.ContentsEdit_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView LVIn;
		private System.Windows.Forms.ListView LVOut;
		private System.Windows.Forms.Button BTAdd;
		private System.Windows.Forms.Button BTRemove;
		private System.Windows.Forms.Button BTEdit;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label LBLAvailableMessage;
		private System.Windows.Forms.Button BTClose;
		private System.Windows.Forms.TreeView TVOut;
        private System.Windows.Forms.Button BTRemoveAll;
        private System.Windows.Forms.Button BTUp;
        private System.Windows.Forms.Button BTDown;
	}
}