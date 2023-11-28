import { Api_File } from '@/models/api/File';
import Api_TypeCode from '@/models/api/TypeCode';

import { Api_AuditFields } from './AuditFields';
import { Api_ConcurrentVersion } from './ConcurrentVersion';
import { Api_Organization } from './Organization';
import { Api_Person } from './Person';
import { Api_Product, Api_Project } from './Project';
import { Api_PropertyFile } from './PropertyFile';

// LINK @backend/api/Models/Concepts/DispositionFile/DispositionFileModel.cs
export interface Api_DispositionFile extends Api_ConcurrentVersion, Api_AuditFields, Api_File {
  id?: number;
  referenceNumber: string | null;
  assignedDate: string | null;
  initiatingDocumentDate: string | null;
  completionDate: string | null;
  dispositionTypeOther: string | null;
  initiatingDocumentTypeOther: string | null;
  // Code Tables
  physicalFileStatusTypeCode?: Api_TypeCode<string> | null;
  dispositionStatusTypeCode: Api_TypeCode<string> | null;
  initiatingBranchTypeCode?: Api_TypeCode<string> | null;
  fundingTypeCode?: Api_TypeCode<string> | null;
  initiatingDocumentTypeCode?: Api_TypeCode<string> | null;
  dispositionTypeCode: Api_TypeCode<string>;
  // MOTI region
  regionCode: Api_TypeCode<number>;
  dispositionTeam: Api_DispositionFileTeam[];
  project: Api_Project | null;
  projectId: number | null;
  product: Api_Product | null;
  productId: number | null;
}

export interface Api_DispositionFileProperty
  extends Api_ConcurrentVersion,
    Api_PropertyFile,
    Api_AuditFields {}

export interface Api_DispositionFileTeam extends Api_ConcurrentVersion, Api_AuditFields {
  id?: number;
  dispositionFileId: number;
  personId?: number;
  person?: Api_Person;
  organizationId?: number;
  organization?: Api_Organization;
  primaryContactId?: number;
  primaryContact?: Api_Person;
  teamProfileTypeCode?: string;
  teamProfileType?: Api_TypeCode<string>;
}
