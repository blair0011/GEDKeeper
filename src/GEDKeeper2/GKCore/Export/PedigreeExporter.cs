﻿using System;
using System.Collections.Generic;

using ExtUtils;
using GedCom551;
using GKCore.Interfaces;
using GKCore.Options;
using iTextSharp.text;
using iTextSharp.text.pdf;
using it = iTextSharp.text;

/// <summary>
/// Localization: dirty
/// Code-Quality: dirty
/// </summary>

namespace GKCore.Export
{
	public sealed class PedigreeExporter : PDFExporter
	{
		private class TPersonObj
		{
			public TPersonObj Parent;
			public string Id;
			public TGEDCOMIndividualRecord iRec;
			public int Level;
			//public string BirthDate;
			public readonly List<string> Sources;
			public int FamilyOrder;
			public int ChildIdx;

            public TPersonObj()
            {
                this.Sources = new List<string>();
            }

			public string GetOrderStr()
			{
				char order = (char)this.FamilyOrder;
				string result = ((this.Parent == null) ? new string(order, 1) : this.Parent.GetOrderStr() + order);
				return result;
			}
		}

		private class TEventObj
		{
			public readonly TGEDCOMCustomEvent Event;
			public readonly TGEDCOMIndividualRecord iRec;

			public TEventObj(TGEDCOMCustomEvent aEvent, TGEDCOMIndividualRecord aRec)
			{
				this.Event = aEvent;
				this.iRec = aRec;
			}

			public DateTime GetDate()
			{
				DateTime result = ((this.Event == null) ? new DateTime(0) : GKUtils.GEDCOMDateToDate(this.Event.Detail.Date));
				return result;
			}
		}

		public enum TPedigreeKind
		{
			pk_dAboville,
			pk_Konovalov
		}

		private TGEDCOMIndividualRecord FAncestor;
		private TPedigreeKind FKind;
		private ExtList<TPersonObj> FPersonList;
		private ShieldState FShieldState;
		private StringList FSourceList;

		private Font title_font;
		private Font chap_font;
		private Font person_font;
		private Font link_font;
		private Font text_font, suptext;
		
		private PedigreeOptions.PedigreeFormat format;

		public TGEDCOMIndividualRecord Ancestor
		{
			get { return this.FAncestor; }
			set { this.FAncestor = value; }
		}

		public TPedigreeKind Kind
		{
			get { return this.FKind; }
			set { this.FKind = value; }
		}

		public ShieldState ShieldState
		{
			get { return this.FShieldState; }
			set { this.FShieldState = value; }
		}

		private TPersonObj FindPerson(TGEDCOMIndividualRecord iRec)
		{
            if (iRec == null) return null;

            TPersonObj res = null;

			int num = this.FPersonList.Count - 1;
			for (int i = 0; i <= num; i++)
			{
				if ((this.FPersonList[i] as TPersonObj).iRec == iRec)
				{
					res = (this.FPersonList[i] as TPersonObj);
					break;
				}
			}

			return res;
		}

		private void WritePerson(TPersonObj person)
		{
			Paragraph p = new Paragraph();
			p.Alignment = Element.ALIGN_JUSTIFIED;
			p.SpacingBefore = 6f;
			p.SpacingAfter = 6f;
			p.Add(new Chunk(this.GetIdStr(person) + ". " + person.iRec.aux_GetNameStr(true, false), person_font).SetLocalDestination(person.Id));
			p.Add(new Chunk(this.GetPedigreeLifeStr(person.iRec), text_font));

			if (this.FOptions.PedigreeOptions.IncludeSources && person.Sources.Count > 0)
			{
				p.Add(new Chunk(" ", text_font));

				for (int i = 0; i < person.Sources.Count; i++) {
					string lnk = person.Sources[i];
					
					if (i > 0) {
						p.Add(new Chunk(", ", text_font).SetTextRise(4));
					}

					Chunk lnkChunk = new Chunk(lnk, suptext);
					lnkChunk.SetTextRise(4);
					lnkChunk.SetLocalGoto("src_" + lnk);
					lnkChunk.SetUnderline(0.5f, 3f);
					p.Add(lnkChunk);
				}
			}

			fDocument.Add(p);

			switch (format) {
				case PedigreeOptions.PedigreeFormat.pfExcess:
					this.WriteExcessFmt(person);
					break;

				case PedigreeOptions.PedigreeFormat.pfCompact:
					this.WriteCompactFmt(person);
					break;
			}
		}

		private Chunk idLink(TPersonObj person)
		{
			if (person != null) {
				return new Chunk(person.Id, link_font).SetLocalGoto(person.Id);
			} else {
				return new Chunk();
			}
		}

