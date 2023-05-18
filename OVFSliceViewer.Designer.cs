namespace OVFSliceViewer
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
            this.glCanvas = new OpenTK.WinForms.GLControl();
            this.timeLayerLabel = new System.Windows.Forms.Label();
            this.checkboxPanel = new System.Windows.Forms.Panel();
            this.cBLaserIndexColor = new System.Windows.Forms.CheckBox();
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
            this.btnCloseFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.exportButton = new System.Windows.Forms.Button();
            this.paintFunctrionCheckedListBox = new System.Windows.Forms.CheckedListBox();
            this.paintSelectionPanel = new System.Windows.Forms.Panel();
            this.paintSelectionLabel = new System.Windows.Forms.Label();
            this.btnReload = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.layerTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTrackBar)).BeginInit();
            this.checkboxPanel.SuspendLayout();
            this.partPanel.SuspendLayout();
            this.paintSelectionPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // loadFileButton
            // 
            this.loadFileButton.AccessibleName = "loadFileButton";
            this.loadFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.loadFileButton.Location = new System.Drawing.Point(1044, 678);
            this.loadFileButton.Margin = new System.Windows.Forms.Padding(4);
            this.loadFileButton.Name = "loadFileButton";
            this.loadFileButton.Size = new System.Drawing.Size(150, 28);
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
            this.layerTrackBar.Location = new System.Drawing.Point(1140, 52);
            this.layerTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.layerTrackBar.Maximum = 0;
            this.layerTrackBar.Name = "layerTrackBar";
            this.layerTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.layerTrackBar.Size = new System.Drawing.Size(45, 460);
            this.layerTrackBar.TabIndex = 2;
            this.layerTrackBar.Scroll += new System.EventHandler(this.layerTrackBarScroll);
            this.layerTrackBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.layerTrackBarMouseUp);
            // 
            // timeTrackBar
            // 
            this.timeTrackBar.AccessibleName = "timeTrackBar";
            this.timeTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeTrackBar.Location = new System.Drawing.Point(1081, 52);
            this.timeTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.timeTrackBar.Maximum = 1;
            this.timeTrackBar.Minimum = 1;
            this.timeTrackBar.Name = "timeTrackBar";
            this.timeTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.timeTrackBar.Size = new System.Drawing.Size(45, 460);
            this.timeTrackBar.TabIndex = 5;
            this.timeTrackBar.Value = 1;
            this.timeTrackBar.Scroll += new System.EventHandler(this.timeTrackBarScroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 1027);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 15);
            this.label1.TabIndex = 7;
            // 
            // layerNumberLabel
            // 
            this.layerNumberLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.layerNumberLabel.Location = new System.Drawing.Point(1076, 530);
            this.layerNumberLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.layerNumberLabel.Name = "layerNumberLabel";
            this.layerNumberLabel.Size = new System.Drawing.Size(116, 26);
            this.layerNumberLabel.TabIndex = 8;
            this.layerNumberLabel.Text = "Layer: 0 von 0";
            // 
            // glCanvas
            // 
            this.glCanvas.AccessibleName = "glCanvas";
            this.glCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glCanvas.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.glCanvas.APIVersion = new System.Version(3, 3, 0, 0);
            this.glCanvas.BackColor = System.Drawing.Color.Black;
            this.glCanvas.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.glCanvas.IsEventDriven = true;
            this.glCanvas.Location = new System.Drawing.Point(0, 0);
            this.glCanvas.Margin = new System.Windows.Forms.Padding(0);
            this.glCanvas.Name = "glCanvas";
            this.glCanvas.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            this.glCanvas.Size = new System.Drawing.Size(1212, 787);
            this.glCanvas.TabIndex = 9;
            this.glCanvas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.STLKeyActions);
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
            this.timeLayerLabel.Location = new System.Drawing.Point(1077, 21);
            this.timeLayerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.timeLayerLabel.Name = "timeLayerLabel";
            this.timeLayerLabel.Size = new System.Drawing.Size(116, 26);
            this.timeLayerLabel.TabIndex = 20;
            this.timeLayerLabel.Text = "Time         Layer";
            // 
            // checkboxPanel
            // 
            this.checkboxPanel.AccessibleName = "checkboxPanel";
            this.checkboxPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.checkboxPanel.Controls.Add(this.cBLaserIndexColor);
            this.checkboxPanel.Controls.Add(this.viewSelectionLabel);
            this.checkboxPanel.Controls.Add(this.highlightCheckedListBox);
            this.checkboxPanel.Location = new System.Drawing.Point(4, 32);
            this.checkboxPanel.Margin = new System.Windows.Forms.Padding(4);
            this.checkboxPanel.Name = "checkboxPanel";
            this.checkboxPanel.Size = new System.Drawing.Size(207, 130);
            this.checkboxPanel.TabIndex = 21;
            // 
            // cBLaserIndexColor
            // 
            this.cBLaserIndexColor.AutoSize = true;
            this.cBLaserIndexColor.Location = new System.Drawing.Point(3, 105);
            this.cBLaserIndexColor.Margin = new System.Windows.Forms.Padding(4);
            this.cBLaserIndexColor.Name = "cBLaserIndexColor";
            this.cBLaserIndexColor.Size = new System.Drawing.Size(135, 19);
            this.cBLaserIndexColor.TabIndex = 25;
            this.cBLaserIndexColor.Text = "Highlight Laserindex";
            this.cBLaserIndexColor.UseVisualStyleBackColor = true;
            this.cBLaserIndexColor.CheckedChanged += new System.EventHandler(this.cBLaserIndexColor_CheckedChanged);
            // 
            // viewSelectionLabel
            // 
            this.viewSelectionLabel.AccessibleName = "viewSelectionLabel";
            this.viewSelectionLabel.AutoSize = true;
            this.viewSelectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.viewSelectionLabel.Location = new System.Drawing.Point(-3, 0);
            this.viewSelectionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
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
            this.highlightCheckedListBox.Location = new System.Drawing.Point(0, 26);
            this.highlightCheckedListBox.Margin = new System.Windows.Forms.Padding(4);
            this.highlightCheckedListBox.Name = "highlightCheckedListBox";
            this.highlightCheckedListBox.Size = new System.Drawing.Size(197, 36);
            this.highlightCheckedListBox.TabIndex = 17;
            this.highlightCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.highlightCheckedListBox_ItemCheck);
            // 
            // xTextBox
            // 
            this.xTextBox.AccessibleName = "xTextBox";
            this.xTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.xTextBox.Location = new System.Drawing.Point(1123, 582);
            this.xTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.xTextBox.Name = "xTextBox";
            this.xTextBox.Size = new System.Drawing.Size(68, 23);
            this.xTextBox.TabIndex = 23;
            this.xTextBox.TextChanged += new System.EventHandler(this.numBoxTextChanged);
            this.xTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numBoxKeyDown);
            // 
            // yTextBox
            // 
            this.yTextBox.AccessibleName = "yTextBox";
            this.yTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.yTextBox.Location = new System.Drawing.Point(1123, 612);
            this.yTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.yTextBox.Name = "yTextBox";
            this.yTextBox.Size = new System.Drawing.Size(68, 23);
            this.yTextBox.TabIndex = 24;
            this.yTextBox.TextChanged += new System.EventHandler(this.numBoxTextChanged);
            this.yTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.numBoxKeyDown);
            // 
            // moveButton
            // 
            this.moveButton.AccessibleName = "moveButton";
            this.moveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.moveButton.Location = new System.Drawing.Point(1041, 596);
            this.moveButton.Margin = new System.Windows.Forms.Padding(4);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(55, 26);
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
            this.xLabel.Location = new System.Drawing.Point(1102, 584);
            this.xLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.xLabel.Name = "xLabel";
            this.xLabel.Size = new System.Drawing.Size(16, 15);
            this.xLabel.TabIndex = 26;
            this.xLabel.Text = "x:";
            // 
            // yLabel
            // 
            this.yLabel.AccessibleName = "yLabel";
            this.yLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.yLabel.AutoSize = true;
            this.yLabel.Location = new System.Drawing.Point(1103, 612);
            this.yLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.yLabel.Name = "yLabel";
            this.yLabel.Size = new System.Drawing.Size(16, 15);
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
            this.partsCheckedListBox.Location = new System.Drawing.Point(4, 34);
            this.partsCheckedListBox.Margin = new System.Windows.Forms.Padding(4);
            this.partsCheckedListBox.Name = "partsCheckedListBox";
            this.partsCheckedListBox.Size = new System.Drawing.Size(193, 162);
            this.partsCheckedListBox.TabIndex = 17;
            this.partsCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.partsCheckedListBox_ItemCheck);
            // 
            // partSelectionLabel
            // 
            this.partSelectionLabel.AccessibleName = "partSelectionLabel";
            this.partSelectionLabel.AutoSize = true;
            this.partSelectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.partSelectionLabel.Location = new System.Drawing.Point(1, 8);
            this.partSelectionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
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
            this.partPanel.Location = new System.Drawing.Point(4, 170);
            this.partPanel.Margin = new System.Windows.Forms.Padding(4);
            this.partPanel.Name = "partPanel";
            this.partPanel.Size = new System.Drawing.Size(207, 238);
            this.partPanel.TabIndex = 22;
            // 
            // btnCloseFile
            // 
            this.btnCloseFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseFile.Location = new System.Drawing.Point(1043, 750);
            this.btnCloseFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnCloseFile.Name = "btnCloseFile";
            this.btnCloseFile.Size = new System.Drawing.Size(150, 26);
            this.btnCloseFile.TabIndex = 28;
            this.btnCloseFile.Text = "Close File";
            this.btnCloseFile.UseVisualStyleBackColor = true;
            this.btnCloseFile.Click += new System.EventHandler(this.btnCloseFile_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(4, 764);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.MinimumSize = new System.Drawing.Size(291, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(291, 21);
            this.label2.TabIndex = 29;
            // 
            // exportButton
            // 
            this.exportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.exportButton.Enabled = false;
            this.exportButton.Location = new System.Drawing.Point(882, 750);
            this.exportButton.Margin = new System.Windows.Forms.Padding(4);
            this.exportButton.Name = "exportButton";
            this.exportButton.Size = new System.Drawing.Size(154, 26);
            this.exportButton.TabIndex = 30;
            this.exportButton.Text = "Export STL-File as Lgdff";
            this.exportButton.UseVisualStyleBackColor = true;
            this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
            // 
            // paintFunctrionCheckedListBox
            // 
            this.paintFunctrionCheckedListBox.AccessibleName = "paintFunctrionCheckedListBox";
            this.paintFunctrionCheckedListBox.CheckOnClick = true;
            this.paintFunctrionCheckedListBox.FormattingEnabled = true;
            this.paintFunctrionCheckedListBox.Items.AddRange(new object[] {
            "no support",
            "accessibility",
            "functional areas",
            "erase"});
            this.paintFunctrionCheckedListBox.Location = new System.Drawing.Point(0, 28);
            this.paintFunctrionCheckedListBox.Margin = new System.Windows.Forms.Padding(4);
            this.paintFunctrionCheckedListBox.Name = "paintFunctrionCheckedListBox";
            this.paintFunctrionCheckedListBox.Size = new System.Drawing.Size(140, 58);
            this.paintFunctrionCheckedListBox.TabIndex = 31;
            this.paintFunctrionCheckedListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.paintFunctrionCheckedListBox_ItemCheck);
            // 
            // paintSelectionPanel
            // 
            this.paintSelectionPanel.AccessibleName = "paintSelectionPanel";
            this.paintSelectionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.paintSelectionPanel.Controls.Add(this.paintSelectionLabel);
            this.paintSelectionPanel.Controls.Add(this.paintFunctrionCheckedListBox);
            this.paintSelectionPanel.Location = new System.Drawing.Point(882, 639);
            this.paintSelectionPanel.Margin = new System.Windows.Forms.Padding(4);
            this.paintSelectionPanel.Name = "paintSelectionPanel";
            this.paintSelectionPanel.Size = new System.Drawing.Size(154, 104);
            this.paintSelectionPanel.TabIndex = 32;
            // 
            // paintSelectionLabel
            // 
            this.paintSelectionLabel.AccessibleName = "paintSelectionLabel";
            this.paintSelectionLabel.AutoSize = true;
            this.paintSelectionLabel.Location = new System.Drawing.Point(20, 10);
            this.paintSelectionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.paintSelectionLabel.Name = "paintSelectionLabel";
            this.paintSelectionLabel.Size = new System.Drawing.Size(84, 15);
            this.paintSelectionLabel.TabIndex = 32;
            this.paintSelectionLabel.Text = "Region Marker";
            // 
            // btnReload
            // 
            this.btnReload.AccessibleName = "loadFileButton";
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReload.Location = new System.Drawing.Point(1043, 714);
            this.btnReload.Margin = new System.Windows.Forms.Padding(4);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(150, 28);
            this.btnReload.TabIndex = 33;
            this.btnReload.Text = "Reload";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // OVFSliceViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1209, 790);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.paintSelectionPanel);
            this.Controls.Add(this.exportButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCloseFile);
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
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "OVFSliceViewer";
            this.Text = "OVF Slice Viewer";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OVFSliceViewer_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.OVFSliceViewer_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.OVFSliceViewer_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.layerTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeTrackBar)).EndInit();
            this.checkboxPanel.ResumeLayout(false);
            this.checkboxPanel.PerformLayout();
            this.partPanel.ResumeLayout(false);
            this.partPanel.PerformLayout();
            this.paintSelectionPanel.ResumeLayout(false);
            this.paintSelectionPanel.PerformLayout();
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
        private OpenTK.WinForms.GLControl glCanvas;
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
        private System.Windows.Forms.CheckBox cBLaserIndexColor;
        private System.Windows.Forms.Button btnCloseFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.CheckedListBox paintFunctrionCheckedListBox;
        private System.Windows.Forms.Panel paintSelectionPanel;
        private System.Windows.Forms.Label paintSelectionLabel;
        private System.Windows.Forms.Button btnReload;
    }
}

