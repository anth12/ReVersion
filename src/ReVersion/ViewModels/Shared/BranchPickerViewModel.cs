using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using ReVersion.Models.Shared;
using ReVersion.Views.Shared;

namespace ReVersion.ViewModels.Shared
{
    internal class BranchPickerViewModel : BaseViewModel<BranchPickerModel>
    {
        public BranchPickerViewModel(BranchPicker control, List<string> branches, Action<string> checkoutAction)
        {
            Control = control;

            Model = new BranchPickerModel
            {
                Branches = branches,
                SelectedBranch = branches.First()
            };

            CheckoutCommand = CommandFromFunction(c=>
            {
                Close();
                checkoutAction(Model.SelectedBranch);
            });

            CloseCommand = CommandFromFunction(c=> Close());

        }

        private readonly BranchPicker Control;
        
        #region Commands
        public ICommand CheckoutCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        #endregion
        
        #region Events

        private void Close()
        {
            ((MetroWindow)Application.Current.MainWindow).HideMetroDialogAsync(
                    Control
                );
        }

        #endregion

    }
}
