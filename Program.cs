using System;
using System.Net.Http;
using static System.Console;
using System.Net.Http.Headers;
using HighscoreTerminal.Models;
using Newtonsoft.Json;
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
                        WriteLine("4. Update game (PUT)");

                        keyPressed = ReadKey(true);

                        Clear();

                        switch (keyPressed.Key)
                        {
                            case ConsoleKey.D1:

                                ListGames();

                                break;

                            case ConsoleKey.D2:

                                ShowAddGame();

                                break;

                            case ConsoleKey.D3:

                                DeleteGame();

                                break;

                            case ConsoleKey.D4:

                                UpdateGameUsingPUT();

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

        private static void ListGames()
        {
            throw new NotImplementedException();
        }

        private static void UpdateGameUsingPUT()
        {

            Write("Select game (ID): ");
            var gameId = ReadLine();

            Clear();

            var response = httpClient.GetAsync($"/api/games/{gameId}").Result;

            if (!response.IsSuccessStatusCode)
            {
                WriteLine("Game not found");
                Thread.Sleep(2000);
                return;
            }
            else
            {
                var stringContent = response.Content.ReadAsStringAsync().Result;  

                // lägg till tom ctor
                var game = JsonConvert.DeserializeObject<Game>(stringContent);

                WriteLine("ID: " + game.Id);
                WriteLine("Title: " + game.Title);
                WriteLine("Description: " + game.Description);
                WriteLine("Image URL: " + game.ImageUrl);
                WriteLine("-------------------------------------------------------------");

                WriteLine("ID: " + game.Id);

                Write("Title: ");
                var title = ReadLine();

                Write("Description: ");
                var description = ReadLine();

                Write("Image URL: ");
                var imageUrl = new Uri(ReadLine());

                bool Exit = false;

                while (!Exit)
                {
                    WriteLine();

                    WriteLine("Is this correct? (Y)es or (N)o \n(L)eave without saving");

                    ConsoleKeyInfo menuChoice = ReadKey(true);

                    Clear();

                    switch (menuChoice.Key)
                    {
                        case ConsoleKey.Y:

                            // glöm inte att göra en ny Game constructor som tar ID!
                            // vi sätter aldrig Id sjäv men måste ha med det
                            var updatedGame = new Game(game.Id, title, description, imageUrl);

                            var serializedUpdatedGame = JsonConvert.SerializeObject(updatedGame);

                            var content = new StringContent(serializedUpdatedGame, Encoding.UTF8, "application/json");

                            response = httpClient.PutAsync($"/api/games/{game.Id}", content).Result;

                            Clear();

                            if (response.IsSuccessStatusCode)
                            {
                                WriteLine("Game updated");
                            }
                            else
                            {
                                WriteLine("Failed to update game");
                            }

                            Thread.Sleep(2000);

                            Exit = true;

                            break;

                        case ConsoleKey.N:
                            break;

                        case ConsoleKey.L:
                            Exit = true;
                            break;

                        default:
                            break;
                    }
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

        private static void ShowAddGame()
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

            var response = httpClient.PostAsync("games", data).Result; // skicka till OnPost metoden i Controllern

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
