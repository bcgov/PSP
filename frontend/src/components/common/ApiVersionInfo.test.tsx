import React from 'react';
import { cleanup, render } from '@testing-library/react';
import { ApiVersionInfo } from './ApiVersionInfo';
import { act } from 'react-test-renderer';

const defaultVersion = {
  environment: 'test',
  version: '11.1.1.1',
};

const mockGetVersion = jest.fn(async () => {
  return Promise.resolve({ data: defaultVersion });
});

jest.mock('hooks/pims-api', () => ({
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
    await act(async () => {
      const { container } = render(<ApiVersionInfo />);
      expect(container).toMatchSnapshot();
    });
  });

  it('Displays version information', async () => {
    await act(async () => {
      const { findByTestId } = render(<ApiVersionInfo />);
      const element = await findByTestId('version');
      expect(element).toContainHTML(`v${defaultVersion.version}`);
      expect(mockGetVersion).toHaveBeenCalledTimes(1);
    });
  });
});
