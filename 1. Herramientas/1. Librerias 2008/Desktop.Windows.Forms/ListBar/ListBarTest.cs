using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Desktop.Windows.Forms
{
	/// <summary>
	/// Summary description for frmTestListBar.
	/// </summary>
	public class frmListBarTest : System.Windows.Forms.Form
	{
		private ListBarItem contextItem = null;
		private ListBarGroup contextGroup = null;
		private System.Windows.Forms.ImageList ilsIconsLarge;
		private System.Windows.Forms.ImageList ilsIconsSmall;
		private System.Windows.Forms.TreeView tvwCustom;
        private Desktop.Windows.Forms.ListBar listBar1;
		private System.Windows.Forms.ToolTip toolTips;
		private System.Windows.Forms.PropertyGrid groupPropertyGrid;
		private System.Windows.Forms.Panel pnlProperties;
		private System.Windows.Forms.ContextMenu itemContextMenu;
		private System.Windows.Forms.MenuItem mnuContextOpen;
		private System.Windows.Forms.MenuItem mnuContextOpenNew;
		private System.Windows.Forms.MenuItem mnuContextSendTo;
		private System.Windows.Forms.MenuItem mnuContextAdvancedFind;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem mnuContextRemove;
		private System.Windows.Forms.MenuItem mnuContextRename;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem mnuContextProperties;
		private System.Windows.Forms.ContextMenu groupContextMenu;
		private System.Windows.Forms.MenuItem mnuLargeIcons;
		private System.Windows.Forms.MenuItem mnuSmallIcons;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem mnuAddNewGroup;
		private System.Windows.Forms.MenuItem mnuRemoveGroup;
		private System.Windows.Forms.MenuItem mnuRenameGroup;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem mnuListBarShortcut;
		private System.Windows.Forms.MenuItem mnuListBarWebShortcut;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem mnuHideOutlookBar;
		private System.Windows.Forms.Splitter splitListBar;
		private System.Windows.Forms.Panel pnlMain;
		private System.Windows.Forms.Splitter splitProperties;
		private System.Windows.Forms.CheckBox chkCustomColors;
		private System.Windows.Forms.CheckBox chkBackground;
		private System.Windows.Forms.PictureBox picBackground;
		private System.Windows.Forms.CheckBox chkStyle;
		private System.Windows.Forms.ComboBox cboGroups;
		private System.Windows.Forms.CheckBox chkEnabled;
		private SectionHeader sectionHeader1;
		private System.ComponentModel.IContainer components;

		/// <summary>
		/// A form for testing the features of the vbAccelerator .NET ListBar control.
		/// </summary>
		public frmListBarTest()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmListBarTest));
            this.listBar1 = new Desktop.Windows.Forms.ListBar();
            this.tvwCustom = new System.Windows.Forms.TreeView();
            this.ilsIconsLarge = new System.Windows.Forms.ImageList(this.components);
            this.ilsIconsSmall = new System.Windows.Forms.ImageList(this.components);
            this.toolTips = new System.Windows.Forms.ToolTip(this.components);
            this.pnlProperties = new System.Windows.Forms.Panel();
            this.sectionHeader1 = new Desktop.Windows.Forms.SectionHeader();
            this.cboGroups = new System.Windows.Forms.ComboBox();
            this.groupPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.itemContextMenu = new System.Windows.Forms.ContextMenu();
            this.mnuContextOpen = new System.Windows.Forms.MenuItem();
            this.mnuContextOpenNew = new System.Windows.Forms.MenuItem();
            this.mnuContextSendTo = new System.Windows.Forms.MenuItem();
            this.mnuContextAdvancedFind = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.mnuContextRemove = new System.Windows.Forms.MenuItem();
            this.mnuContextRename = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.mnuContextProperties = new System.Windows.Forms.MenuItem();
            this.groupContextMenu = new System.Windows.Forms.ContextMenu();
            this.mnuLargeIcons = new System.Windows.Forms.MenuItem();
            this.mnuSmallIcons = new System.Windows.Forms.MenuItem();
            this.menuItem4 = new System.Windows.Forms.MenuItem();
            this.mnuAddNewGroup = new System.Windows.Forms.MenuItem();
            this.mnuRemoveGroup = new System.Windows.Forms.MenuItem();
            this.mnuRenameGroup = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.mnuListBarShortcut = new System.Windows.Forms.MenuItem();
            this.mnuListBarWebShortcut = new System.Windows.Forms.MenuItem();
            this.menuItem12 = new System.Windows.Forms.MenuItem();
            this.mnuHideOutlookBar = new System.Windows.Forms.MenuItem();
            this.splitListBar = new System.Windows.Forms.Splitter();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.chkEnabled = new System.Windows.Forms.CheckBox();
            this.chkCustomColors = new System.Windows.Forms.CheckBox();
            this.chkBackground = new System.Windows.Forms.CheckBox();
            this.picBackground = new System.Windows.Forms.PictureBox();
            this.chkStyle = new System.Windows.Forms.CheckBox();
            this.splitProperties = new System.Windows.Forms.Splitter();
            this.pnlProperties.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).BeginInit();
            this.SuspendLayout();
            // 
            // listBar1
            // 
            this.listBar1.AllowDragGroups = true;
            this.listBar1.AllowDragItems = true;
            this.listBar1.AllowDrop = true;
            this.listBar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.listBar1.DrawStyle = Desktop.Windows.Forms.ListBarDrawStyle.ListBarDrawStyleOfficeXP;
            this.listBar1.LargeImageList = null;
            this.listBar1.Location = new System.Drawing.Point(0, 0);
            this.listBar1.Name = "listBar1";
            this.listBar1.SelectOnMouseDown = false;
            this.listBar1.Size = new System.Drawing.Size(176, 398);
            this.listBar1.SmallImageList = null;
            this.listBar1.TabIndex = 0;
            this.listBar1.ToolTip = null;
            // 
            // tvwCustom
            // 
            this.tvwCustom.Location = new System.Drawing.Point(37, 174);
            this.tvwCustom.Name = "tvwCustom";
            this.tvwCustom.Size = new System.Drawing.Size(121, 97);
            this.tvwCustom.TabIndex = 1;
            // 
            // ilsIconsLarge
            // 
            this.ilsIconsLarge.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilsIconsLarge.ImageStream")));
            this.ilsIconsLarge.TransparentColor = System.Drawing.Color.Transparent;
            this.ilsIconsLarge.Images.SetKeyName(0, "");
            this.ilsIconsLarge.Images.SetKeyName(1, "");
            this.ilsIconsLarge.Images.SetKeyName(2, "");
            this.ilsIconsLarge.Images.SetKeyName(3, "");
            this.ilsIconsLarge.Images.SetKeyName(4, "");
            this.ilsIconsLarge.Images.SetKeyName(5, "");
            // 
            // ilsIconsSmall
            // 
            this.ilsIconsSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilsIconsSmall.ImageStream")));
            this.ilsIconsSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.ilsIconsSmall.Images.SetKeyName(0, "");
            this.ilsIconsSmall.Images.SetKeyName(1, "");
            this.ilsIconsSmall.Images.SetKeyName(2, "");
            this.ilsIconsSmall.Images.SetKeyName(3, "");
            this.ilsIconsSmall.Images.SetKeyName(4, "");
            // 
            // toolTips
            // 
            this.toolTips.IsBalloon = true;
            this.toolTips.ToolTipTitle = "Hola a todos";
            // 
            // pnlProperties
            // 
            this.pnlProperties.Controls.Add(this.sectionHeader1);
            this.pnlProperties.Controls.Add(this.cboGroups);
            this.pnlProperties.Controls.Add(this.groupPropertyGrid);
            this.pnlProperties.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlProperties.Location = new System.Drawing.Point(324, 0);
            this.pnlProperties.Name = "pnlProperties";
            this.pnlProperties.Size = new System.Drawing.Size(208, 398);
            this.pnlProperties.TabIndex = 3;
            // 
            // sectionHeader1
            // 
            this.sectionHeader1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(200)))), ((int)(((byte)(245)))));
            this.sectionHeader1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sectionHeader1.ForeColor = System.Drawing.Color.Black;
            this.sectionHeader1.Location = new System.Drawing.Point(0, 0);
            this.sectionHeader1.Name = "sectionHeader1";
            this.sectionHeader1.Size = new System.Drawing.Size(208, 20);
            this.sectionHeader1.TabIndex = 5;
            this.sectionHeader1.Text = "Properties";
            // 
            // cboGroups
            // 
            this.cboGroups.FormattingEnabled = true;
            this.cboGroups.Location = new System.Drawing.Point(0, 25);
            this.cboGroups.Name = "cboGroups";
            this.cboGroups.Size = new System.Drawing.Size(200, 21);
            this.cboGroups.TabIndex = 4;
            this.cboGroups.Text = "comboBox1";
            // 
            // groupPropertyGrid
            // 
            this.groupPropertyGrid.CommandsBackColor = System.Drawing.Color.White;
            this.groupPropertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.groupPropertyGrid.Location = new System.Drawing.Point(0, 52);
            this.groupPropertyGrid.Name = "groupPropertyGrid";
            this.groupPropertyGrid.Size = new System.Drawing.Size(200, 334);
            this.groupPropertyGrid.TabIndex = 3;
            // 
            // itemContextMenu
            // 
            this.itemContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuContextOpen,
            this.mnuContextOpenNew,
            this.mnuContextSendTo,
            this.mnuContextAdvancedFind,
            this.menuItem3,
            this.mnuContextRemove,
            this.mnuContextRename,
            this.menuItem8,
            this.mnuContextProperties});
            // 
            // mnuContextOpen
            // 
            this.mnuContextOpen.Index = 0;
            this.mnuContextOpen.Text = "&Open";
            // 
            // mnuContextOpenNew
            // 
            this.mnuContextOpenNew.Index = 1;
            this.mnuContextOpenNew.Text = "Open in New &Window";
            // 
            // mnuContextSendTo
            // 
            this.mnuContextSendTo.Enabled = false;
            this.mnuContextSendTo.Index = 2;
            this.mnuContextSendTo.Text = "&Send Link to this Folder";
            // 
            // mnuContextAdvancedFind
            // 
            this.mnuContextAdvancedFind.Index = 3;
            this.mnuContextAdvancedFind.Text = "A&dvanced Find";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 4;
            this.menuItem3.Text = "-";
            // 
            // mnuContextRemove
            // 
            this.mnuContextRemove.Index = 5;
            this.mnuContextRemove.Text = "Re&move from ListBar";
            // 
            // mnuContextRename
            // 
            this.mnuContextRename.Index = 6;
            this.mnuContextRename.Text = "&Rename this shortcut";
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 7;
            this.menuItem8.Text = "-";
            // 
            // mnuContextProperties
            // 
            this.mnuContextProperties.Index = 8;
            this.mnuContextProperties.Text = "Propert&ies";
            // 
            // groupContextMenu
            // 
            this.groupContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuLargeIcons,
            this.mnuSmallIcons,
            this.menuItem4,
            this.mnuAddNewGroup,
            this.mnuRemoveGroup,
            this.mnuRenameGroup,
            this.menuItem9,
            this.mnuListBarShortcut,
            this.mnuListBarWebShortcut,
            this.menuItem12,
            this.mnuHideOutlookBar});
            // 
            // mnuLargeIcons
            // 
            this.mnuLargeIcons.Checked = true;
            this.mnuLargeIcons.Index = 0;
            this.mnuLargeIcons.RadioCheck = true;
            this.mnuLargeIcons.Text = "&Large Icons";
            // 
            // mnuSmallIcons
            // 
            this.mnuSmallIcons.Index = 1;
            this.mnuSmallIcons.RadioCheck = true;
            this.mnuSmallIcons.Text = "&Small Icons";
            // 
            // menuItem4
            // 
            this.menuItem4.Index = 2;
            this.menuItem4.Text = "-";
            // 
            // mnuAddNewGroup
            // 
            this.mnuAddNewGroup.Index = 3;
            this.mnuAddNewGroup.Text = "&Add New Group";
            // 
            // mnuRemoveGroup
            // 
            this.mnuRemoveGroup.Index = 4;
            this.mnuRemoveGroup.Text = "R&emove Group";
            // 
            // mnuRenameGroup
            // 
            this.mnuRenameGroup.Index = 5;
            this.mnuRenameGroup.Text = "&Rename Group";
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 6;
            this.menuItem9.Text = "-";
            // 
            // mnuListBarShortcut
            // 
            this.mnuListBarShortcut.Index = 7;
            this.mnuListBarShortcut.Text = "List&Bar Shortcut...";
            // 
            // mnuListBarWebShortcut
            // 
            this.mnuListBarWebShortcut.Index = 8;
            this.mnuListBarWebShortcut.Text = "ListBar Shortcut to &Web Page...";
            // 
            // menuItem12
            // 
            this.menuItem12.Index = 9;
            this.menuItem12.Text = "-";
            // 
            // mnuHideOutlookBar
            // 
            this.mnuHideOutlookBar.Index = 10;
            this.mnuHideOutlookBar.Text = "&Hide Outlook Bar";
            // 
            // splitListBar
            // 
            this.splitListBar.Location = new System.Drawing.Point(176, 0);
            this.splitListBar.Name = "splitListBar";
            this.splitListBar.Size = new System.Drawing.Size(3, 398);
            this.splitListBar.TabIndex = 4;
            this.splitListBar.TabStop = false;
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.White;
            this.pnlMain.Controls.Add(this.chkEnabled);
            this.pnlMain.Controls.Add(this.chkCustomColors);
            this.pnlMain.Controls.Add(this.chkBackground);
            this.pnlMain.Controls.Add(this.picBackground);
            this.pnlMain.Controls.Add(this.chkStyle);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(179, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(145, 398);
            this.pnlMain.TabIndex = 5;
            // 
            // chkEnabled
            // 
            this.chkEnabled.Checked = true;
            this.chkEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnabled.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkEnabled.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnabled.Location = new System.Drawing.Point(4, 76);
            this.chkEnabled.Name = "chkEnabled";
            this.chkEnabled.Size = new System.Drawing.Size(164, 16);
            this.chkEnabled.TabIndex = 24;
            this.chkEnabled.Text = "E&nabled";
            // 
            // chkCustomColors
            // 
            this.chkCustomColors.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkCustomColors.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCustomColors.Location = new System.Drawing.Point(4, 32);
            this.chkCustomColors.Name = "chkCustomColors";
            this.chkCustomColors.Size = new System.Drawing.Size(164, 16);
            this.chkCustomColors.TabIndex = 23;
            this.chkCustomColors.Text = "Custom &Colours";
            this.chkCustomColors.CheckedChanged += new System.EventHandler(this.chkCustomColors_CheckedChanged_1);
            // 
            // chkBackground
            // 
            this.chkBackground.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkBackground.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBackground.Location = new System.Drawing.Point(4, 52);
            this.chkBackground.Name = "chkBackground";
            this.chkBackground.Size = new System.Drawing.Size(164, 16);
            this.chkBackground.TabIndex = 20;
            this.chkBackground.Text = "&Background Image";
            // 
            // picBackground
            // 
            this.picBackground.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picBackground.BackgroundImage")));
            this.picBackground.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.picBackground.Location = new System.Drawing.Point(4, 128);
            this.picBackground.Name = "picBackground";
            this.picBackground.Size = new System.Drawing.Size(128, 128);
            this.picBackground.TabIndex = 21;
            this.picBackground.TabStop = false;
            this.picBackground.Visible = false;
            // 
            // chkStyle
            // 
            this.chkStyle.Checked = true;
            this.chkStyle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkStyle.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStyle.Location = new System.Drawing.Point(4, 10);
            this.chkStyle.Name = "chkStyle";
            this.chkStyle.Size = new System.Drawing.Size(164, 16);
            this.chkStyle.TabIndex = 22;
            this.chkStyle.Text = "Office &XP Style";
            this.chkStyle.CheckedChanged += new System.EventHandler(this.chkStyle_CheckedChanged_1);
            // 
            // splitProperties
            // 
            this.splitProperties.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitProperties.Location = new System.Drawing.Point(321, 0);
            this.splitProperties.MinExtra = 4;
            this.splitProperties.Name = "splitProperties";
            this.splitProperties.Size = new System.Drawing.Size(3, 398);
            this.splitProperties.TabIndex = 6;
            this.splitProperties.TabStop = false;
            // 
            // frmListBarTest
            // 
            this.ClientSize = new System.Drawing.Size(532, 398);
            this.ControlBox = false;
            this.Controls.Add(this.splitProperties);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.splitListBar);
            this.Controls.Add(this.pnlProperties);
            this.Controls.Add(this.tvwCustom);
            this.Controls.Add(this.listBar1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmListBarTest";
            this.Text = "vbAccelerator .NET ListBar Control Demonstration";
            this.Load += new System.EventHandler(this.frmTestListBar_Load);
            this.pnlProperties.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBackground)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new frmListBarTest());
		}


		private void frmTestListBar_Load(object sender, System.EventArgs e)
		{
			Random randGen = new Random();

			// Add some items to the TreeView:
			tvwCustom.ImageList = ilsIconsSmall;
			tvwCustom.ShowLines = true;
			tvwCustom.ShowPlusMinus = true;		
			TreeNode[] childNodes = new TreeNode[10];
			for (int i = 0; i < 10; i++)
			{
				int iconIndex = randGen.Next(ilsIconsSmall.Images.Count);
				childNodes[i] = new TreeNode(
					String.Format("TreeItem {0}", i),
					iconIndex, iconIndex);
			}
			TreeNode nodTop = new TreeNode("Connections", 0, 0, childNodes);
			tvwCustom.Nodes.Add(nodTop);
			nodTop.Expand();			

			// Add some items to the ListBar:
			listBar1.LargeImageList = ilsIconsLarge;
			listBar1.SmallImageList = ilsIconsSmall;
			listBar1.ToolTip = toolTips;

			// Add some items to the ListBar:
			for (int i = 0; i < 4; i++)
			{
				int jMax = 2 + randGen.Next(10);
				ListBarItem[] subItems = new ListBarItem[jMax];
				for (int j = 0; j < jMax; j++)
				{
					subItems[j] = new ListBarItem(
						String.Format("Test Item {0} in Bar {1}", j + 1, i + 1), 
						j % 4, 
						String.Format("Tooltip text for test item {0}", j + 1));
					if (j == 2)
					{
						subItems[j].Enabled = false;
					}
				}
				listBar1.Groups.Add(
					new ListBarGroup(String.Format("Test {0}", i + 1), 
					subItems));
			}
			// Add a bar containing the Tree control:
			ListBarGroup treeGroup = listBar1.Groups.Add("Tree");
			treeGroup.ChildControl = tvwCustom;

			// Configure ListBar events:
			this.listBar1.ItemClicked += new ItemClickedEventHandler(listBar1_ItemClicked);
			this.listBar1.GroupClicked += new GroupClickedEventHandler(listBar1_GroupClicked);
			this.listBar1.SelectedGroupChanged += new EventHandler(listBar1_SelectedGroupChanged);

			// Configure Menu events:
			this.mnuLargeIcons.Click += new System.EventHandler(this.mnuLargeIcons_Click);
			this.mnuSmallIcons.Click += new System.EventHandler(this.mnuSmallIcons_Click);
			this.mnuAddNewGroup.Click += new System.EventHandler(this.mnuAddNewGroup_Click);
			this.mnuRemoveGroup.Click += new System.EventHandler(this.mnuRemoveGroup_Click);
			this.mnuRenameGroup.Click += new System.EventHandler(this.mnuRenameGroup_Click);
			this.mnuListBarShortcut.Click += new System.EventHandler(this.mnuListBarShortcut_Click);
			this.mnuListBarWebShortcut.Click += new System.EventHandler(this.mnuListBarWebShortcut_Click);
			this.mnuContextOpen.Click += new System.EventHandler(this.mnuContextGeneral_Click);
			this.mnuContextAdvancedFind.Click += new System.EventHandler(this.mnuContextGeneral_Click);
			this.mnuContextOpenNew.Click += new System.EventHandler(this.mnuContextGeneral_Click);
			this.mnuContextProperties.Click += new System.EventHandler(this.mnuContextGeneral_Click);
			this.mnuContextSendTo.Click += new System.EventHandler(this.mnuContextGeneral_Click);
			this.mnuContextRemove.Click += new System.EventHandler(this.mnuContextRemove_Click);
			this.mnuContextRename.Click += new System.EventHandler(this.mnuContextRename_Click);
			this.mnuHideOutlookBar.Click += new System.EventHandler(this.mnuHideOutlookBar_Click);

			// Check box events:
			this.chkStyle.CheckedChanged += new System.EventHandler(this.chkStyle_CheckedChanged);
			this.chkCustomColors.CheckedChanged += new System.EventHandler(this.chkCustomColors_CheckedChanged);
			this.chkBackground.CheckedChanged += new System.EventHandler(this.chkBackground_CheckedChanged);
			this.chkEnabled.CheckedChanged += new System.EventHandler(this.chkEnabled_CheckedChanged);

			// Group property editor
			this.cboGroups.SelectedIndexChanged += new System.EventHandler(this.cboGroups_SelectedIndexChanged);
			this.pnlProperties.SizeChanged += new System.EventHandler(this.pnlProperties_SizeChanged);

			this.showGroupProperties();

		}

		private void listBar1_SelectedGroupChanged(object sender, EventArgs e)
		{
			this.showGroupProperties();
		}

		private void listBar1_ItemClicked(object sender, ItemClickedEventArgs e)
		{
			if (e.MouseButton == MouseButtons.Right)
			{
				contextItem = e.Item;
				itemContextMenu.Show(listBar1, e.Location);
			}
		}

		private void listBar1_GroupClicked(object sender, GroupClickedEventArgs e)
		{
			if (e.MouseButton == MouseButtons.Right)
			{
				contextGroup = e.Group;
				groupContextMenu.Show(listBar1, e.Location);
			}
		}


		private void mnuContextGeneral_Click(object sender, System.EventArgs e)
		{
			string message = String.Format(
				"Chose {0} for item {1}", ((MenuItem)sender).Text, contextItem.Caption);
			MessageBox.Show(this, message,
				this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void mnuContextRemove_Click(object sender, System.EventArgs e)
		{
			string message = String.Format(
				"Are you sure you want to remove the item '{0}'?", contextItem.Caption);
			if (MessageBox.Show(this, message,
				this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				listBar1.SelectedGroup.Items.Remove(contextItem);
			}
			contextItem = null;
		}

		private void mnuContextRename_Click(object sender, System.EventArgs e)
		{
			contextItem.StartEdit();
		}

		private void chkBackground_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkBackground.Checked)
			{
				listBar1.BackgroundImage = picBackground.BackgroundImage;
			}
			else
			{
				listBar1.BackgroundImage = null;
			}
		}

		private void chkStyle_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkStyle.Checked )
			{
				listBar1.DrawStyle = ListBarDrawStyle.ListBarDrawStyleOffice2003;
				listBar1.BackColor = Color.FromKnownColor(KnownColor.Control);
			}
			else
			{
				listBar1.DrawStyle = ListBarDrawStyle.ListBarDrawStyleNormal;
				listBar1.BackColor = Color.FromKnownColor(KnownColor.ControlDark);
			}
		}

		private void mnuLargeIcons_Click(object sender, System.EventArgs e)
		{
			this.listBar1.SelectedGroup.View = ListBarGroupView.LargeIcons;		
			this.mnuSmallIcons.Checked = false;
			this.mnuLargeIcons.Checked = true;
		}

		private void mnuSmallIcons_Click(object sender, System.EventArgs e)
		{
			this.listBar1.SelectedGroup.View = ListBarGroupView.SmallIcons;
			this.mnuSmallIcons.Checked = true;
			this.mnuLargeIcons.Checked = false;
		}

		private void mnuAddNewGroup_Click(object sender, System.EventArgs e)
		{
			this.contextGroup = this.listBar1.Groups.Add("");
			this.listBar1.AfterLabelEdit += new ListBarLabelEditEventHandler(listBar1_AfterLabelEdit);
			this.contextGroup.StartEdit();
		}

		private void listBar1_AfterLabelEdit(object sender, ListBarLabelEditEventArgs e)
		{
			if (e.Label.Trim().Length == 0)
			{
				if (((ListBarGroup)e.LabelEditObject).Caption.Length == 0)
				{
					// Cancel adding this item:
					e.CancelEdit = true;
					// Remove the new bar from the control:
					this.listBar1.Groups.Remove((ListBarGroup)e.LabelEditObject);
				}
				else
				{
					MessageBox.Show(this, "Please enter a caption for the group.", this.Text,
						MessageBoxButtons.OK, MessageBoxIcon.Warning);
					e.CancelEdit = true;
				}
			}
			this.listBar1.AfterLabelEdit -= new ListBarLabelEditEventHandler(listBar1_AfterLabelEdit);
		}

		private void mnuRemoveGroup_Click(object sender, System.EventArgs e)
		{
			this.listBar1.Groups.Remove(this.contextGroup);
			showGroupProperties();
		}

		private void mnuRenameGroup_Click(object sender, System.EventArgs e)
		{
			this.listBar1.AfterLabelEdit += new ListBarLabelEditEventHandler(listBar1_AfterLabelEdit);
			this.contextGroup.StartEdit();
		}

		private void mnuHideOutlookBar_Click(object sender, System.EventArgs e)
		{
			this.listBar1.SelectedGroup.Visible = false;
		}

		private void mnuListBarShortcut_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(this, "Show dialog to add new shortcuts here.", this.Text,
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void mnuListBarWebShortcut_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show(this, "Show dialog to add new web shortcuts here.", this.Text,
				MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void chkCustomColors_CheckedChanged(object sender, System.EventArgs e)
		{
			if (chkBackground.Checked)
			{
				chkBackground.Checked = false;
			}
			if (chkCustomColors.Checked)
			{
				foreach (ListBarGroup group in listBar1.Groups)
				{
					group.BackColor = Color.SteelBlue; //Color.FromArgb(1, 115, 255);
					group.ForeColor = Color.White;//Color.FromArgb(225, 238, 255);
				}
				listBar1.BackColor = Color.FromArgb(185,216, 255);
			}
			else
			{
				foreach (ListBarGroup group in listBar1.Groups)
				{
					group.BackColor = Color.FromKnownColor(KnownColor.Control);
					group.ForeColor = Color.FromKnownColor(KnownColor.WindowText);
				}
				listBar1.BackColor = Color.FromKnownColor(KnownColor.Control);
			}
		}

		private void cboGroups_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListBarGroup grp = (ListBarGroup)cboGroups.SelectedItem;
			groupPropertyGrid.SelectedObject = grp;
			groupPropertyGrid.Update();
		}

		private void pnlProperties_SizeChanged(object sender, EventArgs e)
		{
			int height = pnlProperties.Height - groupPropertyGrid.Top - 4;
			if (height > 0)
			{
				groupPropertyGrid.Height = height;
			}
			int width = pnlProperties.Width - cboGroups.Left * 2;
			if (width > 0)
			{
				cboGroups.Width = width;
				groupPropertyGrid.Width = width;
			}
		}

		private void showGroupProperties()
		{
			cboGroups.Items.Clear();
			foreach (ListBarGroup group in listBar1.Groups)
			{
				int newIndex = cboGroups.Items.Add(group);				
				if (group.Selected)
				{
					cboGroups.SelectedIndex = newIndex;
				}
			}
		}

		private void chkEnabled_CheckedChanged(object sender, System.EventArgs e)
		{
			this.listBar1.Enabled = chkEnabled.Checked;
		}

		private void chkCustomColors_CheckedChanged_1(object sender, System.EventArgs e)
		{
		
		}

		private void sectionHeader1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

        private void chkStyle_CheckedChanged_1(object sender, EventArgs e)
        {

        }

		

		

		
	
	}
}
