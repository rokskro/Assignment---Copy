using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RMS.Data.Entities;
using RMS.Data.Services;
using RMS.Web.Models;

namespace RMS.Web.Controllers;

public class RecipeController : BaseController
{
    // complete
    private IRecipeService svc;

    public RecipeController()
    {
        svc = new RecipeServiceDb();
    }

   // [AllowAnonymous]
   // public IActionResult Index()
    //{
        //var data = svc.GetRecipes();
        //return View(data);
    //}// get recipes

    public IActionResult Index(RecipeSearchViewModel search)
    {
        search.Recipes = svc.SearchRecipes(search.Range, search.Query);
        return View(search);                
    }

    public IActionResult Details(int id)
    {
        var recipe = svc.GetRecipe(id);
       
        if (recipe is null) {
            Alert("Recipe not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }
        return View(recipe);
    }//redirects if recipe not found

    // ============== Recipe management ==============
    [Authorize(Roles="admin")]
    public IActionResult Create()
    {
        return View();
    }//returns form view

    [Authorize(Roles="admin")]
    [HttpPost] [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name, Cuisine, Diet, Difficulty, ImageUrl, CookTime, Ingredients, Instructions, FlavourTxt")] Recipe r)
    {   
        if (svc.GetRecipeByName(r.Name) != null)
        {
            ModelState.AddModelError(nameof(r.Name), "Recipe of that name in use");
        }//validates recipe name

        if (ModelState.IsValid)
        {
            var recipe = svc.AddRecipe(r);
            if (recipe is null) 
            {
                Alert("Issue creating recipe", AlertType.warning);
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Details), new { Id = recipe.Id});   
        }
        
        // redisplay the form for editing as there are validation errors
        return View(r);
    }//creation action

    [Authorize(Roles="admin")]
     public IActionResult Edit(int id)
    {
        var recipe = svc.GetRecipe(id);

        if (recipe is null)
        {
            Alert("Recipe not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }  
        return View(recipe);
    }//edit recipe action

    [Authorize(Roles="admin")]
    [HttpPost] [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Recipe r)
    {
        var existing = svc.GetRecipeByName(r.Name);
        if (existing != null && r.Id != existing.Id) 
        {
           ModelState.AddModelError(nameof(r.Name), "Recipe of that name in use");
        } 

        if (ModelState.IsValid)
        {            
            var recipe = svc.UpdateRecipe(r);
            if (recipe is null) 
            {
                Alert("Issue updating recipe", AlertType.warning);
            }
            return RedirectToAction(nameof(Details), new { Id = r.Id });
        }
        // redisplay the form for editing as validation errors
        return View(r);
    }//edit forgery check action

    [Authorize(Roles="admin")]
    public IActionResult Delete(int id)
    {
        var recipe = svc.GetRecipe(id);
        if (recipe == null)
        {
            Alert("Recipe not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }
        return View(recipe);
    }//delete recipe action

    [Authorize(Roles="admin")]
    [HttpPost] [ValidateAntiForgeryToken]   
    public IActionResult DeleteConfirm(int id)
    {
        // delete student via service
        var deleted = svc.DeleteRecipe(id);
        if (deleted)
        {
            Alert("Poof! Recipe deleted", AlertType.success);            
        }
        else
        {
            Alert("Uh oh, Recipe could not be deleted", AlertType.warning);           
        }
        
        // redirect to the index view
        return RedirectToAction(nameof(Index));
    }//delete action

    // ============== Review management ==============

    [AllowAnonymous]
    public IActionResult ReviewCreate(int id)
    {
        var recipe = svc.GetRecipe(id);
        if (recipe == null)
        {
            Alert("Recipe not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }

        var review = new Review { RecipeId = id }; 
        // render blank form
        return View( review );
    }//review creation action 

    [AllowAnonymous]
    [HttpPost][ValidateAntiForgeryToken]
    public IActionResult ReviewCreate([Bind("RecipeId, Author, Comment, Rating")] Review rev)
    {
        if (ModelState.IsValid)
        {                
            var review = svc.CreateReview(rev.RecipeId, rev.Author, rev.Comment, rev.Rating);
            Alert($"Review successfully created for the {rev.RecipeId} recipe!", AlertType.info);            
            return RedirectToAction(
                nameof(Details), new { Id = review.RecipeId }
            );
        }
        // redisplay the form for editing
        return View(rev);
    }//review creation forgery action

    [Authorize(Roles="admin")]
    public IActionResult ReviewEdit(int id)
    {
        var review = svc.GetReview(id);
        if (review == null)
        {
            Alert($"Oh no! Review {id} not found", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }        
        return View( review );
    }//review edit action 

    [Authorize(Roles="admin")]
    [HttpPost] [ValidateAntiForgeryToken]
    public IActionResult ReviewEdit(int id, [Bind("RecipeId, Author, Comment, Rating")] Review rev)
    {
        if (ModelState.IsValid)
        {                
            var review = svc.UpdateReview(id, rev.Author, rev.Comment, rev.Rating);
            return RedirectToAction(
                nameof(Details), new { Id = review.RecipeId}
            );
        }
        // redisplay the form for editing
        return View(rev);
    }//review edit anti forgery action

    [Authorize(Roles="admin")]
    public IActionResult ReviewDelete(int id)
    {
        var review = svc.GetReview(id);
        if (review == null)
        {
            Alert("Oh no! Review not found", AlertType.warning);
            return RedirectToAction(nameof(Index));;
        }     
        // pass to view for deletion confirmation
        return View(review);
    }//delete action

    [Authorize(Roles="admin")]
    [HttpPost][ValidateAntiForgeryToken]
    public IActionResult ReviewDeleteConfirm(int id, int recipeId)
    {
        var deleted = svc.DeleteReview(id);
        if (deleted)
        {
            Alert("Review deleted", AlertType.success);            
        }
        else
        {
            Alert("Whoops, Review could not be deleted", AlertType.warning);           
        }
        return RedirectToAction(nameof(Details), new { Id = recipeId });
    }//anti forgery delete review action

public IActionResult GetReviews()
{
    var revs = svc.GetAllReviews();
    if (revs == null)
    {
        Alert("No reviews found", AlertType.warning);
        return NotFound();
    }
    return View(revs);
}   

}
