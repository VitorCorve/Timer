using System;
using System.Windows;
using System.Windows.Controls;

using Timer.ViewModel;

namespace Timer.Internal.Models
{
    public class MessengerEntity
    {
        public PropertyChangedAbstract ViewModel { get; private set; }
        public Window Window { get; private set; }
        public UserControl UserControl { get; private set; }
        public Guid ID { get; private set; }
        public string Name { get; private set; }
        public MessengerEntity(PropertyChangedAbstract viewModel, UserControl userControl, Guid guid, string entityName)
        {
            ViewModel = viewModel;
            UserControl = userControl;
            ID = guid;
            Name = entityName;
        }
        public MessengerEntity(PropertyChangedAbstract viewModel, Window window, Guid guid, string entityName)
        {
            ViewModel = viewModel;
            Window = window;
            ID = guid;
            Name = entityName;
        }
        public MessengerEntity(PropertyChangedAbstract viewModel, string entityName)
        {
            ViewModel = viewModel;
            Name = entityName;
            ID = Guid.NewGuid();
        }
    }
}
