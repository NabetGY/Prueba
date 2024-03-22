namespace Prueba.Entities;

public class Ticket
{
    public Guid Id { get; set; }

    public int Valor { get; set; }

    public Guid IdSorteo { get; set; }

    public Guid IdCliente { get; set; }

    public Guid IdUsuario { get; set; }
    
    public Sorteo Sorteo { get; set; }

    public Cliente Cliente { get; set; }

    public Usuario Usuario { get; set; }
}