import { createMemoryHistory } from 'history';

import { Claims } from '@/constants';
import {
  mockAcquisitionFileOwnersResponse,
  mockAcquisitionFileResponse,
} from '@/mocks/acquisitionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { Api_Form8 } from '@/models/api/Form8';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { systemConstantsSlice } from '@/store/slices/systemConstants/systemConstantsSlice';
import { act, render, RenderOptions, waitFor } from '@/utils/test-utils';

import { IForm8FormProps } from '../UpdateForm8Form';
import AddForm8Container, { IAddForm8ContainerProps } from './AddForm8Container';

const history = createMemoryHistory();
const mockAcquisitionFile = mockAcquisitionFileResponse();

const mockPostApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

jest.mock('@/hooks/repositories/useAcquisitionProvider', () => ({
  useAcquisitionProvider: () => {
    return {
      getAcquisitionOwners: {
        error: undefined,
        response: mockAcquisitionFileOwnersResponse(mockAcquisitionFile.id),
        execute: jest
          .fn()
          .mockReturnValue(mockAcquisitionFileOwnersResponse(mockAcquisitionFile.id)),
        loading: false,
      },
      postAcquisitionForm8: mockPostApi,
    };
  },
}));

jest.mock('@/hooks/repositories/useInterestHolderRepository', () => ({
  useInterestHolderRepository: () => {
    return {
      getAcquisitionInterestHolders: {
        execute: jest.fn(),
        loading: false,
      },
    };
  },
}));

let viewProps: IForm8FormProps | null;
const TestView: React.FC<IForm8FormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('Add Form8 Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IAddForm8ContainerProps>;
    } = {},
  ) => {
    const component = render(
      <AddForm8Container
        acquisitionFileId={renderOptions?.props?.acquisitionFileId ?? mockAcquisitionFile.id!}
        View={TestView}
      />,
      {
        history,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
          [systemConstantsSlice.name]: { systemConstants: [{ name: 'GST', value: '5.0' }] },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.ACQUISITION_EDIT],
        ...renderOptions,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    viewProps = null;
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('Loads props with the initial values', async () => {
    await setup();

    expect(viewProps?.initialValues?.id).toBe(null);
    expect(viewProps?.initialValues?.acquisitionFileId).toBe(1);
  });

  it('navigates back to expropriation tab when form is cancelled', async () => {
    await setup();
    await act(async () => {
      viewProps?.onCancel();
    });

    expect(history.location.pathname).toBe('/');
  });

  it('makes request to create a new form8 and returns the response', async () => {
    await setup();
    await act(async () => {
      viewProps?.onSave({} as Api_Form8);
    });

    await waitFor(async () => {
      expect(mockPostApi.execute).toHaveBeenCalled();
    });
  });
});
