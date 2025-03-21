using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace SimpleBackupService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        private ServiceProcessInstaller ServiceProcessInstaller;
        private ServiceInstaller serviceInstaller;
        public ProjectInstaller()
        {
            InitializeComponent();
            //Configure the service process installer
            ServiceProcessInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem    //Adjust  as needed (e.g., networkService,LocalService)
            };

            //Configure the service installer
            serviceInstaller = new ServiceInstaller
            {
                ServiceName = "SimpleBackupService", // Must match service name in your service base class
                DisplayName = "SimpleBackupService",
                StartType = ServiceStartMode.Manual //Or automatic , depending on requirements
            };

            //Add installers to the installer collection
            Installers.Add(ServiceProcessInstaller);
            Installers.Add(serviceInstaller);

            //C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe SimpleBackupService.exe

            //This command for install service it is used in command prop :   C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe MyFirstWindowsService.exe
            //This for get all services in windows services: sc queryex type= service state= all
            //This for delete service: sc delete ServiceName


        }
    }
}
