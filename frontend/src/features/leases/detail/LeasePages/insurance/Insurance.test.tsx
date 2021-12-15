import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, IFormLease, IInsurance } from 'interfaces';
import { noop } from 'lodash';
import { mockOrganization, mockUser } from 'mocks/filterDataMock';
import { render, RenderOptions, RenderResult } from 'utils/test-utils';

import Insurance from './Insurance';

const mockInsurance: IInsurance = {
  id: 123459,
  insuranceType: { description: 'Test Insurance Type', id: '2135689', isDisabled: false },
  otherInsuranceType: '',
  coverageDescription: '',
  coverageLimit: 777,
  expiryDate: '2022-01-01',
  isInsuranceInPlace: true,
};

const history = createMemoryHistory();

describe('Lease Insurance', () => {
  const setup = (renderOptions: RenderOptions & { lease?: IFormLease } = {}): RenderResult => {
    // render component under test
    const result = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? defaultFormLease}>
        <Insurance />
      </Formik>,
      {
        ...renderOptions,
        history,
      },
    );
    return result;
  };

  it('renders as expected', () => {
    const result = setup({
      lease: {
        ...defaultFormLease,
        insurances: [mockInsurance],
        persons: [mockUser],
        organizations: [mockOrganization],
      },
    });
    expect(result.asFragment()).toMatchSnapshot();
  });

  it('Insurance count is correct', () => {
    const testInsurance: IInsurance = { ...mockInsurance };
    testInsurance.insuranceType.description = 'Another Type';

    const result = setup({
      lease: {
        ...defaultFormLease,
        insurances: [mockInsurance, testInsurance],
        persons: [mockUser],
        organizations: [mockOrganization],
      },
    });
    const titles = result.getAllByTestId('insurance-title');
    expect(titles.length).toBe(2);
  });

  it('Insurance title is set', () => {
    const result = setup({
      lease: {
        ...defaultFormLease,
        insurances: [mockInsurance],
        persons: [mockUser],
        organizations: [mockOrganization],
      },
    });
    const title = result.getByTestId('insurance-title');
    expect(title.textContent).toBe(mockInsurance.insuranceType.description);
  });
});
