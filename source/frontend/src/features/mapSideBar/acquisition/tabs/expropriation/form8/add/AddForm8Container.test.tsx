import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { Claims } from '@/constants';
import { mockAcquisitionFileOwnersResponse } from '@/mocks/acquisitionFiles.mock';
import { mockGetExpropriationPaymentApi } from '@/mocks/ExpropriationPayment.mock';
import { getMockApiInterestHolders } from '@/mocks/interestHolders.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_ExpropriationPayment } from '@/models/api/generated/ApiGen_Concepts_ExpropriationPayment';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { systemConstantsSlice } from '@/store/slices/systemConstants/systemConstantsSlice';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { IForm8FormProps } from '../UpdateForm8Form';
import AddForm8Container, { IAddForm8ContainerProps } from './AddForm8Container';

const history = createMemoryHistory();
const mockInteresHoldersResponse = getMockApiInterestHolders();
const mockFileOwnersResponse = mockAcquisitionFileOwnersResponse();

const mockPostApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

const mockGetInterestHoldersApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

const mockGetFileOwnersApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

jest.mock('@/hooks/repositories/useAcquisitionProvider', () => ({
  useAcquisitionProvider: () => {
    return {
      getAcquisitionOwners: mockGetFileOwnersApi,
      postAcquisitionForm8: mockPostApi,
    };
  },
}));

jest.mock('@/hooks/repositories/useInterestHolderRepository', () => ({
  useInterestHolderRepository: () => {
    return {
      getAcquisitionInterestHolders: mockGetInterestHoldersApi,
    };
  },
}));

let viewProps: IForm8FormProps | undefined;
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
      <AddForm8Container acquisitionFileId={1} View={TestView} onSuccess={noop} />,
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
    viewProps = undefined;
    jest.resetAllMocks();
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

  it('makes request to create a new form8 and returns the response', async () => {
    await setup({ props: { acquisitionFileId: 1 } });
    mockGetInterestHoldersApi.execute.mockReturnValue(mockInteresHoldersResponse);
    mockGetFileOwnersApi.execute.mockReturnValue(mockFileOwnersResponse);
    mockPostApi.execute.mockReturnValue(mockGetExpropriationPaymentApi());

    let createdForm8: ApiGen_Concepts_ExpropriationPayment | undefined;
    await act(async () => {
      createdForm8 = await viewProps?.onSave({} as ApiGen_Concepts_ExpropriationPayment);
    });

    expect(mockPostApi.execute).toHaveBeenCalled();
    expect(createdForm8).toStrictEqual({ ...mockGetExpropriationPaymentApi() });

    expect(history.location.pathname).toBe('/');
  });

  it('navigates back to expropriation tab when form is cancelled', async () => {
    await setup();
    act(() => {
      viewProps?.onCancel();
    });

    expect(history.location.pathname).toBe('/');
    expect(mockPostApi.execute).not.toHaveBeenCalled();
  });
});
