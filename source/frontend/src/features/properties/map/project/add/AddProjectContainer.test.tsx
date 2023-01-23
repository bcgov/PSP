import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { MapStateContextProvider } from 'components/maps/providers/MapStateContext';
import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { mockProjectPostResponse } from 'mocks/mockProjects';
import { Api_Project } from 'models/api/Project';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import AddProjectContainer, { IAddProjectContainerProps } from './AddProjectContainer';
import { ProjectForm } from './models';

jest.mock('@react-keycloak/web');

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);

const onClose = jest.fn();

const DEFAULT_PROPS: IAddProjectContainerProps = {
  onClose,
};

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

describe('AddProjectContainer component', () => {
  // render component under test
  const setup = (
    props: IAddProjectContainerProps = DEFAULT_PROPS,
    renderOptions: RenderOptions = {},
    selectedProperty: Feature<Geometry, GeoJsonProperties> | null = null,
  ) => {
    const utils = render(
      <MapStateContextProvider values={{ selectedFileFeature: selectedProperty }}>
        <AddProjectContainer {...props} />
      </MapStateContextProvider>,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        claims: [],
        history,
      },
    );

    return {
      ...utils,
      getSaveButton: () => utils.getByText(/Save/i),
      getNameTextbox: () =>
        utils.container.querySelector(`input[name="projectName"]`) as HTMLInputElement,
      getRegionDropdown: () =>
        utils.container.querySelector(`select[name="region"]`) as HTMLSelectElement,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the underlying form', () => {
    const { getByText, getNameTextbox, getRegionDropdown } = setup();

    const formTitle = getByText(/Create Project/i);
    const input = getNameTextbox();
    const select = getRegionDropdown();

    expect(formTitle).toBeVisible();
    expect(input).toBeVisible();
    expect(input.tagName).toBe('INPUT');
    expect(select).toBeVisible();
    expect(select.tagName).toBe('SELECT');
  });

  it.skip('should save the form and navigate to details view when Save button is clicked', async () => {
    const formValues = new ProjectForm();
    formValues.projectName = 'TRANS-CANADA HWY - 10';
    formValues.region = 1;

    const { getSaveButton, getNameTextbox, getRegionDropdown } = setup(DEFAULT_PROPS);

    await waitFor(() => userEvent.paste(getNameTextbox(), formValues.projectName as string));
    userEvent.selectOptions(getRegionDropdown(), formValues.region.toString());

    mockAxios
      .onPost()
      .reply(201, mockProjectPostResponse(1, 1, formValues.projectName, formValues.region));
    userEvent.click(getSaveButton());

    await waitFor(() => {
      const axiosData: Api_Project = JSON.parse(mockAxios.history.post[0].data);
      const expectedValues = formValues.toApi();

      expect(mockAxios.history.post[0].url).toBe('/projects');
      expect(axiosData).toEqual(expectedValues);

      // expect(history.location.pathname).toBe('/mapview/sidebar/acquisition/1');
    });
  });
});
