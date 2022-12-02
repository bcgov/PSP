import { LeaseInitiatorTypes } from 'constants/leaseInitiatorTypes';
import {
  DetailAcquisitionFile,
  DetailAcquisitionFilePerson,
} from 'features/properties/map/acquisition/detail/models';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_Lease } from 'models/api/Lease';
import { Api_Property } from 'models/api/Property';
import { Api_PropertyLease } from 'models/api/PropertyLease';
import Api_TypeCode from 'models/api/TypeCode';
import { NumberFieldValue } from 'typings/NumberFieldValue';
import { fromTypeCode, stringToNull, toTypeCode } from 'utils/formUtils';
import { pidParser } from 'utils/propertyUtils';

import { getPrettyLatLng } from './../properties/selector/utils';

export class LeaseFormModel {
  id?: number;
  lFileNo: string = '';
  psFileNo: string = '';
  tfaFileNumber: string = '';
  expiryDate: string = '';
  renewalDate: string = '';
  startDate: string = '';
  responsibilityEffectiveDate: string = '';
  paymentReceivableTypeCode: string = '';
  categoryTypeCode: string = '';
  purposeTypeCode: string = '';
  responsibilityTypeCode: string = '';
  initiatorTypeCode: string = '';
  leaseTypeCode: string = '';
  statusTypeCode: string = '';
  regionId: number = 0;
  programTypeCode: string = '';
  otherLeaseTypeDescription: string = '';
  otherProgramTypeDescription: string = '';
  otherCategoryTypeDescription: string = '';
  otherPurposeTypeDescription: string = '';
  note: string = '';
  programName: string = '';
  motiName: string = '';
  amount: NumberFieldValue = '';
  renewalCount: NumberFieldValue = '';
  description: string = '';
  isResidential: boolean = false;
  isCommercialBuilding: boolean = false;
  isOtherImprovement: boolean = false;
  returnNotes: string = ''; // security deposit notes (free form text)
  documentationReference: string = '';
  hasPhysicalLicense?: boolean;
  hasDigitalLicense?: boolean;
  tenantNotes: string[] = [];
  properties: FormLeaseProperty[] = [];
  rowVersion: number = 0;

  static fromApi(apiModel?: Api_Lease): LeaseFormModel {
    const leaseDetail = new LeaseFormModel();

    leaseDetail.id = apiModel?.id;
    leaseDetail.lFileNo = apiModel?.lFileNo || '';
    leaseDetail.psFileNo = apiModel?.psFileNo || '';
    leaseDetail.tfaFileNumber = apiModel?.tfaFileNumber || '';
    leaseDetail.expiryDate = apiModel?.expiryDate || '';
    leaseDetail.startDate = apiModel?.startDate || '';
    leaseDetail.responsibilityEffectiveDate = apiModel?.responsibilityEffectiveDate || '';
    leaseDetail.amount = parseFloat(apiModel?.amount?.toString() ?? '') || 0.0;
    leaseDetail.paymentReceivableTypeCode = fromTypeCode(apiModel?.paymentReceivableType) || '';
    leaseDetail.categoryTypeCode = fromTypeCode(apiModel?.categoryType) || '';
    leaseDetail.purposeTypeCode = fromTypeCode(apiModel?.purposeType) || '';
    leaseDetail.responsibilityTypeCode = fromTypeCode(apiModel?.responsibilityType) || '';
    leaseDetail.initiatorTypeCode = fromTypeCode(apiModel?.initiatorType) || LeaseInitiatorTypes.Hq;
    leaseDetail.statusTypeCode = fromTypeCode(apiModel?.statusType) || '';
    leaseDetail.leaseTypeCode = fromTypeCode(apiModel?.type) || '';
    leaseDetail.regionId = fromTypeCode(apiModel?.region) || 0;
    leaseDetail.programTypeCode = fromTypeCode(apiModel?.programType) || '';
    leaseDetail.note = apiModel?.note || '';
    leaseDetail.returnNotes = apiModel?.returnNotes || '';
    leaseDetail.documentationReference = apiModel?.documentationReference || '';
    leaseDetail.motiName = apiModel?.motiName || '';
    leaseDetail.hasDigitalLicense = apiModel?.hasDigitalLicense;
    leaseDetail.hasPhysicalLicense = apiModel?.hasPhysicalLicense;
    leaseDetail.properties = apiModel?.properties?.map(p => FormLeaseProperty.fromApi(p)) ?? [];
    leaseDetail.isResidential = apiModel?.isResidential || false;
    leaseDetail.isCommercialBuilding = apiModel?.isCommercialBuilding || false;
    leaseDetail.isOtherImprovement = apiModel?.isOtherImprovement || false;
    leaseDetail.rowVersion = apiModel?.rowVersion || 0;
    leaseDetail.description = apiModel?.description || '';
    leaseDetail.otherCategoryTypeDescription = apiModel?.otherCategoryType || '';
    leaseDetail.otherProgramTypeDescription = apiModel?.otherProgramType || '';
    leaseDetail.otherPurposeTypeDescription = apiModel?.otherPurposeType || '';
    leaseDetail.otherLeaseTypeDescription = apiModel?.otherType || '';

    return leaseDetail;
  }

