import Claims from 'constants/claims';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { useMap } from 'react-leaflet';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from 'utils/test-utils';

import { ILayerPopupProps } from './LayerPopup';
import { LayerPopupContainer } from './LayerPopupContainer';

jest.mock('@react-keycloak/web');
jest.mock('react-leaflet');

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onViewPropertyInfo = jest.fn();
const history = createMemoryHistory();
(useMap as jest.Mock).mockReturnValue({});

describe('LayerPopupContainer component', () => {
  const setup = (renderOptions: RenderOptions & ILayerPopupProps) => {
    // render component under test
    const component = render(
      <LayerPopupContainer
        layerPopup={renderOptions.layerPopup}
        onViewPropertyInfo={renderOptions.onViewPropertyInfo}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
        useMockAuthentication: true,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    jest.resetAllMocks();
  });

  it('renders as expected with layer popup content', async () => {
    const { asFragment } = setup({
      layerPopup: {} as any,
      onViewPropertyInfo: onViewPropertyInfo,
    });
    expect(asFragment()).toMatchSnapshot();
  });
  describe('fly out behaviour', () => {
    it('fly out is hidden by default', async () => {
      const { queryByText } = setup({
        layerPopup: {} as any,
        onViewPropertyInfo: onViewPropertyInfo,
      });
      expect(queryByText('View more property info')).toBeNull();
    });

    it('opens fly out when ellipsis is clicked', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: {} as any,
        onViewPropertyInfo: onViewPropertyInfo,
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      expect(getByText('View more property info')).toBeVisible();
    });

    it('handles view property action for inventory properties', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: {
          pimsProperty: { properties: { PROPERTY_ID: 1 } },
          data: { PID: '123456789' },
        } as any,
        onViewPropertyInfo: onViewPropertyInfo,
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      const link = getByText('View more property info');
      act(() => userEvent.click(link));
      expect(onViewPropertyInfo).toHaveBeenCalledWith('123456789', 1);
    });

    it('handles view property action for non-inventory properties', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: { data: { PID: '123456789' } } as any,
        onViewPropertyInfo: onViewPropertyInfo,
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      const link = getByText('View more property info');
      act(() => userEvent.click(link));
      expect(onViewPropertyInfo).toHaveBeenCalledWith('123456789', undefined);
    });

    it('handles create research file action', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: {} as any,
        onViewPropertyInfo: onViewPropertyInfo,
        claims: [Claims.RESEARCH_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      const link = getByText('Research File - Create new');
      act(() => userEvent.click(link));
      expect(history.location.pathname).toBe('/mapview/sidebar/research/new');
    });

    it('handles create acquisition file action', async () => {
      const { getByTestId, getByText } = setup({
        layerPopup: {} as any,
        onViewPropertyInfo: onViewPropertyInfo,
        claims: [Claims.ACQUISITION_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      act(() => userEvent.click(ellipsis));
      const link = getByText('Acquisition File - Create new');
      act(() => userEvent.click(link));
      expect(history.location.pathname).toBe('/mapview/sidebar/acquisition/new');
    });
  });
});
