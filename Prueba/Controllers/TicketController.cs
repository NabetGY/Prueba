using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Prueba.Entities;

namespace Prueba.Controllers;

[Route("api/tickets")]
public class TicketController: ControllerBase
{
    private readonly ApplicationDbContext _context;

    private int _limit = 99999;

    public TicketController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public async Task<IActionResult> Index(string idSorteo, string idCliente, string idUsuario)
    {
        // Validaciones 
        var sorteo = await _context.Sorteos.Where(sorteo => sorteo.Id.ToString() == idSorteo).FirstOrDefaultAsync();

        if (sorteo == null) return NotFound("No se encontro el sorteo...");

        var cliente = await _context.Clientes.Where(cliente => cliente.Id.ToString() == idCliente).FirstOrDefaultAsync();
        
        if (cliente == null) return NotFound("No se encontro el cliente...");
        
        var usuario = await _context.Usuarios.Where(usuario => usuario.Id.ToString() == idUsuario).FirstOrDefaultAsync();

        if (usuario == null) return NotFound("No se encontro el usuario...");
        
        // Extrae de la base de datos los numeros que ya han sido generados por el sorteo y cliente
        var numerosGenerados = await _context.Tickets
            .Where(ticket => ticket.IdSorteo.ToString() == idSorteo && 
                             ticket.IdCliente.ToString() == idCliente).ToListAsync();

        // Lista de numeros con rango completo
        var numerosDisponibles = Enumerable.Range(1, _limit);

        // filtramos de la lista los numeros que estan generados
        var numerosNoGenerados = numerosDisponibles
            .Where(num => !numerosGenerados.Any(aux => aux.Valor == num));

        // se escoge un numero aleatorio
        var numeroAleatorio = numerosNoGenerados
            .OrderBy(num => Guid.NewGuid()).FirstOrDefault();

        // si es diferente de cero se agina el numero
        if (numeroAleatorio != 0)
        {
            Ticket ticket = new Ticket()
            {
                Id = Guid.NewGuid(),
                Valor = numeroAleatorio,
                IdSorteo = sorteo.Id,
                IdCliente = cliente.Id,
                IdUsuario = usuario.Id
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            
            return Ok(numeroAleatorio.ToString("D5"));
        }
        
        return Ok(numeroAleatorio);
    }
}