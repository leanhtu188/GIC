namespace Core
{
    public interface IStatementService
    {
        Statement? GetStatement(string accountId, int month, int year = 2023);
    }
}