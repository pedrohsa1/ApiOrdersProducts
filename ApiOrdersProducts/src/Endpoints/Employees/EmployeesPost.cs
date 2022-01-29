using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace ApiOrderProducts.Endpoints.Employees
{
    public class EmployeesPost
    {
        public static string Template => "/employees";
        public static string[] Methods => new string[] { HttpMethod.Post.ToString() };
        public static Delegate Handle => Action;

        [Authorize(Policy = "EmployeePolicy")] //Só pode ser acessado se o usuário autenticado tiver o EmployeeCode
        public static IResult Action(EmployeeRequest employeeRequest, UserManager<IdentityUser> userManager)
        {
            //para salvar o usuário, precisamos de um serviço chamado UserManager e não o Context
            var user = new IdentityUser { UserName = employeeRequest.Email, 
                                          Email =  employeeRequest.Email
                                        };
            var userResult = userManager.CreateAsync(user, employeeRequest.Password).Result;

            if (!userResult.Succeeded)
                return Results.ValidationProblem(userResult.Errors.ConvertToProblemDetails());

            var userClaims = new List<Claim> {
                new Claim("EmployeeCode", employeeRequest.EmployeeCode),
                new Claim("Name", employeeRequest.Name)
            };

            var claimResult = userManager.AddClaimsAsync(user, userClaims).Result;

            if(!claimResult.Succeeded)
                return Results.BadRequest(claimResult.Errors.First());

            return Results.Created($"/employees/{user.Id}", user.Id);
        }
    }
}
