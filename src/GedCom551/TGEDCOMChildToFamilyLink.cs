using System;

namespace GedCom551
{
	public sealed class TGEDCOMChildToFamilyLink : TGEDCOMPointerWithNotes
	{
		public TGEDCOMChildLinkageStatus ChildLinkageStatus
		{
			get { return GEDCOMUtils.GetChildLinkageStatusVal(base.GetTagStringValue("STAT").Trim().ToLower()); }
			set { base.SetTagStringValue("STAT", GEDCOMUtils.GetChildLinkageStatusStr(value)); }
		}

		public TGEDCOMPedigreeLinkageType PedigreeLinkageType
		{
			get { return GEDCOMUtils.GetPedigreeLinkageTypeVal(base.GetTagStringValue("PEDI").Trim().ToLower()); }
			set { base.SetTagStringValue("PEDI", GEDCOMUtils.GetPedigreeLinkageTypeStr(value)); }
		}

		public TGEDCOMFamilyRecord Family
		{
			get { return (base.Value as TGEDCOMFamilyRecord); }
			set { base.Value = value; }
		}

		protected override void CreateObj(TGEDCOMTree owner, TGEDCOMObject parent)
		{
			base.CreateObj(owner, parent);
			this.FName = "FAMC";
		}

		public TGEDCOMChildToFamilyLink(TGEDCOMTree owner, TGEDCOMObject parent, string tagName, string tagValue) : base(owner, parent, tagName, tagValue)
		{
		}

        public new static TGEDCOMTag Create(TGEDCOMTree owner, TGEDCOMObject parent, string tagName, string tagValue)
		{
			return new TGEDCOMChildToFamilyLink(owner, parent, tagName, tagValue);
		}
	}
}
