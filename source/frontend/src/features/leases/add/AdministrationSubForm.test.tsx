import { Formik, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { mockLookups } from '@/mocks/lookups.mock';
import { getUserMock } from '@/mocks/user.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fillInput,
  renderAsync,
  RenderOptions,
  userEvent,
  fireEvent,
} from '@/utils/test-utils';

import { getDefaultFormLease, LeaseFormModel } from '../models';
import AdministrationSubForm from './AdministrationSubForm';
import React from 'react';
import { ApiGen_CodeTypes_LeasePurposeTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeasePurposeTypes';
import { ApiGen_Concepts_LeasePurpose } from '@/models/api/generated/ApiGen_Concepts_LeasePurpose';
import { getMockApiLease } from '@/mocks/lease.mock';

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
  const setup = async (renderOptions: RenderOptions & { initialValues?: LeaseFormModel } = {}) => {
    const formikRef = React.createRef<FormikProps<LeaseFormModel>>();

    // render component under test
    const utils = await renderAsync(
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

    return {
      ...utils,
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

  it('displays other type text if Other is selected', async () => {
    const { container, findByText } = await setup({});

    let otherField = await container.querySelector(`input[name="otherLeaseTypeDescription"]`);
    expect(otherField).toBeNull();

    await act(async () => {
      await fillInput(container, 'leaseTypeCode', 'OTHER', 'select');
    });
    const otherText = await findByText('Describe other:');
    expect(otherText).toBeVisible();

    otherField = await container.querySelector(`input[name="otherLeaseTypeDescription"]`);
    expect(otherField).toBeVisible();
  });

  it('displays other program text if Other is selected', async () => {
    const { container, getByText } = await setup({});
    let otherField = await container.querySelector(`input[name="otherProgramTypeDescription"]`);
    expect(otherField).toBeNull();

    await act(async () => {
      await fillInput(container, 'programTypeCode', 'OTHER', 'select');
    });
    const otherText = await getByText('Other Program:');
    expect(otherText).toBeVisible();
    otherField = await container.querySelector(`input[name="otherProgramTypeDescription"]`);
    expect(otherField).toBeVisible();
  });

  it('displays other purpose text if Other is selected', async () => {
    const { container, getByText, getOtherPurposeTextbox, getPurposeMultiSelect } = await setup({});
    expect(getOtherPurposeTextbox()).toBeNull();

    const multiSelectPurposes = getPurposeMultiSelect();
    await act(async () => {
      expect(multiSelectPurposes).not.toBeNull();
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
});
