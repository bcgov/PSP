import { act } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { mockAgreementsResponse } from '@/mocks/agreements.mock';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';

import { useGenerateAgreement } from './useGenerateAgreement';

const generateFn = vi.fn();
const getAcquisitionFileFn = vi.fn();
const getAcquisitionFileProperties = vi.fn();
const getPersonConceptFn = vi.fn().mockResolvedValue({});
const getOrganizationConceptFn = vi.fn().mockResolvedValue({});

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
      getAcquisitionProperties: { execute: getAcquisitionFileProperties },
    } as unknown as ReturnType<typeof useAcquisitionProvider>),
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

  const { result } = renderHook(useGenerateAgreement, {
    wrapper: getWrapper(getStore(params?.storeValues)),
  });
  return result.current;
};

describe('useGenerateAgreement functions', () => {
  it('makes requests to expected api endpoints', async () => {
    const generate = setup();
    await act(async () => {
      await generate(mockAgreementsResponse()[1]);
      expect(generateFn).toHaveBeenCalled();
    });
  });
  it('makes requests to expected api endpoints if a team member is a property coordinator with person', async () => {
    const responseWithTeam: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      acquisitionTeam: [
        {
          id: 1,
          acquisitionFileId: 1,
          personId: 1,
          teamProfileTypeCode: 'PROPCOORD',
          rowVersion: 2,
          organization: null,
          person: null,
          primaryContact: null,
          primaryContactId: null,
          teamProfileType: null,
          organizationId: null,
        },
      ],
    };
    const generate = setup({ acquisitionResponse: responseWithTeam });

    await act(async () => {
      await generate(mockAgreementsResponse()[1]);
      expect(generateFn).toHaveBeenCalled();
      expect(getPersonConceptFn).toHaveBeenCalledWith(1);
      expect(getAcquisitionFileProperties).toHaveBeenCalled();
    });
  });

  it('makes requests to expected api endpoints if a team member is a negotiating agent with person', async () => {
    const responseWithTeam: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      acquisitionTeam: [
        {
          id: 1,
          acquisitionFileId: 1,
          personId: 1,
          teamProfileTypeCode: 'NEGOTAGENT',
          rowVersion: 2,
          organization: null,
          person: null,
          primaryContact: null,
          primaryContactId: null,
          teamProfileType: null,
          organizationId: null,
        },
      ],
    };
    const generate = setup({ acquisitionResponse: responseWithTeam });

    await act(async () => {
      await generate(mockAgreementsResponse()[1]);
      expect(generateFn).toHaveBeenCalled();
      expect(getPersonConceptFn).toHaveBeenCalledWith(1);
      expect(getAcquisitionFileProperties).toHaveBeenCalled();
    });
  });

  it('makes requests to expected api endpoints if a team member is a property coordinator with org', async () => {
    const responseWithTeam: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      acquisitionTeam: [
        {
          id: 1,
          acquisitionFileId: 1,
          organizationId: 1,
          teamProfileTypeCode: 'PROPCOORD',
          rowVersion: 2,
          organization: null,
          person: null,
          primaryContact: null,
          primaryContactId: null,
          teamProfileType: null,
          personId: null,
        },
      ],
    };
    const generate = setup({ acquisitionResponse: responseWithTeam });

    await act(async () => {
      await generate(mockAgreementsResponse()[1]);
      expect(generateFn).toHaveBeenCalled();
      expect(getOrganizationConceptFn).toHaveBeenCalledWith(1);
      expect(getAcquisitionFileProperties).toHaveBeenCalled();
    });
  });

  it('makes requests to expected api endpoints if a team member is a negotiating agent with organization', async () => {
    const responseWithTeam: ApiGen_Concepts_AcquisitionFile = {
      ...mockAcquisitionFileResponse(),
      acquisitionTeam: [
        {
          id: 1,
          acquisitionFileId: 1,
          organizationId: 1,
          teamProfileTypeCode: 'NEGOTAGENT',
          rowVersion: 2,
          organization: null,
          person: null,
          primaryContact: null,
          primaryContactId: null,
          teamProfileType: null,
          personId: null,
        },
      ],
    };
    const generate = setup({ acquisitionResponse: responseWithTeam });

    await act(async () => {
      await generate(mockAgreementsResponse()[1]);
      expect(generateFn).toHaveBeenCalled();
      expect(getOrganizationConceptFn).toHaveBeenCalledWith(1);
      expect(getAcquisitionFileProperties).toHaveBeenCalled();
    });
  });
});
