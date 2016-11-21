using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using ThomasClaudiusHuber.EventHub.Common.Model;

namespace ThomasClaudiusHuber.EventHub.Sender.DataAccess
{
  public class EventPublisher : IEventPublisher
  {
    public async Task<PublishResult> PublishAsync(string connectionString, SensorData sensorData)
    {
      try
      {
        EventHubClient client = EventHubClient.CreateFromConnectionString(connectionString);

        string sensorDataJson = JsonConvert.SerializeObject(sensorData);

        var eventData = new EventData(Encoding.UTF8.GetBytes(sensorDataJson));

        // NOTE: For multipe events there's also a SendBatchAsync-event
        await client.SendAsync(eventData);
        
        return new PublishResult { IsSuccess = true };
      }
      catch(Exception ex)
      {
        return new PublishResult { Error = ex.Message };
      }
    }
  }

  public class PublishResult
  {
    public bool IsSuccess { get; set; }
    public string Error { get; set; }
  }
}
