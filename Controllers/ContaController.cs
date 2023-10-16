using iTask.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

public class ContaController : Controller
{
    private string connectionString = "Data Source=67.211.223.122;Initial Catalog=itask;User ID=tarin;Password=Vitoriade10.;";


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

            return RedirectToAction("Index", "Home");
        }
        else
        {

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

                return RedirectToAction("Index", "Home");
            }
        }


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
