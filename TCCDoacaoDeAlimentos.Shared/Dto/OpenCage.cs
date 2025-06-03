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
}
