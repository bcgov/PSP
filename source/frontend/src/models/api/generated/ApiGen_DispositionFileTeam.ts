/**
 * File autogenerated by TsGenerator.
 * Do not manually modify, changes made to this file will be lost when this file is regenerated.
 * Generated on 07/12/2023 13:34
 */
import { ApiGen_BaseConcurrent } from './ApiGen_BaseConcurrent';
import { ApiGen_Organization } from './ApiGen_Organization';
import { ApiGen_Person } from './ApiGen_Person';
import { ApiGen_Type } from './ApiGen_Type';

// LINK: @backend/apimodels/Models/Concepts/DispositionFile/DispositionFileTeamModel.cs
export interface ApiGen_DispositionFileTeam extends ApiGen_BaseConcurrent {
  id: number;
  dispositionFileId: number;
  personId: number | null;
  person: ApiGen_Person | null;
  organizationId: number | null;
  organization: ApiGen_Organization | null;
  primaryContactId: number | null;
  primaryContact: ApiGen_Person | null;
  teamProfileTypeCode: string | null;
  teamProfileType: ApiGen_Type<string> | null;
}