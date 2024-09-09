import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { getEmptyBaseAudit, getEmptyLease } from '@/models/defaultInitializers';
import { render, RenderOptions, screen } from '@/utils/test-utils';

import { LeaseRenewalsView } from './LeaseRenewalsView';

const history = createMemoryHistory();

describe('LeaseRenewalsView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { lease?: ApiGen_Concepts_Lease } = {}) => {
    const apiLease = renderOptions.lease ?? getEmptyLease();
    const utils = render(
      <Formik onSubmit={noop} initialValues={apiLease}>
        <LeaseRenewalsView renewals={apiLease?.renewals ?? []} />
      </Formik>,
      {
        ...renderOptions,
        history,
      },
    );

    return { ...utils };
  };

  it('renders the lease renewals', () => {
    setup({
      lease: {
        ...getEmptyLease(),
        renewals: [
          {
            id: 1,
            leaseId: 1,
            isExercised: true,
            commencementDt: '2024-08-26',
            expiryDt: null,
            lease: null,
            renewalNote: 'renewal notes go here',
            ...getEmptyBaseAudit(),
          },
        ],
      },
    });
    expect(screen.getByText('Aug 26, 2024')).toBeVisible();
    expect(screen.getByText('renewal notes go here')).toBeVisible();
  });

  it('displays a custom message when there are no renewals', () => {
    setup({
      lease: {
        ...getEmptyLease(),
        renewals: [],
      },
    });
    expect(screen.getByText('No Renewal Information')).toBeVisible();
  });
});
