using EZTicket.Exceptions;
using EZTicket.Models;
using EZTicket.Services;
using Xunit.Abstractions;

namespace EZTicketTests;

public class PriorityTicketService
{
    private readonly ITestOutputHelper _testOutputHelper;
    
    public PriorityTicketService(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public void GetTickets_EmptyQueue_ReturnsEmptyList()
    {
        // Arrange
        EZTicket.Services.PriorityTicketService ticketService = new EZTicket.Services.PriorityTicketService();

        // Act
        var tickets = ticketService.GetTickets();

        // Assert
        Assert.Empty(tickets);
    }

    [Fact]
    public void Peek_EmptyQueue_ThrowsInvalidOperationException()
    {
        // Arrange
        EZTicket.Services.PriorityTicketService ticketService = new EZTicket.Services.PriorityTicketService();

        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => ticketService.Peek());
    }

    [Fact]
    public void IsEmpty_EmptyQueue_ReturnsTrue()
    {
        // Arrange
        EZTicket.Services.PriorityTicketService ticketService = new EZTicket.Services.PriorityTicketService();

        // Act
        bool isEmpty = ticketService.isEmpty();

        // Assert
        Assert.True(isEmpty);
    }

    [Fact]
    public void AddTicket_NullTicket_DoesNotAddToQueue()
    {
        // Arrange
        var ticketService = new EZTicket.Services.PriorityTicketService();

        // Act
        ticketService.AddTicket(null);

        // Assert
        Assert.Empty(ticketService.GetTickets());
    }

    [Fact]
    public void AddTicket_Priority3_AddsToBeginningOfQueue()
    {
        // Arrange
        var ticketService = new EZTicket.Services.PriorityTicketService();
        var ticket = new Ticket() { Priority = 3 };

        // Act
        ticketService.AddTicket(ticket);

        // Assert
        Assert.Equal(ticket, ticketService.Peek());
    }

    [Fact]
    public void AddTicket_Priority2_AddsToSecondPositionInQueue()
    {
        // Arrange
        EZTicket.Services.PriorityTicketService ticketService = new EZTicket.Services.PriorityTicketService();
        var ticket1 = new Ticket() { Priority = 1 };
        var ticket2 = new Ticket() { Priority = 2 };

        // Act
        ticketService.AddTicket(ticket1);
        ticketService.AddTicket(ticket2);

        // Assert
        Assert.Equal(ticket2, ticketService.GetTickets()[1]);
    }

    [Fact]
    public void AddTicket_Priority1_AddsToEndOfQueue()
    {
        // Arrange
        var ticketService = new EZTicket.Services.PriorityTicketService();
        var ticket = new Ticket { Priority = 1 };

        // Act
        ticketService.AddTicket(ticket);

        // Assert
        Assert.Equal(ticket, ticketService.GetTickets().Last());
    }

    [Fact]
    public void RemoveTicket_EmptyQueue_ThrowsListEmptyException()
    {
        // Arrange
        var ticketService = new EZTicket.Services.PriorityTicketService();

        // Act and Assert
        Assert.Throws<ListEmptyException>(() => ticketService.RemoveTicket());
    }

    [Fact]
    public void RemoveTicket_NonEmptyQueue_RemovesAndReturnsFirstTicket()
    {
        // Arrange
        var ticketService = new EZTicket.Services.PriorityTicketService();
        var ticket = new Ticket { Priority = 1 };
        ticketService.AddTicket(ticket);

        // Act
        var removedTicket = ticketService.RemoveTicket();

        // Assert
        Assert.Equal(ticket, removedTicket);
        Assert.Empty(ticketService.GetTickets());
    }

    // Add more tests as needed
}