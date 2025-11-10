/*  
Author: Elijah Weatherwax 
Start Date: 10/04/2025 
Name: #anged Man 
This is a application designed to help users learn C# terminology and concepts through a classic game of hangman.
*/
using Avalonia.Controls; 
using Avalonia.Interactivity;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using FinalH.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace FinalH
{
    public partial class MainWindow : Window
    {
        private readonly GameMan gameMan;
        private int maxLives = 5; //Medium Difficulty Default

        public MainWindow()
        {
            InitializeComponent();
            string[] words = {
                "abstraction", "encapsulation", "inheritance", "polymorphism",
                "gui", "method", "dotnet", "event", "console", "syntax",
                "class", "public", "private", "protected", "return",
                "try", "catch", "finally", "void", "refactoring"
            };

            string[] definitions = {
                "The process of hiding the complex reality while exposing only the necessary parts.",
                "The bundling of data with the methods that operate on that data.",
                "The mechanism of basing an object or class upon another object or class.",
                "The ability of different classes to be treated as instances of the same class through a common interface.",
                "This is a visual way of interacting with a computer using items like windows, icons, and menus.",
                "A block of code that performs a specific task.",
                "Free, cross-platform, open source developer platform for building many different types of applications.",
                "An action or occurrence recognized by software that may be handled by the software.",
                "A text-based interface used to interact with software and operating systems by typing commands.",
                "The set of rules that defines the combinations of symbols that are considered to be correctly structured programs in that language.",
                "The blueprint for creating objects, providing initial values for state (member variables) and implementations of behavior (member functions or methods).",
                "Access modifier that allows members to be accessible from any other code.",
                "A bundle of data and methods that operate on that data that is only accessible within its own class.",
                "A bundle of data and methods that operate on that data that restrict and controls its inheritance and visibility from outside access.",
                "The proccess of ending the execution of a method and optionally returning a value to the caller.",
                "The first part of exception handling that defines a block of code to be tested for errors while it is being executed.",
                "The second part of exception handling that defines how to respond to an exception.",
                "The end proccess of handling exceptions, used to execute code after try and catch blocks.",
                "Signifies that a method does not return a value.",
                "An act of improving the internal structure of existing code without changing its external behavior."
            };

            gameMan = new GameMan(words, definitions);
            gameMan.GameStateChanged += GameMan_GameStateChanged;
            gameMan.GameOver += GameMan_GameOver;
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
            UpdateHangmanDisplay(0);
        }

        private void UpdateHangmanDisplay(int remainingLives)
        {
            //Calculate Incorrect Guesses
            int wrongGuesses = maxLives - remainingLives;

            //Update colors based on wrong guesses
            var black = new SolidColorBrush(Colors.Black);
            var white = new SolidColorBrush(Colors.White);

            //Legs first (wrong guesses 1)
            if (wrongGuesses >= 1)
            {
                LeftLeg.Stroke = black;
                RightLeg.Stroke = black;
            }
            else
            {
                LeftLeg.Stroke = white;
                RightLeg.Stroke = white;
            }
            
            // Arms second (wrong guesses 2)
            if (wrongGuesses >= 2)
            {
                LeftArm.Stroke = black;
                RightArm.Stroke = black;
            }
            else
            {
                LeftArm.Stroke = white;
                RightArm.Stroke = white;
            }
            
            // Body third (wrong guesses 3)
            Body.Stroke = wrongGuesses >= 3 ? black : white;
            
            // Head last (wrong guesses 4)
            Head.Stroke = wrongGuesses >= 4 ? black : white;
            
            // Face features at game over
            if (wrongGuesses >= maxLives)
            {
                LeftEye.Fill = black;
                RightEye.Fill = black;
                Smile.Stroke = black;
            }
            else
            {
                LeftEye.Fill = white;
                RightEye.Fill = white;
                Smile.Stroke = white;
            }
        }

        private void GameMan_GameStateChanged(object? sender, EventArgs e)
        {
            UpdateDisplays();
        }

        private void GameMan_GameOver(object? sender, EventArgs e)
        {
            EndGameCheck();
        }

        private void StartGame_Click(object? sender, RoutedEventArgs e)
        {
            var difficultySelected = (DifficultyCombo.SelectedItem as ComboBoxItem)?.Content?.ToString()?.ToLower() ?? "medium";
            maxLives = difficultySelected switch
            {
                "easy" => 7,
                "medium" => 5,
                "hard" => 3,
                _ => 5
            };

            gameMan.StartNewGame(difficultySelected);
            MessageText.Text = string.Empty;
            GuessTextBox.IsEnabled = true;
            UpdateDisplays();
            UpdateHangmanDisplay(gameMan.Lives);
        }

        private void Restart_Click(object? sender, RoutedEventArgs e)
        {
            gameMan.ResetGame();
            UpdateUiForNoGame();
        }

        private void Guess_Click(object? sender, RoutedEventArgs e)
        {
            var text = GuessTextBox.Text ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text)) return;
            var key = text.Trim().ToLower()[0];

            if (!gameMan.GuessLetter(key))
            {
                MessageText.Text = $"'{key}' has already been guessed.";
                return;
            }

            MessageText.Text = gameMan.ChosenWord.Contains(key) 
                ? $"'{key}' is in the word."
                : $"'{key}' is not present.";

            UpdateDisplays();
            UpdateHangmanDisplay(gameMan.Lives);
            GuessTextBox.Text = string.Empty;
        }

        private void UpdateDisplays()
        {
            WordDisplay.Text = gameMan.GetDisplayWord();
            LivesDisplay.Text = gameMan.Lives.ToString();
            MissedDisplay.Text = gameMan.GetMissedLetters();
        }

        private void EndGameCheck()
        {
            if (gameMan.Lives <= 0)
            {
                MessageText.Text = $"You lost! The word was: {gameMan.ChosenWord}"; 
                DefinitionDisplay.Text = $"Definition: {gameMan.ChosenDefinition}";
                GuessTextBox.IsEnabled = false;
                return;
            }

            if (gameMan.IsWordGuessed())
            {
                MessageText.Text = $"You won with {gameMan.Lives} lives remaining!";
                DefinitionDisplay.Text = $"Definition: {gameMan.ChosenDefinition}";
                GuessTextBox.IsEnabled = false;
            }
        }
    } 
}