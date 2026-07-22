namespace OmoriMod.Content.Util;

public static class OmoriModStringExtensions
{
    private static readonly string String = OmoriMod.MOD_NAME;

    /// <summary>
    /// Creates a string that has <see cref="OmoriMod.MOD_NAME"/> attached to the front.
    /// Simply because I want to stop typing it out.
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string OmoriModString(this string str)
    {
        return String + ":" + str;
    }
}