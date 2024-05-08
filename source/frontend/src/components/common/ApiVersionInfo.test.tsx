import { cleanup, render, waitForElementToBeRemoved } from '@testing-library/react';

import IApiVersion from '@/hooks/pims-api/interfaces/IApiVersion';

import { ApiVersionInfo } from './ApiVersionInfo';

const defaultVersion: IApiVersion = {
  environment: 'test',
  version: '11.1.1.1',
  fileVersion: '11.1.1.1',
  informationalVersion: '11.1.1-1.999',
};

const mockGetVersion = vi.fn(async () => {
  return Promise.resolve({ data: defaultVersion });
});

vi.mock('@/hooks/pims-api/useApiHealth', () => ({
  useApiHealth: () => ({
    getVersion: mockGetVersion,
  }),
}));

describe('ApiVersionInfo suite', () => {
  afterEach(() => {
    mockGetVersion.mockClear();
    cleanup();
  });

  it('Displays version component', async () => {
    const { asFragment, getByText } = render(<ApiVersionInfo />);
    await waitForElementToBeRemoved(() => getByText('api unavailable'));
    expect(asFragment()).toMatchSnapshot();
  });

  it('Displays version information', async () => {
    const { findByText } = render(<ApiVersionInfo />);
    const element = await findByText(`v${defaultVersion.informationalVersion}`);
    expect(element).toBeInTheDocument();
    expect(mockGetVersion).toHaveBeenCalledTimes(1);
  });
});