		private string GetIdStr(TPersonObj person)
		{
			string Result = person.Id;

			if (this.FKind == TPedigreeKind.pk_Konovalov && person.Parent != null)
			{
				TGEDCOMFamilyRecord family = person.iRec.ChildToFamilyLinks[0].Family;
				string sp_str = "";
				int idx = person.Parent.iRec.IndexOfSpouse(family);
				if (person.Parent.iRec.SpouseToFamilyLinks.Count > 1)
				{
					sp_str = "/" + (idx + 1).ToString();
				}
				Result += sp_str;
			}
			return Result;
		}

		private void WriteExcessFmt(TPersonObj person)
		{
			fDocument.Add(new Paragraph(LangMan.LS(LSID.LSID_Sex) + ": " + GKUtils.SexStr(person.iRec.Sex), text_font));

			string st = GKUtils.GetLifeExpectancy(person.iRec);
			if (st != "?" && st != "") {
				fDocument.Add(new Paragraph(LangMan.LS(LSID.LSID_LifeExpectancy) + ": " + st, text_font));
			}

			TGEDCOMIndividualRecord father, mother;
			person.iRec.aux_GetParents(out father, out mother);
			if (father != null) {
				Paragraph p = new Paragraph();
				p.Add(new Chunk(LangMan.LS(LSID.LSID_Father) + ": " + father.aux_GetNameStr(true, false) + " ", text_font));
				p.Add(this.idLink(this.FindPerson(father)));
				fDocument.Add(p);
			}
			if (mother != null) {
				Paragraph p = new Paragraph();
				p.Add(new Chunk(LangMan.LS(LSID.LSID_Mother) + ": " + mother.aux_GetNameStr(true, false) + " ", text_font));
				p.Add(this.idLink(this.FindPerson(mother)));
				fDocument.Add(p);
			}

			ExtList<TEventObj> ev_list = new ExtList<TEventObj>(true);
			try
			{
				int i;
				if (person.iRec.IndividualEvents.Count > 0)
				{
					fDocument.Add(new Paragraph(LangMan.LS(LSID.LSID_Events) + ":", text_font));

					int num = person.iRec.IndividualEvents.Count - 1;
					for (i = 0; i <= num; i++)
					{
						TGEDCOMCustomEvent evt = person.iRec.IndividualEvents[i];
						if (!(evt is TGEDCOMIndividualAttribute) || (evt is TGEDCOMIndividualAttribute && this.FOptions.PedigreeOptions.IncludeAttributes))
						{
							ev_list.Add(new TEventObj(evt, person.iRec));
						}
					}
					this.WriteEventList(person, ev_list);
				}

				int num2 = person.iRec.SpouseToFamilyLinks.Count - 1;
				for (i = 0; i <= num2; i++)
				{
					TGEDCOMFamilyRecord family = person.iRec.SpouseToFamilyLinks[i].Family;
					if (GKUtils.IsRecordAccess(family.Restriction, this.FShieldState))
					{
						TGEDCOMPointer sp;
						string unk;
						if (person.iRec.Sex == TGEDCOMSex.svMale) {
							sp = family.Wife;
							st = LangMan.LS(LSID.LSID_Wife) + ": ";
							unk = LangMan.LS(LSID.LSID_UnkFemale);
						} else {
							sp = family.Husband;
							st = LangMan.LS(LSID.LSID_Husband) + ": ";
							unk = LangMan.LS(LSID.LSID_UnkMale);
						}

						TGEDCOMIndividualRecord irec = sp.Value as TGEDCOMIndividualRecord;
						string sps;
						if (irec != null) {
							sps = st + irec.aux_GetNameStr(true, false) + this.GetPedigreeLifeStr(irec) + this.idLink(this.FindPerson(irec));
						} else {
							sps = st + unk;
						}

						fDocument.Add(new Paragraph(sps, text_font));

						ev_list.Clear();
						int num3 = family.Childrens.Count - 1;
						for (int j = 0; j <= num3; j++)
						{
							irec = (family.Childrens[j].Value as TGEDCOMIndividualRecord);
							ev_list.Add(new TEventObj(irec.GetIndividualEvent("BIRT"), irec));
						}
						this.WriteEventList(person, ev_list);
					}
				}
			}
			finally
			{
				ev_list.Dispose();
			}

			if (this.FOptions.PedigreeOptions.IncludeNotes && person.iRec.Notes.Count != 0)
			{
				fDocument.Add(new Paragraph(LangMan.LS(LSID.LSID_RPNotes) + ":", text_font));
				
				it.List list = new it.List(it.List.UNORDERED);
				list.SetListSymbol("\u2022");
				list.IndentationLeft = 10f;

				int num4 = person.iRec.Notes.Count - 1;
				for (int i = 0; i <= num4; i++)
				{
					TGEDCOMNotes note = person.iRec.Notes[i];
					list.Add(new it.ListItem(new Chunk(" " + GKUtils.ConStrings(note.Notes), text_font)));
				}
				fDocument.Add(list);
			}
		}

