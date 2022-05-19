import { screen } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { noop } from 'lodash';
import { mockPropertyLayerSearchResponse } from 'mocks/filterDataMock';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { fillInput, render, RenderOptions, userEvent } from 'utils/test-utils';

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
    const component = render(
      <Formik initialValues={{ properties: [] }} onSubmit={noop}>
        <MapSelectorContainer
          onRemoveProperty={onRemoveProperty}
          onSelectedProperty={onSelectedProperty}
          selectedProperties={renderOptions.selectedProperties ?? []}
        />
      </Formik>,
      {
        ...renderOptions,
        store: store,
      },
    );

    return {
      store,
      component,
    };
  };

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('renders as expected when provided no properties', () => {
    const { component } = setup({});
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('displays two tabs', () => {
    const {
      component: { getByText },
    } = setup({});
    expect(getByText('Locate on Map')).toBeVisible();
  });
  it('displays locate on Map by default', () => {
    const {
      component: { getByText },
    } = setup({});
    expect(getByText('Locate on Map')).toHaveClass('active');
  });

  it('allows the search tab to be selected', () => {
    const {
      component: { getByText },
    } = setup({});
    const searchTab = getByText('Search');
    userEvent.click(searchTab);
    expect(searchTab).toHaveClass('active');
  });

  it('lists selected properties', () => {
    const {
      component: { getByText },
    } = setup({ selectedProperties: [testProperty] });
    expect(getByText('PID: 123-456-789')).toBeVisible();
  });

  it('selected properties can be removed', () => {
    const {
      component: { getByText },
    } = setup({ selectedProperties: [testProperty] });
    const removeButton = getByText('Remove');
    userEvent.click(removeButton);
    expect(onRemoveProperty).toHaveBeenCalled();
  });

  it('selected properties display a warning if added multiple times', async () => {
    mockAxios.onGet().reply(200, mockPropertyLayerSearchResponse);
    const {
      component: { getByText, getByTitle, findByTestId, container },
    } = setup({
      selectedProperties: featuresToIdentifiedMapProperty(mockPropertyLayerSearchResponse as any),
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
