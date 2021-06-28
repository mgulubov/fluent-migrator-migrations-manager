namespace FluentMigrator.MigrationsManager
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    using FluentMigrator.Runner;

    using Interfaces;

    public class MigrationsManager : IMigrationsManager
    {
        private readonly IServiceProvider serviceProvider;

        private long targetVersion;
        private IServiceScope scope;
        private IMigrationRunner runner;

        public MigrationsManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public bool MigrateTo(long version)
        {
            this.targetVersion = version;
            using (this.scope = this.serviceProvider.CreateScope())
            {
                this.BuildMigrationRunner();
                return this.TryRunMigrations();
            }
        }

        private void BuildMigrationRunner()
        {
            IServiceProvider serviceProvider = this.scope.ServiceProvider;

            this.runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        }

        private bool TryRunMigrations()
        {
            this.runner.ValidateVersionOrder();
            bool didMigrate = false;
            if (this.runner.HasMigrationsToApplyUp(this.targetVersion))
            {
                this.runner.MigrateUp(this.targetVersion);
                didMigrate = true;
            }
            else if (runner.HasMigrationsToApplyDown(this.targetVersion))
            {
                this.runner.MigrateDown(this.targetVersion);
                didMigrate = true;
            }

            return didMigrate;
        }
    }
}
