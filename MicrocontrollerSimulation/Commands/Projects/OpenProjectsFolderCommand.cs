using MicrocontrollerSimulation.Commands.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MicrocontrollerSimulation.Commands.Projects
{
    public class OpenProjectsFolderCommand : AsyncCommandBase
    {
        private readonly string _folderPath;

        public OpenProjectsFolderCommand(string folderPath)
        {
            _folderPath = folderPath;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                Directory.CreateDirectory(_folderPath);

                await Task.Run(() => Process.Start(new ProcessStartInfo()
                {
                    FileName = "explorer.exe",
                    Arguments = _folderPath
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
