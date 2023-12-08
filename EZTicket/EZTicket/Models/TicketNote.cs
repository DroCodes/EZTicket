namespace EZTicket.Models;

public class TicketNote
{
    // Model for Ticket Notes Table
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Note { get; set; }
    public DateTime Created { get; set; }
    
    public int TicketId { get; set; }
    
    public Ticket Ticket { get; set; }
}