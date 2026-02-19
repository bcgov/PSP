import { mockAgreementResponseApi } from '@/mocks/agreements.mock';
import { createMemoryHistory } from 'history';
import { IUpdateAgreementFormProps } from '../common/UpdateAgreementForm';
import { RenderOptions, act, render, waitForEffects } from '@/utils/test-utils';
import UpdateAgreementContainer, {
  IUpdateAcquisitionAgreementContainerProps,
} from './UpdateAgreementContainer';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { Claims } from '@/constants/claims';
import { AgreementFormModel } from '../models/AgreementFormModel';

const history = createMemoryHistory();
const mockAgreementApi = mockAgreementResponseApi(1);

const mockPutAgreementApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockGetAgreementApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn().mockResolvedValue(mockAgreementApi),
  loading: false,
};

const onSuccess = vi.fn();

vi.mock('@/hooks/repositories/useAgreementProvider', () => ({
  useAgreementProvider: () => {
    return {
      getAcquisitionAgreementById: mockGetAgreementApi,
      updateAcquisitionAgreement: mockPutAgreementApi,
    };
  },
}));

let viewProps: IUpdateAgreementFormProps | undefined;
const TestView: React.FC<IUpdateAgreementFormProps> = props => {
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
      <UpdateAgreementContainer
        fileId={1}
        agreementId={10}
        fileType="acquisition"
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
    vi.resetAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = await setup({ props: { fileId: 1, agreementId: 10 } });

    expect(getByText(/Content Rendered/)).toBeVisible();
    expect(mockGetAgreementApi.execute).toHaveBeenCalledWith(1, 10);
  });

  it('Loads props with the initial values', async () => {
    mockGetAgreementApi.execute.mockResolvedValue(mockAgreementApi);
    await setup({ props: { fileId: 1, agreementId: 10 } });
    await waitForEffects();

    expect(mockGetAgreementApi.execute).toHaveBeenCalledWith(1, 10);
    const formModel = AgreementFormModel.fromApi(mockAgreementApi);

    expect(viewProps?.initialValues).toStrictEqual(formModel);
  });

  it('makes request to create a new Agreement and returns the response', async () => {
    mockGetAgreementApi.execute.mockResolvedValue(mockAgreementApi);
    mockPutAgreementApi.execute.mockReturnValue(mockAgreementApi);

    await setup({ props: { fileId: 1, agreementId: 10 } });

    let agreementFormModel = AgreementFormModel.fromApi(mockAgreementApi);
    agreementFormModel.agreementStatusTypeCode = 'FINAL';

    await act(async () => {
      return viewProps?.onSubmit(agreementFormModel, { setSubmitting: vi.fn() } as any);
    });

    expect(mockPutAgreementApi.execute).toHaveBeenCalledWith(
      1,
      10,
      expect.objectContaining({
        agreementId: 10,
        fileId: 1,
        agreementType: { description: null, displayOrder: null, id: 'H0074', isDisabled: false },
        agreementDate: null,
        agreementStatusType: {
          id: 'FINAL',
          description: null,
          displayOrder: null,
          isDisabled: false,
        },
        cancellationNote: null,
        commencementDate: null,
        completionDate: null,
        depositAmount: null,
        expiryDateTime: null,
        inspectionDate: null,
        isDraft: null,
        legalSurveyPlanNum: null,
        noLaterThanDays: 0,
        offerDate: null,
        possessionDate: null,
        purchasePrice: 0,
        rowVersion: 1,
        signedDate: null,
        terminationDate: null,
      }),
    );
    expect(onSuccess).toHaveBeenCalled();
    expect(history.location.pathname).toBe('/');
  });

  it('navigates back to Agreement when form is cancelled', async () => {
    await setup({ props: { fileId: 1, agreementId: 10 } });
    await act(async () => {
      viewProps?.onCancel();
    });

    expect(history.location.pathname).toBe('/');
    expect(mockPutAgreementApi.execute).not.toHaveBeenCalled();
  });
});
