using System;

public class Region
{
    public CloudRegionCode Code;
    public string HostAndPort;
    public int Ping;

    public static CloudRegionCode Parse(string codeAsString)
    {
        codeAsString = codeAsString.ToLower();
        CloudRegionCode result = CloudRegionCode.none;
        if (Enum.IsDefined(typeof(CloudRegionCode), codeAsString))
        {
            result = (CloudRegionCode)(int)Enum.Parse(typeof(CloudRegionCode), codeAsString);
        }
        return result;
    }

    public override string ToString()
    {
        return $"'{Code}' \t{Ping}ms \t{HostAndPort}";
    }
}
