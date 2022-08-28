import Claims from 'constants/claims';
import { createMemoryHistory } from 'history';
import { IProperty } from 'interfaces/IProperty';
import { mockLookups } from 'mocks/mockLookups';
import { useMap } from 'react-leaflet';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, userEvent } from 'utils/test-utils';

import { ILayerPopupProps } from './LayerPopup';
import { LayerPopupContainer } from './LayerPopupContainer';

jest.mock('@react-keycloak/web');
jest.mock('react-leaflet');

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};
const mockProperty: IProperty = {
  id: 52,
  pid: '009456789',
  address: {
    id: 1,
    streetAddress1: '1234 mock Street',
    streetAddress2: 'N/A',
    municipality: 'Victoria',
    provinceId: 1,
    province: 'BC',
    postal: 'V1V1V1',
  },
};

const onViewPropertyInfo = jest.fn();
const onClose = jest.fn();
const history = createMemoryHistory();
(useMap as jest.Mock).mockReturnValue({});

describe('LayerPopupContainer component', () => {
  const setup = (renderOptions: RenderOptions & ILayerPopupProps) => {
    // render component under test
    const component = render(
      <LayerPopupContainer
        propertyInfo={mockProperty}
        layerPopup={renderOptions.layerPopup}
        onViewPropertyInfo={renderOptions.onViewPropertyInfo}
        onClose={onClose}
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
      propertyInfo: mockProperty,
      layerPopup: {} as any,
      onViewPropertyInfo: onViewPropertyInfo,
    });
    expect(asFragment()).toMatchSnapshot();
  });
  describe('fly out behaviour', () => {
    it('fly out is hidden by default', async () => {
      const { queryByText } = setup({
        propertyInfo: mockProperty,
        layerPopup: {} as any,
        onViewPropertyInfo: onViewPropertyInfo,
      });
      expect(queryByText('View more property info')).toBeNull();
    });

    it('opens fly out when ellipsis is clicked', async () => {
      const { getByTestId, getByText } = setup({
        propertyInfo: mockProperty,
        layerPopup: {} as any,
        onViewPropertyInfo: onViewPropertyInfo,
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      userEvent.click(ellipsis);
      expect(getByText('View more property info')).toBeVisible();
    });

    it('handles view property action', async () => {
      const { getByTestId, getByText } = setup({
        propertyInfo: mockProperty,
        layerPopup: {} as any,
        onViewPropertyInfo: onViewPropertyInfo,
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      userEvent.click(ellipsis);
      const link = getByText('View more property info');
      userEvent.click(link);
      expect(onViewPropertyInfo).toHaveBeenCalled();
    });

    it('handles create research file action', async () => {
      const { getByTestId, getByText } = setup({
        propertyInfo: mockProperty,
        layerPopup: {} as any,
        onViewPropertyInfo: onViewPropertyInfo,
        claims: [Claims.RESEARCH_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      userEvent.click(ellipsis);
      const link = getByText('Research File - Create new');
      userEvent.click(link);
      expect(history.location.pathname).toBe('/mapview/sidebar/research/new');
    });

    it('handles create acquisition file action', async () => {
      const { getByTestId, getByText } = setup({
        propertyInfo: mockProperty,
        layerPopup: {} as any,
        onViewPropertyInfo: onViewPropertyInfo,
        claims: [Claims.ACQUISITION_ADD],
      });
      const ellipsis = getByTestId('fly-out-ellipsis');
      userEvent.click(ellipsis);
      const link = getByText('Acquisition File - Create new');
      userEvent.click(link);
      expect(history.location.pathname).toBe('/mapview/sidebar/acquisition/new');
    });
  });
});
