public class OpenCageResponse
{
    public List<OpenCageResult> Results { get; set; }
}

public class OpenCageResult
{
    public OpenCageComponents Components { get; set; }
}

public class OpenCageComponents
{
    public string City { get; set; }
    public string Town { get; set; } // Added property
    public string Village { get; set; } // Added property
    public string Municipality { get; set; } // Added property
}
