using System.ComponentModel;

namespace PxWorkLog.Core.Utils
{
    public class ObservableValue<T> : IReadOnlyObservableValue<T>
    {
        private T value;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableValue(T value = default)
        {
            this.value = value;
        }
    }
}
