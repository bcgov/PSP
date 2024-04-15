import { act } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useProperties } from '@/hooks/repositories/useProperties';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { getMockPerson } from '@/mocks/contacts.mock';
import { getMockOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';

import { useGenerateH0443 } from './useGenerateH0443';

const getPropertiesFn = vi.fn<ApiGen_Concepts_Property[], any[]>();
const generateFn = vi.fn();
const getAcquisitionFileFn = vi.fn();
const getPersonConceptFn = vi.fn();
const getOrganizationConceptFn = vi.fn();

vi.mock('@/features/documents/hooks/useDocumentGenerationRepository');
vi.mocked(useDocumentGenerationRepository).mockImplementation(
  () =>
    ({
      generateDocumentDownloadWrappedRequest: generateFn,
    } as unknown as ReturnType<typeof useDocumentGenerationRepository>),
);

vi.mock('@/hooks/repositories/useAcquisitionProvider');
vi.mocked(useAcquisitionProvider).mockImplementation(
  () =>
    ({
      getAcquisitionFile: { execute: getAcquisitionFileFn },
    } as unknown as ReturnType<typeof useAcquisitionProvider>),
);

vi.mock('@/hooks/repositories/useProperties');
vi.mocked(useProperties).mockImplementation(
  () =>
    ({
      getMultiplePropertiesById: { execute: getPropertiesFn },
    } as unknown as ReturnType<typeof useProperties>),
);

vi.mock('@/hooks/pims-api/useApiContacts');
vi.mocked(useApiContacts).mockImplementation(
  () =>
    ({
      getPersonConcept: getPersonConceptFn,
      getOrganizationConcept: getOrganizationConceptFn,
    } as unknown as ReturnType<typeof useApiContacts>),
);

let currentStore: MockStoreEnhanced<any, {}>;
const mockStore = configureMockStore([thunk]);
const getStore = (values?: any) => {
  currentStore = mockStore(values ?? {});
  return currentStore;
};
const getWrapper =
  (store: any) =>
  ({ children }: any) =>
    <Provider store={store}>{children}</Provider>;

const setup = (params?: {
  storeValues?: any;
  acquisitionResponse?: ApiGen_Concepts_AcquisitionFile;
}) => {
  var acquisitionResponse = mockAcquisitionFileResponse();
  if (params?.acquisitionResponse !== undefined) {
    acquisitionResponse = params.acquisitionResponse;
  }

  getAcquisitionFileFn.mockReturnValue(acquisitionResponse);

  const { result } = renderHook(useGenerateH0443, {
    wrapper: getWrapper(getStore(params?.storeValues)),
  });
  return result.current;
};

describe('useGenerateH0443 functions', () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });
  it('makes requests to expected api endpoints', async () => {
    const generate = setup();
    await act(async () => await generate(0));

    expect(generateFn).toHaveBeenCalled();
  });

  it('makes requests to expected api endpoints for each required team member', async () => {
    const organizationPersonMock = getMockPerson({ id: 3, firstName: 'JONH', surname: 'Doe' });
    getPersonConceptFn.mockResolvedValue(Promise.resolve({ data: organizationPersonMock }));

    const responseWithTeam: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      acquisitionTeam: [
        {
          id: 1,
          acquisitionFileId: 1,
          personId: 1,
          teamProfileTypeCode: 'PROPCOORD',
          rowVersion: 2,
          person: null,
          organizationId: null,
          organization: null,
          primaryContactId: null,
          primaryContact: null,
          teamProfileType: null,
        },
        {
          id: 2,
          acquisitionFileId: 1,
          personId: 2,
          teamProfileTypeCode: 'PROPAGENT',
          rowVersion: 2,
          person: null,
          organizationId: null,
          organization: null,
          primaryContactId: null,
          primaryContact: null,
          teamProfileType: null,
        },
      ],
    };
    const generate = setup({ acquisitionResponse: responseWithTeam });

    await act(async () => await generate(0));

    expect(generateFn).toHaveBeenCalled();
    expect(getPersonConceptFn).toHaveBeenCalledTimes(2);
  });

  it('makes requests to Organization when Property Coordinator is an organization with a Primary Contact', async () => {
    const organizationPersonMock = getMockPerson({ id: 3, firstName: 'JONH', surname: 'Doe' });
    getPersonConceptFn.mockResolvedValue(Promise.resolve({ data: organizationPersonMock }));

    const organizationMock = getMockOrganization();
    getOrganizationConceptFn.mockResolvedValue({ data: organizationMock });

    const responseWithTeam: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      acquisitionTeam: [
        {
          id: 1,
          acquisitionFileId: 1,
          teamProfileTypeCode: 'PROPCOORD',
          rowVersion: 2,
          organizationId: 100,
          primaryContactId: 3,
          person: null,
          organization: null,
          primaryContact: null,
          teamProfileType: null,
          personId: null,
        },
      ],
    };
    const generate = setup({ acquisitionResponse: responseWithTeam });

    await act(async () => await generate(0));

    expect(generateFn).toHaveBeenCalled();
    expect(getPersonConceptFn).toHaveBeenLastCalledWith(3);
    expect(getOrganizationConceptFn).toHaveBeenCalledTimes(1);
  });

  it('makes requests to Organization when Property Coordinator is an organization and no Primary Contact', async () => {
    const organizationPersonMock = getMockPerson({ id: 3, firstName: 'JONH', surname: 'Doe' });
    getPersonConceptFn.mockResolvedValue(Promise.resolve({ data: organizationPersonMock }));

    const organizationMock = getMockOrganization();
    getOrganizationConceptFn.mockResolvedValue(Promise.resolve({ data: organizationMock }));

    const responseWithTeam: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      acquisitionTeam: [
        {
          id: 1,
          acquisitionFileId: 1,
          teamProfileTypeCode: 'PROPCOORD',
          rowVersion: 2,
          organizationId: 100,
          person: null,
          organization: null,
          primaryContactId: null,
          primaryContact: null,
          teamProfileType: null,
          personId: null,
        },
      ],
    };
    const generate = setup({ acquisitionResponse: responseWithTeam });

    await act(async () => await generate(0));

    expect(generateFn).toHaveBeenCalled();
    expect(getOrganizationConceptFn).toHaveBeenCalledTimes(1);
    expect(getPersonConceptFn).not.toHaveBeenCalled();
  });

  it('makes requests to Organization when Property Agent is an organization with Primary Contact', async () => {
    const organizationMock = getMockOrganization();
    const organizationPersonMock = getMockPerson({ id: 3, firstName: 'JONH', surname: 'Doe' });
    getOrganizationConceptFn.mockResolvedValue(Promise.resolve({ data: organizationMock }));
    getPersonConceptFn.mockResolvedValue(Promise.resolve({ data: organizationPersonMock }));

    const responseWithTeam: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      acquisitionTeam: [
        {
          id: 2,
          acquisitionFileId: 1,
          teamProfileTypeCode: 'PROPAGENT',
          organizationId: 2,
          primaryContactId: 3,
          rowVersion: 2,
          person: null,
          organization: null,
          primaryContact: null,
          teamProfileType: null,
          personId: null,
        },
      ],
    };
    const generate = setup({ acquisitionResponse: responseWithTeam });

    await act(async () => generate(0));

    expect(generateFn).toHaveBeenCalled();
    expect(getOrganizationConceptFn).toHaveBeenCalledTimes(1);
    expect(getPersonConceptFn).toHaveBeenCalledWith(3);
  });

  it('makes requests to Organization when Property Agent is an organization with no Primary Contact', async () => {
    const organizationMock = getMockOrganization();
    const organizationPersonMock = getMockPerson({ id: 3, firstName: 'JONH', surname: 'Doe' });
    getOrganizationConceptFn.mockResolvedValue(Promise.resolve({ data: organizationMock }));
    getPersonConceptFn.mockResolvedValue(Promise.resolve({ data: organizationPersonMock }));

    const responseWithTeam: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      acquisitionTeam: [
        {
          id: 2,
          acquisitionFileId: 1,
          teamProfileTypeCode: 'PROPAGENT',
          organizationId: 2,
          rowVersion: 2,
          person: null,
          organization: null,
          primaryContactId: null,
          primaryContact: null,
          teamProfileType: null,
          personId: null,
        },
      ],
    };
    const generate = setup({ acquisitionResponse: responseWithTeam });

    await act(async () => await generate(0));

    expect(generateFn).toHaveBeenCalled();
    expect(getOrganizationConceptFn).toHaveBeenCalledTimes(1);
    expect(getPersonConceptFn).not.toHaveBeenCalled();
  });

  it('makes requests to expected api endpoints if there are properties', async () => {
    const responseWithTeam: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      fileProperties: [
        {
          propertyId: 1,
          displayOrder: null,
          file: null,
          fileId: 0,
          id: 0,
          property: null,
          propertyName: null,
          rowVersion: null,
        },
        {
          propertyId: 2,
          displayOrder: null,
          file: null,
          fileId: 0,
          id: 0,
          property: null,
          propertyName: null,
          rowVersion: null,
        },
      ],
    };
    const generate = setup({ acquisitionResponse: responseWithTeam });

    await act(async () => {
      await generate(0);
      expect(generateFn).toHaveBeenCalled();
      expect(getPropertiesFn).toHaveBeenCalledWith([1, 2]);
    });
  });
});
