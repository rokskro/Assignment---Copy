
using Xunit;
using RMS.Data.Entities;
using RMS.Data.Services;

namespace RMS.Test;

// ==================== RecipeService Tests =============================
[Collection("Sequential")]
public class RecipeServiceTests
{
    private readonly IRecipeService svc;

    public RecipeServiceTests()
    {
        // general arrangement
        svc = new RecipeServiceDb();
        
        // ensure data source is empty before each test
        svc.Initialise();
    }

    // ========================== Get All Recipe Tests  =========================
    [Fact] 
    public void GetAllRecipes_WhenNoneExist_ShouldReturn0()
    {
        // act 
        var recipes = svc.GetRecipes();
        var count = recipes.Count;
        // assert
        Assert.Equal(0, count);
    }//get all recipes

    [Fact]
    public void GetRecipes_With2Added_ShouldReturnCount2()
    {
        // arrange       
        var r1 = svc.AddRecipe(
            new Recipe { Name="Mac & Cheese", Cuisine = "American", Diet = "Vegetarian", Difficulty = 1, CookTime = 30, Ingredients = "cheese, macaroni", Instructions = "combine to make mac", FlavourTxt = "cheesey goodness"});
        var r2 = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2 , CookTime = 20, Ingredients = "Pork, Panko breadcumbs", Instructions = "Cover seasoned meat in panko, fry on pan, serve with rice", FlavourTxt = "Crispy breaded pork cutlets"});       
        // act
        var recipes = svc.GetRecipes();
        var count = recipes.Count;
        // assert
        Assert.Equal(2, count);
    }//return 2 if 2

    // ========================== Get Single Recipe Tests  =========================
    [Fact] 
    public void GetRecipe_WhenNoneExist_ShouldReturnNull()
    {
        // act 
        var recipe = svc.GetRecipe(1); // non existent 
        // assert
        Assert.Null(recipe);
    }// if 0 return null

    [Fact] 
    public void GetRecipe_WhenAdded_ShouldReturnRecipe()
    {
        // arrange 
         var r = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2, CookTime = 20, Ingredients = "Pork, Panko breadcumbs", Instructions = "Cover seasoned meat in panko, fry on pan, serve with rice", FlavourTxt = "Crispy breaded pork cutlets" });
        // act
        var nr = svc.GetRecipe(r.Id);
        // assert
        Assert.NotNull(nr);
        Assert.Equal(r.Id, nr.Id);
    }//when added, return

    [Fact] 
    public void GetRecipe_WithReviews_RetrievesRecipeAndReviews()
    {
        // arrange
        var r = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2, CookTime = 20, Ingredients = "Pork, Panko breadcumbs", Instructions = "Cover seasoned meat in panko, fry on pan, serve with rice", FlavourTxt = "Crispy breaded pork cutlets" });
        svc.CreateReview(r.Id, "Bot", "Does this work?", 3);  
        // act      
        var nr = svc.GetRecipe(r.Id);
        // assert
        Assert.NotNull(nr);
        Assert.Equal(1, nr.Reviews.Count);
    } //retieve recipe + reviews

    [Fact] 
    public void GetRecipeByName_WhenAdded_ShouldReturnRecipe()
    {
        var r = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2, CookTime = 20, Ingredients = "Pork, Panko breadcumbs", Instructions = "Cover seasoned meat in panko, fry on pan, serve with rice", FlavourTxt = "Crispy breaded pork cutlets" });
        // act
        var nr = svc.GetRecipeByName("Tonkatsu");
        // assert
        Assert.NotNull(nr);
        Assert.Equal(r.Name, nr.Name);
    }//get recipe by name

    // ========================== Add Recipe Tests  =========================
    [Fact]
    public void AddRecipe_WhenValid_ShouldAddRecipe()
    {
        // arrange 
        var added = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2, CookTime = 20, Ingredients = "Pork, Panko breadcumbs", Instructions = "Dont burn it", FlavourTxt = "Crispy breaded pork cutlets" });
        // act 
        var r = svc.GetRecipe(added.Id);
        // assert
        Assert.NotNull(r);

        Assert.Equal(r.Id, r.Id);
        Assert.Equal("Tonkatsu", r.Name);
        Assert.Equal("Japanese", r.Cuisine);
        Assert.Equal("Whole", r.Diet);
        Assert.Equal(2, r.Difficulty);
        Assert.Equal(20, r.CookTime);
        Assert.Equal("Pork, Panko breadcumbs", r.Ingredients);
        Assert.Equal("Dont burn it", r.Instructions);
        Assert.Equal("Crispy breaded pork cutlets", r.FlavourTxt);

    }//valid entries produce recipe

    [Fact] 
    public void AddRecipe_WhenDuplicateName_ShouldReturnNull()
    {
        // arrange
        var r1 = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2, CookTime = 20, Ingredients = "Pork, Panko breadcumbs", Instructions = "Dont burn it", FlavourTxt = "Crispy breaded pork cutlets" });
        // act 
        var r2 = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Korean", Diet = "Whole", Difficulty = 1, CookTime = 20, Ingredients = "Pork,", Instructions = "Dont burn it", FlavourTxt = "Crispy pork cutlets" });
        // assert
        Assert.NotNull(r1);
        Assert.Null(r2);       
    }//duplication test

    [Fact] 
    public void InavlidDifficultyReturnNull()
    {
        var added = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Korean", Diet = "Whole", Difficulty = 4, CookTime = 20, Ingredients = "Pork, Panko breadcumbs", Instructions = "Dont burn it", FlavourTxt = "Crispy breaded pork cutlets" });
        // assert
        Assert.Null(added);
    }//invalid difficulty test

    // ========================== Update Recipe Tests  =========================
    [Fact]
    public void UpdateRecipe_ThatExists_ShouldSetAllProperties()
    {
        // arrange        
        var r = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2, CookTime = 20, Ingredients = "Pork, Panko breadcumbs", Instructions = "Dont burn it", FlavourTxt = "Crispy breaded pork cutlets" });

        // act 
        var u = svc.UpdateRecipe(           
            new Recipe {
                Id = r.Id,
                Name = "Vegan Tonkatsu",
                Cuisine = "Japanese",
                Diet = "Vegan",
                Difficulty = 3,
                CookTime = 30,
                Ingredients = "Flattened oyster mushrooms",
                Instructions = "bread and fry mushrooms",
                FlavourTxt = "Crispy breaded vegan tonkatsu"
            }
        ); 

        // reload updated recipe
        var ur = svc.GetRecipe(r.Id);
        // assert
        Assert.NotNull(ur);           

        // now assert that the properties were set properly           
        Assert.Equal(u.Name, ur.Name);
        Assert.Equal(u.Cuisine, ur.Cuisine);
        Assert.Equal(u.Diet, ur.Diet);
        Assert.Equal(u.Difficulty, ur.Difficulty);
        Assert.Equal(u.CookTime, ur.CookTime);
        Assert.Equal(u.Ingredients, ur.Ingredients);
        Assert.Equal(u.Instructions, ur.Instructions);
        Assert.Equal(u.FlavourTxt, ur.FlavourTxt);

    }//tests recipe updates

    [Fact] 
    public void UpdateRecipe_InvalidDifficulty_ReturnNull()
    {
        // arrange       
        var r = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2, CookTime = 20, Ingredients = "Pork,", Instructions = "Dont burn it", FlavourTxt = "Crispy pork cutlets" });
        // update
        var updated = svc.UpdateRecipe(
            new Recipe { Id = r.Id, Name = r.Name, Cuisine = r.Cuisine, Diet = r.Diet,  Difficulty = 4, CookTime = r.CookTime, Ingredients = r.Ingredients, Instructions = r.Instructions, FlavourTxt = r.FlavourTxt }    
        );
        
        // assert
        Assert.NotNull(r);
        Assert.Null(updated);
    }//update with invalid difficulty test

    [Fact]
    public void UpdateRecipe_DuplicateName_ReturnNull()
    {
        // arrange
        var r1 = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2, CookTime = 20, Ingredients = "Pork,", Instructions = "Dont burn it", FlavourTxt = "Crispy pork cutlets" });
        var r2 = svc.AddRecipe(
            new Recipe { Name="Mac & Cheese", Cuisine = "American", Diet = "Vegetarian", Difficulty = 1, CookTime = 20, Ingredients = "cheese", Instructions = "Cook it", FlavourTxt = "It's cheese" });
        // update r2 Name with duplicate of r1 name
        var updated = svc.UpdateRecipe(
            new Recipe { Name = r1.Name, Id = r2.Id, Cuisine = r2.Cuisine, Diet = r2.Diet, Difficulty = r2.Difficulty, CookTime = r2.CookTime, Ingredients = r2.Ingredients, Instructions = r2.Instructions, FlavourTxt = r2.FlavourTxt }    
        );
        
        // assert
        Assert.NotNull(r1);
        Assert.NotNull(r2);
        Assert.Null(updated);
    }//duplicate name returns null

    // ========================== Delete Recipe Tests  =========================
    [Fact]
    public void DeleteExistingRecipe_ShouldReturnTrue()
    {
        // arrange 
        var r = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2 , CookTime = 20, Ingredients = "Pork,", Instructions = "Dont burn it", FlavourTxt = "Crispy pork cutlets"});
        // act
        var deleted = svc.DeleteRecipe(r.Id);

        // attempt retrieval
        var attempt = svc.GetRecipe(r.Id);

        // assert
        Assert.True(deleted); 
        Assert.Null(attempt);      
    }//delete test

    [Fact]
    public void DeleteNonexistentRecipe_ReturnFalse()
    {
        // act 	
        var deleted = svc.DeleteRecipe(0);
        // assert
        Assert.False(deleted);
    }//delete nonexistent recipe test


    

    // ========================== Review Tests  =========================
    [Fact] 
    public void CreateReview_ForExistingRecipe_ShouldBeCreated()
    {
        var r = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2, CookTime = 20, Ingredients = "Pork,", Instructions = "Dont burn it", FlavourTxt = "Crispy pork cutlets" });
        // act
        var rev =svc.CreateReview(r.Id, "Bot", "Does this work?", 3); 
        
        // assert
        Assert.NotNull(rev);
        Assert.Equal(r.Id, rev.RecipeId);
    }//review creation for exisitng recipe test

    [Fact] 
    public void GetReview_WhenExists_ShouldReturnReviewAndRecipe()
    {
        var r = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2, CookTime = 20, Ingredients = "Pork,", Instructions = "Dont burn it", FlavourTxt = "Crispy pork cutlets"});
        // act
        var rev =svc.CreateReview(r.Id, "Bot", "Does this work?", 3);

        // act
        var dummy = svc.GetReview(rev.ReviewID);

        // assert
        Assert.NotNull(dummy);
        Assert.NotNull(dummy.Recipe);
        Assert.Equal(r.Name, dummy.Recipe.Name); 
    }//return recipe name & 

    [Fact] // --- GetOpenTickets When two added should return two 
    public void GetRecipes_WhenTwoAdded_ShouldReturnTwo()
    {
        var r = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2, CookTime = 20, Ingredients = "Pork,", Instructions = "Dont burn it", FlavourTxt = "Crispy pork cutlets" });
        // act
        var rev1 = svc.CreateReview(r.Id, "Bot", "Does this work?", 3);
        var rev2 = svc.CreateReview(r.Id, "Bot-Chan", "Very tasty!", 5);
        // act
        var all = svc.GetAllReviews();
        // assert
        Assert.Equal(2,all.Count);                        
    }// return 2 if 2 exist

    [Fact] 
    public void DeleteReview_WhenExists_ShouldReturnTrue()
    {
        var r = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2, CookTime = 20, Ingredients = "Pork,", Instructions = "Dont burn it", FlavourTxt = "Crispy pork cutlets" });
        var rev1 = svc.CreateReview(r.Id, "Bot", "Does this work?", 3);
        // act
        var deleted = svc.DeleteReview(r.Id);       
        // assert
        Assert.True(deleted);                    
    }  //delete review if it exists test
 

    [Fact] 
    public void DeleteReview_WhenNonExistant_ShouldReturnFalse()
    {
        // act
        var deleted = svc.DeleteReview(1);         
        // assert
        Assert.False(deleted);                  
    }//delete

    [Fact] 
    public void DeleteReview_WhenValid_ShouldBeRemovedFromRecipe()
    {
        var r = svc.AddRecipe(
            new Recipe { Name="Tonkatsu", Cuisine = "Japanese", Diet = "Whole", Difficulty = 2, CookTime = 20, Ingredients = "Pork,", Instructions = "Dont burn it", FlavourTxt = "Crispy pork cutlets" });
        var rev = svc.CreateReview(r.Id, "Bot", "Does this work?", 3);
        // act
        svc.DeleteReview(rev.ReviewID);        // delete the ticket
        var nr = svc.GetRecipe(r.Id); // reload the student

        // assert
        Assert.NotNull(nr);
        Assert.Equal(0, nr.Reviews.Count);
    }//delete review when none exists

}

