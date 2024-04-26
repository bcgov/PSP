import { act, screen } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import noop from 'lodash/noop';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { mockFAParcelLayerResponse, mockGeocoderOptions } from '@/mocks/index.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { featuresToIdentifiedMapProperty } from '@/utils/mapPropertyUtils';
import { fillInput, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { PropertyForm } from '../../features/mapSideBar/shared/models';
import { useMapStateMachine } from '../common/mapFSM/MapStateMachineContext';
import MapSelectorContainer, { IMapSelectorContainerProps } from './MapSelectorContainer';
import { IMapProperty } from './models';

const mockStore = configureMockStore([thunk]);

const mockAxios = new MockAdapter(axios);

const store = mockStore({});

const onSelectedProperties = vi.fn();

const testProperty: IMapProperty = {
  propertyId: 123,
  pid: '123-456-789',
  planNumber: 'SPS22411',
  address: 'Test address 123',
  region: 1,
  regionName: 'South Coast',
  district: 5,
  districtName: 'Okanagan-Shuswap',
};

vi.mock('@/components/common/mapFSM/MapStateMachineContext');

describe('MapSelectorContainer component', () => {
  const setup = (renderOptions: RenderOptions & Partial<IMapSelectorContainerProps>) => {
    // render component under test
    const utils = render(
      <Formik initialValues={{ properties: [] }} onSubmit={noop}>
        <MapSelectorContainer
          addSelectedProperties={onSelectedProperties}
          modifiedProperties={renderOptions.modifiedProperties ?? []}
        />
      </Formik>,
      {
        ...renderOptions,
        store: store,
      },
    );

    return {
      store,
      ...utils,
    };
  };

  beforeEach(() => {
    mockAxios
      .onGet(
        new RegExp(
          'https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW/ows',
        ),
      )
      .reply(200, mockFAParcelLayerResponse)
      .onGet(new RegExp('tools/geocoder/nearest*'))
      .reply(200, mockGeocoderOptions[0]);
  });

  afterEach(() => {
    vi.resetAllMocks();
  });

  it('renders as expected when provided no properties', async () => {
    const { asFragment } = setup({});
    await act(async () => {
      expect(asFragment()).toMatchSnapshot();
    });
  });

  it('displays two tabs', async () => {
    const { getByText } = setup({});
    await act(async () => {
      expect(getByText('Locate on Map')).toBeVisible();
    });
  });

  it('displays locate on Map by default', async () => {
    const { getByText } = setup({});
    await act(async () => {
      expect(getByText('Locate on Map')).toHaveClass('active');
    });
  });

  it('allows the search tab to be selected', async () => {
    const { getByText } = setup({});
    const searchTab = getByText('Search');
    await act(async () => userEvent.click(searchTab));
    expect(searchTab).toHaveClass('active');
  });

  it('displays all selected property attributes', async () => {
    const { getByText } = setup({
      modifiedProperties: [PropertyForm.fromMapProperty(testProperty)],
    });
    await act(async () => {
      expect(getByText(/SPS22411/i)).toBeVisible();
      expect(getByText(/Test address 123/i)).toBeVisible();
      expect(getByText(/1 - South Coast/i)).toBeVisible();
      expect(getByText(/5 - Okanagan-Shuswap/i)).toBeVisible();
    });
  });

  it('selected properties display a warning if added', async () => {
    const { getByText, getByTitle, findByTestId, container } = setup({
      modifiedProperties: [],
    });

    const searchTab = getByText('Search');
    await act(async () => userEvent.click(searchTab));
    await act(async () => {
      fillInput(container, 'searchBy', 'pid', 'select');
    });
    await fillInput(container, 'pid', '123-456-789');
    const searchButton = getByTitle('search');
    await act(async () => userEvent.click(searchButton));
    const checkbox = await findByTestId('selectrow-PID-009-727-493');
    await act(async () => userEvent.click(checkbox));
    const addButton = getByText('Add to selection');
    await act(async () => userEvent.click(addButton));

    expect(onSelectedProperties).toHaveBeenCalledWith([
      {
        address: '1234 Fake St',
        areaUnit: 'M2',
        district: 12,
        districtName: 'Cannot determine',
        id: 'PID-009-727-493',
        landArea: 29217,
        latitude: 48.76613749999999,
        longitude: -123.46163749999998,
        name: undefined,
        pid: '9727493',
        pin: undefined,
        planNumber: 'NO_PLAN',
        propertyId: undefined,
        region: 4,
        regionName: 'Cannot determine',
      },
    ]);
  });

  it('selected properties display a warning if added multiple times', async () => {
    const { getByText, getByTitle, findByTestId, container } = setup({
      modifiedProperties: featuresToIdentifiedMapProperty(mockFAParcelLayerResponse as any)?.map(
        p => PropertyForm.fromMapProperty(p),
      ),
    });

    const searchTab = getByText('Search');
    await act(async () => userEvent.click(searchTab));
    await act(async () => {
      fillInput(container, 'searchBy', 'pid', 'select');
    });
    await fillInput(container, 'pid', '123-456-789');
    const searchButton = getByTitle('search');

    await act(async () => userEvent.click(searchButton));

    const checkbox = await findByTestId('selectrow-PID-009-727-493');
    await act(async () => userEvent.click(checkbox));
    const addButton = getByText('Add to selection');
    await act(async () => userEvent.click(addButton));

    const toast = await screen.findAllByText(
      'A property that the user is trying to select has already been added to the selected properties list',
    );
    expect(toast[0]).toBeVisible();
  });
});
