using EZTicket.Exceptions;
using EZTicket.Models;
using EZTicket.Services;
using Xunit.Abstractions;

namespace EZTicketTests;

public class PendingTicketServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    
    public PendingTicketServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    
    [Fact]
    public void GetTickets_EmptyQueue_ReturnsEmptyList()
    {
        // Arrange
        PendingTicketService ticketService = new PendingTicketService();

        // Act
        var tickets = ticketService.GetTickets();

        // Assert
        Assert.Empty(tickets);
    }

    [Fact]
    public void Peek_EmptyQueue_ThrowsInvalidOperationException()
    {
        // Arrange
        PendingTicketService ticketService = new PendingTicketService();

        // Act and Assert
        Assert.Throws<InvalidOperationException>(() => ticketService.Peek());
    }

    [Fact]
    public void IsEmpty_EmptyQueue_ReturnsTrue()
    {
        // Arrange
        PendingTicketService ticketService = new PendingTicketService();

        // Act
        bool isEmpty = ticketService.isEmpty();

        // Assert
        Assert.True(isEmpty);
    }

    [Fact]
    public void AddTicket_NullTicket_DoesNotAddToQueue()
    {
        // Arrange
        PendingTicketService ticketService = new PendingTicketService();

        // Act
        ticketService.AddTicket(null);

        // Assert
        Assert.Empty(ticketService.GetTickets());
    }

    [Fact]
    public void AddTicket_Priority3_AddsToBeginningOfQueue()
    {
        // Arrange
        PendingTicketService ticketService = new PendingTicketService();
        PendingTickets ticket = new PendingTickets { Priority = 3 };

        // Act
        ticketService.AddTicket(ticket);

        // Assert
        Assert.Equal(ticket, ticketService.Peek());
    }

    [Fact]
    public void AddTicket_Priority2_AddsToSecondPositionInQueue()
    {
        // Arrange
        PendingTicketService ticketService = new PendingTicketService();
        PendingTickets ticket1 = new PendingTickets { Priority = 1 };
        PendingTickets ticket2 = new PendingTickets { Priority = 2 };

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
        PendingTicketService ticketService = new PendingTicketService();
        PendingTickets ticket = new PendingTickets { Priority = 1 };

        // Act
        ticketService.AddTicket(ticket);

        // Assert
        Assert.Equal(ticket, ticketService.GetTickets().Last());
    }

    [Fact]
    public void RemoveTicket_EmptyQueue_ThrowsListEmptyException()
    {
        // Arrange
        PendingTicketService ticketService = new PendingTicketService();

        // Act and Assert
        Assert.Throws<ListEmptyException>(() => ticketService.RemoveTicket());
    }

    [Fact]
    public void RemoveTicket_NonEmptyQueue_RemovesAndReturnsFirstTicket()
    {
        // Arrange
        PendingTicketService ticketService = new PendingTicketService();
        PendingTickets ticket = new PendingTickets { Priority = 1 };
        ticketService.AddTicket(ticket);

        // Act
        var removedTicket = ticketService.RemoveTicket();

        // Assert
        Assert.Equal(ticket, removedTicket);
        Assert.Empty(ticketService.GetTickets());
    }

    // Add more tests as needed
}