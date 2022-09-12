namespace Ex05.CheckersUI
{
    partial class GameSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameSettingsForm));
            this.boardSizeLabel = new System.Windows.Forms.Label();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton10 = new System.Windows.Forms.RadioButton();
            this.playersLabel = new System.Windows.Forms.Label();
            this.player1TextBox = new System.Windows.Forms.TextBox();
            this.player2TextBox = new System.Windows.Forms.TextBox();
            this.player1Label = new System.Windows.Forms.Label();
            this.player2CheckBox = new System.Windows.Forms.CheckBox();
            this.doneButton = new System.Windows.Forms.Button();
            this.showMovesCB = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // boardSizeLabel
            // 
            this.boardSizeLabel.AutoSize = true;
            this.boardSizeLabel.Location = new System.Drawing.Point(16, 16);
            this.boardSizeLabel.Name = "boardSizeLabel";
            this.boardSizeLabel.Size = new System.Drawing.Size(61, 13);
            this.boardSizeLabel.TabIndex = 0;
            this.boardSizeLabel.Text = "Board Size:";
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(33, 34);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(48, 17);
            this.radioButton6.TabIndex = 1;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "6 x 6";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(87, 34);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(48, 17);
            this.radioButton8.TabIndex = 2;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "8 x 8";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // radioButton10
            // 
            this.radioButton10.AutoSize = true;
            this.radioButton10.BackColor = System.Drawing.SystemColors.Control;
            this.radioButton10.Cursor = System.Windows.Forms.Cursors.Default;
            this.radioButton10.Location = new System.Drawing.Point(141, 34);
            this.radioButton10.Name = "radioButton10";
            this.radioButton10.Size = new System.Drawing.Size(60, 17);
            this.radioButton10.TabIndex = 3;
            this.radioButton10.TabStop = true;
            this.radioButton10.Text = "10 x 10";
            this.radioButton10.UseVisualStyleBackColor = false;
            // 
            // playersLabel
            // 
            this.playersLabel.AutoSize = true;
            this.playersLabel.Location = new System.Drawing.Point(16, 58);
            this.playersLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.playersLabel.Name = "playersLabel";
            this.playersLabel.Size = new System.Drawing.Size(44, 13);
            this.playersLabel.TabIndex = 4;
            this.playersLabel.Text = "Players:";
            // 
            // player1TextBox
            // 
            this.player1TextBox.Location = new System.Drawing.Point(104, 78);
            this.player1TextBox.Margin = new System.Windows.Forms.Padding(2);
            this.player1TextBox.MaxLength = 10;
            this.player1TextBox.Name = "player1TextBox";
            this.player1TextBox.Size = new System.Drawing.Size(112, 20);
            this.player1TextBox.TabIndex = 5;
            // 
            // player2TextBox
            // 
            this.player2TextBox.Enabled = false;
            this.player2TextBox.Location = new System.Drawing.Point(104, 105);
            this.player2TextBox.Margin = new System.Windows.Forms.Padding(2);
            this.player2TextBox.MaxLength = 10;
            this.player2TextBox.Name = "player2TextBox";
            this.player2TextBox.Size = new System.Drawing.Size(112, 20);
            this.player2TextBox.TabIndex = 6;
            this.player2TextBox.Text = "[Computer]";
            // 
            // player1Label
            // 
            this.player1Label.AutoSize = true;
            this.player1Label.Location = new System.Drawing.Point(17, 82);
            this.player1Label.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.player1Label.Name = "player1Label";
            this.player1Label.Size = new System.Drawing.Size(48, 13);
            this.player1Label.TabIndex = 7;
            this.player1Label.Text = "Player 1:";
            // 
            // player2CheckBox
            // 
            this.player2CheckBox.AutoSize = true;
            this.player2CheckBox.Location = new System.Drawing.Point(19, 106);
            this.player2CheckBox.Margin = new System.Windows.Forms.Padding(2);
            this.player2CheckBox.Name = "player2CheckBox";
            this.player2CheckBox.Size = new System.Drawing.Size(67, 17);
            this.player2CheckBox.TabIndex = 8;
            this.player2CheckBox.Text = "Player 2:";
            this.player2CheckBox.UseVisualStyleBackColor = true;
            this.player2CheckBox.CheckedChanged += new System.EventHandler(this.player2CheckBox_CheckedChanged);
            // 
            // doneButton
            // 
            this.doneButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.doneButton.Location = new System.Drawing.Point(155, 161);
            this.doneButton.Margin = new System.Windows.Forms.Padding(2);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(76, 22);
            this.doneButton.TabIndex = 9;
            this.doneButton.Text = "Done";
            this.doneButton.UseVisualStyleBackColor = true;
            this.doneButton.Click += new System.EventHandler(this.doneButton_Click);
            // 
            // showMovesCB
            // 
            this.showMovesCB.AutoSize = true;
            this.showMovesCB.Checked = true;
            this.showMovesCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showMovesCB.Location = new System.Drawing.Point(19, 136);
            this.showMovesCB.Margin = new System.Windows.Forms.Padding(2);
            this.showMovesCB.Name = "showMovesCB";
            this.showMovesCB.Size = new System.Drawing.Size(128, 17);
            this.showMovesCB.TabIndex = 10;
            this.showMovesCB.Text = "Show possible moves";
            this.showMovesCB.UseVisualStyleBackColor = true;
            // 
            // GameSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 192);
            this.Controls.Add(this.showMovesCB);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.player2CheckBox);
            this.Controls.Add(this.player1Label);
            this.Controls.Add(this.player2TextBox);
            this.Controls.Add(this.player1TextBox);
            this.Controls.Add(this.playersLabel);
            this.Controls.Add(this.radioButton10);
            this.Controls.Add(this.radioButton8);
            this.Controls.Add(this.radioButton6);
            this.Controls.Add(this.boardSizeLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameSettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Game Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label boardSizeLabel;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton10;
        private System.Windows.Forms.Label playersLabel;
        private System.Windows.Forms.TextBox player1TextBox;
        private System.Windows.Forms.TextBox player2TextBox;
        private System.Windows.Forms.Label player1Label;
        private System.Windows.Forms.CheckBox player2CheckBox;
        private System.Windows.Forms.Button doneButton;
        private System.Windows.Forms.CheckBox showMovesCB;
    }
}