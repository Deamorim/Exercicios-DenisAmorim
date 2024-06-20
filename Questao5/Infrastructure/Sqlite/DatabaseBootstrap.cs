using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Queries.Responses;

namespace Questao5.Infrastructure.Sqlite
{
    public class DatabaseBootstrap : IDatabaseBootstrap
    {
        private readonly DatabaseConfig databaseConfig;

        public DatabaseBootstrap(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public void Setup()
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            var table = connection.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND (name = 'contacorrente' or name = 'movimento' or name = 'idempotencia');");
            var tableName = table.FirstOrDefault();

            //connection.Open();

            //var teste = connection.CreateCommand();
            //teste.CommandText = "SELECT * FROM contacorrente";

            //using (var reader = teste.ExecuteReader())
            //{
            //    while (reader.Read())
            //    {
            //        var result = reader.GetString(0);
            //        Console.WriteLine($"Retorno Select 01 => {result}");
            //    }
            //}

            //connection.Close();

            if (!string.IsNullOrEmpty(tableName) && (tableName == "contacorrente" || tableName == "movimento" || tableName == "idempotencia"))
                return;

            connection.Execute("CREATE TABLE contacorrente ( " +
                               "idcontacorrente TEXT(37) PRIMARY KEY," +
                               "numero INTEGER(10) NOT NULL UNIQUE," +
                               "nome TEXT(100) NOT NULL," +
                               "ativo INTEGER(1) NOT NULL default 0," +
                               "CHECK(ativo in (0, 1)) " +
                               ");");

            connection.Execute("CREATE TABLE movimento ( " +
                "idmovimento TEXT(37) PRIMARY KEY," +
                "idcontacorrente INTEGER(10) NOT NULL," +
                "datamovimento TEXT(25) NOT NULL," +
                "tipomovimento TEXT(1) NOT NULL," +
                "valor REAL NOT NULL," +
                "CHECK(tipomovimento in ('C', 'D')), " +
                "FOREIGN KEY(idcontacorrente) REFERENCES contacorrente(idcontacorrente) " +
                ");");

            connection.Execute("CREATE TABLE idempotencia (" +
                               "chave_idempotencia TEXT(37) PRIMARY KEY," +
                               "requisicao TEXT(1000)," +
                               "resultado TEXT(1000));");

            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('B6BAFC09-6967-ED11-A567-055DFA4A16C9', 123, 'Katherine Sanchez', 1);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('FA99D033-7067-ED11-96C6-7C5DFA4A16C9', 456, 'Eva Woodward', 1);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('382D323D-7067-ED11-8866-7D5DFA4A16C9', 789, 'Tevin Mcconnell', 1);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('F475F943-7067-ED11-A06B-7E5DFA4A16C9', 741, 'Ameena Lynn', 0);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('BCDACA4A-7067-ED11-AF81-825DFA4A16C9', 852, 'Jarrad Mckee', 0);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('D2E02051-7067-ED11-94C0-835DFA4A16C9', 963, 'Elisha Simons', 0);");
        }

        public async Task<Guid> InserirMovimentoAsync(string idContaCorrente, string tipoMovimento, decimal valor)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            var idMovimento = Guid.NewGuid();
            var sql = "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                      "VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor);";
            var parameters = new
            {
                IdMovimento = idMovimento,
                IdContaCorrente = idContaCorrente,
                DataMovimento = DateTime.UtcNow,
                TipoMovimento = tipoMovimento,
                Valor = valor
            };
            await connection.ExecuteAsync(sql, parameters);

            return idMovimento;
        }

        public async Task<bool> ContaCorrenteExisteAsync(string idContaCorrente)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            var sql = "SELECT COUNT(1) FROM contacorrente WHERE idcontacorrente = @IdContaCorrente;";
            var count = await connection.QuerySingleAsync<int>(sql, new { IdContaCorrente = idContaCorrente });
            return count > 0;
        }

        public async Task<bool> ContaCorrenteAtivaAsync(string idContaCorrente)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            var sql = "SELECT ativo FROM contacorrente WHERE idcontacorrente = @IdContaCorrente;";
            var ativo = await connection.QuerySingleOrDefaultAsync<int>(sql, new { IdContaCorrente = idContaCorrente });
            return ativo == 1;
        }

        public async Task<SaldoContaCorrente> ObterSaldoContaCorrenteAsync(string idContaCorrente)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            // Consulta para calcular saldo da conta corrente
            var query = @"
            SELECT 
                c.numero AS NumeroContaCorrente,
                c.nome AS NomeTitular,
                COALESCE(SUM(CASE WHEN m.tipomovimento = 'C' THEN m.valor ELSE 0 END), 0) -
                COALESCE(SUM(CASE WHEN m.tipomovimento = 'D' THEN m.valor ELSE 0 END), 0) AS SaldoAtual
            FROM contacorrente c
            LEFT JOIN movimento m ON c.idcontacorrente = m.idcontacorrente
            WHERE c.idcontacorrente = @IdContaCorrente
            GROUP BY c.numero, c.nome";

            var saldo = await connection.QuerySingleOrDefaultAsync<SaldoContaCorrente>(query, new { IdContaCorrente = idContaCorrente });
            return saldo;
        }
    }
}
