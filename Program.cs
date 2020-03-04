using System;
using System.Net.Http;
using static System.Console;
using System.Net.Http.Headers;
using System.Collections;
using HighscoreTerminal.Models;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HighscoreTerminal
{



//    Install-Package Microsoft.Extensions.Http
//    Install-Package Newtonsoft.Json 
    class Program
    {

        static readonly HttpClient httpClient = new HttpClient();

        static void Main(string[] args)
        {
            // Set headers to:
            // Accept: application/json
            // User-Agent: HighscoreTerminal
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
           // httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
           // Om vi vill acceptera tex XML
            httpClient.DefaultRequestHeaders.Add("User-Agent", "HighscoreTerminal");
            httpClient.BaseAddress = new Uri("https://localhost:5001/api/");

            bool shouldRun = true;

            while (shouldRun)
            {
                Clear();

                WriteLine("1. Games");
                WriteLine("2. Exit");

                ConsoleKeyInfo keyPressed = ReadKey(true);

                Clear();

                switch (keyPressed.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:

                        WriteLine("1. List games");
                        WriteLine("2. Add game");
                        WriteLine("3. Delete game");

                        keyPressed = ReadKey(true);

                        Clear();

                        switch (keyPressed.Key)
                        {
                            case ConsoleKey.D2:

                                ShowAddGameForm();

                                break;

                            case ConsoleKey.D3:

                                DeleteGame();

                                break;
                        }

                        //// Make a HTTP GET /api/games request
                        //var response = httpClient.GetAsync("games")
                        //    .GetAwaiter()
                        //    .GetResult();

                        //var games = Enumerable.Empty<Game>();

                        //if (response.IsSuccessStatusCode)
                        //{
                        //    var stringContent = response.Content.ReadAsStringAsync()
                        //        .GetAwaiter()
                        //        .GetResult();  // som Promises (JS) fast i C#

                        //    games = JsonConvert.DeserializeObject<IEnumerable<Game>>(stringContent);
                        //}

                        //foreach(var game in games)
                        //{
                        //    WriteLine(game.Title);
                        //}

                        ReadKey(true);

                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:

                        shouldRun = false;

                        break;

                    default:
                        break;
                }
            }
        }

        private static void DeleteGame()
        {
            Write("ID: ");
            var gameId = ushort.Parse(ReadLine());

            var response = httpClient.DeleteAsync($"games/{gameId}")
                .GetAwaiter()
                .GetResult(); // Kan även skriva endast .Result()

            Clear();

            if (response.IsSuccessStatusCode)
            {
                WriteLine("Game deleted");
            }
            else
            {
                WriteLine("Failed!");
            }

            Thread.Sleep(2000);
        }

        private static void ShowAddGameForm()
        {
            Write("Title: ");
            var title = ReadLine();

            Write("Description: ");
            var description = ReadLine();

            Write("Image URL: ");
            var imageUrl = new Uri(ReadLine());

            var game = new Game(title, description, imageUrl); // skapa ett game

            var serializedGame = JsonConvert.SerializeObject(game); // gör om till JSON

            var data = new StringContent( // lägg till nödvändig data
                serializedGame,  
                Encoding.UTF8,  
                "application/json");

            var response = httpClient.PostAsync("games", data) // skicka till OnPost metoden i Controllern
                .GetAwaiter()
                .GetResult();

            Clear();

            if (response.IsSuccessStatusCode)
            {
                WriteLine("Game added");
            }
            else
            {
                WriteLine("Failed!");
            }

            Thread.Sleep(2000);

        }
    }
}
