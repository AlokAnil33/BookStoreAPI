namespace BookStore.API.Common
{
    public static class Enumerations
    {
        public enum Operators
        {
            EqualTo,
            Contains,
            NotEqualTo,
            GreaterThan,
            GreaterThanOrEqual,
            LesserThan,
            LessThanOrEqual,
            Between,
            BetweenOrEqual,
        }

        public enum BaseTypes
        {
            String,
            Number,
            DateTime
        }

    }
}
