using System;
using System.Collections.Generic;

namespace Pims.Dal.Exceptions
{
    public class UserOverrideCode
    {
        public static UserOverrideCode AddPropertyToInventory
        {
            get { return new UserOverrideCode("ADD_PROPERTY_TO_INVENTORY"); }
        }

        public static UserOverrideCode AddLocationToProperty
        {
            get { return new UserOverrideCode("ADD_LOCATION_TO_PROPERTY"); }
        }

        public static UserOverrideCode UpdateRegion
        {
            get { return new UserOverrideCode("UPDATE_REGION"); }
        }

        public static UserOverrideCode PoiToInventory
        {
            get { return new UserOverrideCode("PROPERTY_OF_INTEREST_TO_INVENTORY"); }
        }

        public static UserOverrideCode ContractorSelfRemoved
        {
            get { return new UserOverrideCode("CONTRACTOR_SELFREMOVED"); }
        }

        public string Code { get; private set; }

        private static List<UserOverrideCode> UserOverrideCodes
        {
            get { return new List<UserOverrideCode>() { UserOverrideCode.AddPropertyToInventory, UserOverrideCode.AddLocationToProperty, UserOverrideCode.UpdateRegion, UserOverrideCode.PoiToInventory }; }
        }

        private UserOverrideCode(string code)
        {
            Code = code;
        }

        public static UserOverrideCode Parse(string toParse)
        {
            if (toParse == null)
            {
                return null;
            }
            foreach (UserOverrideCode overrideCode in UserOverrideCodes)
            {
                if (toParse == overrideCode.Code)
                {
                    return overrideCode;
                }
            }
            throw new InvalidOperationException("Invalid User Override Code Provided");
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            UserOverrideCode userOverrideCode = obj as UserOverrideCode;
            return this.Code == userOverrideCode.Code;
        }

        public override int GetHashCode()
        {
            return this.Code.GetHashCode();
        }
    }
}
