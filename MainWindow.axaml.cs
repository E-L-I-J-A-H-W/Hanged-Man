/*  
Author: Elijah Weatherwax 
Start Date: 10/04/2025 
Name: #anged Man 
This is a application designed to help users learn C# terminology and concepts through a classic game of hangman.
*/
using Avalonia.Controls; 
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FinalH
{
    public partial class MainWindow : Window
    {
        private readonly string[] words = new[]
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

        private readonly string[] definitions = new[]
        {
            "The process of hiding the complex reality while exposing only the necessary parts.",
            "The bundling of data with the methods that operate on that data.",
            "The mechanism of basing an object or class upon another object or class.",
            "The ability of different classes to be treated as instances of the same class through a common interface.",
            "This is a visual way of interacting with a computer using items like windows, icons, and menus.",
            "A block of code that performs a specific task.",
            "Free, cross-platform, open source developer platform for building many different types of applications.",
            "An action or occurrence recognized by software that may be handled by the software.",
            "A text-based interface used to interact with software and operating systems by typing commands.",
            "The set of rules that defines the combinations of symbols that are considered to be correctly structured programs in that language."
        };
        //Default values for game state
        private string chosenWord = string.Empty;
        private string chosenDefinition = string.Empty;
        private HashSet<char> lettersGuessed = new();
        private int lives = 3;

        public MainWindow()
        {
            InitializeComponent();
            UpdateUiForNoGame();
        }

        private void UpdateUiForNoGame()
        {
            LogoDisplay.Text = "#anged Man";
            WordDisplay.Text = "Press Start Game";
            LivesDisplay.Text = "-";
            MissedDisplay.Text = "-";
            MessageText.Text = string.Empty;
            DefinitionDisplay.Text = string.Empty;
            GuessTextBox.IsEnabled = false;
        }
        //StartGame_Click event handler XAML
        private void StartGame_Click(object? sender, RoutedEventArgs e)
        {
            var difficultySelected = (DifficultyCombo.SelectedItem as ComboBoxItem)?.Content?.ToString()?.ToLower() ?? "medium";
            lives = difficultySelected switch
            {
                "easy" => 5,
                "medium" => 3,
                "hard" => 1,
                _ => 3
            };

            var rand = new Random();
            chosenWord = words[rand.Next(words.Length)];
            //Set Definition if available
            var index = Array.IndexOf(words, chosenWord);
            chosenDefinition = index >= 0 && index < definitions.Length ? definitions[index] : string.Empty;

            lettersGuessed.Clear();
            MessageText.Text = string.Empty;
            GuessTextBox.IsEnabled = true;
            UpdateDisplays();
        }

        //XAML Restart_Click
        private void Restart_Click(object? sender, RoutedEventArgs e)
        {
            lettersGuessed.Clear();
            chosenWord = string.Empty;
            chosenDefinition = string.Empty;
            lives = 3;
            UpdateUiForNoGame();
        }

        //XAML Guess_Click
        private void Guess_Click(object? sender, RoutedEventArgs e)
        {
            var text = GuessTextBox.Text ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text)) return;
            var key = text.Trim().ToLower()[0];

            if (lettersGuessed.Contains(key))
            {
                MessageText.Text = $"'{key}' has already been guessed.";
                return;
            }

            lettersGuessed.Add(key);

            if (!chosenWord.Contains(key))
            {
                lives--;
                MessageText.Text = $"'{key}' is not present.";
            }
            else
            {
                MessageText.Text = $"'{key}' is in the word.";
            }

            UpdateDisplays();
            GuessTextBox.Text = string.Empty;
            EndGameCheck();
        }

        private void UpdateDisplays()
        {
            if (string.IsNullOrEmpty(chosenWord))
            {
                UpdateUiForNoGame();
                return;
            }

            LivesDisplay.Text = lives.ToString();

            var displayWord = string.Concat(chosenWord.Select(c => lettersGuessed.Contains(c) ? c : '_'));
            WordDisplay.Text = displayWord;

            var missedLetters = lettersGuessed.Where(c => !chosenWord.Contains(c)).ToArray(); 
            MissedDisplay.Text = string.Join(" ", missedLetters);
        }

        private void EndGameCheck()
        {
            if (lives <= 0)
            {
                MessageText.Text = $"You lost! The word was: {chosenWord}"; 
                DefinitionDisplay.Text = $"Definition: {chosenDefinition}";
                GuessTextBox.IsEnabled = false;
                return;
            }

            bool allRevealed = chosenWord.All(c => lettersGuessed.Contains(c));
            if (allRevealed)
            {
                MessageText.Text = $"You won with {lives} lives remaining!";
                DefinitionDisplay.Text = $"Definition: {chosenDefinition}";
                GuessTextBox.IsEnabled = false;
            }
        }
    } 
}
