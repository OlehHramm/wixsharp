using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using ConsoleApplication1;
using WixSharp;
using WixSharp.UI.WPF;

public class Script
{
    static public void Main(string[] args)
    {
        var project = new ManagedProject("ManagedSetup",
                      new Dir(@"%ProgramFiles%\My Company\My Product",
                          new File("readme.md")));

        project.GUID = new Guid("6f330b47-2577-43ad-9095-1861ba25889b");
        project.ManagedUI = new ManagedUI();
        project.ManagedUI.InstallDialogs.Add<WixSharp.UI.Forms.WelcomeDialog>()     // stock WinForm dialog
                                        .Add<FeaturesDialog>()                      // stock WinForm dialog
                                        .Add<CustomDialogWith<CustomDialogPanel>>() // custom WPF dialog (minimalistic);
                                        .Add<CustomDialogRawView>()                 // custom WPF dialog
                                        .Add<CustomDialogView>()                    // custom WPF dialog (with Claiburn.Micro as MVVM)
                                        .Add<WixSharp.UI.Forms.ProgressDialog>()    // stock WinForm dialog
                                        .Add<WixSharp.UI.Forms.ExitDialog>();       // stock WinForm dialog

        project.ManagedUI.ModifyDialogs.Add<ProgressDialog>()
                                       .Add<ExitDialog>();

        project.ManagedUI.AutoScaleMode = AutoScaleMode.Dpi;

        project.PreserveTempFiles = true;
        project.SourceBaseDir = @"..\..\";

        project.DefaultRefAssemblies.Add(typeof(Caliburn.Micro.ActivateExtensions).Assembly.Location);  // Caliburn.Micro.dll
        project.DefaultRefAssemblies.Add(typeof(Caliburn.Micro.Bind).Assembly.Location);                // Caliburn.Micro.Platform.dll
        project.DefaultRefAssemblies.Add(typeof(Caliburn.Micro.NameTransformer).Assembly.Location);     // Caliburn.Micro.Platform.Core.dll
        project.DefaultRefAssemblies.Add(typeof(Microsoft.Xaml.Behaviors.Behavior).Assembly.Location);  // Microsoft.Xaml.Behaviors.dll

        project.BuildMsi();
    }
}