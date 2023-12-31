﻿namespace Sudoku
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
            SudokuBoard = new PictureBox();
            Generate = new Button();
            ((System.ComponentModel.ISupportInitialize)SudokuBoard).BeginInit();
            SuspendLayout();
            // 
            // SudokuBoard
            // 
            SudokuBoard.Location = new Point(12, 12);
            SudokuBoard.Name = "SudokuBoard";
            SudokuBoard.Size = new Size(360, 360);
            SudokuBoard.TabIndex = 0;
            SudokuBoard.TabStop = false;
            // 
            // Generate
            // 
            Generate.Location = new Point(399, 349);
            Generate.Name = "Generate";
            Generate.Size = new Size(75, 23);
            Generate.TabIndex = 1;
            Generate.Text = "Generate";
            Generate.UseVisualStyleBackColor = true;
            Generate.Click += Generate_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(626, 387);
            Controls.Add(Generate);
            Controls.Add(SudokuBoard);
            Name = "MainForm";
            Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)SudokuBoard).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox SudokuBoard;
        private Button Generate;
    }
}