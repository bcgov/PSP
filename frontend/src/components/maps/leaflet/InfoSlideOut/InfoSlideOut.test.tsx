import * as React from 'react';
import 'leaflet';
import { Map as LeafletMap } from 'leaflet';
import { Map as ReactLeafletMap, MapProps } from 'react-leaflet';
import Adapter from 'enzyme-adapter-react-16';
import Enzyme, { mount } from 'enzyme';
import InfoSlideOut from './InfoSlideOut';
import 'leaflet/dist/leaflet.css';
import { fireEvent, waitFor } from '@testing-library/dom';
import { Button } from 'react-bootstrap';
import { createMemoryHistory } from 'history';
import { Router } from 'react-router-dom';
import thunk from 'redux-thunk';
import configureMockStore from 'redux-mock-store';
import { Provider } from 'react-redux';
import {
  PropertyPopUpContextProvider,
  IPopUpContext,
} from 'components/maps/providers/PropertyPopUpProvider';
import { PimsAPI, useApi } from 'hooks/useApi';
import { render, wait, screen, cleanup } from '@testing-library/react';
import { mockKeycloak } from 'features/projects/dispose/testUtils';
import { Claims } from 'constants/index';
import { mockParcel, mockBuildingWithAssociatedLand } from 'mocks/filterDataMock';

jest.mock('@react-keycloak/web');

Enzyme.configure({ adapter: new Adapter() });
const history = createMemoryHistory();
const mockStore = configureMockStore([thunk]);
const store = mockStore({});

let mapRef: React.RefObject<ReactLeafletMap<MapProps, LeafletMap>> | undefined;

// This mocks the parcels of land a user can see - should be able to see 2 markers
jest.mock('hooks/useApi');
((useApi as unknown) as jest.Mock<Partial<PimsAPI>>).mockReturnValue({
  getParcel: async () => {
    return mockParcel;
  },
  getBuilding: async () => {
    return mockBuildingWithAssociatedLand;
  },
});

const MapComponent = ({ context }: { context?: Partial<IPopUpContext> }) => {
  const [open, setOpen] = React.useState(false);
  mapRef = React.useRef<any>();
  return (
    <Provider store={store}>
      <Router history={history}>
        <PropertyPopUpContextProvider values={context}>
          <div id="mapid" style={{ width: 500, height: 500 }}>
            <ReactLeafletMap ref={mapRef} center={[48.423078, -123.360956]} zoom={18}>
              <InfoSlideOut open={open} setOpen={() => setOpen(!open)} />
            </ReactLeafletMap>
          </div>
        </PropertyPopUpContextProvider>
      </Router>
    </Provider>
  );
};

describe('InfoSlideOut View', () => {
  beforeEach(() => {
    mapRef = undefined;
    jest.clearAllMocks();
    mockKeycloak([], [1]);
    cleanup();
  });

  it('Should render the slide out button', () => {
    const component = mount(<MapComponent />);
    waitFor(() => expect(mapRef?.current).toBeTruthy(), { timeout: 500 });
    const infoButton = component.find(Button).first();
    expect(infoButton).toBeDefined();
    expect(infoButton.props().id).toBe('slideOutInfoButton');
  });

  it('Component should be closed by default', () => {
    const component = mount(<MapComponent />);
    waitFor(() => expect(mapRef?.current).toBeTruthy(), { timeout: 500 });
    const infoContainer = component.find('#infoContainer').first();
    expect(infoContainer).toBeDefined();
    expect(infoContainer.props().className?.includes('closed')).toBeTruthy();
  });

  it('Clicking the button should open the parcel details within the info component', async () => {
    const { container } = render(
      <MapComponent context={{ propertyTypeID: 0, propertyInfo: { id: 1 } as any }} />,
    );
    const infoButton = container.querySelector('#slideOutInfoButton');
    fireEvent.click(infoButton!);
    await wait(() => {
      const filterBackdrop = screen.queryByTestId('filter-backdrop-loading');
      expect(filterBackdrop).toBeNull();
    });
    const headerLabel = container.querySelector('p.label.header');
    expect(headerLabel).toHaveTextContent('icon-lot.svgParcel Identification');
  });

  it('Clicking the button should open the viewable parcel details within the info component if the user has permissions', async () => {
    mockKeycloak([Claims.ADMIN_PROPERTIES], [1]);
    const { container } = render(
      <MapComponent context={{ propertyTypeID: 0, propertyInfo: { id: 1 } as any }} />,
    );
    const infoButton = container.querySelector('#slideOutInfoButton');
    fireEvent.click(infoButton!);
    await wait(() => {
      const filterBackdrop = screen.queryByTestId('filter-backdrop-loading');
      expect(filterBackdrop).toBeNull();
    });
    const headerLabel = container.querySelector('p.label.header');
    expect(headerLabel).toHaveTextContent('icon-lot.svgParcel Identification');
    const tabButton = container.querySelector('#slideOutTab');
    fireEvent.click(tabButton!);
    screen.getByText('Associated Buildings');
  });

  it('Clicking the button should open the building details within the info component', async () => {
    const { container } = render(
      <MapComponent context={{ propertyTypeID: 1, propertyInfo: { id: 1 } as any }} />,
    );
    const infoButton = container.querySelector('#slideOutInfoButton');
    fireEvent.click(infoButton!);
    await wait(() => {
      const filterBackdrop = screen.queryByTestId('filter-backdrop-loading');
      expect(filterBackdrop).toBeNull();
    });
    const headerLabel = container.querySelector('p.label.header');
    expect(headerLabel).toHaveTextContent('icon-business.svgBuilding Identification');
  });

  it('Clicking the button should open the viewable building details within the info component if the user has permissions', async () => {
    mockKeycloak([Claims.ADMIN_PROPERTIES], [1]);
    const { container } = render(
      <MapComponent context={{ propertyTypeID: 1, propertyInfo: { id: 1 } as any }} />,
    );
    const infoButton = container.querySelector('#slideOutInfoButton');
    fireEvent.click(infoButton!);
    await wait(() => {
      const filterBackdrop = screen.queryByTestId('filter-backdrop-loading');
      expect(filterBackdrop).toBeNull();
    });
    const headerLabel = container.querySelector('p.label.header');
    expect(headerLabel).toHaveTextContent('icon-business.svgBuilding Identification');
    const tabButton = container.querySelector('#slideOutTab');
    fireEvent.click(tabButton!);
    expect(screen.getByText('Associated Land')).toBeVisible();
  });

  it('Clicking the button should close the info component', () => {
    const component = mount(<MapComponent />);
    waitFor(() => expect(mapRef?.current).toBeTruthy(), { timeout: 500 });
    const infoContainer = component.find('#infoContainer').first();
    const infoButton = component.find('#slideOutInfoButton').first();
    expect(infoContainer).toBeDefined();
    expect(infoButton).toBeDefined();
    expect(infoContainer.props().className?.includes('closed')).toBeTruthy();
    infoButton.simulate('click');
    waitFor(() => expect(infoContainer.props().className?.includes('closed')).toBeFalsy(), {
      timeout: 500,
    });
    infoButton.simulate('click');
    waitFor(() => expect(infoContainer.props().className?.includes('closed')).toBeTruthy(), {
      timeout: 500,
    });
  });
});
