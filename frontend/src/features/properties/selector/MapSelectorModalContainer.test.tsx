import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { propertiesSlice } from 'store/slices/properties';
import { render, RenderOptions, userEvent } from 'utils/test-utils';

import MapSelectorModalContainer, {
  IMapSelectorModalContainerProps,
} from './MapSelectorModalContainer';

const mockStore = configureMockStore([thunk]);

const store = mockStore({
  [propertiesSlice.name]: {},
});

const setSelectedProperties = jest.fn();

describe('MapSelectorModalContainer component', () => {
  const setup = (
    renderOptions: RenderOptions & IMapSelectorModalContainerProps = {
      setSelectedProperties: setSelectedProperties,
    } as any,
  ) => {
    // render component under test
    const component = render(
      <MapSelectorModalContainer
        selectedProperties={renderOptions.selectedProperties}
        setSelectedProperties={setSelectedProperties}
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
    const { component } = setup();
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('does not display by default', async () => {
    const {
      component: { queryByText },
    } = await setup();

    expect(queryByText('Property Selection')).toBeNull();
  });

  it('displays on button click', async () => {
    const {
      component: { getByText },
    } = await setup();

    const btn = getByText('Select Properties');
    userEvent.click(btn);
    expect(getByText('Property Selection')).toBeVisible();
  });

  it('saves selected properties on save', async () => {
    const {
      component: { getByText },
    } = await setup({
      setSelectedProperties: setSelectedProperties,
      selectedProperties: [{ pid: '111' } as any],
    });

    const btn = getByText('Select Properties');
    userEvent.click(btn);
    const saveBtn = getByText('Add to File');
    userEvent.click(saveBtn);
    expect(setSelectedProperties).toHaveBeenCalledWith([{ pid: '111' }]);
  });

  it('saves no properties on cancel', async () => {
    const {
      component: { getByText },
    } = await setup();

    const btn = getByText('Select Properties');
    userEvent.click(btn);
    const saveBtn = getByText('Add to File');
    userEvent.click(saveBtn);
    expect(setSelectedProperties).toHaveBeenCalledWith([]);
  });
});
