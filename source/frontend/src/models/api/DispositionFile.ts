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
  fileReference: string | null;
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
  dispositionTypeCode: Api_TypeCode<string> | null;
  // MOTI region
  regionCode: Api_TypeCode<number> | null;
  dispositionTeam: Api_DispositionFileTeam[];
  project: Api_Project | null;
  projectId: number | null;
  product: Api_Product | null;
  productId: number | null;
  // Appraisal
  appraisedValueAmount: number | null;
  appraisalDate: string | null;
  bcaValueAmount: number | null;
  bcaRollYear: string | null;
  listPriceAmount: number | null;
  // Offers
  dispositionOffers: Api_DispositionFileOffer[];
  // Sale
  dispositionSales: Api_DispositionFileSale[];
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

export interface Api_DispositionFileOffer {
  id: number | null;
  dispositionFileId: number;
  dispositionOfferStatusTypeCode: string | null;
  dispositionOfferStatusType: Api_TypeCode<string> | null;
  offerName: string | null;
  offerDate: string | null;
  offerExpiryDate: string | null;
  offerAmount: number | null;
  offerNote: string | null;
}

export interface Api_DispositionFileSale {
  id: number | null;
  dispositionFileId: number;
  finalConditionRemovalDate: string | null;
  saleCompletionDate: string | null;
  saleFiscalYear: string | null;
  finalSaleAmount: number | null;
  realtorCommissionAmount: number | null;
  isGstRequired: boolean | null;
  gstCollectedAmount: number | null;
  netBookAmount: number | null;
  totalCostAmount: number | null;
  netProceedsBeforeSppAmount: number | null;
  sppAmount: number | null;
  netProceedsAfterSppAmount: number | null;
  remediationAmount: number | null;
  dispositionPurchasers: Api_DispositionSalePurchaser[];
  dispositionPurchaserAgents: Api_DispositionSalePurchaserAgent[];
  dispositionPurchaserSolicitors: Api_DispositionSalePurchaserSolicitor[];
}

export interface ContactInformation {
  personId: number | null;
  person: Api_Person | null;
  organizationId: number | null;
  organization: Api_Organization | null;
  primaryContactId: number | null;
  primaryContact: Api_Person | null;
}

export interface Api_DispositionSalePurchaser
  extends ContactInformation,
    Api_ConcurrentVersion,
    Api_AuditFields {
  id?: number;
  dispositionSaleId: number;
}

export interface Api_DispositionSalePurchaserAgent
  extends ContactInformation,
    Api_ConcurrentVersion,
    Api_AuditFields {
  id?: number;
  dispositionSaleId: number;
}

export interface Api_DispositionSalePurchaserSolicitor
  extends ContactInformation,
    Api_ConcurrentVersion,
    Api_AuditFields {
  id?: number;
  dispositionSaleId: number;
}
