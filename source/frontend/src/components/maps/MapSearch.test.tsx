import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  cleanup,
  getByName,
  render,
  RenderOptions,
  screen,
  userEvent,
} from '@/utils/test-utils';

import MapSearch from './MapSearch';

describe('MapSearch component', () => {
  afterEach(cleanup);

  const setup = async (renderOptions: RenderOptions = {}) => {
    const utils = render(<MapSearch />, {
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
      ...renderOptions,
    });
    // wait for useEffects
    await act(async () => {});

    const searchButton = screen.getByTitle('search');
    const resetButton = screen.getByTitle('reset-button');
    const searchByDropdown = getByName('searchBy') as HTMLSelectElement;

    return {
      ...utils,
      searchButton,
      resetButton,
      searchByDropdown,
    };
  };

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('searches by PID', async () => {
    const { searchButton } = await setup({
      mockMapMachine: mapMachineBaseMock,
    });

    // enter values on the form fields, then click the Search button
    await act(async () => {
      const input = getByName('pid');
      userEvent.paste(input, '9999999');
    });
    await act(async () => {
      userEvent.click(searchButton);
    });

    expect(mapMachineBaseMock.setMapSearchCriteria).toHaveBeenCalled();
  });

  it('searches by Lat/Long coordinates', async () => {
    const { searchByDropdown, searchButton } = await setup({
      mockMapMachine: mapMachineBaseMock,
    });

    await act(async () => {
      userEvent.selectOptions(searchByDropdown, 'coordinates');
    });

    expect(screen.getByText('Lat:')).toBeVisible();

    // enter values on the form fields, then click the Search button
    await act(async () => {
      const input = getByName('coordinates.latitude.degrees');
      userEvent.paste(input, '55');
    });
    await act(async () => {
      const input = getByName('coordinates.latitude.minutes');
      userEvent.paste(input, '46');
    });
    await act(async () => {
      const input = getByName('coordinates.latitude.seconds');
      userEvent.paste(input, '48.155');
    });
    await act(async () => {
      userEvent.click(searchButton);
    });

    // it tells the map FSM to execute a map click and center on the lat/long
    expect(mapMachineBaseMock.mapClick).toHaveBeenCalled();
    expect(mapMachineBaseMock.requestCenterToLocation).toHaveBeenCalled();
  });
});
