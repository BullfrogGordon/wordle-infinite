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

        public string theGame(string guessWord)
        {
        if (Convert.ToInt32(HttpContext.Session.GetString("guess")) >= 5)
            {
                return "You are out of guesses!";
            }

            int wordLength = 5;
            string gameWord = "tacos";
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
                return htmlBlob;
                } 
                else
                {
                return htmlBlob;
                }
        }
        public void OnGet()
        {
            HttpContext.Session.SetString("guess", "0");
        }
        public void OnPost()
        {
            
        }
    }
}