/*  
Author: Elijah Weatherwax 
Start Date: 10/04/2025 
Name: #anged Man 
This is a application designed to help users learn C# terminology and concepts through a classic game of hangman.
*/
using System;
using System.Collections.Generic;
using System.Linq;

internal class Hangman
{
    private static void Main(string[] args)
    {
        var words = new[]
        {
            "abstraction",
            "encapsulation",
            "inheritance",
            "polymorphism",
            "gui",
            "method",
            "dotnet",
            "event",
            "console",
            "syntax"
        };

        Console.WriteLine("Select a difficulty level: Easy, Medium, Hard");
        var DifficultySelect = (Console.ReadLine() ?? string.Empty).Trim().ToLower();
        int lives = DifficultySelect switch
        {
            "easy" => 5,
            "medium" => 3,
            "hard" => 1,
            _ =>3
        };

        var rand = new Random();
        var ChosenWord = words[rand.Next(words.Length)];

        var letters = new List<string>();
        while (lives != 0)
        {
            int charaleft = 0;
            foreach (var character in ChosenWord)
            {
                var letter = character.ToString();
                if (letters.Contains(letter))
                {
                    Console.Write(letter);
                }
                else
                {
                    Console.Write("_");
                    charaleft++;
                }
                Console.WriteLine(string.Empty);
                if (charaleft == 0)
                {
                    break;
                }

                Console.Write("Enter a letter:_");

                var key = Console.ReadKey().Key.ToString().ToLower();
                if (!ChosenWord.Contains(key))
                {
                    lives--; 
                    if (lives > 0)
                    {
                        Console.WriteLine($"The letter {key} is not in the word. {lives} lives remaining.");
                    } 
                }
            }
        }
    
        string result = lives > 0 ? $"You won with {lives} lives remaining!" : "You lost! The word was: ";
        Console.WriteLine(result);
    }
}
