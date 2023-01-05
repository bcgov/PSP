import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { MapStateContextProvider } from 'components/maps/providers/MapStateContext';
import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import { createMemoryHistory } from 'history';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { mockLookups } from 'mocks/mockLookups';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import { AddAcquisitionContainer, IAddAcquisitionContainerProps } from './AddAcquisitionContainer';
import { AcquisitionForm } from './models';
jest.mock('@react-keycloak/web');

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);

const onClose = jest.fn();

const DEFAULT_PROPS: IAddAcquisitionContainerProps = {
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

describe('AddAcquisitionContainer component', () => {
  // render component under test
  const setup = (
    props: IAddAcquisitionContainerProps = DEFAULT_PROPS,
    renderOptions: RenderOptions = {},
    selectedProperty: Feature<Geometry, GeoJsonProperties> | null = null,
  ) => {
    const utils = render(
      <MapStateContextProvider values={{ selectedFileFeature: selectedProperty }}>
        <AddAcquisitionContainer {...props} />
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
      getCancelButton: () => utils.getByText(/Cancel/i),
      getNameTextbox: () =>
        utils.container.querySelector(`input[name="fileName"]`) as HTMLInputElement,
      getRegionDropdown: () =>
        utils.container.querySelector(`select[name="region"]`) as HTMLSelectElement,
      getAcquisitionTypeDropdown: () =>
        utils.container.querySelector(`select[name="acquisitionType"]`) as HTMLSelectElement,
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

    const formTitle = getByText(/Create Acquisition File/i);
    const input = getNameTextbox();
    const select = getRegionDropdown();

    expect(formTitle).toBeVisible();
    expect(input).toBeVisible();
    expect(input.tagName).toBe('INPUT');
    expect(select).toBeVisible();
    expect(select.tagName).toBe('SELECT');
  });

  it('should close the form when Cancel button is clicked', () => {
    const { getCancelButton, getByText } = setup();

    expect(getByText(/Create Acquisition File/i)).toBeVisible();
    userEvent.click(getCancelButton());

    expect(onClose).toBeCalled();
  });

  it('should pre-populate the region if a property is selected', async () => {
    const { findByDisplayValue } = setup(undefined, undefined, {
      properties: { REGION_NUMBER: 1 },
      geometry: {
        type: 'Polygon',
        coordinates: [
          [
            [-120.69195885, 50.25163372],
            [-120.69176022, 50.2588544],
            [-120.69725103, 50.25889407],
            [-120.70326422, 50.25893724],
            [-120.70352697, 50.25172245],
            [-120.70287648, 50.25171749],
            [-120.70200152, 50.25171082],
            [-120.69622707, 50.2516666],
            [-120.69195885, 50.25163372],
          ],
        ],
      },
    } as any);
    const text = await findByDisplayValue(/South Coast Region/i);
    expect(text).toBeVisible();
  });

  it('should save the form and navigate to details view when Save button is clicked', async () => {
    const formValues = new AcquisitionForm();
    formValues.fileName = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.';
    formValues.acquisitionType = 'CONSEN';
    formValues.region = '1';

    const { getSaveButton, getNameTextbox, getAcquisitionTypeDropdown, getRegionDropdown } =
      setup(DEFAULT_PROPS);

    await waitFor(() => userEvent.paste(getNameTextbox(), formValues.fileName as string));
    await userEvent.selectOptions(getAcquisitionTypeDropdown(), formValues.acquisitionType);
    await userEvent.selectOptions(getRegionDropdown(), formValues.region);

    mockAxios.onPost().reply(200, mockAcquisitionFileResponse(1, formValues.fileName));
    userEvent.click(getSaveButton());

    await waitFor(() => {
      const axiosData: Api_AcquisitionFile = JSON.parse(mockAxios.history.post[0].data);
      const expectedValues = formValues.toApi();

      expect(mockAxios.history.post[0].url).toBe('/acquisitionfiles');
      expect(axiosData).toEqual(expectedValues);

      expect(history.location.pathname).toBe('/mapview/sidebar/acquisition/1');
    });
  });
});
