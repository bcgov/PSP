import { getEmptyLease } from '@/models/defaultInitializers';
import LeaseGenerateContainer, { ILeaseGenerateContainerProps } from './LeaseGenerateFormContainer';
import { act, render, RenderOptions, screen, userEvent } from '@/utils/test-utils';
import { Claims } from '@/constants';
import { ApiGen_CodeTypes_LeaseLicenceTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseLicenceTypes';
import { toTypeCode } from '@/utils/formUtils';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { ApiGen_CodeTypes_LeasePaymentReceivableTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePaymentReceivableTypes';
import GenerateFormView from '../acquisition/common/GenerateForm/GenerateFormView';
import { useGenerateLicenceOfOccupation } from '../acquisition/common/GenerateForm/hooks/useGenerateLicenceOfOccupation';

const onGenerate = vi.fn();
vi.mock('../acquisition/common/GenerateForm/hooks/useGenerateLicenceOfOccupation');
vi.mocked(useGenerateLicenceOfOccupation).mockReturnValue(onGenerate);

describe('LeaseGenerateFormContainer component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<ILeaseGenerateContainerProps> } = {},
  ) => {
    const utils = render(
      <LeaseGenerateContainer
        lease={renderOptions?.props?.lease ?? getEmptyLease()}
        View={GenerateFormView}
      />,
      {
        ...renderOptions,
        claims: renderOptions?.claims ?? [Claims.LEASE_VIEW, Claims.FORM_ADD],
      },
    );

    return { ...utils };
  };

  it.each([
    [ApiGen_CodeTypes_LeaseLicenceTypes.LOOBCTFA, 'Generate H-1005(a)'],
    [ApiGen_CodeTypes_LeaseLicenceTypes.LIPPUBHWY, 'Generate H-1005'],
  ])(
    'does not render generation button if missing permissions - %s',
    async (leaseTypeCode: string, buttonText: string) => {
      setup({
        props: {
          lease: {
            ...getEmptyLease(),
            type: toTypeCode(leaseTypeCode),
            fileStatusTypeCode: {
              ...toTypeCode(ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE),
              description: 'Active',
            },
            paymentReceivableType: {
              ...toTypeCode(ApiGen_CodeTypes_LeasePaymentReceivableTypes.RCVBL),
              description: 'Receivable',
            },
          },
        },
        claims: [],
      });

      expect(screen.queryByText(buttonText)).toBeNull();
    },
  );

  it('does not render generation button if lease is not of expected type', async () => {
    setup({
      props: {
        lease: {
          ...getEmptyLease(),
          type: toTypeCode(ApiGen_CodeTypes_LeaseLicenceTypes.OTHER),
          fileStatusTypeCode: {
            ...toTypeCode(ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE),
            description: 'Active',
          },
          paymentReceivableType: {
            ...toTypeCode(ApiGen_CodeTypes_LeasePaymentReceivableTypes.RCVBL),
            description: 'Receivable',
          },
        },
      },
    });

    expect(screen.queryByText(/Generate H-1005/i)).toBeNull();
  });

  it.each([
    [ApiGen_CodeTypes_LeaseLicenceTypes.LOOBCTFA, 'Generate H-1005(a)'],
    [ApiGen_CodeTypes_LeaseLicenceTypes.LIPPUBHWY, 'Generate H-1005'],
  ])(
    'only renders generation button for specific lease types - %s',
    async (leaseTypeCode: string, buttonText: string) => {
      setup({
        props: {
          lease: {
            ...getEmptyLease(),
            type: toTypeCode(leaseTypeCode),
            fileStatusTypeCode: {
              ...toTypeCode(ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE),
              description: 'Active',
            },
            paymentReceivableType: {
              ...toTypeCode(ApiGen_CodeTypes_LeasePaymentReceivableTypes.RCVBL),
              description: 'Receivable',
            },
          },
        },
      });

      expect(await screen.findByText(buttonText)).toBeInTheDocument();
    },
  );

  it.each([
    [ApiGen_CodeTypes_LeaseLicenceTypes.LOOBCTFA, 'Generate H-1005(a)'],
    [ApiGen_CodeTypes_LeaseLicenceTypes.LIPPUBHWY, 'Generate H-1005'],
  ])(
    'calls onGenerate when generation button is clicked - %s',
    async (leaseTypeCode: string, buttonTitle: string) => {
      setup({
        props: {
          lease: {
            ...getEmptyLease(),
            type: toTypeCode(leaseTypeCode),
            fileStatusTypeCode: {
              ...toTypeCode(ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE),
              description: 'Active',
            },
            paymentReceivableType: {
              ...toTypeCode(ApiGen_CodeTypes_LeasePaymentReceivableTypes.RCVBL),
              description: 'Receivable',
            },
          },
        },
      });

      const generateButton = await screen.findByText(buttonTitle);
      expect(generateButton).toBeInTheDocument();

      await act(async () => userEvent.click(generateButton));
      expect(onGenerate).toHaveBeenCalled();
    },
  );
});
