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

using BSLib.Design.MVP.Controls;
using GDModel;
using GKCore;
using GKCore.Controllers;
using GKCore.Interfaces;
using GKCore.MVP.Views;
using Windows.UI.Xaml;

namespace GKUI.Forms
{
    public sealed partial class NoteEditDlg : EditorDialog, INoteEditDlg
    {
        private readonly NoteEditDlgController fController;

        public GDMNoteRecord NoteRecord
        {
            get { return fController.NoteRecord; }
            set { fController.NoteRecord = value; }
        }

        #region View Interface

        ITextBox INoteEdit.Note
        {
            get { return GetControlHandler<ITextBox>(txtNote); }
        }

        #endregion

        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = fController.Accept() ? DialogResult.Ok : DialogResult.None;
        }

        public NoteEditDlg() : this(null)
        {
        }

        public NoteEditDlg(IBaseWindow baseWin)
        {
            InitializeComponent();

            //btnAccept.Image = UIHelper.LoadResourceImage("Resources.btn_accept.gif");
            //btnCancel.Image = UIHelper.LoadResourceImage("Resources.btn_cancel.gif");

            // SetLang()
            btnAccept.Content = LangMan.LS(LSID.LSID_DlgAccept);
            btnCancel.Content = LangMan.LS(LSID.LSID_DlgCancel);
            Title = LangMan.LS(LSID.LSID_Note);

            fController = new NoteEditDlgController(this);
            fController.Init(baseWin);
        }
    }
}
