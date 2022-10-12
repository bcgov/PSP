import { AreaUnitTypes } from 'constants/index';
import { fillInput, render, RenderOptions, waitFor } from 'utils/test-utils';

import {
  IUpdateLandMeasurementEditTableProps,
  LandMeasurementEditTable,
} from './LandMeasurementEditTable';

describe('LandMeasurementTable component', () => {
  // render component under test
  const setup = (props: RenderOptions & IUpdateLandMeasurementEditTableProps = {}) => {
    const utils = render(
      <LandMeasurementEditTable
        area={props.area}
        areaUnitTypeCode={props.areaUnitTypeCode}
        onChange={props.onChange}
      />,
      {
        ...props,
      },
    );

    return {
      ...utils,
      getSqMetersInput: () =>
        utils.getByRole('spinbutton', { name: /square metres/i }) as HTMLInputElement,
      getSqFeetInput: () =>
        utils.getByRole('spinbutton', { name: /square feet/i }) as HTMLInputElement,
      getHectaresInput: () =>
        utils.getByRole('spinbutton', { name: /hectares/i }) as HTMLInputElement,
      getAcresInput: () => utils.getByRole('spinbutton', { name: /acres/i }) as HTMLInputElement,
    };
  };

  beforeEach(() => {});

  afterEach(() => {
    jest.resetAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls onChange callback when values are changed', async () => {
    const onChange = jest.fn();
    const { container } = setup({ onChange });
    await fillInput(container, 'area-sq-meters', 15000);
    await waitFor(() => expect(onChange).toBeCalledWith(15000, AreaUnitTypes.SquareMeters));
  });

  it('performs unit conversions when values are changed', async () => {
    const { container, getSqFeetInput, getHectaresInput, getAcresInput } = setup();
    await fillInput(container, 'area-sq-meters', 15000);
    // assert
    await waitFor(() => expect(getSqFeetInput().valueAsNumber).toBe(161458.66));
    await waitFor(() => expect(getHectaresInput().valueAsNumber).toBe(1.5));
    await waitFor(() => expect(getAcresInput().valueAsNumber).toBe(3.71));
  });
});
