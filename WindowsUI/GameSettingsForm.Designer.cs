namespace WindowsUI
{
    public partial class GameSettingsForm
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
            this.BoardSizeButton = new System.Windows.Forms.Button();
            this.AgainstComputerButton = new System.Windows.Forms.Button();
            this.AgainstPlayerButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BoardSizeButton
            // 
            this.BoardSizeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BoardSizeButton.BackColor = System.Drawing.SystemColors.Control;
            this.BoardSizeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.BoardSizeButton.Location = new System.Drawing.Point(80, 32);
            this.BoardSizeButton.Name = "BoardSizeButton";
            this.BoardSizeButton.Size = new System.Drawing.Size(401, 74);
            this.BoardSizeButton.TabIndex = 0;
            this.BoardSizeButton.Text = "Board Size: 6X6 (Click Here To Increase)";
            this.BoardSizeButton.UseVisualStyleBackColor = true;
            this.BoardSizeButton.Click += new System.EventHandler(this.BoardSizeButton_Click);
            // 
            // AgainstComputerButton
            // 
            this.AgainstComputerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AgainstComputerButton.BackColor = System.Drawing.SystemColors.Control;
            this.AgainstComputerButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.AgainstComputerButton.Location = new System.Drawing.Point(42, 124);
            this.AgainstComputerButton.Name = "AgainstComputerButton";
            this.AgainstComputerButton.Size = new System.Drawing.Size(229, 46);
            this.AgainstComputerButton.TabIndex = 1;
            this.AgainstComputerButton.Text = "Play Against The Computer";
            this.AgainstComputerButton.UseVisualStyleBackColor = true;
            this.AgainstComputerButton.Click += new System.EventHandler(this.AgainstComputerButton_Click);
            // 
            // AgainstPlayerButton
            // 
            this.AgainstPlayerButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AgainstPlayerButton.BackColor = System.Drawing.SystemColors.Control;
            this.AgainstPlayerButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.AgainstPlayerButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.AgainstPlayerButton.Location = new System.Drawing.Point(290, 124);
            this.AgainstPlayerButton.Name = "AgainstPlayerButton";
            this.AgainstPlayerButton.Size = new System.Drawing.Size(229, 46);
            this.AgainstPlayerButton.TabIndex = 2;
            this.AgainstPlayerButton.Text = "Play Against Your Friend";
            this.AgainstPlayerButton.UseVisualStyleBackColor = false;
            this.AgainstPlayerButton.Click += new System.EventHandler(this.AgainstPlayerButton_Click);
            // 
            // GameSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(571, 199);
            this.Controls.Add(this.AgainstPlayerButton);
            this.Controls.Add(this.AgainstComputerButton);
            this.Controls.Add(this.BoardSizeButton);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GameSettingsForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Othello - Game Settings";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BoardSizeButton;
        private System.Windows.Forms.Button AgainstComputerButton;
        private System.Windows.Forms.Button AgainstPlayerButton;
    }
}