using System.ComponentModel.DataAnnotations;

namespace RMS.Data.Entities;

public class Review
{
    public int ReviewID { get; set; }
        
    // suitable review attributes / relationships

    //Author name
    [Required]
    [StringLength(25, MinimumLength = 2)]
    public string Author {get; set;}

    //date of post in longdate format
    public string Date {get; set;} = DateTime.Now.ToLongDateString();
    
    
    //review comment
    [StringLength(150, MinimumLength = 5)]
    public string Comment {get; set;}

    //review rating
    [Required]
    [Range(0,5)]
    public double Rating {get; set;}

    public int RecipeId {get; set;} //foreign key
    public Recipe Recipe {get; set;} //nav property


  
}//review