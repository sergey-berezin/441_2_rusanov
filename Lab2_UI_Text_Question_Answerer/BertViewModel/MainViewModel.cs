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
    public class MainViewModel: BaseViewModel
    {
        public ObservableCollection<TabItemViewModel> TabItems { get; set; } = new ObservableCollection<TabItemViewModel>();
        public int SelectedTab { get; set; }

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
            //GetAnswerCommand = new AsyncRelayCommand(o => { ComputeSplineCommandHandler(); }, o => CanComputeSplineCommandHandler());
        }

        private void NewTabCommandHandler()
        {
            try
            {
                TabItems.Add(new TabItemViewModel(string.Format("Tab {0}", tabCount), errorSender, fileDialog));
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
