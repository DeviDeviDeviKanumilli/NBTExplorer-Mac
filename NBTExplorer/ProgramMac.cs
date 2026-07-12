using System;
using System.Collections.Generic;
using Foundation;
using AppKit;
using ObjCRuntime;

namespace NBTExplorer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        static void Main (string[] args)
        {
			NSApplication.Init ();
			// Modern .NET for macOS does not always infer the delegate from the
			// legacy MainMenu.xib. Attach it explicitly before entering AppKit's
			// event loop so the main window is created at launch.
			AppDelegate appDelegate = new AppDelegate ();
			NSApplication.SharedApplication.Delegate = appDelegate;
			appDelegate.ShowMainWindow ();
			NSApplication.Main (args);
        }

        /*public static void StaticInitFailure (Exception e)
        {
            Console.WriteLine("Static Initialization Failure:");

            Exception original = e;
            while (e != null) {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                e = e.InnerException;
            }

            MessageBox.Show("Application failed during static initialization: " + original.Message);
            Application.Exit();
        }*/
    }
}
