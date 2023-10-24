using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BertViewModel
{
    public class TabItemViewModel : BaseViewModel
    {
        public string TabName { get; set; }

        public String TextFromFile { get; set; } = "";
        public String Question { get; set; } = "";
        public String Answer { get; set; } = "....";
        private readonly IErrorSender errorSender;
        private readonly IFileDialog fileDialog;

        public ICommand LoadTextFileCommand { get; private set; }
        public ICommand GetAnswerCommand { get; private set; }
        public TabItemViewModel(string tabName, IErrorSender errorSender, IFileDialog fileDialog)
        {
            this.TabName = tabName;
            this.fileDialog = fileDialog;
            this.errorSender = errorSender;
            LoadTextFileCommand = new RelayCommand(_ => { LoadTextFileCommandHandler();});
            //GetAnswerCommand = new AsyncRelayCommand(o => { ComputeSplineCommandHandler(); }, o => CanComputeSplineCommandHandler());
        }

        private void LoadTextFileCommandHandler()
        {
            try
            {
                string filename = fileDialog.OpenFileDialog();
                if (!string.IsNullOrEmpty(filename))
                {
                    LoadFile(filename);
                    RaisePropertyChanged("TextFromFile");
                }
            }
            catch (Exception ex)
            {
                errorSender.SendError("Ошибка:" + ex.Message);
            }
        }

        private void LoadFile(string filename)
        {
            try
            {
                TextFromFile = GetTextFromFile(path: filename);
            }
            catch (Exception)
            {
                throw;
            }
        }

        static string GetTextFromFile(string path)
        {
            StreamReader? reader = null;
            try
            {
                reader = new StreamReader(path);
                string text = reader.ReadToEnd();
                return text;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }
        }

    }
}
