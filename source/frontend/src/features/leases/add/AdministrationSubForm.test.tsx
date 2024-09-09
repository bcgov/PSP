import { Formik, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { mockLookups } from '@/mocks/lookups.mock';
import { getUserMock } from '@/mocks/user.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { getDefaultFormLease, LeaseFormModel } from '../models';
import AdministrationSubForm from './AdministrationSubForm';
import React from 'react';

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
  const setup = (renderOptions: RenderOptions & { initialValues?: LeaseFormModel } = {}) => {
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

  it('renders as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays other type text if Other is selected', async () => {
    const { container, findByText } = setup({});

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
    const { container, getByText } = setup({});
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
    const { container, getByText, getOtherPurposeTextbox, getPurposeMultiSelect } = setup({});
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
