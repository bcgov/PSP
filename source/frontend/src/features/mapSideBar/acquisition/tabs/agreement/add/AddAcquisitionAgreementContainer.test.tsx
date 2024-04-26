import { createMemoryHistory } from 'history';
import { RenderOptions, act, render } from '@/utils/test-utils';
import AddAcquisitionAgreementContainer, {
  IAddAcquisitionAgreementContainerProps,
} from './AddAcquisitionAgreementContainer';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { IUpdateAcquisitionAgreementViewProps } from '../common/UpdateAcquisitionAgreementForm';
import { Claims } from '@/constants/claims';
import { mockAgreementResponseApi } from '@/mocks/agreements.mock';
import { ApiGen_Concepts_Agreement } from '@/models/api/generated/ApiGen_Concepts_Agreement';
import { AcquisitionAgreementFormModel } from '../models/AcquisitionAgreementFormModel';
import { FormikHelpers } from 'formik';

const history = createMemoryHistory();
const mockPostApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};
const onSuccess = vi.fn();

let viewProps: IUpdateAcquisitionAgreementViewProps | undefined;
const TestView: React.FC<IUpdateAcquisitionAgreementViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

vi.mock('@/hooks/repositories/useAgreementProvider', () => ({
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
      <AddAcquisitionAgreementContainer
        acquisitionFileId={1}
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
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('Loads props with the initial values', async () => {
    await setup({ props: { acquisitionFileId: 1 } });

    expect(viewProps?.initialValues?.agreementId).toBe(0);
    expect(viewProps?.initialValues?.acquisitionFileId).toBe(1);
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

    const agreementFormModel = new AcquisitionAgreementFormModel(1);
    agreementFormModel.agreementTypeCode = 'H0074';
    await act(async () => {
      return viewProps?.onSubmit(agreementFormModel, { setSubmitting: vi.fn() } as any);
    });

    expect(mockPostApi.execute).toHaveBeenCalledWith(
      1,
      expect.objectContaining({
        agreementId: 0,
        acquisitionFileId: 1,
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
