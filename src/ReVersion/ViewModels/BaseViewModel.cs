using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReVersion.Commands;
using ReVersion.Models;

namespace ReVersion.ViewModels
{
    public abstract class BaseViewModel<TModel> : INotifyPropertyChanged
        where TModel : IModel
    {
        protected BaseViewModel()
        {
        }
        

        private TModel model;
        public TModel Model
        {
            get { return model; }
            set { SetField(ref model, value); }
        }

        #region Property Helpers

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        #region Command Helpers

        protected RelayCommand CommandFromFunction(Action<object> func)
        {
            return new RelayCommand(func);
        }
        
        #endregion
    }
}
