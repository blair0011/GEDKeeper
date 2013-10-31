﻿using System;

namespace GKUI
{
	partial class TfmTreeTools
	{
		private System.Windows.Forms.TabControl PageControl;
		private System.Windows.Forms.TabPage SheetTreeCompare;
		private System.Windows.Forms.TextBox ListCompare;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label Label1;
		private System.Windows.Forms.TextBox edCompareFile;
		private System.Windows.Forms.Button btnFileChoose;
		private System.Windows.Forms.OpenFileDialog OpenDialog1;
		private System.Windows.Forms.TabPage SheetTreeMerge;
		private System.Windows.Forms.TabPage SheetTreeSplit;
		private System.Windows.Forms.SaveFileDialog SaveDialog1;
		private System.Windows.Forms.Button btnSelectAll;
		private System.Windows.Forms.ListBox ListSelected;
		private System.Windows.Forms.ListBox ListSkipped;
		private System.Windows.Forms.Button btnSelectFamily;
		private System.Windows.Forms.Button btnSelectAncestors;
		private System.Windows.Forms.Button btnSelectDescendants;
		private System.Windows.Forms.Button btnDelete;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TabPage SheetRecMerge;
		private System.Windows.Forms.TabControl PageControl1;
		private System.Windows.Forms.TabPage SheetMerge;
		private System.Windows.Forms.Button btnSearch;
		private System.Windows.Forms.Button btnSkip;
		private System.Windows.Forms.ProgressBar ProgressBar1;
		private System.Windows.Forms.TabPage SheetOptions;
		private System.Windows.Forms.GroupBox rgMode;
		private System.Windows.Forms.GroupBox GroupBox1;
		private System.Windows.Forms.Label Label5;
		private System.Windows.Forms.Label Label6;
		private System.Windows.Forms.NumericUpDown edNameAccuracy;
		private System.Windows.Forms.NumericUpDown edYearInaccuracy;
		private System.Windows.Forms.CheckBox chkBirthYear;
		private System.Windows.Forms.TabPage SheetTreeImport;
		private System.Windows.Forms.Label Label3;
		private System.Windows.Forms.TextBox edImportFile;
		private System.Windows.Forms.Button btnImportFileChoose;
		private System.Windows.Forms.ListBox ListBox1;
		private System.Windows.Forms.OpenFileDialog OpenDialog2;
		private System.Windows.Forms.TabPage SheetFamilyGroups;
		private System.Windows.Forms.TreeView TreeView1;
		private System.Windows.Forms.TabPage SheetTreeCheck;
		private System.Windows.Forms.Button btnBaseRepair;
		private System.Windows.Forms.Panel Panel1;
		private System.Windows.Forms.Label Label4;
		private System.Windows.Forms.TextBox edMasterBase;
		private System.Windows.Forms.Label Label7;
		private System.Windows.Forms.TextBox edUpdateBase;
		private System.Windows.Forms.Button btnTreeMerge;
		private System.Windows.Forms.TextBox mSyncRes;
		private System.Windows.Forms.TabPage SheetPatSearch;
		private System.Windows.Forms.Button btnPatSearch;
		private System.Windows.Forms.Panel Panel3;
		private System.Windows.Forms.Label Label8;
		private System.Windows.Forms.NumericUpDown edMinGens;
		private System.Windows.Forms.TabPage SheetPlaceManage;
		private System.Windows.Forms.Panel Panel4;
		private System.Windows.Forms.Button btnHelp;
		private System.Windows.Forms.Button btnSetPatriarch;
		private System.Windows.Forms.Button btnIntoList;
		private System.Windows.Forms.RadioButton RadioButton5;
		private System.Windows.Forms.RadioButton RadioButton6;
		private System.Windows.Forms.RadioButton RadioButton7;
		private System.Windows.Forms.RadioButton RadioButton8;

		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TfmTreeTools));
			this.PageControl = new System.Windows.Forms.TabControl();
			this.SheetTreeCompare = new System.Windows.Forms.TabPage();
			this.btnMatch = new System.Windows.Forms.Button();
			this.gbMatchType = new System.Windows.Forms.GroupBox();
			this.rbtnAnalysis = new System.Windows.Forms.RadioButton();
			this.Label1 = new System.Windows.Forms.Label();
			this.edCompareFile = new System.Windows.Forms.TextBox();
			this.btnFileChoose = new System.Windows.Forms.Button();
			this.rbtnMatchInternal = new System.Windows.Forms.RadioButton();
			this.rbtnMathExternal = new System.Windows.Forms.RadioButton();
			this.ListCompare = new System.Windows.Forms.TextBox();
			this.SheetTreeMerge = new System.Windows.Forms.TabPage();
			this.Label4 = new System.Windows.Forms.Label();
			this.Label7 = new System.Windows.Forms.Label();
			this.edMasterBase = new System.Windows.Forms.TextBox();
			this.edUpdateBase = new System.Windows.Forms.TextBox();
			this.btnTreeMerge = new System.Windows.Forms.Button();
			this.mSyncRes = new System.Windows.Forms.TextBox();
			this.SheetTreeSplit = new System.Windows.Forms.TabPage();
			this.btnSelectAll = new System.Windows.Forms.Button();
			this.ListSelected = new System.Windows.Forms.ListBox();
			this.ListSkipped = new System.Windows.Forms.ListBox();
			this.btnSelectFamily = new System.Windows.Forms.Button();
			this.btnSelectAncestors = new System.Windows.Forms.Button();
			this.btnSelectDescendants = new System.Windows.Forms.Button();
			this.btnDelete = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.SheetTreeImport = new System.Windows.Forms.TabPage();
			this.Label3 = new System.Windows.Forms.Label();
			this.edImportFile = new System.Windows.Forms.TextBox();
			this.btnImportFileChoose = new System.Windows.Forms.Button();
			this.ListBox1 = new System.Windows.Forms.ListBox();
			this.SheetRecMerge = new System.Windows.Forms.TabPage();
			this.PageControl1 = new System.Windows.Forms.TabControl();
			this.SheetMerge = new System.Windows.Forms.TabPage();
			this.MergeCtl = new GKUI.Controls.GKMergeControl();
			this.btnSearch = new System.Windows.Forms.Button();
			this.btnSkip = new System.Windows.Forms.Button();
			this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
			this.SheetOptions = new System.Windows.Forms.TabPage();
			this.rgMode = new System.Windows.Forms.GroupBox();
			this.RadioButton8 = new System.Windows.Forms.RadioButton();
			this.RadioButton7 = new System.Windows.Forms.RadioButton();
			this.RadioButton6 = new System.Windows.Forms.RadioButton();
			this.RadioButton5 = new System.Windows.Forms.RadioButton();
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.Label5 = new System.Windows.Forms.Label();
			this.Label6 = new System.Windows.Forms.Label();
			this.chkIndistinctMatching = new System.Windows.Forms.CheckBox();
			this.edNameAccuracy = new System.Windows.Forms.NumericUpDown();
			this.edYearInaccuracy = new System.Windows.Forms.NumericUpDown();
			this.chkBirthYear = new System.Windows.Forms.CheckBox();
			this.SheetFamilyGroups = new System.Windows.Forms.TabPage();
			this.TreeView1 = new System.Windows.Forms.TreeView();
			this.SheetTreeCheck = new System.Windows.Forms.TabPage();
			this.btnBaseRepair = new System.Windows.Forms.Button();
			this.Panel1 = new System.Windows.Forms.Panel();
			this.SheetPatSearch = new System.Windows.Forms.TabPage();
			this.btnPatriarchsDiagram = new System.Windows.Forms.Button();
			this.chkWithoutDates = new System.Windows.Forms.CheckBox();
			this.Label8 = new System.Windows.Forms.Label();
			this.btnPatSearch = new System.Windows.Forms.Button();
			this.Panel3 = new System.Windows.Forms.Panel();
			this.edMinGens = new System.Windows.Forms.NumericUpDown();
			this.btnSetPatriarch = new System.Windows.Forms.Button();
			this.SheetPlaceManage = new System.Windows.Forms.TabPage();
			this.Panel4 = new System.Windows.Forms.Panel();
			this.btnIntoList = new System.Windows.Forms.Button();
			this.btnClose = new System.Windows.Forms.Button();
			this.OpenDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.SaveDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.OpenDialog2 = new System.Windows.Forms.OpenFileDialog();
			this.btnHelp = new System.Windows.Forms.Button();
			this.gkLogChart1 = new GKUI.Controls.GKLogChart();
			this.PageControl.SuspendLayout();
			this.SheetTreeCompare.SuspendLayout();
			this.gbMatchType.SuspendLayout();
			this.SheetTreeMerge.SuspendLayout();
			this.SheetTreeSplit.SuspendLayout();
			this.SheetTreeImport.SuspendLayout();
			this.SheetRecMerge.SuspendLayout();
			this.PageControl1.SuspendLayout();
			this.SheetMerge.SuspendLayout();
			this.SheetOptions.SuspendLayout();
			this.rgMode.SuspendLayout();
			this.GroupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.edNameAccuracy)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.edYearInaccuracy)).BeginInit();
			this.SheetFamilyGroups.SuspendLayout();
			this.SheetTreeCheck.SuspendLayout();
			this.SheetPatSearch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.edMinGens)).BeginInit();
			this.SheetPlaceManage.SuspendLayout();
			this.SuspendLayout();
			// 
			// PageControl
			// 
			this.PageControl.Controls.Add(this.SheetTreeCompare);
			this.PageControl.Controls.Add(this.SheetTreeMerge);
			this.PageControl.Controls.Add(this.SheetTreeSplit);
			this.PageControl.Controls.Add(this.SheetTreeImport);
			this.PageControl.Controls.Add(this.SheetRecMerge);
			this.PageControl.Controls.Add(this.SheetFamilyGroups);
			this.PageControl.Controls.Add(this.SheetTreeCheck);
			this.PageControl.Controls.Add(this.SheetPatSearch);
			this.PageControl.Controls.Add(this.SheetPlaceManage);
			this.PageControl.Location = new System.Drawing.Point(8, 8);
			this.PageControl.Name = "PageControl";
			this.PageControl.SelectedIndex = 0;
			this.PageControl.Size = new System.Drawing.Size(721, 449);
			this.PageControl.TabIndex = 0;
			this.PageControl.SelectedIndexChanged += new System.EventHandler(this.PageControl_SelectedIndexChanged);
			// 
			// SheetTreeCompare
			// 
			this.SheetTreeCompare.Controls.Add(this.btnMatch);
			this.SheetTreeCompare.Controls.Add(this.gbMatchType);
			this.SheetTreeCompare.Controls.Add(this.ListCompare);
			this.SheetTreeCompare.Location = new System.Drawing.Point(4, 22);
			this.SheetTreeCompare.Name = "SheetTreeCompare";
			this.SheetTreeCompare.Size = new System.Drawing.Size(713, 423);
			this.SheetTreeCompare.TabIndex = 0;
			this.SheetTreeCompare.Text = "Сравнить базы данных";
			// 
			// btnMatch
			// 
			this.btnMatch.Location = new System.Drawing.Point(624, 11);
			this.btnMatch.Name = "btnMatch";
			this.btnMatch.Size = new System.Drawing.Size(81, 25);
			this.btnMatch.TabIndex = 7;
			this.btnMatch.Text = "Сравнить";
			this.btnMatch.Click += new System.EventHandler(this.btnMatch_Click);
			// 
			// gbMatchType
			// 
			this.gbMatchType.Controls.Add(this.rbtnAnalysis);
			this.gbMatchType.Controls.Add(this.Label1);
			this.gbMatchType.Controls.Add(this.edCompareFile);
			this.gbMatchType.Controls.Add(this.btnFileChoose);
			this.gbMatchType.Controls.Add(this.rbtnMatchInternal);
			this.gbMatchType.Controls.Add(this.rbtnMathExternal);
			this.gbMatchType.Location = new System.Drawing.Point(8, 3);
			this.gbMatchType.Name = "gbMatchType";
			this.gbMatchType.Size = new System.Drawing.Size(402, 115);
			this.gbMatchType.TabIndex = 6;
			this.gbMatchType.TabStop = false;
			this.gbMatchType.Text = "Тип сравнения";
			// 
			// rbtnAnalysis
			// 
			this.rbtnAnalysis.Location = new System.Drawing.Point(16, 90);
			this.rbtnAnalysis.Name = "rbtnAnalysis";
			this.rbtnAnalysis.Size = new System.Drawing.Size(328, 17);
			this.rbtnAnalysis.TabIndex = 7;
			this.rbtnAnalysis.Text = "Анализ (экспериментальный)";
			this.rbtnAnalysis.CheckedChanged += new System.EventHandler(this.rbtnMatch_CheckedChanged);
			// 
			// Label1
			// 
			this.Label1.Enabled = false;
			this.Label1.Location = new System.Drawing.Point(16, 71);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(35, 13);
			this.Label1.TabIndex = 4;
			this.Label1.Text = "Файл";
			// 
			// edCompareFile
			// 
			this.edCompareFile.Enabled = false;
			this.edCompareFile.Location = new System.Drawing.Point(57, 63);
			this.edCompareFile.Name = "edCompareFile";
			this.edCompareFile.ReadOnly = true;
			this.edCompareFile.Size = new System.Drawing.Size(242, 21);
			this.edCompareFile.TabIndex = 5;
			// 
			// btnFileChoose
			// 
			this.btnFileChoose.Enabled = false;
			this.btnFileChoose.Location = new System.Drawing.Point(305, 60);
			this.btnFileChoose.Name = "btnFileChoose";
			this.btnFileChoose.Size = new System.Drawing.Size(81, 25);
			this.btnFileChoose.TabIndex = 6;
			this.btnFileChoose.Text = "Выбрать...";
			this.btnFileChoose.Click += new System.EventHandler(this.btnFileChoose_Click);
			// 
			// rbtnMatchInternal
			// 
			this.rbtnMatchInternal.Checked = true;
			this.rbtnMatchInternal.Location = new System.Drawing.Point(16, 16);
			this.rbtnMatchInternal.Name = "rbtnMatchInternal";
			this.rbtnMatchInternal.Size = new System.Drawing.Size(328, 17);
			this.rbtnMatchInternal.TabIndex = 2;
			this.rbtnMatchInternal.TabStop = true;
			this.rbtnMatchInternal.Text = "Сравнение внутреннее (поиск дубликатов)";
			this.rbtnMatchInternal.CheckedChanged += new System.EventHandler(this.rbtnMatch_CheckedChanged);
			// 
			// rbtnMathExternal
			// 
			this.rbtnMathExternal.Location = new System.Drawing.Point(16, 40);
			this.rbtnMathExternal.Name = "rbtnMathExternal";
			this.rbtnMathExternal.Size = new System.Drawing.Size(328, 17);
			this.rbtnMathExternal.TabIndex = 3;
			this.rbtnMathExternal.Text = "Сравнение с другой базой";
			this.rbtnMathExternal.CheckedChanged += new System.EventHandler(this.rbtnMatch_CheckedChanged);
			// 
			// ListCompare
			// 
			this.ListCompare.Location = new System.Drawing.Point(8, 124);
			this.ListCompare.Multiline = true;
			this.ListCompare.Name = "ListCompare";
			this.ListCompare.ReadOnly = true;
			this.ListCompare.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.ListCompare.Size = new System.Drawing.Size(697, 292);
			this.ListCompare.TabIndex = 0;
			// 
			// SheetTreeMerge
			// 
			this.SheetTreeMerge.Controls.Add(this.Label4);
			this.SheetTreeMerge.Controls.Add(this.Label7);
			this.SheetTreeMerge.Controls.Add(this.edMasterBase);
			this.SheetTreeMerge.Controls.Add(this.edUpdateBase);
			this.SheetTreeMerge.Controls.Add(this.btnTreeMerge);
			this.SheetTreeMerge.Controls.Add(this.mSyncRes);
			this.SheetTreeMerge.Location = new System.Drawing.Point(4, 22);
			this.SheetTreeMerge.Name = "SheetTreeMerge";
			this.SheetTreeMerge.Size = new System.Drawing.Size(713, 423);
			this.SheetTreeMerge.TabIndex = 1;
			this.SheetTreeMerge.Text = "Объединить базы данных";
			// 
			// Label4
			// 
			this.Label4.Location = new System.Drawing.Point(8, 8);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(80, 13);
			this.Label4.TabIndex = 0;
			this.Label4.Text = "Мастер-база";
			// 
			// Label7
			// 
			this.Label7.Location = new System.Drawing.Point(8, 56);
			this.Label7.Name = "Label7";
			this.Label7.Size = new System.Drawing.Size(100, 13);
			this.Label7.TabIndex = 1;
			this.Label7.Text = "Обновление базы";
			// 
			// edMasterBase
			// 
			this.edMasterBase.BackColor = System.Drawing.SystemColors.Control;
			this.edMasterBase.Location = new System.Drawing.Point(8, 24);
			this.edMasterBase.Name = "edMasterBase";
			this.edMasterBase.ReadOnly = true;
			this.edMasterBase.Size = new System.Drawing.Size(609, 21);
			this.edMasterBase.TabIndex = 0;
			this.edMasterBase.Text = "[текущая база данных]";
			// 
			// edUpdateBase
			// 
			this.edUpdateBase.Location = new System.Drawing.Point(8, 72);
			this.edUpdateBase.Name = "edUpdateBase";
			this.edUpdateBase.ReadOnly = true;
			this.edUpdateBase.Size = new System.Drawing.Size(609, 21);
			this.edUpdateBase.TabIndex = 1;
			// 
			// btnTreeMerge
			// 
			this.btnTreeMerge.Location = new System.Drawing.Point(624, 70);
			this.btnTreeMerge.Name = "btnTreeMerge";
			this.btnTreeMerge.Size = new System.Drawing.Size(81, 25);
			this.btnTreeMerge.TabIndex = 2;
			this.btnTreeMerge.Text = "Выбрать...";
			this.btnTreeMerge.Click += new System.EventHandler(this.btnTreeMerge_Click);
			// 
			// mSyncRes
			// 
			this.mSyncRes.Location = new System.Drawing.Point(8, 108);
			this.mSyncRes.Multiline = true;
			this.mSyncRes.Name = "mSyncRes";
			this.mSyncRes.ReadOnly = true;
			this.mSyncRes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.mSyncRes.Size = new System.Drawing.Size(697, 308);
			this.mSyncRes.TabIndex = 4;
			// 
			// SheetTreeSplit
			// 
			this.SheetTreeSplit.Controls.Add(this.btnSelectAll);
			this.SheetTreeSplit.Controls.Add(this.ListSelected);
			this.SheetTreeSplit.Controls.Add(this.ListSkipped);
			this.SheetTreeSplit.Controls.Add(this.btnSelectFamily);
			this.SheetTreeSplit.Controls.Add(this.btnSelectAncestors);
			this.SheetTreeSplit.Controls.Add(this.btnSelectDescendants);
			this.SheetTreeSplit.Controls.Add(this.btnDelete);
			this.SheetTreeSplit.Controls.Add(this.btnSave);
			this.SheetTreeSplit.Location = new System.Drawing.Point(4, 22);
			this.SheetTreeSplit.Name = "SheetTreeSplit";
			this.SheetTreeSplit.Size = new System.Drawing.Size(713, 423);
			this.SheetTreeSplit.TabIndex = 2;
			this.SheetTreeSplit.Text = "Разделить базу данных";
			// 
			// btnSelectAll
			// 
			this.btnSelectAll.Location = new System.Drawing.Point(8, 352);
			this.btnSelectAll.Name = "btnSelectAll";
			this.btnSelectAll.Size = new System.Drawing.Size(120, 25);
			this.btnSelectAll.TabIndex = 0;
			this.btnSelectAll.Text = "Выбрать все связи";
			this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
			// 
			// ListSelected
			// 
			this.ListSelected.Location = new System.Drawing.Point(8, 8);
			this.ListSelected.Name = "ListSelected";
			this.ListSelected.Size = new System.Drawing.Size(345, 316);
			this.ListSelected.TabIndex = 1;
			// 
			// ListSkipped
			// 
			this.ListSkipped.Location = new System.Drawing.Point(360, 8);
			this.ListSkipped.Name = "ListSkipped";
			this.ListSkipped.Size = new System.Drawing.Size(345, 316);
			this.ListSkipped.TabIndex = 2;
			// 
			// btnSelectFamily
			// 
			this.btnSelectFamily.Location = new System.Drawing.Point(136, 352);
			this.btnSelectFamily.Name = "btnSelectFamily";
			this.btnSelectFamily.Size = new System.Drawing.Size(120, 25);
			this.btnSelectFamily.TabIndex = 3;
			this.btnSelectFamily.Text = "Выбрать семью";
			this.btnSelectFamily.Click += new System.EventHandler(this.btnSelectFamily_Click);
			// 
			// btnSelectAncestors
			// 
			this.btnSelectAncestors.Location = new System.Drawing.Point(8, 384);
			this.btnSelectAncestors.Name = "btnSelectAncestors";
			this.btnSelectAncestors.Size = new System.Drawing.Size(120, 25);
			this.btnSelectAncestors.TabIndex = 4;
			this.btnSelectAncestors.Text = "Выбрать предков";
			this.btnSelectAncestors.Click += new System.EventHandler(this.btnSelectAncestors_Click);
			// 
			// btnSelectDescendants
			// 
			this.btnSelectDescendants.Location = new System.Drawing.Point(136, 384);
			this.btnSelectDescendants.Name = "btnSelectDescendants";
			this.btnSelectDescendants.Size = new System.Drawing.Size(120, 25);
			this.btnSelectDescendants.TabIndex = 5;
			this.btnSelectDescendants.Text = "Выбрать потомков";
			this.btnSelectDescendants.Click += new System.EventHandler(this.btnSelectDescendants_Click);
			// 
			// btnDelete
			// 
			this.btnDelete.Location = new System.Drawing.Point(600, 352);
			this.btnDelete.Name = "btnDelete";
			this.btnDelete.Size = new System.Drawing.Size(105, 25);
			this.btnDelete.TabIndex = 6;
			this.btnDelete.Text = "Удалить";
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(600, 384);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(105, 25);
			this.btnSave.TabIndex = 7;
			this.btnSave.Text = "Сохранить...";
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// SheetTreeImport
			// 
			this.SheetTreeImport.Controls.Add(this.Label3);
			this.SheetTreeImport.Controls.Add(this.edImportFile);
			this.SheetTreeImport.Controls.Add(this.btnImportFileChoose);
			this.SheetTreeImport.Controls.Add(this.ListBox1);
			this.SheetTreeImport.Location = new System.Drawing.Point(4, 22);
			this.SheetTreeImport.Name = "SheetTreeImport";
			this.SheetTreeImport.Size = new System.Drawing.Size(713, 423);
			this.SheetTreeImport.TabIndex = 4;
			this.SheetTreeImport.Text = "Импорт росписей";
			// 
			// Label3
			// 
			this.Label3.Location = new System.Drawing.Point(8, 16);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(31, 13);
			this.Label3.TabIndex = 0;
			this.Label3.Text = "Файл";
			// 
			// edImportFile
			// 
			this.edImportFile.Location = new System.Drawing.Point(40, 8);
			this.edImportFile.Name = "edImportFile";
			this.edImportFile.ReadOnly = true;
			this.edImportFile.Size = new System.Drawing.Size(577, 21);
			this.edImportFile.TabIndex = 0;
			// 
			// btnImportFileChoose
			// 
			this.btnImportFileChoose.Location = new System.Drawing.Point(624, 6);
			this.btnImportFileChoose.Name = "btnImportFileChoose";
			this.btnImportFileChoose.Size = new System.Drawing.Size(81, 25);
			this.btnImportFileChoose.TabIndex = 1;
			this.btnImportFileChoose.Text = "Выбрать...";
			this.btnImportFileChoose.Click += new System.EventHandler(this.btnImportFileChoose_Click);
			// 
			// ListBox1
			// 
			this.ListBox1.Location = new System.Drawing.Point(8, 40);
			this.ListBox1.Name = "ListBox1";
			this.ListBox1.Size = new System.Drawing.Size(697, 342);
			this.ListBox1.TabIndex = 2;
			// 
			// SheetRecMerge
			// 
			this.SheetRecMerge.Controls.Add(this.PageControl1);
			this.SheetRecMerge.Location = new System.Drawing.Point(4, 22);
			this.SheetRecMerge.Name = "SheetRecMerge";
			this.SheetRecMerge.Size = new System.Drawing.Size(713, 423);
			this.SheetRecMerge.TabIndex = 3;
			this.SheetRecMerge.Text = "Объединить дубликаты";
			// 
			// PageControl1
			// 
			this.PageControl1.Controls.Add(this.SheetMerge);
			this.PageControl1.Controls.Add(this.SheetOptions);
			this.PageControl1.Location = new System.Drawing.Point(8, 8);
			this.PageControl1.Name = "PageControl1";
			this.PageControl1.SelectedIndex = 0;
			this.PageControl1.Size = new System.Drawing.Size(689, 406);
			this.PageControl1.TabIndex = 0;
			// 
			// SheetMerge
			// 
			this.SheetMerge.Controls.Add(this.MergeCtl);
			this.SheetMerge.Controls.Add(this.btnSearch);
			this.SheetMerge.Controls.Add(this.btnSkip);
			this.SheetMerge.Controls.Add(this.ProgressBar1);
			this.SheetMerge.Location = new System.Drawing.Point(4, 22);
			this.SheetMerge.Name = "SheetMerge";
			this.SheetMerge.Size = new System.Drawing.Size(681, 380);
			this.SheetMerge.TabIndex = 0;
			this.SheetMerge.Text = "Объединение";
			// 
			// MergeCtl
			// 
			this.MergeCtl.Base = null;
			this.MergeCtl.Dock = System.Windows.Forms.DockStyle.Top;
			this.MergeCtl.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.MergeCtl.Location = new System.Drawing.Point(0, 0);
			this.MergeCtl.MergeMode = GedCom551.TGEDCOMRecordType.rtNone;
			this.MergeCtl.Name = "MergeCtl";
			this.MergeCtl.Size = new System.Drawing.Size(681, 340);
			this.MergeCtl.TabIndex = 11;
			// 
			// btnSearch
			// 
			this.btnSearch.Location = new System.Drawing.Point(12, 346);
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(75, 25);
			this.btnSearch.TabIndex = 0;
			this.btnSearch.Text = "Автопоиск";
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnSkip
			// 
			this.btnSkip.Location = new System.Drawing.Point(92, 346);
			this.btnSkip.Name = "btnSkip";
			this.btnSkip.Size = new System.Drawing.Size(75, 25);
			this.btnSkip.TabIndex = 9;
			this.btnSkip.Text = "Пропустить";
			this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
			// 
			// ProgressBar1
			// 
			this.ProgressBar1.Location = new System.Drawing.Point(173, 346);
			this.ProgressBar1.Name = "ProgressBar1";
			this.ProgressBar1.Size = new System.Drawing.Size(500, 25);
			this.ProgressBar1.Step = 1;
			this.ProgressBar1.TabIndex = 10;
			// 
			// SheetOptions
			// 
			this.SheetOptions.Controls.Add(this.rgMode);
			this.SheetOptions.Controls.Add(this.GroupBox1);
			this.SheetOptions.Location = new System.Drawing.Point(4, 22);
			this.SheetOptions.Name = "SheetOptions";
			this.SheetOptions.Size = new System.Drawing.Size(681, 380);
			this.SheetOptions.TabIndex = 1;
			this.SheetOptions.Text = "Настройки";
			// 
			// rgMode
			// 
			this.rgMode.Controls.Add(this.RadioButton8);
			this.rgMode.Controls.Add(this.RadioButton7);
			this.rgMode.Controls.Add(this.RadioButton6);
			this.rgMode.Controls.Add(this.RadioButton5);
			this.rgMode.Location = new System.Drawing.Point(8, 8);
			this.rgMode.Name = "rgMode";
			this.rgMode.Size = new System.Drawing.Size(225, 97);
			this.rgMode.TabIndex = 0;
			this.rgMode.TabStop = false;
			this.rgMode.Text = "Записи";
			// 
			// RadioButton8
			// 
			this.RadioButton8.Location = new System.Drawing.Point(16, 72);
			this.RadioButton8.Name = "RadioButton8";
			this.RadioButton8.Size = new System.Drawing.Size(192, 16);
			this.RadioButton8.TabIndex = 3;
			this.RadioButton8.Text = "Источники";
			this.RadioButton8.Click += new System.EventHandler(this.RadioButton8_Click);
			// 
			// RadioButton7
			// 
			this.RadioButton7.Location = new System.Drawing.Point(16, 56);
			this.RadioButton7.Name = "RadioButton7";
			this.RadioButton7.Size = new System.Drawing.Size(192, 16);
			this.RadioButton7.TabIndex = 2;
			this.RadioButton7.Text = "Семьи";
			this.RadioButton7.Click += new System.EventHandler(this.RadioButton8_Click);
			// 
			// RadioButton6
			// 
			this.RadioButton6.Location = new System.Drawing.Point(16, 40);
			this.RadioButton6.Name = "RadioButton6";
			this.RadioButton6.Size = new System.Drawing.Size(192, 16);
			this.RadioButton6.TabIndex = 1;
			this.RadioButton6.Text = "Заметки";
			this.RadioButton6.Click += new System.EventHandler(this.RadioButton8_Click);
			// 
			// RadioButton5
			// 
			this.RadioButton5.Checked = true;
			this.RadioButton5.Location = new System.Drawing.Point(16, 24);
			this.RadioButton5.Name = "RadioButton5";
			this.RadioButton5.Size = new System.Drawing.Size(192, 16);
			this.RadioButton5.TabIndex = 0;
			this.RadioButton5.TabStop = true;
			this.RadioButton5.Text = "Персоны";
			this.RadioButton5.Click += new System.EventHandler(this.RadioButton8_Click);
			// 
			// GroupBox1
			// 
			this.GroupBox1.Controls.Add(this.Label5);
			this.GroupBox1.Controls.Add(this.Label6);
			this.GroupBox1.Controls.Add(this.chkIndistinctMatching);
			this.GroupBox1.Controls.Add(this.edNameAccuracy);
			this.GroupBox1.Controls.Add(this.edYearInaccuracy);
			this.GroupBox1.Controls.Add(this.chkBirthYear);
			this.GroupBox1.Location = new System.Drawing.Point(8, 112);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(295, 159);
			this.GroupBox1.TabIndex = 1;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "Поиск персон";
			// 
			// Label5
			// 
			this.Label5.Location = new System.Drawing.Point(16, 40);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(108, 13);
			this.Label5.TabIndex = 0;
			this.Label5.Text = "Точность имени, %";
			// 
			// Label6
			// 
			this.Label6.Location = new System.Drawing.Point(16, 109);
			this.Label6.Name = "Label6";
			this.Label6.Size = new System.Drawing.Size(108, 13);
			this.Label6.TabIndex = 1;
			this.Label6.Text = "Погрешность лет";
			// 
			// chkIndistinctMatching
			// 
			this.chkIndistinctMatching.Location = new System.Drawing.Point(6, 20);
			this.chkIndistinctMatching.Name = "chkIndistinctMatching";
			this.chkIndistinctMatching.Size = new System.Drawing.Size(265, 17);
			this.chkIndistinctMatching.TabIndex = 1;
			this.chkIndistinctMatching.Text = "Нечеткое сравнение";
			// 
			// edNameAccuracy
			// 
			this.edNameAccuracy.Location = new System.Drawing.Point(16, 56);
			this.edNameAccuracy.Name = "edNameAccuracy";
			this.edNameAccuracy.Size = new System.Drawing.Size(108, 21);
			this.edNameAccuracy.TabIndex = 2;
			this.edNameAccuracy.Value = new decimal(new int[] {
									90,
									0,
									0,
									0});
			// 
			// edYearInaccuracy
			// 
			this.edYearInaccuracy.Location = new System.Drawing.Point(16, 125);
			this.edYearInaccuracy.Name = "edYearInaccuracy";
			this.edYearInaccuracy.Size = new System.Drawing.Size(108, 21);
			this.edYearInaccuracy.TabIndex = 4;
			this.edYearInaccuracy.Value = new decimal(new int[] {
									3,
									0,
									0,
									0});
			// 
			// chkBirthYear
			// 
			this.chkBirthYear.Location = new System.Drawing.Point(6, 90);
			this.chkBirthYear.Name = "chkBirthYear";
			this.chkBirthYear.Size = new System.Drawing.Size(265, 17);
			this.chkBirthYear.TabIndex = 6;
			this.chkBirthYear.Text = "Учитывать год рождения";
			// 
			// SheetFamilyGroups
			// 
			this.SheetFamilyGroups.Controls.Add(this.gkLogChart1);
			this.SheetFamilyGroups.Controls.Add(this.TreeView1);
			this.SheetFamilyGroups.Location = new System.Drawing.Point(4, 22);
			this.SheetFamilyGroups.Name = "SheetFamilyGroups";
			this.SheetFamilyGroups.Size = new System.Drawing.Size(713, 423);
			this.SheetFamilyGroups.TabIndex = 5;
			this.SheetFamilyGroups.Text = "Проверка связности семей";
			// 
			// TreeView1
			// 
			this.TreeView1.Location = new System.Drawing.Point(8, 8);
			this.TreeView1.Name = "TreeView1";
			this.TreeView1.Size = new System.Drawing.Size(697, 360);
			this.TreeView1.TabIndex = 0;
			this.TreeView1.DoubleClick += new System.EventHandler(this.TreeView1_DoubleClick);
			// 
			// SheetTreeCheck
			// 
			this.SheetTreeCheck.Controls.Add(this.btnBaseRepair);
			this.SheetTreeCheck.Controls.Add(this.Panel1);
			this.SheetTreeCheck.Location = new System.Drawing.Point(4, 22);
			this.SheetTreeCheck.Name = "SheetTreeCheck";
			this.SheetTreeCheck.Size = new System.Drawing.Size(713, 423);
			this.SheetTreeCheck.TabIndex = 6;
			this.SheetTreeCheck.Text = "Проверка базы данных";
			// 
			// btnBaseRepair
			// 
			this.btnBaseRepair.Location = new System.Drawing.Point(560, 382);
			this.btnBaseRepair.Name = "btnBaseRepair";
			this.btnBaseRepair.Size = new System.Drawing.Size(145, 25);
			this.btnBaseRepair.TabIndex = 0;
			this.btnBaseRepair.Text = "Исправить";
			this.btnBaseRepair.Click += new System.EventHandler(this.btnBaseRepair_Click);
			// 
			// Panel1
			// 
			this.Panel1.Location = new System.Drawing.Point(0, 0);
			this.Panel1.Name = "Panel1";
			this.Panel1.Size = new System.Drawing.Size(713, 369);
			this.Panel1.TabIndex = 1;
			// 
			// SheetPatSearch
			// 
			this.SheetPatSearch.Controls.Add(this.btnPatriarchsDiagram);
			this.SheetPatSearch.Controls.Add(this.chkWithoutDates);
			this.SheetPatSearch.Controls.Add(this.Label8);
			this.SheetPatSearch.Controls.Add(this.btnPatSearch);
			this.SheetPatSearch.Controls.Add(this.Panel3);
			this.SheetPatSearch.Controls.Add(this.edMinGens);
			this.SheetPatSearch.Controls.Add(this.btnSetPatriarch);
			this.SheetPatSearch.Location = new System.Drawing.Point(4, 22);
			this.SheetPatSearch.Name = "SheetPatSearch";
			this.SheetPatSearch.Size = new System.Drawing.Size(713, 423);
			this.SheetPatSearch.TabIndex = 7;
			this.SheetPatSearch.Text = "Поиск патриархов";
			// 
			// btnPatriarchsDiagram
			// 
			this.btnPatriarchsDiagram.Location = new System.Drawing.Point(622, 382);
			this.btnPatriarchsDiagram.Name = "btnPatriarchsDiagram";
			this.btnPatriarchsDiagram.Size = new System.Drawing.Size(75, 25);
			this.btnPatriarchsDiagram.TabIndex = 6;
			this.btnPatriarchsDiagram.Text = "Диаграмма";
			this.btnPatriarchsDiagram.Click += new System.EventHandler(this.BtnPatriarchsDiagramClick);
			// 
			// chkWithoutDates
			// 
			this.chkWithoutDates.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.chkWithoutDates.Location = new System.Drawing.Point(247, 383);
			this.chkWithoutDates.Name = "chkWithoutDates";
			this.chkWithoutDates.Size = new System.Drawing.Size(159, 24);
			this.chkWithoutDates.TabIndex = 5;
			this.chkWithoutDates.Text = "Не учитывать даты";
			this.chkWithoutDates.UseVisualStyleBackColor = true;
			// 
			// Label8
			// 
			this.Label8.Location = new System.Drawing.Point(12, 388);
			this.Label8.Name = "Label8";
			this.Label8.Size = new System.Drawing.Size(166, 13);
			this.Label8.TabIndex = 0;
			this.Label8.Text = "Поколений потомков не менее";
			// 
			// btnPatSearch
			// 
			this.btnPatSearch.Location = new System.Drawing.Point(541, 382);
			this.btnPatSearch.Name = "btnPatSearch";
			this.btnPatSearch.Size = new System.Drawing.Size(75, 25);
			this.btnPatSearch.TabIndex = 0;
			this.btnPatSearch.Text = "Поиск";
			this.btnPatSearch.Click += new System.EventHandler(this.btnPatSearch_Click);
			// 
			// Panel3
			// 
			this.Panel3.Location = new System.Drawing.Point(0, 0);
			this.Panel3.Name = "Panel3";
			this.Panel3.Size = new System.Drawing.Size(713, 369);
			this.Panel3.TabIndex = 1;
			// 
			// edMinGens
			// 
			this.edMinGens.Location = new System.Drawing.Point(184, 384);
			this.edMinGens.Name = "edMinGens";
			this.edMinGens.Size = new System.Drawing.Size(57, 21);
			this.edMinGens.TabIndex = 2;
			this.edMinGens.Value = new decimal(new int[] {
									2,
									0,
									0,
									0});
			// 
			// btnSetPatriarch
			// 
			this.btnSetPatriarch.Location = new System.Drawing.Point(412, 382);
			this.btnSetPatriarch.Name = "btnSetPatriarch";
			this.btnSetPatriarch.Size = new System.Drawing.Size(123, 25);
			this.btnSetPatriarch.TabIndex = 4;
			this.btnSetPatriarch.Text = "Установить признак";
			this.btnSetPatriarch.Click += new System.EventHandler(this.btnSetPatriarch_Click);
			// 
			// SheetPlaceManage
			// 
			this.SheetPlaceManage.Controls.Add(this.Panel4);
			this.SheetPlaceManage.Controls.Add(this.btnIntoList);
			this.SheetPlaceManage.Location = new System.Drawing.Point(4, 22);
			this.SheetPlaceManage.Name = "SheetPlaceManage";
			this.SheetPlaceManage.Size = new System.Drawing.Size(713, 423);
			this.SheetPlaceManage.TabIndex = 8;
			this.SheetPlaceManage.Text = "Управление местами";
			// 
			// Panel4
			// 
			this.Panel4.Location = new System.Drawing.Point(0, 0);
			this.Panel4.Name = "Panel4";
			this.Panel4.Size = new System.Drawing.Size(713, 369);
			this.Panel4.TabIndex = 0;
			// 
			// btnIntoList
			// 
			this.btnIntoList.Location = new System.Drawing.Point(8, 384);
			this.btnIntoList.Name = "btnIntoList";
			this.btnIntoList.Size = new System.Drawing.Size(128, 25);
			this.btnIntoList.TabIndex = 1;
			this.btnIntoList.Text = "Внести в справочник";
			this.btnIntoList.Click += new System.EventHandler(this.btnIntoList_Click);
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Image = global::GKResources.iBtnCancel;
			this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnClose.Location = new System.Drawing.Point(648, 480);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(81, 25);
			this.btnClose.TabIndex = 1;
			this.btnClose.Text = "Закрыть";
			this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// OpenDialog1
			// 
			this.OpenDialog1.Filter = "GEDCOM|*.ged|Все файлы (*.*)|*.*";
			// 
			// SaveDialog1
			// 
			this.SaveDialog1.DefaultExt = "ged";
			this.SaveDialog1.Filter = "GEDCOM|*.ged";
			// 
			// OpenDialog2
			// 
			this.OpenDialog2.Filter = resources.GetString("OpenDialog2.Filter");
			// 
			// btnHelp
			// 
			this.btnHelp.Location = new System.Drawing.Point(552, 480);
			this.btnHelp.Name = "btnHelp";
			this.btnHelp.Size = new System.Drawing.Size(81, 25);
			this.btnHelp.TabIndex = 2;
			this.btnHelp.Text = "Справка";
			this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
			// 
			// gkLogChart1
			// 
			this.gkLogChart1.Location = new System.Drawing.Point(8, 380);
			this.gkLogChart1.Name = "gkLogChart1";
			this.gkLogChart1.Size = new System.Drawing.Size(697, 28);
			this.gkLogChart1.TabIndex = 1;
			this.gkLogChart1.TabStop = true;
			// 
			// TfmTreeTools
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(736, 515);
			this.Controls.Add(this.PageControl);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.btnHelp);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TfmTreeTools";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Инструменты";
			this.PageControl.ResumeLayout(false);
			this.SheetTreeCompare.ResumeLayout(false);
			this.SheetTreeCompare.PerformLayout();
			this.gbMatchType.ResumeLayout(false);
			this.gbMatchType.PerformLayout();
			this.SheetTreeMerge.ResumeLayout(false);
			this.SheetTreeMerge.PerformLayout();
			this.SheetTreeSplit.ResumeLayout(false);
			this.SheetTreeImport.ResumeLayout(false);
			this.SheetTreeImport.PerformLayout();
			this.SheetRecMerge.ResumeLayout(false);
			this.PageControl1.ResumeLayout(false);
			this.SheetMerge.ResumeLayout(false);
			this.SheetOptions.ResumeLayout(false);
			this.rgMode.ResumeLayout(false);
			this.GroupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.edNameAccuracy)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.edYearInaccuracy)).EndInit();
			this.SheetFamilyGroups.ResumeLayout(false);
			this.SheetTreeCheck.ResumeLayout(false);
			this.SheetPatSearch.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.edMinGens)).EndInit();
			this.SheetPlaceManage.ResumeLayout(false);
			this.ResumeLayout(false);
		}
		private GKUI.Controls.GKLogChart gkLogChart1;
		private GKUI.Controls.GKMergeControl MergeCtl;
		private System.Windows.Forms.Button btnPatriarchsDiagram;
		private System.Windows.Forms.CheckBox chkIndistinctMatching;
		private System.Windows.Forms.RadioButton rbtnAnalysis;
		private System.Windows.Forms.Button btnMatch;
		private System.Windows.Forms.RadioButton rbtnMathExternal;
		private System.Windows.Forms.RadioButton rbtnMatchInternal;
		private System.Windows.Forms.GroupBox gbMatchType;
		private System.Windows.Forms.CheckBox chkWithoutDates;
	}
}