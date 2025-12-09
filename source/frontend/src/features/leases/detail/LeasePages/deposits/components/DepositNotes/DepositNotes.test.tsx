import { Formik } from 'formik';
import noop from 'lodash/noop';

import { Claims } from '@/constants/claims';
import { LeaseFormModel } from '@/features/leases/models';
import {
  act,
  fillInput,
  queryByTestId,
  render,
  RenderOptions,
  userEvent,
} from '@/utils/test-utils';

import { DepositNotes, IDepositNotesProps } from './DepositNotes';

const onCancel = vi.fn();
const onSave = vi.fn();
const onEdit = vi.fn();

const setup = (
  renderOptions: RenderOptions & Partial<IDepositNotesProps> & { lease?: LeaseFormModel } = {},
) => {
  // render component under test
  const result = render(
    <Formik onSubmit={noop} initialValues={renderOptions.lease ?? new LeaseFormModel()}>
      <DepositNotes
        disabled={renderOptions.disabled}
        onCancel={onCancel}
        onSave={onSave}
        onEdit={onEdit}
        isFileFinalStatus={renderOptions.isFileFinalStatus}
      />
    </Formik>,
    {
      ...renderOptions,
    },
  );

  return { ...result };
};

describe('DepositNotes component', () => {
  beforeEach(() => {
    onSave.mockReset();
    onCancel.mockReset();
    onEdit.mockReset();
  });
  it('renders as expected', async () => {
    const leaseForm = new LeaseFormModel();
    leaseForm.returnNotes = 'security deposit notes';
    const { asFragment } = await setup({
      lease: leaseForm,
      claims: [],
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the lease notes', async () => {
    const leaseForm = new LeaseFormModel();
    leaseForm.returnNotes = 'security deposit notes';
    const { getByDisplayValue } = await setup({
      lease: leaseForm,
      claims: [],
    });
    expect(getByDisplayValue('security deposit notes')).toBeVisible();
  });

  it('notes are only editable via correct claims', async () => {
    const leaseForm = new LeaseFormModel();
    leaseForm.returnNotes = 'security deposit notes';
    const { queryByTestId } = setup({
      lease: leaseForm,
      claims: [],
    });
    expect(queryByTestId('edit-notes')).toBeNull();
  });

  it('displays notes in read only form by default', async () => {
    const leaseForm = new LeaseFormModel();
    leaseForm.returnNotes = 'security deposit notes';
    const { container } = await setup({
      lease: leaseForm,
      disabled: true,
      claims: [Claims.LEASE_EDIT],
    });
    const input = container.querySelector(`textarea[name="returnNotes"]`);
    expect(input).toBeDisabled();
  });

  it('edit button allows notes field to be edited', async () => {
    const leaseForm = new LeaseFormModel();
    leaseForm.returnNotes = 'security deposit notes';
    const { getByTestId } = await setup({
      lease: leaseForm,
      claims: [Claims.LEASE_EDIT],
      disabled: true,
    });
    const editButton = getByTestId('edit-comments');
    await act(async () => userEvent.click(editButton));
    expect(onEdit).toHaveBeenCalled();
  });

  it('edited notes can be saved', async () => {
    const leaseForm = new LeaseFormModel();
    leaseForm.returnNotes = 'security deposit notes';
    const { container, getByText } = await setup({
      lease: leaseForm,
      claims: [Claims.LEASE_EDIT],
      disabled: false,
    });
    await fillInput(container, 'returnNotes', 'test note', 'textarea');
    const saveButton = getByText('Save');
    await act(async () => userEvent.click(saveButton));
    expect(onSave).toHaveBeenCalledWith('test note');
  });

  it('edited notes can be cancelled', async () => {
    const leaseForm = new LeaseFormModel();
    leaseForm.returnNotes = 'security deposit notes';
    const { getByText } = await setup({
      lease: leaseForm,
      claims: [Claims.LEASE_EDIT],
      disabled: false,
    });
    const cancelButton = getByText('Cancel');
    await act(async () => userEvent.click(cancelButton));
    expect(onCancel).toHaveBeenCalled();
  });

  it('edit button does not display if file in final state', async () => {
    const leaseForm = new LeaseFormModel();
    leaseForm.returnNotes = 'security deposit notes';
    const { queryByTestId } = await setup({
      lease: leaseForm,
      claims: [Claims.LEASE_EDIT],
      isFileFinalStatus: true,
    });
    const editButton = queryByTestId('edit-comments');
    expect(editButton).toBeNull();
  });
});
