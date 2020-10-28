using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Win32;
using View.Annotations;
using Container = AssemblyBrowserLib.Container;

namespace View
{
    public sealed class ModelView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private readonly AssemblyBrowserLib.AssemblyBrowser _assemblyBrowser = new AssemblyBrowserLib.AssemblyBrowser();
    
    
        private List<Container> _namespace;
        public List<Container> Namespaces { get; set; }
    
        private string _openFile;
        public string OpenFile
        {
            get => _openFile;
            set
            {
                _openFile = value;
                Namespaces = null;
                try
                {
                    Namespaces = _assemblyBrowser.GetAssemblyInfo(_openFile);
                }
                catch (Exception e)
                {
                    _openFile = e.Message;
                    Console.WriteLine(_openFile);
                }
                OnPropertyChanged(nameof(Namespaces));
    
            }
        }
        public ICommand OpenFileCommand => new OpenFileCommand(OpenAssembly);
    
        private void OpenAssembly()
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "Assemblies|*.dll;*.exe", Title = "Select assembly", Multiselect = false
            };
                
            var isOpen = fileDialog.ShowDialog() ;
                
            if (isOpen != null && isOpen.Value)
            {
                OpenFile = fileDialog.FileName;
                OnPropertyChanged(nameof(OpenFile));
            }
        }
    }
    
}