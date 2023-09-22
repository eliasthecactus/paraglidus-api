using Microsoft.AspNetCore.Mvc;
using api.Models;


namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExerciseController : ControllerBase
    {
        private readonly ExerciseDbContext dbContext;
        private readonly List<string> possibleTopics = new List<string> { "wetterkunde", "fluglehre", "materialkunde", "gesetzgebung", "flugpraxis" };
        public ExerciseController(ExerciseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet("all")]
        public IActionResult GetExercises(bool withAnswer)
        {
            try
            {
                // Get the query parameter "answer" from the URL
                bool includeAnswer = bool.TryParse(HttpContext.Request.Query["withAnswer"], out bool answer) && answer;
                var exercises = dbContext.Exercises.ToList();
                if (includeAnswer)
                {
                    // Include answers in the response
                    return Ok(exercises);
                }
                else
                {
                    // Exclude answers from the response
                    var exercisesWithoutAnswers = exercises.Select(e => new { e.Id, e.possible_answers, e.img, e.question, e.topic }).ToList();
                    return Ok(exercisesWithoutAnswers);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately.
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("all/{topic}")]
        public IActionResult GetTopicQuestion(string topic, bool withAnswer)
        {
            try
            {
                // Get the query parameter "answer" from the URL
                bool includeAnswer = bool.TryParse(HttpContext.Request.Query["withAnswer"], out bool answer) && answer;
                
                if (possibleTopics.Contains(topic.ToLower()))
                {
                    var exercises = dbContext.Exercises
                .Where(e => string.Compare(e.topic.ToLower(), topic.ToLower()) == 0)
                .ToList();
                    if (includeAnswer)
                    {
                        // Include answers in the response
                        return Ok(exercises);
                    }
                    else
                    {
                        // Exclude answers from the response
                        var exercisesWithoutAnswers = exercises.Select(e => new { e.Id, e.possible_answers, e.img, e.question, e.topic }).ToList();
                        return Ok(exercisesWithoutAnswers);
                    }
                } else
                {
                    return StatusCode(422, "'" + topic + "' is not a valid topic");
                }
                
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately.
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("random")]
        public IActionResult GetRandomQuestion(bool withAnswer)
        {
            try
            {
                // Get the query parameter "answer" from the URL
                bool includeAnswer = bool.TryParse(HttpContext.Request.Query["withAnswer"], out bool answer) && answer;
                var allExercises = dbContext.Exercises.ToList();

                // Check if there are any exercises
                if (allExercises.Count > 0)
                {
                    // Generate a random index
                    Random random = new Random();
                    int randomIndex = random.Next(0, allExercises.Count);

                    // Get the random exercise
                    var randomExercise = allExercises[randomIndex];

                    if (includeAnswer)
                    {
                        // Include answers in the response
                        return Ok(randomExercise);
                    }
                    else
                    {
                        // Exclude answers from the response
                        var exerciseWithoutAnswers = new
                        {
                            randomExercise.Id,
                            randomExercise.possible_answers,
                            randomExercise.img,
                            randomExercise.question,
                            randomExercise.topic
                        };
                        return Ok(exerciseWithoutAnswers);
                    }
                }
                else
                {
                    return StatusCode(404, "No exercises found");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately.
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("random/{topic}")]
        public IActionResult GetRandomTopicQuestion(string topic, bool withAnswer)
        {
            try
            {
                // Get the query parameter "answer" from the URL
                bool includeAnswer = bool.TryParse(HttpContext.Request.Query["withAnswer"], out bool answer) && answer;
                if (possibleTopics.Contains(topic.ToLower()))
                {
                    var allExercises = dbContext.Exercises
                .Where(e => string.Compare(e.topic.ToLower(), topic.ToLower()) == 0)
                .ToList();
                    if (allExercises.Count > 0)
                    {
                        // Generate a random index
                        Random random = new Random();
                        int randomIndex = random.Next(0, allExercises.Count);

                        // Get the random exercise
                        var randomExercise = allExercises[randomIndex];

                        if (includeAnswer)
                        {
                            // Include answers in the response
                            return Ok(randomExercise);
                        }
                        else
                        {
                            // Exclude answers from the response
                            var exerciseWithoutAnswers = new
                            {
                                randomExercise.Id,
                                randomExercise.possible_answers,
                                randomExercise.img,
                                randomExercise.question,
                                randomExercise.topic
                            };
                            return Ok(exerciseWithoutAnswers);
                        }
                    }
                    else
                    {
                        return StatusCode(404, "No exercises found");
                    }
                }
                else
                {
                    return StatusCode(422, "'" + topic + "' is not a valid topic");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately.
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("id/{id}")]
        public IActionResult GetIdQuestion(int id, bool withAnswer)
        {
            try
            {
                // Get the query parameter "answer" from the URL
                bool includeAnswer = bool.TryParse(HttpContext.Request.Query["withAnswer"], out bool answer) && answer;

                // Retrieve the exercise with the specified ID
                var exercise = dbContext.Exercises.FirstOrDefault(e => e.Id == id);

                if (exercise != null)
                {
                    if (includeAnswer)
                    {
                        // Include answers in the response
                        return Ok(exercise);
                    }
                    else
                    {
                        // Exclude answers from the response
                        var exerciseWithoutAnswers = new
                        {
                            exercise.Id,
                            exercise.possible_answers,
                            exercise.img,
                            exercise.question,
                            exercise.topic
                        };
                        return Ok(exerciseWithoutAnswers);
                    }
                }
                else
                {
                    return StatusCode(404, "Exercise not found");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately.
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet("answer/{id}")]
        public IActionResult GetAnswer(int id)
        {
            try
            {
                // Retrieve the exercise with the specified ID
                var exercise = dbContext.Exercises.FirstOrDefault(e => e.Id == id);

                if (exercise != null)
                {
                    // Return only the answer
                    return Ok(exercise.answer);
                }
                else
                {
                    return StatusCode(404, "Exercise not found");
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately.
                return StatusCode(500, "Internal server error");
            }
        }
    }
}