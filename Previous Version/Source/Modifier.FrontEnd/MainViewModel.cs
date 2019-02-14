using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Interfaces;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.ServiceLocation;
using Price.Commons.Desktop;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace Modifier.FrontEnd
{
    public class MainViewModel : ViewModel
    {
        private readonly ISettingsManager _settingsManager;

        public MainViewModel(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
            HookupCommands();
        }

        #region Properties

        private ObservableCollection<FileViewModel> _modifyFileResults;
        public ObservableCollection<FileViewModel> ModifyFileResults
        {
            get { return _modifyFileResults; }
            set
            {
                if (_modifyFileResults != value)
                {
                    _modifyFileResults = value;
                    RaisePropertyChanged(() => ModifyFileResults);
                }
            }
        }

        public string BaseTvPath
        {
            get { return _settingsManager.BaseTvPath; }
            set
            {
                if (_settingsManager.BaseTvPath != value) 
                {
                    _settingsManager.BaseTvPath = value;
                    RaisePropertyChanged(() => BaseTvPath);
                }
            }
        }

        #endregion

        #region Commands

        public DelegateCommand SelectFilesToModifyCommand { get; set; }
        public DelegateCommand SelectDirectoryToModifyCommand { get; set; }
        public DelegateCommand SelectBaseTvPathCommand { get; set; }
        public DelegateCommand MoveAndRenameFilesCommand { get; set; }

        private void HookupCommands()
        {
            SelectFilesToModifyCommand = new DelegateCommand(SelectFilesToModify);
            SelectDirectoryToModifyCommand = new DelegateCommand(SelectDirectoryToModify);
            SelectBaseTvPathCommand = new DelegateCommand(SelectBaseTvPath);
            MoveAndRenameFilesCommand = new DelegateCommand(MoveAndRenameFiles);
        }

        private void SelectDirectoryToModify()
        {
            
        }

        private void SelectFilesToModify()
        {
            // Configure open file dialog box
            var dlg = new OpenFileDialog
                {
                    Multiselect = true,
                    Filter = "All Files (.*)|*.*",
                    InitialDirectory = _settingsManager.LastDirectoryOpened
                };

            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true && dlg.FileNames.Any())
            {
                _settingsManager.LastDirectoryOpened = Path.GetDirectoryName(dlg.FileName);

                var vms = dlg.FileNames
                             .Select(x =>
                                 {
                                     var vm = ServiceLocator.Current.GetInstance<FileViewModel>();
                                     vm.Load(x);
                                     return vm;
                                 });
                ModifyFileResults = new ObservableCollection<FileViewModel>(vms);
            }
            else
            {
                ModifyFileResults = new ObservableCollection<FileViewModel>();
            }
        }

        private void SelectBaseTvPath()
        {
            var dlg = new FolderBrowserDialog()
                {
                    ShowNewFolderButton = true,
                    SelectedPath = BaseTvPath
                };

            var result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                BaseTvPath = dlg.SelectedPath;
            }
        }

        private void MoveAndRenameFiles()
        {
            ModifyFileResults.ForEach(x => x.MoveAndRename());
        }

        #endregion
    }
}