// ==================== UserService Tests =============================
[Collection("Sequential")]
public class UserServiceTests
{
    private readonly IUserService svc;

    public UserServiceTests()
    {
        // general arrangement
        svc = new UserServiceDb();
        
        // ensure data source is empty before each test
        svc.Initialise();
    }

    // ========================== User Tests =========================

    [Fact] // --- Register Valid User test
    public void User_Register_WhenValid_ShouldReturnUser()
    {
        // arrange 
        var reg = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
        
        // act
        var user = svc.GetUserByEmail(reg.Email);
        
        // assert
        Assert.NotNull(reg);
        Assert.NotNull(user);
    } 

    [Fact] // --- Register Duplicate Test
    public void User_Register_WhenDuplicateEmail_ShouldReturnNull()
    {
        // arrange 
        var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
        
        // act
        var s2 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);

        // assert
        Assert.NotNull(s1);
        Assert.Null(s2);
    } 

    [Fact] // --- Authenticate Invalid Test
    public void User_Authenticate_WhenInValidCredentials_ShouldReturnNull()
    {
        // arrange 
        var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
    
        // act
        var user = svc.Authenticate("xxx@email.com", "guest");
        // assert
        Assert.Null(user);

    } 

    [Fact] // --- Authenticate Valid Test
    public void User_Authenticate_WhenValidCredentials_ShouldReturnUser()
    {
        // arrange 
        var s1 = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
    
        // act
        var user = svc.Authenticate("xxx@email.com", "admin");
        
        // assert
        Assert.NotNull(user);
    } 
    
    [Fact]
    public void User_GetUser_WhenExists_ShouldReturnUser()
    {
        var u = svc.Register("XXX", "xxx@email.com", "admin", Role.admin);
        var user = svc.GetUser(u.Id);

        Assert.NotNull(user);
    }

    [Fact] 
    public void User_GetUser_WhenDoesntExist_ShouldReturnNull()
    {
        var user = svc.GetUser(1);
        Assert.Null(user);
    } 
 
}
