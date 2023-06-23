import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { useProjectTypeahead } from '@/hooks/useProjectTypeahead';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, renderAsync, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { getDefaultFormLease } from '../models';
import { AddLeaseYupSchema } from './AddLeaseYupSchema';
import LeaseDetailSubForm, { ILeaseDetailsSubFormProps } from './LeaseDetailSubForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

jest.mock('@/hooks/useProjectTypeahead');
const mockUseProjectTypeahead = useProjectTypeahead as jest.MockedFunction<
  typeof useProjectTypeahead
>;

const handleTypeaheadSearch = jest.fn();

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
      getProjectSelector: () => {
        return document.querySelector(`input[name="typeahead-project"]`);
      },
      findProjectSelectorItems: async () => {
        return document.querySelectorAll(`a[id^="typeahead-project-item"]`);
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
    jest.resetAllMocks();
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
});
