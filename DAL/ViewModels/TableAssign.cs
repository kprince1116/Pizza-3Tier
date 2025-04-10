namespace Pizzashop.DAL.ViewModels;

public class TableAssign
{
    public List<TableAssignment> SelectedTables { get; set; } = new List<TableAssignment>();

    public CustomerData customerdata { get; set; } = new CustomerData();
}

public class TableAssignment
{
    public int TableId { get; set; }
    public int SectionId { get; set; }
}

public class CustomerData
{
    public string Email { get; set; }
    public string Name { get; set; }
    public string Mobile { get; set; }
    public int NoOfPersons { get; set; }
    public int SectionId { get; set; }
}