import { act, screen } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { noop } from 'lodash';
import { mockFAParcelLayerResponse, mockGeocoderOptions } from 'mocks';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { fillInput, render, RenderOptions, userEvent } from 'utils/test-utils';

import { PropertyForm } from '../map/shared/models';
import MapSelectorContainer, { IMapSelectorContainerProps } from './MapSelectorContainer';
import { IMapProperty } from './models';
import { featuresToIdentifiedMapProperty } from './search/PropertySelectorSearchContainer';

const mockStore = configureMockStore([thunk]);

const mockAxios = new MockAdapter(axios);

const store = mockStore({});

const onSelectedProperty = jest.fn();
const onRemoveProperty = jest.fn();

const testProperty: IMapProperty = {
  pid: '123-456-789',
  planNumber: 'SPS22411',
  address: 'Test address 123',
  legalDescription: 'Test Legal Description',
  region: 1,
  regionName: 'South Coast',
  district: 5,
  districtName: 'Okanagan-Shuswap',
};

describe('MapSelectorContainer component', () => {
  const setup = (renderOptions: RenderOptions & Partial<IMapSelectorContainerProps>) => {
    // render component under test
    const utils = render(
      <Formik initialValues={{ properties: [] }} onSubmit={noop}>
        <MapSelectorContainer
          onRemoveProperty={onRemoveProperty}
          onSelectedProperty={onSelectedProperty}
          existingProperties={renderOptions.existingProperties ?? []}
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
          'https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_FA_SVW/ows',
        ),
      )
      .reply(200, mockFAParcelLayerResponse)
      .onGet(new RegExp('tools/geocoder/nearest*'))
      .reply(200, mockGeocoderOptions[0]);
  });

  afterEach(() => {
    jest.resetAllMocks();
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

  it('lists selected properties', async () => {
    const { getByText } = setup({
      existingProperties: [PropertyForm.fromMapProperty(testProperty)],
    });
    await act(async () => {
      expect(getByText('PID: 123-456-789')).toBeVisible();
    });
  });

  it('displays all selected property attributes', async () => {
    const { getByText } = setup({
      existingProperties: [PropertyForm.fromMapProperty(testProperty)],
    });
    await act(async () => {
      expect(getByText(/SPS22411/g)).toBeVisible();
      expect(getByText(/Test address 123/g)).toBeVisible();
      expect(getByText(/1 - South Coast/g)).toBeVisible();
      expect(getByText(/5 - Okanagan-Shuswap/g)).toBeVisible();
      expect(getByText(/Test Legal Description/g)).toBeVisible();
    });
  });

  it('selected properties can be removed', async () => {
    const { getByText } = setup({
      existingProperties: [PropertyForm.fromMapProperty(testProperty)],
    });
    const removeButton = getByText('Remove');

    await act(async () => userEvent.click(removeButton));
    expect(onRemoveProperty).toHaveBeenCalled();
  });

  it('selected properties display a warning if added', async () => {
    const { getByText, getByTitle, findByTestId, container } = setup({
      existingProperties: [],
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

    expect(onSelectedProperty).toHaveBeenCalledWith({
      address: '1234 Fake St',
      district: 12,
      districtName: 'Cannot determine',
      id: 'PID-009-727-493',
      latitude: 48.7662,
      legalDescription:
        'THAT PART OF SECTION 13, RANGE 1, SOUTH SALT SPRING ISLAND, COWICHAN DISTRICT',
      longitude: -123.4617,
      pid: '9727493',
      pin: '',
      planNumber: 'NO_PLAN',
      region: 4,
      regionName: 'Cannot determine',
    });
  });

  it('selected properties display a warning if added multiple times', async () => {
    const { getByText, getByTitle, findByTestId, container } = setup({
      existingProperties: featuresToIdentifiedMapProperty(mockFAParcelLayerResponse as any)?.map(
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
