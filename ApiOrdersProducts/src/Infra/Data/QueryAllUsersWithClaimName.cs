using ApiOrderProducts.Endpoints.Employees;
using Dapper;
using Npgsql;

namespace ApiOrderProducts.Infra.Data
{
    public class QueryAllUsersWithClaimName
    {
        private readonly IConfiguration configuration;

        public QueryAllUsersWithClaimName(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IEnumerable<EmployeeResponse> Execute(int page, int rows)
        {
            //Aqui utilizamos o Dapper, uma lib mantida pelo stackoverflow para ganhar performace em consultas que 
            //precisa de muitas interações como a relação de Users e Claims.
            //O Dapper precisa de uma conexão com BD independente aou do Entity Framework, neste caso passamos o IConfiguration

            //Micro ERM que vai fazer a consulta inserindo na classe EmployeeResponse

            var db = new NpgsqlConnection(configuration["ConnectionString:OrderProducts"]);
            var query = @"select ""Email"",""ClaimValue"" as Name 
                          from ""AspNetUsers"" anu inner join ""AspNetUserClaims"" anuc
                          on anu.""Id"" = anuc.""UserId"" and anuc.""ClaimType"" = 'Name'
                          order by Name
                          LIMIT @rows
                          OFFSET (@page -1) * @rows";
            return db.Query<EmployeeResponse>(
                query,
                new { page, rows }
            );
        }
    }
}
