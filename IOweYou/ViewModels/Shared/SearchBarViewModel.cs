namespace IOweYou.Models.Shared;

public class SearchBarViewModel
{
    public string Id { get; set; }
    public string Link { get; set; }
    public bool SendOnClick { get; set; } = true;
    public bool ShowMyself { get; set; } = false;
}