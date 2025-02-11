import { screen } from '@testing-library/react';
import { describe, it, expect, vi } from 'vitest';
import { LeasePageNames } from '@/features/mapSideBar/lease/LeaseContainer';
import { LeaseStateContext } from '../../context/LeaseContext';
import LeaseViewPageForm, { ILeasePageFormProps } from './LeasePageForm';
import { Claims, Roles } from '@/constants';
import { getMockApiLease } from '@/mocks/lease.mock';
import noop from 'lodash/noop';
import { RenderOptions, render } from '@/utils/test-utils';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { toTypeCode } from '@/utils/formUtils';

const setup = (
  renderOptions: RenderOptions & {
    props?: Partial<ILeasePageFormProps>;
    lease?: ApiGen_Concepts_Lease;
  } = {},
) => {
  const defaultProps: ILeasePageFormProps = {
    leasePageName: LeasePageNames.DETAILS,
    isEditing: false,
    onEdit: vi.fn(),
    ...renderOptions.props,
  };

  return render(
    <LeaseStateContext.Provider
      value={{ lease: renderOptions?.lease ?? getMockApiLease(), setLease: noop }}
    >
      <LeaseViewPageForm {...defaultProps} />
    </LeaseStateContext.Provider>,
    {
      ...renderOptions,
      claims: renderOptions?.claims ?? [Claims.LEASE_ADD],
      roles: renderOptions.roles,
    },
  );
};

describe('LeaseViewPageForm', () => {
  it('renders', () => {
    setup();
  });

  it('renders edit button when user has permission', () => {
    setup({
      props: {
        leasePageName: LeasePageNames.DETAILS,
        isEditing: false,
      },
      roles: [Roles.SYSTEM_ADMINISTRATOR],
      claims: [Claims.LEASE_EDIT],
      lease: {
        ...getMockApiLease(),
        fileStatusTypeCode: toTypeCode(ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE),
      },
    });

    expect(screen.getByTitle('lease-edit')).toBeInTheDocument();
  });

  it('renders tooltip when user does not have permission to edit', () => {
    setup({
      props: {
        leasePageName: LeasePageNames.DETAILS,
        isEditing: false,
      },
      roles: [],
    });

    expect(
      screen.getByTestId(/tooltip-icon-lease-actions-cannot-edit-tooltip/i),
    ).toBeInTheDocument();
  });

  it('renders tooltip when user does not have permission to edit due to ', () => {
    setup({
      props: {
        leasePageName: LeasePageNames.DETAILS,
        isEditing: false,
      },
      roles: [],
      lease: {
        ...getMockApiLease(),
        fileStatusTypeCode: toTypeCode(ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED),
      },
    });

    expect(
      screen.getByTestId(/tooltip-icon-lease-actions-cannot-edit-tooltip/i),
    ).toBeInTheDocument();
  });
});
