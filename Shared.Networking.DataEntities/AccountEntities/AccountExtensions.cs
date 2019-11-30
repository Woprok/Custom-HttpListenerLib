namespace Shared.Networking.DataEntities.AccountEntities
{
    public static class AccountExtensions
    {
        /// <summary>
        /// Provides copy of AccountEntity without password.
        /// </summary>
        public static AccountEntity ShareableAccount(this AccountEntity accountEntity)
        {
            return new AccountEntity(accountEntity) { Password = string.Empty };
        }
    }
}