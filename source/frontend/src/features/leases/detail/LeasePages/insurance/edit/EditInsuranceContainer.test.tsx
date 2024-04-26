import userEvent from '@testing-library/user-event';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { TypeCodeUtils } from '@/interfaces/ITypeCode';
import { ApiGen_Concepts_Insurance } from '@/models/api/generated/ApiGen_Concepts_Insurance';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { ILookupCode } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, RenderResult } from '@/utils/test-utils';

import InsuranceEditContainer, { InsuranceEditContainerProps } from './EditInsuranceContainer';
import { createRef } from 'react';

export const mockInsuranceTypeHome: ILookupCode = {
  id: 'HOME',
  name: 'Home Insurance',
  type: 'PimsInsuranceType',
  isDisabled: false,
  displayOrder: 1,
};
export const mockInsuranceTypeCar: ILookupCode = {
  id: 'CAR',
  name: 'Car insurance',
  type: 'PimsInsuranceType',
  isDisabled: false,
  displayOrder: 2,
};

const mockInsuranceHome: ApiGen_Concepts_Insurance = {
  id: 123459,
  leaseId: 1,
  insuranceType: TypeCodeUtils.createFromLookup(mockInsuranceTypeHome),
  otherInsuranceType: '',
  coverageDescription: '',
  coverageLimit: 777,
  expiryDate: '2022-01-01',
  isInsuranceInPlace: true,
  ...getEmptyBaseAudit(),
};

const defaultProps: InsuranceEditContainerProps = {
  leaseId: 1,
  insuranceList: [mockInsuranceHome],
  insuranceTypes: [mockInsuranceTypeHome, mockInsuranceTypeCar],
  onSave: () => Promise.resolve(),
  formikRef: createRef(),
};

const history = createMemoryHistory();

describe('Edit Lease Insurance', () => {
  const setup = (
    renderOptions: RenderOptions & InsuranceEditContainerProps = {
      leaseId: defaultProps.leaseId,
      insuranceList: defaultProps.insuranceList,
      insuranceTypes: defaultProps.insuranceTypes,
      onSave: defaultProps.onSave,
      formikRef: defaultProps.formikRef,
    },
  ): RenderResult => {
    // render component under test
    const result = render(
      <Formik onSubmit={noop} initialValues={{}}>
        <InsuranceEditContainer
          insuranceList={renderOptions.insuranceList}
          insuranceTypes={renderOptions.insuranceTypes}
          leaseId={renderOptions.leaseId}
          onSave={renderOptions.onSave}
          formikRef={renderOptions.formikRef}
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
    const result = setup();
    expect(result.asFragment()).toMatchSnapshot();
  });

  it('Shows correct comboboxes', () => {
    const result = setup({
      ...defaultProps,
      insuranceList: [mockInsuranceHome],
      insuranceTypes: [mockInsuranceTypeHome, mockInsuranceTypeCar],
    });
    const checkboxes = result.getAllByTestId('insurance-checkbox');
    expect(checkboxes.length).toBe(2);
    expect((checkboxes[0] as HTMLInputElement).value).toBe(mockInsuranceTypeHome.id);
    expect((checkboxes[1] as HTMLInputElement).value).toBe(mockInsuranceTypeCar.id);

    expect(checkboxes[0] as HTMLInputElement).toBeChecked();
    expect(checkboxes[1] as HTMLInputElement).not.toBeChecked();
  });

  it('Updates form lists when clicked', async () => {
    const testInsuranceCar: ApiGen_Concepts_Insurance = {
      ...mockInsuranceHome,
      coverageLimit: 888,
      coverageDescription: 'test description for a test insurance car',
    };
    testInsuranceCar.insuranceType = TypeCodeUtils.createFromLookup(mockInsuranceTypeCar);

    const result = setup({
      ...defaultProps,
      insuranceList: [mockInsuranceHome, testInsuranceCar],
      insuranceTypes: [mockInsuranceTypeHome, mockInsuranceTypeCar],
    });

    // Verify initial state
    const forms = result.getAllByTestId('insurance-form');
    expect(forms.length).toBe(2);

    const homeLimit = result.container.querySelector(`input[name="insurances.0.coverageLimit"]`);
    const carLimit = result.container.querySelector(`input[name="insurances.1.coverageLimit"]`);
    expect(homeLimit).toHaveValue(`$${mockInsuranceHome.coverageLimit?.toFixed(2)}`);
    expect(carLimit).toHaveValue(`$${testInsuranceCar.coverageLimit?.toFixed(2)}`);

    const formDescriptions = result.getAllByTestId('insurance-form-description');
    expect(formDescriptions[0] as HTMLInputElement).toHaveValue(
      mockInsuranceHome.coverageDescription,
    );
    expect(formDescriptions[1] as HTMLInputElement).toHaveValue(
      testInsuranceCar.coverageDescription,
    );

    // Update view
    const checkboxes = result.getAllByTestId('insurance-checkbox');

    await act(async () => userEvent.click(checkboxes[0]));

    // Verify update
    const updatedForms = result.getAllByTestId('insurance-form');
    expect(updatedForms.length).toBe(1);

    const formTitle = result.getByTestId('insurance-form-title');
    expect(formTitle.textContent).toBe(mockInsuranceTypeCar.name);
  });
});
