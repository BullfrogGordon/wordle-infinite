using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace wordlegram.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        //global variables=
        public bool showNext = false;
        public void startGame()
        {
            HttpContext.Session.SetString("guess", "0");
            string gameWord = wordPicker();
            HttpContext.Session.SetString("gameWord", gameWord);
            showNext = false;
            for (int i = 0;i < 6; i++) 
            { 
                HttpContext.Session.SetString("game" + i, "");
            }

        }

        public void endGame(bool victory, int guessPoints) 
        //Handles the game ending in glorious victory or shameful defeat
            {
            if (victory)
            {

                //scorekeep increments the score upon correct guessing of word- we're passing in the guess number to get the correct score value
                scoreKeep(guessPoints);
                startGame();
                showNext= true;

            }
            else
            {

            }     
            }
        public string wordPicker()
        //Picks a word from a text file and 
        {
            string[] newWord = System.IO.File.ReadAllLines("Pages/wordlist.txt");
            var rand = new Random();
            long randomWord = rand.NextInt64(0, newWord.Length);
            return newWord[randomWord];
        } 

        public void newWord()
        {

            HttpContext.Session.SetString("score", "0");
            wordPicker();

        }
        public void scoreKeep(int pointsToGive)
        {
            //Handles scorekeeping 
          int score = Convert.ToInt32(HttpContext.Session.GetString("score"));
            switch (pointsToGive)
            {
                case 1:
                    score += 1000;
                    break;
                case 2:
                    score += 500;
                    break;
                case 3:
                    score += 250;
                    break;
                case 4:
                    score += 175;
                    break;
                case 5:
                    score += 100;
                    break;
                case 6:
                    score += 50;
                    break;
            }
            HttpContext.Session.SetString("score", score.ToString());
        }

        public string theGame(string guessWord)
        {
            //General Game 'engine'
        if (Convert.ToInt32(HttpContext.Session.GetString("guess")) >= 6)
            {
                return "You are out of tries!";
            }
            string gameWord = HttpContext.Session.GetString("gameWord")!;
            int wordLength = 5;
            int guessNumber = Convert.ToInt32(HttpContext.Session.GetString("guess")) + 1;
            HttpContext.Session.SetString("guess", guessNumber.ToString());
            var gameResponse = new List<string>();
            char[] theWord = gameWord.ToUpper().ToArray();
            char[] theGuess = guessWord.ToUpper().ToArray();
            string[] theResponse = new string[wordLength];
            for (int j = 0; j < theGuess.Length; j++)
            {
                if (theWord.Contains(theGuess[j]))
                {
                    if (theWord[j] == theGuess[j])
                    {
                        gameResponse.Add("<div class=\"square-green\">" + theGuess[j] + "</div>");
                    }
                    else
                    {
                        gameResponse.Add("<div class=\"square-yellow\">" + theGuess[j] + "</div>");
                    }
                }
                else
                {
                    gameResponse.Add("<div class=\"square-darkgrey\">" + theGuess[j] + "</div>");
                }
            }
                string htmlBlob = String.Join("",gameResponse);
                HttpContext.Session.SetString("game" + guessNumber, htmlBlob);

                if (gameWord.ToUpper() == guessWord.ToUpper())
                {
                endGame(true, guessNumber);
                return htmlBlob;
                } 
                else if (guessNumber >= 6)
                {
                endGame(false, guessNumber);
                return htmlBlob + "\nGame Over!";
                }
                else
                {
                return htmlBlob;
                }
        }
        public void OnGet()
        {
            HttpContext.Session.SetString("guess", "0");
            HttpContext.Session.SetString("score", "0");
            startGame();
        }
        public void OnPost()
        {

        }
    }
}