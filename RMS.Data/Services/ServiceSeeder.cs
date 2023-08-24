using RMS.Data.Entities;
namespace RMS.Data.Services;

public static class ServiceSeeder
{


    public static void Seed(){
        IRecipeService recipeSvc = new RecipeServiceDb();
        IUserService userSvc = new UserServiceDb();


        SeedRecipes(recipeSvc);
        SeedUsers(userSvc);
    }//seed
    
    // use this method FIRST to seed the database with dummy test data using an IUserService
    public static void SeedUsers(IUserService svc)
    {
        svc.Initialise();


        //svc.Register("admin","admin@mail.com","password",Role.admin);
        svc.Register("Admin", "admin@mail.com", "testpass", Role.admin);
       
    }
    
    // use this method SECOND to seed the database with dummy test data using an IRecipeService
    public static void SeedRecipes(IRecipeService svc)
    {        
        // Note: do not call initialise again

        // add relevant seed data 
        var recipe1 = svc.AddRecipe( new Recipe {
            Name = "Spicy Miso Ramen", Cuisine = "Japanese", Diet = "Vegetarian", Difficulty = 1, ImageUrl = "https://inquiringchef.com/wp-content/uploads/2022/11/Easy-Miso-Ramen_square-0723.jpg",
            CookTime = 40, Ingredients = "1.8oz Chopped Tofu, 8oz Shitake mushrooms, Ramen noodles, Miso Stock, Green Onions, 2Tbsp Chili oil, Dried Seaweed", 
            Instructions = "Cook Noodles until done, Cook mushrooms and tofu in broth, Add in green onions and oil, Pour on top of set noodles, Garnish with seaweed",
            FlavourTxt = "A spicy twist on a usually mild combination of miso soup and ramen"
        }); 
        var recipe2 = svc.AddRecipe( new Recipe {
            Name = "Radish Oysters", Cuisine = "Korean", Diet = "Pescatarian", Difficulty = 2, ImageUrl = "https://www.deliciousmagazine.co.uk/wp-content/uploads/2021/07/960_OYSTERS_ICE_DONGCHIMI-768x960.jpg",
            CookTime = 50, Ingredients = "Oysters, Smashed Garlic, 50g Ginger, 1 Yuzu/Lemon, 1Kg Radishes, 60g Salt, Chives, 1 Carrot, Ice", 
            Instructions ="Combine the garlic, ginger, yuzu zest, chives, pears with water in a blender, Strain liquid over vegetables, Store for 25 min in Freezer, Serve with fresh oysters over ice",
            FlavourTxt = "Korean inspired sauce from carrots and radishes served on fresh oysters"
        }); 
        var recipe3 = svc.AddRecipe( new Recipe {
            Name = "Linseed Baguettes", Cuisine = "Baking", Diet = "Vegan", Difficulty = 3, ImageUrl = "https://www.deliciousmagazine.co.uk/wp-content/uploads/2022/07/960-Linseed_Baguettes_Reshoot_06-768x960.jpg",
            CookTime = 60, Ingredients = "100g Wholmeal Flour, 375g White bread flour, 15g Sea salt, 100g Soaked golden linseeds, 135g Sourdough starter", 
            Instructions ="Mix the flours with 300g of warm water, Set aside for 2-4 hours, Combine starter with dough and set aside for 30 min, Combine dough with salt and linseeds, Let dough rest and fold every hour for 4 hours, Roll into baguette form, Bake on high heat",
            FlavourTxt = "Crunchy on the outside but chewy on inside linseed baguettes"
        });
        var recipe4 = svc.AddRecipe( new Recipe {
            Name = "Tuna Poke Bowl", Cuisine = "Hawaiian", Diet = "Pescatarian", Difficulty = 1, ImageUrl = "https://thewoodenskillet.com/wp-content/uploads/2021/08/tuna-poke-1-600x600.jpg",
            CookTime = 20, Ingredients = "White Rice, 1lb Sushi grade tuna, 1 Avocado, 50g Radishes, 1 Cucumber, Sesame seeds, 1 Carrot, Edamane beans", 
            Instructions ="Prepare rice, Dice tuna into bite sized pieces, Slice vegetables, Arrange meat and vegetables ontop of rice in bowl, Sprinke sesame seeds",
            FlavourTxt = "Easy and fresh feeling deconstructed sushi for a quick meal"
        });


        //seeding reviews
        var r1 = svc.CreateReview(recipe1.Id, "Yae Miko", "Very nice", 4);
        var r2 = svc.CreateReview(recipe1.Id, "Raiden Shogun", "Not Dango Milk", 1);

        var r3 = svc.CreateReview(recipe2.Id, "Itto", "Very spicy bro, 10/10", 5);


        


    }//seed recipes
}

