using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PrintAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PrintAPI.Middleware
{
    public class WebSocketServerMiddleware
    {
        private readonly RequestDelegate _next;
        public WebSocketServerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // This methid will keep the connection open.
        public async Task InvokeAsync(HttpContext context)
        {
            // Checking if it's a WebSocket Request
            if(context.WebSockets.IsWebSocketRequest)
            {
                // For testing purposes only
                //Console.WriteLine("Websocket request => 1st request delegate");

                // If so, create a WebSocket object and WebSocket connection is going to be established.
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("");
                Console.WriteLine(">>> Agent Connected <<<");

                // user input
                userInput:
                string userAnswer = UserInput();

                // ---------------------------------------------------------------------
 
                // String to be sent to the printer agent
                string stringToBeSent = "";


                // User want to send a printJob to the printer
                if (userAnswer.ToLower() == "1")
                {
                    stringToBeSent = "print";
                }
                // User Want to get SNMP info from the printer
                else if(userAnswer.ToLower() == "2")
                {
                    stringToBeSent = "getinfo";
                }
                // Invalid input from user.
                else
                {
                    Console.WriteLine("Error.");
                    Thread.Sleep(4 * 1000);
                    goto userInput;
                }

                // Send printJobString to the client.
                await SendToAgent(webSocket, stringToBeSent);

                // Go back to the user input
                Thread.Sleep(4 * 1000);
                goto userInput;
            }
            // It's not websocket request and it will continue to the next requested delegate (middleware) in the pipeline.
            else
            {
                // For testing purposes only
                //Console.WriteLine("Http request => 2nd request delegate.");

                await _next(context);
            }
        }

        // Send a "ping" signal to the client, so that agent can do appropriate api call.
        private async Task SendToAgent(WebSocket socket, string stringToBeSent)
        {
            // Convert string to binary
            var buffer = Encoding.UTF8.GetBytes(stringToBeSent);

            // Send a message to the agent (client).
            await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// Allows user to choose one of the available options.
        /// </summary>
        /// <returns>user input</returns>
        private static string UserInput()
        {
            string output = "";
            Console.WriteLine("");
            Console.WriteLine("What you want to do?");
            Console.WriteLine("1) -> send a print job to the Agent");
            Console.WriteLine("2) -> get SNMP info about the printer");
            Console.WriteLine("");
            Console.Write("Your Answer: ");
            output = Console.ReadLine();

            return output;
        }
    }
}
