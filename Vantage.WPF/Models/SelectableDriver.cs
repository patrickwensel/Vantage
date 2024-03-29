﻿using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Vantage.Common.Models;

namespace Vantage.WPF.Models
{
    public class SelectableDriver : Driver, INotifyPropertyChanged
    {
        private bool _isSelected;

        public bool IsSelected 
        {
            get { return _isSelected; }
            set 
            { 
                _isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }

        public int LessonsCompleted => GroupedAttemptsByLessons != null ? GroupedAttemptsByLessons.Count(x => x.IsComplete) : 0;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
