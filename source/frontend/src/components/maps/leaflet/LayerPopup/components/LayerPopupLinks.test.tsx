import userEvent from '@testing-library/user-event';
import L from 'leaflet';

import {
  IMapStateMachineContext,
  useMapStateMachine,
} from '@/components/common/mapFSM/MapStateMachineContext';
import { act, cleanup, render } from '@/utils/test-utils';

import { ILayerPopupLinksProps, LayerPopupLinks } from './LayerPopupLinks';

vi.mock('react-leaflet');

// Mock react-leaflet dependencies
const mapMachineMock: Partial<IMapStateMachineContext> = {
  requestFlyToBounds: vi.fn(),
};

const northEast = new L.LatLng(50.5, -120.7);
const southWest = new L.LatLng(50.3, -121.2);

const renderLinks = (props: ILayerPopupLinksProps) => {
  return render(<LayerPopupLinks {...props} />, {
    mockMapMachine: mapMachineMock as unknown as any,
  });
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

  it(`Zooms the map to property bounds when Zoom link is clicked`, async () => {
    // render popup
    const { getByText } = renderLinks({ bounds: new L.LatLngBounds(southWest, northEast) });
    const link = getByText(/Zoom/i);
    expect(link).toBeInTheDocument();
    // click link
    await act(async () => userEvent.click(link));
    expect(mapMachineMock.requestFlyToBounds).toBeCalled();
  });
});
