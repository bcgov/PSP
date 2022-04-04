import { PropertyPopUpContext } from 'components/maps/providers/PropertyPopUpProvider';
import { IProperty } from 'interfaces';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { propertiesSlice } from 'store/slices/properties';
import { render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import { IMapSelectorContainerProps, MapSelectorContainer } from './MapSelectorContainer';

const mockStore = configureMockStore([thunk]);

const store = mockStore({
  [propertiesSlice.name]: {},
});

const setCursor = jest.fn();

describe('PropertySelectorFormView component', () => {
  const setup = (renderOptions: RenderOptions & IMapSelectorContainerProps) => {
    // render component under test
    const component = render(
      <PropertyPopUpContext.Provider value={{ setCursor: setCursor } as any}>
        <MapSelectorContainer properties={renderOptions.properties} formikRef={{} as any} />
      </PropertyPopUpContext.Provider>,
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
    const { component } = setup({ properties: [], formikRef: {} as any });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('changes the cursor when clicking on the draft marker', async () => {
    const {
      component: { getByTitle },
    } = await setup({
      properties: [{ pid: '123-456-789' } as IProperty, { pin: '1111222' } as any],
      formikRef: {} as any,
    });

    await waitFor(async () => {
      expect(store.getActions()).toContainEqual({
        payload: [],
        type: 'properties/storeDraftProperties',
      });
    });
    const draftMarker = getByTitle('select properties on the map');
    userEvent.click(draftMarker);
    expect(setCursor).toHaveBeenCalled();
  });

  it('changes the cursor when clicking away', async () => {
    const {
      component: { getByTitle },
    } = await setup({
      properties: [{ pid: '123-456-789' } as IProperty, { pin: '1111222' } as any],
      formikRef: {} as any,
    });

    await waitFor(async () => {
      expect(store.getActions()).toContainEqual({
        payload: [],
        type: 'properties/storeDraftProperties',
      });
    });
    const draftMarker = getByTitle('select properties on the map');
    userEvent.click(draftMarker);
    const text = getByTitle('1');
    userEvent.click(text);
    expect(setCursor).toHaveBeenCalledWith(undefined);
  });
});
