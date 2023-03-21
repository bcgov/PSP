import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { mockLookups } from 'mocks/mockLookups';
import { mockNotesResponse } from 'mocks/mockNoteResponses';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions } from 'utils/test-utils';

import { UpdateAcquisitionSummaryFormModel } from './models';
import UpdateAcquisitionForm, { IUpdateAcquisitionFormProps } from './UpdateAcquisitionForm';

const mockAxios = new MockAdapter(axios);

// mock auth library
jest.mock('@react-keycloak/web');

const onSubmit = jest.fn();

const DEFAULT_PROPS: IUpdateAcquisitionFormProps = {
  validationSchema: {} as any,
  initialValues: UpdateAcquisitionSummaryFormModel.fromApi(mockAcquisitionFileResponse()),
  onSubmit,
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

describe('UpdateAcquisitionForm component', () => {
  // render component under test
  const setup = (
    props: IUpdateAcquisitionFormProps = { ...DEFAULT_PROPS },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(<UpdateAcquisitionForm {...props} />, {
      claims: [],
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
    });

    return {
      ...utils,
      getCloseButton: () => utils.getByTitle('close'),
    };
  };

  beforeEach(() => {
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
    mockAxios
      .onGet(new RegExp('acquisitionfiles/1/properties'))
      .reply(200, mockAcquisitionFileResponse().fileProperties);
    mockAxios.onGet(new RegExp('acquisitionfiles/*')).reply(200, mockAcquisitionFileResponse());
    mockAxios.onGet(new RegExp('notes/*')).reply(200, mockNotesResponse());
  });

  afterEach(() => {
    mockAxios.resetHistory();
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();

    expect(asFragment()).toMatchSnapshot();
  });

  it('displays legacy file number', async () => {
    const { getByDisplayValue } = setup();

    expect(getByDisplayValue('legacy file number')).toBeVisible();
  });

  it('Displays owners', async () => {
    const { getByDisplayValue } = setup();

    expect(getByDisplayValue('FORTIS BC')).toBeVisible();
    expect(getByDisplayValue('123 Main Street')).toBeVisible();
    expect(getByDisplayValue('John')).toBeVisible();
    expect(getByDisplayValue('456 Souris Street')).toBeVisible();
  });
});
