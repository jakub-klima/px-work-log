using System;
using System.ComponentModel;

namespace PxWorkLog.Core.Utils
{
    public class ProjectedObservableValue<TValue> : INotifyPropertyChanged
    {
        private readonly Func<TValue> projector;

        private TValue value;

        public TValue Value
        {
            get => value;
            private set
            {
                this.value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ProjectedObservableValue(Func<TValue> projector)
        {
            this.projector = projector;
            value = this.projector();
        }

        public void Refresh(object sender, EventArgs e)
        {
            Value = projector();
        }
    }
}
