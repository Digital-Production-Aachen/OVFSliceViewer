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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OVFSliceViewer));
            loadFileButton = new System.Windows.Forms.Button();
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            layerTrackBar = new System.Windows.Forms.TrackBar();
            timeTrackBar = new System.Windows.Forms.TrackBar();
            label1 = new System.Windows.Forms.Label();
            layerNumberLabel = new System.Windows.Forms.Label();
            glCanvas = new OpenTK.GLControl.GLControl();
            timeLayerLabel = new System.Windows.Forms.Label();
            checkboxPanel = new System.Windows.Forms.Panel();
            cBLaserIndexColor = new System.Windows.Forms.CheckBox();
            viewSelectionLabel = new System.Windows.Forms.Label();
            highlightCheckedListBox = new System.Windows.Forms.CheckedListBox();
            xTextBox = new System.Windows.Forms.TextBox();
            yTextBox = new System.Windows.Forms.TextBox();
            moveButton = new System.Windows.Forms.Button();
            xLabel = new System.Windows.Forms.Label();
            yLabel = new System.Windows.Forms.Label();
            partsCheckedListBox = new System.Windows.Forms.CheckedListBox();
            partSelectionLabel = new System.Windows.Forms.Label();
            partPanel = new System.Windows.Forms.Panel();
            btnCloseFile = new System.Windows.Forms.Button();
            label2 = new System.Windows.Forms.Label();
            exportButton = new System.Windows.Forms.Button();
            paintFunctrionCheckedListBox = new System.Windows.Forms.CheckedListBox();
            paintSelectionPanel = new System.Windows.Forms.Panel();
            paintSelectionLabel = new System.Windows.Forms.Label();
            btnReload = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)layerTrackBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)timeTrackBar).BeginInit();
            checkboxPanel.SuspendLayout();
            partPanel.SuspendLayout();
            paintSelectionPanel.SuspendLayout();
            SuspendLayout();
            // 
            // loadFileButton
            // 
            loadFileButton.AccessibleName = "loadFileButton";
            loadFileButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            loadFileButton.Location = new System.Drawing.Point(1044, 531);
            loadFileButton.Margin = new System.Windows.Forms.Padding(4);
            loadFileButton.Name = "loadFileButton";
            loadFileButton.Size = new System.Drawing.Size(150, 28);
            loadFileButton.TabIndex = 1;
            loadFileButton.Text = "Load File";
            loadFileButton.UseVisualStyleBackColor = true;
            loadFileButton.Click += LoadJobButtonClick;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // layerTrackBar
            // 
            layerTrackBar.AccessibleName = "layerTrackBar";
            layerTrackBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            layerTrackBar.Location = new System.Drawing.Point(1140, 52);
            layerTrackBar.Margin = new System.Windows.Forms.Padding(4);
            layerTrackBar.Maximum = 0;
            layerTrackBar.Name = "layerTrackBar";
            layerTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            layerTrackBar.Size = new System.Drawing.Size(45, 313);
            layerTrackBar.TabIndex = 2;
            layerTrackBar.Scroll += layerTrackBarScroll;
            layerTrackBar.MouseUp += layerTrackBarMouseUp;
            // 
            // timeTrackBar
            // 
            timeTrackBar.AccessibleName = "timeTrackBar";
            timeTrackBar.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            timeTrackBar.Location = new System.Drawing.Point(1081, 52);
            timeTrackBar.Margin = new System.Windows.Forms.Padding(4);
            timeTrackBar.Maximum = 1;
            timeTrackBar.Minimum = 1;
            timeTrackBar.Name = "timeTrackBar";
            timeTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            timeTrackBar.Size = new System.Drawing.Size(45, 313);
            timeTrackBar.TabIndex = 5;
            timeTrackBar.Value = 1;
            timeTrackBar.Scroll += timeTrackBarScroll;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(14, 1027);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(0, 15);
            label1.TabIndex = 7;
            // 
            // layerNumberLabel
            // 
            layerNumberLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            layerNumberLabel.Location = new System.Drawing.Point(1076, 383);
            layerNumberLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            layerNumberLabel.Name = "layerNumberLabel";
            layerNumberLabel.Size = new System.Drawing.Size(116, 26);
            layerNumberLabel.TabIndex = 8;
            layerNumberLabel.Text = "Layer: 0 von 0";
            // 
            // glCanvas
            // 
            glCanvas.AccessibleName = "glCanvas";
            glCanvas.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            glCanvas.APIVersion = new System.Version(3, 3, 0, 0);
            glCanvas.BackColor = System.Drawing.Color.Black;
            glCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            glCanvas.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            glCanvas.IsEventDriven = true;
            glCanvas.Location = new System.Drawing.Point(0, 0);
            glCanvas.Margin = new System.Windows.Forms.Padding(0);
            glCanvas.Name = "glCanvas";
            glCanvas.Profile = OpenTK.Windowing.Common.ContextProfile.Core;
            glCanvas.SharedContext = null;
            glCanvas.Size = new System.Drawing.Size(1209, 643);
            glCanvas.TabIndex = 9;
            glCanvas.KeyPress += STLKeyActions;
            glCanvas.MouseClick += canvasMouseClick;
            glCanvas.MouseDown += canvasMouseDown;
            glCanvas.MouseMove += canvasMouseMove;
            glCanvas.MouseUp += canvasMoveMouseUp;
            glCanvas.Resize += glCanvas_Resize;
            // 
            // timeLayerLabel
            // 
            timeLayerLabel.AccessibleName = "timeLayerLabel";
            timeLayerLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            timeLayerLabel.Location = new System.Drawing.Point(1077, 21);
            timeLayerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            timeLayerLabel.Name = "timeLayerLabel";
            timeLayerLabel.Size = new System.Drawing.Size(116, 26);
            timeLayerLabel.TabIndex = 20;
            timeLayerLabel.Text = "Time         Layer";
            // 
            // checkboxPanel
            // 
            checkboxPanel.AccessibleName = "checkboxPanel";
            checkboxPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            checkboxPanel.Controls.Add(cBLaserIndexColor);
            checkboxPanel.Controls.Add(viewSelectionLabel);
            checkboxPanel.Controls.Add(highlightCheckedListBox);
            checkboxPanel.Location = new System.Drawing.Point(4, 32);
            checkboxPanel.Margin = new System.Windows.Forms.Padding(4);
            checkboxPanel.Name = "checkboxPanel";
            checkboxPanel.Size = new System.Drawing.Size(207, 130);
            checkboxPanel.TabIndex = 21;
            // 
            // cBLaserIndexColor
            // 
            cBLaserIndexColor.AutoSize = true;
            cBLaserIndexColor.Location = new System.Drawing.Point(3, 105);
            cBLaserIndexColor.Margin = new System.Windows.Forms.Padding(4);
            cBLaserIndexColor.Name = "cBLaserIndexColor";
            cBLaserIndexColor.Size = new System.Drawing.Size(135, 19);
            cBLaserIndexColor.TabIndex = 25;
            cBLaserIndexColor.Text = "Highlight Laserindex";
            cBLaserIndexColor.UseVisualStyleBackColor = true;
            cBLaserIndexColor.CheckedChanged += cBLaserIndexColor_CheckedChanged;
            // 
            // viewSelectionLabel
            // 
            viewSelectionLabel.AccessibleName = "viewSelectionLabel";
            viewSelectionLabel.AutoSize = true;
            viewSelectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            viewSelectionLabel.Location = new System.Drawing.Point(-3, 0);
            viewSelectionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            viewSelectionLabel.Name = "viewSelectionLabel";
            viewSelectionLabel.Size = new System.Drawing.Size(131, 20);
            viewSelectionLabel.TabIndex = 23;
            viewSelectionLabel.Text = "Highlight Feature";
            // 
            // highlightCheckedListBox
            // 
            highlightCheckedListBox.AccessibleName = "highlightCheckedListBox";
            highlightCheckedListBox.BackColor = System.Drawing.SystemColors.Control;
            highlightCheckedListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            highlightCheckedListBox.CheckOnClick = true;
            highlightCheckedListBox.FormattingEnabled = true;
            highlightCheckedListBox.Items.AddRange(new object[] { "Contour", "Support" });
            highlightCheckedListBox.Location = new System.Drawing.Point(0, 26);
            highlightCheckedListBox.Margin = new System.Windows.Forms.Padding(4);
            highlightCheckedListBox.Name = "highlightCheckedListBox";
            highlightCheckedListBox.Size = new System.Drawing.Size(197, 36);
            highlightCheckedListBox.TabIndex = 17;
            highlightCheckedListBox.ItemCheck += highlightCheckedListBox_ItemCheck;
            // 
            // xTextBox
            // 
            xTextBox.AccessibleName = "xTextBox";
            xTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            xTextBox.Location = new System.Drawing.Point(1123, 435);
            xTextBox.Margin = new System.Windows.Forms.Padding(4);
            xTextBox.Name = "xTextBox";
            xTextBox.Size = new System.Drawing.Size(68, 23);
            xTextBox.TabIndex = 23;
            xTextBox.TextChanged += numBoxTextChanged;
            xTextBox.KeyDown += numBoxKeyDown;
            // 
            // yTextBox
            // 
            yTextBox.AccessibleName = "yTextBox";
            yTextBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            yTextBox.Location = new System.Drawing.Point(1123, 465);
            yTextBox.Margin = new System.Windows.Forms.Padding(4);
            yTextBox.Name = "yTextBox";
            yTextBox.Size = new System.Drawing.Size(68, 23);
            yTextBox.TabIndex = 24;
            yTextBox.TextChanged += numBoxTextChanged;
            yTextBox.KeyDown += numBoxKeyDown;
            // 
            // moveButton
            // 
            moveButton.AccessibleName = "moveButton";
            moveButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            moveButton.Location = new System.Drawing.Point(1041, 449);
            moveButton.Margin = new System.Windows.Forms.Padding(4);
            moveButton.Name = "moveButton";
            moveButton.Size = new System.Drawing.Size(55, 26);
            moveButton.TabIndex = 25;
            moveButton.Text = "Go to";
            moveButton.UseVisualStyleBackColor = true;
            moveButton.Click += moveButton_Click;
            // 
            // xLabel
            // 
            xLabel.AccessibleName = "xLabel";
            xLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            xLabel.AutoSize = true;
            xLabel.Location = new System.Drawing.Point(1102, 437);
            xLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            xLabel.Name = "xLabel";
            xLabel.Size = new System.Drawing.Size(16, 15);
            xLabel.TabIndex = 26;
            xLabel.Text = "x:";
            // 
            // yLabel
            // 
            yLabel.AccessibleName = "yLabel";
            yLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            yLabel.AutoSize = true;
            yLabel.Location = new System.Drawing.Point(1103, 465);
            yLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            yLabel.Name = "yLabel";
            yLabel.Size = new System.Drawing.Size(16, 15);
            yLabel.TabIndex = 27;
            yLabel.Text = "y:";
            // 
            // partsCheckedListBox
            // 
            partsCheckedListBox.AccessibleName = "partsCheckedListBox";
            partsCheckedListBox.BackColor = System.Drawing.SystemColors.Control;
            partsCheckedListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            partsCheckedListBox.CheckOnClick = true;
            partsCheckedListBox.FormattingEnabled = true;
            partsCheckedListBox.Location = new System.Drawing.Point(4, 34);
            partsCheckedListBox.Margin = new System.Windows.Forms.Padding(4);
            partsCheckedListBox.Name = "partsCheckedListBox";
            partsCheckedListBox.Size = new System.Drawing.Size(193, 162);
            partsCheckedListBox.TabIndex = 17;
            partsCheckedListBox.ItemCheck += partsCheckedListBox_ItemCheck;
            // 
            // partSelectionLabel
            // 
            partSelectionLabel.AccessibleName = "partSelectionLabel";
            partSelectionLabel.AutoSize = true;
            partSelectionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            partSelectionLabel.Location = new System.Drawing.Point(1, 8);
            partSelectionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            partSelectionLabel.Name = "partSelectionLabel";
            partSelectionLabel.Size = new System.Drawing.Size(108, 20);
            partSelectionLabel.TabIndex = 24;
            partSelectionLabel.Text = "Part Selection";
            // 
            // partPanel
            // 
            partPanel.AccessibleName = "partPanel";
            partPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            partPanel.Controls.Add(partSelectionLabel);
            partPanel.Controls.Add(partsCheckedListBox);
            partPanel.Location = new System.Drawing.Point(4, 170);
            partPanel.Margin = new System.Windows.Forms.Padding(4);
            partPanel.Name = "partPanel";
            partPanel.Size = new System.Drawing.Size(207, 238);
            partPanel.TabIndex = 22;
            // 
            // btnCloseFile
            // 
            btnCloseFile.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnCloseFile.Location = new System.Drawing.Point(1043, 603);
            btnCloseFile.Margin = new System.Windows.Forms.Padding(4);
            btnCloseFile.Name = "btnCloseFile";
            btnCloseFile.Size = new System.Drawing.Size(150, 26);
            btnCloseFile.TabIndex = 28;
            btnCloseFile.Text = "Close File";
            btnCloseFile.UseVisualStyleBackColor = true;
            btnCloseFile.Click += btnCloseFile_Click;
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Microsoft Tai Le", 12F);
            label2.Location = new System.Drawing.Point(4, 617);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.MinimumSize = new System.Drawing.Size(291, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(291, 21);
            label2.TabIndex = 29;
            // 
            // exportButton
            // 
            exportButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            exportButton.Enabled = false;
            exportButton.Location = new System.Drawing.Point(882, 603);
            exportButton.Margin = new System.Windows.Forms.Padding(4);
            exportButton.Name = "exportButton";
            exportButton.Size = new System.Drawing.Size(154, 26);
            exportButton.TabIndex = 30;
            exportButton.Text = "Export STL-File as Lgdff";
            exportButton.UseVisualStyleBackColor = true;
            exportButton.Click += exportButton_Click;
            // 
            // paintFunctrionCheckedListBox
            // 
            paintFunctrionCheckedListBox.AccessibleName = "paintFunctrionCheckedListBox";
            paintFunctrionCheckedListBox.CheckOnClick = true;
            paintFunctrionCheckedListBox.FormattingEnabled = true;
            paintFunctrionCheckedListBox.Items.AddRange(new object[] { "no support", "accessibility", "functional areas", "erase" });
            paintFunctrionCheckedListBox.Location = new System.Drawing.Point(0, 28);
            paintFunctrionCheckedListBox.Margin = new System.Windows.Forms.Padding(4);
            paintFunctrionCheckedListBox.Name = "paintFunctrionCheckedListBox";
            paintFunctrionCheckedListBox.Size = new System.Drawing.Size(140, 58);
            paintFunctrionCheckedListBox.TabIndex = 31;
            paintFunctrionCheckedListBox.ItemCheck += paintFunctrionCheckedListBox_ItemCheck;
            // 
            // paintSelectionPanel
            // 
            paintSelectionPanel.AccessibleName = "paintSelectionPanel";
            paintSelectionPanel.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            paintSelectionPanel.Controls.Add(paintSelectionLabel);
            paintSelectionPanel.Controls.Add(paintFunctrionCheckedListBox);
            paintSelectionPanel.Location = new System.Drawing.Point(882, 492);
            paintSelectionPanel.Margin = new System.Windows.Forms.Padding(4);
            paintSelectionPanel.Name = "paintSelectionPanel";
            paintSelectionPanel.Size = new System.Drawing.Size(154, 104);
            paintSelectionPanel.TabIndex = 32;
            // 
            // paintSelectionLabel
            // 
            paintSelectionLabel.AccessibleName = "paintSelectionLabel";
            paintSelectionLabel.AutoSize = true;
            paintSelectionLabel.Location = new System.Drawing.Point(20, 10);
            paintSelectionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            paintSelectionLabel.Name = "paintSelectionLabel";
            paintSelectionLabel.Size = new System.Drawing.Size(84, 15);
            paintSelectionLabel.TabIndex = 32;
            paintSelectionLabel.Text = "Region Marker";
            // 
            // btnReload
            // 
            btnReload.AccessibleName = "loadFileButton";
            btnReload.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            btnReload.Location = new System.Drawing.Point(1043, 567);
            btnReload.Margin = new System.Windows.Forms.Padding(4);
            btnReload.Name = "btnReload";
            btnReload.Size = new System.Drawing.Size(150, 28);
            btnReload.TabIndex = 33;
            btnReload.Text = "Reload";
            btnReload.UseVisualStyleBackColor = true;
            btnReload.Click += btnReload_Click;
            // 
            // OVFSliceViewer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ClientSize = new System.Drawing.Size(1209, 643);
            Controls.Add(btnReload);
            Controls.Add(paintSelectionPanel);
            Controls.Add(exportButton);
            Controls.Add(label2);
            Controls.Add(btnCloseFile);
            Controls.Add(layerNumberLabel);
            Controls.Add(timeTrackBar);
            Controls.Add(yLabel);
            Controls.Add(xLabel);
            Controls.Add(moveButton);
            Controls.Add(yTextBox);
            Controls.Add(xTextBox);
            Controls.Add(partPanel);
            Controls.Add(checkboxPanel);
            Controls.Add(timeLayerLabel);
            Controls.Add(label1);
            Controls.Add(layerTrackBar);
            Controls.Add(loadFileButton);
            Controls.Add(glCanvas);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4);
            Name = "OVFSliceViewer";
            Text = "OVF Slice Viewer";
            KeyDown += OVFSliceViewer_KeyDown;
            KeyUp += OVFSliceViewer_KeyUp;
            PreviewKeyDown += OVFSliceViewer_PreviewKeyDown;
            ((System.ComponentModel.ISupportInitialize)layerTrackBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)timeTrackBar).EndInit();
            checkboxPanel.ResumeLayout(false);
            checkboxPanel.PerformLayout();
            partPanel.ResumeLayout(false);
            partPanel.PerformLayout();
            paintSelectionPanel.ResumeLayout(false);
            paintSelectionPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button loadFileButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TrackBar layerTrackBar;
        private System.Windows.Forms.TrackBar timeTrackBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label layerNumberLabel;
        private OpenTK.GLControl.GLControl glCanvas;
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

