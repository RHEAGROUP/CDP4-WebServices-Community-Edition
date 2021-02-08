// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeNotificationSubscriptionType.cs" company="RHEA System S.A.">
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
    using CDP4Common.CommonData;
    using CDP4Common.EngineeringModelData;
    using CDP4Common.SiteDirectoryData;

    /// <summary>
    /// The type of <see cref="ChangeNotificationSubscription"/>
    /// </summary>
    public enum ChangeNotificationSubscriptionType
    {
        /// <summary>
        /// Assertion that the <see cref="ChangeNotificationSubscription"/> is on a <see cref="INamedThing"/>
        /// </summary>
        NamedThing,

        /// <summary>
        /// Assertion that the <see cref="ChangeNotificationSubscription"/> is on a <see cref="ICategorizableThing"/>
        /// </summary>
        Category,

        /// <summary>
        /// Assertion that the <see cref="ChangeNotificationSubscription"/> is on a <see cref="IOwnedThing"/>
        /// </summary>
        OwnedThing,

        /// <summary>
        /// Assertion that the <see cref="ChangeNotificationSubscription"/> is on a <see cref="ParameterType"/>
        /// </summary>
        ParameterType,

        /// <summary>
        /// Assertion that the <see cref="ChangeNotificationSubscription"/> is on a <see cref="ParameterSubscription"/>
        /// </summary>
        ParameterSubscription,

        /// <summary>
        /// Assertion that the <see cref="ChangeNotificationSubscription"/> is on a <see cref="Parameter"/>
        /// </summary>
        Parameter
    }
}
