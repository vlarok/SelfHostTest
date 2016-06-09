using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using SelfHostApi;
using Microsoft.AspNet.SignalR.Client;

namespace SelfHostTest
{
    public class Program
    {
        static string baseAddress = "http://localhost:9000/";
        private static IHubProxy myHub;

        static void Main()
        {
            
            WebApp.Start<OlStartup>(url: baseAddress);
            // Start OWIN host 
            /*  using (WebApp.Start<OlStartup>(url: baseAddress))
              {
                  // Create HttpCient and make a request to api/values 
                  HttpClient client = new HttpClient();

                  var response = client.GetAsync(baseAddress + "api/values").Result;

                  Console.WriteLine(response);
                  Console.WriteLine(response.Content.ReadAsStringAsync().Result);
              }
              */


            var connection = new HubConnection(baseAddress);
            //Proxy hubile põhineb serveris kirjeldatud nimel
             myHub = connection.CreateHubProxy("MyHub");
            connection.Start().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    Console.WriteLine("Tekkis viga ühendamisel:{0}",
                                      task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Ühendatud");
                }

            }).Wait();
            myHub.On<Envelope>("addMessage", param =>
            {
                Console.WriteLine(param.Name + " "+ param.Message+ " serveri sõnum kohal ");
            });


            var fw = new FileSystemWatcher(@"C:\Partcom\VoiceloggerSip\Client");
            fw.Created += fw_Changed;
            fw.EnableRaisingEvents = true;

            while (true) // Lo
            {
                Console.WriteLine("Sisesta :"); // Prompt
                string line = Console.ReadLine(); // Sisestus
                //Objekti kasutus
                //  myHub.Invoke<Message>("DoSomething", new Message(){Title = line}).Wait();
                myHub.Invoke<Envelope>("Send", new Envelope() {Name ="console", Message = line }).Wait();
            }
            





            Console.ReadLine();
        }

        static void fw_Changed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.Name);
            myHub.Invoke<Envelope>("Send", new Envelope() { Name = "New file", Message = e.Name }).Wait();

        }
    }
}
