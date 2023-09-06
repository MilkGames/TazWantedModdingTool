namespace Taz_Wanted_Modding_Tool
{
    partial class Form1
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.convertAllBmps = new System.Windows.Forms.Button();
            this.convertGif = new System.Windows.Forms.Button();
            this.convertTga = new System.Windows.Forms.Button();
            this.convertWav = new System.Windows.Forms.Button();
            this.SoundNote = new System.Windows.Forms.Label();
            this.SoXpath = new System.Windows.Forms.TextBox();
            this.ps2bmp = new System.Windows.Forms.Button();
            this.xbpbmp = new System.Windows.Forms.Button();
            this.gcpbmp = new System.Windows.Forms.Button();
            this.findoffsets = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.convertBmp = new System.Windows.Forms.Button();
            this.Note = new System.Windows.Forms.Label();
            this.convertAllGifs = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.allFilesPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.saveFile = new System.Windows.Forms.SaveFileDialog();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.openFile2 = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.convertAllBmps, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.convertGif, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.convertTga, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.convertWav, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.SoundNote, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.SoXpath, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.ps2bmp, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.xbpbmp, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.gcpbmp, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.findoffsets, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.richTextBox1, 0, 11);
            this.tableLayoutPanel1.Controls.Add(this.convertBmp, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Note, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.convertAllGifs, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.allFilesPath, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(50);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 12;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 42F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 31F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 11F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(463, 625);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // convertAllBmps
            // 
            this.convertAllBmps.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.convertAllBmps.Location = new System.Drawing.Point(277, 8);
            this.convertAllBmps.Name = "convertAllBmps";
            this.convertAllBmps.Size = new System.Drawing.Size(140, 23);
            this.convertAllBmps.TabIndex = 5;
            this.convertAllBmps.Text = "Convert all bmps";
            this.convertAllBmps.UseVisualStyleBackColor = true;
            this.convertAllBmps.Click += new System.EventHandler(this.convertAllBmps_Click);
            // 
            // convertGif
            // 
            this.convertGif.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.convertGif.Location = new System.Drawing.Point(45, 48);
            this.convertGif.Name = "convertGif";
            this.convertGif.Size = new System.Drawing.Size(140, 23);
            this.convertGif.TabIndex = 0;
            this.convertGif.Text = "Convert gif";
            this.convertGif.UseVisualStyleBackColor = true;
            this.convertGif.Click += new System.EventHandler(this.convertGif_Click);
            // 
            // convertTga
            // 
            this.convertTga.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.convertTga.Enabled = false;
            this.convertTga.Location = new System.Drawing.Point(45, 88);
            this.convertTga.Name = "convertTga";
            this.convertTga.Size = new System.Drawing.Size(140, 23);
            this.convertTga.TabIndex = 0;
            this.convertTga.Text = "Convert tga";
            this.convertTga.UseVisualStyleBackColor = true;
            this.convertTga.Click += new System.EventHandler(this.convertTga_Click);
            // 
            // convertWav
            // 
            this.convertWav.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.convertWav.Enabled = false;
            this.convertWav.Location = new System.Drawing.Point(45, 128);
            this.convertWav.Name = "convertWav";
            this.convertWav.Size = new System.Drawing.Size(140, 23);
            this.convertWav.TabIndex = 0;
            this.convertWav.Text = "Convert wav (sounds)";
            this.convertWav.UseVisualStyleBackColor = true;
            this.convertWav.Click += new System.EventHandler(this.convertWav_Click);
            // 
            // SoundNote
            // 
            this.SoundNote.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.SoundNote.AutoSize = true;
            this.SoundNote.Location = new System.Drawing.Point(88, 179);
            this.SoundNote.Name = "SoundNote";
            this.SoundNote.Size = new System.Drawing.Size(54, 13);
            this.SoundNote.TabIndex = 1;
            this.SoundNote.Text = "SoX path:";
            // 
            // SoXpath
            // 
            this.SoXpath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SoXpath.Location = new System.Drawing.Point(3, 195);
            this.SoXpath.Name = "SoXpath";
            this.SoXpath.Size = new System.Drawing.Size(225, 20);
            this.SoXpath.TabIndex = 2;
            this.SoXpath.Text = "C:\\Program Files (x86)\\sox-14-4-2\\sox.exe";
            // 
            // ps2bmp
            // 
            this.ps2bmp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ps2bmp.Enabled = false;
            this.ps2bmp.Location = new System.Drawing.Point(45, 270);
            this.ps2bmp.Name = "ps2bmp";
            this.ps2bmp.Size = new System.Drawing.Size(140, 23);
            this.ps2bmp.TabIndex = 3;
            this.ps2bmp.Text = "Convert ps2 bmp";
            this.ps2bmp.UseVisualStyleBackColor = true;
            this.ps2bmp.Click += new System.EventHandler(this.ps2bmp_Click);
            // 
            // xbpbmp
            // 
            this.xbpbmp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.xbpbmp.Enabled = false;
            this.xbpbmp.Location = new System.Drawing.Point(45, 305);
            this.xbpbmp.Name = "xbpbmp";
            this.xbpbmp.Size = new System.Drawing.Size(140, 23);
            this.xbpbmp.TabIndex = 3;
            this.xbpbmp.Text = "Convert xbp bmp";
            this.xbpbmp.UseVisualStyleBackColor = true;
            this.xbpbmp.Click += new System.EventHandler(this.xbpbmp_Click);
            // 
            // gcpbmp
            // 
            this.gcpbmp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.gcpbmp.Enabled = false;
            this.gcpbmp.Location = new System.Drawing.Point(45, 336);
            this.gcpbmp.Name = "gcpbmp";
            this.gcpbmp.Size = new System.Drawing.Size(140, 23);
            this.gcpbmp.TabIndex = 3;
            this.gcpbmp.Text = "Convert gcp bmp";
            this.gcpbmp.UseVisualStyleBackColor = true;
            this.gcpbmp.Click += new System.EventHandler(this.gcpbmp_Click);
            // 
            // findoffsets
            // 
            this.findoffsets.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.findoffsets.Enabled = false;
            this.findoffsets.Location = new System.Drawing.Point(45, 368);
            this.findoffsets.Name = "findoffsets";
            this.findoffsets.Size = new System.Drawing.Size(140, 20);
            this.findoffsets.TabIndex = 3;
            this.findoffsets.Text = "Find offsets";
            this.findoffsets.UseVisualStyleBackColor = true;
            this.findoffsets.Click += new System.EventHandler(this.findoffsets_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(3, 397);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(225, 197);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // convertBmp
            // 
            this.convertBmp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.convertBmp.Location = new System.Drawing.Point(45, 8);
            this.convertBmp.Name = "convertBmp";
            this.convertBmp.Size = new System.Drawing.Size(140, 23);
            this.convertBmp.TabIndex = 0;
            this.convertBmp.Text = "Convert bmp";
            this.convertBmp.UseVisualStyleBackColor = true;
            this.convertBmp.Click += new System.EventHandler(this.convertBmp_Click);
            // 
            // Note
            // 
            this.Note.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Note.AutoSize = true;
            this.Note.Location = new System.Drawing.Point(9, 223);
            this.Note.Name = "Note";
            this.Note.Size = new System.Drawing.Size(212, 39);
            this.Note.TabIndex = 1;
            this.Note.Text = "This program uses to convert files recieved from QuickBms and blitz.bms script af" +
    "ter unpacking .pc game files";
            // 
            // convertAllGifs
            // 
            this.convertAllGifs.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.convertAllGifs.Location = new System.Drawing.Point(277, 48);
            this.convertAllGifs.Name = "convertAllGifs";
            this.convertAllGifs.Size = new System.Drawing.Size(140, 23);
            this.convertAllGifs.TabIndex = 6;
            this.convertAllGifs.Text = "Convert all gifs";
            this.convertAllGifs.UseVisualStyleBackColor = true;
            this.convertAllGifs.Click += new System.EventHandler(this.convertAllGifs_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(288, 179);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "All files decrypting path:";
            // 
            // allFilesPath
            // 
            this.allFilesPath.Location = new System.Drawing.Point(234, 195);
            this.allFilesPath.Name = "allFilesPath";
            this.allFilesPath.Size = new System.Drawing.Size(226, 20);
            this.allFilesPath.TabIndex = 8;
            this.allFilesPath.Text = "C:\\TazWantedEarlyBuilds\\Windows";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 223);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 39);
            this.label2.TabIndex = 9;
            this.label2.Text = "After clicking \"Convert all bmps/gifs\", all .bmp/.gif files will be decrypted in " +
    "this folder and ALL subfolders.";
            // 
            // saveFile
            // 
            this.saveFile.FileName = "Save files here";
            this.saveFile.InitialDirectory = "%UserProfile%\\Desktop\\";
            // 
            // openFile
            // 
            this.openFile.Filter = "All files (*.*)|*.*";
            this.openFile.Multiselect = true;
            // 
            // openFile2
            // 
            this.openFile2.Filter = "All files (*.*)|*.*";
            this.openFile2.Multiselect = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 625);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "Taz: Wanted Modding Tool v1.1";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button convertBmp;
        private System.Windows.Forms.Button convertGif;
        private System.Windows.Forms.Label Note;
        private System.Windows.Forms.SaveFileDialog saveFile;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.Button convertTga;
        private System.Windows.Forms.Button convertWav;
        private System.Windows.Forms.Label SoundNote;
        private System.Windows.Forms.TextBox SoXpath;
        private System.Windows.Forms.Button ps2bmp;
        private System.Windows.Forms.Button xbpbmp;
        private System.Windows.Forms.Button gcpbmp;
        private System.Windows.Forms.Button findoffsets;
        private System.Windows.Forms.OpenFileDialog openFile2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button convertAllBmps;
        private System.Windows.Forms.Button convertAllGifs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox allFilesPath;
        private System.Windows.Forms.Label label2;
    }
}

