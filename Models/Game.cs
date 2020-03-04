using System;

namespace HighscoreTerminal.Models
{
    class Game
    {
        public Game(string title, string description, Uri imageUrl)
        {
            Title = title;
            Description = description;
            ImageUrl = imageUrl;
        }

        public Game(int id, string title, string description, Uri imageUrl)
            : this(title, description, imageUrl)
        {
            Id = id;
        }

        public Game()
        {

        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Uri ImageUrl { get; set; }
    }
}
