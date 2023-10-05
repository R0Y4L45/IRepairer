using IRepairer.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace IRepairer.TagHelpers;

[HtmlTargetElement("repairers")]
public class RepairersTagHelper : TagHelper
{
    [HtmlAttributeName("Items")]
    public IEnumerable<RepairerViewModel>? Items { get; set; }

    [HtmlAttributeName("Role")]
    public bool Role { get; set; } = default;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "tbody";
        var sb = new StringBuilder();

        sb.Append("<thead class=\"thead-dark\">");
        sb.Append("<tr>");
        sb.Append("<th class=\"text-center\" scope=\"col\">Photo</th>");
        sb.Append("<th class=\"text-center\" scope=\"col\">UserName</th>");
        sb.Append("<th class=\"text-center\" scope=\"col\">Category</th>");
        sb.Append("<th class=\"text-center\" scope=\"col\">Rating</th>");
        sb.Append("<th class=\"text-center\" scope=\"col\">Action</th>");
        sb.Append("</tr>");
        sb.Append("</thead>");
        sb.Append("<tbody>");

        foreach (RepairerViewModel item in Items!)
        {
            if (Role)
                ScreeResultAdmin(sb, item);
            else
                ScreeResultUser(sb, item);
        }
        sb.Append("</tbody>");

        output.PreContent.SetHtmlContent(sb.ToString());
    }

    private StringBuilder ScreeResultAdmin(StringBuilder sb, RepairerViewModel item)
    {
        sb.Append("<tr>");
        sb.AppendFormat("<td class=\"text-center\"><img style='width:64px;' class=\"rounded-circle\" src=\"0\" onerror=\"if (this.src != 'https://img.icons8.com/external-wanicon-lineal-color-wanicon/64/external-avatar-professions-avatar-wanicon-lineal-color-wanicon-3.png') this.src = 'https://img.icons8.com/external-wanicon-lineal-color-wanicon/64/external-avatar-professions-avatar-wanicon-lineal-color-wanicon-3.png';\" alt=\"No Photo\"></td>", item.Photo);
        sb.AppendFormat("<td class=\"text-center\"> {0} </td>", item.UserName);
        sb.AppendFormat("<td class=\"text-center\"> {0} </td>", item.Category);
        sb.AppendFormat("<td class=\"text-center\"> {0} </td>", item.Rating);
        sb.AppendFormat("<td class=\"text-center\"><ul class=\"list-inline mb-0\"><li class=\"list-inline-item dropdown\"><a class=\"text-muted dropdown-toggle font-size-18 px-2\" href=\"#\" role=\"button\" data-bs-toggle=\"dropdown\" aria-haspopup=\"true\"><i class=\"bx bx-dots-vertical-rounded\"></i></a><div class=\"dropdown-menu dropdown-menu-end\"><a class=\"dropdown-item link-danger\" href=\"/Admin/Admin/DeleteRepairer/{0}\"> Delete </a></div></li></ul></td>", item.Id);
        sb.Append("</tr>");

        return sb;
    }

    private StringBuilder ScreeResultUser(StringBuilder sb, RepairerViewModel item)
    {
        sb.Append("<tr>");
        sb.AppendFormat("<td class=\"text-center\"><img style='width:64px;' class=\"rounded-circle\" src=\"0\" onerror=\"if (this.src != 'https://img.icons8.com/external-wanicon-lineal-color-wanicon/64/external-avatar-professions-avatar-wanicon-lineal-color-wanicon-3.png') this.src = 'https://img.icons8.com/external-wanicon-lineal-color-wanicon/64/external-avatar-professions-avatar-wanicon-lineal-color-wanicon-3.png';\" alt=\"No Photo\"></td>", item.Photo);
        sb.AppendFormat("<td class=\"text-center\"> {0} </td>", item.UserName);
        sb.AppendFormat("<td class=\"text-center\"> {0} </td>", item.Category);
        sb.AppendFormat("<td class=\"text-center\"> {0} </td>", item.Rating);
        sb.AppendFormat("<td class=\"text-center\"><ul class=\"list-inline mb-0\"><li class=\"list-inline-item dropdown\"><a class=\"text-muted dropdown-toggle font-size-18 px-2\" href=\"#\" role=\"button\" data-bs-toggle=\"dropdown\" aria-haspopup=\"true\"><i class=\"bx bx-dots-vertical-rounded\"></i></a><div class=\"dropdown-menu dropdown-menu-end\"><a class=\"dropdown-item link-danger\" href=\"/User/User/Message/{0}\"> Message </a></div></li></ul></td>", item.Id);
        sb.Append("</tr>");

        return sb;
    }
}