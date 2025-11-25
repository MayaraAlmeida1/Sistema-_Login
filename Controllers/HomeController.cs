using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SistemLogin.Models;

namespace SistemLogin.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        if(HttpContext.Session.GetString("UsuarioNome") == null) //GetString - Puxa a informação da string UsuarioNome(criado no LoginController) e verifica se não é nulo
        {
            return RedirectToAction("Index", "Login"); //Redireciona oara a index do login
        }

        //* ViewBag -> mochilinha que carrega as informações(do usuário que está acessando) para a view
        ViewBag.Usuario = HttpContext.Session.GetString("UsuarioNome");
        return View();
    }
}
