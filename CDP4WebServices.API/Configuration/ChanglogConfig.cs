﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangelogConfig.cs" company="RHEA System S.A.">
//    Copyright (c) 2015-2021 RHEA System S.A.
//
//    Author: Sam Gerené, Alex Vorobiev, Alexander van Delft, Nathanael Smiechowski, Ahmed Abulwafa Ahmed
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
//    Lesser General Public License for more details.
//
//    You should have received a copy of the GNU Affero General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace CDP4WebServices.API.Configuration
{
    using CDP4Common.DTO;

    /// <summary>
    /// The change log configuration.
    /// </summary>
    public class ChangelogConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangelogConfig"/> class.
        /// </summary>
        public ChangelogConfig()
        {
            // set defaults
            this.CollectChanges = false;
        }

        /// <summary>
        /// Gets or sets the Changelog collection setting.
        /// If set to true, <see cref="ModelLogEntry"/>s will automatically be created for specific changes to <see cref="Thing"/>s.
        /// </summary>
        /// <remarks>
        /// The default value is false
        /// </remarks>
        public bool CollectChanges { get; set; }
    }
}
