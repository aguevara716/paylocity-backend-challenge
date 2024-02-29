namespace Api.Services;

public static class PayPeriodSettings
{
    public const int CHECKS_PER_YEAR = 26;
    public static readonly int DAYS_PER_PAY_PERIOD = 365 / CHECKS_PER_YEAR;
}
