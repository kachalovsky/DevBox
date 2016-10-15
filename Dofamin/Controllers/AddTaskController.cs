using System.Linq;
using System.Web.Mvc;
using DevBox.Models;
using System.Web;
using System.Collections.Generic;

namespace DevBox.Controllers
{
    public class AddTaskController : Controller
    {
        // GET: AddTask
        private DevBoxEntities _entities = new DevBoxEntities();

        public ActionResult Index()
        {
            return View(_entities.Puzzle.ToList());
        }
        public ActionResult AddPuzzle(int id = 0)
        {
            ViewBag.Categories = _entities.Categories.ToList();
            Puzzle puzzle;
            if (id == 0)
                puzzle = new Puzzle();
            else
                puzzle = _entities.Puzzle.FirstOrDefault(x => x.Id == id);
            return View(puzzle);
        }
        [HttpPost]
        public void AddPuzzle(Puzzle puzzle)
        {
            Puzzle tempPuzzle;
            if (puzzle.Id == 0)
            {
                _entities.Puzzle.Add(puzzle);
                tempPuzzle = puzzle;
            }
            else
            {
                tempPuzzle = _entities.Puzzle.FirstOrDefault(x => x.Id == puzzle.Id);
                tempPuzzle.Id_Category = puzzle.Id_Category;
                tempPuzzle.Name = puzzle.Name;
                tempPuzzle.Description = puzzle.Description;
            }
            _entities.SaveChanges();

            Response.Redirect(Url.Action("AddQuestion", new { idPuzzle = puzzle.Id }));
        }
        public ActionResult AddQuestion(int idPuzzle, int num = 1)
        {
            AddInfo model = new AddInfo()
            {
                Categories = _entities.Categories.ToList(),
            };
            var questions = _entities.Complies_Puzzle_Question.Where(x => x.Id_Puzzle == idPuzzle).OrderBy(x => x.Index).Select(x => x.Question).ToList();

            if (num - questions.Count() == 1)
            {
                model.PuzzleId = idPuzzle;
                model.Num = num;
                model.Question = new Question();
                model.All = questions.Count() + 1;
                model.Answer = new Answers();
            }
            else if (num <= questions.Count())
            {
                model.PuzzleId = idPuzzle;
                model.Num = num;
                model.Question = questions.Skip(num - 1).FirstOrDefault();
                model.All = questions.Count();
                model.Answer = _entities.Answers.Where(x => x.Id_Question == model.Question.Id).FirstOrDefault();

            }
            else
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        public void AddQuestion(AddInfo addInfo, HttpPostedFileBase image)
        {
            if (image != null)
            {
                addInfo.Question.ImgMimeType = image.ContentType;
                addInfo.Question.Img = new byte[image.ContentLength];
                image.InputStream.Read(addInfo.Question.Img, 0, image.ContentLength);
            }

            if (addInfo.Question.Id == 0)
            {
                _entities.Question.Add(addInfo.Question);
                IEnumerable<Complies_Puzzle_Question> compouse = _entities.Complies_Puzzle_Question.Where(x => x.Id_Puzzle == addInfo.PuzzleId).ToList();
                int index;
                if (compouse.Count() > 0)
                    index = compouse.Max(x => x.Index).Value + 1;
                else index = 0;
                _entities.Complies_Puzzle_Question.Add(new Complies_Puzzle_Question { Id_Puzzle = addInfo.PuzzleId, Id_Question = addInfo.Question.Id, Index = index });
                Answers ans = new Answers { Id_Question = addInfo.Question.Id };
                ans.Answer = addInfo.Answer.Answer;
                _entities.Answers.Add(ans);
            }
            else
            {
                Question que = _entities.Question.Where(x => x.Id == addInfo.Question.Id).FirstOrDefault();
                que.Img = addInfo.Question.Img;
                que.ImgMimeType = addInfo.Question.ImgMimeType;
                que.Text = addInfo.Question.Text;
                Answers ans = _entities.Answers.FirstOrDefault(x => x.Id_Question == que.Id);
                ans.Answer = addInfo.Answer.Answer;
            }
            _entities.SaveChanges();
            Response.Redirect(Url.Action("AddQuestion", new { idPuzzle = addInfo.PuzzleId, num = addInfo.Num + 1 }));
        }

        public FileContentResult GetImage(int id)
        {
            Question que = _entities.Question.FirstOrDefault(x => x.Id == id);
            return que != null ? File(que.Img, que.ImgMimeType) : null;
        }

    }
}