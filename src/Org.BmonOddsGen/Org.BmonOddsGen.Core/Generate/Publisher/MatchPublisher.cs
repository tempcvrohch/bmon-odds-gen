using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Org.BmonOddsGen.Host;
using Org.OpenAPITools.Model;

namespace Org.BmonOddsGen.Core.Generate.Publisher;

public interface IMatchPublisher
{
	public void Publish(MatchDto matchDto);
}

// https://github.com/Kamil-Zakiev/kafka-samples/blob/master/KafkaSamples
public class MatchPublisher : IMatchPublisher
{
	private readonly string kafkaTopic;
	private readonly ILogger<MatchPublisher> _logger;
	private readonly IProducer<string, MatchDto> _producer;

	public MatchPublisher(ILogger<MatchPublisher> logger, IOptions<ConsumerConfig> options, IOptions<EnviromentConfiguration> optionsEnv)
	{
		_producer = new ProducerBuilder<string, MatchDto>(options.Value).Build();
		kafkaTopic = optionsEnv.Value.KAFKA_MATCHES_TOPIC ?? throw new Exception("Required .env variable KAFKA_MATCHES_TOPIC is missing.");
		_logger = logger;
	}

	public void Publish(MatchDto matchDto)
	{
		_producer.Produce(kafkaTopic, new Message<string, MatchDto> { Key = "", Value = matchDto }, DeliveryHandler);
	}

	public void DeliveryHandler(DeliveryReport<string, MatchDto> deliveryReport)
	{
		_logger.LogInformation("Delivery report: {Key}, {UUid}, {Status}", deliveryReport.Key, deliveryReport.Message.Value.Id, deliveryReport.Status.ToString());
	}
}