using System; 
using System.Collections.Generic;
using System.Linq;
//Attempted MVVM modeling for refactoring requirements
namespace FinalH.Models
{
    public class GameMan
    {
        private readonly string[] words;
        private readonly string[] definitions;
        private readonly Random random = new();


        public event EventHandler? GameStateChanged;
        public event EventHandler? GameOver;

        public string ChosenWord { get; private set; } = string.Empty;
        public string ChosenDefinition { get; private set; } = string.Empty; 
        public HashSet<char> LettersGuessed { get; private set; } = new();
        public int Lives { get; private set; }


        public GameMan(string[] words, string[] definitions)
        {
            this.words = words;
            this.definitions = definitions;
        }

        public string GetDisplayWord()
        {
            return string.IsNullOrEmpty(ChosenWord)
                ? "Press Start Game"
                : string.Concat(ChosenWord.Select(c => LettersGuessed.Contains(c) ? c : '_'));
        }

        public string GetMissedLetters()
        {
            return string.Join(" ", LettersGuessed.Where(c => !ChosenWord.Contains(c)));
        }

        public bool IsGameOver()
        {
            return Lives <= 0 || IsWordGuessed();
        }

        public bool IsWordGuessed()
        {
            return !string.IsNullOrEmpty(ChosenWord) && ChosenWord.All(c => LettersGuessed.Contains(c));
        }

        public bool GuessLetter(char letter)
        {
            if (LettersGuessed.Contains(letter)) return false;
            
            LettersGuessed.Add(letter);
            if (!ChosenWord.Contains(letter))
                Lives--;

            GameStateChanged?.Invoke(this, EventArgs.Empty);

            if (IsGameOver())
                GameOver?.Invoke(this, EventArgs.Empty);

            return true;
        }

        private int GetLivesForDifficulty(string difficulty)
        {
            return difficulty.ToLower() switch
            {
                "easy" => 7,
                "medium" => 5,
                "hard" => 3,
                _ => 5
            };
        }

        public void StartNewGame(string difficulty)
        {
            Lives = GetLivesForDifficulty(difficulty);
            LettersGuessed.Clear();
            var index = random.Next(words.Length);
            ChosenWord = words[index];
            ChosenDefinition = index < definitions.Length ? definitions[index] : string.Empty;
        }

        public void ResetGame()
        {
            ChosenWord = string.Empty;
            ChosenDefinition = string.Empty;
            LettersGuessed.Clear();
            Lives = 5;
        }
    }
} 