import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import AcquisitionGenerateContainer, {
  IGenerateFormContainerProps,
} from './AcquisitionGenerateContainer';
import { IGenerateFormViewProps } from './GenerateFormView';
import { useGenerateH0443 } from './hooks/useGenerateH0443';
import { useGenerateLetter } from './hooks/useGenerateLetter';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';

const mockAxios = new MockAdapter(axios);
const generateLetterFn = vi.fn().mockResolvedValue({});
const generateH0443Fn = vi.fn().mockResolvedValue({});

// mock auth library

vi.mock('./hooks/useGenerateLetter');
vi.mocked(useGenerateLetter).mockImplementation(() => generateLetterFn);

vi.mock('./hooks/useGenerateH0443');
vi.mocked(useGenerateH0443).mockImplementation(() => generateH0443Fn);

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children;
    }),
  };
});

let viewProps: React.PropsWithChildren<IGenerateFormViewProps>= {} as any;
const GenerateFormViewStub = (props: IGenerateFormViewProps) => {
  viewProps = props;
  return <>Generate Form View Rendered<div >{viewProps.children}</div></>;
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
          fileType: ApiGen_CodeTypes_FileTypes.Acquisition,
        }}
      >
        <AcquisitionGenerateContainer {...props} View={GenerateFormViewStub} />
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
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();

    expect(asFragment()).toMatchSnapshot();
  });

  it('calls document H0443 generation', async () => {
    const { getAllByText } = setup();
    const generateButton = getAllByText('Conditions of Entry (H0443)')[0];

    await act(async () => {
      userEvent.click(generateButton);
    });

    await waitFor(async () => {
      expect(generateLetterFn).toHaveBeenCalledTimes(0);
      expect(generateH0443Fn).toHaveBeenCalledTimes(1);
    });
  });

  it('opens document letter generation modal', async () => {
    const { getAllByText } = setup();
    const letterButton = getAllByText('Generate Letter')[0];

    await act(async () => {
      userEvent.click(letterButton);
    });

    await waitFor(async () => {
      expect(generateLetterFn).toHaveBeenCalledTimes(0);
      expect(generateH0443Fn).toHaveBeenCalledTimes(0);
    });
  });
});
