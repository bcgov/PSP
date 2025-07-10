import { Formik } from 'formik';
import noop from 'lodash/noop';

import { IContactSearchResult } from '@/interfaces';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import ContactInputView, { IContactInputViewProps } from './ContactInputView';

const setSelectedRows = vi.fn();
const setShowContactManager = vi.fn();
const clear = vi.fn();

describe('ContactInputView component', () => {
  // render component under test
  const setup = (
    props: IContactInputViewProps & { initialValues?: { test: Partial<IContactSearchResult> } },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <Formik
        onSubmit={noop}
        initialValues={
          props.initialValues ??
          ({ test: { id: '234', firstName: 'blah', surname: 'blah' } } as any)
        }
      >
        <ContactInputView {...{ ...props }} />
      </Formik>,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        claims: [],
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {});

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({
      contactManagerProps: { display: false, setSelectedRows: setSelectedRows, selectedRows: [] },
      field: 'test',
      setShowContactManager: setShowContactManager,
      onClear: clear,
      canEditDetails: true,
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays default text', async () => {
    const { getByText } = setup({
      contactManagerProps: { display: false, setSelectedRows: setSelectedRows, selectedRows: [] },
      field: 'test',
      setShowContactManager: setShowContactManager,
      onClear: clear,
      canEditDetails: true,
    });
    expect(getByText('Select from contacts')).toBeInTheDocument();
  });

  it('displays person name', async () => {
    const { getByText } = setup({
      contactManagerProps: { display: false, setSelectedRows: setSelectedRows, selectedRows: [] },
      field: 'test',
      setShowContactManager: setShowContactManager,
      onClear: clear,
      initialValues: { test: { firstName: 'blah', surname: 'blah2', personId: 1 } },
      canEditDetails: true,
    });
    expect(getByText('blah blah2')).toBeInTheDocument();
  });

  it('displays organization text', async () => {
    const { getByText } = setup({
      contactManagerProps: { display: false, setSelectedRows: setSelectedRows, selectedRows: [] },
      field: 'test',
      setShowContactManager: setShowContactManager,
      onClear: clear,
      initialValues: { test: { organizationName: 'blah org', organizationId: 1 } },
      canEditDetails: true,
    });
    expect(getByText('blah org')).toBeInTheDocument();
  });

  it('calls setShowContactManager when icon clicked', async () => {
    const { getByTitle } = setup({
      contactManagerProps: { display: false, setSelectedRows: setSelectedRows, selectedRows: [] },
      field: 'test',
      setShowContactManager: setShowContactManager,
      onClear: clear,
      canEditDetails: true,
    });
    const icon = getByTitle('Select Contact');
    await act(async () => userEvent.click(icon));
    expect(setShowContactManager).toHaveBeenCalled();
  });

  it('calls onClear when cancel clicked', async () => {
    const { queryByTitle } = setup({
      contactManagerProps: { display: false, setSelectedRows: setSelectedRows, selectedRows: [] },
      field: 'test',
      setShowContactManager: setShowContactManager,
      onClear: clear,
      canEditDetails: true,
    });
    const icon = queryByTitle('remove');
    await act(async () => userEvent.click(icon));
    expect(clear).toHaveBeenCalled();
  });

  it('remove button not present when no contact selected', async () => {
    const { queryByTitle } = setup({
      contactManagerProps: { display: false, setSelectedRows: setSelectedRows, selectedRows: [] },
      field: 'invalid',
      setShowContactManager: setShowContactManager,
      onClear: clear,
      canEditDetails: true,
    });
    const icon = queryByTitle('remove');
    expect(icon).not.toBeInTheDocument();
  });
});
