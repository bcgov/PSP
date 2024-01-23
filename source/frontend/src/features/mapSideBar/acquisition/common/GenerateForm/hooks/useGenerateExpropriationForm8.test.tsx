import { act, renderHook } from '@testing-library/react-hooks';
import { Provider } from 'react-redux';
import configureMockStore, { MockStoreEnhanced } from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import { useForm8Repository } from '@/hooks/repositories/useForm8Repository';
import { mockGetExpropriationPaymentApi } from '@/mocks/ExpropriationPayment.mock';
import { ApiGen_CodeTypes_ExternalResponseStatus } from '@/models/api/generated/ApiGen_CodeTypes_ExternalResponseStatus';
import { ApiGen_Concepts_ExpropriationPayment } from '@/models/api/generated/ApiGen_Concepts_ExpropriationPayment';

import { useGenerateExpropriationForm8 } from './useGenerateExpropriationForm8';

const generateFn = jest
  .fn()
  .mockResolvedValue({ status: ApiGen_CodeTypes_ExternalResponseStatus.Success, payload: {} });

const getExpropriationPaymentApi = jest.fn<
  Promise<ApiGen_Concepts_ExpropriationPayment | undefined>,
  any[]
>();

jest.mock('@/hooks/repositories/useForm8Repository');
(useForm8Repository as jest.Mock).mockImplementation(() => ({
  getForm8: { execute: getExpropriationPaymentApi },
}));

jest.mock('@/features/documents/hooks/useDocumentGenerationRepository');
(useDocumentGenerationRepository as jest.Mock).mockImplementation(() => ({
  generateDocumentDownloadWrappedRequest: generateFn,
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

const setup = (params?: {
  storeValues?: any;
  expropriationPaymentResponse?: ApiGen_Concepts_ExpropriationPayment;
}) => {
  var expropriationPaymentResponse = mockGetExpropriationPaymentApi();
  if (params?.expropriationPaymentResponse !== undefined) {
    expropriationPaymentResponse = params.expropriationPaymentResponse;
  }

  getExpropriationPaymentApi.mockResolvedValue(expropriationPaymentResponse);

  const { result } = renderHook(useGenerateExpropriationForm8, {
    wrapper: getWrapper(getStore(params?.storeValues)),
  });
  return result.current;
};

describe('useGenerateExpropriationForm8 functions', () => {
  it('makes requests to expected api endpoints', async () => {
    const generate = setup();
    await act(async () => generate(1, '01-123'));

    expect(generateFn).toHaveBeenCalled();
    expect(getExpropriationPaymentApi).toHaveBeenCalled();
  });

  it('throws an error if generation api call is unsuccessful', async () => {
    generateFn.mockResolvedValue({
      status: ApiGen_CodeTypes_ExternalResponseStatus.Error,
      payload: null,
    });
    const generate = setup();
    await expect(generate(1, '01-123')).rejects.toThrow('Failed to generate file');
  });
});
