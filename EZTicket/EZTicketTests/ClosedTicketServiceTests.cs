using EZTicket.Exceptions;
using EZTicket.Models;
using EZTicket.Services;

namespace EZTicketTests;

using Xunit;

public class ClosedTicketServiceTests
{
    [Fact]
    public void IsEmpty_EmptyList_ReturnsTrue()
    {
        // Arrange
        ClosedTicketService ticketService = new ClosedTicketService();

        // Act
        bool isEmpty = ticketService.IsEmpty();

        // Assert
        Assert.True(isEmpty);
    }

    [Fact]
    public void AddTicket_AddOneTicket_ListIsNotEmpty()
    {
        // Arrange
        ClosedTicketService ticketService = new ClosedTicketService();
        Ticket ticket = new Ticket(/* Initialize your ticket object */);

        // Act
        ticketService.AddTicket(ticket);
        bool isEmpty = ticketService.IsEmpty();

        // Assert
        Assert.False(isEmpty);
    }

    [Fact]
    public void RemoveTicket_EmptyList_ThrowsListEmptyException()
    {
        // Arrange
        ClosedTicketService ticketService = new ClosedTicketService();

        // Act and Assert
        Assert.Throws<ListEmptyException>(() => ticketService.RemoveTicket());
    }

    [Fact]
    public void RemoveTicket_NotEmptyList_ReturnsRemovedTicket()
    {
        // Arrange
        ClosedTicketService ticketService = new ClosedTicketService();
        Ticket ticket = new Ticket(/* Initialize your ticket object */);
        ticketService.AddTicket(ticket);

        // Act
        Ticket removedTicket = ticketService.RemoveTicket();

        // Assert
        Assert.Equal(ticket, removedTicket);
        Assert.True(ticketService.IsEmpty());
    }

    [Fact]
    public void Peek_EmptyList_ThrowsListEmptyException()
    {
        // Arrange
        ClosedTicketService ticketService = new ClosedTicketService();

        // Act and Assert
        Assert.Throws<ListEmptyException>(() => ticketService.Peek());
    }
}
