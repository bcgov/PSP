import { mockLookups } from 'mocks/mockLookups';
import { getUserMock } from 'mocks/userMock';
import React from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import { FormUser } from '../users/models';
import EditUserForm from './EditUserForm';

const onSubmit = jest.fn();
const onCancel = jest.fn();

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
    const { getSaveButton } = setup({
      initialValues,
    });
    const submitButton = getSaveButton();
    userEvent.click(submitButton);
    await waitFor(async () => expect(onSubmit).toHaveBeenCalledWith(initialValues.toApi()));
  });

  it('saves the form with updated values when Submit button is clicked', async () => {
    const { getSaveButton, container } = setup({ initialValues });

    // modify form values
    await fillInput(container, 'position', 'position');
    await fillInput(container, 'note', 'test note', 'textarea');
    const submitButton = getSaveButton();
    userEvent.click(submitButton);

    const expectedValues = { ...initialValues.toApi() };
    expectedValues.position = 'position';
    expectedValues.note = 'test note';

    await waitFor(() => expect(onSubmit).toHaveBeenCalledWith(expectedValues));
  });
});
