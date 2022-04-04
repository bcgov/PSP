import { IProperty } from 'interfaces';
import { act } from 'react-dom/test-utils';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { propertiesSlice } from 'store/slices/properties';
import { render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import PropertySelectorFormView, {
  IPropertySelectorFormViewProps,
} from './PropertySelectorFormView';

const onClickAway = jest.fn();
const onClickDraftMarker = jest.fn();

const mockStore = configureMockStore([thunk]);

const store = mockStore({
  [propertiesSlice.name]: {},
});

describe('PropertySelectorFormView component', () => {
  const setup = (renderOptions: RenderOptions & IPropertySelectorFormViewProps) => {
    // render component under test
    const component = render(
      <PropertySelectorFormView
        properties={renderOptions.properties}
        onClickAway={renderOptions.onClickAway}
        onClickDraftMarker={renderOptions.onClickDraftMarker}
      />,
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
    const { component } = setup({ properties: [], onClickAway, onClickDraftMarker });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders as expected when provided a list of properties', async () => {
    const {
      component: { getByText },
      store,
    } = await setup({
      properties: [{ pid: '123-456-789' } as IProperty, { pin: '1111222' } as any],
      onClickAway,
      onClickDraftMarker,
    });

    await waitFor(async () => {
      expect(store.getActions()).toContainEqual({
        payload: [],
        type: 'properties/storeDraftProperties',
      });
    });
    expect(getByText('PID: 123-456-789')).toBeVisible();
    expect(getByText('PIN: 1111222')).toBeVisible();
  });

  it('properties can be removed', async () => {
    const {
      component: { getAllByTitle, queryByText },
    } = await setup({
      properties: [{ pid: '123-456-789' } as IProperty, { pin: '1111222' } as any],
      onClickAway,
      onClickDraftMarker,
    });
    const pidRow = getAllByTitle('remove')[0];

    await act(async () => {
      userEvent.click(pidRow);
      await waitFor(async () => {
        expect(store.getActions()).toContainEqual({
          payload: [],
          type: 'properties/storeDraftProperties',
        });
      });
    });

    expect(queryByText('PID: 123-456-789')).toBeNull();
  });

  it('properties with lat/lng are synchronized', async () => {
    await setup({
      properties: [
        { pid: '123-456-789', latitude: 1, longitude: 2 } as IProperty,
        { pin: '1111222' } as any,
      ],
      onClickAway,
      onClickDraftMarker,
    });

    await act(async () => {
      await waitFor(async () => {
        expect(store.getActions()).toContainEqual({
          payload: [
            {
              geometry: { coordinates: [2, 1], type: 'Point' },
              properties: { id: 0, name: 'New Parcel' },
              type: 'Feature',
            },
          ],
          type: 'properties/storeDraftProperties',
        });
      });
    });
  });

  it('multiple properties with lat/lng are synchronized', async () => {
    await setup({
      properties: [
        { pid: '123-456-789', latitude: 1, longitude: 2 } as IProperty,
        { pin: '1111222', latitude: 3, longitude: 4 } as any,
      ],
      onClickAway,
      onClickDraftMarker,
    });

    await act(async () => {
      await waitFor(async () => {
        expect(store.getActions()).toContainEqual({
          payload: [
            {
              geometry: { coordinates: [2, 1], type: 'Point' },
              properties: { id: 0, name: 'New Parcel' },
              type: 'Feature',
            },
            {
              geometry: { coordinates: [4, 3], type: 'Point' },
              properties: { id: 0, name: 'New Parcel' },
              type: 'Feature',
            },
          ],
          type: 'properties/storeDraftProperties',
        });
      });
    });
  });

  it('properties are prefixed by svg with incrementing id', async () => {
    const {
      component: { getByTitle },
    } = await setup({
      properties: [{ pid: '123-456-789' } as IProperty, { pin: '1111222' } as any],
      onClickAway,
      onClickDraftMarker,
    });

    await waitFor(async () => {
      expect(store.getActions()).toContainEqual({
        payload: [],
        type: 'properties/storeDraftProperties',
      });
    });
    expect(getByTitle('1')).toBeInTheDocument();
    expect(getByTitle('2')).toBeInTheDocument();
  });

  it('clicking the draft marker fires the expected event', async () => {
    const {
      component: { getByTitle },
    } = await setup({
      properties: [{ pid: '123-456-789' } as IProperty, { pin: '1111222' } as any],
      onClickAway,
      onClickDraftMarker,
    });

    await waitFor(async () => {
      expect(store.getActions()).toContainEqual({
        payload: [],
        type: 'properties/storeDraftProperties',
      });
    });
    const draftMarker = getByTitle('select properties on the map');
    userEvent.click(draftMarker);
    expect(onClickDraftMarker).toHaveBeenCalled();
  });

  it('clicking off the draft marker fires the expected event', async () => {
    const {
      component: { getByTitle },
    } = await setup({
      properties: [{ pid: '123-456-789' } as IProperty, { pin: '1111222' } as any],
      onClickAway,
      onClickDraftMarker,
    });

    await waitFor(async () => {
      expect(store.getActions()).toContainEqual({
        payload: [],
        type: 'properties/storeDraftProperties',
      });
    });
    const draftMarker = getByTitle('select properties on the map');
    userEvent.click(draftMarker);
    expect(onClickDraftMarker).toHaveBeenCalled();
    const draftIcon = getByTitle('1');
    userEvent.click(draftIcon);
    expect(onClickAway).toHaveBeenCalled();
  });
});
