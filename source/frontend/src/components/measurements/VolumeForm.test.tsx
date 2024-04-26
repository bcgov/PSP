import { VolumeUnitTypes } from '@/constants/index';
import { fillInput, render, RenderOptions, waitFor } from '@/utils/test-utils';

import { IVolumeFormProps, VolumeForm } from './VolumeForm';

describe('VolumeForm component', () => {
  // render component under test
  const setup = (props: RenderOptions & IVolumeFormProps) => {
    const utils = render(
      <VolumeForm
        volume={props.volume}
        volumeUnitTypeCode={props.volumeUnitTypeCode}
        onChange={props.onChange}
      />,
      {
        ...props,
      },
    );

    return {
      ...utils,
      getCubicMetersInput: () =>
        utils.getByRole('spinbutton', { name: /cubic metres/i }) as HTMLInputElement,
      getCubicFeetInput: () =>
        utils.getByRole('spinbutton', { name: /cubic feet/i }) as HTMLInputElement,
    };
  };

  beforeEach(() => {});

  afterEach(() => {
    vi.resetAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({
      onChange: () => {},
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('calls onChange callback when values are changed', async () => {
    const onChange = vi.fn();
    const { container } = setup({ onChange });
    await fillInput(container, 'volume-cubic-meters', 15000);
    await waitFor(() => expect(onChange).toBeCalledWith(15000, VolumeUnitTypes.CubicMeters));
  });

  it('performs unit conversions when values are changed', async () => {
    const { container, getCubicFeetInput } = setup({
      onChange: () => {},
    });
    await fillInput(container, 'volume-cubic-meters', 15000);
    await waitFor(() => expect(getCubicFeetInput().valueAsNumber).toBe(535714.29));
  });
});
