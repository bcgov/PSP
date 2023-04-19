import { act } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import { useDocumentGenerationRepository } from 'features/documents/hooks/useDocumentGenerationRepository';
import { useApiProperties } from 'hooks/pims-api';
import { useApiContacts } from 'hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from 'hooks/repositories/useAcquisitionProvider';
import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useGenerateH0443 } from './useGenerateH0443';

const generateFn = jest.fn();
const getAcquisitionFileFn = jest.fn();
const getPersonConceptFn = jest.fn();
const getPropertiesFn = jest.fn();

jest.mock('features/documents/hooks/useDocumentGenerationRepository');
(useDocumentGenerationRepository as jest.Mock).mockImplementation(() => ({
  generateDocumentDownloadWrappedRequest: generateFn,
}));

jest.mock('hooks/repositories/useAcquisitionProvider');
(useAcquisitionProvider as jest.Mock).mockImplementation(() => ({
  getAcquisitionFileWrappedRequest: getAcquisitionFileFn,
}));

jest.mock('hooks/pims-api/useApiContacts');
(useApiContacts as jest.Mock).mockImplementation(() => ({
  getPersonConcept: getPersonConceptFn,
}));

jest.mock('hooks/pims-api/useApiProperties');
(useApiProperties as jest.Mock).mockImplementation(() => ({
  generateDocumentDownloadWrappedRequest: getPropertiesFn,
}));

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

const setup = (values?: any) => {
  const { result } = renderHook(useGenerateH0443, { wrapper: getWrapper(getStore(values)) });
  return result.current;
};

describe('useGenerateH0443 functions', () => {
  it('makes requests to expected api endpoints', async () => {
    const generate = setup();
    await act(async () => {
      await generate(0);
      expect(generateFn).toHaveBeenCalled();
    });
  });
  it('makes requests to expected api endpoints if a team member is a property coordinator', async () => {
    const generate = setup();
    await act(async () => {
      const response = {
        ...mockAcquisitionFileResponse(),
        acquisitionTeam: [
          {
            id: 1,
            personId: 1,
            person: {
              id: 1,
              isDisabled: false,
              surname: 'Smith',
              firstName: 'Bob',
              middleNames: 'Billy',
              preferredName: 'Tester McTest',
              personOrganizations: [],
              personAddresses: [],
              contactMethods: [],
              comment: 'This is a test comment.',
              rowVersion: 2,
            },
            personProfileTypeCode: 'PROPCOORD',
            personProfileType: {
              id: 'PROPCOORD',
              description: 'Property Coordinator',
              isDisabled: false,
            },
            isDisabled: false,
            rowVersion: 2,
          },
        ],
      };
      await generate(0);
      expect(generateFn).toHaveBeenCalled();
      expect(getPersonConceptFn).toHaveBeenCalled();
      expect(getPropertiesFn).toHaveBeenCalled();
    });
  });
});
