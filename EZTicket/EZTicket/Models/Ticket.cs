using EZTicket.Services;

namespace EZTicket.Models;

// Model for Active Tickets Table
public class Ticket
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string ServiceType { get; set; }
    public int Priority { get; set; }
    public string CreatedBy { get; set; }
    public string? AssignedTo { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
    public bool IsClosed { get; set; }
    public bool IsPending { get; set; }
    public string? CompletedBy { get; set; }
    public DateTime? DateCompleted { get; set; }
    public List<TicketNote>? Notes { get; set; }

}