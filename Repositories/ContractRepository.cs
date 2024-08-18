using Microsoft.EntityFrameworkCore;
using mini_crm.DbContext;
using mini_crm.Models;
using Npgsql;

namespace mini_crm.Repositories
{
    public interface IContractRepository
    {
        decimal GetSumOfContractsThisYear();
        List<LegalPersonContractSum> GetSumByLegalPersonInRussia();
        List<string> GetAuthorizedPersonEmailsThisMonth();
        int UpdateContractsStatusForOlderPersons();
        List<PhysicalPersonReport> GetActiveContractsReport();
    }
    public class ContractRepository : IContractRepository
    {
        private readonly ApplicationDbContext _context;

        public ContractRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public decimal GetSumOfContractsThisYear()
        {
            var currentYear = DateTime.UtcNow.Year;
            var sql = @"SELECT SUM(""ContractAmount"") FROM public.""Contracts""  WHERE EXTRACT(YEAR FROM ""SigningDate"") = @p0";
            var result = _context.Database.GetDbConnection()
                .CreateCommand();
            result.CommandText = sql;
            result.Parameters.Add(new NpgsqlParameter("@p0", currentYear));

            _context.Database.OpenConnection();
            try
            {
                using var reader = result.ExecuteReader();
                if (reader.Read())
                {
                    return reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                }
            }
            finally
            {
                _context.Database.CloseConnection();
            }

            return 0;

        }

        public List<LegalPersonContractSum> GetSumByLegalPersonInRussia()
        {
            var sql = @"
            SELECT
                l.""Id"" AS ""LegalPersonId"",
				l.""CompanyName"" AS ""CompanyName"",
                SUM(c.""ContractAmount"") AS ""TotalAmount""
            FROM
                public.""Contracts"" c
            JOIN
                public.""LegalPersons"" l
                ON c.""CounterpartyId"" = l.""Id""
            WHERE
                l.""Country"" = 'Россия' OR
				l.""Country"" = 'Russia'
            GROUP BY
                l.""Id""";
            var result = _context.Database.GetDbConnection()
                .CreateCommand();
            result.CommandText = sql;
            var results = new List<LegalPersonContractSum>();
            _context.Database.OpenConnection();
            try
            {
                using var reader = result.ExecuteReader();
                while (reader.Read())
                {
                    var legalPersonId = reader.GetInt32(reader.GetOrdinal("LegalPersonId"));
                    var companyName = reader.GetString(reader.GetOrdinal("CompanyName"));
                    var totalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount"));

                    results.Add(new LegalPersonContractSum
                    {
                        LegalPersonId = legalPersonId,
                        CompanyName = companyName,
                        TotalAmount = totalAmount
                    });
                    
                }
            }
            finally
            {
                _context.Database.CloseConnection();
            }

            return results;

        }

        public List<string> GetAuthorizedPersonEmailsThisMonth()
        {
            var sql = @"
            SELECT
                pp.""Email""
            FROM
                public.""Contracts"" c
            JOIN
                public.""PhysicalPersons"" pp
                ON c.""AuthorizedPersonId"" = pp.""Id""
            WHERE
                c.""SigningDate"" >= CURRENT_DATE - INTERVAL '30 days'
                AND c.""ContractAmount"" > 40000
            GROUP BY
                pp.""Email""";
            var result = _context.Database.GetDbConnection()
                .CreateCommand();
            result.CommandText = sql;

            var emails = new List<string>();

            _context.Database.OpenConnection();
            try
            {
                using var reader = result.ExecuteReader();
                while (reader.Read())
                {
                    var email = reader.GetString(reader.GetOrdinal("Email"));
                    emails.Add(email);
                }
            }
            finally
            {
                _context.Database.CloseConnection();
            }

            return emails;

        }

        public int UpdateContractsStatusForOlderPersons()
        {
            var sql = @"
            UPDATE public.""Contracts"" c
            SET
                ""Status"" = 5
            FROM
                public.""PhysicalPersons"" pp
            WHERE
                c.""AuthorizedPersonId"" = pp.""Id""
                AND pp.""Age"" >= 60
                AND c.""Status"" = 3";
            var result = _context.Database.GetDbConnection()
                .CreateCommand();
            result.CommandText = sql;


            _context.Database.OpenConnection();
            try
            { 
                var updateCount = result.ExecuteNonQuery();
                return updateCount;
            }
            finally
            {
                _context.Database.CloseConnection();
            }

        }

        public List<PhysicalPersonReport> GetActiveContractsReport()
        {
            var sql = @"
            SELECT
                pp.""FirstName"",
                pp.""LastName"",
                pp.""MiddleName"",
                pp.""Email"",
                pp.""Phone"",
                pp.""DateOfBirth""
            FROM
                public.""Contracts"" c
            JOIN
                public.""LegalPersons"" lp
                ON c.""CounterpartyId"" = lp.""Id""
            JOIN
                public.""PhysicalPersons"" pp
                ON c.""AuthorizedPersonId"" = pp.""Id""
            WHERE
                c.""Status"" = 3
                AND lp.""City"" = 'Москва' OR lp.""City"" = 'Moscow'
            GROUP BY
                pp.""FirstName"", pp.""LastName"", pp.""MiddleName"", pp.""Email"", pp.""Phone"", pp.""DateOfBirth""";

            var result = _context.Database.GetDbConnection()
                .CreateCommand();
            result.CommandText = sql;

            var reportData = new List<PhysicalPersonReport>();

            _context.Database.OpenConnection();
            try
            {
                using var reader = result.ExecuteReader();
                while (reader.Read())
                {
                    reportData.Add(new PhysicalPersonReport
                    {
                        FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                        LastName = reader.GetString(reader.GetOrdinal("LastName")),
                        MiddleName = reader.GetString(reader.GetOrdinal("MiddleName")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Phone = reader.GetString(reader.GetOrdinal("Phone")),
                        DateOfBirth = reader.GetDateTime(reader.GetOrdinal("DateOfBirth"))
                    });
                }
            }
            finally
            {
                _context.Database.CloseConnection();
            }

            return reportData;
        }
    }
}

