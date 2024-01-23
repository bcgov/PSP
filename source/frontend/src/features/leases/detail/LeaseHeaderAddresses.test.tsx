import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { IProperty } from '@/interfaces';
import { mockLookups } from '@/mocks/lookups.mock';
import { getEmptyPropertyLease, getMockProperties } from '@/mocks/properties.mock';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import LeaseHeaderAddresses, { ILeaseHeaderAddressesProps } from './LeaseHeaderAddresses';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('LeaseHeaderAddresses component', () => {
  const mockAxios = new MockAdapter(axios);
  const setup = (renderOptions?: RenderOptions & ILeaseHeaderAddressesProps) => {
    // render component under test
    const component = render(
      <LeaseHeaderAddresses propertyLeases={renderOptions?.propertyLeases} />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    mockAxios.reset();
  });

  it('renders 2 addresses by default', async () => {
    const { getAllByText, getByText } = setup({
      propertyLeases: getMockProperties().map(p => ({
        ...getEmptyPropertyLease(),
        fileId: 1,
        property: p as any,
      })),
    });

    const text = getAllByText('1234 Mock street, Victoria', { exact: false });
    expect(text).toHaveLength(2);
    expect(getByText('[+1 more...]')).toBeVisible();
  });
  it('formats addresses as expected', async () => {
    const { getByText, getAllByText } = setup({
      propertyLeases: [
        {
          fileId: 1,
          property: noStreetOrMunicipality as unknown as ApiGen_Concepts_Property,
          file: null,
          leaseArea: null,
          areaUnitType: null,
          displayOrder: null,
          id: 0,
          propertyId: 0,
          propertyName: null,
          rowVersion: null,
        },
        {
          fileId: 1,
          property: streetNoMunicipality as unknown as ApiGen_Concepts_Property,
          file: null,
          leaseArea: null,
          areaUnitType: null,
          displayOrder: null,
          id: 0,
          propertyId: 0,
          propertyName: null,
          rowVersion: null,
        },
        {
          fileId: 1,
          property: noStreetButMunicipality as unknown as ApiGen_Concepts_Property,
          file: null,
          leaseArea: null,
          areaUnitType: null,
          displayOrder: null,
          id: 0,
          propertyId: 0,
          propertyName: null,
          rowVersion: null,
        },
        {
          fileId: 1,
          property: streetAndMunicipality as unknown as ApiGen_Concepts_Property,
          file: null,
          leaseArea: null,
          areaUnitType: null,
          displayOrder: null,
          id: 0,
          propertyId: 0,
          propertyName: null,
          rowVersion: null,
        },
        {
          fileId: 1,
          property: undefinedAddress as unknown as ApiGen_Concepts_Property,
          file: null,
          leaseArea: null,
          areaUnitType: null,
          displayOrder: null,
          id: 0,
          propertyId: 0,
          propertyName: null,
          rowVersion: null,
        },
      ],
    });

    const moreButton = getByText('[+3 more...]');
    await act(async () => userEvent.click(moreButton));

    expect(
      getAllByText('000-000-000 - Address not available in PIMS', { exact: false }),
    ).toHaveLength(2);
    expect(getByText('1234 fake st', { exact: false })).toBeVisible();
    expect(getByText('Victoria', { exact: false })).toBeVisible();
    expect(getByText('4321 real st, Vancouver', { exact: false })).toBeVisible();
  });
});

const undefinedAddress: Partial<IProperty> = {
  id: 1,
  pid: '000-000-000',
  address: undefined as any,
};

const noStreetOrMunicipality: Partial<IProperty> = {
  id: 1,
  pid: '000-000-000',
  address: { streetAddress1: '', municipality: '' } as any,
};

const streetNoMunicipality: Partial<IProperty> = {
  id: 2,
  address: { streetAddress1: '1234 fake st', municipality: '' } as any,
};

const noStreetButMunicipality: Partial<IProperty> = {
  id: 3,
  address: { streetAddress1: '', municipality: 'Victoria' } as any,
};

const streetAndMunicipality: Partial<IProperty> = {
  id: 4,
  address: { streetAddress1: '4321 real st', municipality: 'Vancouver' } as any,
};
