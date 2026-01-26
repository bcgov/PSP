import { act, fakeText, render, RenderOptions, userEvent } from '@/utils/test-utils';
import PropertyImprovementForm, { IPropertyImprovementFormProps } from './PropertyImprovementForm';
import { FormikProps } from 'formik';
import { PropertyImprovementFormModel } from '../models/PropertyImprovementFormModel';
import { createRef } from 'react';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockPropertyImprovementApi } from '@/mocks/propertyImprovements.mock';
import { ApiGen_CodeTypes_PropertyImprovementTypes } from '@/models/api/generated/ApiGen_CodeTypes_PropertyImprovementTypes';

const onSubmit = vi.fn();
const onCancel = vi.fn();
const PropertyId = 1000;

const mockInitialValues = new PropertyImprovementFormModel(null, PropertyId);
const mockPropertyImprovementApi: PropertyImprovementFormModel =
  PropertyImprovementFormModel.fromApi(getMockPropertyImprovementApi(PropertyId));

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('PropertyImprovementForm component', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IPropertyImprovementFormProps> },
  ) => {
    const formikRef = createRef<FormikProps<PropertyImprovementFormModel>>();
    const utils = render(
      <PropertyImprovementForm
        isLoading={renderOptions.props?.isLoading ?? false}
        initialValues={renderOptions.props?.initialValues ?? mockInitialValues}
        onSubmit={onSubmit}
        onCancel={onCancel}
      />,
      {
        ...renderOptions,
        store: storeState,
      },
    );

    return {
      ...utils,
      formikRef,
      getImprovementTypeDropdown: () =>
        utils.container.querySelector(
          `select[name="propertyImprovementTypeCode"]`,
        ) as HTMLSelectElement,
      getImprovementDescriptionTextarea: () =>
        utils.container.querySelector(`textarea[name="description"]`) as HTMLInputElement,
    };
  };

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a loading spinner when loading', async () => {
    const { getByTestId } = await setup({ props: { isLoading: true } });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('calls onCancel when cancel button is clicked', async () => {
    const { getByText } = await setup({});
    const cancelButton = getByText('Cancel');
    await act(async () => userEvent.click(cancelButton));
    expect(onCancel).toHaveBeenCalled();
  });

  it('asks for confirmation before cancelling when the form has changes', async () => {
    const { getImprovementDescriptionTextarea, getByText, getByTitle } = await setup({});
    await act(async () => {
      userEvent.paste(getImprovementDescriptionTextarea(), 'some new description');
    });
    const cancelButton = getByText('Cancel');
    await act(async () => userEvent.click(cancelButton));

    // Confirmation modal should be shown
    expect(onCancel).not.toHaveBeenCalled();
    expect(getByText(/If you choose to cancel now, your changes will not be saved/i)).toBeVisible();

    const confirmButton = getByTitle('ok-modal');
    await act(async () => userEvent.click(confirmButton));

    // onCancel should be called after confirming
    expect(onCancel).toHaveBeenCalled();
  });

  it('should validate character limits', async () => {
    const { findByText, getByText, getImprovementDescriptionTextarea } = await setup({});

    await act(async () => {
      userEvent.paste(getImprovementDescriptionTextarea(), fakeText(2001));
    });

    const saveButton = getByText('Save');
    await act(async () => userEvent.click(saveButton));

    expect(await findByText(/Description field must be at most 2000 characters/i)).toBeVisible();
  });

  it('should validate type required', async () => {
    const { findByText, getByText, getImprovementDescriptionTextarea } = await setup({});

    await act(async () => {
      userEvent.paste(getImprovementDescriptionTextarea(), fakeText(1001));
    });

    const saveButton = getByText('Save');
    await act(async () => userEvent.click(saveButton));

    expect(await findByText(/Improvement type is required/i)).toBeVisible();
  });

  it('displays the existing values for the improvement', async () => {
    const { getImprovementTypeDropdown, getImprovementDescriptionTextarea } = await setup({
      props: { initialValues: mockPropertyImprovementApi },
    });

    expect(getImprovementTypeDropdown()).toHaveValue(
      ApiGen_CodeTypes_PropertyImprovementTypes.COMMBLDG,
    );
    expect(getImprovementDescriptionTextarea()).toHaveValue('TEST DESCRIPTION');
  });

  it('call on submit for minimun form data', async () => {
    const { getImprovementTypeDropdown, getByText } =
      await setup({});

    await act(async () => {
      userEvent.selectOptions(
        getImprovementTypeDropdown(),
        ApiGen_CodeTypes_PropertyImprovementTypes.RTA,
      );
    });

    const saveButton = getByText('Save');
    await act(async () => userEvent.click(saveButton));

    expect(onSubmit).toHaveBeenCalled();
  });
});
