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
            String API_KEY = "9uxSgUOcEtLuCyLWV6vy";
            String API_SECRET = "xmSBziqxBPbIje2H4Z6BYv6GbLqZkC";
            Boolean HMAC = false;
            MessageMediaMessagesClient client = new MessageMediaMessagesClient(API_KEY, API_SECRET, HMAC);
            MessagesController messages = client.Messages;
            SendMessagesRequest body = new SendMessagesRequest();
            body.Messages = new List<Message>();
            var count = 1;
            var regex = new Regex(Regex.Escape("0"));
            foreach (string x in lines)
            {
                Message body_messages_0 = new Message();
                body_messages_0.Content = "Hi " + x.Split(',')[1].TrimStart().Replace("'", "") + ",\n\n" + x.Split(',')[4].TrimStart().Replace("'", "") + " is offering 5% off all orders placed through YQme on Cyber Monday. \n\nOrder now: " + x.Split(',')[5].TrimStart().Replace("'", "") + "\n\nEnjoy!\n" + x.Split(',')[4].TrimStart().Replace("'", "");
                /*if (x.Split(',')[3].TrimStart().Replace("'", "").Length != 10)
                {
                    var tempFile = Path.GetTempFileName();
                    var linesToKeep = File.ReadLines(textFile).Where(l => l != x);
                    File.WriteAllLines(tempFile, linesToKeep);
                    File.Delete(textFile);
                    File.Move(tempFile, textFile);
                    continue;
                }*/
                if (x.Split(',')[3].TrimStart().Replace("'", "").Contains("+61"))
                {
                    body_messages_0.DestinationNumber = x.Split(',')[3].TrimStart().Replace("'", "");
                }
                else if (Regex.IsMatch(x.Split(',')[3].TrimStart().Replace("'", ""), "^614[0-9]{8}$"))
                {
                    body_messages_0.DestinationNumber = "+" + x.Split(',')[3].TrimStart().Replace("'", "");
                }
                else
                {
                    body_messages_0.DestinationNumber = regex.Replace(x.Split(',')[3].TrimStart().Replace("'", ""), "+61", 1);
                }
                if (Regex.IsMatch(body_messages_0.DestinationNumber, "^\\+614[0-9]{8}$") == false)
                {
                    var tempFile = Path.GetTempFileName();
                    var linesToKeep = File.ReadLines(textFile).Where(l => l != x);
                    File.WriteAllLines(tempFile, linesToKeep);
                    File.Delete(textFile);
                    File.Move(tempFile, textFile);
                    continue;
                }
                body.Messages.Add(body_messages_0);
                try
                {
                    SendMessagesResponse result = messages.SendMessagesAsync(body).Result;
                    Console.WriteLine(result);
                    var tempFile = Path.GetTempFileName();
                    var linesToKeep = File.ReadLines(textFile).Where(l => l != x);
                    File.WriteAllLines(tempFile, linesToKeep);
                    File.Delete(textFile);
                    File.Move(tempFile, textFile);
                    Console.WriteLine("SMS " + count + " sent successfully!");
                }
                catch (APIException e)
                {
                    Console.WriteLine(e.Message + e.ResponseCode + e.HttpContext.ToString());
                    Console.WriteLine("SMS " + count + " failed!");
                }
                body.Messages.Remove(body_messages_0);
                count++;
            }
        }
    }
}