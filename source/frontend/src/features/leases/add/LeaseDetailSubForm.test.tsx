import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { useProjectTypeahead } from '@/hooks/useProjectTypeahead';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fillInput,
  fireEvent,
  renderAsync,
  RenderOptions,
  userEvent,
  waitFor,
} from '@/utils/test-utils';

import { getDefaultFormLease } from '../models';
import { AddLeaseYupSchema } from './AddLeaseYupSchema';
import LeaseDetailSubForm, { ILeaseDetailsSubFormProps } from './LeaseDetailSubForm';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

vi.mock('@/hooks/useProjectTypeahead');
const mockUseProjectTypeahead = vi.mocked(useProjectTypeahead);

const handleTypeaheadSearch = vi.fn();

describe('LeaseDetailSubForm component', () => {
  const setup = async (renderOptions: RenderOptions & Partial<ILeaseDetailsSubFormProps> = {}) => {
    // render component under test
    const component = await renderAsync(
      <Formik
        onSubmit={noop}
        initialValues={getDefaultFormLease()}
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
      ...component,
      // Finding elements
      getStatusDropDown: () =>
        component.container.querySelector(`select[name="statusTypeCode"]`) as HTMLInputElement,
      getProjectSelector: () => {
        return document.querySelector(`input[name="typeahead-project"]`);
      },
      findProjectSelectorItems: async () => {
        return document.querySelectorAll(`a[id^="typeahead-project-item"]`);
      },
      getTerminationReason: () => {
        return document.querySelector(`textarea[name="terminationReason"]`);
      },
      getCancellationReason: () => {
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

  afterEach(() => {
    vi.resetAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('checks that expiry date must be later than start date', async () => {
    const { findByText, container } = await setup({});
    await fillInput(container, 'startDate', '01/02/2020', 'datepicker');
    await fillInput(container, 'expiryDate', '01/01/2020', 'datepicker');
    expect(await findByText('Expiry Date must be after Start Date')).toBeVisible();
  });

  it('shows matching projects based on user input', async () => {
    const { getProjectSelector, findProjectSelectorItems } = await setup({});
    await act(async () => userEvent.type(getProjectSelector()!, 'test'));
    await waitFor(() => expect(handleTypeaheadSearch).toHaveBeenCalled());

    const items = await findProjectSelectorItems();
    expect(items).toHaveLength(2);
    expect(items[0]).toHaveTextContent(/MOCK TEST PROJECT/i);
    expect(items[1]).toHaveTextContent(/ANOTHER MOCK/i);
  });

  it('displays the cancellation reason textbox whe status is changed to "Cancelled"', async () => {
    const { container, getCancellationReason } = await setup({});

    await act(async () => {
      fillInput(container, 'statusTypeCode', ApiGen_CodeTypes_LeaseStatusTypes.DISCARD, 'select');
    });

    expect(getCancellationReason()).toBeInTheDocument();
  });

  it('displays the termination reason textbox whe status is changed to "Terminated"', async () => {
    const { container, getTerminationReason } = await setup({});

    await act(async () => {
      fillInput(container, 'statusTypeCode', ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED, 'select');
    });

    expect(getTerminationReason()).toBeInTheDocument();
  });
});
