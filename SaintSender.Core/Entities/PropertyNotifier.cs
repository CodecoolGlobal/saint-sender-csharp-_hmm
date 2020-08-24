using System;
using System.ComponentModel;

namespace SaintSender.Core.Entities
{
    /// <summary>
    /// Basic Notifier to update FrontEnd
    /// </summary>
    public abstract class PropertyNotifier : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyNotifier"/> class.
        /// Basic Constructor
        /// </summary>
        public PropertyNotifier()
            : base()
        {
        }

        /// <summary>
        /// Event
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// On firing Event
        /// </summary>
        /// <param name="propertyName">What we invoke/change</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
