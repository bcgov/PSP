import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { ITablePageSizeSelectorProps, TablePageSizeSelector } from './PageSizeSelector';

const setup = (
  renderOptions: RenderOptions & { props?: Partial<ITablePageSizeSelectorProps> } = {},
) => {
  const { props: selectorProps, ...rest } = renderOptions;
  const utils = render(
    <TablePageSizeSelector
      alignTop
      value={selectorProps?.value ?? 10}
      options={selectorProps?.options ?? [5, 10, 20, 50, 100]}
      onChange={selectorProps?.onChange ?? vi.fn}
    />,
    rest,
  );
  return { ...utils };
};

describe('Page size selector', () => {
  it('matches snapshot', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it(`calls 'onChange' when page size is changed`, async () => {
    const onChange = vi.fn();
    const { getByTitle } = setup({ props: { onChange } });

    await act(async () => {
      userEvent.click(getByTitle('menu-item-5'));
    });

    expect(onChange).toHaveBeenCalledWith(5);
  });

  it(`displays the new page size value when page size is changed`, async () => {
    const onChange = vi.fn();
    const { getByTitle, getByTestId } = setup({ props: { onChange } });

    await act(async () => {
      userEvent.click(getByTitle('menu-item-50'));
    });

    expect(onChange).toHaveBeenCalledWith(50);
    expect(getByTestId('input-page-size')).toHaveValue(50);
  });
});
