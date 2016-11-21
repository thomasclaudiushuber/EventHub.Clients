using Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using ThomasClaudiusHuber.EventHub.Common.Model;
using ThomasClaudiusHuber.EventHub.Receiver.DataAccess;

namespace ThomasClaudiusHuber.EventHub.Receiver.ViewModel
{
  public class MainViewModel : ViewModelBase
  {
    private string _connectionString;
    private bool _isReceivingDataEnabled;
    private readonly EventHubSubscriberBase _eventHubSubscriber;
    private static readonly object _lock = new object();
    public MainViewModel(EventHubSubscriberBase eventHubSubscriber)
    {
      _eventHubSubscriber = eventHubSubscriber;
      _eventHubSubscriber.SensorDataReceived += EventHubAccess_SensorDataReceived;
      ReceivedTempSensorDataItems = new ObservableCollection<SensorData>();
      ReceivedHumSensorDataItems = new ObservableCollection<SensorData>();
      BindingOperations.EnableCollectionSynchronization(ReceivedHumSensorDataItems, _lock);
      BindingOperations.EnableCollectionSynchronization(ReceivedTempSensorDataItems, _lock);

      StartReceivingEventsCommand = new DelegateCommand(OnStartReceivingEventsExecute, OnStartReceivingEventsCanExecute);
    }

    public ObservableCollection<SensorData> ReceivedTempSensorDataItems { get; }
    public ObservableCollection<SensorData> ReceivedHumSensorDataItems { get; }

    public ICommand StartReceivingEventsCommand { get; }

    public string ConnectionString
    {
      get { return _connectionString; }
      set
      {
        _connectionString = value;
        OnPropertyChanged();
      }
    }

    private bool _isStarting;
    private bool OnStartReceivingEventsCanExecute()
    {
      return !_isStarting;
    }

    private void OnStartReceivingEventsExecute()
    {
      _isStarting = true;
      ((DelegateCommand)StartReceivingEventsCommand).RaiseCanExecuteChanged();
      try
      {
        _eventHubSubscriber.SubscribeToEvents(ConnectionString);
      }
      finally
      {
        _isStarting = false;
        ((DelegateCommand)StartReceivingEventsCommand).RaiseCanExecuteChanged();
      }
    }

    private void EventHubAccess_SensorDataReceived(object sender, SensorData e)
    {
      lock (_lock)
      {
        if (e.SensorType == "Temp")
        {
          ReceivedTempSensorDataItems.Insert(0, e);
        }
        else
        {
          ReceivedHumSensorDataItems.Insert(0, e);
        }
      }
    }
  }
}
