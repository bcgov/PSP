import userEvent from '@testing-library/user-event';
import L from 'leaflet';

import {
  IMapStateMachineContext,
  useMapStateMachine,
} from '@/components/common/mapFSM/MapStateMachineContext';
import { cleanup, render } from '@/utils/test-utils';

import { ILayerPopupLinksProps, LayerPopupLinks } from './LayerPopupLinks';

jest.mock('react-leaflet');

// Mock react-leaflet dependencies
const mapMachineMock: Partial<IMapStateMachineContext> = {
  requestFlyToBounds: jest.fn(),
};

jest.mock('@/components/common/mapFSM/MapStateMachineContext');
(useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineMock);

const northEast = new L.LatLng(50.5, -120.7);
const southWest = new L.LatLng(50.3, -121.2);

const renderLinks = (props: ILayerPopupLinksProps) => {
  return render(<LayerPopupLinks {...props} />);
};

describe('Layer Popup links', () => {
  afterEach(cleanup);

  it('Renders correctly', () => {
    const { asFragment } = renderLinks({ bounds: new L.LatLngBounds(southWest, northEast) });
    expect(asFragment()).toMatchSnapshot();
  });

  it(`Renders the Zoom link`, () => {
    const { getByText } = renderLinks({ bounds: new L.LatLngBounds(southWest, northEast) });
    const link = getByText(/Zoom/i);
    expect(link).toBeInTheDocument();
  });

  it(`Zooms the map to property bounds when Zoom link is clicked`, () => {
    // render popup
    const { getByText } = renderLinks({ bounds: new L.LatLngBounds(southWest, northEast) });
    const link = getByText(/Zoom/i);
    expect(link).toBeInTheDocument();
    // click link
    userEvent.click(link);
    expect(mapMachineMock.requestFlyToBounds).toBeCalled();
  });
});
