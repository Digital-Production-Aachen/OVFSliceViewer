﻿namespace OVFSliceViewer
{
    partial class OVFSliceViewer
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
            this.loadFileButton = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.layerTrackBar = new System.Windows.Forms.TrackBar();
            this.timeTrackBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.layerNumberLabel = new System.Windows.Forms.Label();
            this.glCanvas = new OpenTK.GLControl();
            this.timeLayerLabel = new System.Windows.Forms.Label();
            this.checkboxPanel = new System.Windows.Forms.Panel();
            this.CbShow3dModel = new System.Windows.Forms.CheckBox();
            this.viewSelectionLabel = new System.Windows.Forms.Label();
            this.highlightCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.xTextBox = new System.Windows.Forms.TextBox();
            this.yTextBox = new System.Windows.Forms.TextBox();
            this.moveButton = new System.Windows.Forms.Button();
            this.xLabel = new System.Windows.Forms.Label();
            this.yLabel = new System.Windows.Forms.Label();
            this.partsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.partSelectionLabel = new System.Windows.Forms.Label();
            this.partPanel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.layerTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTrackBar)).BeginInit();
            this.checkboxPanel.SuspendLayout();
            this.partPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadFileButton
            // 
            this.loadFileButton.AccessibleName = "loadFileButton";
            this.loadFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loadFileButton.Location = new System.Drawing.Point(1530, 789);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(129, 50);
            this.loadFileButton.TabIndex = 1;
            this.loadFileButton.Text = "Load File";
            this.loadFileButton.UseVisualStyleBackColor = true;
            this.loadFileButton.Click += new System.EventHandler(this.LoadJobButtonClick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // layerTrackBar
            // 
            this.layerTrackBar.AccessibleName = "layerTrackBar";
            this.layerTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layerTrackBar.Location = new System.Drawing.Point(1612, 45);
            this.layerTrackBar.Maximum = 0;
            this.layerTrackBar.Name = "layerTrackBar";
            this.layerTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.layerTrackBar.Size = new System.Drawing.Size(45, 638);
            this.layerTrackBar.TabIndex = 2;
            this.layerTrackBar.Scroll += new System.EventHandler(this.layerTrackBarScroll);
            this.layerTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.layerTrackBarMouseUp);
            // 
            // timeTrackBar
            // 
            this.timeTrackBar.AccessibleName = "timeTrackBar";
            this.timeTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeTrackBar.Location = new System.Drawing.Point(1561, 45);
            this.timeTrackBar.Maximum = 1;
            this.timeTrackBar.Minimum = 1;
            this.timeTrackBar.Name = "timeTrackBar";
            this.timeTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.timeTrackBar.Size = new System.Drawing.Size(45, 638);
            this.timeTrackBar.TabIndex = 5;
            this.timeTrackBar.Value = 1;
            this.timeTrackBar.Scroll += new System.EventHandler(this.timeTrackBarScroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 890);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 7;
            // 
            // layerNumberLabel
            // 
            this.layerNumberLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.layerNumberLabel.Location = new System.Drawing.Point(1558, 686);
            this.layerNumberLabel.Name = "layerNumberLabel";
            this.layerNumberLabel.Size = new System.Drawing.Size(99, 22);
            this.layerNumberLabel.TabIndex = 8;
            this.layerNumberLabel.Text = "Layer: 0 von 0";
            // 
            // glCanvas
            // 
            this.glCanvas.AccessibleName = "glCanvas";
            this.glCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glCanvas.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.glCanvas.BackColor = System.Drawing.Color.Black;
            this.glCanvas.Location = new System.Drawing.Point(0, 0);
            this.glCanvas.Margin = new System.Windows.Forms.Padding(0);
            this.glCanvas.Name = "glCanvas";
            this.glCanvas.Size = new System.Drawing.Size(1673, 845);
            this.glCanvas.TabIndex = 9;
            this.glCanvas.VSync = false;
            this.glCanvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.canvasMouseClick);
            this.glCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvasMouseDown);
            this.glCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvasMouseMove);
            this.glCanvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.canvasMoveMouseUp);
            this.glCanvas.Resize += new System.EventHandler(this.glCanvas_Resize);
            // 
            // timeLayerLabel
            // 
            this.timeLayerLabel.AccessibleName = "timeLayerLabel";
            this.timeLayerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timeLayerLabel.Location = new System.Drawing.Point(1558, 18);
            this.timeLayerLabel.Name = "timeLayerLabel";
            this.timeLayerLabel.Size = new System.Drawing.Size(99, 22);
            this.timeLayerLabel.TabIndex = 20;
            this.timeLayerLabel.Text = "Time         Layer";
            // 
            // checkboxPanel
            // 
            this.checkboxPanel.AccessibleName = "checkboxPanel";
            this.checkboxPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.checkboxPanel.Controls.Add(this.CbShow3dModel);
            this.checkboxPanel.Controls.Add(this.viewSelectionLabel);
            this.checkboxPanel.Controls.Add(this.highlightCheckedListBox);
            this.checkboxPanel.Location = new System.Drawing.Point(4, 28);
            this.checkboxPanel.Name = "checkboxPanel";
            this.checkboxPanel.Size = new System.Drawing.Size(178, 113);
            this.checkboxPanel.TabIndex = 21;
            // 
            // CbShow3dModel
            // 
            this.CbShow3dModel.AutoSize = true;
            this.CbShow3dModel.Location = new System.Drawing.Point(2, 93);
            this.CbShow3dModel.Name = "CbShow3dModel";
            this.CbShow3dModel.Size = new System.Drawing.Size(100, 17);
            this.CbShow3dModel.TabIndex = 24;
            this.CbShow3dModel.Text = "Show 3d Model";
            this.CbShow3dModel.UseVisualStyleBackColor = true;
            this.CbShow3dModel.CheckedChanged += new System.EventHandler(this.CbShow3dModel_CheckedChanged);
            // 
            // viewSelectionLabel
            // 
            this.viewSelectionLabel.AccessibleName = "viewSelectionLabel";
            this.viewSelectionLabel.AutoSize = true;
            this.viewSelectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewSelectionLabel.Location = new System.Drawing.Point(-2, 0);
            this.viewSelectionLabel.Name = "viewSelectionLabel";
            this.viewSelectionLabel.Size = new System.Drawing.Size(131, 20);
            this.viewSelectionLabel.TabIndex = 23;
            this.viewSelectionLabel.Text = "Highlight Feature";
            // 
            // highlightCheckedListBox
            // 
            this.highlightCheckedListBox.AccessibleName = "highlightCheckedListBox";
            this.highlightCheckedListBox.BackColor = System.Drawing.SystemColors.Control;
            this.highlightCheckedListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.highlightCheckedListBox.CheckOnClick = true;
            this.highlightCheckedListBox.FormattingEnabled = true;
            this.highlightCheckedListBox.Items.AddRange(new object[] {
            "Contour",
            "Support"});
            this.highlightCheckedListBox.Location = new System.Drawing.Point(0, 23);
            this.highlightCheckedListBox.Name = "highlightCheckedListBox";
            this.highlightCheckedListBox.Size = new System.Drawing.Size(169, 45);
            this.highlightCheckedListBox.TabIndex = 17;
            this.highlightCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.highlightCheckedListBox_ItemCheck);
            // 
            // xTextBox
            // 
            this.xTextBox.AccessibleName = "xTextBox";
            this.xTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.xTextBox.Location = new System.Drawing.Point(1600, 732);
            this.xTextBox.Name = "xTextBox";
            this.xTextBox.Size = new System.Drawing.Size(59, 20);
            this.xTextBox.TabIndex = 23;
            this.xTextBox.TextChanged += new System.EventHandler(this.numBoxTextChanged);
            this.xTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numBoxKeyDown);
            // 
            // yTextBox
            // 
            this.yTextBox.AccessibleName = "yTextBox";
            this.yTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.yTextBox.Location = new System.Drawing.Point(1600, 758);
            this.yTextBox.Name = "yTextBox";
            this.yTextBox.Size = new System.Drawing.Size(59, 20);
            this.yTextBox.TabIndex = 24;
            this.yTextBox.TextChanged += new System.EventHandler(this.numBoxTextChanged);
            this.yTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numBoxKeyDown);
            // 
            // moveButton
            // 
            this.moveButton.AccessibleName = "moveButton";
            this.moveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.moveButton.Location = new System.Drawing.Point(1530, 744);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(47, 22);
            this.moveButton.TabIndex = 25;
            this.moveButton.Text = "Go to";
            this.moveButton.UseVisualStyleBackColor = true;
            this.moveButton.Click += new System.EventHandler(this.moveButton_Click);
            // 
            // xLabel
            // 
            this.xLabel.AccessibleName = "xLabel";
            this.xLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.xLabel.AutoSize = true;
            this.xLabel.Location = new System.Drawing.Point(1582, 733);
            this.xLabel.Name = "xLabel";
            this.xLabel.Size = new System.Drawing.Size(15, 13);
            this.xLabel.TabIndex = 26;
            this.xLabel.Text = "x:";
            // 
            // yLabel
            // 
            this.yLabel.AccessibleName = "yLabel";
            this.yLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.yLabel.AutoSize = true;
            this.yLabel.Location = new System.Drawing.Point(1583, 758);
            this.yLabel.Name = "yLabel";
            this.yLabel.Size = new System.Drawing.Size(15, 13);
            this.yLabel.TabIndex = 27;
            this.yLabel.Text = "y:";
            // 
            // partsCheckedListBox
            // 
            this.partsCheckedListBox.AccessibleName = "partsCheckedListBox";
            this.partsCheckedListBox.BackColor = System.Drawing.SystemColors.Control;
            this.partsCheckedListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.partsCheckedListBox.CheckOnClick = true;
            this.partsCheckedListBox.FormattingEnabled = true;
            this.partsCheckedListBox.Location = new System.Drawing.Point(3, 30);
            this.partsCheckedListBox.Name = "partsCheckedListBox";
            this.partsCheckedListBox.Size = new System.Drawing.Size(166, 150);
            this.partsCheckedListBox.TabIndex = 17;
            this.partsCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.partsCheckedListBox_ItemCheck);
            // 
            // partSelectionLabel
            // 
            this.partSelectionLabel.AccessibleName = "partSelectionLabel";
            this.partSelectionLabel.AutoSize = true;
            this.partSelectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.partSelectionLabel.Location = new System.Drawing.Point(1, 7);
            this.partSelectionLabel.Name = "partSelectionLabel";
            this.partSelectionLabel.Size = new System.Drawing.Size(108, 20);
            this.partSelectionLabel.TabIndex = 24;
            this.partSelectionLabel.Text = "Part Selection";
            // 
            // partPanel
            // 
            this.partPanel.AccessibleName = "partPanel";
            this.partPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.partPanel.Controls.Add(this.partSelectionLabel);
            this.partPanel.Controls.Add(this.partsCheckedListBox);
            this.partPanel.Location = new System.Drawing.Point(4, 147);
            this.partPanel.Name = "partPanel";
            this.partPanel.Size = new System.Drawing.Size(178, 207);
            this.partPanel.TabIndex = 22;
            // 
            // OVFSliceViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1671, 847);
            this.Controls.Add(this.layerNumberLabel);
            this.Controls.Add(this.timeTrackBar);
            this.Controls.Add(this.yLabel);
            this.Controls.Add(this.xLabel);
            this.Controls.Add(this.moveButton);
            this.Controls.Add(this.yTextBox);
            this.Controls.Add(this.xTextBox);
            this.Controls.Add(this.partPanel);
            this.Controls.Add(this.checkboxPanel);
            this.Controls.Add(this.timeLayerLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.layerTrackBar);
            this.Controls.Add(this.loadFileButton);
            this.Controls.Add(this.glCanvas);
            this.Name = "OVFSliceViewer";
            this.Text = "OVF Slice Viewer";
            ((System.ComponentModel.ISupportInitialize)(this.layerTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTrackBar)).EndInit();
            this.checkboxPanel.ResumeLayout(false);
            this.checkboxPanel.PerformLayout();
            this.partPanel.ResumeLayout(false);
            this.partPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button loadFileButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TrackBar layerTrackBar;
        private System.Windows.Forms.TrackBar timeTrackBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label layerNumberLabel;
        private OpenTK.GLControl glCanvas;
        private System.Windows.Forms.Label timeLayerLabel;
        private System.Windows.Forms.Panel checkboxPanel;
        private System.Windows.Forms.CheckedListBox highlightCheckedListBox;
        private System.Windows.Forms.Label viewSelectionLabel;
        private System.Windows.Forms.TextBox xTextBox;
        private System.Windows.Forms.TextBox yTextBox;
        private System.Windows.Forms.Button moveButton;
        private System.Windows.Forms.Label xLabel;
        private System.Windows.Forms.Label yLabel;
        private System.Windows.Forms.CheckedListBox partsCheckedListBox;
        private System.Windows.Forms.Label partSelectionLabel;
        private System.Windows.Forms.Panel partPanel;
        private System.Windows.Forms.CheckBox CbShow3dModel;
    }
}

