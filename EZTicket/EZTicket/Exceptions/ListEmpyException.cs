namespace EZTicket.Exceptions;

public class ListEmptyException : Exception
{
    public override string Message => "List is Empty";
    public string CustomMessage { get; set; }
    
    public ListEmptyException()
    {
        
    }
    
    public ListEmptyException(string message) : base(message)
    {
        CustomMessage = message;
    }
}