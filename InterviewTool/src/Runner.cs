using InterviewTool.src.Models;
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
            var gradePath = configuration.GetValue<string>("GradePath");
            var htmlTemplatePath = Path.Combine(Directory.GetParent(AppContext.BaseDirectory).FullName, configuration.GetValue<string>("HtmlTemplatePath"));
            var minRange = configuration.GetValue<int>("MinRange");
            var maxRange = configuration.GetValue<int>("MaxRange");
            var expAndCerts = new InterviewPart { Description = "Overrall experience and Certificates", Skipped = configuration.GetValue<bool>("SkipExpAndCert") };
            var businessComments = new InterviewPart { Description = "Business experience", Skipped = configuration.GetValue<bool>("SkipBusinessComments") };
            var finalComments = new InterviewPart { Description = "Comments & Final review", Skipped = configuration.GetValue<bool>("SkipFinalComments") };

            var grades = LoadEntity<Grade>(Path.Combine(Directory.GetParent(AppContext.BaseDirectory).FullName, gradePath));
            var questions = LoadEntity<QuestionDto>(Path.Combine(Directory.GetParent(AppContext.BaseDirectory).FullName, questionPath)).OrderBy(x => x.Area);
            _interview = new Interview(
                _interviewer
                , questions.Select(x => new Question(minRange, maxRange, x)).ToList()
                , grades.OrderBy(x => x.Name).ToList()
                , htmlTemplatePath
                , businessComments
                , finalComments
                , expAndCerts
                ,(minRange, maxRange));
        }

        public void Run()
        {
            _interview.PrintInfo();
            _interview.CandidateName = Prompter.PromptForCandidate();
            if (!_interview.ExperienceAndCerts.Skipped)
                _interview.ExperienceAndCerts.Input = Prompter.PromptForExpAndCerts(_interview);

            string prevCat = _interview.Questions.First().Area;
            Helpers.PrintMsgWithoutSeparator($"Category: {prevCat}", ConsoleColor.Yellow);
            foreach (var q in _interview.Questions)
            {
                if (!prevCat.Equals(q.Area))
                    Helpers.PrintMsgWithoutSeparator($"Category: {q.Area}", ConsoleColor.Yellow);

                Prompter.PrintQuestion(q);
                prevCat = q.Area;
            }
            Helpers.PrintSuccessMsg("Interview finished!");
            if (!_interview.BusinessComments.Skipped)
                _interview.BusinessComments.Input = Prompter.PromptForBussinessExp(_interview);

            if (!_interview.FinalComments.Skipped)
                _interview.FinalComments.Input = Prompter.PromptForComments(_interview);

            _interview.FinalGrade = Prompter.PromptForGrade(_interview);
            _interview.Accepted = Prompter.PromptForAcceptance();
            Prompter.PromptForStrategy(_interview);
            _interview.PrintReport();
        }

        private List<T> LoadEntity<T>(string path)
        {
            var type = typeof(T);
            Helpers.PrintLoadingMsg($"{type.Name}s");

            using (StreamReader sr = new StreamReader(path))
            {
                string json = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<List<T>>(json);
            }
        }
    }
}
