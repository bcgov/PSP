import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { getDefaultFormLease, LeaseFormModel } from '@/features/leases/models';
import { render, RenderOptions } from '@/utils/test-utils';

import { defaultFormLeasePeriod } from '../payment/models';
import { DetailPeriods, IDetailPeriodsProps } from './DetailPeriods';

const history = createMemoryHistory();

describe('DetailPeriodInformation component', () => {
  const setup = (
    renderOptions: RenderOptions & IDetailPeriodsProps & { lease?: LeaseFormModel } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? new LeaseFormModel()}>
        <DetailPeriods nameSpace={renderOptions.nameSpace} />
      </Formik>,
      {
        ...renderOptions,
        history,
      },
    );

    return {
      component,
    };
  };
  it('renders with the expected headers', () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        ...getDefaultFormLease(),
        amount: 1000,
        renewalCount: 31,
      },
    });
    expect(getByDisplayValue('$1,000.00')).toBeVisible();
    expect(getByDisplayValue('31')).toBeVisible();
  });
  it('renders with the expected columns', () => {
    const {
      component: { getByText },
    } = setup({
      lease: {
        ...getDefaultFormLease(),
        periods: [
          {
            ...defaultFormLeasePeriod,
            id: 1,
            leaseId: 1,
            startDate: '2020-01-01T18:00',
            expiryDate: '2022-01-01T18:00',
            renewalDate: '2021-01-01T18:00',
            statusTypeCode: {
              id: 'EX',
              description: 'exercised',
              isDisabled: false,
              displayOrder: null,
            },
          },
        ],
      },
    });
    expect(getByText('initial period')).toBeVisible();
    expect(getByText('Jan 1, 2020')).toBeVisible();
    expect(getByText('Jan 1, 2021')).toBeVisible();
    expect(getByText('Jan 1, 2022')).toBeVisible();
    expect(getByText('exercised')).toBeVisible();
  });

  it('renders normally when provided empty lease date information', () => {
    const {
      component: { getByText },
    } = setup({
      lease: {
        ...getDefaultFormLease(),
        startDate: '',
        expiryDate: '',
        renewalDate: '',
        periods: [
          {
            ...defaultFormLeasePeriod,
            id: 1,
            leaseId: 1,
            startDate: '',
            expiryDate: '',
            renewalDate: '',
            statusTypeCode: {
              id: 'EX',
              description: 'exercised',
              isDisabled: false,
              displayOrder: null,
            },
          },
        ],
      },
    });
    expect(getByText('initial period')).toBeVisible();
    expect(getByText('exercised')).toBeVisible();
  });
});
