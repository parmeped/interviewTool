using InterviewTool.src.PrintStrategy;
using System;

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

        public static void PromptForCandidate(Interview interview)
        {
            var result = false;
            string name = "";
            while (!result)
            {
                Helpers.PrintMsgWithoutSeparator("Please input candidate name.");
                name = Console.ReadLine();
                result = PromptForConfirmation(name);
            }
            interview.CandidateName = name;
        }

        public static bool PromptForConfirmation(string toConfirm)
        {
            ConsoleKey key;
            do
            {
                Helpers.PrintMsgWithoutSeparator("Received input (Y to confirm):");
                Helpers.PrintMsgWithoutSeparator(toConfirm);
                key = Console.ReadKey().Key;
            } while (key != ConsoleKey.Y);
            return true;
        }

        public static void PromptForStrategy(Interview interview)
        {
            ConsoleKey key;
            do
            {
                Helpers.PrintMsgWithoutSeparator("Please select how to print:");
                Helpers.PrintMsgWithoutSeparator("Screen (S); File (F)");
                key = Console.ReadKey().Key;
            } while (key != ConsoleKey.S && key != ConsoleKey.F);
            
            // ScreenPrint is the default
            switch(key)
            {
                case ConsoleKey.F:
                    interview.SetStrategy(new FilePrint());
                    break;
                default:
                    interview.SetStrategy(new ScreenPrint());
                    break;
            }
        }
    }
}
