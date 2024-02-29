import { getMockAccessRequest } from '@/mocks/accessRequest.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_Concepts_AccessRequest } from '@/models/api/generated/ApiGen_Concepts_AccessRequest';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { fakeText, fillInput, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import AccessRequestForm from './AccessRequestForm';
import { FormAccessRequest } from './models';

const addAccessRequest = jest.fn();
const onCancel = jest.fn();

describe('AccessRequestForm component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { initialValues: FormAccessRequest }) => {
    const utils = render(
      <AccessRequestForm
        initialValues={renderOptions.initialValues}
        addAccessRequest={addAccessRequest}
        onCancel={onCancel}
      />,

      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
      },
    );

    return {
      ...utils,
      getSubmitButton: () => utils.getAllByText(/Submit/i)[1],
      getUpdateButton: () => utils.getByText(/Update/i),
      getCancelButton: () => utils.getByText(/Cancel/i),
    };
  };

  let initialValues: FormAccessRequest;

  beforeEach(() => {
    initialValues = new FormAccessRequest(getMockAccessRequest());
  });

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup({ initialValues });
    expect(asFragment()).toMatchSnapshot();
  });

  it('cancels the form when Cancel is clicked', () => {
    const { getCancelButton } = setup({ initialValues });
    userEvent.click(getCancelButton());
    expect(onCancel).toBeCalled();
  });

  it('saves the form with minimal data when Submit button is clicked', async () => {
    const { getUpdateButton } = setup({
      initialValues,
    });
    const submitButton = getUpdateButton();
    userEvent.click(submitButton);
    await waitFor(async () => expect(addAccessRequest).toHaveBeenCalledWith(initialValues.toApi()));
  });

  it('saves the form with updated values when Submit button is clicked', async () => {
    const { getUpdateButton, container } = setup({ initialValues });

    // modify form values
    await fillInput(container, 'position', 'position');
    await fillInput(container, 'note', 'test note', 'textarea');
    const submitButton = getUpdateButton();
    userEvent.click(submitButton);

    const expectedValues: ApiGen_Concepts_AccessRequest = { ...initialValues.toApi() };
    expectedValues.user!.position = 'position';
    expectedValues.note = 'test note';

    await waitFor(() => expect(addAccessRequest).toHaveBeenCalledWith(expectedValues));
  });

  it('validates character limits', async () => {
    const { container, findByText, getUpdateButton } = setup({ initialValues });

    // first name cannot exceed 50 characters
    await fillInput(container, 'position', fakeText(150));
    expect(await findByText(/Position must be less than 100 characters/i)).toBeVisible();

    await fillInput(container, 'note', fakeText(4001), 'textarea');
    expect(await findByText(/Note must be less than 4000 characters/i)).toBeVisible();

    const submitButton = getUpdateButton();
    userEvent.click(submitButton);

    await waitFor(() => expect(addAccessRequest).not.toHaveBeenCalled());
  });
});
