namespace Aiba.Enums
{
    [Flags]
    public enum MediaTypeFlag
    {
        MANGA = 1,
        VIDEO = 2,
        ALL = MANGA | VIDEO
    }
}