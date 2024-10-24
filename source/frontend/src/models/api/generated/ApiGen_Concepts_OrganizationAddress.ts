/**
 * File autogenerated by TsGenerator.
 * Do not manually modify, changes made to this file will be lost when this file is regenerated.
 */
import { ApiGen_Base_BaseConcurrent } from './ApiGen_Base_BaseConcurrent';
import { ApiGen_Base_CodeType } from './ApiGen_Base_CodeType';
import { ApiGen_Concepts_Address } from './ApiGen_Concepts_Address';

// LINK: @backend/apimodels/Models/Concepts/Organization/OrganizationAddressModel.cs
export interface ApiGen_Concepts_OrganizationAddress extends ApiGen_Base_BaseConcurrent {
  id: number;
  organizationId: number;
  address: ApiGen_Concepts_Address | null;
  addressUsageType: ApiGen_Base_CodeType<string> | null;
}
