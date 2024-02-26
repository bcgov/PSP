
import { createMemoryHistory } from 'history';
import { RenderOptions, act, render } from "@/utils/test-utils";
import AddAcquisitionAgreementContainer, { IAddAcquisitionAgreementContainerProps } from "./AddAcquisitionAgreementContainer";
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { IUpdateAcquisitionAgreementViewProps } from '../common/UpdateAcquisitionAgreementView';
import { Claims } from '@/constants/claims';
import { mockAgreementResponseApi } from '@/mocks/agreements.mock';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';


const history = createMemoryHistory();
const mockPostApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};
const onSuccess = jest.fn();

// eslint-disable-next-line @typescript-eslint/no-unused-vars
let viewProps: IUpdateAcquisitionAgreementViewProps | undefined;
const TestView: React.FC<IUpdateAcquisitionAgreementViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

jest.mock('@/hooks/repositories/useAgreementProvider', () => ({
  useAgreementProvider: () => {
    return {
      addAcquisitionAgreement: mockPostApi,
    };
  },
}));

describe('Add Disposition Offer Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IAddAcquisitionAgreementContainerProps>;
    } = {},
  ) => {
    const component = render(
      <AddAcquisitionAgreementContainer acquisitionFileId={1} View={TestView} onSuccess={onSuccess} />,
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
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('Loads props with the initial values', async () => {
    await setup({ props: { acquisitionFileId: 1 } });

    expect(viewProps?.initialValues?.agreementId).toBe(0);
    expect(viewProps?.initialValues?.acquisitionFileId).toBe(1);
    expect(viewProps?.initialValues?.agreementStatusTypeCode).toBe('DRAFT');
    expect(viewProps?.initialValues?.agreementTypeCode).toBe('');
    expect(viewProps?.initialValues?.agreementDate).toBe('');
    expect(viewProps?.initialValues?.cancellationNote).toBe('');
    expect(viewProps?.initialValues?.rowVersion).toBe(null);
  });

  it('makes request to create a new Agreement and returns the response', async () => {
    await setup();
    const agreementMock = mockAgreementResponseApi(1);
    mockPostApi.execute.mockReturnValue(agreementMock);

    let createdAgreement: ApiGen_Concepts_Agreement | undefined;
    await act(async () => {
      createdAgreement = await viewProps?.onSave({} as ApiGen_Concepts_Agreement);
    });

    expect(mockPostApi.execute).toHaveBeenCalled();
    expect(createdAgreement).toStrictEqual({ ...agreementMock });

    expect(history.location.pathname).toBe('/');
  });

  it('navigates back to Agreements when form is cancelled', async () => {
    await setup();
    act(() => {
      viewProps?.onCancel();
    });

    expect(history.location.pathname).toBe('/');
    expect(mockPostApi.execute).not.toHaveBeenCalled();
  });
});
