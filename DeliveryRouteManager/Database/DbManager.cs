using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeliveryRouteManager.Models;
using Microsoft.Data.Sqlite;

namespace DeliveryRouteManager.Database
{
    public class DbManager
    {
        private readonly string _connectionString;

        public DbManager(string rutaDb = "delivery.db")
        {
            _connectionString = $"Data Source={rutaDb}";
            InicializarTablas();
        }

        private void InicializarTablas()
        {
            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS Puntos (
                    Id      INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre  TEXT    NOT NULL,
                    X       REAL    NOT NULL,
                    Y       REAL    NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Rutas (
                    Id        INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdOrigen  INTEGER NOT NULL,
                    IdDestino INTEGER NOT NULL,
                    Peso      REAL    NOT NULL,
                    FOREIGN KEY (IdOrigen)  REFERENCES Puntos(Id),
                    FOREIGN KEY (IdDestino) REFERENCES Puntos(Id)
                );

                CREATE TABLE IF NOT EXISTS Pedidos (
                    Id            INTEGER PRIMARY KEY AUTOINCREMENT,
                    Cliente       TEXT    NOT NULL,
                    IdPunto       INTEGER NOT NULL,
                    Prioridad     INTEGER NOT NULL,
                    FechaRegistro TEXT    NOT NULL,
                    FOREIGN KEY (IdPunto) REFERENCES Puntos(Id)
                );

                CREATE TABLE IF NOT EXISTS Historial (
                    Id             INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdPedido       INTEGER NOT NULL,
                    Cliente        TEXT    NOT NULL DEFAULT '',
                    RutaRecorrida  TEXT    NOT NULL,
                    DistanciaTotal REAL    NOT NULL,
                    FechaEntrega   TEXT    NOT NULL
                );";

            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            // Migración: agregar columna Cliente si no existe (bases de datos previas)
            try
            {
                using SqliteCommand mig = new SqliteCommand(
                    "ALTER TABLE Historial ADD COLUMN Cliente TEXT NOT NULL DEFAULT ''", conn);
                mig.ExecuteNonQuery();
            }
            catch { /* columna ya existe, se ignora */ }
        }

        public int InsertarPunto(string nombre, double x, double y)
        {
            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();
            string sql = "INSERT INTO Puntos (Nombre, X, Y) VALUES (@n, @x, @y); SELECT last_insert_rowid();";
            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@n", nombre);
            cmd.Parameters.AddWithValue("@x", x);
            cmd.Parameters.AddWithValue("@y", y);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public List<NodoPunto> CargarPuntos()
        {
            List<NodoPunto> lista = new List<NodoPunto>();
            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();
            string sql = "SELECT Id, Nombre, X, Y FROM Puntos";
            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            using SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new NodoPunto(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetDouble(2),
                    reader.GetDouble(3)
                ));
            }
            return lista;
        }

        public void InsertarRuta(int idOrigen, int idDestino, double peso)
        {
            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();
            string sql = "INSERT INTO Rutas (IdOrigen, IdDestino, Peso) VALUES (@o, @d, @p)";
            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@o", idOrigen);
            cmd.Parameters.AddWithValue("@d", idDestino);
            cmd.Parameters.AddWithValue("@p", peso);
            cmd.ExecuteNonQuery();
        }

        public List<(int idOrigen, int idDestino, double peso)> CargarRutas()
        {
            List<(int, int, double)> lista = new List<(int, int, double)>();
            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();
            string sql = "SELECT IdOrigen, IdDestino, Peso FROM Rutas";
            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            using SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                lista.Add((reader.GetInt32(0), reader.GetInt32(1), reader.GetDouble(2)));
            return lista;
        }

        public int InsertarPedido(string cliente, int idPunto, int prioridad, string fechaRegistro)
        {
            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();
            string sql = @"INSERT INTO Pedidos (Cliente, IdPunto, Prioridad, FechaRegistro)
                           VALUES (@c, @p, @pr, @f); SELECT last_insert_rowid();";
            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@c", cliente);
            cmd.Parameters.AddWithValue("@p", idPunto);
            cmd.Parameters.AddWithValue("@pr", prioridad);
            cmd.Parameters.AddWithValue("@f", fechaRegistro);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void InsertarHistorial(int idPedido, string cliente, string ruta, double distancia, string fecha)
        {
            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();
            string sql = @"INSERT INTO Historial (IdPedido, Cliente, RutaRecorrida, DistanciaTotal, FechaEntrega)
                           VALUES (@id, @cl, @r, @d, @f)";
            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", idPedido);
            cmd.Parameters.AddWithValue("@cl", cliente);
            cmd.Parameters.AddWithValue("@r", ruta);
            cmd.Parameters.AddWithValue("@d", distancia);
            cmd.Parameters.AddWithValue("@f", fecha);
            cmd.ExecuteNonQuery();
        }

        public List<(int Id, string Cliente, int IdPunto, int Prioridad, string FechaRegistro)> CargarPedidosRaw()
        {
            var lista = new List<(int, string, int, int, string)>();
            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();
            string sql = "SELECT Id, Cliente, IdPunto, Prioridad, FechaRegistro FROM Pedidos ORDER BY Prioridad, FechaRegistro";
            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            using SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                lista.Add((reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4)));
            return lista;
        }

        public void EliminarPedido(int id)
        {
            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();
            string sql = "DELETE FROM Pedidos WHERE Id = @id";
            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

        public List<(int Id, int IdPedido, string Cliente, string RutaRecorrida, double DistanciaTotal, string FechaEntrega)> CargarHistorialCompleto()
        {
            var lista = new List<(int, int, string, string, double, string)>();
            using SqliteConnection conn = new SqliteConnection(_connectionString);
            conn.Open();
            string sql = @"SELECT Id, IdPedido, Cliente, RutaRecorrida, DistanciaTotal, FechaEntrega
                           FROM Historial ORDER BY Id DESC";
            using SqliteCommand cmd = new SqliteCommand(sql, conn);
            using SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                lista.Add((
                    reader.GetInt32(0),
                    reader.GetInt32(1),
                    reader.IsDBNull(2) ? "" : reader.GetString(2),
                    reader.GetString(3),
                    reader.GetDouble(4),
                    reader.GetString(5)
                ));
            return lista;
        }

    }
}
