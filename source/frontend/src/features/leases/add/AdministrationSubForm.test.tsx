import { Formik, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';
import React from 'react';

import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { mockLookups } from '@/mocks/lookups.mock';
import { getUserMock } from '@/mocks/user.mock';
import { ApiGen_CodeTypes_LeaseLicenceTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseLicenceTypes';
import { ApiGen_CodeTypes_LeaseProgramTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseProgramTypes';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { getDefaultFormLease, LeaseFormModel } from '../models';
import AdministrationSubForm from './AdministrationSubForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

// mock auth library
vi.mock('@/hooks/repositories/useUserInfoRepository');
vi.mocked(useUserInfoRepository).mockReturnValue({
  retrieveUserInfo: vi.fn(),
  retrieveUserInfoLoading: true,
  retrieveUserInfoResponse: getUserMock(),
});

describe('AdministrationSubForm component', () => {
  // render component under test
  const setup = async (renderOptions: RenderOptions & { initialValues?: LeaseFormModel } = {}) => {
    const formikRef = React.createRef<FormikProps<LeaseFormModel>>();
    const utils = render(
      <Formik
        onSubmit={noop}
        innerRef={formikRef}
        initialValues={renderOptions.initialValues ?? getDefaultFormLease()}
      >
        {formikProps => <AdministrationSubForm formikProps={formikProps} />}
      </Formik>,
      {
        ...renderOptions,
        claims: [],
        store: storeState,
        history,
      },
    );

    await act(async () => {});

    return {
      ...utils,
      formikRef,
      getPurposeMultiSelect: () =>
        utils.container.querySelector(`#multiselect-purposes_input`) as HTMLElement,
      getOtherPurposeTextbox: () =>
        utils.container.querySelector(`input[name="purposeOtherDescription"]`) as HTMLElement,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('clears other type description when lease type is changed from Other to another value', async () => {
    const leaseForm = getDefaultFormLease();
    leaseForm.leaseTypeCode = ApiGen_CodeTypes_LeaseLicenceTypes.OTHER;
    leaseForm.otherLeaseTypeDescription = 'Some other type';
    const { container, findByText, formikRef } = await setup({
      initialValues: leaseForm,
    });

    expect(formikRef.current?.values.otherLeaseTypeDescription).toBe('Some other type');

    const otherText = await findByText('Describe other:');
    expect(otherText).toBeVisible();

    let otherField = container.querySelector(`input[name="otherLeaseTypeDescription"]`);
    expect(otherField).toBeVisible();

    await act(async () => {
      await fillInput(
        container,
        'leaseTypeCode',
        ApiGen_CodeTypes_LeaseLicenceTypes.MANUFHOME,
        'select',
      );
    });

    // Verify that the otherLeaseTypeDescription field has been cleared
    expect(formikRef.current?.values.otherLeaseTypeDescription).toBe('');

    otherField = container.querySelector(`input[name="otherLeaseTypeDescription"]`);
    expect(otherField).toBeNull();
  });

  it('displays other type text if Other is selected', async () => {
    const { container, findByText } = await setup({});

    let otherField = container.querySelector(`input[name="otherLeaseTypeDescription"]`);
    expect(otherField).toBeNull();

    await act(async () => {
      await fillInput(
        container,
        'leaseTypeCode',
        ApiGen_CodeTypes_LeaseLicenceTypes.OTHER,
        'select',
      );
    });
    const otherText = await findByText('Describe other:');
    expect(otherText).toBeVisible();

    otherField = container.querySelector(`input[name="otherLeaseTypeDescription"]`);
    expect(otherField).toBeVisible();
  });

  it('displays other program text if Other is selected', async () => {
    const { container, getByText } = await setup({});
    let otherField = container.querySelector(`input[name="otherProgramTypeDescription"]`);
    expect(otherField).toBeNull();

    await act(async () => {
      await fillInput(
        container,
        'programTypeCode',
        ApiGen_CodeTypes_LeaseProgramTypes.OTHER,
        'select',
      );
    });
    const otherText = await getByText('Other Program:');
    expect(otherText).toBeVisible();
    otherField = container.querySelector(`input[name="otherProgramTypeDescription"]`);
    expect(otherField).toBeVisible();
  });

  it('displays other purpose text if Other is selected', async () => {
    const { container, getByText, getOtherPurposeTextbox, getPurposeMultiSelect } = await setup({});
    expect(getOtherPurposeTextbox()).toBeNull();

    const multiSelectPurposes = getPurposeMultiSelect();
    expect(multiSelectPurposes).not.toBeNull();
    await act(async () => {
      userEvent.click(multiSelectPurposes);
    });

    await act(async () => {
      userEvent.type(multiSelectPurposes, 'Other*');
      userEvent.click(multiSelectPurposes);

      const otherOption = container.querySelector(`div ul li.option`);
      userEvent.click(otherOption);
    });

    const otherText = await getByText('Describe other:');
    expect(otherText).toBeVisible();
    expect(getOtherPurposeTextbox()).toBeInTheDocument();
  });

  it('clears other program description when program type is changed from Other to another value', async () => {
    const leaseForm = getDefaultFormLease();
    leaseForm.programTypeCode = ApiGen_CodeTypes_LeaseLicenceTypes.OTHER;
    leaseForm.otherProgramTypeDescription = 'Some other program';
    const { container, formikRef } = await setup({
      initialValues: leaseForm,
    });

    expect(formikRef.current?.values.otherProgramTypeDescription).toBe('Some other program');

    const otherField = container.querySelector(`input[name="otherProgramTypeDescription"]`);
    expect(otherField).toBeVisible();

    await act(async () => {
      await fillInput(
        container,
        'programTypeCode',
        ApiGen_CodeTypes_LeaseProgramTypes.AGRIC,
        'select',
      );
    });

    // Verify that the otherProgramTypeDescription field has been cleared
    expect(formikRef.current?.values.otherProgramTypeDescription).toBe('');
  });
});
