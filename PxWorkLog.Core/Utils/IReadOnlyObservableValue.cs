using System.ComponentModel;

namespace PxWorkLog.Core.Utils
{
    public interface IReadOnlyObservableValue<T> : INotifyPropertyChanged
    {
        T Value { get; }
    }
}