		private void WriteCompactFmt(TPersonObj person)
		{
			if (this.FOptions.PedigreeOptions.IncludeNotes && person.iRec.Notes.Count != 0)
			{
				int num = person.iRec.Notes.Count - 1;
				for (int i = 0; i <= num; i++)
				{
					TGEDCOMNotes note = person.iRec.Notes[i];
					fDocument.Add(new Paragraph(GKUtils.ConStrings(note.Notes), text_font));
				}
			}

			try
			{
				bool sp_index = person.iRec.SpouseToFamilyLinks.Count > 1;

				int num2 = person.iRec.SpouseToFamilyLinks.Count - 1;
				for (int i = 0; i <= num2; i++)
				{
					TGEDCOMFamilyRecord family = person.iRec.SpouseToFamilyLinks[i].Family;
					if (GKUtils.IsRecordAccess(family.Restriction, this.FShieldState))
					{
						TGEDCOMPointer sp;
						string st;
						string unk;
						if (person.iRec.Sex == TGEDCOMSex.svMale)
						{
							sp = family.Wife;
							st = "Ж";
							unk = LangMan.LS(LSID.LSID_UnkFemale);
						}
						else
						{
							sp = family.Husband;
							st = "М";
							unk = LangMan.LS(LSID.LSID_UnkMale);
						}

						if (sp_index)
						{
							st += (i + 1).ToString();
						}
						st += " - ";

						TGEDCOMIndividualRecord irec = sp.Value as TGEDCOMIndividualRecord;
						if (irec != null)
						{
							st = st + irec.aux_GetNameStr(true, false) + this.GetPedigreeLifeStr(irec) + this.idLink(this.FindPerson(irec));
						}
						else
						{
							st += unk;
						}

						fDocument.Add(new Paragraph(st, text_font));
					}
				}
			}
			finally
			{
			}
		}

		private string GetPedigreeLifeStr(TGEDCOMIndividualRecord iRec)
		{
			string res_str = "";

			PedigreeOptions.PedigreeFormat fmt = this.FOptions.PedigreeOptions.Format;

			if (fmt != PedigreeOptions.PedigreeFormat.pfExcess)
			{
				if (fmt == PedigreeOptions.PedigreeFormat.pfCompact)
				{
					string ds = GKUtils.GetBirthDate(iRec, DateFormat.dfDD_MM_YYYY, true);
					string ps = GKUtils.GetBirthPlace(iRec);
					if (ps != "")
					{
						if (ds != "")
						{
							ds += ", ";
						}
						ds += ps;
					}
					if (ds != "")
					{
						ds = "*" + ds;
					}
					res_str += ds;
					ds = GKUtils.GetDeathDate(iRec, DateFormat.dfDD_MM_YYYY, true);
					ps = GKUtils.GetDeathPlace(iRec);
					if (ps != "")
					{
						if (ds != "")
						{
							ds += ", ";
						}
						ds += ps;
					}
					if (ds != "")
					{
						ds = "+" + ds;
					}
					if (ds != "")
					{
						res_str = res_str + " " + ds;
					}
				}
			}
			else
			{
				string ds = GKUtils.GetBirthDate(iRec, DateFormat.dfDD_MM_YYYY, true);
				if (ds == "")
				{
					ds = "?";
				}
				res_str += ds;
				ds = GKUtils.GetDeathDate(iRec, DateFormat.dfDD_MM_YYYY, true);
				if (ds == "")
				{
					TGEDCOMCustomEvent ev = iRec.GetIndividualEvent("DEAT");
					if (ev != null)
					{
						ds = "?";
					}
				}
				if (ds != "")
				{
					res_str = res_str + " - " + ds;
				}
			}

			string result;
			if (res_str == "" || res_str == " ")
			{
				result = "";
			}
			else
			{
				result = " (" + res_str + ")";
			}
			return result;
		}

