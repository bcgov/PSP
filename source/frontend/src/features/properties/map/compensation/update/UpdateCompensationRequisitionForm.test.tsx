import { FormikProps } from 'formik';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { getMockApiDefaultCompensation } from 'mocks/mockCompensations';
import { mockLookups } from 'mocks/mockLookups';
import { createRef } from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import {
  act,
  fakeText,
  fillInput,
  render,
  RenderOptions,
  userEvent,
  waitFor,
} from 'utils/test-utils';

import { CompensationRequisitionFormModel } from '../models';
import UpdateCompensationRequisitionForm, {
  CompensationRequisitionFormProps,
} from './UpdateCompensationRequisitionForm';

const onSave = jest.fn();
const onCancel = jest.fn();

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const defauiltApiCompensation = getMockApiDefaultCompensation();
const defaultCompensation = new CompensationRequisitionFormModel(
  defauiltApiCompensation.id,
  defauiltApiCompensation.acquisitionFileId,
);

describe('TakesUpdateForm component', () => {
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<CompensationRequisitionFormProps> },
  ) => {
    const formikRef = createRef<FormikProps<CompensationRequisitionFormModel>>();
    const utils = render(
      <UpdateCompensationRequisitionForm
        {...renderOptions.props}
        onSave={onSave}
        onCancel={onCancel}
        initialValues={renderOptions.props?.initialValues ?? defaultCompensation}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? mockAcquisitionFileResponse()}
        isLoading={renderOptions.props?.isLoading ?? false}
        formikRef={formikRef}
      />,
      {
        ...renderOptions,
        store: storeState,
      },
    );

    return {
      ...utils,
      formikRef,
      getSpecialInstructionsTextbox: () =>
        utils.container.querySelector(`textarea[name="specialInstruction"]`) as HTMLInputElement,
      getDetailedRemarksTextbox: () =>
        utils.container.querySelector(`textarea[name="detailedRemarks"]`) as HTMLInputElement,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a loading spinner when loading', () => {
    const { getByTestId } = setup({ props: { isLoading: true } });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('should validate character limits', async () => {
    const { findByText, formikRef, getSpecialInstructionsTextbox, getDetailedRemarksTextbox } =
      setup({
        props: { initialValues: defaultCompensation },
      });

    await waitFor(() => userEvent.paste(getSpecialInstructionsTextbox(), fakeText(2001)));
    await waitFor(() => userEvent.paste(getDetailedRemarksTextbox(), fakeText(2001)));

    await act(async () => {
      formikRef.current?.submitForm();
    });

    expect(await findByText(/Special instructions must be at most 2000 characters/i)).toBeVisible();
    expect(await findByText(/Detailed remarks must be at most 2000 characters/i)).toBeVisible();
  });

  it('should validate extra fields when changing to final status', async () => {
    const { findByText, formikRef, container } = setup({
      props: { initialValues: defaultCompensation },
    });

    await fillInput(container, 'status', 'final', 'select');

    await act(async () => {
      formikRef.current?.submitForm();
    });

    expect(await findByText(/Fiscal year is required/i)).toBeVisible();
  });
});
