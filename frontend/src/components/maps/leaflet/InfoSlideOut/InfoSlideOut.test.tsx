import 'leaflet';
import 'leaflet/dist/leaflet.css';

import userEvent from '@testing-library/user-event';
import {
  IPopUpContext,
  PropertyPopUpContextProvider,
} from 'components/maps/providers/PropertyPopUpProvider';
import { PropertyTypes } from 'constants/index';
import { useApiProperties } from 'hooks/pims-api';
import noop from 'lodash/noop';
import { mockParcel } from 'mocks/filterDataMock';
import React, { useState } from 'react';
import {
  cleanup,
  createMapContainer,
  deferred,
  render,
  RenderOptions,
  waitFor,
} from 'utils/test-utils';

import InfoSlideOut from './InfoSlideOut';

jest.mock('@react-keycloak/web');

// This mocks the parcels of land a user can see - should be able to see 2 markers
jest.mock('hooks/useApi');
jest.mock('hooks/pims-api');
((useApiProperties as unknown) as jest.Mock<Partial<typeof useApiProperties>>).mockReturnValue({
  getProperty: async () => {
    return mockParcel;
  },
});

// component under test
function Template({ openByDefault = false }) {
  const [open, setOpen] = useState(openByDefault);
  return <InfoSlideOut open={open} setOpen={() => setOpen(!open)} />;
}

function createParcelContext(): Partial<IPopUpContext> {
  return { propertyTypeId: PropertyTypes.Land, propertyInfo: { id: 1 } as any };
}

function setup(
  context: Partial<IPopUpContext>,
  ui = <Template />,
  renderOptions: RenderOptions = { useMockAuthentication: true },
  setMap = noop,
) {
  // create a promise to wait for the map to be ready (which happens after initial render)
  const { promise, resolve } = deferred();
  const MapContainer = createMapContainer(resolve, setMap);

  const uiToRender = (
    <PropertyPopUpContextProvider values={context}>
      <MapContainer>{ui}</MapContainer>
    </PropertyPopUpContextProvider>
  );

  const component = render(uiToRender, renderOptions);
  return {
    component,
    ready: promise,
    findContainer: () => document.querySelector('#infoContainer') as HTMLElement,
    findToggleButton: () => document.querySelector('#slideOutInfoButton') as HTMLElement,
  };
}

describe('InfoSlideOut View', () => {
  afterEach(() => {
    jest.clearAllMocks();
    cleanup();
  });

  it('renders correctly', () => {
    const context = createParcelContext();
    const { component } = setup(context);
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('should render the slide out button', async () => {
    const context = createParcelContext();
    const { ready, findToggleButton } = setup(context);
    await waitFor(() => ready);
    const toggleBtn = findToggleButton();
    expect(toggleBtn).toBeInTheDocument();
  });

  it('should be closed by default', async () => {
    const context = createParcelContext();
    const { ready, findContainer } = setup(context);
    await waitFor(() => ready);
    const slideOut = findContainer();
    expect(slideOut).toBeInTheDocument();
    expect(slideOut.className).toContain('closed');
  });

  it('when open, clicking the button should close the layers list', async () => {
    const context = createParcelContext();
    const { ready, findContainer, findToggleButton } = setup(
      context,
      <Template openByDefault={true} />,
    );
    await waitFor(() => ready);
    // when info component is closed...
    const slideOut = findContainer();
    expect(slideOut).toBeInTheDocument();
    expect(slideOut.className).not.toContain('closed');
    // clicking the button should open it...
    const toggleBtn = findToggleButton();
    userEvent.click(toggleBtn);
    // wait for it to open...
    await waitFor(() => expect(slideOut.className).toContain('closed'));
  });
});
