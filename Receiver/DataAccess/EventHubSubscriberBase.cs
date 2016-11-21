using System;
using ThomasClaudiusHuber.EventHub.Common.Model;

namespace ThomasClaudiusHuber.EventHub.Receiver.DataAccess
{
  public abstract class EventHubSubscriberBase
  {
    public event EventHandler<SensorData> SensorDataReceived;

    public abstract void SubscribeToEvents(string connectionString);

    protected virtual void OnSensorDataReceived(SensorData data)
    {
      SensorDataReceived?.Invoke(this, data);
    }
  }
}
