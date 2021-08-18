using InterviewTool.src.Models;
using InterviewTool.src.PrintStrategy;
using System;
using System.Linq;

namespace InterviewTool.src
{
    public static class Prompter
    {
        public static void PrintQuestion(Question ques)
        {
            ques.PrintQuestion();
            ques.PrintAnswer();
            var done = false;
            while (!done)
                done = ques.WaitForInput();
        }

        public static string Prompt(string entity)
        {
            bool confirmed;
            string result;
            do
            {
                Helpers.PrintMsgWithoutSeparator($"Please input {entity}.");
                result = Console.ReadLine();
                confirmed = PromptForConfirmation(result);
            } while (!confirmed);
            return result;
        }

        public static string PromptForCandidate() => Prompt("candidate name");

        private static bool PromptForConfirmation(string toConfirm)
        {
            ConsoleKey key;
            do
            {
                Helpers.PrintMsgWithoutSeparator("Received input (Y to confirm):");
                Helpers.PrintMsgWithoutSeparator(toConfirm);
                key = Console.ReadKey().Key;
            } while (key != ConsoleKey.Y && key != ConsoleKey.N);
            return key == ConsoleKey.Y;
        }

        public static void PromptForStrategy(Interview interview)
        {
            ConsoleKey key;
            do
            {
                Helpers.PrintMsgWithoutSeparator("Please select how to print:");
                Helpers.PrintMsgWithoutSeparator("Screen (S); PDF (P); File (F)");
                key = Console.ReadKey().Key;
            } while (key != ConsoleKey.S && key != ConsoleKey.F && key != ConsoleKey.P);
            
            // ScreenPrint is the default
            switch(key)
            {
                case ConsoleKey.P:
                    interview.SetStrategy(new PdfPrint());
                    break;
                case ConsoleKey.F:
                    interview.SetStrategy(new FilePrint());
                    break;
                default:
                    interview.SetStrategy(new ScreenPrint());
                    break;
            }
        }

        public static string PromptForExpAndCerts(Interview interview) => Prompt(interview.ExperienceAndCerts.Description);
        public static string PromptForBussinessExp(Interview interview) => Prompt(interview.BusinessComments.Description);
        public static string PromptForComments(Interview interview) => Prompt(interview.FinalComments.Description);
        public static Grade? PromptForGrade(Interview interview)
        {
            if (interview.Grades.Any())
            {
                int idx = 1, res;
                
                Helpers.PrintMsgWithoutSeparator($"Final calculated score: {interview.GetFinalScore()}");
                Helpers.PrintMsgWithoutSeparator("Please select the final suggested grade:");
                interview.Grades.ForEach(g =>
                    {
                        Helpers.PrintMsgWithoutSeparator($"#{idx}; {g.Acronym} {g.Name} {g.RequiredScore}");
                        idx++;
                    });
                string keyPress = "";
                do
                {
                    Helpers.PrintMsgWithoutSeparator("Input grade (#id):");
                    keyPress = Console.ReadLine();

                } while (!int.TryParse(keyPress, out res));
                return interview.Grades.ToArray()[res - 1];
            }
            return null;
        }

        public static bool PromptForAcceptance()
        {
            ConsoleKey keypress;
            do
            {
                Helpers.PrintMsgWithoutSeparator($"Is the candidate accepted?");
                keypress = Console.ReadKey().Key;
            } while (keypress != ConsoleKey.Y && keypress != ConsoleKey.N);
            return keypress == ConsoleKey.Y;
        }
    }
}
