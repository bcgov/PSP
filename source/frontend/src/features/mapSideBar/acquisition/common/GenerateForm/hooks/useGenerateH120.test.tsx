import { act } from '@testing-library/react';
import { renderHook } from '@testing-library/react-hooks';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useH120CategoryRepository } from '@/hooks/repositories/useH120CategoryRepository';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { getMockApiCompensationList } from '@/mocks/compensations.mock';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';
import { ExternalResultStatus } from '@/models/api/ExternalResult';

import { useGenerateH120 } from './useGenerateH120';

const generateFn = jest
  .fn()
  .mockResolvedValue({ status: ExternalResultStatus.Success, payload: {} });
const getAcquisitionFileFn = jest.fn<Api_AcquisitionFile | undefined, any[]>();
const getAcquisitionPropertiesFn = jest.fn();
const getAcquisitionCompReqH120s = jest.fn();
const getH120sCategoryFn = jest.fn();
const getInterestHolderFn = jest.fn();

jest.mock('@/features/documents/hooks/useDocumentGenerationRepository');
(useDocumentGenerationRepository as jest.Mock).mockImplementation(() => ({
  generateDocumentDownloadWrappedRequest: generateFn,
}));

jest.mock('@/hooks/repositories/useH120CategoryRepository');
(useH120CategoryRepository as jest.Mock).mockImplementation(() => ({
  execute: getH120sCategoryFn,
}));

jest.mock('@/hooks/repositories/useAcquisitionProvider');
(useAcquisitionProvider as jest.Mock).mockImplementation(() => ({
  getAcquisitionFile: { execute: getAcquisitionFileFn },
  getAcquisitionProperties: { execute: getAcquisitionPropertiesFn },
  getAcquisitionCompReqH120s: { execute: getAcquisitionCompReqH120s },
}));
jest.mock('@/hooks/repositories/useInterestHolderRepository');
(useInterestHolderRepository as jest.Mock).mockImplementation(() => ({
  getAcquisitionInterestHolders: { execute: getInterestHolderFn },
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

const setup = (params?: { storeValues?: any; acquisitionResponse?: Api_AcquisitionFile }) => {
  var acquisitionResponse = mockAcquisitionFileResponse();
  if (params?.acquisitionResponse !== undefined) {
    acquisitionResponse = params.acquisitionResponse;
  }

  getAcquisitionFileFn.mockReturnValue(acquisitionResponse);

  const { result } = renderHook(useGenerateH120, {
    wrapper: getWrapper(getStore(params?.storeValues)),
  });
  return result.current;
};

describe('useGenerateH120 functions', () => {
  it('makes requests to expected api endpoints', async () => {
    const generate = setup();
    await act(async () => {
      await generate(getMockApiCompensationList()[0]);
      expect(generateFn).toHaveBeenCalled();
    });
  });

  it('throws an error if no compensation is passed', async () => {
    const generate = setup();
    await expect(generate({} as any)).rejects.toThrow(
      'user must choose a valid compensation requisition in order to generate a document',
    );
  });
});
