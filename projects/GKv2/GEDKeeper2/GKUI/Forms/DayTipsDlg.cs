﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2018 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GEDKeeper".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Windows.Forms;

using BSLib;
using GKCore;
using GKCore.UIContracts;
using GKUI.Components;

namespace GKUI.Forms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DayTipsDlg : CommonDialog, IDayTipsDlg
    {
        private readonly StringList fTips;

        public DayTipsDlg()
        {
            InitializeComponent();

            Image1.Image = UIHelper.LoadResourceImage("Resources.image_tips_light.png");
            btnClose.Image = UIHelper.LoadResourceImage("Resources.btn_cancel.gif");

            fTips = new StringList();

            // SetLang()
            btnClose.Text = LangMan.LS(LSID.LSID_DlgClose);
            chkShow.Text = LangMan.LS(LSID.LSID_StartupTips);
            btnNextTip.Text = LangMan.LS(LSID.LSID_Next);
            lblTitle.Text = LangMan.LS(LSID.LSID_YouKnowWhat);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                fTips.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GetNextTip()
        {
            if (fTips.Count > 0)
            {
                string tip = fTips[0];

                // processing "title's directives"
                if (!string.IsNullOrEmpty(tip) && tip[0] == '#') {
                    tip = tip.Substring(1);
                    lblTitle.Text = tip;

                    fTips.Delete(0);
                    tip = fTips[0];
                }

                txtTip.Text = tip;
                fTips.Delete(0);
            }
            btnNextTip.Enabled = (fTips.Count > 0);
        }

        private void btnNextTip_Click(object sender, EventArgs e)
        {
            GetNextTip();
        }

        public bool ShowTipsChecked
        {
            get { return chkShow.Checked; }
            set { chkShow.Checked = value; }
        }

        public void Init(string caption, bool showTipsChecked, StringList tips)
        {
            chkShow.Checked = showTipsChecked;
            Text = caption;
            lblTitle.Text = caption;
            fTips.Assign(tips);
            GetNextTip();
        }

        public bool ShowModalX(object owner)
        {
            return (ShowDialog() == DialogResult.OK);
        }
    }
}
