﻿using CefSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MarkDownEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static public List<CultureInfo> AllLanguages = new List<CultureInfo>()
        {
            new CultureInfo("en-US"),
            new CultureInfo("zh-CN")
        };

        public App()
        {
            var settings = new CefSettings();
            settings.EnableInternalPdfViewerOffScreen();
            settings.CefCommandLineArgs.Add("disable-gpu", "1");
            Cef.Initialize(settings, shutdownOnProcessExit: false, performDependencyCheck: false);

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var languageName = MarkDownEditor.Properties.Settings.Default.Language;
            if (string.IsNullOrEmpty(languageName))
            {
                var systemCulture = CultureInfo.InstalledUICulture;
                if (AllLanguages.Select(s => s.Name).ToList().Contains(systemCulture.Name))
                {
                    MarkDownEditor.Properties.Resources.Culture = systemCulture;
                    MarkDownEditor.Properties.Settings.Default.Language = systemCulture.Name;
                    MarkDownEditor.Properties.Settings.Default.Save();
                }
                else
                    MarkDownEditor.Properties.Resources.Culture = new CultureInfo("en-US");
            }
            else
                MarkDownEditor.Properties.Resources.Culture = new CultureInfo(languageName);
            ICSharpCode.AvalonEdit.AvalonEditCommands.IndentSelection.InputGestures.Clear();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            ViewModel.ViewModelLocator.Cleanup();
        }        
    }
}
