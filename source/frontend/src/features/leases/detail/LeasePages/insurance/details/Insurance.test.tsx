import { useKeycloak } from '@react-keycloak/web';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { IInsurance, TypeCodeUtils } from 'interfaces';
import { noop } from 'lodash';
import { ILookupCode, lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, RenderResult } from 'utils/test-utils';

import InsuranceDetailsView from './Insurance';

jest.mock('@react-keycloak/web');
(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    subject: 'test',
    authenticated: true,
    userInfo: {
      roles: [],
    },
  },
});

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: [] },
};

const mockInsuranceTypeHome: ILookupCode = {
  id: 'HOME',
  name: 'Home Insurance',
  type: 'PimsInsuranceType',
  isDisabled: false,
  displayOrder: 1,
};
const mockInsuranceTypeCar: ILookupCode = {
  id: 'CAR',
  name: 'Car insurance',
  type: 'PimsInsuranceType',
  isDisabled: false,
  displayOrder: 2,
};

const mockInsurance: IInsurance = {
  id: 123459,
  insuranceType: TypeCodeUtils.createFromLookup(mockInsuranceTypeHome),
  otherInsuranceType: '',
  coverageDescription: '',
  coverageLimit: 777,
  expiryDate: '2022-01-01',
  isInsuranceInPlace: true,
  rowVersion: 0,
};

const history = createMemoryHistory();

describe('Lease Insurance', () => {
  const setup = (
    renderOptions: RenderOptions & {
      insuranceList: IInsurance[];
      insuranceTypes: ILookupCode[];
    } = {
      store: storeState,
      insuranceList: [],
      insuranceTypes: [],
    },
  ): RenderResult => {
    // render component under test
    const result = render(
      <Formik
        onSubmit={noop}
        initialValues={(renderOptions.insuranceList ?? [], renderOptions.insuranceTypes ?? [])}
      >
        <InsuranceDetailsView
          insuranceList={renderOptions.insuranceList}
          insuranceTypes={renderOptions.insuranceTypes}
        />
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
      insuranceList: [mockInsurance],
      insuranceTypes: [],
    });
    expect(result.asFragment()).toMatchSnapshot();
  });

  it('Insurance count is correct', () => {
    const testInsurance: IInsurance = { ...mockInsurance };
    testInsurance.insuranceType = TypeCodeUtils.createFromLookup(mockInsuranceTypeCar);

    const result = setup({
      insuranceList: [mockInsurance],
      insuranceTypes: [mockInsuranceTypeHome, mockInsuranceTypeCar],
    });
    const titles = result.getAllByTestId('insurance-section');
    expect(titles.length).toBe(2);
  });
});
