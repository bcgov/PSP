import { AreaUnitTypes } from '@/constants/index';
import { fillInput, render, RenderOptions, waitFor } from '@/utils/test-utils';

import { AreaForm, IAreaFormProps } from './AreaForm';

describe('LandMeasurementTable component', () => {
  // render component under test
  const setup = (props: RenderOptions & IAreaFormProps) => {
    const utils = render(
      <AreaForm
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
    const { asFragment } = setup({
      onChange: () => {},
      area: undefined,
      areaUnitTypeCode: undefined,
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls onChange callback when values are changed', async () => {
    const onChange = jest.fn();
    const { container } = setup({
      onChange,
      area: undefined,
      areaUnitTypeCode: undefined,
    });
    await fillInput(container, 'area-sq-meters', 15000);
    await waitFor(() => expect(onChange).toBeCalledWith(15000, AreaUnitTypes.SquareMeters));
  });

  it('performs unit conversions when values are changed', async () => {
    const { container, getSqFeetInput, getHectaresInput, getAcresInput } = setup({
      onChange: () => {},
      area: undefined,
      areaUnitTypeCode: undefined,
    });
    await fillInput(container, 'area-sq-meters', 15000);
    // assert
    await waitFor(() => expect(getSqFeetInput().valueAsNumber).toBe(161458.66));
    await waitFor(() => expect(getHectaresInput().valueAsNumber).toBe(1.5));
    await waitFor(() => expect(getAcresInput().valueAsNumber).toBe(3.71));
  });
});
