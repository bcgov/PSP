import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import React from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { renderAsync, RenderOptions, waitFor } from 'utils/test-utils';

import AddLeaseTenantNote, { IAddLeaseTenantNoteProps } from './AddLeaseTenantNote';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockAxios = new MockAdapter(axios);

describe('AddLeaseTenantNote component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IAddLeaseTenantNoteProps> & { initialValues?: { note: string } } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
        <AddLeaseTenantNote />
      </Formik>,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      component,
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
  });
  it('renders as expected', async () => {
    const { component } = await setup({});
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('displays existing note content if exists', async () => {
    const {
      component: { getByTitle, findByTestId },
    } = await setup({ initialValues: { note: 'test note' } });
    userEvent.click(getByTitle('notes'));
    const noteField = await findByTestId('note-field');
    expect(noteField).toHaveValue('test note');
  });

  it('saves note in formik field if modal saved', async () => {
    const {
      component: { getByTitle, getByText, findByTestId },
    } = await setup({});
    userEvent.click(getByTitle('notes'));

    const noteField = await findByTestId('note-field');
    userEvent.type(noteField, 'test note');
    const saveButton = getByText('Save');
    userEvent.click(saveButton);

    userEvent.click(getByTitle('notes'));
    expect(noteField).toHaveValue('test note');
  });

  it('does not save note if modal cancelled', async () => {
    const {
      component: { getByTitle, getByText, findByTestId },
    } = await setup({ initialValues: { note: '' } });
    userEvent.click(getByTitle('notes'));
    let noteField = await findByTestId('note-field');
    userEvent.type(noteField, 'test note');
    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);

    userEvent.click(getByTitle('notes'));
    noteField = await findByTestId('note-field');
    await waitFor(() => {
      expect(noteField).toHaveValue('');
    });
  });
});
