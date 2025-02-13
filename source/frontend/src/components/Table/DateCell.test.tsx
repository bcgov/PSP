import { render, RenderOptions, screen } from '@/utils/test-utils';
import { CellProps } from 'react-table';

import { UtcDateCell } from './DateCell';

describe('DateCell table renderer', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & {
      props?: Partial<CellProps<any, string | Date | undefined | null>>;
    } = {},
  ) => {
    const utils = render(<UtcDateCell {...(renderOptions?.props ?? ({} as any))} />, {
      ...renderOptions,
    });

    return { ...utils };
  };

  it('UtcDateCell displays the correct date when the date is after 5pm PST', async () => {
    setup({
      props: { cell: { value: '2025-02-12T00:59:37.953' } } as any,
    });
    expect(await screen.findByText('Feb 11, 2025')).toBeVisible();
  });
});
