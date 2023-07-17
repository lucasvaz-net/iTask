using System.Data;
using System.Data.SqlClient;
using iTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace iTask.Controllers
{
    public class TarefasController : Controller
    {
        private string connectionString = "Data Source=SQL5085.site4now.net;Initial Catalog=db_a9c2c8_tarefas;User ID=db_a9c2c8_tarefas_admin;Password=Vitoriade10.;";


        // GET: Tarefas
        public IActionResult Index()
        {
            List<Tarefa> tarefas = new List<Tarefa>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM ViewTarefasNaoExcluidas";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Tarefa tarefa = new Tarefa
                    {
                        Id = (int)reader["id"],
                        Titulo = (string)reader["titulo"],
                        Descricao = (string)reader["descricao"],
                        Prioridade = (int)reader["prioridade"],
                        CategoriaId = (string)reader["categoria"],
                        StatusId = (string)reader["status"],
                        UsuarioId = (string)reader["usuario"]
                    };

                    if (reader["data_vencimento"] != DBNull.Value)
                    {
                        tarefa.DataVencimento = (DateTime)reader["data_vencimento"];
                    }
                    else
                    {
                        tarefa.DataVencimento = DateTime.MinValue; // Ou qualquer valor padrão que você desejar
                    }

                    tarefas.Add(tarefa);
                }

            }

            return View(tarefas);
        }


        // GET: Tarefas/Create
        public IActionResult Create()
        {
            return View();
        }




        // POST: Tarefas/Create
        [HttpPost]
        public IActionResult Create(Tarefa tarefa)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"INSERT INTO Tarefas (titulo, descricao, data_vencimento, prioridade, categoria_id, status_id, usuario_id)
                                 VALUES (@titulo, @descricao, @data_vencimento, @prioridade, @categoria_id, @status_id, @usuario_id)";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@titulo", tarefa.Titulo);
                command.Parameters.AddWithValue("@descricao", tarefa.Descricao);
                command.Parameters.AddWithValue("@data_vencimento", tarefa.DataVencimento);
                command.Parameters.AddWithValue("@prioridade", tarefa.Prioridade);
                command.Parameters.AddWithValue("@categoria_id", tarefa.CategoriaId);
                command.Parameters.AddWithValue("@status_id", tarefa.StatusId);
                command.Parameters.AddWithValue("@usuario_id", tarefa.UsuarioId);

                command.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // GET: Tarefas/Edit/5
        public IActionResult Edit(int id)
        {
            Tarefa tarefa = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Tarefas WHERE id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    tarefa = new Tarefa
                    {
                        Id = (int)reader["id"],
                        Titulo = (string)reader["titulo"],
                        Descricao = (string)reader["descricao"],
                        DataVencimento = (DateTime)reader["data_vencimento"],
                        Prioridade = (int)reader["prioridade"],
                        CategoriaId = (string)reader["categoria_id"],
                        StatusId = (string)reader["status_id"],
                        UsuarioId = (string)reader["usuario_id"]
                    };
                }
            }

            return View(tarefa);
        }

        // POST: Tarefas/Edit/5
        [HttpPost]
        public IActionResult Edit(int id, Tarefa tarefa)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = @"UPDATE Tarefas SET titulo = @titulo, descricao = @descricao, data_vencimento = @data_vencimento,
                                 prioridade = @prioridade, categoria_id = @categoria_id, status_id = @status_id, usuario_id = @usuario_id
                                 WHERE id = @id";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@titulo", tarefa.Titulo);
                command.Parameters.AddWithValue("@descricao", tarefa.Descricao);
                command.Parameters.AddWithValue("@data_vencimento", tarefa.DataVencimento);
                command.Parameters.AddWithValue("@prioridade", tarefa.Prioridade);
                command.Parameters.AddWithValue("@categoria_id", tarefa.CategoriaId);
                command.Parameters.AddWithValue("@status_id", tarefa.StatusId);
                command.Parameters.AddWithValue("@usuario_id", tarefa.UsuarioId);
                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // GET: Tarefas/Delete/5
        public IActionResult Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("ExcluirTarefa", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tarefa_id", id);

                command.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

    }
}
