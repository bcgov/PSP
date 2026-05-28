using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using Pims.Dal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a Province.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public static Entity.PimsProvinceState CreateProvince(short id, string code, Entity.PimsCountry country = null)
        {
            country ??= EntityHelper.CreateCountry(1, "CAN");
            return new Entity.PimsProvinceState(code, country) { ProvinceStateId = id, ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now, Description = "desc" };
        }

        /// <summary>
        /// Creates a default list of Province.
        /// </summary>
        /// <returns></returns>
        public static List<Entity.PimsProvinceState> CreateDefaultProvinces()
        {
            return new List<Entity.PimsProvinceState>()
            {
                new () { ProvinceStateId = 1, CountryId = 1, ProvinceStateCode = "BC", Description = "British Columbia", ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now },
                new () { ProvinceStateId = 2, CountryId = 1, ProvinceStateCode = "AB", Description = "Alberta", ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now },
                new () { ProvinceStateId = 3, CountryId = 1, ProvinceStateCode = "MB", Description = "Manitoba", ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now },
                new () { ProvinceStateId = 4, CountryId = 1, ProvinceStateCode = "NL", Description = "Newfoundland", ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now },
                new () { ProvinceStateId = 5, CountryId = 1, ProvinceStateCode = "NB", Description = "New Brunswick", ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now },
                new () { ProvinceStateId = 6, CountryId = 1, ProvinceStateCode = "NS", Description = "Nova Scotia", ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now },
                new () { ProvinceStateId = 7, CountryId = 1, ProvinceStateCode = "NT", Description = "North West Territories", ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now },
                new () { ProvinceStateId = 8, CountryId = 1, ProvinceStateCode = "NU", Description = "Nunavut", ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now },
                new () { ProvinceStateId = 9, CountryId = 1, ProvinceStateCode = "ON", Description = "Ontario", ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now },
                new () { ProvinceStateId = 10, CountryId = 1, ProvinceStateCode = "PE", Description = "Prince Edward Island", ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now },
                new () { ProvinceStateId = 11, CountryId = 1, ProvinceStateCode = "QC", Description = "Quebec", ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now },
                new () { ProvinceStateId = 12, CountryId = 1, ProvinceStateCode = "SK", Description = "Saskatchewan", ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now },
                new () { ProvinceStateId = 13, CountryId = 1, ProvinceStateCode = "YT", Description = "Yukon Territory", ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now },

            };
        }

        /// <summary>
        /// Create a new instance of a Province.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="code"></param>
        /// <param name="country"></param>
        /// <returns></returns>
        public static Entity.PimsProvinceState CreateProvince(this PimsContext context, short id, string code, Entity.PimsCountry country = null)
        {
            country ??= context.PimsCountries.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a country.");
            return new Entity.PimsProvinceState(code, country) { ProvinceStateId = id, ConcurrencyControlNumber = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", DbLastUpdateTimestamp = System.DateTime.Now, Description = "desc" };
        }
    }
}
