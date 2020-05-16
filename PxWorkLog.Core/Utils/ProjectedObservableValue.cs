using System;
using System.ComponentModel;

namespace PxWorkLog.Core.Utils
{
    public class ProjectedObservableValue<T> : IReadOnlyObservableValue<T>
    {
        private readonly Func<T> projector;

        private T value;

        public T Value
        {
            get => value;
            private set
            {
                this.value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ProjectedObservableValue(Func<T> projector)
        {
            this.projector = projector;
            value = this.projector();
        }

        public static ProjectedObservableValue<T> FromObservableValue<TSource>(IReadOnlyObservableValue<TSource> source, Func<TSource, T> projector)
        {
            var result = new ProjectedObservableValue<T>(() => projector(source.Value));
            source.PropertyChanged += result.Refresh;
            return result;
        }

        public void Refresh(object sender, EventArgs e)
        {
            Value = projector();
        }
    }
}
