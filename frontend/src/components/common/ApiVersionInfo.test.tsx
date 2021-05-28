import { cleanup, render } from '@testing-library/react';
import React from 'react';
import { act } from 'react-test-renderer';

import { ApiVersionInfo } from './ApiVersionInfo';

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
