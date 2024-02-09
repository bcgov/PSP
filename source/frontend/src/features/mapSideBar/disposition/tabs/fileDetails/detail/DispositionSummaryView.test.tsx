import Claims from '@/constants/claims';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { Api_DispositionFile } from '@/models/api/DispositionFile';
import { act, cleanup, render, RenderOptions, userEvent, waitForEffects } from '@/utils/test-utils';

import DispositionSummaryView, { IDispositionSummaryViewProps } from './DispositionSummaryView';

// mock auth library
jest.mock('@react-keycloak/web');

const onEdit = jest.fn();

const mockDispositionFileApi = mockDispositionFileResponse() as unknown as Api_DispositionFile;

describe('DispositionSummaryView component', () => {
  // render component under test
  const setup = (
    props: Partial<IDispositionSummaryViewProps>,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <DispositionSummaryView dispositionFile={props.dispositionFile} onEdit={onEdit} />,
      {
        useMockAuthentication: true,
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  beforeEach(() => {});

  afterEach(() => {
    cleanup();
    jest.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({
      dispositionFile: mockDispositionFileApi,
    });
    await waitForEffects();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the edit button for users with disposition edit permissions', async () => {
    const { getByTitle } = setup(
      { dispositionFile: mockDispositionFileApi },
      { claims: [Claims.DISPOSITION_EDIT] },
    );
    await waitForEffects();
    const editButton = getByTitle('Edit disposition file');
    expect(editButton).toBeVisible();
    await act(async () => userEvent.click(editButton));
    expect(onEdit).toHaveBeenCalled();
  });

  it('does not render the edit button for users that do not have disposition edit permissions', async () => {
    const { queryByTitle } = setup({ dispositionFile: mockDispositionFileApi }, { claims: [] });
    await waitForEffects();
    const editButton = queryByTitle('Edit disposition file');
    expect(editButton).toBeNull();
  });

  it('renders historical file number', async () => {
    const mockResponse = mockDispositionFileApi;
    const { getByText } = setup({ dispositionFile: mockResponse }, { claims: [] });
    await waitForEffects();
    expect(getByText('FILE_REFERENCE 8128827 3EAD56A')).toBeVisible();
  });

  it('renders other initiating document field when iniating document is OTHER', async () => {
    const { getByText } = setup(
      {
        dispositionFile: {
          ...mockDispositionFileApi,
          initiatingDocumentTypeCode: { id: 'OTHER' },
        },
      },
      { claims: [] },
    );
    await waitForEffects();
    expect(getByText(/Other \(initiating document\)/g, { exact: false })).toBeVisible();
  });

  it('renders other other disposition type field when disposition type is OTHER', async () => {
    const { getByText } = setup(
      {
        dispositionFile: {
          ...mockDispositionFileApi,
          dispositionTypeCode: { id: 'OTHER' },
        },
      },
      { claims: [] },
    );
    await waitForEffects();
    expect(getByText(/Other \(disposition type\)/g, { exact: false })).toBeVisible();
  });

  it('renders disposition team member person', async () => {
    const apiMock = mockDispositionFileResponse() as unknown as Api_DispositionFile;
    const { findByText } = setup(
      {
        dispositionFile: {
          ...apiMock,
          dispositionTeam: [
            {
              id: 1,
              dispositionFileId: 1,
              personId: 1,
              person: {
                id: 1,
                surname: 'Smith',
                firstName: 'Bob',
                middleNames: 'Billy',
                personOrganizations: [],
                personAddresses: [],
                contactMethods: [],
                rowVersion: 2,
              },
              teamProfileType: {
                id: 'NEGOTAGENT',
                description: 'Negotiation agent',
                isDisabled: false,
              },
              rowVersion: 2,
            },
          ],
        },
      },
      { claims: [] },
    );
    await waitForEffects();
    expect(await findByText(/Negotiation agent/)).toBeVisible();
    expect(await findByText(/Bob Billy Smith/)).toBeVisible();
  });

  it('renders disposition team member organization', async () => {
    const apiMock = mockDispositionFileApi;
    const { findByText } = setup(
      {
        dispositionFile: {
          ...apiMock,
          dispositionTeam: [
            {
              id: 1,
              dispositionFileId: 1,
              organizationId: 1,
              organization: {
                id: 1,
                name: 'Test Organization',
                alias: 'ABC Inc',
                incorporationNumber: '1234',
                comment: '',
              },
              teamProfileType: {
                id: 'NEGOTAGENT',
                description: 'Negotiation agent',
                isDisabled: false,
              },
              rowVersion: 2,
            },
          ],
        },
      },
      { claims: [] },
    );
    await waitForEffects();
    expect(await findByText(/Negotiation agent/)).toBeVisible();
    expect(await findByText(/Test Organization/)).toBeVisible();
    expect(await findByText(/No contacts available/)).toBeVisible();
  });

  it('renders disposition team member organization and primary contact', async () => {
    const apiMock = mockDispositionFileApi;
    const { findByText } = setup(
      {
        dispositionFile: {
          ...apiMock,
          dispositionTeam: [
            {
              id: 1,
              dispositionFileId: 1,
              organizationId: 1,
              organization: {
                id: 1,
                name: 'Test Organization',
                alias: 'ABC Inc',
                incorporationNumber: '1234',
                comment: '',
              },
              primaryContactId: 1,
              primaryContact: {
                id: 1,
                surname: 'Smith',
                firstName: 'Bob',
                middleNames: 'Billy',
                personOrganizations: [],
                personAddresses: [],
                contactMethods: [],
                rowVersion: 2,
              },
              teamProfileType: {
                id: 'NEGOTAGENT',
                description: 'Negotiation agent',
                isDisabled: false,
              },
              rowVersion: 2,
            },
          ],
        },
      },
      { claims: [] },
    );
    await waitForEffects();
    expect(await findByText(/Negotiation agent/)).toBeVisible();
    expect(await findByText(/Test Organization/)).toBeVisible();
    expect(await findByText(/Primary contact/)).toBeVisible();
    expect(await findByText(/Bob Billy Smith/)).toBeVisible();
  });

  it('renders the project and product', async () => {
    const { queryByTestId } = setup({
      dispositionFile: mockDispositionFileApi,
    });

    await waitForEffects();
    expect(queryByTestId('dsp-project')).toHaveTextContent('00048 - CLAIMS');
    expect(queryByTestId('dsp-product')).toHaveTextContent('00055 AVALANCHE & PROGRAM REVIEW');
  });
});
