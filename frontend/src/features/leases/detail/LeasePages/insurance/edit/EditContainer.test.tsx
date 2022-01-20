import userEvent from '@testing-library/user-event';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { IInsurance, TypeCodeUtils } from 'interfaces';
import { noop } from 'lodash';
import { ILookupCode } from 'store/slices/lookupCodes';
import { act, render, RenderOptions, RenderResult } from 'utils/test-utils';

import InsuranceEditContainer, { InsuranceEditContainerProps } from './EditContainer';

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

const mockInsuranceHome: IInsurance = {
  id: 123459,
  insuranceType: TypeCodeUtils.createFromLookup(mockInsuranceTypeHome),
  otherInsuranceType: '',
  coverageDescription: '',
  coverageLimit: 777,
  expiryDate: '2022-01-01',
  isInsuranceInPlace: true,
  rowVersion: 0,
};

const defaultProps: InsuranceEditContainerProps = {
  leaseId: 1,
  insuranceList: [mockInsuranceHome],
  insuranceTypes: [mockInsuranceTypeHome, mockInsuranceTypeCar],
  onCancel: () => {},
  onSuccess: () => {},
};

const history = createMemoryHistory();

describe('Lease Insurance', () => {
  const setup = (
    renderOptions: RenderOptions & InsuranceEditContainerProps = {
      leaseId: defaultProps.leaseId,
      insuranceList: defaultProps.insuranceList,
      insuranceTypes: defaultProps.insuranceTypes,
      onCancel: () => defaultProps.onCancel,
      onSuccess: () => defaultProps.onSuccess,
    },
  ): RenderResult => {
    // render component under test
    const result = render(
      <Formik onSubmit={noop} initialValues={{}}>
        <InsuranceEditContainer
          insuranceList={renderOptions.insuranceList}
          insuranceTypes={renderOptions.insuranceTypes}
          leaseId={renderOptions.leaseId}
          onCancel={renderOptions.onCancel}
          onSuccess={renderOptions.onSuccess}
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
    const testInsuranceCar: IInsurance = {
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

    const formLimits = result.getAllByTestId('insurance-form-limit');
    expect(formLimits[0] as HTMLInputElement).toHaveValue(mockInsuranceHome.coverageLimit);
    expect(formLimits[1] as HTMLInputElement).toHaveValue(testInsuranceCar.coverageLimit);

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
