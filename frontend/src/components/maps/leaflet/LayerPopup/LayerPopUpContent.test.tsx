import { cleanup, render } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import { createMemoryHistory } from 'history';
import L from 'leaflet';
import queryString from 'query-string';
import React from 'react';
import { useMap } from 'react-leaflet';
import { createRouteProvider } from 'utils/test-utils';

import { IPopupContentProps, LayerPopupContent } from './LayerPopupContent';

const history = createMemoryHistory();

jest.mock('react-leaflet');

// Mock react-leaflet dependencies
const map: Partial<L.Map> = {
  getZoom: jest.fn(),
  getBoundsZoom: jest.fn(),
  flyToBounds: jest.fn(),
};

(useMap as jest.Mock).mockReturnValue(map);

const northEast = new L.LatLng(50.5, -120.7);
const southWest = new L.LatLng(50.3, -121.2);
const bounds = new L.LatLngBounds(southWest, northEast);

const mockLayer: IPopupContentProps = {
  config: {},
  data: {
    feature_area_sqm: '10000',
    feature_length_m: '500',
    municipality: 'Rural',
    objectid: '0',
    owner_type: 'Private',
    parcel_class: 'Subdivision',
    parcel_name: '000000000',
    parcel_start_date: '2020-01-01',
    parcel_status: 'Active',
    PID: '000000001',
    pid_number: '000000000',
    PIN: '1',
    plan_number: 'VIP00000',
    regional_district: 'Fake District',
    se_anno_cad_data: '',
    when_updated: '2020-01-01',
  },
  onAddToParcel: jest.fn(),
  bounds: bounds,
};

const renderPopup = (props: IPopupContentProps) => {
  const wrapper = createRouteProvider(history);
  return render(<LayerPopupContent {...props} />, { wrapper });
};

describe('Layer Popup Content', () => {
  afterEach(cleanup);

  it('Renders correctly', () => {
    const { asFragment } = renderPopup(mockLayer);
    expect(asFragment()).toMatchSnapshot();
  });

  it(`Doesn't render Populate details link by default`, () => {
    const { queryByText } = renderPopup(mockLayer);
    const link = queryByText(/Populate property details/i);
    expect(link).toBeNull();
  });

  it('Renders the Populate details link when sideBar is open', () => {
    history.location.search = queryString.stringify({
      disabled: false,
      loadDraft: false,
      sidebar: true,
    });
    const { getByText } = renderPopup(mockLayer);
    const link = getByText(/Populate property details/i);
    expect(link).toBeInTheDocument();
  });

  it('Calls onAddToParcel when Populate details is clicked', () => {
    history.location.search = queryString.stringify({
      disabled: false,
      loadDraft: false,
      sidebar: true,
    });
    // render popup
    const { getByText } = renderPopup(mockLayer);
    const link = getByText(/Populate property details/i);
    expect(link).toBeInTheDocument();
    // click link
    userEvent.click(link);
    expect(mockLayer.onAddToParcel).toBeCalled();
  });

  it(`Doesn't render Zoom link when bounds are not provided`, () => {
    (map.getZoom as jest.Mock).mockReturnValue(5);
    (map.getBoundsZoom as jest.Mock).mockReturnValue(10);
    const { queryByText } = renderPopup({ ...mockLayer, bounds: undefined });
    const link = queryByText(/Zoom/i);
    expect(link).toBeNull();
  });

  it(`Renders the Zoom link when bounds are provided`, () => {
    (map.getZoom as jest.Mock).mockReturnValue(5);
    (map.getBoundsZoom as jest.Mock).mockReturnValue(10);
    const { getByText } = renderPopup(mockLayer);
    const link = getByText(/Zoom/i);
    expect(link).toBeInTheDocument();
  });

  it(`Zooms the map to property bounds when Zoom link is clicked`, () => {
    (map.getZoom as jest.Mock).mockReturnValue(5);
    (map.getBoundsZoom as jest.Mock).mockReturnValue(10);
    // render popup
    const { getByText } = renderPopup(mockLayer);
    const link = getByText(/Zoom/i);
    expect(link).toBeInTheDocument();
    // click link
    userEvent.click(link);
    expect(map.flyToBounds).toBeCalled();
  });
});
