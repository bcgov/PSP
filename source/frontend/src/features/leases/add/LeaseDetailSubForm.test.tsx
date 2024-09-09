import { Formik, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { useProjectTypeahead } from '@/hooks/useProjectTypeahead';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fillInput,
  render,
  RenderOptions,
  screen,
  userEvent,
  waitFor,
  waitForEffects,
} from '@/utils/test-utils';

import { getDefaultFormLease, LeaseFormModel } from '../models';
import { AddLeaseYupSchema } from './AddLeaseYupSchema';
import LeaseDetailSubForm from './LeaseDetailSubForm';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import React from 'react';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { getMockApiLease } from '@/mocks/lease.mock';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

vi.mock('@/hooks/useProjectTypeahead');
const mockUseProjectTypeahead = vi.mocked(useProjectTypeahead);

const handleTypeaheadSearch = vi.fn();

const retrieveProjectProductsFn = vi.fn();
vi.mock('@/hooks/repositories/useProjectProvider');
vi.mocked(useProjectProvider).mockReturnValue({
  retrieveProjectProducts: retrieveProjectProductsFn,
} as unknown as ReturnType<typeof useProjectProvider>);

describe('LeaseDetailSubForm component', () => {
  const setup = async (renderOptions: RenderOptions & { initialValues?: LeaseFormModel } = {}) => {
    // render component under test
    const formikRef = React.createRef<FormikProps<LeaseFormModel>>();
    const utils = render(
      <Formik<LeaseFormModel>
        innerRef={formikRef}
        onSubmit={noop}
        initialValues={renderOptions.initialValues ?? getDefaultFormLease()}
        validationSchema={AddLeaseYupSchema}
      >
        {formikProps => <LeaseDetailSubForm formikProps={formikProps} />}
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
      formikRef,
      // Finding elements
      getStatusDropDown: (): HTMLSelectElement => {
        return document.querySelector(`select[name="statusTypeCode"]`);
      },
      getProjectSelector: (): HTMLElement => {
        return document.querySelector(`input[name="typeahead-project"]`);
      },
      findProjectSelectorItems: async () => {
        return document.querySelectorAll(`a[id^="typeahead-project-item"]`);
      },
      getTerminationReason: (): HTMLElement => {
        return document.querySelector(`textarea[name="terminationReason"]`);
      },
      getCancellationReason: (): HTMLElement => {
        return document.querySelector(`textarea[name="cancellationReason"]`);
      },
    };
  };

  beforeEach(() => {
    mockUseProjectTypeahead.mockReturnValue({
      handleTypeaheadSearch,
      isTypeaheadLoading: false,
      matchedProjects: [
        {
          id: 1,
          text: 'MOCK TEST PROJECT',
        },
        {
          id: 2,
          text: 'ANOTHER MOCK',
        },
      ],
    });
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it.each([
    [ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE, true],
    [ApiGen_CodeTypes_LeaseStatusTypes.ARCHIVED, false],
    [ApiGen_CodeTypes_LeaseStatusTypes.DISCARD, false],
    [ApiGen_CodeTypes_LeaseStatusTypes.DRAFT, false],
    [ApiGen_CodeTypes_LeaseStatusTypes.DUPLICATE, false],
    [ApiGen_CodeTypes_LeaseStatusTypes.INACTIVE, false],
    [ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED, false],
  ])(
    'checks that start date is required only for ACTIVE leases - %s',
    async (leaseStatus: string, startDateRequired: boolean) => {
      const { container, getStatusDropDown, findByText, queryByText } = await setup({});

      await act(async () => userEvent.selectOptions(getStatusDropDown(), leaseStatus));
      await act(async () => {
        fillInput(container, 'startDate', '', 'datepicker');
      });

      if (startDateRequired) {
        expect(await findByText('Required')).toBeVisible();
      } else {
        expect(queryByText('Required')).toBeNull();
      }
    },
  );

  it('checks that expiry date must be later than start date', async () => {
    // start date is not mandatory for DRAFT leases
    const { queryByText, container } = await setup({
      initialValues: {
        ...getDefaultFormLease(),
        statusTypeCode: ApiGen_CodeTypes_LeaseStatusTypes.DRAFT,
      },
    });

    await act(async () => {
      fillInput(container, 'startDate', '', 'datepicker');
    });
    await act(async () => {
      fillInput(container, 'expiryDate', '01/01/2010', 'datepicker');
    });

    expect(queryByText('Expiry Date must be after Start Date')).toBeNull();
  });

  it(`doesn't enforce expiry date logic when start date is not mandatory`, async () => {
    const { findByText, container } = await setup({});

    await act(async () => {
      fillInput(container, 'startDate', '01/02/2025', 'datepicker');
    });
    await act(async () => {
      fillInput(container, 'expiryDate', '01/01/2010', 'datepicker');
    });

    expect(await findByText('Expiry Date must be after Start Date')).toBeVisible();
  });

  it('shows matching projects based on user input', async () => {
    retrieveProjectProductsFn.mockResolvedValue([]);

    const { getProjectSelector, findProjectSelectorItems, container } = await setup({});
    await act(async () => userEvent.type(getProjectSelector()!, 'test'));
    await waitFor(() => expect(handleTypeaheadSearch).toHaveBeenCalled());

    const items = await findProjectSelectorItems();
    expect(items).toHaveLength(2);
    expect(items[0]).toHaveTextContent(/MOCK TEST PROJECT/i);
    expect(items[1]).toHaveTextContent(/ANOTHER MOCK/i);

    const firstOption = container.querySelector(`#typeahead-project-item-0`);
    expect(firstOption).toBeInTheDocument();

    await act(async () => {
      userEvent.click(firstOption);
    });
    await waitForEffects();

    expect(retrieveProjectProductsFn).toHaveBeenCalled();
  });

  it('Removes product when Project Removed', async () => {
    retrieveProjectProductsFn.mockResolvedValue([]);

    const { getProjectSelector, findProjectSelectorItems, container } = await setup({});
    await act(async () => userEvent.type(getProjectSelector()!, 'test'));
    await waitFor(() => expect(handleTypeaheadSearch).toHaveBeenCalled());

    const items = await findProjectSelectorItems();
    expect(items).toHaveLength(2);
    expect(items[0]).toHaveTextContent(/MOCK TEST PROJECT/i);
    expect(items[1]).toHaveTextContent(/ANOTHER MOCK/i);

    const firstOption = container.querySelector(`#typeahead-project-item-0`);
    expect(firstOption).toBeInTheDocument();

    await act(async () => {
      userEvent.click(firstOption);
    });
    await waitForEffects();

    expect(retrieveProjectProductsFn).toHaveBeenCalled();

    const projectTypeahead = container.querySelector(`#typeahead-project`);
    await act(async () => {
      userEvent.clear(projectTypeahead);
    });
    await waitForEffects();

    const productMultiSelect = container.querySelector(`#input-productId`);
    expect(productMultiSelect).not.toBeInTheDocument();
  });

  it('Show project and product values for the form when loaded', async () => {
    const mockLease = getMockApiLease();
    retrieveProjectProductsFn.mockResolvedValue([
      {
        id: 6,
        projectProducts: [],
        acquisitionFiles: [],
        code: '00053EXP',
        description: 'DIR C\u0026M \u0026 CLAIMS OVERHEAD',
        startDate: null,
        costEstimate: null,
        costEstimateDate: null,
        objective: null,
        scope: null,
        appCreateTimestamp: '2024-09-04T17:09:56.527',
        appLastUpdateTimestamp: '2024-09-04T17:09:56.527',
        appLastUpdateUserid: 'dbo',
        appCreateUserid: 'dbo',
        appLastUpdateUserGuid: null,
        appCreateUserGuid: null,
        rowVersion: 1,
      },
    ]);

    const { container } = await setup({
      initialValues: LeaseFormModel.fromApi(mockLease),
    });
    await waitForEffects();
    expect(retrieveProjectProductsFn).toHaveBeenCalled();

    const productMultiSelect = container.querySelector(`#input-productId`);
    expect(productMultiSelect).toBeInTheDocument();
  });

  it('displays the cancellation reason textbox when status is changed to "Discarded"', async () => {
    const { getCancellationReason, getStatusDropDown } = await setup({});

    await act(async () =>
      userEvent.selectOptions(getStatusDropDown(), ApiGen_CodeTypes_LeaseStatusTypes.DISCARD),
    );

    expect(getCancellationReason()).toBeInTheDocument();
  });

  it('displays a confirmation modal when user changes the status from "Discarded" to a new status', async () => {
    const { getByTestId, getCancellationReason, getStatusDropDown, formikRef } = await setup({
      initialValues: {
        ...getDefaultFormLease(),
        statusTypeCode: ApiGen_CodeTypes_LeaseStatusTypes.DISCARD,
      },
    });

    expect(formikRef.current).not.toBeNull();
    expect(getCancellationReason()).toBeInTheDocument();

    await act(async () => {
      userEvent.paste(getCancellationReason(), 'Lorem ipsum');
    });

    // cancellation reason is captured in the form values
    expect(formikRef.current.values.cancellationReason).toBe('Lorem ipsum');

    // changing from "Discarded" a new status should trigger a confirmation modal
    await act(async () =>
      userEvent.selectOptions(getStatusDropDown(), ApiGen_CodeTypes_LeaseStatusTypes.DRAFT),
    );

    const popup = await screen.findByText('The lease is no longer in', { exact: false });
    expect(popup.textContent).toBe(
      'The lease is no longer in Cancelled state. The reason for doing so will be cleared from the file details and can only be viewed in the notes tab.Do you want to proceed?',
    );

    const okButton = getByTestId('ok-modal-button');
    await act(async () => userEvent.click(okButton));

    // cancellation reason is cleared upon closing the modal
    expect(formikRef.current.values.cancellationReason).toBe('');
  });

  it(`doesn't clear the cancellation reason textbox if the user cancels the confirmation modal`, async () => {
    const { getByTestId, getCancellationReason, getStatusDropDown, formikRef } = await setup({
      initialValues: {
        ...getDefaultFormLease(),
        statusTypeCode: ApiGen_CodeTypes_LeaseStatusTypes.DISCARD,
      },
    });

    expect(formikRef.current).not.toBeNull();
    expect(getCancellationReason()).toBeInTheDocument();

    await act(async () => {
      userEvent.paste(getCancellationReason(), 'Lorem ipsum');
    });

    // cancellation reason is captured in the form values
    expect(formikRef.current.values.cancellationReason).toBe('Lorem ipsum');

    // changing from "Discarded" a new status should trigger a confirmation modal
    await act(async () =>
      userEvent.selectOptions(getStatusDropDown(), ApiGen_CodeTypes_LeaseStatusTypes.DRAFT),
    );

    const popup = await screen.findByText('The lease is no longer in', { exact: false });
    expect(popup.textContent).toBe(
      'The lease is no longer in Cancelled state. The reason for doing so will be cleared from the file details and can only be viewed in the notes tab.Do you want to proceed?',
    );

    const cancelButton = getByTestId('cancel-modal-button');
    await act(async () => userEvent.click(cancelButton));

    // cancellation reason should remain the same if modal was cancelled
    expect(formikRef.current.values.cancellationReason).toBe('Lorem ipsum');
  });

  it('displays the termination reason textbox when status is changed to "Terminated"', async () => {
    const { getTerminationReason, getStatusDropDown } = await setup({});

    await act(async () =>
      userEvent.selectOptions(getStatusDropDown(), ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED),
    );

    expect(getTerminationReason()).toBeInTheDocument();
  });

  it('displays a confirmation modal when user changes the status from "Terminated" to a new status', async () => {
    const { getByTestId, getTerminationReason, getStatusDropDown, formikRef } = await setup({
      initialValues: {
        ...getDefaultFormLease(),
        statusTypeCode: ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED,
      },
    });

    expect(formikRef.current).not.toBeNull();
    expect(getTerminationReason()).toBeInTheDocument();

    await act(async () => {
      userEvent.paste(getTerminationReason(), 'Lorem ipsum');
    });

    // cancellation reason is captured in the form values
    expect(formikRef.current.values.terminationReason).toBe('Lorem ipsum');

    // changing from "Discarded" a new status should trigger a confirmation modal
    await act(async () =>
      userEvent.selectOptions(getStatusDropDown(), ApiGen_CodeTypes_LeaseStatusTypes.DRAFT),
    );

    const popup = await screen.findByText('The lease is no longer in', { exact: false });
    expect(popup.textContent).toBe(
      'The lease is no longer in Terminated state. The reason for doing so will be cleared from the file details and can only be viewed in the notes tab.Do you want to proceed?',
    );

    const okButton = getByTestId('ok-modal-button');
    await act(async () => userEvent.click(okButton));

    // cancellation reason is cleared upon closing the modal
    expect(formikRef.current.values.terminationReason).toBe('');
  });

  it(`doesn't clear the termination reason textbox if the user cancels the confirmation modal`, async () => {
    const { getByTestId, getTerminationReason, getStatusDropDown, formikRef } = await setup({
      initialValues: {
        ...getDefaultFormLease(),
        statusTypeCode: ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED,
      },
    });

    expect(formikRef.current).not.toBeNull();
    expect(getTerminationReason()).toBeInTheDocument();

    await act(async () => {
      userEvent.paste(getTerminationReason(), 'Lorem ipsum');
    });

    // cancellation reason is captured in the form values
    expect(formikRef.current.values.terminationReason).toBe('Lorem ipsum');

    // changing from "Discarded" a new status should trigger a confirmation modal
    await act(async () =>
      userEvent.selectOptions(getStatusDropDown(), ApiGen_CodeTypes_LeaseStatusTypes.DRAFT),
    );

    const popup = await screen.findByText('The lease is no longer in', { exact: false });
    expect(popup.textContent).toBe(
      'The lease is no longer in Terminated state. The reason for doing so will be cleared from the file details and can only be viewed in the notes tab.Do you want to proceed?',
    );

    const cancelButton = getByTestId('cancel-modal-button');
    await act(async () => userEvent.click(cancelButton));

    // cancellation reason should remain the same if modal was cancelled
    expect(formikRef.current.values.terminationReason).toBe('Lorem ipsum');
  });
});
