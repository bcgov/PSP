import { mockAgreementResponseApi } from '@/mocks/agreements.mock';
import { createMemoryHistory } from 'history';
import { IUpdateAcquisitionAgreementViewProps } from '../common/UpdateAcquisitionAgreementForm';
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
  execute: vi.fn(),
  loading: false,
};

const mockGetAgreementApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn().mockResolvedValue(mockAcquisitionAgreementApi),
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
    vi.resetAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = await setup({ props: { acquisitionFileId: 1, agreementId: 10 } });

    expect(getByText(/Content Rendered/)).toBeVisible();
    expect(mockGetAgreementApi.execute).toHaveBeenCalledWith(1, 10);
  });

  it('Loads props with the initial values', async () => {
    mockGetAgreementApi.execute.mockResolvedValue(mockAcquisitionAgreementApi);
    await setup({ props: { acquisitionFileId: 1, agreementId: 10 } });
    await waitForEffects();

    expect(mockGetAgreementApi.execute).toHaveBeenCalledWith(1, 10);
    const formModel = AcquisitionAgreementFormModel.fromApi(mockAcquisitionAgreementApi);

    expect(viewProps?.initialValues).toStrictEqual(formModel);
  });

  it('makes request to create a new Agreement and returns the response', async () => {
    mockGetAgreementApi.execute.mockResolvedValue(mockAcquisitionAgreementApi);
    mockPutAgreementApi.execute.mockReturnValue(mockAcquisitionAgreementApi);

    await setup({ props: { acquisitionFileId: 1, agreementId: 10 } });

    let agreementFormModel = AcquisitionAgreementFormModel.fromApi(mockAcquisitionAgreementApi);
    agreementFormModel.agreementStatusTypeCode = 'FINAL';

    await act(async () => {
      return viewProps?.onSubmit(agreementFormModel, { setSubmitting: vi.fn() } as any);
    });

    expect(mockPutAgreementApi.execute).toHaveBeenCalledWith(
      1,
      10,
      expect.objectContaining({
        agreementId: 10,
        acquisitionFileId: 1,
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
    await setup({ props: { acquisitionFileId: 1, agreementId: 10 } });
    await act(async () => {
      viewProps?.onCancel();
    });

    expect(history.location.pathname).toBe('/');
    expect(mockPutAgreementApi.execute).not.toHaveBeenCalled();
  });
});
