using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace RecipeOrganizer.Models
{
    public class Recipe
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null;

        [BsonElement("name")]
        public string Name { get; set; } = null;

        [BsonElement("ingredients")]
        public List<string> Ingredients { get; set; } = null;

        [BsonElement("tags")]
        public List<string> Tags { get; set; } = null;

        [BsonElement("instructions")]
        public string Instructions { get; set; } = null;
    }
}
