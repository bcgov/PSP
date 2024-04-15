import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { TypeCodeUtils } from '@/interfaces/ITypeCode';
import { getMockInsurance } from '@/mocks/insurance.mock';
import { ApiGen_Concepts_Insurance } from '@/models/api/generated/ApiGen_Concepts_Insurance';
import { ILookupCode, lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, RenderResult } from '@/utils/test-utils';

import { mockInsuranceTypeCar, mockInsuranceTypeHome } from '../edit/EditInsuranceContainer.test';
import InsuranceDetailsView from './Insurance';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: [] },
};

const history = createMemoryHistory();

describe('Lease Insurance', () => {
  const setup = (
    renderOptions: RenderOptions & {
      insuranceList: ApiGen_Concepts_Insurance[];
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
      insuranceList: [getMockInsurance()],
      insuranceTypes: [],
    });
    expect(result.asFragment()).toMatchSnapshot();
  });

  it('Insurance count is correct', () => {
    const testInsurance: ApiGen_Concepts_Insurance = { ...getMockInsurance() };
    testInsurance.insuranceType = TypeCodeUtils.createFromLookup(mockInsuranceTypeCar);

    const result = setup({
      insuranceList: [getMockInsurance()],
      insuranceTypes: [mockInsuranceTypeHome, mockInsuranceTypeCar],
    });
    const titles = result.getAllByTestId('insurance-section');
    expect(titles.length).toBe(2);
  });
});
