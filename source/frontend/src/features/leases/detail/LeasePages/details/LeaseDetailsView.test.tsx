import { Claims } from '@/constants';
import { getEmptyLease } from '@/models/defaultInitializers';
import { render, RenderOptions, screen } from '@/utils/test-utils';

import { ApiGen_CodeTypes_LeaseLicenceTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseLicenceTypes';
import { ApiGen_CodeTypes_LeasePaymentReceivableTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePaymentReceivableTypes';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { ApiGen_Concepts_Product } from '@/models/api/generated/ApiGen_Concepts_Product';
import { ApiGen_Concepts_Project } from '@/models/api/generated/ApiGen_Concepts_Project';
import { toTypeCode } from '@/utils/formUtils';

import LeaseDetailView, { ILeaseDetailViewProps } from './LeaseDetailView';

describe('LeaseDetailView component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<ILeaseDetailViewProps> } = {},
  ) => {
    const utils = render(
      <LeaseDetailView lease={renderOptions?.props?.lease ?? getEmptyLease()} />,
      {
        ...renderOptions,
        claims: renderOptions?.claims ?? [Claims.LEASE_VIEW],
      },
    );

    return { ...utils };
  };

  it('renders minimally as expected', () => {
    const { asFragment } = setup({
      props: {
        lease: {
          ...getEmptyLease(),
          type: toTypeCode(ApiGen_CodeTypes_LeaseLicenceTypes.LOOBCTFA),
          fileStatusTypeCode: {
            ...toTypeCode(ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE),
            description: 'Active',
          },
          paymentReceivableType: {
            ...toTypeCode(ApiGen_CodeTypes_LeasePaymentReceivableTypes.RCVBL),
            description: 'Receivable',
          },
          project: {
            code: 'TESTPROJ',
            description: 'Test Project',
          } as ApiGen_Concepts_Project,
        },
      },
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('shows lease project and product if available', async () => {
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
          project: {
            code: 'PROJ',
            description: 'Test Project',
          } as ApiGen_Concepts_Project,
          product: {
            code: 'PROD',
            description: 'Test Product',
          } as ApiGen_Concepts_Product,
        },
      },
    });

    expect(await screen.findByText('PROJ - Test Project')).toBeVisible();
    expect(await screen.findByText('PROD Test Product')).toBeVisible();
  });

  it('shows cancellation reason for discarded leases', async () => {
    setup({
      props: {
        lease: {
          ...getEmptyLease(),
          type: toTypeCode(ApiGen_CodeTypes_LeaseLicenceTypes.OTHER),
          fileStatusTypeCode: {
            ...toTypeCode(ApiGen_CodeTypes_LeaseStatusTypes.DISCARD),
            description: 'Cancelled',
          },
          paymentReceivableType: {
            ...toTypeCode(ApiGen_CodeTypes_LeasePaymentReceivableTypes.RCVBL),
            description: 'Receivable',
          },
          cancellationReason: 'test cancellation',
        },
      },
    });

    expect(await screen.findByText('test cancellation')).toBeVisible();
  });

  it('shows termination date and reason for terminated leases', async () => {
    setup({
      props: {
        lease: {
          ...getEmptyLease(),
          type: toTypeCode(ApiGen_CodeTypes_LeaseLicenceTypes.OTHER),
          fileStatusTypeCode: {
            ...toTypeCode(ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED),
            description: 'Terminated',
          },
          paymentReceivableType: {
            ...toTypeCode(ApiGen_CodeTypes_LeasePaymentReceivableTypes.RCVBL),
            description: 'Receivable',
          },
          terminationDate: '2024-03-15',
          terminationReason: 'test termination',
        },
      },
    });

    expect(await screen.findByText('Mar 15, 2024')).toBeVisible();
    expect(await screen.findByText('test termination')).toBeVisible();
  });
});
