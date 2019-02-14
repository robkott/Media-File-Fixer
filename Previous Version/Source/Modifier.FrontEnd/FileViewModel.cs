using Interfaces;
using Price.Commons.Desktop;

namespace Modifier.FrontEnd
{
    public class FileViewModel : ViewModel
    {
        private readonly IModifier _modifier;
        private string _sourceFileName;
        private string _destinationFileName;
        private bool? _isTransferSuccessful;

        public FileViewModel(IModifier modifier)
        {
            _modifier = modifier;
        }

        public string SourceFileName
        {
            get { return _sourceFileName; }
            set
            {
                if (value == _sourceFileName) return;
                _sourceFileName = value;
                RaisePropertyChanged(() => SourceFileName);
            }
        }

        public string DestinationFileName
        {
            get { return _destinationFileName; }
            set
            {
                if (value == _destinationFileName) return;
                _destinationFileName = value;
                RaisePropertyChanged(() => DestinationFileName);
            }
        }

        public bool? IsTransferSuccessful
        {
            get { return _isTransferSuccessful; }
            set
            {
                if (value.Equals(_isTransferSuccessful)) return;
                _isTransferSuccessful = value;
                RaisePropertyChanged(() => IsTransferSuccessful);
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if(_errorMessage != value) 
                {
                    _errorMessage = value;
                    RaisePropertyChanged(() => ErrorMessage);
                }
            }
        } 

        public void Load(string fileName)
        {
            SourceFileName = fileName;
        }

        public void MoveAndRename()
        {
            var modifyResults = _modifier.RenameAndMove(SourceFileName);
            DestinationFileName = modifyResults.DestinationFileName;
            IsTransferSuccessful = modifyResults.Success;
            ErrorMessage = modifyResults.Exception != null
                               ? modifyResults.Exception.Message.Trim()
                               : string.Empty;
        }
    }
}