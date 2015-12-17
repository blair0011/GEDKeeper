﻿using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

using GKCommon;
using GKCommon.Controls;
using GKCommon.GEDCOM;
using GKCommon.GEDCOM.Enums;
using GKCore;
using GKCore.Interfaces;

namespace GKUI.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TfmMediaView : Form, ILocalization
    {
        private readonly IBaseWindow fBase;

        private GEDCOMFileReferenceWithTitle fFileRef;
        private bool fExtern;
        private ImageView fImageCtl;

        public bool Extern
        {
            get { return this.fExtern; }
        }

        public GEDCOMFileReferenceWithTitle FileRef
        {
            get { return this.fFileRef; }
            set { this.SetFileRef(value); }
        }

        private void SetFileRef(GEDCOMFileReferenceWithTitle value)
        {
            this.fFileRef = value;
            this.fExtern = false;
            this.Text = this.fFileRef.Title;
            Control ctl = null;

            this.SuspendLayout();

            //MultimediaKind mmKind = GKUtils.GetMultimediaKind(this.fFileRef.MultimediaFormat);

            switch (this.fFileRef.MultimediaFormat) {
                case GEDCOMMultimediaFormat.mfBMP:
                case GEDCOMMultimediaFormat.mfGIF:
                case GEDCOMMultimediaFormat.mfJPG:
                case GEDCOMMultimediaFormat.mfPCX:
                case GEDCOMMultimediaFormat.mfTIF:
                case GEDCOMMultimediaFormat.mfTGA:
                case GEDCOMMultimediaFormat.mfPNG: {
                        Image img = this.fBase.Context.BitmapLoad(this.fFileRef, -1, -1, false);

                        this.fImageCtl = new ImageView();
                        this.fImageCtl.OpenImage(img);
                        ctl = this.fImageCtl;
                        break;
                    }

                case GEDCOMMultimediaFormat.mfWAV:
                case GEDCOMMultimediaFormat.mfAVI:
                case GEDCOMMultimediaFormat.mfMPG: {
                        this.fExtern = true;
                        string targetFile = "";
                        this.fBase.Context.MediaLoad(this.fFileRef, ref targetFile);
                        SysUtils.LoadExtFile(targetFile);
                        break;
                    }

                case GEDCOMMultimediaFormat.mfTXT: {
                        Stream fs;
                        this.fBase.Context.MediaLoad(this.fFileRef, out fs, false);
                        using (StreamReader strd = new StreamReader(fs, Encoding.GetEncoding(1251))) {
                            TextBox txtBox = new TextBox();
                            txtBox.Multiline = true;
                            txtBox.ReadOnly = true;
                            txtBox.ScrollBars = ScrollBars.Both;
                            txtBox.Text = strd.ReadToEnd();
                            ctl = txtBox;
                        }
                        break;
                    }

                case GEDCOMMultimediaFormat.mfRTF: {
                        Stream fs;
                        this.fBase.Context.MediaLoad(this.fFileRef, out fs, false);
                        using (StreamReader strd = new StreamReader(fs)) {
                            RichTextBox txtBox = new RichTextBox();
                            txtBox.ReadOnly = true;
                            txtBox.Text = strd.ReadToEnd();
                            ctl = txtBox;
                        }
                        break;
                    }

                case GEDCOMMultimediaFormat.mfHTM: {
                        Stream fs;
                        this.fBase.Context.MediaLoad(this.fFileRef, out fs, false);

                        ctl = new WebBrowser();
                        (ctl as WebBrowser).DocumentStream = fs;

                        break;
                    }
            }

            if (ctl != null) {
                ctl.Dock = DockStyle.Fill;
                ctl.Location = new Point(0, 0);
                ctl.Size = new Size(100, 100);
                base.Controls.Add(ctl);
                base.Controls.SetChildIndex(ctl, 0);
            }

            this.ResumeLayout(false);
        }

        private void TfmMediaView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) {
                base.Close();
            }
        }

        public TfmMediaView(IBaseWindow aBase)
        {
            this.InitializeComponent();
            this.fBase = aBase;

            this.SetLang();
        }

        public void SetLang()
        {
            if (this.fImageCtl != null) {
                this.fImageCtl.btnSizeToFit.Text = LangMan.LS(LSID.LSID_SizeToFit);
                this.fImageCtl.btnZoomIn.Text = LangMan.LS(LSID.LSID_ZoomIn);
                this.fImageCtl.btnZoomOut.Text = LangMan.LS(LSID.LSID_ZoomOut);
            }
        }
    }
}
