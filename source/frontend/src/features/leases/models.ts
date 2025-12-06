import isNumber from 'lodash/isNumber';

import {
  fromApiOrganization,
  fromApiPerson,
  IAutocompletePrediction,
  IContactSearchResult,
} from '@/interfaces';
import { ApiGen_CodeTypes_AreaUnitTypes } from '@/models/api/generated/ApiGen_CodeTypes_AreaUnitTypes';
import { ApiGen_CodeTypes_LeasePaymentReceivableTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePaymentReceivableTypes';
import { ApiGen_CodeTypes_LeasePurposeTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePurposeTypes';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_LeaseFileTeam } from '@/models/api/generated/ApiGen_Concepts_LeaseFileTeam';
import { ApiGen_Concepts_LeaseRenewal } from '@/models/api/generated/ApiGen_Concepts_LeaseRenewal';
import { ApiGen_Concepts_PropertyLease } from '@/models/api/generated/ApiGen_Concepts_PropertyLease';
import { EpochIsoDateTime, UtcIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { NumberFieldValue } from '@/typings/NumberFieldValue';
import { applyDisplayOrder, enumFromValue, exists, isValidId, isValidIsoDateTime } from '@/utils';
import {
  emptyStringToNull,
  fromTypeCode,
  stringToNull,
  stringToNumberOrNull,
  toTypeCodeNullable,
} from '@/utils/formUtils';

import { FileForm, PropertyForm } from '../mapSideBar/shared/models';
import { ChecklistItemFormModel } from '../mapSideBar/shared/tabs/checklist/update/models';
import { FormLeaseDeposit } from './detail/LeasePages/deposits/models/FormLeaseDeposit';
import { FormLeaseDepositReturn } from './detail/LeasePages/deposits/models/FormLeaseDepositReturn';
import { FormLeasePeriod } from './detail/LeasePages/payment/models';
import { FormStakeholder } from './detail/LeasePages/stakeholders/models';
import { LeasePurposeModel } from './models/LeasePurposeModel';

export class FormLeaseRenewal {
  id = 0;
  leaseId = 0;
  commencementDt: UtcIsoDateTime = '';
  expiryDt: UtcIsoDateTime = '';
  isExercised = false;
  renewalNote = '';
  rowVersion = 0;

  static fromApi(apiModel?: ApiGen_Concepts_LeaseRenewal): FormLeaseRenewal {
    const renewal = new FormLeaseRenewal();

    renewal.id = apiModel.id;
    renewal.leaseId = apiModel.leaseId;
    renewal.commencementDt = apiModel.commencementDt || '';
    renewal.expiryDt = apiModel.expiryDt || '';
    renewal.isExercised = apiModel.isExercised || false;
    renewal.renewalNote = apiModel.renewalNote || '';
    renewal.rowVersion = apiModel.rowVersion || 0;

    return renewal;
  }

  toApi(): ApiGen_Concepts_LeaseRenewal {
    return {
      id: this.id,
      leaseId: this.leaseId,
      lease: null,
      commencementDt: emptyStringToNull(this.commencementDt),
      expiryDt: emptyStringToNull(this.expiryDt),
      isExercised: this.isExercised,
      renewalNote: emptyStringToNull(this.renewalNote),
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }
}

export class LeaseFormModel extends FileForm implements WithLeaseTeam {
  lFileNo = '';
  psFileNo = '';
  tfaFileNumber = '';
  expiryDate = '';
  startDate = '';
  renewals: FormLeaseRenewal[] = [];
  terminationDate = '';
  responsibilityEffectiveDate = '';
  paymentReceivableTypeCode = '';
  purposes: LeasePurposeModel[] = [];
  purposeOtherDescription: string | null = '';
  responsibilityTypeCode = '';
  initiatorTypeCode = '';
  leaseTypeCode = '';
  statusTypeCode = '';
  regionId = '';
  programTypeCode = '';
  otherLeaseTypeDescription = '';
  otherProgramTypeDescription = '';
  otherCategoryTypeDescription = '';
  programName = '';
  motiName = '';
  amount: NumberFieldValue = '';
  renewalCount: NumberFieldValue = '';
  description = '';
  isResidential = false;
  isCommercialBuilding = false;
  isOtherImprovement = false;
  returnNotes = ''; // security deposit notes (free form text)
  documentationReference = '';
  hasPhysicalLicense?: boolean;
  hasDigitalLicense?: boolean;
  cancellationReason: string | null = null;
  terminationReason: string | null = null;
  project?: IAutocompletePrediction;
  productId: number | null;
  tenantNotes: string[] = [];
  properties: PropertyForm[] = [];
  securityDeposits: FormLeaseDeposit[] = [];
  securityDepositReturns: FormLeaseDepositReturn[] = [];
  periods: FormLeasePeriod[] = [];
  stakeholders: FormStakeholder[] = [];
  fileChecklist: ChecklistItemFormModel[] = [];
  team: LeaseTeamFormModel[] = [];
  primaryArbitrationCity: string | null;
  isPublicBenefit: boolean;
  isFinancialGain: boolean;
  feeDeterminationNote: string | null = null;
  rowVersion = 0;

  static fromApi(apiModel?: ApiGen_Concepts_Lease): LeaseFormModel {
    const leaseDetail = new LeaseFormModel();

    leaseDetail.id = apiModel?.id ?? undefined;
    leaseDetail.lFileNo = apiModel?.lFileNo || '';
    leaseDetail.psFileNo = apiModel?.psFileNo || '';
    leaseDetail.tfaFileNumber = apiModel?.tfaFileNumber || '';
    leaseDetail.expiryDate = isValidIsoDateTime(apiModel?.expiryDate) ? apiModel.expiryDate : '';
    leaseDetail.startDate = isValidIsoDateTime(apiModel?.startDate) ? apiModel.startDate : '';
    leaseDetail.terminationDate = isValidIsoDateTime(apiModel?.terminationDate)
      ? apiModel.terminationDate
      : '';
    leaseDetail.responsibilityEffectiveDate = apiModel?.responsibilityEffectiveDate || '';
    leaseDetail.amount = parseFloat(apiModel?.amount?.toString() ?? '') || 0.0;
    leaseDetail.paymentReceivableTypeCode = fromTypeCode(apiModel?.paymentReceivableType) || '';
    leaseDetail.purposes = apiModel?.leasePurposes?.map(x => LeasePurposeModel.fromApi(x)) ?? [];
    const otherPurpose =
      apiModel?.leasePurposes?.find(
        x => x.leasePurposeTypeCode.id === ApiGen_CodeTypes_LeasePurposeTypes.OTHER,
      ) ?? null;
    leaseDetail.purposeOtherDescription = otherPurpose ? otherPurpose.purposeOtherDescription : '';

    leaseDetail.responsibilityTypeCode = fromTypeCode(apiModel?.responsibilityType) || '';
    leaseDetail.initiatorTypeCode = fromTypeCode(apiModel?.initiatorType) || '';
    leaseDetail.statusTypeCode = fromTypeCode(apiModel?.fileStatusTypeCode) || '';
    leaseDetail.leaseTypeCode = fromTypeCode(apiModel?.type) || '';
    leaseDetail.regionId = fromTypeCode(apiModel?.region)?.toString() || '';
    leaseDetail.programTypeCode = fromTypeCode(apiModel?.programType) || '';
    leaseDetail.returnNotes = apiModel?.returnNotes || '';
    leaseDetail.documentationReference = apiModel?.documentationReference || '';
    leaseDetail.motiName = apiModel?.motiName || '';
    leaseDetail.hasDigitalLicense = apiModel?.hasDigitalLicense ?? undefined;
    leaseDetail.hasPhysicalLicense = apiModel?.hasPhysicalLicense ?? undefined;
    leaseDetail.properties =
      apiModel?.fileProperties?.map(p => LeaseFormModel.fromLeasePropertyApi(p)) ?? [];
    leaseDetail.isResidential = apiModel?.isResidential || false;
    leaseDetail.isCommercialBuilding = apiModel?.isCommercialBuilding || false;
    leaseDetail.isOtherImprovement = apiModel?.isOtherImprovement || false;
    leaseDetail.rowVersion = apiModel?.rowVersion || null;
    leaseDetail.description = apiModel?.description || '';
    leaseDetail.otherProgramTypeDescription = apiModel?.otherProgramType || '';
    leaseDetail.otherLeaseTypeDescription = apiModel?.otherType || '';
    leaseDetail.project = apiModel?.project
      ? { id: apiModel?.project?.id || 0, text: apiModel?.project?.description || '' }
      : undefined;

    leaseDetail.productId = apiModel?.productId;
    leaseDetail.periods = apiModel?.periods?.map(t => FormLeasePeriod.fromApi(t)) || [];
    leaseDetail.stakeholders = apiModel?.stakeholders?.map(t => new FormStakeholder(t)) || [];
    leaseDetail.renewals = apiModel?.renewals?.map(r => FormLeaseRenewal.fromApi(r)) || [];
    leaseDetail.team = apiModel?.leaseTeam?.map(t => LeaseTeamFormModel.fromApi(t));
    leaseDetail.cancellationReason = apiModel?.cancellationReason || '';
    leaseDetail.terminationReason = apiModel?.terminationReason || '';
    leaseDetail.primaryArbitrationCity = apiModel?.primaryArbitrationCity;
    leaseDetail.isPublicBenefit = apiModel?.isPublicBenefit;
    leaseDetail.isFinancialGain = apiModel?.isFinancialGain;
    leaseDetail.feeDeterminationNote = apiModel?.feeDeterminationNote;

    return leaseDetail;
  }

  public static toApi(formLease: LeaseFormModel): ApiGen_Concepts_Lease {
    const fileProperties =
      formLease.properties?.map(p => LeaseFormModel.toLeasePropertyApi(p)).filter(x => exists(x)) ??
      [];
    const sortedProperties = applyDisplayOrder(fileProperties);
    return {
      id: formLease.id ?? 0,
      lFileNo: stringToNull(formLease.lFileNo),
      psFileNo: stringToNull(formLease.psFileNo),
      tfaFileNumber: stringToNull(formLease.tfaFileNumber),
      expiryDate: isValidIsoDateTime(formLease.expiryDate) ? formLease.expiryDate : null,
      startDate: isValidIsoDateTime(formLease.startDate) ? formLease.startDate : null,
      terminationDate: isValidIsoDateTime(formLease.terminationDate)
        ? formLease.terminationDate
        : null,
      responsibilityEffectiveDate: isValidIsoDateTime(formLease.responsibilityEffectiveDate)
        ? formLease.responsibilityEffectiveDate
        : null,
      amount: parseFloat(formLease.amount?.toString() ?? '') || 0.0,
      paymentReceivableType: toTypeCodeNullable(formLease.paymentReceivableTypeCode) ?? null,
      leasePurposes: formLease.purposes?.map(x => x.toApi(formLease.id ?? 0)) ?? [],
      responsibilityType: toTypeCodeNullable(formLease.responsibilityTypeCode) ?? null,
      initiatorType: toTypeCodeNullable(formLease.initiatorTypeCode) ?? null,
      fileStatusTypeCode: toTypeCodeNullable(formLease.statusTypeCode) ?? null,
      type: toTypeCodeNullable(formLease.leaseTypeCode) ?? null,
      region: toTypeCodeNullable(Number(formLease.regionId)) ?? null,
      programType: toTypeCodeNullable(formLease.programTypeCode) ?? null,
      note: null,
      returnNotes: formLease.returnNotes,
      documentationReference: stringToNull(formLease.documentationReference),
      motiName: formLease.motiName,
      hasDigitalLicense: formLease.hasDigitalLicense ?? null,
      hasPhysicalLicense: formLease.hasPhysicalLicense ?? null,
      fileProperties: sortedProperties ?? [],
      isResidential: formLease.isResidential,
      isCommercialBuilding: formLease.isCommercialBuilding,
      isOtherImprovement: formLease.isOtherImprovement,
      description: stringToNull(formLease.description),
      otherProgramType: stringToNull(formLease.otherProgramTypeDescription),
      otherType: stringToNull(formLease.otherLeaseTypeDescription),
      projectId: stringToNumberOrNull(formLease?.project?.id),
      project: isValidId(formLease.project?.id) ? ({ id: formLease.project?.id } as any) : null,
      productId: stringToNumberOrNull(formLease.productId),
      product: null,
      consultations: null,
      stakeholders: formLease.stakeholders?.map(t => FormStakeholder.toApi(t)) ?? [],
      periods: formLease.periods?.map(t => FormLeasePeriod.toApi(t)) ?? [],
      renewals: formLease.renewals?.map(r => r.toApi()) ?? [],
      leaseTeam: formLease.team?.map(t => t?.toApi(formLease?.id)) ?? [],
      fileName: null,
      fileNumber: null,
      hasDigitalFile: formLease.hasDigitalLicense ?? false,
      hasPhysicalFile: formLease.hasPhysicalLicense ?? false,
      cancellationReason: stringToNull(formLease.cancellationReason),
      terminationReason: stringToNull(formLease.terminationReason),
      primaryArbitrationCity: stringToNull(formLease.primaryArbitrationCity),
      isPublicBenefit: formLease.isPublicBenefit ?? null,
      isFinancialGain: formLease.isFinancialGain ?? null,
      feeDeterminationNote: stringToNull(formLease.feeDeterminationNote),
      fileChecklistItems: formLease.fileChecklist?.map(ck => ck.toApi()) ?? [],
      isExpired: false,
      programName: null,
      renewalCount: formLease.renewals?.length,
      totalAllowableCompensation: null,
      ...getEmptyBaseAudit(formLease.rowVersion),
    };
  }

  private static fromLeasePropertyApi(
    apiPropertyLease: ApiGen_Concepts_PropertyLease,
  ): PropertyForm {
    const propertyForm = PropertyForm.fromApi(apiPropertyLease);
    propertyForm.landArea = apiPropertyLease.leaseArea ?? 0;
    propertyForm.areaUnit = enumFromValue(
      apiPropertyLease.areaUnitType?.id ?? ApiGen_CodeTypes_AreaUnitTypes.M2,
      ApiGen_CodeTypes_AreaUnitTypes,
    );
    return propertyForm;
  }

  private static toLeasePropertyApi(property: PropertyForm): ApiGen_Concepts_PropertyLease {
    const apiFileProperty = property.toFilePropertyApi();
    const leaseFileProperty: ApiGen_Concepts_PropertyLease = {
      ...apiFileProperty,
      leaseArea: isNumber(property.landArea) ? property.landArea : 0,
      areaUnitType: isNumber(property.landArea)
        ? toTypeCodeNullable(property.areaUnit) ?? null
        : null,
      file: null,
    };
    return leaseFileProperty;
  }
}

export interface WithLeaseTeam {
  team: LeaseTeamFormModel[];
}

export class LeaseTeamFormModel {
  id?: number;
  rowVersion?: number;
  contact?: IContactSearchResult;
  contactTypeCode: string;
  primaryContactId = '';

  constructor(contactTypeCode: string, id?: number, contact?: IContactSearchResult) {
    this.id = id;
    this.contactTypeCode = contactTypeCode;
    this.contact = contact;
  }

  toApi(leaseId: number): ApiGen_Concepts_LeaseFileTeam | null {
    const personId = this.contact?.personId ?? null;
    const organizationId = !personId ? this.contact?.organizationId ?? null : null;
    if (!isValidId(personId) && !isValidId(organizationId)) {
      return null;
    }

    return {
      id: this.id ?? 0,
      leaseId: leaseId,
      personId: personId ?? null,
      person: null,
      organizationId: organizationId ?? null,
      organization: null,
      primaryContactId:
        !!this.primaryContactId && isNumber(+this.primaryContactId)
          ? Number(this.primaryContactId)
          : null,
      teamProfileType: toTypeCodeNullable(this.contactTypeCode),
      teamProfileTypeCode: this.contactTypeCode,
      primaryContact: null,
      ...getEmptyBaseAudit(this.rowVersion),
    };
  }

  static fromApi(model: ApiGen_Concepts_LeaseFileTeam | null): LeaseTeamFormModel {
    // The method 'exists' below allows the compiler to validate the child property. This works correctly in typescript 5.3+
    const contact: IContactSearchResult | undefined = exists(model?.person)
      ? fromApiPerson(model.person)
      : exists(model?.organization)
      ? fromApiOrganization(model.organization)
      : undefined;

    const newForm = new LeaseTeamFormModel(
      fromTypeCode(model?.teamProfileType) || '',
      model?.id ?? 0,
      contact,
    );

    if (model?.primaryContactId) {
      newForm.primaryContactId = model.primaryContactId.toString();
    }

    newForm.rowVersion = model?.rowVersion ?? 0;

    return newForm;
  }
}

export const getDefaultFormLease: () => LeaseFormModel = () =>
  LeaseFormModel.fromApi({
    fileProperties: [],
    stakeholders: [],
    startDate: EpochIsoDateTime,
    expiryDate: EpochIsoDateTime,
    terminationDate: null,
    lFileNo: '',
    tfaFileNumber: '',
    psFileNo: '',
    motiName: '',
    amount: 0,
    isResidential: false,
    isCommercialBuilding: false,
    isOtherImprovement: false,
    returnNotes: '',
    hasDigitalLicense: null,
    hasPhysicalLicense: null,
    fileStatusTypeCode: toTypeCodeNullable(ApiGen_CodeTypes_LeaseStatusTypes.DRAFT),
    paymentReceivableType: toTypeCodeNullable(ApiGen_CodeTypes_LeasePaymentReceivableTypes.RCVBL),
    leasePurposes: [],
    programType: null,
    type: null,
    initiatorType: null,
    responsibilityType: null,
    region: null,
    otherType: null,
    otherProgramType: null,
    consultations: [],
    periods: [],
    id: 0,
    programName: null,
    documentationReference: null,
    note: null,
    description: null,
    renewalCount: 0,
    responsibilityEffectiveDate: null,
    hasPhysicalFile: false,
    hasDigitalFile: false,
    cancellationReason: null,
    terminationReason: null,
    isExpired: false,
    projectId: null,
    project: null,
    productId: null,
    product: null,
    fileName: null,
    fileNumber: null,
    fileChecklistItems: [],
    isPublicBenefit: null,
    isFinancialGain: null,
    feeDeterminationNote: null,
    renewals: [],
    primaryArbitrationCity: null,
    ...getEmptyBaseAudit(),
    totalAllowableCompensation: null,
    leaseTeam: [],
  });

export const isLeaseFile = (file: object): file is ApiGen_Concepts_Lease =>
  exists(file) && Object.hasOwn(file, 'paymentReceivableType');
