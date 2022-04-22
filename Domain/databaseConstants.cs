namespace Domain
{
    public static class DatabaseConstants
    {
        public const int standartStringLength = 50;
        public const int passwordMaxLength = 60;
        public const string standartStringType = "varchar";

        public const string namePattern = @"(^\p{IsCyrillic}+$)|(^\p{IsBasicLatin}+$)";
    }
}