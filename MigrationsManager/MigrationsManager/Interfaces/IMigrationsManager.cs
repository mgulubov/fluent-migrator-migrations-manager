namespace MigrationsManager.Interfaces
{
    using System;

    public interface IMigrationsManager
    {
        bool MigrateTo(long version);
    }
}