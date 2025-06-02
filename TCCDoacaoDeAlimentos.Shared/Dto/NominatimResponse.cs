public class NominatimResponse
{
    public NominatimAddress Address { get; set; }
}

public class NominatimAddress
{
    public string City { get; set; }
    public string Town { get; set; }
    public string Village { get; set; }
    public string Municipality { get; set; }
}
