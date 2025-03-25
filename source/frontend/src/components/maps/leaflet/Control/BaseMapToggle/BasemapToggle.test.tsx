import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import BasemapToggle, { BasemapToggleEvent, BasemapToggleProps } from './BasemapToggle';
import { BaseLayer } from './types';

const onToggle = vi.fn();

const basemaps: BaseLayer[] = [
  {
    name: 'BC Roads',
    kind: 'esri-vector',
    itemId: 'b1624fea73bd46c681fab55be53d96ae',
    attribution: '',
    thumbnail: 'streets.jpg',
  },
  {
    name: 'Satellite',
    kind: 'raster',
    urls: [
      'https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/tile/{z}/{y}/{x}',
    ],
    attribution:
      'Tiles &copy; Esri &mdash; Source: Esri, DigitalGlobe, GeoEye, Earthstar Geographics, CNES/Airbus DS, USDA, USGS, AeroGRID, IGN, and the GIS User Community',
    thumbnail: 'satellite.jpg',
  },
];

const DEFAULT_PROPS: BasemapToggleProps = {
  baseLayers: basemaps,
  onToggle,
};

describe('Basemap Toggle', () => {
  // render component under test
  function setup(
    props: BasemapToggleProps = { ...DEFAULT_PROPS },
    renderOptions: RenderOptions = {},
  ) {
    const utils = render(
      <BasemapToggle baseLayers={props.baseLayers} onToggle={props.onToggle} />,
      {
        ...renderOptions,
      },
    );
    return { ...utils };
  }

  it('renders correctly - defaults to street layer', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls toggle event handler when toggled', async () => {
    const { getByAltText } = setup();
    await act(async () => userEvent.click(getByAltText(/Map Thumbnail/i)));
    expect(onToggle).toHaveBeenCalledTimes(1);
    expect(onToggle).toHaveBeenCalledWith<[BasemapToggleEvent]>({
      current: basemaps[1],
      previous: basemaps[0],
    });
  });

  it('shows satellite layer thumbnail when street layer is active', () => {
    const { getByAltText } = setup();
    const img = getByAltText(/Map Thumbnail/i);
    expect(img).toHaveAttribute('src', 'satellite.jpg');
  });

  it('shows street layer thumbnail when satellite layer is active', () => {
    const { getByAltText } = setup({
      ...DEFAULT_PROPS,
      baseLayers: [basemaps[1], basemaps[0]],
    });
    const img = getByAltText(/Map Thumbnail/i);
    expect(img).toHaveAttribute('src', 'streets.jpg');
  });
});
