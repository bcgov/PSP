import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { ColumnWithProps, Table, TableProps } from '.';
import { IIdentifiedObject } from './Table';

const setSort = jest.fn();

interface test extends IIdentifiedObject {
  name: string;
}

const testColumns: ColumnWithProps<test>[] = [
  {
    Header: 'Column number',
    accessor: 'name',
    align: 'left',
    width: 20,
    maxWidth: 40,
  },
  {
    Header: 'Column name',
    accessor: 'id',
    align: 'left',
    width: 20,
    maxWidth: 40,
  },
];

const testData: test[] = [
  { name: 'one', id: 1 },
  { name: 'two', id: 2 },
  { name: 'three', id: 3 },
  { name: 'four', id: 4 },
  { name: 'five', id: 5 },
  { name: 'six', id: 6 },
];

// render component under test
const setup = <T extends IIdentifiedObject>(
  renderOptions: RenderOptions & { props?: Partial<TableProps<T>> },
) => {
  const { props: tableProps, ...rest } = renderOptions;
  const utils = render(
    <Table<T>
      {...tableProps}
      name={tableProps?.name ?? 'default'}
      columns={tableProps?.columns ?? (testColumns as any)}
      data={(tableProps?.data ?? testData) as any}
    />,
    {
      ...rest,
    },
  );
  const tableRows = utils.container.querySelectorAll('.table .tbody .tr-wrapper');
  return {
    ...utils,
    tableRows,
  };
};

