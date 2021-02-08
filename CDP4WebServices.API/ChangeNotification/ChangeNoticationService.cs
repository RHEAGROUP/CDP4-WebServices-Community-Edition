// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeNoticationService.cs" company="RHEA System S.A.">
//    Copyright (c) 2015-2021 RHEA System S.A.
//
//    Author: Sam Gerené, Alex Vorobiev, Alexander van Delft
//
//    This file is part of CDP4 Web Services Community Edition. 
//    The CDP4 Web Services Community Edition is the RHEA implementation of ECSS-E-TM-10-25 Annex A and Annex C.
//    This is an auto-generated class. Any manual changes to this file will be overwritten!
//
//    The CDP4 Web Services Community Edition is free software; you can redistribute it and/or
//    modify it under the terms of the GNU Affero General Public
//    License as published by the Free Software Foundation; either
//    version 3 of the License, or (at your option) any later version.
//
//    The CDP4 Web Services Community Edition is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
//    GNU Affero General Public License for more details.
//
//    You should have received a copy of the GNU Affero General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace CDP4WebServices.API.ChangeNotification
{
    using System;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Autofac;
    
    using CDP4JsonSerializer;

    using CDP4Orm.Dao;
    using CDP4Orm.Dao.Resolve;

    using CDP4WebServices.API.Configuration;
    using CDP4WebServices.API.Services;
    using CDP4WebServices.API.Services.Email;

    using NLog;
    using NLog.Fluent;

    using Npgsql;

    /// <summary>
    /// The purpose of the <see cref="ChangeNoticationService"/> is to send email notifications to users
    /// </summary>
    public class ChangeNoticationService
    {
        /// <summary>
        /// A <see cref="NLog.Logger"/> instance
        /// </summary>
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The DI container used to resolve the services required to interact with the database
        /// </summary>
        private readonly IContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeNoticationService"/>
        /// </summary>
        public ChangeNoticationService()
        {
            this.container = this.RegisterServices();
        }

        /// <summary>
        /// Register the required services to interact with the database
        /// </summary>
        /// <returns></returns>
        public IContainer RegisterServices()
        {
            var builder = new ContainerBuilder();

            builder.RegisterTypeAsPropertyInjectedSingleton<DataModelUtils, IDataModelUtils>();

            // wireup command logger for this request
            builder.RegisterTypeAsPropertyInjectedSingleton<CommandLogger, ICommandLogger>();

            // wireup class meta info provider
            builder.RegisterTypeAsPropertyInjectedSingleton<MetaInfoProvider, IMetaInfoProvider>();

            // wireup class cdp4JsonSerializer
            builder.RegisterTypeAsPropertyInjectedSingleton<Cdp4JsonSerializer, ICdp4JsonSerializer>();

            // the ResolveDao is used to get type info on any Thing instance based on it's unique identifier
            builder.RegisterTypeAsPropertyInjectedSingleton<ResolveDao, IResolveDao>();

            // wireup DAO classes
            builder.RegisterDerivedTypesAsPropertyInjectedSingleton<BaseDao>();

            builder.RegisterTypeAsPropertyInjectedSingleton<EmailService, IEmailService>();

            return builder.Build();
        }

        /// <summary>
        /// Executes the <see cref="ChangeNoticationService"/>, processes all 
        /// </summary>
        /// <returns></returns>
        public async Task Execute()
        {
            NpgsqlConnection connection = null;
            NpgsqlTransaction transaction = null;
            var sw = Stopwatch.StartNew();

            connection = new NpgsqlConnection(Services.Utils.GetConnectionString(AppConfig.Current.Backtier.Database));

            // ensure an open connection
            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                    transaction = connection.BeginTransaction();

                    var personDao = this.container.Resolve<IPersonDao>();
                    var userPreferenceDao = this.container.Resolve<IUserPreferenceDao>();

                    var persons = personDao.Read(transaction, "SiteDirectory", null, true).ToList();

                    foreach (var person in persons)
                    {
                        if (!person.IsActive)
                        {
                            continue;
                        }

                        var userPreferences = userPreferenceDao.Read(transaction, "SiteDirectory", person.UserPreference, true).ToList();

                        var changeLogSubscriptions = userPreferences.Where(x => x.ShortName.StartsWith("ChangeLogSubscriptions_")).ToList();

                        // if a user is no longer a participant in a model, or if the participant is not active, then do not send report
                        var participantDao = this.container.Resolve<IParticipantDao>();

                        // if a model does not exist anyamore, do not send report
                        var engineeringModelSetupDao = this.container.Resolve<IEngineeringModelSetupDao>();
                    }
                }
                catch (PostgresException postgresException)
                {
                    Logger.Error("Could not connect to the database to process Change Notifications. Error message: {0}", postgresException.Message);
                }
                finally
                {
                    if (connection?.State == ConnectionState.Open)
                    {
                        await connection.CloseAsync();
                    }

                    Logger.Info($"ChangeNotifications processed in {sw.ElapsedMilliseconds} [ms]");
                }
            }
        }
    }
}
