import { getMockAccessRequest } from 'mocks/accessRequestMock';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

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

    const expectedValues = { ...initialValues.toApi() };
    expectedValues.user!.position = 'position';
    expectedValues.note = 'test note';

    await waitFor(() => expect(addAccessRequest).toHaveBeenCalledWith(expectedValues));
  });
});
