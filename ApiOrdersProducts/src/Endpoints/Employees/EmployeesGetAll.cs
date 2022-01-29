using ApiOrderProducts.Infra.Data;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using System.Security.Claims;

namespace ApiOrderProducts.Endpoints.Employees
{
    public class EmployeesGetAll
    {
        public static string Template => "/employees";
        public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
        public static Delegate Handle => Action;

        public static async Task<IResult> Action(int? page, int? rows, QueryAllUsersWithClaimName query)
        {
            var result = query.Execute(page.Value, rows.Value);
            return Results.Ok(result);
        }
    }
}
