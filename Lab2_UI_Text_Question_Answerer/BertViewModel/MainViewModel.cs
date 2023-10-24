using BertModelLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BertViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private string modelWebSource = "https://storage.yandexcloud.net/dotnet4/bert-large-uncased-whole-word-masking-finetuned-squad.onnx111";

        private BertModel bertModel;
        public ObservableCollection<TabItemViewModel> TabItems { get; set; } = new ObservableCollection<TabItemViewModel>();
        public int SelectedTab { get; set; }
        public string DownloadingValue { get; set; } = "....";
        private int tabCount = 0;

        private readonly IErrorSender errorSender;
        private readonly IFileDialog fileDialog;
        public ICommand NewTabCommand { get; private set; }
        public ICommand RemoveTabCommand { get; private set; }

        public MainViewModel(IErrorSender errorSender, IFileDialog fileDialog)
        {
            this.fileDialog = fileDialog;
            this.errorSender = errorSender;
            NewTabCommand = new RelayCommand(o => { NewTabCommandHandler(); });
            RemoveTabCommand = new RelayCommand(o => { RemoveTabCommandHandler(o); });
            getBertModel();
        }

        private async void getBertModel()
        {
            try
            {
                var createTask = BertModel.Create(modelWebSource);
                while (true && !createTask.IsCompleted)
                {
                    lock (BertModel.progressBar)
                    {
                        while (BertModel.progressBar.Count > 0)
                        {
                            DownloadingValue = BertModel.progressBar.Dequeue();
                            RaisePropertyChanged("DownloadingValue");
                        }

                    }
                }
                bertModel = await createTask;
            }
            catch(Exception ex)
            {
                errorSender.SendError("Ошибка:" + ex.Message);
            }
            
        }

        private void NewTabCommandHandler()
        {
            try
            {
                TabItems.Add(new TabItemViewModel(string.Format("Tab {0}", tabCount), bertModel, errorSender, fileDialog));
                SelectedTab = TabItems.Count - 1;
                RaisePropertyChanged("TabItems");
                RaisePropertyChanged("SelectedTab");
                tabCount++;
            }
            catch (Exception ex)
            {
                errorSender.SendError("Ошибка:" + ex.Message);
            }
        }

        private void RemoveTabCommandHandler(object sender)
        {
            try
            {
                TabItemViewModel item = sender as TabItemViewModel;
                string tabName = item.TabName;
                int index = TabItems.IndexOf(item);
                if (SelectedTab == index)
                    SelectedTab = SelectedTab - 1;
                TabItems.RemoveAt(index);
                RaisePropertyChanged("TabItems");
                RaisePropertyChanged("SelectedTab");
            }
            catch (Exception ex)
            {
                errorSender.SendError("Ошибка:" + ex.Message);
            }
        }

    }
}
