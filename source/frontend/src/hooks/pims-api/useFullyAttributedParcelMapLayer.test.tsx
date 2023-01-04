import { act, renderHook } from '@testing-library/react-hooks';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { mockFAParcelLayerResponse } from 'mocks';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import { useFullyAttributedParcelMapLayer } from '.';

const mockAxios = new MockAdapter(axios);
const mockStore = configureMockStore([thunk]);

const wfsUrl = 'ogs-internal/ows';
const wfsLayer = 'PMBC_PARCEL_POLYGON_FABRIC';

describe('useFullyAttributedParcelMapLayer hook', () => {
  const setup = () => {
    const { result } = renderHook(() => useFullyAttributedParcelMapLayer(wfsUrl, wfsLayer), {
      wrapper: (props: React.PropsWithChildren) => (
        <TestCommonWrapper store={mockStore}>{props.children}</TestCommonWrapper>
      ),
    });

    return result.current;
  };

  beforeEach(() => {
    mockAxios.reset();
    mockAxios.onGet(new RegExp('ogs-internal/ows')).reply(200, mockFAParcelLayerResponse);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('searches map layer by legal description', async () => {
    const { findByLegalDescription, loadingIndicator } = setup();

    await act(async () => {
      const response = await findByLegalDescription('some legal description');
      expect(response).toStrictEqual(mockFAParcelLayerResponse);
    });

    expect(loadingIndicator).toBe(false);
    expect(mockAxios.history.get).toHaveLength(1);
    expect(mockAxios.history.get[0].url).toBe(
      'http://localhost/ogs-internal/ows?service=WFS&version=2.0.0&outputFormat=json&typeNames=PMBC_PARCEL_POLYGON_FABRIC&srsName=EPSG%3A4326&request=GetFeature&cql_filter=LEGAL_DESCRIPTION+ilike+%27%25some+legal+description%25%27',
    );
  });
});
