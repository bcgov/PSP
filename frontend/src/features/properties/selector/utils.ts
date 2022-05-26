import { IMapProperty } from 'features/properties/selector/models';
import { compact } from 'lodash';
import { pidFormatter } from 'utils';

export const getPropertyIdentifier = (property: IMapProperty) => {
  if (!!property.pid) {
    return `PID: ${pidFormatter(property.pid)}`;
  } else if (!!property.pin) {
    return `PIN: ${property.pin}`;
  } else if (!!property.planNumber) {
    return `Plan #: ${property.planNumber}`;
  } else {
    return compact([property.latitude?.toFixed(5), property.longitude?.toFixed(5)]).join(', ');
  }
};
