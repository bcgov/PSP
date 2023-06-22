import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { render, RenderOptions, waitFor } from '@/utils/test-utils';

import DocumentTemplateManagementContainer from './DocumentTemplateManagementContainer';

const mockAxios = new MockAdapter(axios);
jest.mock('@react-keycloak/web');

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

describe('DocumentTemplateManagementContainer component', () => {
  afterEach(() => {
    jest.resetAllMocks();
    mockAxios.resetHistory();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({});
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });
});
