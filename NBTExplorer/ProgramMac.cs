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
			// Attach a delegate before entering AppKit because this legacy nib is
			// not reliably inferred by modern .NET for macOS.
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
