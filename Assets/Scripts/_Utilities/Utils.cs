public static class Utils
{
    public static bool IsEmpty(string value)
    {
        return string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);
    }
}
