using EZTicket.Exceptions;
using EZTicket.Models;
using EZTicket.Services;

namespace EZTicketTests;

using Xunit;

public class ActiveTicketServiceTests
{
    [Fact]
    public void IsEmpty_EmptyList_ReturnsTrue()
    {
        // Arrange
        ActiveTicketService ticketService = new ActiveTicketService();

        // Act
        bool isEmpty = ticketService.IsEmpty();

        // Assert
        Assert.True(isEmpty);
    }

    [Fact]
    public void AddTicket_AddOneTicket_ListIsNotEmpty()
    {
        // Arrange
        ActiveTicketService ticketService = new ActiveTicketService();
        ActiveTickets ticket = new ActiveTickets(/* Initialize your ticket object */);

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
        ActiveTicketService ticketService = new ActiveTicketService();

        // Act and Assert
        Assert.Throws<ListEmptyException>(() => ticketService.RemoveTicket());
    }

    [Fact]
    public void RemoveTicket_NotEmptyList_ReturnsRemovedTicket()
    {
        // Arrange
        ActiveTicketService ticketService = new ActiveTicketService();
        ActiveTickets ticket = new ActiveTickets(/* Initialize your ticket object */);
        ticketService.AddTicket(ticket);

        // Act
        ActiveTickets removedTicket = ticketService.RemoveTicket();

        // Assert
        Assert.Equal(ticket, removedTicket);
        Assert.True(ticketService.IsEmpty());
    }

    [Fact]
    public void Peek_EmptyList_ThrowsListEmptyException()
    {
        // Arrange
        ActiveTicketService ticketService = new ActiveTicketService();

        // Act and Assert
        Assert.Throws<ListEmptyException>(() => ticketService.Peek());
    }
}
