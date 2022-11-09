import { IMapProperty } from 'features/properties/selector/models';
import { compact } from 'lodash';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import { formatApiAddress, pidFormatter } from 'utils';

import { Api_Geometry, Api_Property } from './../../../models/api/Property';

export enum NameSourceType {
  PID = 'PID',
  PIN = 'PIN',
  PLAN = 'Plan #',
  LOCATION = 'Location',
  NAME = 'Descriptive Name',
  NONE = 'n/a',
  ADDRESS = 'Address',
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
      value: compact([property.latitude?.toFixed(6), property.longitude?.toFixed(6)]).join(', '),
    };
  } else if (property.address !== undefined) {
    return {
      label: NameSourceType.ADDRESS,
      value: property.address,
    };
  }
  return { label: NameSourceType.NONE, value: '' };
};

export const getPrettyLatLng = (location?: Api_Geometry) =>
  compact([
    location?.coordinate?.x?.toFixed(6) ?? 0,
    location?.coordinate?.y?.toFixed(6) ?? 0,
  ]).join(', ');

export const getFilePropertyName = (
  fileProperty?: Api_PropertyFile,
  skipName: boolean = false,
): PropertyName => {
  if (fileProperty === undefined) {
    return { label: NameSourceType.NONE, value: '' };
  }

  if (
    fileProperty.propertyName !== undefined &&
    fileProperty.propertyName !== '' &&
    skipName === false
  ) {
    return { label: NameSourceType.NAME, value: fileProperty.propertyName };
  } else if (fileProperty.property !== undefined) {
    const property = fileProperty.property;
    return getApiPropertyName(property);
  }
  return { label: NameSourceType.NONE, value: '' };
};

export const getApiPropertyName = (property?: Api_Property): PropertyName => {
  if (property === undefined) {
    return { label: NameSourceType.NONE, value: '' };
  }

  let mapProperty: IMapProperty = {
    pin: property.pin?.toString(),
    pid: property.pid?.toString(),
    latitude: property.latitude,
    longitude: property.longitude,
    planNumber: property.planNumber,
    address: property.address !== undefined ? formatApiAddress(property.address) : undefined,
  };
  return getPropertyName(mapProperty);
};
