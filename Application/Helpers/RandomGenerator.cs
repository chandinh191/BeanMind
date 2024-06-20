namespace Application.Helpers;

public static class RandomGenerator
{
    private const string randomString = "abcdefghigklmnopqrstuywxyzABCDEFGHIGKLMNOPQRSTUVWXYZ1234567890-=!@#$%^&*()_+<>";
    public static int GenerateInteger(int min, int max)
    {
        return new Random().Next(min, max);
    }

    public static string GenerateRandomString(int length = 10)
    {
        string strResult = string.Empty;

        for (int i = 0; i < length; i++)
        {
            strResult += randomString.ElementAt(GenerateInteger(0, randomString.Length));
        }

        return strResult;
    }
}
