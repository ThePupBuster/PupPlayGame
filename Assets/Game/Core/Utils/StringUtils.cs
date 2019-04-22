namespace Game.Utils
{
    public static class StringUtils
    {
        public static string CommaSeperateNumber(int number)
        {
            return string.Format("{0:n0}", number);
        }
    }
}
