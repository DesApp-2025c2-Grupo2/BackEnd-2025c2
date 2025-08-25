namespace Application.Utilities;

public class CommonFunctions
{
    private static CommonFunctions? instance;
    private CommonFunctions() { }
    public static CommonFunctions Instance
    {
        get
        {
            instance ??= new CommonFunctions();
            return instance;
        }
    }

    public string GetRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Random random = new Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

}
