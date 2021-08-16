using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InterviewTool.src
{
    public class Runner
    {
        private readonly string _interviewer;
        private readonly Interview _interview;

        public Runner(IConfiguration configuration)
        {
            _interviewer = configuration.GetValue<string>("Interviewer");
            var questionPath = configuration.GetValue<string>("QuestionPath");
            var minRange = configuration.GetValue<int>("MinRange");
            var maxRange = configuration.GetValue<int>("MaxRange");
            var path = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).FullName, questionPath);
            var questions = LoadQuestions(Path.Combine(Directory.GetParent(AppContext.BaseDirectory).FullName, questionPath)).OrderBy(x => x.Category);
            _interview = new Interview(_interviewer, questions.Select(x => new Question(minRange, maxRange, x)).ToList());
        }

        public void Run()
        {
            _interview.PrintInfo();
            Prompter.PromptForCandidate(_interview);

            string prevCat = _interview.Questions.First().Category;
            Helpers.PrintMsgWithoutSeparator($"Category: {prevCat}", ConsoleColor.Yellow);
            foreach (var q in _interview.Questions)
            {
                if (!prevCat.Equals(q.Category))
                    Helpers.PrintMsgWithoutSeparator($"Category: {q.Category}", ConsoleColor.Yellow);

                Prompter.PrintQuestion(q);
                prevCat = q.Category;
            }
            Helpers.PrintSuccessMsg("Interview finished!");
            Prompter.PromptForStrategy(_interview);
            _interview.PrintReport();
        }

        private List<QuestionDto> LoadQuestions(string path)
        {
            Helpers.PrintLoadingMsg($"{nameof(Question)}s");

            using (StreamReader sr = new StreamReader(path))
            {
                string json = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<List<QuestionDto>>(json);
            }
        }
    }
}