		private void WriteEventList(TPersonObj aPerson, ExtList<TEventObj> ev_list)
		{
			int num = ev_list.Count - 1;
			for (int i = 0; i <= num; i++)
			{
				int num2 = ev_list.Count - 1;
				for (int j = i + 1; j <= num2; j++)
				{
					if (ev_list[i].GetDate() > ev_list[j].GetDate())
					{
						ev_list.Exchange(i, j);
					}
				}
			}

			int num3 = ev_list.Count - 1;
			for (int i = 0; i <= num3; i++)
			{
				TGEDCOMCustomEvent evt = ev_list[i].Event;
				if (evt != null && object.Equals(ev_list[i].iRec, aPerson.iRec))
				{
					if (evt.Name == "BIRT") {
						ev_list.Exchange(i, 0);
					} else if (evt.Name == "DEAT") {
						ev_list.Exchange(i, ev_list.Count - 1);
					}
				}
			}

			it.List list = new it.List(it.List.UNORDERED);
			list.SetListSymbol("\u2022");
			list.IndentationLeft = 10f;

			int num4 = ev_list.Count - 1;
			for (int i = 0; i <= num4; i++)
			{
				TEventObj evObj = ev_list[i] as TEventObj;
				TGEDCOMCustomEvent evt = evObj.Event;
				string li;
				Paragraph p = new Paragraph();
				if (evObj.iRec == aPerson.iRec)
				{
					int ev = GKUtils.GetPersonEventIndex(evt.Name);
					string st;
					if (ev == 0) {
						st = evt.Detail.Classification;
					} else {
						if (ev > 0) {
							st = LangMan.LS(GKData.PersonEvents[ev].Name);
						} else {
							st = evt.Name;
						}
					}

					string dt = GKUtils.GEDCOMCustomDateToStr(evt.Detail.Date, DateFormat.dfDD_MM_YYYY, false);
					li = dt + ": " + st + ".";
					if (evt.Detail.Place.StringValue != "")
					{
						li = li + " " + LangMan.LS(LSID.LSID_Place) + ": " + evt.Detail.Place.StringValue;
					}

					p.Add(new Chunk(" " + li, text_font));
				}
				else
				{
					string dt = (evt == null) ? "?" : GKUtils.GEDCOMCustomDateToStr(evt.Detail.Date, DateFormat.dfDD_MM_YYYY, false);

				    string st = (evObj.iRec.Sex == TGEDCOMSex.svMale) ? ": Родился " : ": Родилась ";

					li = dt + st + evObj.iRec.aux_GetNameStr(true, false);
					p.Add(new Chunk(" " + li + " ", text_font));
					p.Add(this.idLink(this.FindPerson(evObj.iRec)));
				}

				list.Add(new it.ListItem(p));
			}

			fDocument.Add(list);
		}

