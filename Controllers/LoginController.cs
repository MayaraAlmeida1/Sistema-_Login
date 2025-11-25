
using Microsoft.AspNetCore.Mvc;
using SistemLogin.Data;
using SistemLogin.Services;

namespace SistemLogin.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost] // Para verificar e validar algo(Dados do login)

        public IActionResult Entrar(string email, string senha) //Parâmetros de usuário e senha
        {
            if(string.IsNullOrWhiteSpace (email) || string.IsNullOrWhiteSpace(senha)) //IsNullOrWhiteSpace - verifica se existe espaços em branco ou vazios, nesse caso da senha e email
            {
                ViewBag.Erro = "Preencha todos os campos."; // ViewBag - Como se fosse uma mochila temporária que carrega essa informação(mensagem) de um lado para o outro
                return View("Index");
            }

            //* hash da senha digitada
            byte[] senhaDigitadaHash = HashService.GerarHashBytes(senha); //Pega a senha digitada, gera uma hash e armazena em "senhaDigitadaHash"

            //* Buscar usuário pelo email, verifica se o usuário existe no sistema
            var usuario = _context.Usuarios.FirstOrDefault(usuario => usuario.Email == email); //FirstOrDefault - procura usuário(nesse caso pelo email), se não encontrou ele retona nulo

            if(usuario == null)
            {
                ViewBag.Erro = "E-mail ou senha incorretos.";
                return View("Index");
            }

            //* Comparar byte a byte da senha
            if(!usuario.SenhaHash.SequenceEqual(senhaDigitadaHash)) // SequenceEqual - verifica se a sequência da "SenhaHash"(que está no banco) está igual a "senhaDigitadaHash", se qualquer byte estiver diferente ele retorna false
            {
                ViewBag.Erro = "E-mail ou senha incorretos.";
                return View("Index");
            }

            //* Login estiver OK -> Salva na sessão
            HttpContext.Session.SetString("UsuarioNome", usuario.NomeCompleto); //SetString - inserindo informações dentro da sessão
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);

            return RedirectToAction("Index", "Home"); //retorna para a index da home, não da login
        }

        public IActionResult Sair()
        {
            HttpContext.Session.Clear(); //Limpa a sessão retirando o usuário e o id da sessão
            return RedirectToAction("Index");   
        }
    }
}