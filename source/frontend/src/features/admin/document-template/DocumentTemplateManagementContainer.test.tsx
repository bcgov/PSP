import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { act, render, RenderOptions, waitFor } from '@/utils/test-utils';

import DocumentTemplateManagementContainer from './DocumentTemplateManagementContainer';
import { useFormDocumentRepository } from '@/hooks/repositories/useFormDocumentRepository';

const mockAxios = new MockAdapter(axios);

// render component under test
const setup = (renderOptions: RenderOptions) => {
  const { ...rest } = renderOptions;
  const utils = render(<DocumentTemplateManagementContainer />, {
    ...rest,
  });

  return {
    ...utils,
  };
};

vi.mock('@/hooks/repositories/useFormDocumentRepository');
vi.mocked(useFormDocumentRepository).mockReturnValue({
  getFormDocumentTypes: {
    error: null,
    response: [],
    execute: vi.fn(),
    loading: false,
    status: 200,
  },
} as unknown as ReturnType<typeof useFormDocumentRepository>);

describe('DocumentTemplateManagementContainer component', () => {
  it('matches snapshot', async () => {
    const { asFragment } = setup({});
    await act(async () => {});
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });
});
