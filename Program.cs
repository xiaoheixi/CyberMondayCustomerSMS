using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MessageMedia.Messages;
using MessageMedia.Messages.Controllers;
using MessageMedia.Messages.Exceptions;
using MessageMedia.Messages.Models;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            String textFile = @"C:\Users\Justin Zhao\Documents\CyberMondayCustomersTest.csv";
            String[] lines = File.ReadAllLines(textFile);
            var count = 1;
            if (File.Exists(textFile))
            {
                String API_KEY = "9uxSgUOcEtLuCyLWV6vy";
                String API_SECRET = "xmSBziqxBPbIje2H4Z6BYv6GbLqZkC";
                Boolean HMAC = false;
                MessageMediaMessagesClient client = new MessageMediaMessagesClient(API_KEY, API_SECRET, HMAC);

                MessagesController messages = client.Messages;

                SendMessagesRequest body = new SendMessagesRequest();
                body.Messages = new List<Message>();
                var regex = new Regex(Regex.Escape("0"));
                foreach (string x in lines)
                {
                    if (count != 1)
                    {
                        Message body_messages_0 = new Message();
                        body_messages_0.Content = x.Split(',')[4].TrimStart().Replace("'", "") + " is offering 5% off all orders placed through YQme on Cyber Monday. Place an order using the following link " + x.Split(',')[5].TrimStart().Replace("'", "") + " and " + x.Split(',')[4].TrimStart().Replace("'", "") + " will give you 5% off your order. \nRegards,\n" + x.Split(',')[4].TrimStart().Replace("'", "");
                        body_messages_0.DestinationNumber = regex.Replace(x.Split(',')[3].TrimStart().Replace("'", ""), "+61", 1);
                        body.Messages.Add(body_messages_0);
                        Console.WriteLine(body_messages_0.Content);
                        Console.WriteLine(body_messages_0.DestinationNumber);
                        int realCount = count - 1;
                        try
                        {
                            SendMessagesResponse result = messages.SendMessagesAsync(body).Result;
                            Console.WriteLine(result);
                            Console.WriteLine("Message " + realCount + " sent!");
                        }
                        catch (APIException e)
                        {
                            Console.WriteLine(e.Message + e.ResponseCode + e.HttpContext.ToString());
                            Console.WriteLine("Message " + realCount + " failed!");
                        }
                    }
                    count++;
                }
            }
        }
    }
}