using EZTicket.Services;

namespace EZTicket.Models;

public class TicketHistory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int ServiceType { get; set; }
    public int Priority { get; set; }
    public string CreatedBy { get; set; }
    public string CompletedBy { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
}