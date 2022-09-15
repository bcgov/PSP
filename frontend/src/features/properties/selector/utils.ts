import { IMapProperty } from 'features/properties/selector/models';
import { compact } from 'lodash';
import { Api_ResearchFileProperty } from 'models/api/ResearchFile';
import { pidFormatter } from 'utils';

export enum NameSourceType {
  PID = 'PID',
  PIN = 'PIN',
  PLAN = 'Plan #',
  LOCATION = 'Location',
  RESEARCH = 'Research Name',
  NONE = 'n/a',
}

interface PropertyName {
  label: NameSourceType;
  value: string;
}

export const getPropertyName = (property: IMapProperty): PropertyName => {
  if (property.pid !== undefined && property.pid?.toString().length > 0 && property.pid !== '0') {
    return { label: NameSourceType.PID, value: pidFormatter(property.pid.toString()) };
  } else if (
    property.pin !== undefined &&
    property.pin?.toString()?.length > 0 &&
    property.pin !== '0'
  ) {
    return { label: NameSourceType.PIN, value: property.pin.toString() };
  } else if (property.planNumber !== undefined) {
    return { label: NameSourceType.PLAN, value: property.planNumber };
  } else if (property.latitude !== undefined && property.longitude !== undefined) {
    return {
      label: NameSourceType.LOCATION,
      value: compact([property.latitude?.toFixed(5), property.longitude?.toFixed(5)]).join(', '),
    };
  }
  return { label: NameSourceType.NONE, value: '' };
};

export const getResearchPropertyName = (
  researchProperty?: Api_ResearchFileProperty,
): PropertyName => {
  if (researchProperty === undefined) {
    return { label: NameSourceType.NONE, value: '' };
  }

  if (researchProperty.propertyName !== undefined && researchProperty.propertyName !== '') {
    return { label: NameSourceType.RESEARCH, value: researchProperty.propertyName };
  } else if (researchProperty.property !== undefined) {
    const property = researchProperty.property;
    let mapProperty: IMapProperty = {
      pin: property.pin?.toString(),
      pid: property.pid?.toString(),
      latitude: property.latitude,
      longitude: property.longitude,
      planNumber: property.planNumber,
    };
    return getPropertyName(mapProperty);
  }
  return { label: NameSourceType.NONE, value: '' };
};
