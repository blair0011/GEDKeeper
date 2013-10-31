﻿using System;
using System.Windows.Forms;

using Ext.Utils;
using GedCom551;
using GKCore;
using GKUI.Controls;

/// <summary>
/// Localization: clean
/// </summary>

namespace GKUI
{
	public partial class TfmCommunicationEdit : Form
	{
		private TfmBase FBase;
		private TGEDCOMCommunicationRecord FCommunication;
		private TGEDCOMIndividualRecord FTempInd;
		private GKSheetList FNotesList;
		private GKSheetList FMediaList;

		public TfmBase Base
		{
			get { return this.FBase; }
		}

		public TGEDCOMCommunicationRecord Communication
		{
			get { return this.FCommunication; }
			set { this.SetCommunication(value); }
		}

		private void ListModify(object sender, ModifyEventArgs eArgs)
		{
			bool res = false;

			if (sender == this.FNotesList) {
				TGEDCOMNotes notes = eArgs.ItemData as TGEDCOMNotes;
				res = (this.Base.ModifyRecNote(this, this.FCommunication, notes, eArgs.Action));
			}
			else if (sender == this.FMediaList)
			{
				TGEDCOMMultimediaLink mmLink = eArgs.ItemData as TGEDCOMMultimediaLink;
				res = this.Base.ModifyRecMultimedia(this, this.FCommunication, mmLink, eArgs.Action);
			}

			if (res) this.ListsRefresh();
		}

		private void ListsRefresh()
		{
			this.Base.RecListNotesRefresh(this.FCommunication, this.FNotesList.List, null);
			this.Base.RecListMediaRefresh(this.FCommunication, this.FMediaList.List, null);
		}

		private void SetCommunication(TGEDCOMCommunicationRecord Value)
		{
			this.FCommunication = Value;
			try
			{
				if (this.FCommunication == null)
				{
					this.EditName.Text = "";
					this.EditCorrType.SelectedIndex = -1;
					this.EditDate.Text = "";
					this.EditDir.SelectedIndex = 0;
					this.EditCorresponder.Text = "";
				}
				else
				{
					this.EditName.Text = this.FCommunication.CommName;
					this.EditCorrType.SelectedIndex = (int)this.FCommunication.CommunicationType;
					this.EditDate.Text = GKUtils.GEDCOMDateToStr(this.FCommunication.Date, TDateFormat.dfDD_MM_YYYY);
					TCommunicationDir dir = TCommunicationDir.cdFrom;
					this.FCommunication.GetCorresponder(ref dir, ref this.FTempInd);
					if (this.FTempInd != null)
					{
						this.EditDir.SelectedIndex = (int)dir;
						this.EditCorresponder.Text = this.FTempInd.aux_GetNameStr(true, false);
					}
					else
					{
						this.EditDir.SelectedIndex = 0;
						this.EditCorresponder.Text = "";
					}
				}
				this.ListsRefresh();
			}
			catch (Exception E)
			{
				SysUtils.LogWrite("CommunicationEdit.SetCommunication(): " + E.Message);
			}
		}

		private void btnAccept_Click(object sender, EventArgs e)
		{
			try
			{
				this.FCommunication.CommName = this.EditName.Text;
				this.FCommunication.CommunicationType = (TCommunicationType)this.EditCorrType.SelectedIndex;
				this.FCommunication.Date.ParseString(GKUtils.StrToGEDCOMDate(this.EditDate.Text, true));
				this.FCommunication.SetCorresponder((TCommunicationDir)this.EditDir.SelectedIndex, this.FTempInd);
				this.Base.ChangeRecord(this.FCommunication);
				base.DialogResult = DialogResult.OK;
			}
			catch (Exception E)
			{
				SysUtils.LogWrite("TfmCommunicationEdit.Accept(): " + E.Message);
				base.DialogResult = DialogResult.None;
			}
		}

		private void btnPersonAdd_Click(object sender, EventArgs e)
		{
			this.FTempInd = this.Base.SelectPerson(null, TTargetMode.tmNone, TGEDCOMSex.svNone);
			this.EditCorresponder.Text = ((this.FTempInd == null) ? "" : this.FTempInd.aux_GetNameStr(true, false));
		}

		public TfmCommunicationEdit(TfmBase aBase)
		{
			this.InitializeComponent();
			this.FBase = aBase;

			for (TCommunicationType ct = TCommunicationType.ctCall; ct <= TCommunicationType.ctLast; ct++)
			{
				this.EditCorrType.Items.Add(LangMan.LSList[(int)GKData.CommunicationNames[(int)ct] - 1]);
			}

			this.FNotesList = new GKSheetList(this.SheetNotes);
			this.FNotesList.OnModify += new GKSheetList.ModifyEventHandler(this.ListModify);
			this.Base.SetupRecNotesList(this.FNotesList);

			this.FMediaList = new GKSheetList(this.SheetMultimedia);
			this.FMediaList.OnModify += new GKSheetList.ModifyEventHandler(this.ListModify);
			this.Base.SetupRecMediaList(this.FMediaList);

			this.FTempInd = null;
			this.btnAccept.Text = LangMan.LSList[97];
			this.btnCancel.Text = LangMan.LSList[98];
			this.Text = LangMan.LSList[191];
			this.SheetNotes.Text = LangMan.LSList[54];
			this.SheetMultimedia.Text = LangMan.LSList[55];
			this.Label1.Text = LangMan.LSList[183];
			this.Label5.Text = LangMan.LSList[184];
			this.Label2.Text = LangMan.LSList[113];
			this.Label4.Text = LangMan.LSList[139];
		}
	}
}
