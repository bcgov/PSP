import { screen } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { noop } from 'lodash';
import {
  mockDistrictLayerResponse,
  mockMotiRegionLayerResponse,
  mockPropertyLayerSearchResponse,
} from 'mocks';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { fillInput, render, RenderOptions, userEvent } from 'utils/test-utils';

import { PropertyForm } from '../map/research/add/models';
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
  planNumber: '123546',
  address: 'Test address 123',
  legalDescription: 'Test Legal Description',
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
          'https://openmaps.gov.bc.ca/geo/pub/WHSE_CADASTRE.PMBC_PARCEL_FABRIC_POLY_SVW/wfs',
        ),
      )
      .reply(200, mockPropertyLayerSearchResponse)
      .onGet(
        new RegExp(
          'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.TADM_MOT_REGIONAL_BNDRY_POLY/wfs',
        ),
      )
      .reply(200, mockMotiRegionLayerResponse)
      .onGet(
        new RegExp(
          'https://openmaps.gov.bc.ca/geo/pub/WHSE_ADMIN_BOUNDARIES.TADM_MOT_DISTRICT_BNDRY_POLY/wfs',
        ),
      )
      .reply(200, mockDistrictLayerResponse);
  });

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('renders as expected when provided no properties', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });
  it('displays two tabs', () => {
    const { getByText } = setup({});
    expect(getByText('Locate on Map')).toBeVisible();
  });
  it('displays locate on Map by default', () => {
    const { getByText } = setup({});
    expect(getByText('Locate on Map')).toHaveClass('active');
  });

  it('allows the search tab to be selected', () => {
    const { getByText } = setup({});
    const searchTab = getByText('Search');
    userEvent.click(searchTab);
    expect(searchTab).toHaveClass('active');
  });

  it('lists selected properties', () => {
    const { getByText } = setup({
      existingProperties: [PropertyForm.fromMapProperty(testProperty)],
    });
    expect(getByText('PID: 123-456-789')).toBeVisible();
  });

  it('selected properties can be removed', () => {
    const { getByText } = setup({
      existingProperties: [PropertyForm.fromMapProperty(testProperty)],
    });
    const removeButton = getByText('Remove');
    userEvent.click(removeButton);
    expect(onRemoveProperty).toHaveBeenCalled();
  });

  it('selected properties display a warning if added', async () => {
    const { getByText, getByTitle, findByTestId, container } = setup({
      existingProperties: [],
    });

    const searchTab = getByText('Search');
    userEvent.click(searchTab);
    await fillInput(container, 'searchBy', 'pid', 'select');
    await fillInput(container, 'pid', '123-456-789');
    const searchButton = getByTitle('search');
    userEvent.click(searchButton);
    const checkbox = await findByTestId('selectrow-PID: 017-723-311');
    userEvent.click(checkbox);
    const addButton = getByText('Add to selection');
    userEvent.click(addButton);

    expect(onSelectedProperty).toHaveBeenCalledWith({
      district: 2,
      districtName: 'Vancouver Island',
      id: 'PID: 017-723-311',
      latitude: 49.104223239999996,
      longitude: -122.6414763,
      pid: '017723311',
      pin: '',
      planNumber: 'LMS312',
      region: 1,
      regionName: 'South Coast',
    });
  });

  it('selected properties display a warning if added multiple times', async () => {
    const { getByText, getByTitle, findByTestId, container } = setup({
      existingProperties: featuresToIdentifiedMapProperty(
        mockPropertyLayerSearchResponse as any,
      )?.map(p => PropertyForm.fromMapProperty(p)),
    });

    const searchTab = getByText('Search');
    userEvent.click(searchTab);
    await fillInput(container, 'searchBy', 'pid', 'select');
    await fillInput(container, 'pid', '123-456-789');
    const searchButton = getByTitle('search');
    userEvent.click(searchButton);
    const checkbox = await findByTestId('selectrow-PID: 017-723-311');
    userEvent.click(checkbox);
    const addButton = getByText('Add to selection');
    userEvent.click(addButton);

    const toast = await screen.findByText(
      'A property that the user is trying to select has already been added to the selected properties list',
    );
    expect(toast).toBeVisible();
  });
});
