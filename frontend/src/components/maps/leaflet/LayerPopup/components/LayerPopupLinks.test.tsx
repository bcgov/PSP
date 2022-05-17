import userEvent from '@testing-library/user-event';
import L from 'leaflet';
import { useMap } from 'react-leaflet';
import { cleanup, render } from 'utils/test-utils';

import { IPopupContentProps } from './LayerPopupContent';
import { LayerPopupLinks } from './LayerPopupLinks';

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

const mockLayer: IPopupContentProps = {
  layerPopup: {
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
    latlng: new L.LatLng(48, -123),
    title: 'Foo Bar',
    feature: undefined,
    bounds: new L.LatLngBounds(southWest, northEast),
  },
};

const renderLinks = (props: IPopupContentProps) => {
  return render(<LayerPopupLinks {...props} />);
};

describe('Layer Popup links', () => {
  afterEach(cleanup);

  it('Renders correctly', () => {
    const { asFragment } = renderLinks(mockLayer);
    expect(asFragment()).toMatchSnapshot();
  });

  it(`Renders the Zoom link`, () => {
    (map.getZoom as jest.Mock).mockReturnValue(5);
    (map.getBoundsZoom as jest.Mock).mockReturnValue(10);
    const { getByText } = renderLinks(mockLayer);
    const link = getByText(/Zoom/i);
    expect(link).toBeInTheDocument();
  });

  it(`Zooms the map to property bounds when Zoom link is clicked`, () => {
    (map.getZoom as jest.Mock).mockReturnValue(5);
    (map.getBoundsZoom as jest.Mock).mockReturnValue(10);
    // render popup
    const { getByText } = renderLinks(mockLayer);
    const link = getByText(/Zoom/i);
    expect(link).toBeInTheDocument();
    // click link
    userEvent.click(link);
    expect(map.flyToBounds).toBeCalled();
  });
});
