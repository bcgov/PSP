import { LeasePageNames } from '@/features/mapSideBar/lease/LeaseContainer';
import LeaseViewPageForm, { ILeasePageFormProps } from './LeasePageForm';
import noop from 'lodash/noop';

import { RenderOptions, render } from '@/utils/test-utils';
import { Claims } from '@/constants/claims';
import Roles from '@/constants/roles';

import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { getDefaultFormLease, LeaseFormModel } from '@/features/leases/models';

const onEdit = vi.fn();

describe('LeasePageForm component', () => {
  // render component under test
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<ILeasePageFormProps>;
    } = {},
  ) => {
    const utils = render(
      <LeaseStateContext.Provider
        value={{
          lease: LeaseFormModel.toApi({ ...getDefaultFormLease(), id: 1 }),
          setLease: noop,
        }}
      >
        <LeaseViewPageForm
          onEdit={onEdit}
          leasePageName={renderOptions.props?.leasePageName ?? LeasePageNames.DETAILS}
          isEditing={false}
        >
          <p>Dummy child</p>
        </LeaseViewPageForm>
      </LeaseStateContext.Provider>,
      {
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [],
        roles: renderOptions?.roles ?? [Roles.SYSTEM_ADMINISTRATOR],
        ...renderOptions,
      },
    );

    return {
      ...utils,
      getEditButton: (pageName: string) =>
        utils.container.querySelector(`button#edit-${pageName}-btn`) as HTMLElement,
    };
  };

  it('matches snapshot', async () => {
    const { asFragment } = await setup({ claims: [Claims.LEASE_EDIT] });

    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the child and edit button', async () => {
    const { getByText, getEditButton } = await setup({ claims: [Claims.LEASE_EDIT] });

    expect(getByText('Dummy child')).toBeInTheDocument();
    expect(getEditButton('details')).toBeInTheDocument();
  });
});
