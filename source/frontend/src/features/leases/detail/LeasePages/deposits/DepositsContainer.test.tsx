import { useKeycloak } from '@react-keycloak/web';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import noop from 'lodash/noop';

import { Claims } from '@/constants';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { getDefaultFormLease, LeaseFormModel } from '@/features/leases/models';
import { getMockDeposits } from '@/mocks/deposits.mock';
import {
  act,
  cleanup,
  fillInput,
  render,
  RenderOptions,
  RenderResult,
  userEvent,
  waitFor,
} from '@/utils/test-utils';

import DepositsContainer from './DepositsContainer';
import { FormLeaseDeposit } from './models/FormLeaseDeposit';
import { toTypeCode } from '@/utils/formUtils';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { getMockApiLease } from '@/mocks/lease.mock';

const onSuccessMock = vi.fn();

const mockAxios = new MockAdapter(axios);

const setup = (renderOptions: RenderOptions & { lease?: LeaseFormModel } = {}): RenderResult => {
  // render component under test
  const result = render(
    <LeaseStateContext.Provider
      value={{
        lease: renderOptions?.lease
          ? LeaseFormModel.toApi(renderOptions?.lease)
          : {
              ...getMockApiLease(),
            },
        setLease: noop,
      }}
    >
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? new LeaseFormModel()}>
        <DepositsContainer onSuccess={onSuccessMock} />
      </Formik>
    </LeaseStateContext.Provider>,
    {
      ...renderOptions,
      useMockAuthentication: true,
    },
  );
  return { ...result };
};

describe('DepositsContainer', () => {
  beforeEach(() => {
    Date.now = vi.fn().mockReturnValue(new Date('2020-10-15T18:33:37.000Z'));
  });
  afterEach(() => {
    mockAxios.reset();
    cleanup();
  });
  afterAll(() => {
    vi.restoreAllMocks();
  });
  it('renders as expected', () => {
    const result = setup({
      lease: {
        ...new LeaseFormModel(),
        returnNotes: 'Tenant no longer has a dog, deposit returned, less fee for carpet cleaning',
        securityDeposits: getMockDeposits().map(s => FormLeaseDeposit.fromApi(s)),
      },
    });
    expect(result.asFragment()).toMatchSnapshot();
  });

  it('saves deposit notes', async () => {
    mockAxios.onPost().reply(200, {});
    const { getByText, getByTestId, container } = setup({
      lease: {
        ...new LeaseFormModel(),
        statusTypeCode: ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE,
        id: 1,
        returnNotes: 'Tenant no longer has a dog, deposit returned, less fee for carpet cleaning',
        securityDeposits: getMockDeposits().map(s => FormLeaseDeposit.fromApi(s)),
      },
      claims: [Claims.LEASE_EDIT],
    });
    const editButton = getByTestId('edit-comments');
    await act(async () => userEvent.click(editButton));
    await fillInput(container, 'returnNotes', 'test note', 'textarea');
    const saveButton = getByText('Save');
    await act(async () => userEvent.click(saveButton));
    await waitFor(async () => {
      expect(mockAxios.history.put).toHaveLength(1);
    });
  });

  it('cancels an edited deposit note', async () => {
    const { getByText, getByTestId, container } = await setup({
      lease: {
        ...new LeaseFormModel(),
        statusTypeCode: ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE,
        id: 1,
        returnNotes: '',
        securityDeposits: getMockDeposits().map(s => FormLeaseDeposit.fromApi(s)),
      },
      claims: [Claims.LEASE_EDIT],
    });
    const editButton = getByTestId('edit-comments');
    await act(async () => userEvent.click(editButton));
    const noteField = await fillInput(container, 'returnNotes', 'test note', 'textarea');
    const cancelButton = getByText('Cancel');
    await act(async () => userEvent.click(cancelButton));
    await waitFor(async () => {
      expect(noteField.input).toHaveValue('');
    });
  });
});
