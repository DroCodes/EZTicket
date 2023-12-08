using EZTicket.Models;

namespace EZTicket.Services;

// Node for Ticket 
public class InsertionSort
{
    // Sorts the list by date
    public List<Ticket> SortByDate(List<Ticket> tickets)
    {
        for (int i = 1; i < tickets.Count; i++)
        {
            Ticket key = tickets[i];
            int j = i - 1;

            // Change the condition to sort from newest to oldest
            while (j >= 0 && tickets[j].DateUpdated < key.DateUpdated)
            {
                tickets[j + 1] = tickets[j];
                j = j - 1;
            }

            tickets[j + 1] = key;
        }

        return tickets;
    }

}