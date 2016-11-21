using System;

namespace ThomasClaudiusHuber.EventHub.Common.Model
{
  public class SensorData
  {
    public string DeviceName { get; set; }
    public DateTime ReadTime { get; set; }
    public string SensorType { get; set; }
    public double Value { get; set; }
  }
}
