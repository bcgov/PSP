import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { FormDocumentType } from '@/constants/formDocumentTypes';
import { FileTypes } from '@/constants/index';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, waitFor } from '@/utils/test-utils';

import GenerateFormContainer, { IGenerateFormContainerProps } from './GenerateFormContainer';
import { IGenerateFormViewProps } from './GenerateFormView';
import { useGenerateH0443 } from './hooks/useGenerateH0443';
import { useGenerateLetter } from './hooks/useGenerateLetter';

const mockAxios = new MockAdapter(axios);
const generateLetterFn = jest.fn();
const generateH0443Fn = jest.fn();

// mock auth library
jest.mock('@react-keycloak/web');
jest.mock('./hooks/useGenerateLetter');
(useGenerateLetter as jest.Mock).mockImplementation(() => generateLetterFn);

jest.mock('./hooks/useGenerateH0443');
(useGenerateH0443 as jest.Mock).mockImplementation(() => generateH0443Fn);

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

jest.mock('@/components/common/mapFSM/MapStateMachineContext');
(useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);

let viewProps: IGenerateFormViewProps = {} as any;
const GenerateFormViewStub = (props: IGenerateFormViewProps) => {
  viewProps = props;
  return <>Generate Form View Rendered</>;
};
const DEFAULT_PROPS: IGenerateFormContainerProps = {
  acquisitionFileId: 1,
  View: GenerateFormViewStub,
};

describe('GenerateFormContainer component', () => {
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
    };
  };

  beforeEach(() => {});

  afterEach(() => {
    mockAxios.resetHistory();
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();

    expect(asFragment()).toMatchSnapshot();
  });

  it('calls document H0443 generation', async () => {
    jest.spyOn(global, 'confirm' as any).mockReturnValueOnce(true);

    await act(async () => viewProps.onGenerateClick(FormDocumentType.H0443));
    await waitFor(async () => {
      expect(generateLetterFn).toHaveBeenCalledTimes(0);
      expect(generateH0443Fn).toHaveBeenCalledTimes(1);
    });
  });

  it('opens document letter generation modal', async () => {
    jest.spyOn(global, 'confirm' as any).mockReturnValueOnce(true);

    await act(async () => viewProps.onGenerateClick(FormDocumentType.LETTER));
    await waitFor(async () => {
      expect(generateLetterFn).toHaveBeenCalledTimes(0);
      expect(generateH0443Fn).toHaveBeenCalledTimes(0);
    });
  });

  it('generates the documment letter on confirmation', async () => {
    await act(async () => viewProps.onGenerateLetterOk([]));
    await waitFor(async () => {
      expect(generateLetterFn).toHaveBeenCalledTimes(1);
    });
  });
});
