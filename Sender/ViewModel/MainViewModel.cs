using System;
using System.Windows.Threading;
using ThomasClaudiusHuber.EventHub.Common.Model;
using ThomasClaudiusHuber.EventHub.Sender.DataAccess;

namespace ThomasClaudiusHuber.EventHub.Sender.ViewModel
{
  public class MainViewModel : ViewModelBase
  {
    private DispatcherTimer _timer;
    private double _humidity;
    private double _temperature;
    private bool _isSendingDataEnabled;
    private DateTime? _lastSendTime;
    private IEventPublisher _eventPublisher;
    private string _lastTempSendStatus;
    private string _connectionString;
    private string _lastHumSendStatus;
    private string _deviceName;

    public MainViewModel(IEventPublisher eventPublisher)
    {
      _eventPublisher = eventPublisher;
      _timer = new DispatcherTimer();
      _timer.Interval = TimeSpan.FromSeconds(2);
      _timer.Tick += OnTimerTick;
      _timer.Start();
    }

    public double Humidity
    {
      get { return _humidity; }
      set
      {
        _humidity = value;
        OnPropertyChanged();
      }
    }

    public double Temperature
    {
      get { return _temperature; }
      set
      {
        _temperature = value;
        OnPropertyChanged();
      }
    }

    public bool IsSendingDataEnabled
    {
      get { return _isSendingDataEnabled; }
      set
      {
        _isSendingDataEnabled = value;
        OnPropertyChanged();
      }
    }


    public DateTime? LastSendTime
    {
      get { return _lastSendTime; }
      private set
      {
        _lastSendTime = value;
        OnPropertyChanged();
      }
    }

    public string LastTempSendStatus
    {
      get { return _lastTempSendStatus; }
      private set
      {
        _lastTempSendStatus = value;
        OnPropertyChanged();
      }
    }

    public string LastHumSendStatus
    {
      get { return _lastHumSendStatus; }
      set
      {
        _lastHumSendStatus = value;
        OnPropertyChanged();
      }
    }

    public string DeviceName
    {
      get { return _deviceName; }
      set
      {
        _deviceName = value;
        OnPropertyChanged();
      }
    }

    public string ConnectionString
    {
      get { return _connectionString; }
      set
      {
        _connectionString = value;
        OnPropertyChanged();
      }
    }


    private async void OnTimerTick(object sender, EventArgs e)
    {
      if (!_isSendingDataEnabled)
      {
        return;
      }
      

      var resultTemp = await _eventPublisher.PublishAsync(
           ConnectionString,
           new SensorData
           {
             DeviceName = DeviceName,
             ReadTime = DateTime.Now,
             SensorType = "Temp",
             Value = Temperature
           });
      LastTempSendStatus = resultTemp.IsSuccess ? "Success" : "Error: " + resultTemp.Error;

      var resultHum = await _eventPublisher.PublishAsync(
           ConnectionString,
           new SensorData
           {
             DeviceName = DeviceName,
             ReadTime = DateTime.Now,
             SensorType = "Hum",
             Value = Humidity
           });
      LastHumSendStatus = resultHum.IsSuccess ? "Success" : "Error: " + resultHum.Error;

      LastSendTime = DateTime.Now;
    }
  }
}
