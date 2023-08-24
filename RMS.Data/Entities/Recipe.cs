
namespace RMS.Data.Entities;
using System.ComponentModel.DataAnnotations;
using RMS.Data.Validators;

public enum DiffRange {EASY, MED, HARD, ALL}
public class Recipe
{
    public int Id { get; set; }

    
    
    // suitable recipe attributes / relationships
    [Required]
    public string Name {get; set;}

    [Required]
    public string Cuisine {get; set;}

    [Required]
    public string Diet {get; set;}

    [Required]
    [Range(1,3)]
    public int Difficulty {get; set;}

    [Url] 
    [UrlResource]
    public string ImageUrl {get; set;}

    [Required]
    [Range(1,1000)]
    public int CookTime {get; set;}

    [Required]
    public string Ingredients {get; set;}

    [Required]
    public string Instructions {get; set;}

    [Required]
    public string FlavourTxt {get; set;}
    //Read only 
    public string DiffRating => Rating();

    public IList<Review> Reviews {get; set;} = new List<Review>();

    //calculates difficulty rating
    private string Rating(){
        switch (Difficulty){
            case 1:
                return "Easy";   
            case 2:
                return "Medium";
            case 3:
                return "Hard";  
        }
        return null;
    }//rating
 
}//recipe


