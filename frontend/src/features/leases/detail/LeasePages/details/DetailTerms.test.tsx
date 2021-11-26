import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { render, RenderOptions } from 'utils/test-utils';

import { DetailTerms, IDetailTermsProps } from './DetailTerms';

const history = createMemoryHistory();

describe('DetailTermInformation component', () => {
  const setup = (
    renderOptions: RenderOptions & IDetailTermsProps & { lease?: IFormLease } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? defaultFormLease}>
        <DetailTerms nameSpace={renderOptions.nameSpace} />
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
        ...defaultFormLease,
        amount: 1000,
        renewalCount: 31,
        paymentFrequencyType: { id: 'ANNUAL', description: 'Annual', isDisabled: false },
      },
    });
    expect(getByDisplayValue('1000')).toBeVisible();
    expect(getByDisplayValue('31')).toBeVisible();
    expect(getByDisplayValue('Annual')).toBeVisible();
  });
  it('renders with the expected columns', () => {
    const {
      component: { getByText },
    } = setup({
      lease: {
        ...defaultFormLease,
        terms: [
          {
            id: 1,
            leaseId: 1,
            startDate: '2020-01-01',
            expiryDate: '2022-01-01',
            renewalDate: '2021-01-01',
            statusTypeCode: { id: 'EX', description: 'exercised', isDisabled: false },
          },
        ],
      },
    });
    expect(getByText('initial term')).toBeVisible();
    expect(getByText('Jan 1, 2020')).toBeVisible();
    expect(getByText('Jan 1, 2020')).toBeVisible();
    expect(getByText('Jan 1, 2021')).toBeVisible();
    expect(getByText('exercised')).toBeVisible();
  });

  it('renders normally when provided empty lease date information', () => {
    const {
      component: { getByText },
    } = setup({
      lease: {
        ...defaultFormLease,
        startDate: '',
        expiryDate: undefined,
        renewalDate: undefined,
        terms: [
          {
            id: 1,
            leaseId: 1,
            startDate: '',
            expiryDate: '',
            renewalDate: '',
            statusTypeCode: { id: 'EX', description: 'exercised', isDisabled: false },
          },
        ],
      },
    });
    expect(getByText('initial term')).toBeVisible();
    expect(getByText('exercised')).toBeVisible();
  });
});
