import 'leaflet';
import 'leaflet/dist/leaflet.css';

import { useKeycloak } from '@react-keycloak/web';
import { render } from '@testing-library/react';
import * as API from 'constants/API';
import {
  AddressTypes,
  PropertyAreaUnitTypes,
  PropertyClassificationTypes,
  PropertyDataSourceTypes,
  PropertyStatusTypes,
  PropertyTenureTypes,
  PropertyTypes,
} from 'constants/index';
import { createMemoryHistory } from 'history';
import { IProperty } from 'interfaces';
import * as React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { lookupCodesSlice } from 'store/slices/lookupCodes';

import InfoContent from './InfoContent';

jest.mock('@react-keycloak/web');

const mockParcel = {
  id: 1,
  pid: '000-000-000',
  propertyTypeId: PropertyTypes.Land,
  statusId: PropertyStatusTypes.FeeSimple,
  dataSourceId: PropertyDataSourceTypes.PAIMS,
  dataSourceEffectiveDate: new Date(),
  classificationId: PropertyClassificationTypes.SurplusActive,
  classification: 'Core Operational',
  tenureId: PropertyTenureTypes.TitledLandMOTI,
  zoning: '',
  zoningPotential: '',
  encumbranceReason: '',
  isSensitive: false,
  latitude: 48,
  longitude: 123,
  name: 'test name',
  description: 'test',
  evaluations: [
    {
      evaluatedOn: '2021-05-12T18:57:19.992Z',
      key: 1,
      value: 10000,
    },
  ],
  addressId: 1,
  address: {
    id: 1,
    addressTypeId: AddressTypes.Physical,
    streetAddress1: '1234 mock Street',
    municipality: 'Victoria',
    provinceId: 1,
    province: 'BC',
    postal: 'V1V1V1',
    region: 'a region',
    district: 'a district',
  },
  regionId: 1,
  region: 'a region',
  districtId: 1,
  district: 'district',
  areaUnitId: PropertyAreaUnitTypes.Hectare,
  areaUnit: 'hectares',
  landArea: 123,
  landLegalDescription: 'test description',
  organizations: [
    {
      id: 1,
      name: 'Ministry of Transportation',
    },
  ],
} as IProperty;

const lCodes = {
  lookupCodes: [
    {
      code: 'AEST',
      id: 1,
      isDisabled: false,
      name: 'Ministry of Advanced Education',
      type: API.ORGANIZATION_CODE_SET_NAME,
    },
    {
      code: 'KPU',
      id: 181,
      isDisabled: false,
      name: 'Kwantlen Polytechnic University',
      type: API.ORGANIZATION_CODE_SET_NAME,
    },
  ],
};

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();

const store = mockStore({
  [lookupCodesSlice.name]: lCodes,
});

const ContentComponent = (
  propertyInfo: IProperty | null,
  propertyTypeId: PropertyTypes | null,
  canViewDetails: boolean,
) => {
  return (
    <Provider store={store}>
      <Router history={history}>
        <InfoContent
          propertyInfo={propertyInfo}
          propertyTypeId={propertyTypeId}
          canViewDetails={canViewDetails}
        />
      </Router>
    </Provider>
  );
};

describe('InfoContent View functionality', () => {
  beforeAll(() => {
    jest.clearAllMocks();
  });
  beforeEach(() => {
    (useKeycloak as jest.Mock).mockReturnValue({
      keycloak: {
        userInfo: {
          organizations: [1],
          roles: [],
        },
        subject: 'test',
      },
    });
  });
  it('InfoContent renders correctly', () => {
    const { container } = render(ContentComponent(mockParcel, PropertyTypes.Land, true));
    expect(container.firstChild).toMatchSnapshot();
  });

  it('Shows all parcel information when can view', () => {
    const { getByText } = render(ContentComponent(mockParcel, PropertyTypes.Land, true));
    expect(getByText('Property Identification')).toBeVisible();
    //Identification information
    expect(getByText('000-000-000')).toBeVisible();
    expect(getByText('test name')).toBeVisible();
    expect(getByText('Ministry of Transportation')).toBeVisible();
    //Location data
    expect(getByText(/1234 mock Street +BC, V1V1V1/i)).toBeVisible();
    expect(getByText('Victoria')).toBeVisible();
    expect(getByText('a district')).toBeVisible();
    expect(getByText('a region')).toBeVisible();
    //Legal Description
    expect(getByText('test description')).toBeVisible();
  });

  it('Lot size formats correctly', () => {
    const { getByText } = render(ContentComponent(mockParcel, PropertyTypes.Land, true));
    expect(getByText('123 hectares')).toBeVisible();
  });

  it('Does not display lease section when there are no leases', () => {
    const { queryByText } = render(ContentComponent(mockParcel, PropertyTypes.Land, true));
    expect(queryByText('Active Leases')).toBeNull();
  });

  it('Displays lease information when there are leases', () => {
    const { getByText } = render(
      ContentComponent(
        { ...mockParcel, leases: [{ tenantName: 'tenantName1' }] },
        PropertyTypes.Land,
        true,
      ),
    );
    expect(getByText('Active Leases')).toBeVisible();
    expect(getByText('tenantName1')).toBeVisible();
  });

  it('Displays all lease information when there are multiple leases', () => {
    const { getByText } = render(
      ContentComponent(
        { ...mockParcel, leases: [{ tenantName: 'tenantName1' }, { tenantName: 'tenantName2' }] },
        PropertyTypes.Land,
        true,
      ),
    );
    expect(getByText('tenantName1')).toBeVisible();
    expect(getByText('tenantName2')).toBeVisible();
  });
});
