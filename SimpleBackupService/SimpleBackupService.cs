using SimpleBackupService.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBackupService
{
    public partial class SimpleBackupService : ServiceBase
    {
        private int _copiedFiles = 0;
        public SimpleBackupService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _copiedFiles = clsHelper.CopyUniqueFiles();
        }

        protected override void OnStop()
        {
            clsHelper.LogTheProcessInEventViewer(_copiedFiles);
        }


        // This is added
        // Simulate service behavior in console mode
        public void StartInConsole()
        {
            OnStart(null); // Trigger OnStart logic
            Console.WriteLine("Press Enter to stop the service...");
            Console.ReadLine(); // Wait for user input to simulate service stopping
            OnStop(); // Trigger OnStop logic
            Console.ReadKey();

        }
    }
}
