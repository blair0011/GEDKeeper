﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2021 by Sergey V. Zhdanovskih.
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

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using BSLib.Design.MVP.Controls;
using GDModel;
using GKCore;
using GKCore.Controllers;
using GKCore.MVP.Views;

namespace GKUI.Forms
{
    public sealed partial class LanguageEditDlg : CommonDialog, ILanguageEditDlg
    {
        private readonly LanguageEditDlgController fController;

        public GDMLanguageID LanguageID
        {
            get { return fController.LanguageID; }
            set { fController.LanguageID = value; }
        }

        #region Design

        private TextBlock lblLanguage;
        private ComboBox cmbLanguage;
        private Button btnAccept;
        private Button btnCancel;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            lblLanguage = this.FindControl<TextBlock>("lblLanguage");
            cmbLanguage = this.FindControl<ComboBox>("cmbLanguage");
            btnAccept = this.FindControl<Button>("btnAccept");
            btnCancel = this.FindControl<Button>("btnCancel");
        }

        #endregion

        #region View Interface

        IComboBox ILanguageEditDlg.LanguageCombo
        {
            get { return GetControlHandler<IComboBox>(cmbLanguage); }
        }

        #endregion

        public LanguageEditDlg()
        {
            InitializeComponent();

            //btnAccept.Image = UIHelper.LoadResourceImage("Resources.btn_accept.gif");
            //btnCancel.Image = UIHelper.LoadResourceImage("Resources.btn_cancel.gif");

            // SetLang()
            btnAccept.Content = LangMan.LS(LSID.LSID_DlgAccept);
            btnCancel.Content = LangMan.LS(LSID.LSID_DlgCancel);
            Title = LangMan.LS(LSID.LSID_Language);
            lblLanguage.Text = LangMan.LS(LSID.LSID_Language);

            fController = new LanguageEditDlgController(this);
        }

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = fController.Accept() ? DialogResult.Ok : DialogResult.None;
        }
    }
}