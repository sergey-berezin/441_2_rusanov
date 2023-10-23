using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BertViewModel
{
    public class MainViewModel: BaseViewModel
    {

        public String textFromFile { get; set; } = "";
        public String userQuestion = "....";

        private readonly IErrorSender errorSender;
        private readonly IFileDialog fileDialog;
        public ICommand LoadTextFileCommand { get; private set; }
        public ICommand GetAnswerCommand { get; private set; }

        public MainViewModel(IErrorSender errorSender, IFileDialog fileDialog)
        {

            this.errorSender = errorSender;
            this.fileDialog = fileDialog;
            LoadTextFileCommand = new AsyncRelayCommand(_ => { LoadTextFileCommandHandler(); return Task.CompletedTask; });
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
                    RaisePropertyChanged("textFromFile");
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
                textFromFile = GetTextFromFile(path: filename);
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
