import { Formik } from 'formik';
import { noop } from 'lodash';

import { Claims } from '@/constants/claims';
import { LeaseFormModel } from '@/features/leases/models';
import { fillInput, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { DepositNotes, IDepositNotesProps } from './DepositNotes';
jest.mock('@react-keycloak/web');

const onCancel = jest.fn();
const onSave = jest.fn();
const onEdit = jest.fn();

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
    const { asFragment } = await setup({
      lease: { ...new LeaseFormModel(), returnNotes: 'security deposit notes' },
      claims: [],
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the lease notes', async () => {
    const { getByDisplayValue } = await setup({
      lease: { ...new LeaseFormModel(), returnNotes: 'security deposit notes' },
      claims: [],
    });
    expect(getByDisplayValue('security deposit notes')).toBeVisible();
  });

  it('notes are only editable via correct claims', async () => {
    const { queryByTestId } = setup({
      lease: { ...new LeaseFormModel(), returnNotes: 'security deposit notes' },
      claims: [],
    });
    expect(queryByTestId('edit-notes')).toBeNull();
  });

  it('displays notes in read only form by default', async () => {
    const { container } = await setup({
      lease: {
        ...new LeaseFormModel(),
        returnNotes: 'security deposit notes',
      },
      disabled: true,
      claims: [Claims.LEASE_EDIT],
    });
    const input = container.querySelector(`textarea[name="returnNotes"]`);
    expect(input).toBeDisabled();
  });

  it('edit button allows notes field to be edited', async () => {
    const { getByTestId } = await setup({
      lease: { ...new LeaseFormModel(), returnNotes: 'security deposit notes' },
      claims: [Claims.LEASE_EDIT],
      disabled: true,
    });
    const editButton = getByTestId('edit-notes');
    userEvent.click(editButton);
    expect(onEdit).toHaveBeenCalled();
  });

  it('edited notes can be saved', async () => {
    const { container, getByText } = await setup({
      lease: { ...new LeaseFormModel(), returnNotes: 'security deposit notes' },
      claims: [Claims.LEASE_EDIT],
      disabled: false,
    });
    await fillInput(container, 'returnNotes', 'test note', 'textarea');
    const saveButton = getByText('Save');
    userEvent.click(saveButton);
    expect(onSave).toHaveBeenCalledWith('test note');
  });

  it('edited notes can be cancelled', async () => {
    const { getByText } = await setup({
      lease: { ...new LeaseFormModel(), returnNotes: 'security deposit notes' },
      claims: [Claims.LEASE_EDIT],
      disabled: false,
    });
    const cancelButton = getByText('Cancel');
    userEvent.click(cancelButton);
    expect(onCancel).toHaveBeenCalled();
  });
});
