import 'leaflet';
import 'leaflet/dist/leaflet.css';

import userEvent from '@testing-library/user-event';
import {
  IPopUpContext,
  PropertyPopUpContextProvider,
} from 'components/maps/providers/PropertyPopUpProvider';
import { Claims, PropertyTypes } from 'constants/index';
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

function createBuildingContext(): Partial<IPopUpContext> {
  return { propertyTypeId: PropertyTypes.Building, propertyInfo: { id: 1 } as any };
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

  it('when closed, clicking the toggle button should open the PARCEL details within the info component', async () => {
    const context = createParcelContext();
    const { component, ready, findContainer, findToggleButton } = setup(context);
    await waitFor(() => ready);
    // when info component is closed...
    const slideOut = findContainer();
    expect(slideOut).toBeInTheDocument();
    expect(slideOut.className).toContain('closed');
    // clicking the button should open it...
    const toggleBtn = findToggleButton();
    userEvent.click(toggleBtn);
    // wait for it to open...
    await waitFor(() => expect(slideOut.className).not.toContain('closed'));
    // check appropriate text has been rendered...
    const headerLabel = component.container.querySelector('p.label.header');
    expect(headerLabel).toHaveTextContent('icon-lot.svgParcel Identification');
  });

  it('with appropriate user permissions, opening the info component should display additional PARCEL information', async () => {
    // user can see additional parcel information
    const renderOptions: RenderOptions = {
      useMockAuthentication: true,
      roles: [Claims.ADMIN_PROPERTIES],
      organizations: [1],
    };

    const context = createParcelContext();
    const { component, ready, findContainer, findToggleButton } = setup(
      context,
      <Template />,
      renderOptions,
    );

    await waitFor(() => ready);
    // when info component is closed...
    const slideOut = findContainer();
    expect(slideOut).toBeInTheDocument();
    expect(slideOut.className).toContain('closed');
    // clicking the button should open it...
    const toggleBtn = findToggleButton();
    userEvent.click(toggleBtn);
    // wait for it to open...
    await waitFor(() => expect(slideOut.className).not.toContain('closed'));
    // check appropriate text has been rendered...
    const { container } = component;
    const headerLabel = container.querySelector('p.label.header');
    expect(headerLabel).toHaveTextContent('icon-lot.svgParcel Identification');
    // additional information should be available...
    const tabButton = container.querySelector('#slideOutTab') as HTMLElement;
    userEvent.click(tabButton);
  });

  it('when closed, clicking the toggle button should open the BUILDING details within the info component', async () => {
    const context = createBuildingContext();
    const { component, ready, findContainer, findToggleButton } = setup(context);
    await waitFor(() => ready);
    // when info component is closed...
    const slideOut = findContainer();
    expect(slideOut).toBeInTheDocument();
    expect(slideOut.className).toContain('closed');
    // clicking the button should open it...
    const toggleBtn = findToggleButton();
    userEvent.click(toggleBtn);
    // wait for it to open...
    await waitFor(() => expect(slideOut.className).not.toContain('closed'));
    // check appropriate text has been rendered...
    const headerLabel = component.container.querySelector('p.label.header');
    expect(headerLabel).toHaveTextContent('icon-business.svgBuilding Identification');
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
