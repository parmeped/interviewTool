using InterviewTool.src.Models;
using RazorLight;
using System;
using System.Text;

namespace InterviewTool.src.PrintStrategy
{
    public class PdfPrint : IPrintStrategy
    {
        public void Print(Interview interview)
        {
            var template = new HtmlTemplate(interview.HtmlTemplatePath);
            var engine = EngineFactory.CreatePhysical(interview.HtmlTemplatePath);
            var result = engine.Parse(interview.HtmlTemplatePath, interview);
            //var engine = RazorEngineService.Create();
            //var result = engine.RunCompile(template._html, "templateKey", null, interview);

            //var builder = new StringBuilder();
            //var output = template.Render(new
            //{
            //    CANDIDATE_NAME = interview.CandidateName,
            //    INTERVIEWER_NAME = interview.InterviewerName,
            //    CANDIDATE_CERT = interview.BusinessComments.Skipped ? "<br/>" : FillExpAndCert(interview, builder),
            //    DATE = DateTime.Now.ToShortDateString(),
            //    INTERVIEW_RESULT = "",
            //    BUSSINESS_COMMENTS = interview.BusinessComments.Skipped ? "<br/>" : FillBusinessComments(interview, builder),
            //    COMMENTS = interview.FinalComments.Skipped ? "<br/>" : FillFinalComments(interview, builder),
            //    GRADE = interview.FinalGrade.Acronym,
            //    ACCEPTED = FillAccepted(interview),
            //});
            //builder.Clear();
            Console.WriteLine(result);
        }

        private string FillAccepted(Interview interview)
        {
            return interview.Accepted ? 
                "<h1 class=\"d-inline float-end\"><span class=\"badge bg-success\">Accepted</span></h1>" : 
                "<h1 class=\"d-inline float-end\"><span class=\"badge bg-warning text-dark\">Rejected</span></h1>";
        }

        private string FillBusinessComments(Interview interview, StringBuilder builder)
        {
            builder.Clear();
            builder.AppendLine("<div class=\"card\">");
            builder.AppendLine("    <h5 class=\"card - header\">Business experience(Transaction systems, B2B, Telecom, etc.)</h5>");
            builder.AppendLine("    <div class=\"card - body\">");
            builder.AppendLine($"       <p class=\"card - text\">{interview.BusinessComments.Input}</p>");
            builder.AppendLine("    </div>");
            builder.AppendLine("</div>");
            return builder.ToString();
        }

        private string FillFinalComments(Interview interview, StringBuilder builder)
        {
            builder.Clear();
            builder.AppendLine("<div class=\"card mt - 3\">");
            builder.AppendLine("    <h5 class=\"card - header\">Comments and Observations</h5>");
            builder.AppendLine("    <div class=\"card - body\">");
            builder.AppendLine($"       <p class=\"card - text\">{interview.FinalComments.Input}</p>");
            builder.AppendLine("    </div>");
            builder.AppendLine("</div>");
            return builder.ToString();
        }
        private string FillExpAndCert(Interview interview, StringBuilder builder)
        {
            builder.Clear();
            builder.AppendLine("<h5 class=\"card - title\">Candidate certificates and overrall experience.</h5>");
            builder.AppendLine($"<h5 class=\"text - primary\">{interview.ExperienceAndCerts}</h5>");
            return builder.ToString();
        }
    }
}
