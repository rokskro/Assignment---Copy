using Microsoft.EntityFrameworkCore;
using RMS.Data.Entities;
using RMS.Data.Repository;
using static RMS.Data.Entities.Recipe;

namespace RMS.Data.Services;

// EntityFramework Implementation of IRecipeService
public class RecipeServiceDb : IRecipeService
{
    private readonly DataContext db;

    public RecipeServiceDb()
    {
        db = new DataContext();
    }

    public void Initialise()
    {
        db.Initialise(); // recreate database
    }

    // ==================== Recipe Management ==================
    
    // implement IRecipeService methods here
    public List<Recipe> SearchRecipes(DiffRange range, string searchString)
    {
        searchString = searchString == null ? "" : searchString.ToLower();

        var recipes = db.Recipes.Where(r => r.Name.ToLower().Contains(searchString) ||
                                            r.Cuisine.ToLower().Contains(searchString) ||
                                            r.Diet.ToLower().Contains(searchString) ||
                                            r.FlavourTxt.ToLower().Contains(searchString)  //&&
                                            //(range == DiffRange.EASY && r.Difficulty == 1 ||
                                            //range == DiffRange.MED && r.Difficulty == 2 ||
                                            //range == DiffRange.HARD && r.Difficulty == 3 ||
                                            //range == DiffRange.ALL)//
                                            )
                                .ToList();
        return recipes;
    }

    public List<Recipe> GetRecipes(){
        return db.Recipes.ToList();
    }//get all recipes

    public Recipe GetRecipe(int id){
        return db.Recipes
                .Include(r => r.Reviews)
                .FirstOrDefault(r => r.Id == id);
    }//get recipe

    public Recipe AddRecipe(Recipe r){
        //checking if recipe exists
        var exists = GetRecipeByName(r.Name);
        
        if (exists != null){
            return null;
        }//if
        //checks if rating is valid
        if (r.Difficulty <= 0 || r.Difficulty > 3){
            return null;
        }//if

        //create new recipe
        var recipe = new Recipe{
            Name = r.Name,
            Cuisine = r.Cuisine,
            Diet = r.Diet,
            Difficulty = r.Difficulty,
            ImageUrl = r.ImageUrl,
            CookTime = r.CookTime,
            Ingredients = r.Ingredients,
            Instructions = r.Instructions,
            FlavourTxt = r.FlavourTxt
        }; //recipe
        db.Recipes.Add(recipe); //adds
        db.SaveChanges(); //saves
        return recipe; // returns recipe
    }//add recipe

    public bool DeleteRecipe(int id){
        var r = GetRecipe(id);
        if (r == null){
            return false;
        }//if
       db.Recipes.Remove(r);
       db.SaveChanges();
       return true;
    }//deletes recipe

    public Recipe UpdateRecipe(Recipe updated)
    {
        //verify recipe existence 
        var recipe = GetRecipe(updated.Id);
        if (recipe == null){
            return null;
        }//if

        //verifies uniqueness
        var exists = GetRecipeByName(updated.Name);
        if (exists != null && exists.Id != updated.Id){
            return null;
        }//if

        //verfies valid difficulty
        if (updated.Difficulty <= 0 || updated.Difficulty > 3){
            return null;
        }//if

        // updates recipe
        recipe.Name = updated.Name;
        recipe.Cuisine = updated.Cuisine;
        recipe.Diet = updated.Diet;
        recipe.Difficulty = updated.Difficulty;
        recipe.ImageUrl = updated.ImageUrl;
        recipe.CookTime = updated.CookTime;
        recipe.Ingredients = updated.Ingredients;
        recipe.Instructions = updated.Instructions;
        recipe.FlavourTxt = updated.FlavourTxt;

        db.SaveChanges();
        return recipe;
    }//updates recipe

    public Recipe GetRecipeByName(string name){
        return db.Recipes.FirstOrDefault(r => r.Name == name);
    }

    // ===================== Review Management ==========================

    public Review CreateReview(int recipeId, string author, string comment, double rating){
        var recipe = GetRecipe(recipeId);
        if(recipe == null) return null;

    //checks if rating is valid
        if(rating < 0 || rating > 5){
            return null;
        }//if

        var review = new Review{
            Author = author,
            RecipeId = recipeId,
            Comment = comment,
            Rating = rating,
            Date = DateTime.Now.ToLongDateString(),
       
        }; //creates review
        db.Reviews.Add(review);
        db.SaveChanges();
        return review;
    }//create review 

    public Review GetReview(int id){
        return db.Reviews
                    .Include(re => re.Recipe)
                    .FirstOrDefault(re => re.ReviewID == id);
    }//gets review 

    public Review UpdateReview(int id, string author, string comment, double rating){
        var review = GetReview(id);
        if (review == null) return null;
        if(rating < 0 || rating > 5){
            return null;
        }//if

        review.Author = author;
        review.Comment = comment;
        review.Rating = rating;
                                   
        db.Reviews.Update(review);
        db.SaveChanges(); 
        return review;
    }//updates review

    public bool DeleteReview(int id){
        var review = GetReview(id);
        if (review == null) return false;
        
        var result = db.Reviews.Remove(review);
        db.SaveChanges();
        return true;
    }//deletes review
  


    public IList<Review> GetAllReviews(){
        return db.Reviews.Include(re => re.Recipe).ToList();
    } //retrieves all reviews attached to a recipe

    
    




}//recipe services