import { createRef } from 'react';

import { useAgreementProvider } from '@/hooks/repositories/useAgreementProvider';
import { mockAgreementsResponse } from '@/mocks/agreements.mock';
import { mockLookups } from '@/mocks/index.mock';
import { Api_Agreement } from '@/models/api/Agreement';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { AgreementsFormModel, SingleAgreementFormModel } from './models';
import { UpdateAgreementsContainer } from './UpdateAgreementsContainer';
import { IUpdateAgreementsFormProps } from './UpdateAgreementsForm';

// mock API service calls
jest.mock('@/hooks/repositories/useAgreementProvider');

type Provider = typeof useAgreementProvider;
const mockUpdateAgreements = jest.fn();
const mockGetAgreements = jest.fn();
const testAcquisitionFileId = 100;

(useAgreementProvider as jest.MockedFunction<Provider>).mockReturnValue({
  updateAcquisitionAgreements: {
    error: undefined,
    response: undefined,
    execute: mockUpdateAgreements,
    loading: false,
  },
  getAcquisitionAgreements: {
    error: undefined,
    response: undefined,
    execute: mockGetAgreements,
    loading: false,
  },
} as unknown as ReturnType<Provider>);

let viewProps: IUpdateAgreementsFormProps | undefined;

const TestView: React.FC<IUpdateAgreementsFormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('UpdateAgreementsContainer component', () => {
  const onSuccess = jest.fn();

  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <UpdateAgreementsContainer
        formikRef={createRef()}
        acquisitionFileId={testAcquisitionFileId}
        onSuccess={onSuccess}
        View={TestView}
      />,
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

  beforeEach(() => {
    viewProps = undefined;
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('makes request to update the agreements and returns the response', async () => {
    setup();
    mockUpdateAgreements.mockResolvedValue(mockAgreementsResponse());
    let updatedAgreements: Api_Agreement[] | undefined;

    const testAgreementForm = new AgreementsFormModel(testAcquisitionFileId);
    const testAgreementItem = new SingleAgreementFormModel();
    testAgreementItem.agreementId = 10;
    testAgreementItem.depositAmount = '50';

    testAgreementForm.agreements = [testAgreementItem];
    await act(async () => {
      updatedAgreements = await viewProps?.onSave(testAgreementForm);
    });

    expect(mockUpdateAgreements).toHaveBeenCalledWith(
      testAcquisitionFileId,
      testAgreementForm.toApi(),
    );
    expect(updatedAgreements).toStrictEqual([...mockAgreementsResponse()]);
  });

  it('calls onSuccess when the agreements are saved successfully', async () => {
    setup();

    await act(async () => {
      viewProps?.onSave(new AgreementsFormModel(1));
    });

    expect(mockUpdateAgreements).toHaveBeenCalled();
    expect(onSuccess).toHaveBeenCalled();
  });

  it('does not call onSucess if the returned value is invalid', async () => {
    setup();
    mockUpdateAgreements.mockResolvedValue(undefined);

    await act(async () => {
      await viewProps?.onSave(new AgreementsFormModel(1));
    });

    expect(mockUpdateAgreements).toHaveBeenCalled();
    expect(onSuccess).not.toHaveBeenCalled();
  });
});
