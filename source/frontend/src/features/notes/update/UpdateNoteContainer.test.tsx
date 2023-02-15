import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { NoteTypes } from 'constants/index';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { mockNoteResponse } from 'mocks/mockNoteResponses';
import { Api_Note } from 'models/api/Note';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from 'utils/test-utils';

import { NoteForm } from '../models';
import { IUpdateNoteContainerProps, UpdateNoteContainer } from './UpdateNoteContainer';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);

const onSaveClick = jest.fn();
const onCancelClick = jest.fn();
const onSuccess = jest.fn();

const BASIC_PROPS: IUpdateNoteContainerProps = {
  isOpened: true,
  loading: false,
  type: NoteTypes.Activity,
  note: mockNoteResponse(1),
  onSuccess,
  onCancelClick,
  onSaveClick,
};

describe('UpdateNoteContainer component', () => {
  // render component under test
  const setup = (
    props: IUpdateNoteContainerProps = { ...BASIC_PROPS },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(<UpdateNoteContainer {...props} />, {
      ...renderOptions,
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
      history,
    });

    return {
      ...utils,
      getSaveButton: () => utils.getByText(/Save/i),
      getCancelButton: () => utils.getByText(/Cancel/i),
    };
  };

  beforeEach(() => {
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
  });

  afterEach(() => {
    mockAxios.reset();
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    setup();
    expect(document.body).toMatchSnapshot();
  });

  it('renders a spinner while loading', () => {
    const { getByTestId } = setup({ ...BASIC_PROPS, loading: true });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('renders the underlying form', () => {
    const { getByLabelText, getByText } = setup();

    const modalTitle = getByText(/Notes/i);
    const textarea = getByLabelText(/Type a note/i);

    expect(modalTitle).toBeVisible();
    expect(textarea).toBeVisible();
    expect(textarea.tagName).toBe('TEXTAREA');
  });

  it('should cancel form when Cancel button is clicked', () => {
    const { getCancelButton, getByText } = setup();

    expect(getByText(/Notes/i)).toBeVisible();
    act(() => userEvent.click(getCancelButton()));

    expect(onCancelClick).toBeCalled();
  });

  it('should save the form when Submit button is clicked', async () => {
    const formValues = NoteForm.fromApi(mockNoteResponse(1));
    formValues.note = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.';

    const { getSaveButton, findByLabelText } = setup({ ...BASIC_PROPS });

    const textarea = await findByLabelText(/Type a note/i);
    act(() => {
      userEvent.clear(textarea);
      userEvent.type(textarea, formValues.note as string);
    });

    mockAxios.onPut().reply(200, mockNoteResponse(1));
    await act(async () => userEvent.click(getSaveButton()));

    const axiosData: Api_Note = JSON.parse(mockAxios.history.put[0].data);
    const expectedValues = formValues.toApi();

    expect(mockAxios.history.put[0].url).toBe('/notes/activity/1');
    expect(axiosData).toEqual(expectedValues);
    expect(onSaveClick).toBeCalled();
    expect(onSuccess).toBeCalled();
  });

  it('should support updating notes from other entity types', async () => {
    const formValues = NoteForm.fromApi(BASIC_PROPS.note as Api_Note);
    formValues.note = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.';

    const { getSaveButton, findByLabelText } = setup({
      ...BASIC_PROPS,
      type: NoteTypes.Acquisition_File,
    });

    const textarea = await findByLabelText(/Type a note/i);
    act(() => {
      userEvent.clear(textarea);
      userEvent.type(textarea, formValues.note as string);
    });

    mockAxios.onPut().reply(200, mockNoteResponse(1));
    await act(async () => userEvent.click(getSaveButton()));

    expect(onSaveClick).toBeCalled();
    expect(mockAxios.history.put[0].url).toBe('/notes/acquisition_file/1');
    expect(onSuccess).toBeCalled();
  });
});
