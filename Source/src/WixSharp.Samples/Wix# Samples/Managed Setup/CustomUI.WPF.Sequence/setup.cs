using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Caliburn.Micro;
using WixSharp;
using WixSharp.UI.WPF;

using Custom = WixSharp.UI.WPF.Sequence;

public class Script
{
    [STAThread]
    static public void Main(string[] args)
    {
        BuildMsi();
        // TestDialogs();
    }

    static void BuildMsi()
    {
        // Note, custom dialogs source code may not be the latest version. Thus for the most recent version use VS WixSharp project template.
        var feature1 = new Feature("Feat1", "Feat1", true);
        var feature2 = new Feature("Feat2", "Feat2", true);

        Feature features21 = new Feature("Feat2Child1", "Feat2Child1", true) { Display = FeatureDisplay.expand };
        Feature features22 = new Feature("Feat2Child2", "Feat2Child2", true) { Display = FeatureDisplay.expand };
        feature2.Add(features21, features22);

        var project = new ManagedProject("ManagedSetup",
                      new Dir(@"%ProgramFiles%\My Company\My Product",
                          new File(feature1, "readme.md"),
                          new File(feature2, "setup.cs"),
                          new File(features21, "exta_fr-fr.wxl"),
                          new File(features22, "app.config")));

        project.GUID = new Guid("6f330b47-2577-43ad-9095-3861ba25889b");

        // custom WPF dialogs
        project.ManagedUI = new ManagedUI();

        project.ManagedUI.InstallDialogs.Add<Custom.WelcomeDialog>()
                                        .Add<Custom.LicenceDialog>()
                                        .Add<Custom.FeaturesDialog>()
                                        .Add<Custom.InstallDirDialog>()
                                        .Add<Custom.ProgressDialog>()
                                        .Add<Custom.ExitDialog>();

        project.ManagedUI.ModifyDialogs.Add<Custom.MaintenanceTypeDialog>()
                                       .Add<Custom.ProgressDialog>()
                                       .Add<Custom.ExitDialog>();

        // custom WPF dialog (this project):        Custom.ProgressDialog
        // stock WPF dialog (WixSharp.UI.WPF.dll):  WixSharp.UI.WPF.ProgressDialog

        // project.ManagedUI = ManagedWpfUI.Default;   // WPF based dialogs
        // project.ManagedUI = ManagedUI.DefaultWpf;   // the same as ManagedWpfUI.Default

        // project.ManagedUI = ManagedUI.Default;      // WinForm based dialogs

        project.PreserveTempFiles = true;
        project.SourceBaseDir = @"..\..\";

        project.ManagedUI.AutoScaleMode = AutoScaleMode.Dpi;

        project.DefaultRefAssemblies.Add(typeof(Caliburn.Micro.ActivateExtensions).Assembly.Location);  // Caliburn.Micro.dll
        project.DefaultRefAssemblies.Add(typeof(Caliburn.Micro.Bind).Assembly.Location);                // Caliburn.Micro.Platform.dll
        project.DefaultRefAssemblies.Add(typeof(Caliburn.Micro.NameTransformer).Assembly.Location);     // Caliburn.Micro.Platform.Core.dll
        project.DefaultRefAssemblies.Add(typeof(Microsoft.Xaml.Behaviors.Behavior).Assembly.Location);  // Microsoft.Xaml.Behaviors.dll

        project.BuildMsi();
    }

    static void TestDialogs()
    {
        UIShell.Play(
            "WixSharp_UI_INSTALLDIR=INSTALLDIR", // required by InstallDirDialog for initialization of the demo MSI session
            typeof(Custom.WelcomeDialog),
            typeof(Custom.LicenceDialog),
            typeof(Custom.InstallDirDialog),
            typeof(Custom.MaintenanceTypeDialog),
            typeof(Custom.SetupTypeDialog),
            typeof(Custom.FeaturesDialog),
            typeof(Custom.ProgressDialog),
            typeof(Custom.ExitDialog));
    }
}