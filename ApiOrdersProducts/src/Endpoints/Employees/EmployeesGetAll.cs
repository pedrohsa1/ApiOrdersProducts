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

        public static IResult Action(int? page, int? rows, IConfiguration configuration)
        {
            //Aqui utilizamos o Dapper, uma lib mantida pelo stackoverflow para ganhar performace em consultas que 
            //precisa de muitas interações como a relação de Users e Claims.
            //O Dapper precisa de uma conexão com BD independente aou do Entity Framework, neste caso passamos o IConfiguration

            //Micro ERM que vai fazer a consulta inserindo na classe EmployeeResponse
            var db = new NpgsqlConnection(configuration["ConnectionString:OrderProducts"]);
            var employees = db.Query<EmployeeResponse>(
                @"select ""Email"",""ClaimValue"" as Name 
                  from ""AspNetUsers"" anu inner join ""AspNetUserClaims"" anuc
                  on anu.""Id"" = anuc.""UserId"" and anuc.""ClaimType"" = 'Name'");

            return Results.Ok(employees);
            
        }
    }
}
