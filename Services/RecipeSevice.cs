using MongoDB.Driver;
using RecipeOrganizer.Models;
using RecipeORganizer.Database;


namespace RecipeOrganizer.Services
{
    public class RecipeService
    {
        private readonly IMongoCollection<Recipe> _recipes;

        public RecipeService()
        {
            var settings = new MongoDBSettings
            {
                ConnectionString = "mongodb://localhost:27017",
                Database = "RecipeDB",
                Collection = "Recipes"
            };

            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.Database);
            _recipes = database.GetCollection<Recipe>(settings.Collection);

            // Създай индекс върху полето Tags
            var indexKeysDefinition = Builders<Recipe>.IndexKeys.Ascending(r => r.Tags);
            _recipes.Indexes.CreateOne(new CreateIndexModel<Recipe>(indexKeysDefinition));
        }

        public List<Recipe> Get() =>
            _recipes.Find(r => true).ToList();

        public Recipe Get(string id) =>
            _recipes.Find(r => r.Id == id).FirstOrDefault();

        public void Create(Recipe recipe) =>
            _recipes.InsertOne(recipe);

        public void Update(string id, Recipe recipeIn) =>
            _recipes.ReplaceOne(r => r.Id == id, recipeIn);

        public void Remove(string id) =>
            _recipes.DeleteOne(r => r.Id == id);

        public List<Recipe> GetByTag(string tag) =>
           _recipes.Find(r => r.Tags.Contains(tag)).ToList();

    }
}
