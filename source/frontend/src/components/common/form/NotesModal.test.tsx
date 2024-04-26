import userEvent from '@testing-library/user-event';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import NotesModal, { INotesModalProps } from '@/components/common/form/NotesModal';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, renderAsync, RenderOptions, waitFor } from '@/utils/test-utils';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockAxios = new MockAdapter(axios);

describe('NotesModal component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<INotesModalProps> & { initialValues?: { note: string } } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
        <NotesModal notesLabel="test label" title="test title" />
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
    await act(async () => {
      userEvent.click(getByTitle('notes'));
    });
    const noteField = await findByTestId('note-field');
    expect(noteField).toHaveValue('test note');
  });

  it('saves note in formik field if modal saved', async () => {
    const {
      component: { getByTitle, getByText, findByTestId },
    } = await setup({});
    await act(async () => {
      userEvent.click(getByTitle('notes'));
    });

    const noteField = await findByTestId('note-field');
    await act(async () => {
      userEvent.type(noteField, 'test note');
    });
    const saveButton = getByText('Yes');
    await act(async () => {
      userEvent.click(saveButton);
    });

    await act(async () => {
      userEvent.click(getByTitle('notes'));
    });

    expect(noteField).toHaveValue('test note');
  });

  it('does not save note if modal cancelled', async () => {
    const {
      component: { getByTitle, getByText, findByTestId },
    } = await setup({ initialValues: { note: '' } });
    await act(async () => {
      userEvent.click(getByTitle('notes'));
    });
    let noteField = await findByTestId('note-field');
    await act(async () => {
      userEvent.type(noteField, 'test note');
    });
    const cancelButton = getByText('No');
    await act(async () => {
      userEvent.click(cancelButton);
    });

    await act(async () => {
      userEvent.click(getByTitle('notes'));
    });
    noteField = await findByTestId('note-field');
    await act(async () => {
      await waitFor(() => {
        expect(noteField).toHaveValue('');
      });
    });
  });
});
