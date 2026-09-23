import { createMemoryHistory } from 'history';

import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';
import { render, RenderOptions } from '@/utils/test-utils';

import PropertyAssociationTabView, {
  IPropertyAssociationTabViewProps,
} from './PropertyAssociationTabView';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';
import { ApiGen_CodeTypes_LeaseStakeholderTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStakeholderTypes';
import { ApiGen_Concepts_LeaseAssociation } from '@/models/api/generated/ApiGen_Concepts_LeaseAssociation';
import { ApiGen_Concepts_LeaseStakeholder } from '@/models/api/generated/ApiGen_Concepts_LeaseStakeholder';
import { ApiGen_Concepts_Person } from '@/models/api/generated/ApiGen_Concepts_Person';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import Claims from '@/constants/claims';

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
      statusCode: 'ACTIVE',
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
      statusCode: ApiGen_CodeTypes_LeaseStatusTypes.ACTIVE,
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
      statusCode: 'ACTIVE',
      createdByGuid: null,
    },
  ],
  dispositionAssociations: null,
  managementAssociations: null,
};

describe('PropertyAssociationTabView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & IPropertyAssociationTabViewProps) => {
    const component = render(
      <PropertyAssociationTabView
        isLoading={renderOptions.isLoading}
        associations={renderOptions.associations}
        leaseAssociations={renderOptions.leaseAssociations}
      />,
      {
        claims: renderOptions?.claims,
        useMockAuthentication: true,
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
      leaseAssociations: [],
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('does not render a lease link when disabled', () => {
    const { getByText } = setup({
      isLoading: false,
      associations: fakeAssociations,
      claims: [],
      useMockAuthentication: true,
      leaseAssociations: [],
    });
    expect(getByText('951547254').nodeName).toBe('DIV');
  });

  it('renders a lease link when not disabled', () => {
    const { getByText } = setup({
      isLoading: false,
      associations: fakeAssociations,
      claims: [Claims.LEASE_VIEW],
      useMockAuthentication: true,
      leaseAssociations: [],
    });
    expect(getByText('951547254').nodeName).toBe('A');
  });

  it('does not render an acquisition file link when disabled', () => {
    const { getByText } = setup({
      isLoading: false,
      associations: fakeAssociations,
      leaseAssociations: [],
      claims: [],
      useMockAuthentication: true,
    });
    expect(getByText('95154').nodeName).toBe('DIV');
  });

  it('renders an acquisition file link when not disabled', () => {
    const { getByText } = setup({
      isLoading: false,
      associations: fakeAssociations,
      leaseAssociations: [],
      claims: [Claims.ACQUISITION_VIEW],
      useMockAuthentication: true,
    });
    expect(getByText('95154').nodeName).toBe('A');
  });

  it('renders only the highest priority lease tenants', () => {
    const { getByText, queryByText } = setup({
      isLoading: false,
      associations: fakeAssociations,
      leaseAssociations: [
        {
          id: 34,
          fileNumber: null,
          leaseExpiryDate: null,
          fileStatusTypeCode: null,
          appCreateUserGuid: null,
          appCreateUserid: null,
          stakeholders: [
            {
              leaseId: 34,
              person: { firstName: 'John', surname: 'Doe' } as ApiGen_Concepts_Person,
              stakeholderTypeCode: { id: ApiGen_CodeTypes_LeaseStakeholderTypes.ASGN },
              lessorType: { id: 'PER' },
            } as ApiGen_Concepts_LeaseStakeholder,
            {
              leaseId: 34,
              person: { firstName: 'John2', surname: 'Doe2' } as ApiGen_Concepts_Person,
              stakeholderTypeCode: { id: ApiGen_CodeTypes_LeaseStakeholderTypes.TEN },
              lessorType: { id: 'PER' },
            } as ApiGen_Concepts_LeaseStakeholder,
          ],
        } as ApiGen_Concepts_LeaseAssociation,
      ],
    });
    expect(getByText('John Doe')).toBeVisible();
    expect(queryByText('John2 Doe2')).toBeNull();
  });

  it('renders multiple lease tenants', () => {
    const { getByText } = setup({
      isLoading: false,
      associations: fakeAssociations,
      leaseAssociations: [
        {
          id: 34,
          fileNumber: null,
          leaseExpiryDate: null,
          fileStatusTypeCode: null,
          appCreateUserGuid: null,
          appCreateUserid: null,
          stakeholders: [
            {
              leaseId: 34,
              person: { firstName: 'John', surname: 'Doe' } as ApiGen_Concepts_Person,
              stakeholderTypeCode: { id: ApiGen_CodeTypes_LeaseStakeholderTypes.TEN },
              lessorType: { id: 'PER' },
            } as ApiGen_Concepts_LeaseStakeholder,
            {
              leaseId: 34,
              person: { firstName: 'John2', surname: 'Doe2' } as ApiGen_Concepts_Person,
              stakeholderTypeCode: { id: ApiGen_CodeTypes_LeaseStakeholderTypes.TEN },
              lessorType: { id: 'PER' },
            } as ApiGen_Concepts_LeaseStakeholder,
          ],
        } as ApiGen_Concepts_LeaseAssociation,
      ],
    });
    expect(getByText('John Doe', { exact: false })).toBeVisible();
    expect(getByText('John2 Doe2', { exact: false })).toBeVisible();
  });

  it('renders multiple lease tenants of different types', () => {
    const { getByText } = setup({
      isLoading: false,
      associations: fakeAssociations,
      leaseAssociations: [
        {
          id: 34,
          fileNumber: null,
          leaseExpiryDate: null,
          fileStatusTypeCode: null,
          appCreateUserGuid: null,
          appCreateUserid: null,
          stakeholders: [
            {
              leaseId: 34,
              organization: { name: 'Org' } as ApiGen_Concepts_Organization,
              stakeholderTypeCode: { id: ApiGen_CodeTypes_LeaseStakeholderTypes.TEN },
              lessorType: { id: 'ORG' },
            } as ApiGen_Concepts_LeaseStakeholder,
            {
              leaseId: 34,
              person: { firstName: 'John2', surname: 'Doe2' } as ApiGen_Concepts_Person,
              stakeholderTypeCode: { id: ApiGen_CodeTypes_LeaseStakeholderTypes.TEN },
              lessorType: { id: 'PER' },
            } as ApiGen_Concepts_LeaseStakeholder,
          ],
        } as ApiGen_Concepts_LeaseAssociation,
      ],
    });
    expect(getByText('Org', { exact: false })).toBeVisible();
    expect(getByText('John2 Doe2', { exact: false })).toBeVisible();
  });

  it('renders lease expiry', () => {
    const { getByText } = setup({
      isLoading: false,
      associations: fakeAssociations,
      leaseAssociations: [
        {
          id: 34,
          fileNumber: null,
          leaseExpiryDate: '2030-07-07',
          fileStatusTypeCode: null,
          appCreateUserGuid: null,
          appCreateUserid: null,
          stakeholders: [],
        } as ApiGen_Concepts_LeaseAssociation,
      ],
    });
    expect(getByText('Jul 7, 2030')).toBeVisible();
  });
});
