﻿
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

//using System.Drawing.Drawing2D;

namespace GKUI.Controls
{
	public class GKLogChart : Panel, IDisposable
	{
		private class Fragment
		{
			public int srcval;

			public double val;
			public double log;
			public double percent;

			public int x;
			public int width;
			
			public Rectangle rect;
		}

		private List<Fragment> fList = new List<Fragment>();
		private System.Windows.Forms.ToolTip toolTip;

		public GKLogChart()
		{
			//base.TabStop = true;
			this.toolTip = new System.Windows.Forms.ToolTip();// this.components
			this.toolTip.AutoPopDelay = 5000;
			this.toolTip.InitialDelay = 250;
			this.toolTip.ReshowDelay = 50;
			this.toolTip.ShowAlways = true;
		}

		protected override void Dispose(bool Disposing)
		{
			if (Disposing)
			{
			}
			base.Dispose(Disposing);
		}

		public void Clear()
		{
			fList.Clear();
			this.UpdateContents();
		}

		public void AddFragment(int val)
		{
			if (val < 1) return;
			
			Fragment frag = new Fragment();

			frag.srcval = val;
			if (val == 1) val++;
			frag.val = (double)val;

			fList.Add(frag);

			this.UpdateContents();
		}

		private void UpdateContents()
		{
			int count = fList.Count;
			if (count == 0) return;
			
			int wid = this.Width - (count - 1);

			Fragment frag;

			double sum = 0.0;
			for (int i = 0; i < count; i++) {
				frag = fList[i];

				sum = sum + frag.val;
			}

			double logSum = 0.0;
			for (int i = 0; i < count; i++) {
				frag = fList[i];
				frag.log = Math.Log(frag.val, sum);

				logSum = logSum + frag.log;
			}

			int resWidth = 0;
			for (int i = 0; i < count; i++) {
				frag = fList[i];
				frag.percent = frag.log / logSum;
				frag.width = (int)((double)wid * frag.percent);

				resWidth = resWidth + frag.width;
			}

			// arrange delta
			int d = wid - resWidth;
			if (d > 0) {
				var list = fList.OrderByDescending(z => z.width).ToList();
				int idx = 0;
				while (d > 0) {
					frag = list[idx];
					frag.width = frag.width + 1;

					if (idx == count - 1) {
						idx = 0;
					} else idx++;
					
					d--;
				}
			}

			int x = 0;
			for (int i = 0; i < count; i++) {
				frag = fList[i];

				frag.x = x;
				frag.rect = new Rectangle(x, 0, frag.width, this.Height);
				x = x + (frag.width + 1);
			}

			base.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

			if (this.Width <= 0 || this.Height <= 0) return;

			int count = fList.Count;
			if (count > 0) {
				for (int i = 0; i < count; i++) {
					Fragment frag = fList[i];
					this.DrawRect(e, frag.x, frag.width, Color.Green, Color.Lime);
				}
			} else {
				this.DrawRect(e, 0, this.Width, Color.Gray, Color.Silver);
			}
		}

		private void DrawRect(PaintEventArgs e, int x, int width, Color color1, Color color2)
		{
			Brush lb = new SolidBrush(color1);
			//LinearGradientBrush lb = new LinearGradientBrush(
			//	new Rectangle(0, 0, width, this.Height),
			//	Color.FromArgb(255, color1), Color.FromArgb(50, color2), LinearGradientMode.ForwardDiagonal);

			e.Graphics.FillRectangle(lb, x, 0, width, this.Height);
			lb.Dispose();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);

			int count = fList.Count;
			for (int i = 0; i < count; i++) {
				Fragment frag = fList[i];

				if (frag.rect.Contains(e.X, e.Y)) {
					string st = (i + 1).ToString();
					toolTip.Show("Фрагмент: " + st + ", размер = " + frag.srcval.ToString(), this, e.X, e.Y, 3000);
					break;
				}
			}
		}

	}
}
