using InterviewTool.src.Models;
using System;
using System.Linq;

namespace InterviewTool.src.PrintStrategy
{
    public class ScreenPrint : IPrintStrategy
    {
        public void Print(Interview interview)
        {
            Helpers.PrintMsgWithSeparator(ConsoleColor.Green, "Printing report...");

            var skippedQuestions = interview.Questions
                .Where(q => q.Skipped)
                .ToList();

            if (skippedQuestions.Any())
            {
                Helpers.PrintMsgWithoutSeparator("Skipped questions:");
                skippedQuestions.ForEach(q => Helpers.PrintMsgWithoutSeparator(q.Query, ConsoleColor.Red));
            }

            interview.Questions
                .Where(q => !q.Skipped)
                .GroupBy(q => new { q.Area })
                .Select(x => new { x.Key.Area, Average = x.Average(g => g.Grade) })
                .ToList()
                .ForEach(rs =>
                {
                    Helpers.PrintMsgWithoutSeparator(rs.Area, ConsoleColor.Green);
                    Helpers.PrintMsgWithoutSeparator(rs.Average.ToString(), ConsoleColor.Yellow);
                });
            Helpers.PrintMsgWithoutSeparator($"Final grade: {interview.GetFinalScore()}", ConsoleColor.Green);
        }
    }
}