  public toApi(): Api_Lease {
    return {
      id: this.id,
      lFileNo: stringToNull(this.lFileNo),
      psFileNo: stringToNull(this.psFileNo),
      tfaFileNumber: stringToNull(this.tfaFileNumber),
      expiryDate: stringToNull(this.expiryDate),
      startDate: this.startDate,
      responsibilityEffectiveDate: stringToNull(this.responsibilityEffectiveDate),
      amount: parseFloat(this.amount?.toString() ?? '') || 0.0,
      paymentReceivableType: toTypeCode(this.paymentReceivableTypeCode),
      categoryType: this.categoryTypeCode ? toTypeCode(this.categoryTypeCode) : undefined,
      purposeType: toTypeCode(this.purposeTypeCode),
      responsibilityType: toTypeCode(this.responsibilityTypeCode),
      initiatorType: toTypeCode(this.initiatorTypeCode),
      statusType: toTypeCode(this.statusTypeCode),
      type: toTypeCode(this.leaseTypeCode),
      region: toTypeCode(this.regionId),
      programType: toTypeCode(this.programTypeCode),
      note: stringToNull(this.note),
      returnNotes: this.returnNotes,
      documentationReference: stringToNull(this.documentationReference),
      motiName: this.motiName,
      hasDigitalLicense: this.hasDigitalLicense,
      hasPhysicalLicense: this.hasPhysicalLicense,
      properties: this.properties?.map(p => p.toApi()) ?? [],
      isResidential: this.isResidential,
      isCommercialBuilding: this.isCommercialBuilding,
      isOtherImprovement: this.isOtherImprovement,
      description: stringToNull(this.description),
      rowVersion: this.rowVersion > 0 ? this.rowVersion : undefined,
      otherCategoryType: stringToNull(this.otherCategoryTypeDescription),
      otherProgramType: stringToNull(this.otherProgramTypeDescription),
      otherPurposeType: stringToNull(this.otherPurposeTypeDescription),
      otherType: stringToNull(this.otherLeaseTypeDescription),
    };
  }
}

export class LeaseDetails {
  fileName?: string;
  assignedDate?: string;
  deliveryDate?: string;
  acquisitionPhysFileStatusTypeDescription?: string;
  acquisitionTypeDescription?: string;
  regionDescription?: string;
  acquisitionTeam: DetailAcquisitionFilePerson[] = [];

  static fromApi(model?: Api_AcquisitionFile): DetailAcquisitionFile {
    const detail = new DetailAcquisitionFile();
    detail.fileName = model?.fileName;
    detail.assignedDate = model?.assignedDate;
    detail.deliveryDate = model?.deliveryDate;
    detail.acquisitionPhysFileStatusTypeDescription =
      model?.acquisitionPhysFileStatusTypeCode?.description;
    detail.acquisitionTypeDescription = model?.acquisitionTypeCode?.description;
    detail.regionDescription = model?.regionCode?.description;
    // acquisition team array
    detail.acquisitionTeam =
      model?.acquisitionTeam?.map(x => DetailAcquisitionFilePerson.fromApi(x)) || [];

    return detail;
  }
}

export class FormLeaseProperty {
  id?: number;
  property?: FormProperty;
  leaseId?: number;
  rowVersion?: number;
  landArea?: number;
  areaUnitType?: Api_TypeCode<string>;

  public constructor(leaseId?: number) {
    this.property = new FormProperty();
    this.leaseId = leaseId;
  }

  static fromApi(apiPropertyLease: Api_PropertyLease): FormLeaseProperty {
    const model = new FormLeaseProperty(apiPropertyLease.lease?.id);
    model.property = FormProperty.fromApi(apiPropertyLease.property ?? {});
    model.id = apiPropertyLease.id;
    model.rowVersion = apiPropertyLease.rowVersion;
    model.landArea = apiPropertyLease.leaseArea;
    model.areaUnitType = apiPropertyLease.areaUnitType;
    return model;
  }

  public toApi(): Api_PropertyLease {
    return {
      id: this.id,
      rowVersion: this.rowVersion,
      property: this.property?.toApi(),
      lease: { id: this.leaseId },
      leaseArea: stringToNull(this.landArea),
      areaUnitType: this.areaUnitType,
    };
  }
}

export class FormProperty {
  id?: number;
  pid?: NumberFieldValue;
  pin?: NumberFieldValue;
  coordinates?: string;
  rowVersion?: number;

  public constructor() {
    this.pid = '';
    this.pin = '';
    this.coordinates = '';
  }

  static fromApi(apiProperty: Api_Property): FormProperty {
    const model = new FormProperty();
    model.pid = apiProperty.pid;
    model.pin = apiProperty.pin;
    model.coordinates = getPrettyLatLng({
      coordinate: { x: apiProperty.longitude, y: apiProperty.latitude },
    });
    model.id = apiProperty.id;
    model.rowVersion = apiProperty.rowVersion;
    return model;
  }

  public toApi(): Api_Property {
    const splitCoordinates = this.coordinates?.split(',');
    return {
      id: this.id,
      pid: pidParser(this.pid),
      pin: parseInt(this.pin?.toString() ?? ''),
      location:
        splitCoordinates !== undefined && splitCoordinates.length === 2
          ? { coordinate: { x: +splitCoordinates[0].trim(), y: +splitCoordinates[1].trim() } }
          : undefined,
    };
  }
}

export const getDefaultFormLease: () => LeaseFormModel = () =>
  LeaseFormModel.fromApi({
    properties: [],
    startDate: '',
    expiryDate: '',
    lFileNo: '',
    tfaFileNumber: '',
    psFileNo: '',
    motiName: '',
    amount: 0,
    isResidential: false,
    isCommercialBuilding: false,
    isOtherImprovement: false,
    returnNotes: '',
    hasDigitalLicense: undefined,
    hasPhysicalLicense: undefined,
    statusType: { id: 'DRAFT' },
    paymentReceivableType: { id: 'RCVBL' },
  });
