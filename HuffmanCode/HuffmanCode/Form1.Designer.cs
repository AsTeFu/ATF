namespace HuffmanCode {
    partial class Form1 {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent() {
            this.input = new System.Windows.Forms.TextBox();
            this.codesText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.output = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openHuffmanTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveHuffmanTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compressButton = new System.Windows.Forms.Button();
            this.uncompressButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // input
            // 
            this.input.Location = new System.Drawing.Point(39, 47);
            this.input.Multiline = true;
            this.input.Name = "input";
            this.input.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.input.Size = new System.Drawing.Size(427, 173);
            this.input.TabIndex = 2;
            // 
            // codesText
            // 
            this.codesText.Location = new System.Drawing.Point(489, 47);
            this.codesText.Multiline = true;
            this.codesText.Name = "codesText";
            this.codesText.ReadOnly = true;
            this.codesText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.codesText.Size = new System.Drawing.Size(173, 431);
            this.codesText.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(556, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Codes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(214, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Input";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(214, 230);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Output";
            // 
            // output
            // 
            this.output.Location = new System.Drawing.Point(39, 246);
            this.output.Multiline = true;
            this.output.Name = "output";
            this.output.ReadOnly = true;
            this.output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.output.Size = new System.Drawing.Size(427, 173);
            this.output.TabIndex = 8;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(701, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileToolStripMenuItem,
            this.openHuffmanTextToolStripMenuItem,
            this.saveHuffmanTextToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // openFileToolStripMenuItem
            // 
            this.openFileToolStripMenuItem.Name = "openFileToolStripMenuItem";
            this.openFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openFileToolStripMenuItem.Text = "Open file";
            // 
            // openHuffmanTextToolStripMenuItem
            // 
            this.openHuffmanTextToolStripMenuItem.Name = "openHuffmanTextToolStripMenuItem";
            this.openHuffmanTextToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.openHuffmanTextToolStripMenuItem.Text = "Open Huffman file";
            // 
            // saveHuffmanTextToolStripMenuItem
            // 
            this.saveHuffmanTextToolStripMenuItem.Name = "saveHuffmanTextToolStripMenuItem";
            this.saveHuffmanTextToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveHuffmanTextToolStripMenuItem.Text = "Save Huffman text";
            this.saveHuffmanTextToolStripMenuItem.Click += new System.EventHandler(this.saveHuffmanTextToolStripMenuItem_Click);
            // 
            // compressButton
            // 
            this.compressButton.Location = new System.Drawing.Point(39, 438);
            this.compressButton.Name = "compressButton";
            this.compressButton.Size = new System.Drawing.Size(204, 40);
            this.compressButton.TabIndex = 10;
            this.compressButton.Text = "Compress";
            this.compressButton.UseVisualStyleBackColor = true;
            this.compressButton.Click += new System.EventHandler(this.compressButton_Click);
            // 
            // uncompressButton
            // 
            this.uncompressButton.Location = new System.Drawing.Point(262, 438);
            this.uncompressButton.Name = "uncompressButton";
            this.uncompressButton.Size = new System.Drawing.Size(204, 40);
            this.uncompressButton.TabIndex = 11;
            this.uncompressButton.Text = "Uncompress(soon)";
            this.uncompressButton.UseVisualStyleBackColor = true;
            this.uncompressButton.Click += new System.EventHandler(this.uncompressButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 504);
            this.Controls.Add(this.uncompressButton);
            this.Controls.Add(this.compressButton);
            this.Controls.Add(this.output);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.codesText);
            this.Controls.Add(this.input);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox input;
        private System.Windows.Forms.TextBox codesText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox output;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openHuffmanTextToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveHuffmanTextToolStripMenuItem;
        private System.Windows.Forms.Button compressButton;
        private System.Windows.Forms.Button uncompressButton;
    }
}

