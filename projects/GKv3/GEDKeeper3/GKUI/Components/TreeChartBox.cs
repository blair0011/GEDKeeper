﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2017 by Sergey V. Zhdanovskih.
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

//define DEBUG_IMAGE

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Eto.Drawing;
using Eto.Forms;
using System.Timers;

using GKCommon;
using GKCommon.GEDCOM;
using GKCore;
using GKCore.Charts;
using GKCore.Interfaces;
using GKCore.Options;
using GKUI.Components;

namespace GKUI.Components
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ITreeControl : BaseObject
    {
        protected readonly ITreeChartBox fChart;

        protected Rectangle fDestRect;
        protected bool fMouseCaptured;
        protected bool fVisible;

        public bool MouseCaptured
        {
            get { return fMouseCaptured; }
        }

        public bool Visible
        {
            get {
                return fVisible;
            }
            set {
                if (fVisible != value) {
                    fVisible = value;
                    fChart.Invalidate();
                }
            }
        }

        public abstract string Tip { get; }
        public abstract int Height { get; }
        public abstract int Width { get; }

        public abstract void UpdateState();
        public abstract void UpdateView();
        public abstract void Draw(Graphics gfx);
        public abstract void MouseDown(int x, int y);
        public abstract void MouseMove(int x, int y);
        public abstract void MouseUp(int x, int y);

        protected ITreeControl(ITreeChartBox chart)
        {
            fChart = chart;
        }

        public virtual bool Contains(int x, int y)
        {
            return fDestRect.Contains(x, y);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class TreeChartBox : CustomChart, ITreeChartBox
    {
        #region Subtypes

        private enum ChartControlMode
        {
            ccmDefault,
            ccmDragImage,
            ccmControlsVisible
        }

        private enum MouseAction
        {
            maNone,
            maSelect,
            maExpand,
            maDrag,
            maProperties,
            maHighlight
        }

        private enum MouseEvent
        {
            meDown,
            meMove,
            meUp
        }

        public sealed class TreeControlsList<T> : List<T>, IDisposable where T : ITreeControl
        {
            public void Draw(Graphics gfx)
            {
                if (gfx == null) return;

                for (int i = 0; i < Count; i++) {
                    T ctl = this[i];
                    if (ctl.Visible) ctl.Draw(gfx);
                }
            }

            public void UpdateState()
            {
                for (int i = 0; i < Count; i++) {
                    this[i].UpdateState();
                }
            }

            public void UpdateView()
            {
                for (int i = 0; i < Count; i++) {
                    this[i].UpdateView();
                }
            }

            public ITreeControl Contains(int x, int y)
            {
                for (int i = 0; i < Count; i++) {
                    ITreeControl ctl = this[i];
                    if (ctl.Contains(x, y)) return ctl;
                }

                return null;
            }

            public void MouseDown(int x, int y)
            {
                for (int i = 0; i < Count; i++) {
                    T ctl = this[i];
                    if (ctl.Visible) ctl.MouseDown(x, y);
                }
            }

            public void MouseMove(int x, int y, bool defaultChartMode)
            {
                for (int i = 0; i < Count; i++) {
                    T ctl = this[i];
                    if (ctl.Visible) ctl.MouseMove(x, y);
                }
            }

            public void MouseUp(int x, int y)
            {
                for (int i = 0; i < Count; i++) {
                    T ctl = this[i];
                    if (ctl.Visible) ctl.MouseUp(x, y);
                }
            }

            public void Dispose()
            {
                for (int i = 0; i < Count; i++) {
                    this[i].Dispose();
                }
                Clear();
            }
        }

        #endregion

        #region Private fields

        private readonly TreeControlsList<ITreeControl> fTreeControls;

        private ITreeControl fActiveControl;
        private long fHighlightedStart;
        private ChartControlMode fMode = ChartControlMode.ccmDefault;
        private TreeChartModel fModel;
        private int fMouseX;
        private int fMouseY;
        private TreeChartOptions fOptions;
        //private PersonControl fPersonControl;
        private ChartRenderer fRenderer;
        private TreeChartPerson fSelected;
        private GEDCOMIndividualRecord fSaveSelection;
        private Timer fTimer;
        private bool fTraceKinships;
        private bool fTraceSelected;
        private TweenLibrary fTween;

        #endregion

        #region Public properties

        public event PersonModifyEventHandler PersonModify;

        public event RootChangedEventHandler RootChanged;

        public event EventHandler PersonProperties;


        public IBaseWindow Base
        {
            get { return fModel.Base; }
            set { fModel.Base = value; }
        }

        public bool CertaintyIndex
        {
            get {
                return fModel.CertaintyIndex;
            }
            set {
                fModel.CertaintyIndex = value;
                Invalidate();
            }
        }

        public int DepthLimit
        {
            get { return fModel.DepthLimit; }
            set { fModel.DepthLimit = value; }
        }

        public int IndividualsCount
        {
            get { return fModel.PreparedIndividuals.Count; }
        }

        public TreeChartKind Kind
        {
            get { return fModel.Kind; }
            set { fModel.Kind = value; }
        }

        public TreeChartModel Model
        {
            get { return fModel; }
        }

        public TreeChartOptions Options
        {
            get {
                return fOptions;
            }
            set {
                fOptions = value;
                fModel.Options = value;
            }
        }

        public float Scale
        {
            get { return fModel.Scale; }
        }

        public TreeChartPerson Selected
        {
            get { return fSelected; }
            set { SetSelected(value); }
        }

        public bool TraceSelected
        {
            get { return fTraceSelected; }
            set { fTraceSelected = value; }
        }

        public bool TraceKinships
        {
            get { return fTraceKinships; }
            set { fTraceKinships = value; }
        }

        #endregion

        #region Instance control

        public TreeChartBox()
        {
            //SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            //UpdateStyles();

            //BorderStyle = BorderStyle.Fixed3D;
            //DoubleBuffered = true;
            BackgroundColor = Colors.White;

            fModel = new TreeChartModel();
            fRenderer = null;
            fSelected = null;
            fTraceSelected = true;

            fTreeControls = new TreeControlsList<ITreeControl>();
            fTreeControls.Add(new TCScaleControl(this));
            fTreeControls.Add(new TCGenerationsControl(this));
            //fPersonControl = new PersonControl(this);

            InitTimer();
            fTween = new TweenLibrary();
        }

        public TreeChartBox(ChartRenderer renderer) : this()
        {
            SetRenderer(renderer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                fTween.Dispose();
                fModel.Dispose();

                if (fTreeControls != null) fTreeControls.Dispose();
            }
            base.Dispose(disposing);
        }

        public void SetRenderer(ChartRenderer renderer)
        {
            fRenderer = renderer;
            fModel.SetRenderer(renderer);
        }

        private void InitTimer()
        {
            fTimer = new Timer();
            fTimer.Interval = 1;
            fTimer.Elapsed += TickTimer;
            fTimer.Stop();
            fTimer.Enabled = false;
            fTimer.Enabled = true;
        }

        private void TickTimer(object sender, EventArgs e)
        {
            if (fModel.HighlightedPerson == null) return;

            DateTime st = DateTime.FromBinary(fHighlightedStart);
            DateTime cur = DateTime.Now;
            TimeSpan d = cur - st;

            if (d.Seconds >= 1/* && !fPersonControl.Visible*/)
            {
                fModel.HighlightedPerson = null;
                //fPersonControl.Visible = true;
                Invalidate();
            }
        }

        public void SetScale(float value)
        {
            fModel.Scale = value;

            RecalcChart();

            if (fTraceSelected && fSelected != null)
            {
                CenterPerson(fSelected, false);
            }

            fTreeControls.UpdateState();
        }

        public void GenChart(GEDCOMIndividualRecord iRec, TreeChartKind kind, bool rootCenter)
        {
            if (iRec == null) return;

            try
            {
                fSelected = null;

                fModel.GenChart(iRec, kind, rootCenter);

                RecalcChart();

                if (rootCenter) CenterPerson(fModel.Root, false);

                NavAdd(iRec);
                DoRootChanged(fModel.Root);
            }
            catch (Exception ex)
            {
                Logger.LogWrite("TreeChartBox.GenChart(): " + ex.Message);
            }
        }

        public void RefreshTree()
        {
            try {
                if (fModel.Root == null) return;

                GEDCOMIndividualRecord rootRec = fModel.Root.Rec;

                SaveSelection();
                GenChart(rootRec, fModel.Kind, false);
                RestoreSelection();
            }
            catch (Exception ex)
            {
                Logger.LogWrite("TreeChartBox.RefreshTree(): " + ex.Message);
            }
        }

        public void SaveSelection()
        {
            fSaveSelection = (fSelected == null) ? null : fSelected.Rec;
        }

        public void RestoreSelection()
        {
            SelectByRec(fSaveSelection);
        }

        public void RebuildKinships(bool noRedraw = false)
        {
            if (!fOptions.Kinship || fSelected == null) return;

            try
            {
                fModel.KinRoot = fSelected;
                RecalcChart(noRedraw);
            }
            catch (Exception ex)
            {
                Logger.LogWrite("TreeChartBox.RebuildKinships(): " + ex.Message);
            }
        }

        #endregion

        #region Drawing routines

        public ExtPoint GetOffsets()
        {
            return fModel.GetOffsets();
        }

        private void DrawBackground(BackgroundMode background)
        {
            if (background == BackgroundMode.bmNone) return;

            bool bgImage = false;/*((BackgroundImage != null) &&
                            (background == BackgroundMode.bmAny ||
                             background == BackgroundMode.bmImage));*/

            if (bgImage) {
                /*var imgRect = new Rectangle(0, 0, fImageWidth, fImageHeight);

                    using (Brush textureBrush = new TextureBrush(BackgroundImage, WrapMode.Tile)) {
                        gfx.FillRectangle(textureBrush, imgRect);
                    }*/
            } else {
                bool bgFill = (background == BackgroundMode.bmAny ||
                               background == BackgroundMode.bmImage);

                if (bgFill) {
                    fRenderer.DrawRectangle(null, UIHelper.ConvertColor(BackgroundColor), 0, 0, fModel.ImageWidth, fModel.ImageHeight);
                }
            }
        }

        private void InternalDraw(ChartDrawMode drawMode, BackgroundMode background)
        {
            // drawing relative offset of tree on graphics
            int spx = 0;
            int spy = 0;

            Size clientSize = ClientRectangle.Size;
            if (drawMode == ChartDrawMode.dmInteractive) {
                ExtRect viewport = GetImageViewport();

                if (fModel.ImageWidth < clientSize.Width) {
                    spx += (clientSize.Width - fModel.ImageWidth) / 2;
                }

                if (fModel.ImageHeight < clientSize.Height) {
                    spy += (clientSize.Height - fModel.ImageHeight) / 2;
                }

                fModel.VisibleArea = viewport;
            } else {
                if (drawMode == ChartDrawMode.dmStaticCentered) {
                    if (fModel.ImageWidth < clientSize.Width) {
                        spx += (clientSize.Width - fModel.ImageWidth) / 2;
                    }

                    if (fModel.ImageHeight < clientSize.Height) {
                        spy += (clientSize.Height - fModel.ImageHeight) / 2;
                    }
                }

                fModel.VisibleArea = ExtRect.CreateBounds(0, 0, fModel.ImageWidth, fModel.ImageHeight);
            }

            fModel.SetOffsets(spx, spy);

            DrawBackground(background);

            #if DEBUG_IMAGE
            using (Pen pen = new Pen(Color.Red)) {
                fRenderer.DrawRectangle(pen, Color.Transparent, fSPX, fSPY, fImageWidth, fImageHeight);
            }
            #endif

            fModel.Draw(fModel.Root, fModel.Kind, drawMode);
        }

        #endregion

        #region Sizes and adjustment routines

        private ExtRect GetImageViewport()
        {
            ExtRect viewport;

            var imageSize = GetImageSize();
            if (!imageSize.IsEmpty) {
                Rectangle scrollableViewport = this.Viewport;
                viewport = ExtRect.CreateBounds(
                    scrollableViewport.Left, scrollableViewport.Top,
                    scrollableViewport.Width, scrollableViewport.Height);
            } else {
                viewport = ExtRect.Empty;
            }

            return viewport;
        }

        private ExtRect GetImageRegion()
        {
            ExtRect viewport;

            var imageSize = GetImageSize();
            if (!imageSize.IsEmpty) {
                Rectangle scrollableViewport = this.Viewport;

                bool hasScroll = (scrollableViewport.Width < imageSize.Width || scrollableViewport.Height < imageSize.Height);
                int x, y;
                if (hasScroll) {
                    x = scrollableViewport.Left;
                    y = scrollableViewport.Top;
                } else {
                    x = (scrollableViewport.Width - imageSize.Width) / 2;
                    y = (scrollableViewport.Height - imageSize.Height) / 2;
                }
                int width = Math.Min(imageSize.Width, scrollableViewport.Width);
                int height = Math.Min(imageSize.Height, scrollableViewport.Height);

                viewport = ExtRect.CreateBounds(x, y, width, height);
            } else {
                viewport = ExtRect.Empty;
            }

            return viewport;
        }

        public ExtRect GetClientRect()
        {
            Rectangle rt = Bounds; // ClientRectangle;
            return ExtRect.CreateBounds(rt.Left, rt.Top, rt.Width, rt.Height);
        }

        public void RecalcChart(bool noRedraw = false)
        {
            float fsz = (float)Math.Round(fOptions.DefFontSize * fModel.Scale);
            fModel.DrawFont = AppHost.GfxProvider.CreateFont(fOptions.DefFontName, fsz, false);

            Graphics gfx = null;
            if (fRenderer is TreeChartGfxRenderer) {
                //gfx = CreateGraphics();
                //fRenderer.SetTarget(gfx, false);
            }

            try {
                fModel.RecalcChart(noRedraw);
            } finally {
                if (fRenderer is TreeChartGfxRenderer && gfx != null) {
                    gfx.Dispose();
                }
            }

            var imageSize = GetImageSize();
            AdjustViewport(imageSize, noRedraw);
        }

        #endregion

        #region Event processing

        private void DoPersonModify(PersonModifyEventArgs eArgs)
        {
            var eventHandler = (PersonModifyEventHandler)PersonModify;
            if (eventHandler != null)
                eventHandler(this, eArgs);
        }

        private void DoRootChanged(TreeChartPerson person)
        {
            var eventHandler = (RootChangedEventHandler)RootChanged;
            if (eventHandler != null)
                eventHandler(this, person);
        }

        private void DoPersonProperties(MouseEventArgs eArgs)
        {
            var eventHandler = (/*Mouse*/ EventHandler)PersonProperties;
            if (eventHandler != null)
                eventHandler(this, eArgs);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Keys.Plus:
                    SetScale(fModel.Scale + 0.05f);
                    e.Handled = true;
                    break;

                case Keys.Minus:
                    SetScale(fModel.Scale - 0.05f);
                    e.Handled = true;
                    break;
            }

            base.OnKeyDown(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            SaveSelection();

            var imageSize = GetImageSize();
            AdjustViewport(imageSize);
            fTreeControls.UpdateView();

            RestoreSelection();

            base.OnSizeChanged(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            fRenderer.SetTarget(gfx, false);
            InternalDraw(ChartDrawMode.dmInteractive, BackgroundMode.bmAny);

            // interactive controls
            fTreeControls.Draw(gfx);
            //if (fPersonControl.Visible) fPersonControl.Draw(gfx);

            base.OnPaint(e);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            TreeChartPerson p = fSelected;
            DoPersonModify(new PersonModifyEventArgs(p));

            base.OnMouseDoubleClick(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Modifiers == Keys.Control) {
                float newScale = (e.Delta.Height > 0) ? fModel.Scale + 0.05f : fModel.Scale - 0.05f;
                SetScale(newScale);
            } else {
                base.OnMouseWheel(e);
            }
        }

        private Point GetChartMouseLocation(PointF mpt)
        {
            ExtRect imRect = GetImageRegion();

            int x = (int)mpt.X + imRect.Left; //fSPX;
            int y = (int)mpt.Y + imRect.Top; //fSPY;

            return new Point(x, y);
        }

        private MouseAction GetMouseAction(MouseEventArgs e, MouseEvent mouseEvent, out TreeChartPerson person)
        {
            var result = MouseAction.maNone;
            person = null;

            Point pt = GetChartMouseLocation(e.Location);
            int aX = pt.X;
            int aY = pt.Y;

            int num = fModel.Persons.Count;
            for (int i = 0; i < num; i++) {
                TreeChartPerson p = fModel.Persons[i];
                ExtRect persRt = p.Rect;

                if (persRt.Contains(aX, aY)) {
                    person = p;

                    if (e.Buttons == MouseButtons.Primary && mouseEvent == MouseEvent.meDown)
                    {
                        result = MouseAction.maSelect;
                        break;
                    }
                    else if (e.Buttons == MouseButtons.Alternate && mouseEvent == MouseEvent.meUp)
                    {
                        result = MouseAction.maProperties;
                        break;
                    }
                    else if (mouseEvent == MouseEvent.meMove)
                    {
                        result = MouseAction.maHighlight;
                        break;
                    }
                }

                ExtRect expRt = TreeChartModel.GetExpanderRect(persRt);
                if ((e.Buttons == MouseButtons.Primary && mouseEvent == MouseEvent.meUp) && expRt.Contains(aX, aY)) {
                    person = p;
                    result = MouseAction.maExpand;
                    break;
                }
            }

            if (result == MouseAction.maNone && person == null) {
                if (e.Buttons == MouseButtons.Alternate && mouseEvent == MouseEvent.meDown) {
                    result = MouseAction.maDrag;
                }
            }

            return result;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Point pt = new Point(e.Location);
            fMouseX = pt.X;
            fMouseY = pt.Y;

            switch (fMode) {
                case ChartControlMode.ccmDefault:
                    TreeChartPerson mPers;
                    MouseAction mAct = GetMouseAction(e, MouseEvent.meDown, out mPers);

                    switch (mAct) {
                        case MouseAction.maSelect:
                            SelectBy(mPers, true);
                            break;

                        case MouseAction.maDrag:
                            Cursor = Cursors.Move; // SizeAll;
                            fMode = ChartControlMode.ccmDragImage;
                            break;
                    }
                    break;

                case ChartControlMode.ccmDragImage:
                    break;

                case ChartControlMode.ccmControlsVisible:
                    fTreeControls.MouseDown(pt.X, pt.Y);
                    break;
            }

            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            Point pt = new Point(e.Location);

            switch (fMode)
            {
                case ChartControlMode.ccmDefault:
                    TreeChartPerson mPers;
                    MouseAction mAct = GetMouseAction(e, MouseEvent.meMove, out mPers);

                    if (mAct == MouseAction.maHighlight) {
                        SetHighlight(mPers);
                    } else {
                        SetHighlight(null);

                        ITreeControl ctl = fTreeControls.Contains(pt.X, pt.Y);

                        if (ctl != null) {
                            fMode = ChartControlMode.ccmControlsVisible;
                            ctl.Visible = true;
                            ctl.MouseMove(pt.X, pt.Y);
                            fActiveControl = ctl;

                            //pt = new Point(pt.X + Left, pt.Y + Top);
                            //fToolTip.Show(ctl.Tip, this, pt, 1500);
                            ToolTip = ctl.Tip;
                        }
                    }
                    break;

                case ChartControlMode.ccmDragImage:
                    AdjustScroll(-(pt.X - fMouseX), -(pt.Y - fMouseY));
                    fMouseX = pt.X;
                    fMouseY = pt.Y;
                    break;

                case ChartControlMode.ccmControlsVisible:
                    if (fActiveControl != null) {
                        if (!(fActiveControl.Contains(pt.X, pt.Y) || fActiveControl.MouseCaptured)) {
                            fMode = ChartControlMode.ccmDefault;
                            fActiveControl.Visible = false;
                            //fToolTip.Hide(this);
                            ToolTip = "";
                            fActiveControl = null;
                        } else {
                            fActiveControl.MouseMove(pt.X, pt.Y);
                        }
                    }
                    break;
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            Point pt = new Point(e.Location);

            switch (fMode) {
                case ChartControlMode.ccmDefault:
                    TreeChartPerson mPers;
                    MouseAction mAct = GetMouseAction(e, MouseEvent.meUp, out mPers);

                    switch (mAct) {
                        case MouseAction.maNone:
                            break;

                        case MouseAction.maProperties:
                            SelectBy(mPers, false);
                            if (fSelected == mPers && fSelected.Rec != null)
                            {
                                DoPersonProperties(new MouseEventArgs(e.Buttons, Keys.None, new PointF(pt.X, pt.Y)));
                            }
                            break;

                        case MouseAction.maExpand:
                            DoRootChanged(mPers);
                            GenChart(mPers.Rec, TreeChartKind.ckBoth, true);
                            break;
                    }
                    break;

                case ChartControlMode.ccmDragImage:
                    Cursor = Cursors.Default;
                    fMode = ChartControlMode.ccmDefault;
                    break;

                case ChartControlMode.ccmControlsVisible:
                    fTreeControls.MouseUp(pt.X, pt.Y);
                    break;
            }

            base.OnMouseUp(e);
        }

        #endregion

        #region Navigation

        protected override void SetNavObject(object obj)
        {
            var iRec = obj as GEDCOMIndividualRecord;
            GenChart(iRec, TreeChartKind.ckBoth, true);
        }

        private void SetSelected(TreeChartPerson value)
        {
            if (fSelected != null) fSelected.Selected = false;
            fSelected = value;
            if (fSelected != null) fSelected.Selected = true;

            Invalidate();
        }

        private void SetHighlight(TreeChartPerson person)
        {
            if (fModel.HighlightedPerson == person) return;

            fModel.HighlightedPerson = person;
            fHighlightedStart = DateTime.Now.ToBinary();

            /*if (person == null) {
                //fPersonControl.Visible = false;
            } else {
                //fPersonControl.SetPerson(person);
            }*/

            Invalidate();
        }

        private void SelectBy(TreeChartPerson person, bool needCenter)
        {
            if (person == null) return;

            SetSelected(person);

            //if (fTraceKinships && fOptions.Kinship) RebuildKinships(true);

            if (needCenter && fTraceSelected) CenterPerson(person);
        }

        public void SelectByRec(GEDCOMIndividualRecord iRec)
        {
            if (iRec == null) return;

            int num = fModel.Persons.Count;
            for (int i = 0; i < num; i++) {
                TreeChartPerson p = fModel.Persons[i];
                if (p.Rec == iRec) {
                    SetSelected(p);

                    if (fTraceSelected) CenterPerson(p);

                    return;
                }
            }

            SetSelected(null);
        }

        private void CenterPerson(TreeChartPerson person, bool animation = true)
        {
            if (person == null) return;

            Size clientSize = ClientRectangle.Size;

            int dstX = ((person.PtX) - (clientSize.Width / 2));
            int dstY = ((person.PtY + (person.Height / 2)) - (clientSize.Height / 2));

            if (dstX < 0) dstX = dstX + (0 - dstX);
            if (dstY < 0) dstY = dstY + (0 - dstY);

            int oldX = Math.Abs(AutoScrollPosition.X);
            int oldY = Math.Abs(AutoScrollPosition.Y);

            if ((oldX == dstX) && (oldY == dstY)) return;

            if (animation) {
                fTween.StartTween(SetScroll, oldX, oldY, dstX, dstY, TweenAnimation.EaseInOutQuad, 20);
            } else {
                //fTween.StopTween();
                //SetScroll(dstX, dstY);
                fTween.StartTween(SetScroll, oldX, oldY, dstX, dstY, TweenAnimation.EaseInOutQuad, 1);
            }
        }

        private void SetScroll(int x, int y)
        {
            TweenDelegate invoker = delegate(int newX, int newY) {
                UpdateScrollPosition(newX, newY);
            };

            /*if (InvokeRequired) {
                Invoke(invoker, x, y);
            } else*/ {
                invoker(x, y);
            }
        }

        #endregion

        #region Print support

        public override ExtSize GetImageSize()
        {
            return new ExtSize(fModel.ImageWidth, fModel.ImageHeight);
        }

        public override void RenderStaticImage(Graphics gfx, bool printer)
        {
            BackgroundMode bgMode = (printer) ? BackgroundMode.bmImage : BackgroundMode.bmAny;

            fRenderer.SetTarget(gfx, false);
            RenderStatic(bgMode);
        }

        public void RenderStatic(BackgroundMode background, bool centered = false)
        {
            ChartDrawMode drawMode = (!centered) ? ChartDrawMode.dmStatic : ChartDrawMode.dmStaticCentered;
            InternalDraw(drawMode, background);
        }

        #endregion
    }
}
