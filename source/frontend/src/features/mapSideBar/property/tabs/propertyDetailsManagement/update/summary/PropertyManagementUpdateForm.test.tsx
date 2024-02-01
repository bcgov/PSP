import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import React from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import {
  getMockApiPropertyManagement,
  getMockApiPropertyManagementPurpose,
} from '@/mocks/propertyManagement.mock';
import { ApiGen_Concepts_PropertyManagementPurpose } from '@/models/api/generated/ApiGen_Concepts_PropertyManagementPurpose';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { fakeText, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { PropertyManagementFormModel } from './models';
import {
  IPropertyManagementUpdateFormProps,
  PropertyManagementUpdateForm,
} from './PropertyManagementUpdateForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onSave = jest.fn();

describe('PropertyManagementUpdateForm component', () => {
  const setup = (
    renderOptions?: RenderOptions & { props?: Partial<IPropertyManagementUpdateFormProps> },
  ) => {
    renderOptions = renderOptions ?? {};
    const formikRef = React.createRef<FormikProps<PropertyManagementFormModel>>();
    const utils = render(
      <PropertyManagementUpdateForm
        {...renderOptions.props}
        onSave={onSave}
        propertyManagement={
          renderOptions.props?.propertyManagement ?? getMockApiPropertyManagement()
        }
        isLoading={renderOptions.props?.isLoading ?? false}
        ref={formikRef}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
      getFormikRef: () => formikRef,
      getAdditionalDetailsTextArea: () =>
        utils.container.querySelector(`textarea[name="additionalDetails"]`) as HTMLTextAreaElement,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a loading spinner when loading', () => {
    const { getByTestId } = setup({ props: { isLoading: true } });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('displays existing values if they exist', () => {
    const { getByText } = setup({
      props: {
        propertyManagement: {
          ...getMockApiPropertyManagement(),
          managementPurposes: [getMockApiPropertyManagementPurpose()],
        },
      },
    });
    expect(getByText('BC Ferries')).toBeVisible();
  });

  it('calls onSave when form is submitted', async () => {
    const { getFormikRef } = setup({
      props: {
        propertyManagement: {
          ...getMockApiPropertyManagement(),
          managementPurposes: [getMockApiPropertyManagementPurpose()],
        },
      },
    });
    await waitFor(() => getFormikRef()?.current?.submitForm());
    expect(onSave).toHaveBeenCalled();
  });

  it('should validate required field Additional Details when purpose type is Other', async () => {
    const otherPurpose: ApiGen_Concepts_PropertyManagementPurpose = {
      ...getMockApiPropertyManagementPurpose(),
      propertyPurposeTypeCode: {
        id: 'OTHER',
        description: 'test',
        displayOrder: null,
        isDisabled: false,
      },
    };
    const { getFormikRef, findByText, getAdditionalDetailsTextArea } = setup({
      props: {
        propertyManagement: {
          ...getMockApiPropertyManagement(),
          managementPurposes: [otherPurpose],
        },
      },
    });

    const textArea = getAdditionalDetailsTextArea();
    await waitFor(() => userEvent.clear(textArea));

    // submit form to trigger validation check
    await waitFor(() => getFormikRef()?.current?.submitForm());

    expect(
      await findByText(/Additional details are required when Other purpose is selected/i),
    ).toBeVisible();
  });

  it('should validate character limits', async () => {
    const otherPurpose: ApiGen_Concepts_PropertyManagementPurpose = {
      ...getMockApiPropertyManagementPurpose(),
    };
    const { getFormikRef, findByText, getAdditionalDetailsTextArea } = setup({
      props: {
        propertyManagement: {
          ...getMockApiPropertyManagement(),
          managementPurposes: [otherPurpose],
        },
      },
    });

    // additional details cannot exceed 4000 characters and it's required when OTHER purpose is selected
    const textArea = getAdditionalDetailsTextArea();
    await waitFor(() => userEvent.paste(textArea, fakeText(4200)));

    // submit form to trigger validation check
    await waitFor(() => getFormikRef()?.current?.submitForm());

    expect(await findByText(/Additional details must be at most 4000 characters/i)).toBeVisible();
  });
});
