namespace EZTicket.Models;

public class TicketNote
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Note { get; set; }
    public DateTime Created { get; set; }
    
    public int TicketId { get; set; }
    
    public ActiveTickets ActiveTickets { get; set; }
}