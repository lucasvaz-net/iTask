using iTask.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

public class ContaController : Controller
{
    private string connectionString = "Data Source=SQL5085.site4now.net;Initial Catalog=db_a9c2c8_tarefas;User ID=db_a9c2c8_tarefas_admin;Password=Vitoriade10.;";

    // GET: Conta/Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(Usuario model)
    {
        Usuario usuario = VerificarCredenciais(model.Email, model.Senha);

        if (usuario != null)
        {
            // Autenticar o usuário e redirecionar para a página desejada
            // Por exemplo, redirecionar para a página de tarefas
            return RedirectToAction("Index", "Home");
        }
        else
        {
            // Credenciais inválidas, exibir mensagem de erro
            return RedirectToAction("AcessoNegado", "Conta");
        }
    }

    [HttpGet]
    public IActionResult LoginDireto(string email, string senha)
    {
        if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(senha))
        {
            Usuario usuario = VerificarCredenciais(email, senha);

            if (usuario != null)
            {
                // Autenticar o usuário e redirecionar para a página desejada
                // Por exemplo, redirecionar para a página de tarefas
                return RedirectToAction("Index", "Home");
            }
        }

        // Credenciais inválidas ou ausentes, exibir mensagem de erro
        return RedirectToAction("AcessoNegado", "Conta");
    }

    private Usuario VerificarCredenciais(string email, string senha)
    {
        Usuario usuario = null;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string query = "SELECT * FROM ViewUsuario WHERE Email = @Email AND Senha = @Senha";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Senha", senha);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                usuario = new Usuario
                {
                    Email = (string)reader["Email"],
                    Id = (int)reader["Id"],
                    Senha = (string)reader["Senha"],
                    // Preencha outros campos necessários do usuário, se houver
                };
            }

            reader.Close();
        }

        return usuario;
    }

    public IActionResult AcessoNegado()
    {
        return View();
    }
}