		protected override void InternalGenerate()
		{
			try
			{
				format = this.FOptions.PedigreeOptions.Format;

				if (this.FAncestor == null)
				{
					GKUtils.ShowError(LangMan.LS(LSID.LSID_NotSelectedPerson));
				}
				else
				{
					string title = LangMan.LS(LSID.LSID_ExpPedigree) + ": " + this.FAncestor.aux_GetNameStr(true, false);

					fDocument.AddTitle("Pedigree");
					fDocument.AddSubject("Pedigree");
					fDocument.AddAuthor("");
					fDocument.AddCreator(GKData.AppTitle);
					fDocument.Open();

					BaseFont base_font = BaseFont.CreateFont(Environment.ExpandEnvironmentVariables(@"%systemroot%\fonts\Times.ttf"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
					title_font = new Font(base_font, 20f, Font.BOLD);
					chap_font = new Font(base_font, 16f, Font.BOLD, BaseColor.BLACK);
					person_font = new Font(base_font, 10f, Font.BOLD, BaseColor.BLACK);
					link_font = new Font(base_font, 8f, Font.UNDERLINE, BaseColor.BLUE);
					text_font = new Font(base_font, 8f, Font.NORMAL, BaseColor.BLACK);
					suptext = new Font(base_font, 5f, Font.NORMAL, BaseColor.BLUE);

					fDocument.Add(new Paragraph(title, title_font) { Alignment = Element.ALIGN_CENTER, SpacingAfter = 6f });

					this.FPersonList = new ExtList<TPersonObj>(true);
					this.FSourceList = new StringList();
					try
					{
						this.GenStep(null, this.FAncestor, 1, 1);
						this.ReIndex();

						int cur_level = 0;
						int num = this.FPersonList.Count - 1;
						for (int i = 0; i <= num; i++)
						{
							TPersonObj person = this.FPersonList[i] as TPersonObj;
							if (cur_level != person.Level)
							{
								cur_level = person.Level;
								string gen_title = LangMan.LS(LSID.LSID_Generation)+" "+RomeNumbers.GetRome(cur_level);
								fDocument.Add(new Paragraph(gen_title, chap_font) { Alignment = Element.ALIGN_LEFT, SpacingBefore = 2f, SpacingAfter = 2f });
							}

							this.WritePerson(person);
						}

						if (this.FSourceList.Count > 0)
						{
							fDocument.Add(new Paragraph(LangMan.LS(LSID.LSID_RPSources), chap_font) { Alignment = Element.ALIGN_CENTER });

							int num2 = this.FSourceList.Count - 1;
							for (int j = 0; j <= num2; j++)
							{
								string sn = (j + 1).ToString();
								Chunk chunk = new Chunk(sn + ". " + this.FSourceList[j], text_font);
								chunk.SetLocalDestination("src_" + sn);
								fDocument.Add(new Paragraph(chunk));
							}
						}
					}
					finally
					{
                        this.FSourceList.Dispose();
						this.FPersonList.Dispose();
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public PedigreeExporter(IBase aBase) : base(aBase)
		{
		}

		private void GenStep(TPersonObj parent, TGEDCOMIndividualRecord iRec, int level, int familyOrder)
		{
			if (iRec != null)
			{
				TPersonObj res = new TPersonObj();
				res.Parent = parent;
				res.iRec = iRec;
				res.Level = level;
				res.ChildIdx = 0;
				//res.BirthDate = GKUtils.GetBirthDate(iRec, TDateFormat.dfYYYY_MM_DD, true);
				res.FamilyOrder = familyOrder;
				this.FPersonList.Add(res);

				//string[] i_sources = new string[0];
				int j;

				if (this.FOptions.PedigreeOptions.IncludeSources)
				{
					int num = iRec.SourceCitations.Count - 1;
					for (int i = 0; i <= num; i++)
					{
						TGEDCOMSourceRecord sourceRec = iRec.SourceCitations[i].Value as TGEDCOMSourceRecord;

						if (sourceRec != null) {
							string src_name = GKUtils.ConStrings(sourceRec.Title);
							if (src_name == "") {
								src_name = sourceRec.FiledByEntry;
							}

							j = this.FSourceList.IndexOf(src_name);
							if (j < 0) {
								j = this.FSourceList.Add(src_name);
							}

							res.Sources.Add((j + 1).ToString());
						}
					}
				}

				int num2 = iRec.SpouseToFamilyLinks.Count - 1;
				for (j = 0; j <= num2; j++)
				{
					TGEDCOMFamilyRecord family = iRec.SpouseToFamilyLinks[j].Family;
					if (GKUtils.IsRecordAccess(family.Restriction, this.FShieldState))
					{
						family.aux_SortChilds();

						int num3 = family.Childrens.Count - 1;
						for (int i = 0; i <= num3; i++)
						{
							TGEDCOMIndividualRecord child = family.Childrens[i].Value as TGEDCOMIndividualRecord;
							GenStep(res, child, level + 1, i + 1);
						}
					}
				}
			}
		}

		private void ReIndex()
		{
			int num = this.FPersonList.Count - 1;
			for (int i = 0; i <= num; i++)
			{
				int num2 = this.FPersonList.Count - 1;
				for (int j = i + 1; j <= num2; j++)
				{
					TPersonObj obj = this.FPersonList[i] as TPersonObj;
					TPersonObj obj2 = this.FPersonList[j] as TPersonObj;
					string i_str = (char)obj.Level + obj.GetOrderStr();
					string k_str = (char)obj2.Level + obj2.GetOrderStr();
					if (string.Compare(i_str, k_str, false) > 0)
					{
						this.FPersonList.Exchange(i, j);
					}
				}
			}

			int num3 = this.FPersonList.Count - 1;
			for (int i = 0; i <= num3; i++)
			{
				TPersonObj obj = this.FPersonList[i] as TPersonObj;

				switch (this.FKind) {
					case TPedigreeKind.pk_dAboville:
						if (obj.Parent == null) {
							obj.Id = "1";
						} else {
							obj.Parent.ChildIdx++;
							obj.Id = obj.Parent.Id + "." + obj.Parent.ChildIdx.ToString();
						}
						break;

					case TPedigreeKind.pk_Konovalov:
						obj.Id = (i + 1).ToString();
						if (obj.Parent != null)
						{
							string pid = obj.Parent.Id;

							int p = pid.IndexOf("-");
							if (p >= 0) pid = pid.Substring(0, p);

							obj.Id = obj.Id + "-" + pid;
						}
						break;
				}
			}
		}
	}
}
