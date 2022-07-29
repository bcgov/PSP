import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { mockLookups } from 'mocks/mockLookups';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import { AddAcquisitionContainer, IAddAcquisitionContainerProps } from './AddAcquisitionContainer';
import { AcquisitionForm } from './models';

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
  ) => {
    const utils = render(<AddAcquisitionContainer {...props} />, {
      ...renderOptions,
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
      history,
    });

    return {
      ...utils,
      getSaveButton: () => utils.getByText(/Save/i),
      getCancelButton: () => utils.getByText(/Cancel/i),
      getNameTextbox: () => utils.container.querySelector(`input[name="name"]`) as HTMLInputElement,
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

  it('should save the form and navigate to details view when Save button is clicked', async () => {
    const formValues = new AcquisitionForm();
    formValues.name = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.';
    formValues.acquisitionType = 'CONSEN';
    formValues.region = 1;

    const { getSaveButton, getNameTextbox, getAcquisitionTypeDropdown, getRegionDropdown } = setup(
      DEFAULT_PROPS,
    );

    await waitFor(() => userEvent.paste(getNameTextbox(), formValues.name as string));
    await userEvent.selectOptions(getAcquisitionTypeDropdown(), formValues.acquisitionType);
    await userEvent.selectOptions(getRegionDropdown(), formValues.region.toString());

    mockAxios.onPost().reply(200, mockAcquisitionFileResponse(1, formValues.name));
    userEvent.click(getSaveButton());

    await waitFor(() => {
      const axiosData: Api_AcquisitionFile = JSON.parse(mockAxios.history.post[0].data);
      const expectedValues = formValues.toApi();

      expect(mockAxios.history.post[0].url).toBe('/acquisitionfiles');
      expect(axiosData).toEqual(expectedValues);

      expect(history.location.pathname).toBe('/mapview/acquisition/1');
    });
  });
});
