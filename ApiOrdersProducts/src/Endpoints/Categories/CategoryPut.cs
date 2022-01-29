using ApiOrderProducts.Domain.Products;
using ApiOrderProducts.Infra.Data;
using Microsoft.AspNetCore.Mvc;

namespace ApiOrderProducts.Endpoints.Categories
{
    public class CategoryPut
    {
        public static string Template => "/categories/{id:guid}";
        public static string[] Methods => new string[] { HttpMethod.Put.ToString() };
        public static Delegate Handle => Action;

        public static IResult Action([FromRoute] Guid Id, CategoryRequest categoryRequest, ApplicationDbContext context)
        {
            var category = context.Categories.Where(c => c.Id == Id).FirstOrDefault();

            if (category == null) 
                return Results.NotFound();

            category.EditInfo(categoryRequest.Name, categoryRequest.Active);
            context.SaveChanges();

            return Results.Ok();
        }
    }
}
