import { createMemoryHistory } from 'history';

import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';
import { render, RenderOptions } from '@/utils/test-utils';

import PropertyAssociationTabView, {
  IPropertyAssociationTabViewProps,
} from './PropertyAssociationTabView';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { ApiGen_Base_CodeType } from '@/models/api/generated/ApiGen_Base_CodeType';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';

const history = createMemoryHistory();

const fakeAssociations: ApiGen_Concepts_PropertyAssociations = {
  id: 168,
  pid: '26934426',
  acquisitionAssociations: [
    {
      id: 45,
      fileNumber: '95154',
      fileName: '-',
      createdDateTime: '2022-05-13T11:51:29.23',
      createdBy: 'Acquisition File Data',
      status: 'Active',
      createdByGuid: null,
    },
  ],
  leaseAssociations: [
    {
      id: 34,
      fileNumber: '951547254',
      fileName: '-',
      createdDateTime: '2022-05-13T11:51:29.23',
      createdBy: 'Lease Seed Data',
      status: 'Active',
      createdByGuid: null,
    },
  ],
  researchAssociations: [
    {
      id: 1,
      fileNumber: 'R-1',
      fileName: 'R-1',
      createdDateTime: '2022-05-17T19:49:16.647',
      createdBy: 'admin',
      status: 'Active',
      createdByGuid: null,
    },
  ],
  dispositionAssociations: null,
};

describe('PropertyAssociationTabView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & IPropertyAssociationTabViewProps) => {
    const component = render(
      <PropertyAssociationTabView
        isLoading={renderOptions.isLoading}
        associations={renderOptions.associations}
        associatedLeaseStakeholders={renderOptions.associatedLeaseStakeholders}
        associatedLeaseRenewals={renderOptions.associatedLeaseRenewals}
        associatedLeases={renderOptions.associatedLeases}
      />,
      {
        history,
      },
    );

    return {
      ...component,
    };
  };

  it('renders as expected when provided valid data object', () => {
    const { asFragment } = setup({
      isLoading: false,
      associations: fakeAssociations,
      associatedLeases: [],
      associatedLeaseRenewals: [],
      associatedLeaseStakeholders: [],
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders only the highest priority lease tenants', () => {
    const { getByText, queryByText } = setup({
      isLoading: false,
      associations: fakeAssociations,
      associatedLeases: [],
      associatedLeaseStakeholders: [
        {
          leaseId: 34,
          person: { firstName: 'John', surname: 'Doe' } as ApiGen_Concepts_Person,
          stakeholderTypeCode: { id: 'ASGN' } as ApiGen_Base_CodeType<string>,
          lessorType: { id: 'PER' } as ApiGen_Base_CodeType<string>,
        } as ApiGen_Concepts_LeaseStakeholder,
        {
          leaseId: 34,
          person: { firstName: 'John2', surname: 'Doe2' } as ApiGen_Concepts_Person,
          stakeholderTypeCode: { id: 'TEN' } as ApiGen_Base_CodeType<string>,
          lessorType: { id: 'PER' } as ApiGen_Base_CodeType<string>,
        } as ApiGen_Concepts_LeaseStakeholder,
      ],
      associatedLeaseRenewals: [],
    });
    expect(getByText('John Doe')).toBeVisible();
    expect(queryByText('John2 Doe2')).toBeNull();
  });

  it('renders multiple lease tenants', () => {
    const { getByText } = setup({
      isLoading: false,
      associations: fakeAssociations,
      associatedLeases: [],
      associatedLeaseStakeholders: [
        {
          leaseId: 34,
          person: { firstName: 'John', surname: 'Doe' } as ApiGen_Concepts_Person,
          stakeholderTypeCode: { id: 'ASGN' } as ApiGen_Base_CodeType<string>,
          lessorType: { id: 'PER' } as ApiGen_Base_CodeType<string>,
        } as ApiGen_Concepts_LeaseStakeholder,
        {
          leaseId: 34,
          person: { firstName: 'John2', surname: 'Doe2' } as ApiGen_Concepts_Person,
          stakeholderTypeCode: { id: 'ASGN' } as ApiGen_Base_CodeType<string>,
          lessorType: { id: 'PER' } as ApiGen_Base_CodeType<string>,
        } as ApiGen_Concepts_LeaseStakeholder,
      ],
      associatedLeaseRenewals: [],
    });
    expect(getByText('John Doe', { exact: false })).toBeVisible();
    expect(getByText('John2 Doe2', { exact: false })).toBeVisible();
  });

  it('renders multiple lease tenants of different types', () => {
    const { getByText } = setup({
      isLoading: false,
      associations: fakeAssociations,
      associatedLeases: [],
      associatedLeaseStakeholders: [
        {
          leaseId: 34,
          organization: { name: 'Org' } as ApiGen_Concepts_Organization,
          stakeholderTypeCode: { id: 'ASGN' } as ApiGen_Base_CodeType<string>,
          lessorType: { id: 'ORG' } as ApiGen_Base_CodeType<string>,
        } as ApiGen_Concepts_LeaseStakeholder,
        {
          leaseId: 34,
          person: { firstName: 'John2', surname: 'Doe2' } as ApiGen_Concepts_Person,
          stakeholderTypeCode: { id: 'ASGN' } as ApiGen_Base_CodeType<string>,
          lessorType: { id: 'PER' } as ApiGen_Base_CodeType<string>,
        } as ApiGen_Concepts_LeaseStakeholder,
      ],
      associatedLeaseRenewals: [],
    });
    expect(getByText('Org', { exact: false })).toBeVisible();
    expect(getByText('John2 Doe2', { exact: false })).toBeVisible();
  });

  it('renders lease expiry', () => {
    const { getByText } = setup({
      isLoading: false,
      associations: fakeAssociations,
      associatedLeases: [{ id: 34, expiryDate: '2024-01-01' } as ApiGen_Concepts_Lease],
      associatedLeaseStakeholders: [],
      associatedLeaseRenewals: [
        {
          id: 1,
          leaseId: 34,
          commencementDt: '',
          expiryDt: '2030-07-07',
          isExercised: true,
          renewalNote: '',
          lease: undefined,
          appCreateTimestamp: '',
          appLastUpdateTimestamp: '',
          appLastUpdateUserid: '',
          appCreateUserid: '',
          appLastUpdateUserGuid: '',
          appCreateUserGuid: '',
          rowVersion: 0,
        },
      ],
    });
    expect(getByText('Jul 7, 2030')).toBeVisible();
  });
});
