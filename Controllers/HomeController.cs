using System.Data;
using System.Data.SqlClient;
using iTask.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace iTask.Controllers
{
    public class Home : Controller
    {
        private string connectionString = "Data Source=67.211.223.122;Initial Catalog=itask;User ID=tarin;Password=Vitoriade10.;";




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
                        tarefa.DataVencimento = DateTime.MinValue; 
                    }

                    tarefas.Add(tarefa);
                }

            }

            return View(tarefas);
        }


        
        public IActionResult Create()
        {
            return View();
        }




  
        
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


        
        public IActionResult Edit(int id)
        {
            Tarefa tarefa = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM ViewTarefasNaoExcluidas WHERE id = @id";
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
                        CategoriaId = (string)reader["categoria"],
                        StatusId = (string)reader["status"],
                        UsuarioId = (string)reader["usuario"]
                    };
                }
            }

            return View(tarefa);
        }


        
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
