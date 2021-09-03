import { IPaginateProperties } from 'constants/API';
import { ENVIRONMENT } from 'constants/index';
import queryString from 'query-string';

import { IPropertyFilter } from '../filter/IPropertyFilter';

export const getPropertyReportUrl = (filter: IPaginateProperties) =>
  `${ENVIRONMENT.apiUrl}/reports/properties?${
    filter ? queryString.stringify({ ...filter, all: true }) : ''
  }`;

export const getAllFieldsPropertyReportUrl = (filter: IPaginateProperties) =>
  `${ENVIRONMENT.apiUrl}/reports/properties/all/fields?${
    filter ? queryString.stringify({ ...filter, all: true }) : ''
  }`;

export const defaultFilterValues: IPropertyFilter = {
  searchBy: 'address',
  pid: '',
  pin: '',
  address: '',
  location: '',
};
