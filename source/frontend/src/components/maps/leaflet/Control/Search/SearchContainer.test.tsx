import { Claims } from '@/constants';
import { useGeographicNamesRepository } from '@/hooks/useGeographicNamesRepository';
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

import { SearchContainer } from './SearchContainer';
import { SearchView } from './SearchView';

vi.mock('@/hooks/useGeographicNamesRepository');

const mockSearchName = {
  execute: vi.fn(),
  error: null,
  loading: false,
  status: null,
  response: null,
};

vi.mocked(useGeographicNamesRepository).mockReturnValue({
  searchName: mockSearchName,
});

describe('SearchContainer component', () => {
  afterEach(cleanup);

  const setup = async (renderOptions: RenderOptions = {}) => {
    const utils = render(<SearchContainer View={SearchView} />, {
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
      ...renderOptions,
      useMockAuthentication: true,
      claims: renderOptions.claims ?? [
        Claims.RESEARCH_ADD,
        Claims.ACQUISITION_ADD,
        Claims.DISPOSITION_ADD,
        Claims.LEASE_ADD,
        Claims.MANAGEMENT_ADD,
      ],
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

  it('searches by geographic name', async () => {
    mockSearchName.execute.mockResolvedValueOnce({
      features: [
        {
          geometry: {
            type: 'Point',
            coordinates: [1, 2],
          },
          properties: {
            name: 'Test Location',
            featureType: 'Type1',
            featureCategoryDescription: 'Category1',
          },
        },
        {
          properties: {
            name: 'Another Location',
            featureType: 'Type2',
            featureCategoryDescription: 'Category2',
          },
        },
      ],
    });

    const { searchByDropdown, searchButton } = await setup({
      mockMapMachine: mapMachineBaseMock,
    });

    await act(async () => {
      userEvent.selectOptions(searchByDropdown, 'name');
    });

    await act(async () => {
      const input = getByName('name');
      userEvent.paste(input, 'langford');
    });

    const nameSuggestion = await screen.findByText('Test Location', { exact: false });
    await act(async () => {
      userEvent.click(nameSuggestion!);
    });

    await act(async () => {
      userEvent.click(searchButton);
    });

    // it tells the map FSM to execute a map click and center on the lat/long
    expect(mapMachineBaseMock.mapClick).toHaveBeenCalled();
    expect(mapMachineBaseMock.requestCenterToLocation).toHaveBeenCalled();
  });
});
