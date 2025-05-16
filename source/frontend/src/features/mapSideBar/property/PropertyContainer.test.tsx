import { createMemoryHistory } from 'history';

import { Claims } from '@/constants';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { cleanup, render, RenderOptions, waitForEffects } from '@/utils/test-utils';

import PropertyContainer, { IPropertyContainerProps } from './PropertyContainer';
import { useApiLeases } from '@/hooks/pims-api/useApiLeases';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { useApiPropertyOperation } from '@/hooks/pims-api/useApiPropertyOperation';
import { useLeaseStakeholderRepository } from '@/hooks/repositories/useLeaseStakeholderRepository';
import { noop } from 'lodash';

// mock keycloak auth library
const getApiLeaseFn = vi.fn();

vi.mock('@/hooks/pims-api/useApiLeases');
vi.mocked(useApiLeases).mockImplementation(
  () =>
    ({
      getApiLease: getApiLeaseFn,
      getLastUpdatedByApi: vi.fn(),
    } as unknown as ReturnType<typeof useApiLeases>),
);
vi.mock('@/hooks/pims-api/useApiPropertyOperation');
const getPropertyOperationsFn = vi.fn();
vi.mocked(useApiPropertyOperation).mockImplementation(
  () =>
    ({
      getPropertyOperationsApi: getPropertyOperationsFn,
    } as unknown as ReturnType<typeof useApiPropertyOperation>),
);

const getLeaseStakeholdersFn = vi.fn();
vi.mock('@/hooks/repositories/useLeaseStakeholderRepository');
vi.mocked(useLeaseStakeholderRepository).mockImplementation(
  () =>
    ({
      getLeaseStakeholders: { execute: getLeaseStakeholdersFn },
    } as unknown as ReturnType<typeof useLeaseStakeholderRepository>),
);

describe('PropertyContainer component', () => {
  const history = createMemoryHistory();
  const storeState = {
    [lookupCodesSlice.name]: { lookupCodes: mockLookups },
  };

  const setup = (renderOptions: RenderOptions & IPropertyContainerProps) => {
    // render component under test
    const utils = render(<PropertyContainer {...renderOptions}  />, {
      ...renderOptions,
      history,
      store: { ...renderOptions.store, ...storeState },
      claims: renderOptions.claims ?? [],
    });

    return { ...utils };
  };

  afterEach(() => {
    cleanup();
  });

  afterAll(() => {
    vi.restoreAllMocks();
  });

  it('hides the management tab if the user does not have permission', async () => {
    const { queryByText } = setup({
      claims: [],
      composedPropertyState: { apiWrapper: { response: {} }, id: 1 } as any,
      onChildSuccess: noop
    });

    expect(queryByText('Management')).toBeNull();
  });

  it('displays the management tab if the user has permission', async () => {
    const { getByText } = setup({
      claims: [Claims.MANAGEMENT_VIEW],
      composedPropertyState: { apiWrapper: { response: {} }, id: 1 } as any,
      onChildSuccess: noop
    });

    expect(getByText('Management')).toBeVisible();
  });

  it('does not request lease information if user is not authorized', async () => {
    setup({
      claims: [],
      composedPropertyState: {
        propertyAssociationWrapper: {
          response: {
            id: 1,
            leaseAssociations: [
              {
                id: 34,
                fileNumber: '951547254',
                fileName: '-',
                createdDateTime: '2022-05-13T11:51:29.23',
                createdBy: 'Lease Seed Data',
                status: 'Active',
                statusCode: ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE,
                createdByGuid: null,
              },
            ],
            researchAssociations: [],
            acquisitionAssociations: [],
            dispositionAssociations: [],
          },
        },
        id: 1,
      } as any,
      onChildSuccess: noop
    });
    await waitForEffects();

    expect(getApiLeaseFn).not.toHaveBeenCalled();
  });

  it('requests lease information if user is authorized', async () => {
    setup({
      claims: [Claims.LEASE_VIEW],
      composedPropertyState: {
        propertyAssociationWrapper: {
          response: {
            id: 1,
            leaseAssociations: [
              {
                id: 34,
                fileNumber: '951547254',
                fileName: '-',
                createdDateTime: '2022-05-13T11:51:29.23',
                createdBy: 'Lease Seed Data',
                status: 'Active',
                statusCode: ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE,
                createdByGuid: null,
              },
            ],
            researchAssociations: [],
            acquisitionAssociations: [],
            dispositionAssociations: [],
          },
        },
        id: 1,
      } as any,
      onChildSuccess: noop
    });
    await waitForEffects();

    expect(getApiLeaseFn).toHaveBeenCalled();
  });
});
