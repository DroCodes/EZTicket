using EZTicket.Models;

namespace EZTicket.Services;

public class InsertionSort
{
    public List<ActiveTickets> InsertionSortByDate(List<ActiveTickets> tickets)
    {
        for (int i = 1; i < tickets.Count; i++)
        {
            ActiveTickets key = tickets[i];
            int j = i - 1;

            while (j >= 0 && tickets[j].DateUpdated > key.DateUpdated)
            {
                tickets[j + 1] = tickets[j];
                j = j - 1;
            }

            tickets[j + 1] = key;
        }

        return tickets;
    }
}