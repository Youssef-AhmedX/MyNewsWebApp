namespace MyNewsMvc.Core.Consts
{
    public static class Errors
    {
        public const string MaxMinLengthError = "The {0} must be at least {2} and at max {1} characters long.";
        public const string MaxLengthError = "{0} Can Not Contain More Than {1} letter!";
        public const string IsExistError = "This {0} Already Exist!";
        public const string IsValueValid = "{0} Value Not Acceptable!";
        public const string EngCharactersOnly = "{0} Must Contain Only English Letters";
    }
}
