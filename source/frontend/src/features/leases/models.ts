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
import { stringToNull } from 'utils/formUtils';
import { pidParser } from 'utils/propertyUtils';

import { getPrettyLatLng } from './../properties/selector/utils';

export class FormLease {
  id?: number;
  lFileNo?: string;
  psFileNo?: string;
  tfaFileNumber?: string;
  expiryDate?: string;
  renewalDate?: string;
  startDate?: string;
  responsibilityEffectiveDate?: string;
  paymentReceivableType?: Api_TypeCode<string>;
  categoryType?: Api_TypeCode<string>;
  purposeType?: Api_TypeCode<string>;
  responsibilityType?: Api_TypeCode<string>;
  initiatorType?: Api_TypeCode<string>;
  type?: Api_TypeCode<string>;
  statusType?: Api_TypeCode<string>;
  region?: Api_TypeCode<number>;
  programType?: Api_TypeCode<string>;
  otherType?: string;
  otherProgramType?: string;
  otherCategoryType?: string;
  otherPurposeType?: string;
  note?: string;
  programName?: string;
  motiName?: string;
  amount?: NumberFieldValue;
  renewalCount?: NumberFieldValue;
  description?: string;
  isResidential?: boolean;
  isCommercialBuilding?: boolean;
  isOtherImprovement?: boolean;
  returnNotes?: string; // security deposit notes (free form text)
  documentationReference?: string;
  hasPhysicalLicense?: boolean;
  hasDigitalLicense?: boolean;
  tenantNotes?: string[] = [];
  properties?: FormLeaseProperty[] = [];
  rowVersion?: number;

  static fromApi(apiModel?: Api_Lease): FormLease {
    const leaseDetail = new FormLease();

    leaseDetail.id = apiModel?.id;
    leaseDetail.lFileNo = apiModel?.lFileNo;
    leaseDetail.psFileNo = apiModel?.psFileNo;
    leaseDetail.tfaFileNumber = apiModel?.tfaFileNumber;
    leaseDetail.expiryDate = apiModel?.expiryDate;
    leaseDetail.startDate = apiModel?.startDate;
    leaseDetail.responsibilityEffectiveDate = apiModel?.responsibilityEffectiveDate;
    leaseDetail.amount = parseFloat(apiModel?.amount?.toString() ?? '') || 0.0;
    leaseDetail.paymentReceivableType = apiModel?.paymentReceivableType;
    leaseDetail.categoryType = apiModel?.categoryType;
    leaseDetail.purposeType = apiModel?.purposeType;
    leaseDetail.responsibilityType = apiModel?.responsibilityType;
    leaseDetail.initiatorType = apiModel?.initiatorType || { id: LeaseInitiatorTypes.Hq };
    leaseDetail.statusType = apiModel?.statusType;
    leaseDetail.type = apiModel?.type;
    leaseDetail.region = apiModel?.region;
    leaseDetail.programType = apiModel?.programType;
    leaseDetail.note = apiModel?.note;
    leaseDetail.returnNotes = apiModel?.returnNotes;
    leaseDetail.documentationReference = apiModel?.documentationReference;
    leaseDetail.motiName = apiModel?.motiName;
    leaseDetail.hasDigitalLicense = apiModel?.hasDigitalLicense;
    leaseDetail.hasPhysicalLicense = apiModel?.hasPhysicalLicense;
    leaseDetail.properties = apiModel?.properties?.map(p => FormLeaseProperty.fromApi(p)) ?? [];
    leaseDetail.isResidential = apiModel?.isResidential;
    leaseDetail.isCommercialBuilding = apiModel?.isCommercialBuilding;
    leaseDetail.isOtherImprovement = apiModel?.isOtherImprovement;
    leaseDetail.rowVersion = apiModel?.rowVersion;
    leaseDetail.description = apiModel?.description;

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
      paymentReceivableType: this.paymentReceivableType,
      categoryType: this.categoryType ? this.categoryType : undefined,
      purposeType: this.purposeType,
      responsibilityType: this.responsibilityType,
      initiatorType: this.initiatorType || { id: LeaseInitiatorTypes.Hq },
      statusType: this.statusType,
      type: this.type,
      region: this.region,
      programType: this.programType,
      note: this.note,
      returnNotes: this.returnNotes,
      documentationReference: this.documentationReference,
      motiName: this.motiName,
      hasDigitalLicense: this.hasDigitalLicense,
      hasPhysicalLicense: this.hasPhysicalLicense,
      properties: this.properties?.map(p => p.toApi()) ?? [],
      isResidential: this.isResidential,
      isCommercialBuilding: this.isCommercialBuilding,
      isOtherImprovement: this.isOtherImprovement,
      description: this.description,
      rowVersion: this.rowVersion,
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

export const defaultFormLease: FormLease = FormLease.fromApi({
  properties: [{}],
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
});
