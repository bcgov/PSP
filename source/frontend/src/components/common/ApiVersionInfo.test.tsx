import { act, cleanup, render } from '@testing-library/react';
import { IApiVersion } from 'hooks/pims-api';
import React from 'react';

import { ApiVersionInfo } from './ApiVersionInfo';

const defaultVersion: IApiVersion = {
  environment: 'test',
  version: '11.1.1.1',
  fileVersion: '11.1.1.1',
  informationalVersion: '11.1.1-1.999',
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
      const { findByText } = render(<ApiVersionInfo />);
      const element = await findByText(`v${defaultVersion.informationalVersion}`);
      expect(element).toBeInTheDocument();
      expect(mockGetVersion).toHaveBeenCalledTimes(1);
    });
  });
});
