/**
 * File autogenerated by TsGenerator.
 * Do not manually modify, changes made to this file will be lost when this file is regenerated.
 * Generated on 07/12/2023 13:34
 */
import { ApiGen_BaseAudit } from './ApiGen_BaseAudit';
import { ApiGen_Person } from './ApiGen_Person';
import { ApiGen_PropertyActivity } from './ApiGen_PropertyActivity';

// LINK: @backend/apimodels/Models/Concepts/Property/PropertyMinistryContactModel.cs
export interface ApiGen_PropertyMinistryContact extends ApiGen_BaseAudit {
  id: number;
  personId: number;
  person: ApiGen_Person | null;
  propertyActivityId: number;
  propertyActivity: ApiGen_PropertyActivity | null;
}