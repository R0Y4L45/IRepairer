namespace IRepairer.Models;

public class RepairerListViewModel
{
    public IEnumerable<RepairerViewModel>? Repairers { get; set; }
    public int CurrentCategory { get; set; }
    public int PageCount { get; set; }
    public int CurrentPage { get; set; }
}
