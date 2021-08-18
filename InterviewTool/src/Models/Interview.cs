using InterviewTool.src.PrintStrategy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTool.src.Models
{
    public class Interview
    {
        public Interview(
            string interviewerName
            , List<Question> questions
            , List<Grade> grades
            , string htmlTemplatePath
            , InterviewPart businessComments
            , InterviewPart finalComments
            , InterviewPart expAndCerts
            , (int, int) gradeRange)
        {
            InterviewerName = interviewerName;
            Questions = questions;
            Grades = grades;
            HtmlTemplatePath = htmlTemplatePath;
            BusinessComments = businessComments;
            FinalComments = finalComments;
            ExperienceAndCerts = expAndCerts;
            GradeRange = gradeRange;
        }
        public readonly (int MinGrade, int MaxGrade) GradeRange;
        private IPrintStrategy _printStrategy { get; set; } = new ScreenPrint();
        public string CandidateName { get; set; }
        public InterviewPart BusinessComments { get; set; }
        public InterviewPart FinalComments { get; set; }
        public InterviewPart ExperienceAndCerts { get; set; }
        public Grade FinalGrade { get; set; }
        public readonly string InterviewerName;
        public readonly string HtmlTemplatePath;
        public bool Accepted { get; set; } = false;
        public List<Question> Questions { get; set; } = new List<Question>(8);
        public List<Grade> Grades { get; set; } = new List<Grade>(8);
        public double GetFinalScore() => !Questions.Any() ? 0 : Questions.Where(x => !x.Skipped).Average(x => x.Grade);
        public void SetStrategy(IPrintStrategy strategy) => _printStrategy = strategy ?? throw new ArgumentNullException(nameof(strategy));
        public void PrintInfo()
        {
            Helpers.PrintMsgWithoutSeparator($"Interviewer: {InterviewerName}");
            Helpers.PrintMsgWithoutSeparator($"Total questions: {Questions.Count()}");
        }
        public void PrintReport() => _printStrategy.Print(this);
    }

    public class QuestionDto
    {
        public string Area { get; set; }
        public string Technology { get; set; }
        public string Query { get; set; }
        public string Answer { get; set; }
    }

    public class Question
    {
        public Question(int minGrade, int maxGrade, QuestionDto dto)
        {
            GradeRange = (minGrade, maxGrade);
            Query = dto.Query;
            Answer = dto.Answer;
            Area = dto.Area;
            Tecnhology = dto.Technology;
        }

        public readonly (int MinGrade, int MaxGrade) GradeRange;
        public readonly string Area;
        public readonly string Tecnhology;
        public readonly string Query;
        public readonly string Answer = "";
        public bool Skipped { get; private set; } = false;
        public int Grade { get; private set; }
        public bool SetGrade(int grade)
        {
            if (grade > GradeRange.MaxGrade || grade < GradeRange.MinGrade)
            {
                Helpers.PrintNoNoMsg(string.Format(Constants.GradeOutsideOfRange, grade));
                return false;
            }
            Grade = grade;
            return true;
        }
        public void SkipQuestion()
        {
            Skipped = true;
            Console.WriteLine("Skipped!");
        }
        public void PrintQuestion() => Helpers.PrintQuestionMsg($"Q: {Query}");
        public void PrintAnswer()
        {
            if (!string.IsNullOrEmpty(Answer))
                Helpers.PrintAnswerMsg($"A: {Answer}");
        }
        public bool WaitForInput()
        {
            Console.WriteLine($"Waiting for input ({GradeRange.MinGrade}-{GradeRange.MaxGrade})..");
            var keyPress = Console.ReadLine();
            
            if (Enum.TryParse(keyPress.ToUpper(), out ConsoleKey key) && key == ConsoleKey.S)
            {
                SkipQuestion();
                return true;
            }
            else if (int.TryParse(keyPress.ToString(), out int res))
            {
                return SetGrade(res);
            }
            Console.WriteLine();
            return false;
        }
    }
}
