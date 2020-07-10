using Newtonsoft.Json;
using nodeset2json.Serializer;
using Opc.Ua;
using Opc.Ua.Configuration;
using Opc.Ua.Export;
using Opc.Ua.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace nodeset2json
{
    class Program
    {
        #region Global Variables

        private static readonly NodesetServer _server = new NodesetServer("");

        private static readonly ApplicationInstance _application = new ApplicationInstance();

        #endregion
        static int Main(string[] args)
        {
            _application.ApplicationName = "Nodeset2Json";
            _application.ApplicationType = ApplicationType.Server;

            try
            {
                //check if argument has been passed
                if (args.Length < 2)
                {
                    Console.WriteLine("usage: nodeset2json.exe [dir]nodeset_file_xml [dir]output_file");
                    return -1;
                }
                if (args[0].EndsWith(".xml"))
                {
                    _server.NodesetFilePath = args[0]; //Input file
                    
                    //Load configuration from xml file "NodesetServer.Config.xml"
                    string configFilePath = Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "NodesetServer.Config.xml");
                    _application.LoadApplicationConfiguration(configFilePath, false).Wait();

                    // check the application certificate.
                    // here the opc foundation certificates are used
                    bool certOk = _application.CheckApplicationInstanceCertificate(false, 0).Result;
                    if (!certOk)
                    {
                        throw new Exception("Application instance certificate invalid!");
                    }

                    // Start the server
                    _application.Start(_server).GetAwaiter().OnCompleted(OnServerStarted);

                    var nsm = _server.NodeManager;
                    var ns = nsm.NodeSet; //BINGO: I got myself a nodeset!

                    RecursiveNode tree = new RecursiveNode(ns); //get the nodeset as a non binary tree

                    //the money shot
                    using (StreamWriter file = File.CreateText(args[1])) //Output file
                    {
                        var settings = new JsonSerializerSettings
                        {
                            ContractResolver = NodesetContractResolver.Instance,
                            NullValueHandling = NullValueHandling.Ignore
                        };

                        var json = JsonConvert.SerializeObject(tree, Formatting.Indented, settings);
                        file.Write(json);
                    }
                    return 0;

                }
                else
                {                    
                    throw new Exception("Invalid nodeset xml file!");
                }
            }
            catch(Exception e)
            {
                //TODO Implement logging sink instead of console.writeline
                Console.WriteLine($"{e.Message} \n {e.InnerException?.Message} \n {e.StackTrace}");
                return -1;
            }
        }


        #region Events

        /// <summary>
        /// This is executed when the Server finished starting
        /// </summary>
        private static void OnServerStarted()
        {
            // Add events for Sessions
            _server.CurrentInstance.SessionManager.SessionActivated += OnSessionActivated;
            _server.CurrentInstance.SessionManager.SessionClosing += OnSessionClosing;

            // Add events for Subscriptions
            _server.CurrentInstance.SubscriptionManager.SubscriptionCreated += OnSubscriptionCreated;
            _server.CurrentInstance.SubscriptionManager.SubscriptionDeleted += OnSubscriptionDeleted;

            Console.WriteLine("*****************************************************************");
            Console.WriteLine("Server started successfully");
            Console.WriteLine("Server URI: " + _application.ApplicationConfiguration.ServerConfiguration.BaseAddresses.First());
            Console.WriteLine("*****************************************************************");
        }

        private static void OnSessionActivated(Session session, SessionEventReason reason)
        {
            string sessionId = $"ns = {session.Id.NamespaceIndex}; i = {session.Id.Identifier} ";
            string name = session.SessionDiagnostics.SessionName;
            string user = session.Identity.DisplayName;
            DateTime lastContact = session.SessionDiagnostics.ClientLastContactTime;

            Console.WriteLine("***************** Session Activated *****************************");
            Console.WriteLine("SessionId: " + sessionId);
            Console.WriteLine("Name: " + name);
            Console.WriteLine("User: " + user);
            Console.WriteLine("LastContact: " + lastContact);
            Console.WriteLine("*****************************************************************");
        }

        private static void OnSessionClosing(Session session, SessionEventReason reason)
        {
            string sessionId = $"ns = {session.Id.NamespaceIndex}; i = {session.Id.Identifier} ";
            string name = session.SessionDiagnostics.SessionName;
            string user = session.Identity.DisplayName;
            DateTime lastContact = session.SessionDiagnostics.ClientLastContactTime;

            Console.WriteLine("***************** Session Closed ********************************");
            Console.WriteLine("SessionId: " + sessionId);
            Console.WriteLine("Name: " + name);
            Console.WriteLine("User: " + user);
            Console.WriteLine("LastContact: " + lastContact);
            Console.WriteLine("*****************************************************************");
        }

        private static void OnSubscriptionCreated(Subscription subscription, bool deleted)
        {
            uint subscriptionId = subscription.Diagnostics.SubscriptionId;
            double publishingInterval = subscription.PublishingInterval;
            string itemCount = subscription.MonitoredItemCount.ToString();
            string nextSequenceNumber = subscription.Diagnostics.NextSequenceNumber.ToString();

            Console.WriteLine("***************** Subscription Created **************************");
            Console.WriteLine("SubscriptionId: " + subscriptionId);
            Console.WriteLine("PublishingInterval: " + publishingInterval);
            Console.WriteLine("ItemCount: " + itemCount);
            Console.WriteLine("NextSequenceNumber: " + nextSequenceNumber);
            Console.WriteLine("*****************************************************************");
        }

        private static void OnSubscriptionDeleted(Subscription subscription, bool deleted)
        {
            uint subscriptionId = subscription.Diagnostics.SubscriptionId;
            double publishingInterval = subscription.PublishingInterval;
            string itemCount = subscription.MonitoredItemCount.ToString();
            string nextSequenceNumber = subscription.Diagnostics.NextSequenceNumber.ToString();

            Console.WriteLine("***************** Subscription Closed ***************************");
            Console.WriteLine("SubscriptionId: " + subscriptionId);
            Console.WriteLine("PublishingInterval: " + publishingInterval);
            Console.WriteLine("ItemCount: " + itemCount);
            Console.WriteLine("NextSequenceNumber: " + nextSequenceNumber);
            Console.WriteLine("*****************************************************************");
        }

        #endregion

    }
}
