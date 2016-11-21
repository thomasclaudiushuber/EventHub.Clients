using System;
using Microsoft.ServiceBus.Messaging;
using ThomasClaudiusHuber.EventHub.Common.Model;

namespace ThomasClaudiusHuber.EventHub.Receiver.DataAccess
{
  public class SensorDataReceivedEventArgs : EventArgs
  {
    public SensorDataReceivedEventArgs(SensorData sensorData, EventData data)
    {
      SensorData = sensorData;
      PartitionKey = data.PartitionKey;
    }

    public string PartitionKey { get; set; }

    public SensorData SensorData { get; set; }
  }
}
