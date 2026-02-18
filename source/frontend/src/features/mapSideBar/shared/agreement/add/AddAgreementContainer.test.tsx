import { createMemoryHistory } from 'history';
import { RenderOptions, act, render } from '@/utils/test-utils';
import AddAgreementContainer, {
  IAddAcquisitionAgreementContainerProps,
} from './AddAgreementContainer';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { IUpdateAgreementFormProps } from '../common/UpdateAgreementForm';
import { Claims } from '@/constants/claims';
import { mockAgreementResponseApi } from '@/mocks/agreements.mock';
import { AgreementFormModel } from '../models/AgreementFormModel';

const history = createMemoryHistory();
const mockPostApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};
const onSuccess = vi.fn();

let viewProps: IUpdateAgreementFormProps | undefined;
const TestView: React.FC<IUpdateAgreementFormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

vi.mock('@/hooks/repositories/useAgreementProvider', () => ({
  useAgreementProvider: () => {
    return {
      addAcquisitionAgreement: mockPostApi,
      addDispositionAgreement: mockPostApi,
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
      <AddAgreementContainer
        acquisitionFileId={1}
        View={TestView}
        fileType="acquisition"
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
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('Loads props with the initial values', async () => {
    await setup({ props: { acquisitionFileId: 1 } });

    expect(viewProps?.initialValues?.agreementId).toBe(0);
    expect(viewProps?.initialValues?.fileId).toBe(1);
    expect(viewProps?.initialValues?.agreementStatusTypeCode).toBe('DRAFT');
    expect(viewProps?.initialValues?.agreementTypeCode).toBe(null);
    expect(viewProps?.initialValues?.agreementDate).toBe(null);
    expect(viewProps?.initialValues?.cancellationNote).toBe(null);
    expect(viewProps?.initialValues?.rowVersion).toBe(null);
  });

  it('makes request to create a new Agreement and returns the response', async () => {
    await setup({ props: { acquisitionFileId: 1 } });
    const agreementMock = mockAgreementResponseApi(1);
    mockPostApi.execute.mockReturnValue(agreementMock);

    const agreementFormModel = new AgreementFormModel(1);
    agreementFormModel.agreementTypeCode = 'H0074';
    await act(async () => {
      return viewProps?.onSubmit(agreementFormModel, { setSubmitting: vi.fn() } as any);
    });

    expect(mockPostApi.execute).toHaveBeenCalledWith(
      1,
      expect.objectContaining({
        agreementId: 0,
        fileId: 1,
        agreementType: { description: null, displayOrder: null, id: 'H0074', isDisabled: false },
        agreementDate: null,
        agreementStatusType: {
          id: 'DRAFT',
          description: null,
          displayOrder: null,
          isDisabled: false,
        },
        cancellationNote: null,
        commencementDate: null,
        completionDate: null,
        depositAmount: 0,
        expiryDateTime: null,
        inspectionDate: null,
        isDraft: null,
        legalSurveyPlanNum: null,
        noLaterThanDays: 0,
        offerDate: null,
        possessionDate: null,
        purchasePrice: 0,
        rowVersion: null,
        signedDate: null,
        terminationDate: null,
      }),
    );

    expect(onSuccess).toHaveBeenCalled();
    expect(history.location.pathname).toBe('/');
  });

  it('navigates back to Agreements when form is cancelled', async () => {
    await setup();
    await act(async () => {
      viewProps?.onCancel();
    });

    expect(history.location.pathname).toBe('/');
    expect(mockPostApi.execute).not.toHaveBeenCalled();
  });
});
