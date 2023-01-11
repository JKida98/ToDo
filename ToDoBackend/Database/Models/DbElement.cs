namespace ToDo.Database.Models;

public class DbElement : BaseModel
{
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime Date { get; set; }
}