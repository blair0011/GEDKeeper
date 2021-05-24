﻿using BSLib.DataViz.ArborGVT;
using Avalonia.Controls;
using Avalonia.Media;

namespace GKUI.Components
{
    /// <summary>
    /// Stub component
    /// </summary>
    public sealed partial class ArborViewer : UserControl
    {
        public Color BackColor
        {
            get { return Colors.White; }
            set { }
        }

        public bool EnergyDebug
        {
            get { return false; }
            set { }
        }

        public bool NodesDragging
        {
            get { return false; }
            set { }
        }

        public ArborSystem Sys
        {
            get { return null; }
            set { }
        }

        public ArborViewer()
        {
            //this.InitializeComponent();
        }

        public void start()
        {

        }
    }
}