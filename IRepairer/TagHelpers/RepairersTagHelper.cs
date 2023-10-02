using IRepairer.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace IRepairer.TagHelpers;

[HtmlTargetElement("repairers")]
public class RepairersTagHelper : TagHelper
{
    [HtmlAttributeName("Items")]
    public IEnumerable<RepairerViewModel>? Items { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "tbody";
        var sb = new StringBuilder();

        sb.Append("<thead class=\"thead-dark\">");
        sb.Append("<tr>");
        sb.Append("<th class=\"text-center\" scope=\"col\">UserName</th>");
        sb.Append("<th class=\"text-center\" scope=\"col\">Category</th>");
        sb.Append("<th class=\"text-center\" scope=\"col\">Rating</th>");
        sb.Append("</tr>");
        sb.Append("</thead>");
        sb.Append("<tbody>");

        foreach (RepairerViewModel item in Items!)
            ScreeResult(sb, item);

        sb.Append("</tbody>");

        output.PreContent.SetHtmlContent(sb.ToString());
    }

    private StringBuilder ScreeResult(StringBuilder sb, RepairerViewModel item)
    {
        sb.Append("<tr>");
        sb.AppendFormat("<td class=\"text-center\"> {0} </td>", item.UserName);
        sb.AppendFormat("<td class=\"text-center\"> {0} </td>", item.Category);
        sb.AppendFormat("<td class=\"text-center\"> {0} </td>", item.Rating);
        sb.Append("</tr>");

        return sb;
    }
}