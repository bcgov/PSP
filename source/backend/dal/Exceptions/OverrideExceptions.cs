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

        public static UserOverrideCode ProductReuse
        {
            get { return new UserOverrideCode("PRODUCT_REUSE"); }
        }

        public static UserOverrideCode DisposingPropertyNotInventoried
        {
            get { return new UserOverrideCode("DISPOSING_PROPERTY_NOT_INVENTORIED"); }
        }

        public static UserOverrideCode ManagingPropertyNotInventoried
        {
            get { return new UserOverrideCode("MANAGING_PROPERTY_NOT_INVENTORIED"); }
        }

        public static UserOverrideCode DispositionFileFinalStatus
        {
            get { return new UserOverrideCode("DISPOSITION_FILE_FINAL_STATUS"); }
        }

        public static UserOverrideCode DisposeOfProperties
        {
            get { return new UserOverrideCode("DISPOSE_OF_PROPERTIES"); }
        }

        public static UserOverrideCode DeleteCompletedTake
        {
            get { return new UserOverrideCode("DELETE_COMPLETED_TAKE"); }
        }

        public static UserOverrideCode DeleteLastTake
        {
            get { return new UserOverrideCode("DELETE_LAST_TAKE"); }
        }

        public static UserOverrideCode DeleteTakeActiveDisposition
        {
            get { return new UserOverrideCode("DELETE_TAKE_ACTIVE_DISPOSITION"); }
        }

        public static UserOverrideCode UpdateSubFilesProjectProduct
        {
            get { return new UserOverrideCode("UPDATE_SUBFILES_PROJECT_PRODUCT"); }
        }

        public static UserOverrideCode CommencementOverlapExpiryDate
        {
            get { return new UserOverrideCode("COMMENCEMENT_DATE_OVERLAP_EXPIRY_DATE"); }
        }

        public string Code { get; private set; }

        private static List<UserOverrideCode> UserOverrideCodes => new List<UserOverrideCode>()
        {
            UserOverrideCode.AddPropertyToInventory,
            UserOverrideCode.AddLocationToProperty,
            UserOverrideCode.UpdateRegion,
            UserOverrideCode.PoiToInventory,
            UserOverrideCode.ContractorSelfRemoved,
            UserOverrideCode.ProductReuse,
            UserOverrideCode.DisposingPropertyNotInventoried,
            UserOverrideCode.DispositionFileFinalStatus,
            UserOverrideCode.DisposeOfProperties,
            UserOverrideCode.DeleteCompletedTake,
            UserOverrideCode.DeleteLastTake,
            UserOverrideCode.DeleteTakeActiveDisposition,
            UserOverrideCode.UpdateSubFilesProjectProduct,
            UserOverrideCode.CommencementOverlapExpiryDate,
            UserOverrideCode.ManagingPropertyNotInventoried,
        };

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
