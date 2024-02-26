import { mockAgreementResponseApi } from '@/mocks/agreements.mock';
import { createMemoryHistory } from 'history';
import { IUpdateAcquisitionAgreementViewProps } from '../common/UpdateAcquisitionAgreementView';
import { RenderOptions, act, render, waitForEffects } from '@/utils/test-utils';
import UpdateAcquisitionAgreementContainer, {
  IUpdateAcquisitionAgreementContainerProps,
} from './UpdateAcquisitionAgreementContainer';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { Claims } from '@/constants/claims';
import { AcquisitionAgreementFormModel } from '../models/AcquisitionAgreementFormModel';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';

const history = createMemoryHistory();
const mockAcquisitionAgreementApi = mockAgreementResponseApi(1);

const mockPutAgreementApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

const mockGetAgreementApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn().mockResolvedValue(mockAcquisitionAgreementApi),
  loading: false,
};

const onSuccess = jest.fn();

jest.mock('@/hooks/repositories/useAgreementProvider', () => ({
  useAgreementProvider: () => {
    return {
      getAcquisitionAgreementById: mockGetAgreementApi,
      updateAcquisitionAgreement: mockPutAgreementApi,
    };
  },
}));

// eslint-disable-next-line @typescript-eslint/no-unused-vars
let viewProps: IUpdateAcquisitionAgreementViewProps | undefined;
const TestView: React.FC<IUpdateAcquisitionAgreementViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('Update AcquisitionAgreementContainer component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IUpdateAcquisitionAgreementContainerProps>;
    } = {},
  ) => {
    const component = render(
      <UpdateAcquisitionAgreementContainer
        acquisitionFileId={1}
        agreementId={10}
        View={TestView}
        onSuccess={onSuccess}
      />,
      {
        history,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.ACQUISITION_EDIT, Claims.AGREEMENT_VIEW],
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
    const { getByText } = await setup({props: { acquisitionFileId: 1, agreementId: 10 }});

    expect(getByText(/Content Rendered/)).toBeVisible();
    expect(mockGetAgreementApi.execute).toHaveBeenCalledWith(1, 10);
  });

  it('Loads props with the initial values', async () => {
    mockGetAgreementApi.execute.mockResolvedValue(mockAcquisitionAgreementApi);
    await setup({props: { acquisitionFileId: 1, agreementId: 10 }});
    await waitForEffects();

    expect(mockGetAgreementApi.execute).toHaveBeenCalledWith(1, 10);
    const formModel = AcquisitionAgreementFormModel.fromApi(mockAcquisitionAgreementApi);

    expect(viewProps?.initialValues).toStrictEqual(formModel);
  });

  it('makes request to create a new Agreement and returns the response', async () => {
    mockGetAgreementApi.execute.mockResolvedValue(mockAcquisitionAgreementApi);
    mockPutAgreementApi.execute.mockReturnValue(mockAcquisitionAgreementApi);

    await setup({props: { acquisitionFileId: 1, agreementId: 10 }});

    let createdAgreement: ApiGen_Concepts_Agreement | undefined;
    await act(async () => {
      createdAgreement = await viewProps?.onSave({} as ApiGen_Concepts_Agreement);
    });

    expect(mockPutAgreementApi.execute).toHaveBeenCalled();
    expect(createdAgreement).toStrictEqual({ ...mockAcquisitionAgreementApi });
    expect(history.location.pathname).toBe('/');
  });

  it('navigates back to Agreement when form is cancelled', async () => {
    await setup({props: { acquisitionFileId: 1, agreementId: 10 }});
    act(() => {
      viewProps?.onCancel();
    });

    expect(history.location.pathname).toBe('/');
    expect(mockPutAgreementApi.execute).not.toHaveBeenCalled();
  });
});
