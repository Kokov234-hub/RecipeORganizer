using RecipeOrganizer.Models;
using RecipeOrganizer.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            RecipeService recipeService = new RecipeService();
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("== Recipe Organizer ==");
                Console.WriteLine("1. Добави нова рецепта");
                Console.WriteLine("2. Покажи всички рецепти");
                Console.WriteLine("3. Търси рецепти по таг");
                Console.WriteLine("4. Обнови рецепта");
                Console.WriteLine("5. Изтрий рецепта");
                Console.WriteLine("0. Изход");
                Console.Write("Избор: ");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddRecipe(recipeService);
                        break;
                    case "2":
                        ShowAllRecipes(recipeService);
                        break;
                    case "3":
                        SearchByTag(recipeService);
                        break;
                    case "4":
                        UpdateRecipe(recipeService);
                        break;
                    case "5":
                        DeleteRecipe(recipeService);
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Невалиден избор...");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("\nНатисни Enter за продължение...");
                    Console.ReadLine();
                }
            }
        }

        static void AddRecipe(RecipeService service)
        {
            Console.WriteLine("\n== Нова рецепта ==");

            Console.Write("Име: ");
            string name = Console.ReadLine();

            Console.Write("Съставки (разделени със запетая): ");
            var ingredients = Console.ReadLine()
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim()).ToList();

            Console.Write("Тагове (разделени със запетая): ");
            var tags = Console.ReadLine()
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim()).ToList();

            Console.Write("Инструкции: ");
            string instructions = Console.ReadLine();

            if (service.Get().Any(r => r.Name == name))
            {
                Console.WriteLine("Рецепта с това име вече съществува.");
                return;
            }

            Recipe recipe = new Recipe
            {
                Name = name,
                Ingredients = ingredients,
                Tags = tags,
                Instructions = instructions
            };

            service.Create(recipe);
            Console.WriteLine("Рецептата е запазена.");
        }

        static void ShowAllRecipes(RecipeService service)
        {
            var recipes = service.Get();
            Console.WriteLine("\n== Всички рецепти ==");

            foreach (var recipe in recipes)
            {
                Console.WriteLine($"- {recipe.Name} | Съставки: {string.Join(", ", recipe.Ingredients)} | Тагове: {string.Join(", ", recipe.Tags)}");
            }
        }

        static void SearchByTag(RecipeService service)
        {
            Console.Write("\nВъведи таг за търсене: ");
            string tag = Console.ReadLine();

            var recipes = service.GetByTag(tag);

            Console.WriteLine($"\n== Рецепти с таг '{tag}' ==");
            foreach (var recipe in recipes)
            {
                Console.WriteLine($"- {recipe.Name}");
            }

            if (!recipes.Any())
            {
                Console.WriteLine("Няма намерени рецепти с този таг.");
            }
        }

        static void UpdateRecipe(RecipeService service)
        {
            Console.Write("\nВъведи името на рецептата за обновяване: ");
            string name = Console.ReadLine();

            var recipe = service.Get().FirstOrDefault(r => r.Name == name);

            if (recipe == null)
            {
                Console.WriteLine("Няма такава рецепта.");
                return;
            }

            Console.Write("Нови тагове (разделени със запетая): ");
            var newTags = Console.ReadLine()
                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim()).ToList();

            recipe.Tags = newTags;
            service.Update(recipe.Id, recipe);

            Console.WriteLine("Рецептата е обновена.");
        }

        static void DeleteRecipe(RecipeService service)
        {
            Console.Write("\nВъведи името на рецептата за изтриване: ");
            string name = Console.ReadLine();

            var recipe = service.Get().FirstOrDefault(r => r.Name == name);

            if (recipe == null)
            {
                Console.WriteLine("Няма такава рецепта.");
                return;
            }

            service.Remove(recipe.Id);
            Console.WriteLine("Рецептата е изтрита.");
        }
    }
}
