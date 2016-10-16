using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevBox.Models;
using Microsoft.Ajax.Utilities;


namespace DevBox.Controllers
{
    public class PlayController : Controller
    {
        DevBoxEntities _entities = new DevBoxEntities();

        // GET: Play
        public ActionResult Index()
        {
            return View(_entities.Categories.ToList());
        }

        public ActionResult Puzzles(int idCategory)
        {
            return View(_entities.Puzzle.Where(x => x.Id_Category == idCategory).ToList());
        }

        public ActionResult Question(int idPuzzle, int? idQuestion)
        {
            if (idQuestion == null)
            {
                idQuestion = _entities.Complies_Puzzle_Question.OrderBy(x => x.Index).FirstOrDefault(x => x.Id_Puzzle == idPuzzle).Id_Question;
            }
            Question question = GetQuestion(idQuestion.Value);
            ViewBag.IdPuzzle = idPuzzle;
            return View("Question", null, question);
        }

        [HttpPost]
        public ActionResult CheckQuestion(int idQuestion, int idPuzzle, string answer)
        {
            Question questinon = GetQuestion(idQuestion);
            ViewBag.idPuzzle = idPuzzle;
            var trueAnswer = _entities.Answers.FirstOrDefault(x => x.Id_Question.Value == idQuestion).Answer;
            if (trueAnswer.ToLower() == answer.ToLower())
            {
                var nextQuestion = GetNextQuestion(questinon, idPuzzle);
                if (nextQuestion != null)
                    return RedirectToAction("Question", "Play", new { idPuzzle = idPuzzle, idQuestion = nextQuestion.Id });
                else return View("Win");
            }
            else if (_entities.Tips.Where(x=>x.Id_Question == idQuestion).Select(x => x.Answers).ToList().Exists(x => x.ToLower() == answer.ToLower()))
            {
                ViewBag.Tip = _entities.Tips.Where(x => x.Id_Question == idQuestion).ToList().FirstOrDefault(x => x.Answers.ToLower() == answer.ToLower()).Text;
                return View("Tip", null, questinon);
            }
            else
            {

                return View("Error", null, questinon);
            }
        }


        private Question GetQuestion(int idQuestion)
        {
            return _entities.Question.FirstOrDefault(x => x.Id == idQuestion);
        }
        private Question GetNextQuestion(Question prevQuestion, int idPuzzle)
        {
            var questions = _entities.Complies_Puzzle_Question.Where(x => x.Id_Puzzle == idPuzzle).OrderBy(x => x.Index).Select(x => x.Question).ToList();
            var indexPrevQuestion = questions.IndexOf(prevQuestion);
            if (indexPrevQuestion + 1 < questions.Count)
                return questions[indexPrevQuestion + 1];
            else return null;
        }
    }
}