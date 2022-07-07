import { screen } from '@testing-library/react';
import { Claims } from 'constants/claims';
import { noop } from 'lodash';
import { mockKeycloak, render, RenderOptions, waitFor } from 'utils/test-utils';

import { AcquisitionTray } from './AcquisitionTray';

// mock auth library
jest.mock('@react-keycloak/web');

// render component under test
const setup = (renderOptions: RenderOptions = {}) => {
  const utils = render(<AcquisitionTray onLinkClick={noop} />, {
    ...renderOptions,
  });
  return { ...utils };
};

describe('AcquisitionTray', () => {
  it('matches snapshot', async () => {
    mockKeycloak({ claims: [Claims.CONTACT_VIEW] });
    const { asFragment } = setup();

    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });
  it('should have the Acquisition Files header in the component', async () => {
    setup({});
    expect(screen.getByText(`Acquisition Files`)).toBeInTheDocument();
  });
  it('should have the Search for an Acquisition file link in the component', async () => {
    mockKeycloak({ claims: [Claims.ACQUISITION_VIEW] });
    setup({});
    expect(screen.getByText(`Search for an Acquisition file`)).toBeInTheDocument();
  });
  it('should have the Create an Acquisition file link in the component', async () => {
    mockKeycloak({ claims: [Claims.ACQUISITION_ADD] });
    setup({});
    expect(screen.getByText(`Create an Acquisition file`)).toBeInTheDocument();
  });

  it('should not have the Search for an Acquisition file link in the component', async () => {
    mockKeycloak({ claims: [] });
    setup({});
    expect(screen.queryByText(`Search for an Acquisition file`)).toBe(null);
  });
  it('should not have the Create an Acquisition file link in the component', async () => {
    mockKeycloak({ claims: [] });
    setup({});
    expect(screen.queryByText(`Create an Acquisition file`)).toBe(null);
  });
});
