import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { FormDocumentType } from 'constants/formDocumentTypes';
import { FileTypes } from 'constants/index';
import { useDocumentGenerationRepository } from 'features/documents/hooks/useDocumentGenerationRepository';
import { SideBarContextProvider } from 'features/properties/map/context/sidebarContext';
import {
  mockAcquisitionFileOwnersResponse,
  mockAcquisitionFileResponse,
} from 'mocks/mockAcquisitionFiles';
import { mockLookups } from 'mocks/mockLookups';
import { mockNotesResponse } from 'mocks/mockNoteResponses';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, render, RenderOptions, waitFor, waitForElementToBeRemoved } from 'utils/test-utils';

import GenerateFormContainer, { IGenerateFormContainerProps } from './GenerateFormContainer';
import { IGenerateFormViewProps } from './GenerateFormView';

const mockAxios = new MockAdapter(axios);
const generateFn = jest.fn();

// mock auth library
jest.mock('@react-keycloak/web');
jest.mock('features/documents/hooks/useDocumentGenerationRepository');
(useDocumentGenerationRepository as jest.Mock).mockImplementation(() => ({
  generateDocumentDownloadWrappedRequest: generateFn,
}));

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

let viewProps: IGenerateFormViewProps = {} as any;
const GenerateFormViewStub = (props: IGenerateFormViewProps) => {
  viewProps = props;
  return <>Generate Form View Rendered</>;
};
const DEFAULT_PROPS: IGenerateFormContainerProps = {
  acquisitionFileId: 1,
  View: GenerateFormViewStub,
};

describe('AcquisitionContainer component', () => {
  // render component under test
  const setup = (
    props: IGenerateFormContainerProps = { ...DEFAULT_PROPS },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <SideBarContextProvider
        file={{
          ...mockAcquisitionFileResponse(),
          fileType: FileTypes.Acquisition,
        }}
      >
        <GenerateFormContainer {...props} View={GenerateFormViewStub} />
      </SideBarContextProvider>,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [],
        ...renderOptions,
      },
    );

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
    mockAxios
      .onGet(new RegExp('acquisitionfiles/1/owners'))
      .reply(200, mockAcquisitionFileOwnersResponse());
    mockAxios.onGet(new RegExp('acquisitionfiles/*')).reply(200, mockAcquisitionFileResponse());
    mockAxios.onGet(new RegExp('notes/*')).reply(200, mockNotesResponse());
  });

  afterEach(() => {
    mockAxios.resetHistory();
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment, getByTestId } = setup();

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    expect(asFragment()).toMatchSnapshot();
  });

  it('calls document generation endpoint when generate function called', async () => {
    const { getByTestId } = setup(undefined, { claims: [] });
    jest.spyOn(global, 'confirm' as any).mockReturnValueOnce(true);

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    await act(async () => viewProps.onGenerateClick(FormDocumentType.LETTER));

    await waitFor(async () => expect(generateFn).toHaveBeenCalledTimes(1));
  });
});
