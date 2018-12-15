using System;
using System.Collections.Generic;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Grpc.Core;

namespace PubSub
{
	public class PubSubConnector
	{
		private const string ProjectId = "projectcloudlearning";
		private const string TopicId = "PubTradeSub";
		private const string SubscriptionId = "pull";

		// To view a message in your subscription:
		// gcloud beta pubsub subscriptions pull testsub1

		// public static void Main(string[] args)
		// {
        //     string value = Environment.GetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS");
        //     Console.WriteLine("hello"+value);
		// 	PublishToTopic();
		// 	PublishToTopic();

		// 	// The loop is necessary as pull doesn't always brings back all your messages
		// 	var response = ShowMessagesForSubscription();
		// 	while (response != null && response.ReceivedMessages.Count > 0)
		// 	{
		// 		response = ShowMessagesForSubscription();
		// 	}
		// }

		private static void CreateTopic()
		{
			// The fully qualified name for the new topic
			var topicName = new TopicName(ProjectId, TopicId);

			// Creates the new CreateTopic
			var publisher = PublisherServiceApiClient.Create();
			Topic topic = publisher.CreateTopic(topicName);
			Console.WriteLine($"Topic {topic.Name} created.");
		}

		private static void ListTopics()
		{
			// List all topics for the project
			// var publisher = PublisherServiceApiClient.Create();
			
			// var topics = publisher.ListTopics(new ProjectName(ProjectId));
			// foreach (Topic topic1 in topics)
			// {
			// 	Console.WriteLine(topic1.Name);
			// }
		}

		public static void PublishToTopic(string trade)
		{
			var topicName = new TopicName(ProjectId, TopicId);
Console.WriteLine("Inside PublishToTopic..." +ProjectId +TopicId);
			var publisher = PublisherServiceApiClient.Create();

			// Create a message
			var message = new PubsubMessage()
			{
				Data = ByteString.CopyFromUtf8(trade)
			};
			message.Attributes.Add("myattrib", "its value");


			// Add it to the list of messages to publish
			var messageList = new List<PubsubMessage>() { message };

PublishResponse response =null;
			// Publish it
			Console.WriteLine("Publishing..." +topicName +messageList);
			try{
			 response = publisher.Publish(topicName, messageList);
			}
			catch(Exception e)
			{
Console.WriteLine(e.Message);

			}
			if(response is null)
			{
Console.WriteLine("response is null");
return;

			}
			// Get the message ids GCloud gave us
			Console.WriteLine("  Message ids published:");
			foreach (string messageId in response.MessageIds)
			{
				Console.WriteLine($"  {messageId}");
			}
		}

		private static PullResponse ShowMessagesForSubscription()
		{
			var subscriptionName = new SubscriptionName(ProjectId, SubscriptionId);

			var subscription = SubscriberServiceApiClient.Create();

			try
			{
				PullResponse response = subscription.Pull(subscriptionName,true,2);

				var all = response.ReceivedMessages;
Console.WriteLine("Inside subscription"+ all.Count);
				foreach (ReceivedMessage message in all)
				{
					string id = message.Message.MessageId;
					string publishDate = message.Message.PublishTime.ToDateTime().ToString("dd-M-yyyy HH:MM:ss");
					string data = message.Message.Data.ToStringUtf8();

					Console.WriteLine($"{id} {publishDate} - {data}");

					Console.Write("  Acknowledging...");
					subscription.Acknowledge(subscriptionName, new string[] { message.AckId });
					Console.WriteLine("done");
				}

				return response;
			}
			catch (RpcException e)
			{
				Console.WriteLine("Something went wrong: {0}", e.Message);
				return null;
			}
		}
	}
}
