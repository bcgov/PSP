/**
 * File autogenerated by TsGenerator.
 * Do not manually modify, changes made to this file will be lost when this file is regenerated.
 * Generated on 07/12/2023 13:34
 */
import { ApiGen_BaseConcurrent } from './ApiGen_BaseConcurrent';

// LINK: @backend/apimodels/Models/Concepts/Claim/ClaimModel.cs
export interface ApiGen_Claim extends ApiGen_BaseConcurrent {
  id: number;
  key: string | null;
  name: string | null;
  keycloakRoleId: string | null;
  description: string | null;
  isDisabled: boolean;
}