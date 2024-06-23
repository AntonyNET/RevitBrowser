using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Autodesk.Revit.UI;
using zRevitFamilyBrowser.Events;
using zRevitFamilyBrowser.Models;
using zRevitFamilyBrowser.Services;
using zRevitFamilyBrowser.ViewModels;

namespace zRevitFamilyBrowser.Windows
{
    public partial class DockPanel : UserControl, IDockablePaneProvider
    {
        private ExternalEvent _externalEvent;
        private SingleInstallEvent _singleInstallEvent;

        private string temp = string.Empty; //удалить
        private int ImageListLength = 0; //удалить

        private string tempFamilyPath = string.Empty; //удалить
        private string tempFamilySymbol = string.Empty; //удалить
        private string tempFamilyName = string.Empty; //удалить

        public DockPanel()
        {
            InitializeComponent();

            _singleInstallEvent = new SingleInstallEvent();
            _externalEvent = ExternalEvent.Create(_singleInstallEvent);

            //var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            //dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            //dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1);
            //dispatcherTimer.Start();

            //CreateEmptyFamilyImage();
        }

        public void SetupDockablePane(DockablePaneProviderData data)
        {
            data.FrameworkElement = this;
            data.InitialState.DockPosition = DockPosition.Left;
        }
    }
}
