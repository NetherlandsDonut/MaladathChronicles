
public class Transport
{
    public Transport(string means, string destination, double price = 0)
    {
        this.means = means;
        this.destination = destination;
        this.price = price;
    }

    public string means; 
    public string destination;
    public double price;
}
