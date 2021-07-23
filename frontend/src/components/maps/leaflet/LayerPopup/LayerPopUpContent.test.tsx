import { cleanup, render } from '@testing-library/react';
import { SidebarContextType } from 'features/mapSideBar/hooks/useQueryParamSideBar';
import { createMemoryHistory } from 'history';
import L from 'leaflet';
import queryString from 'query-string';
import React from 'react';
import { useMap } from 'react-leaflet';
import { Router } from 'react-router-dom';
import { makeRouterProvider } from 'utils/test-utils';

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
  const wrapper = makeRouterProvider(history);
  return render(<LayerPopupContent {...props} />, { wrapper });
};

describe('Layer Popup Content', () => {
  afterEach(cleanup);

  it('Renders correctly', () => {
    const { asFragment } = renderPopup(mockLayer);
    expect(asFragment()).toMatchSnapshot();
  });

  it('Populate details link does not appear on default', () => {
    const { queryByText } = renderPopup(mockLayer);
    const link = queryByText(/Populate property details/i);
    expect(link).toBeNull();
  });

  it('Populate details link appears when sideBar open', () => {
    history.location.search = queryString.stringify({
      disabled: false,
      loadDraft: false,
      sidebar: true,
      sidebarContext: SidebarContextType.ADD_BUILDING,
    });
    const { getByText } = renderPopup(mockLayer);
    const link = getByText(/Populate property details/i);
    expect(link).toBeInTheDocument();
  });

  it('Zoom link does not appear without bounds', () => {
    const { queryByText } = renderPopup(mockLayer);
    const link = queryByText(/Zoom/i);
    expect(link).toBeNull();
  });
});
