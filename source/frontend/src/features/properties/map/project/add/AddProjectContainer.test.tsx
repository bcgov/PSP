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

import { ProjectForm } from '../models';
import AddProjectContainer, { IAddProjectContainerProps } from './AddProjectContainer';

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
      getNumberTextbox: () =>
        utils.container.querySelector(`input[name="projectNumber"]`) as HTMLInputElement,
      getRegionDropdown: () =>
        utils.container.querySelector(`select[name="region"]`) as HTMLSelectElement,
      getStatusDropdown: () =>
        utils.container.querySelector(`select[name="projectStatusType"]`) as HTMLSelectElement,
      getSummaryTextbox: () =>
        utils.container.querySelector(`textarea[name="summary"]`) as HTMLInputElement,
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
    const { getByText, getNameTextbox, getRegionDropdown, getNumberTextbox, getStatusDropdown } =
      setup();

    const formTitle = getByText(/Create Project/i);
    const nameInput = getNameTextbox();
    const numberInput = getNumberTextbox();
    const selectRegion = getRegionDropdown();
    const selectSatus = getStatusDropdown();

    expect(formTitle).toBeVisible();
    expect(nameInput).toBeVisible();
    expect(nameInput.tagName).toBe('INPUT');
    expect(numberInput).toBeVisible();
    expect(numberInput.tagName).toBe('INPUT');
    expect(selectRegion).toBeVisible();
    expect(selectRegion.tagName).toBe('SELECT');
    expect(selectSatus).toBeVisible();
    expect(selectSatus.tagName).toBe('SELECT');
  });

  it('should save the form and navigate to details view when Save button is clicked', async () => {
    const formValues = new ProjectForm();
    formValues.projectName = 'TRANS-CANADA HWY - 10';
    formValues.projectNumber = '99999';
    formValues.region = 1;
    formValues.projectStatusType = 'AC';
    formValues.summary = 'NEW PROJECT';

    const {
      getSaveButton,
      getNameTextbox,
      getRegionDropdown,
      getNumberTextbox,
      getStatusDropdown,
      getSummaryTextbox,
    } = setup(DEFAULT_PROPS);

    await waitFor(() => userEvent.paste(getNameTextbox(), formValues.projectName as string));
    await waitFor(() => userEvent.paste(getNumberTextbox(), formValues.projectNumber as string));
    userEvent.selectOptions(getRegionDropdown(), formValues.region.toString());
    userEvent.selectOptions(getStatusDropdown(), formValues.projectStatusType as string);
    await waitFor(() => userEvent.paste(getSummaryTextbox(), formValues.summary as string));

    mockAxios
      .onPost()
      .reply(
        201,
        mockProjectPostResponse(
          1,
          1,
          formValues.projectName,
          formValues.projectNumber,
          formValues.region,
          formValues.projectStatusType,
          formValues.summary,
        ),
      );
    userEvent.click(getSaveButton());

    await waitFor(() => {
      const axiosData: Api_Project = JSON.parse(mockAxios.history.post[0].data);
      const expectedValues = formValues.toApi();

      expect(mockAxios.history.post[0].url).toBe('/projects');
      expect(axiosData).toEqual(expectedValues);

      expect(history.location.pathname).toBe('/mapview/sidebar/project/1');
    });
  });
});
