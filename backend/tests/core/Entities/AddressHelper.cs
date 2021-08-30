using System;
using System.Linq;
using Pims.Dal;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of an Address.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Entity.Address CreateAddress(long id)
        {
            return CreateAddress(id, "1234 St", "", "", null, null, "V9V9V9");
        }

        /// <summary>
        /// Create a new instance of an Address.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <param name="unitNumber"></param>
        /// <param name="municipality"></param>
        /// <param name="postal"></param>
        /// <returns></returns>
        public static Entity.Address CreateAddress(long id, string address, string unitNumber, string municipality, string postal)
        {
            return CreateAddress(id, address, unitNumber, municipality, null, null, postal);
        }

        /// <summary>
        /// Create a new instance of an Address.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <param name="unitNumber"></param>
        /// <param name="province"></param>
        /// <param name="district"></param>
        /// <param name="postal"></param>
        /// <returns></returns>
        public static Entity.Address CreateAddress(long id, string address, Entity.Province province = null, Entity.District district = null, string postal = "V9V9V9")
        {
            return CreateAddress(id, address, null, null, province, district, postal);
        }

        /// <summary>
        /// Create a new instance of an Address.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <param name="unitNumber"></param>
        /// <param name="municipality"></param>
        /// <param name="province"></param>
        /// <param name="district"></param>
        /// <param name="postal"></param>
        /// <returns></returns>
        public static Entity.Address CreateAddress(long id, string address, string unitNumber, string municipality, Entity.Province province = null, Entity.District district = null, string postal = "V9V9V9")
        {
            province ??= EntityHelper.CreateProvince((int)id, "BC", EntityHelper.CreateCountry((int)id, "CAN"));
            district ??= EntityHelper.CreateDistrict((int)id, "District 1");
            municipality ??= "municipality";
            return new Entity.Address(address, unitNumber, municipality, province, district, postal)
            {
                Id = id,
                RowVersion = 1
            };
        }

        /// <summary>
        /// Create a new instance of an Address.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="address"></param>
        /// <param name="province"></param>
        /// <param name="district"></param>
        /// <param name="postal"></param>
        /// <returns></returns>
        public static Entity.Address CreateAddress(this PimsContext context, long id, string address, Entity.Province province = null, Entity.District district = null, string postal = "")
        {
            province ??= context.Provinces.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a province.");
            district ??= context.Districts.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find a district.");
            return new Entity.Address(address, null, "municipality", province, district, postal)
            {
                Id = id,
                RowVersion = 1
            };
        }
    }
}

