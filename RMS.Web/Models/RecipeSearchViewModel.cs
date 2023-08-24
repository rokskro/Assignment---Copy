using RMS.Data.Entities;

namespace RMS.Web.Models;

public class RecipeSearchViewModel
{
    public IList<Recipe> Recipes {get; set;} = new List<Recipe>();
    public string Query {get; set;} = "";
    public DiffRange Range { get; set; } = DiffRange.ALL;
}