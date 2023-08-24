using System;
using System.Collections.Generic;
	
using RMS.Data.Entities;
	
namespace RMS.Data.Services;

// This interface describes the operations that a RecipeService class should implement
public interface IRecipeService
{
    void Initialise();
        
    // add suitable method definitions to implement assignment requirements            
    
    // ---------------- Recipe Management --------------
    List<Recipe> GetRecipes();
    Recipe GetRecipe(int id);
    Recipe GetRecipeByName(string name);
    Recipe AddRecipe(Recipe r);
    Recipe UpdateRecipe(Recipe updated);
    bool DeleteRecipe(int id);
    List <Recipe> SearchRecipes(DiffRange range, string searchString);

    // ---------------- Review Management --------------
    Review CreateReview(int recipeId, string author, string comment, double rating);
    Review UpdateReview(int id, string author, string comment, double rating);
    Review GetReview(int id);
    bool DeleteReview(int id);
    IList<Review> GetAllReviews();

}
    
