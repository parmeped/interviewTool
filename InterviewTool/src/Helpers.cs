using System;

namespace InterviewTool.src
{
    public static class Helpers
    {
        public static void PrintMsgWithSeparator(ConsoleColor color, string msg)
        {
            Console.WriteLine();
            Console.ForegroundColor = color;
            Console.WriteLine(Constants.Separator);
            Console.WriteLine(msg);
            Console.WriteLine(Constants.Separator);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }

        public static void PrintMsgWithoutSeparator(string msg, ConsoleColor color = ConsoleColor.White)
        {
            Console.WriteLine();
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void PrintWarningMsg(string msg) => PrintMsgWithSeparator(ConsoleColor.Red, $"Warning! {msg}");
        public static void PrintNoNoMsg(string msg) => PrintMsgWithoutSeparator(msg, ConsoleColor.Red);
        public static void PrintSuccessMsg(string msg) => PrintMsgWithSeparator(ConsoleColor.Green, msg);
        public static void PrintQuestionMsg(string q) => PrintMsgWithoutSeparator(q, ConsoleColor.Yellow);
        public static void PrintAnswerMsg(string a) => PrintMsgWithoutSeparator(a, ConsoleColor.Gray);
        public static void PrintLoadingMsg(string entityLoading)
        {
            Console.WriteLine();
            Console.WriteLine(Constants.Separator);
            Console.WriteLine($"Loading {entityLoading}...");
        }
    }
}
