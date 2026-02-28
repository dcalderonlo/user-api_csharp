using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using user_api_csharp.Data;
using user_api_csharp.Models;

namespace user_api_csharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        var usuarios = await context.Usuarios.AsNoTracking().ToListAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        return usuario is null ? NotFound(new { mensaje = "Usuario no encontrado." }) : Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> PostUsuario([FromBody] Usuario usuario)
    {
        var correoNormalizado = usuario.Correo.Trim().ToLower();

        var correoEnUso = await context.Usuarios
            .AnyAsync(u => u.Correo.ToLower() == correoNormalizado);

        if (correoEnUso)
        {
            return BadRequest(new { mensaje = "El correo electrónico ya está en uso." });
        }

        usuario.Correo = correoNormalizado;
        context.Usuarios.Add(usuario);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutUsuario(int id, [FromBody] Usuario usuarioActualizado)
    {
        if (id != usuarioActualizado.Id)
        {
            return BadRequest(new { mensaje = "El ID de la ruta no coincide con el ID del usuario." });
        }

        var usuarioExistente = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        if (usuarioExistente is null)
        {
            return NotFound(new { mensaje = "Usuario no encontrado." });
        }

        var correoNormalizado = usuarioActualizado.Correo.Trim().ToLower();

        var correoEnUsoPorOtro = await context.Usuarios
            .AnyAsync(u => u.Id != id && u.Correo.ToLower() == correoNormalizado);

        if (correoEnUsoPorOtro)
        {
            return BadRequest(new { mensaje = "El correo electrónico ya está en uso." });
        }

        usuarioExistente.Nombre = usuarioActualizado.Nombre;
        usuarioExistente.Correo = correoNormalizado;
        usuarioExistente.FechaDeNacimiento = usuarioActualizado.FechaDeNacimiento;

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        if (usuario is null)
        {
            return NotFound(new { mensaje = "Usuario no encontrado." });
        }

        context.Usuarios.Remove(usuario);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
