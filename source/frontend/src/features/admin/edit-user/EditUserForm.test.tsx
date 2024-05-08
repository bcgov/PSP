import { mockLookups } from '@/mocks/lookups.mock';
import { getUserMock } from '@/mocks/user.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fakeText,
  fillInput,
  render,
  RenderOptions,
  userEvent,
  waitFor,
} from '@/utils/test-utils';

import { FormUser } from '../users/models';
import EditUserForm from './EditUserForm';

const onSubmit = vi.fn();
const onCancel = vi.fn();

describe('EditUserForm component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { initialValues: FormUser }) => {
    const utils = render(
      <EditUserForm
        formUser={renderOptions.initialValues}
        updateUserDetail={onSubmit}
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
      getSaveButton: () => utils.getByText(/Save/i),
      getCancelButton: () => utils.getByText(/Cancel/i),
    };
  };

  let initialValues: FormUser;

  beforeEach(() => {
    initialValues = new FormUser(getUserMock());
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ initialValues });
    expect(asFragment()).toMatchSnapshot();
  });

  it('cancels the form when Cancel is clicked', async () => {
    const { getCancelButton } = setup({ initialValues });
    await act(async () => userEvent.click(getCancelButton()));
    expect(onCancel).toBeCalled();
  });

  it('saves the form with minimal data when Submit button is clicked', async () => {
    const { getSaveButton } = setup({ initialValues });
    const submitButton = getSaveButton();
    await act(async () => userEvent.click(submitButton));
    await waitFor(async () => expect(onSubmit).toHaveBeenCalledWith(initialValues.toApi()));
  });

  it('saves the form with updated values when Submit button is clicked', async () => {
    const { getSaveButton, container } = setup({ initialValues });

    // modify form values
    await fillInput(container, 'position', 'position');
    await fillInput(container, 'note', 'test note', 'textarea');
    const submitButton = getSaveButton();
    await act(async () => userEvent.click(submitButton));

    const expectedValues = { ...initialValues.toApi() };
    expectedValues.position = 'position';
    expectedValues.note = 'test note';

    await waitFor(() => expect(onSubmit).toHaveBeenCalledWith(expectedValues));
  });

  it('should validate required fields', async () => {
    const { container, findByText, getSaveButton } = setup({ initialValues });

    // first name is required
    await fillInput(container, 'firstName', '');
    expect(await findByText(/First Name is required/i)).toBeVisible();

    // last name is required
    await fillInput(container, 'surname', '');
    expect(await findByText(/Last Name is required/i)).toBeVisible();

    const submitButton = getSaveButton();
    await act(async () => userEvent.click(submitButton));

    await waitFor(() => expect(onSubmit).not.toHaveBeenCalled());
  });

  it('should validate character limits', async () => {
    const { container, findByText, getSaveButton } = setup({ initialValues });

    // first name cannot exceed 50 characters
    await fillInput(container, 'firstName', fakeText(51));
    expect(await findByText(/First Name must be less than 50 characters/i)).toBeVisible();

    // last name cannot exceed 50 characters
    await fillInput(container, 'surname', fakeText(51));
    expect(await findByText(/Last Name must be less than 50 characters/i)).toBeVisible();

    await fillInput(container, 'position', fakeText(150));
    expect(await findByText(/Position must be less than 100 characters/i)).toBeVisible();

    await fillInput(container, 'note', fakeText(2000), 'textarea');
    expect(await findByText(/Note must be less than 1000 characters/i)).toBeVisible();

    const submitButton = getSaveButton();
    await act(async () => userEvent.click(submitButton));

    await waitFor(() => expect(onSubmit).not.toHaveBeenCalled());
  });

  it('should identify user as internal staff or contractor', async () => {
    const { getByText } = setup({ initialValues });

    const internal = getByText('Ministry staff');
    const contractor = getByText('Contractor');

    expect(internal).toBeInTheDocument();
    expect(contractor).toBeInTheDocument();
  });
});