describe('Generic table component', () => {
  beforeEach(() => {
    setSort.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows, getByText } = setup({ props: { data: [] } });

    expect(tableRows.length).toBe(0);
    expect(getByText('No rows to display')).toBeVisible();
  });

  describe('table pagination', () => {
    it('by default the first page is the active page', async () => {
      const { getByLabelText } = setup({ props: { pageSize: 5 } });

      const page1Button = getByLabelText('Page 1 is your current page');
      expect(page1Button.parentElement).toHaveClass('active');
    });

    it('it displays a maximum number of rows equal to the pagesize prop if manualPagination is false', async () => {
      const { tableRows } = setup({ props: { pageSize: 5, manualPagination: false } });

      expect(tableRows).toHaveLength(5);
    });

    it('it displays all rows if manualPagination is true', async () => {
      const { tableRows } = setup({ props: { pageSize: 5, manualPagination: true } });

      expect(tableRows).toHaveLength(6);
    });

    it('allows the current page to be changed', async () => {
      const { getByLabelText, getByText } = setup({ props: { pageSize: 5 } });

      const page2Button = getByLabelText('Page 2');
      act(() => {
        userEvent.click(page2Button);
      });
      expect(getByText('six')).toBeVisible();
      expect(page2Button.parentElement).toHaveClass('active');
    });

    it('displays multiple pages when the total number of rows exceed the page size', async () => {
      const { getByLabelText } = setup({ props: { pageSize: 5 } });

      expect(getByLabelText('Page 2')).toBeVisible();
    });

    it('hides the paging control when hidePagination is set', async () => {
      const { tableRows, queryByLabelText } = setup({
        props: { pageSize: 5, hidePagination: true },
      });

      const page1Button = queryByLabelText('Page 1 is your current page');
      expect(page1Button).toBeNull();
      expect(tableRows).toHaveLength(6);
    });

    it('does not change the page when manual pagination is set', async () => {
      const onRequestData = jest.fn();
      const { getByLabelText, tableRows } = setup({
        props: { pageSize: 5, manualPagination: true, onRequestData },
      });

      const page2Button = getByLabelText('Page 2');
      act(() => {
        userEvent.click(page2Button);
      });
      expect(tableRows).toHaveLength(6); //pagination is handled externally, the table component will ignore the page size prop.
      expect(onRequestData).toHaveBeenCalled();
    });

    it('page size works as expected when not using manual pagination', async () => {
      const { getByLabelText, getByTitle, container } = setup({
        props: { pageSize: 10, manualPagination: false },
      });

      await act(async () => {
        userEvent.click(getByTitle('menu-item-5'));
      });
      const tableRows = container.querySelectorAll('.table .tbody .tr-wrapper');
      expect(tableRows).toHaveLength(5);
      expect(getByLabelText('Page 2')).toBeVisible();
    });

    it('page size works as expected when changing page size and then changing page', async () => {
      const { getByLabelText, getByTitle, container } = setup({
        props: { manualPagination: false },
      });

      await act(async () => userEvent.click(getByTitle('menu-item-5')));
      const tableRows = container.querySelectorAll('.table .tbody .tr-wrapper');
      expect(tableRows).toHaveLength(5);
      await act(async () => userEvent.click(getByLabelText('Page 2')));
      expect(getByLabelText('Page 2 is your current page')).toBeVisible();
    });

    it('page size works as expected when using manual pagination', async () => {
      const onPageSizeChange = jest.fn();
      const { getByTitle } = setup({
        props: { pageSize: 10, manualPagination: true, onPageSizeChange },
      });

      act(() => {
        userEvent.click(getByTitle('menu-item-5'));
      });
      expect(onPageSizeChange).toHaveBeenCalledWith(5);
    });
  });

  it('can select only one row at a time when single select prop set', async () => {
    const setSelectedRows = jest.fn();
    const { getByTestId } = setup({
      props: { pageSize: 5, isSingleSelect: true, setSelectedRows, showSelectedRowCount: true },
    });

    const selectRowOneButton = getByTestId('selectrow-1');
    act(() => {
      userEvent.click(selectRowOneButton);
    });

    const selectRowTwoButton = getByTestId('selectrow-2');
    act(() => {
      userEvent.click(selectRowTwoButton);
    });

    expect(setSelectedRows).toHaveBeenNthCalledWith(1, [{ id: 1, name: 'one' }]);
    expect(setSelectedRows).toHaveBeenNthCalledWith(2, [{ id: 2, name: 'two' }]);
    expect(selectRowOneButton).toHaveAttribute('type', 'radio');
    expect(selectRowTwoButton).toHaveAttribute('type', 'radio');
  });

  it('can select multiple rows when isSingleSelect is false', async () => {
    const setSelectedRows = jest.fn();
    const { getByTestId } = setup({
      props: { pageSize: 5, isSingleSelect: false, setSelectedRows, showSelectedRowCount: true },
    });

    const selectRowOneButton = getByTestId('selectrow-1');
    act(() => {
      userEvent.click(selectRowOneButton);
    });
    const selectRowTwoButton = getByTestId('selectrow-2');
    act(() => {
      userEvent.click(selectRowTwoButton);
    });

    expect(setSelectedRows).toHaveBeenNthCalledWith(1, [{ id: 1, name: 'one' }]);
    expect(setSelectedRows).toHaveBeenNthCalledWith(2, [{ id: 2, name: 'two' }]);

    expect(selectRowOneButton).toHaveAttribute('type', 'checkbox');
    expect(selectRowOneButton).toBeChecked();
    expect(selectRowTwoButton).toBeChecked();
  });

  it('can select multiple rows when isSingleSelect is false and page is changed', async () => {
    const setSelectedRows = jest.fn();
    const { getByTestId, getByLabelText, findByLabelText } = setup({
      props: {
        pageSize: 1,
        isSingleSelect: false,
        setSelectedRows,
        showSelectedRowCount: true,
        manualPagination: false,
      },
    });

    const selectRowOneButton = getByTestId('selectrow-1');
    act(() => {
      userEvent.click(selectRowOneButton);
    });

    const page2Button = getByLabelText('Page 2');
    act(() => {
      userEvent.click(page2Button);
    });
    await findByLabelText('Page 2 is your current page');

    const selectRowTwoButton = getByTestId('selectrow-2');
    act(() => {
      userEvent.click(selectRowTwoButton);
    });

    expect(setSelectedRows).toHaveBeenNthCalledWith(1, [{ id: 1, name: 'one' }]);
    expect(setSelectedRows).toHaveBeenNthCalledWith(2, [{ id: 2, name: 'two' }]);

    expect(selectRowOneButton).toHaveAttribute('type', 'checkbox');
    expect(selectRowOneButton).toBeChecked();
    expect(selectRowTwoButton).toBeChecked();
  });
});
