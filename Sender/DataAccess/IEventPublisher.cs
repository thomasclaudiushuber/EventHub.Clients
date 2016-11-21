using System.Threading.Tasks;
using ThomasClaudiusHuber.EventHub.Common.Model;

namespace ThomasClaudiusHuber.EventHub.Sender.DataAccess
{
  public interface IEventPublisher
  {
    Task<PublishResult> PublishAsync(string connectionString, SensorData sensorData);
  }
}
