import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { IProperty } from '@/interfaces';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockProperties } from '@/mocks/properties.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import ExpandableTextList, { IExpandableTextListProps } from './ExpandableTextList';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('ExpandableTextList component', () => {
  const mockAxios = new MockAdapter(axios);
  const setup = (renderOptions?: RenderOptions & IExpandableTextListProps<IProperty>) => {
    // render component under test
    const component = render(
      <ExpandableTextList<IProperty>
        items={renderOptions?.items ?? []}
        renderFunction={renderOptions?.renderFunction ?? (noop as any)}
        keyFunction={renderOptions?.keyFunction ?? (noop as any)}
        delimiter={renderOptions?.delimiter}
        maxCollapsedLength={renderOptions?.maxCollapsedLength}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    mockAxios.reset();
  });

  it('renders all items if no maxCollapsedLength specified', async () => {
    const { getByText } = setup({
      items: getMockProperties(),
      renderFunction: (item: IProperty) => <span>{item.pid}</span>,
      keyFunction: (item: IProperty, index: number) => item.id?.toString() ?? index.toString(),
      delimiter: '|',
    });
    expect(getByText('000-000-000')).toBeVisible();
    expect(getByText('000-000-001')).toBeVisible();
    expect(getByText('000-000-002')).toBeVisible();
  });

  it('only renders delimiters between items', async () => {
    const { getAllByText } = setup({
      items: getMockProperties(),
      renderFunction: (item: IProperty) => <span>{item.pid}</span>,
      keyFunction: (item: IProperty, index: number) => item.id?.toString() ?? index.toString(),
      delimiter: 'delimiter',
    });
    expect(getAllByText('delimiter')).toHaveLength(2);
  });

  it('renders the maxCollapsedLength items if specified', async () => {
    const { getByText, queryByText } = setup({
      items: getMockProperties(),
      renderFunction: (item: IProperty) => <span>{item.pid}</span>,
      keyFunction: (item: IProperty, index: number) => item.id?.toString() ?? index.toString(),
      delimiter: '|',
      maxCollapsedLength: 2,
    });
    expect(getByText('000-000-000')).toBeVisible();
    expect(getByText('000-000-001')).toBeVisible();
    expect(queryByText('000-000-002')).toBeNull();
  });

  it('renders the more button if maxCollapsedLength less then items length', async () => {
    const { getByText } = setup({
      items: getMockProperties(),
      renderFunction: (item: IProperty) => <span>{item.pid}</span>,
      keyFunction: (item: IProperty, index: number) => item.id?.toString() ?? index.toString(),
      delimiter: '|',
      maxCollapsedLength: 2,
    });
    expect(getByText('[+1 more...]')).toBeVisible();
  });

  it('does not render the more button if maxCollapsedLength greater than or equal to items length', async () => {
    const { queryByTestId } = setup({
      items: getMockProperties(),
      renderFunction: (item: IProperty) => <span>{item.pid}</span>,
      keyFunction: (item: IProperty, index: number) => item.id?.toString() ?? index.toString(),
      delimiter: '|',
      maxCollapsedLength: 3,
    });
    expect(queryByTestId('expand')).toBeNull();
  });

  it('renders all items if less then maxCollapsedLength', async () => {
    const { getByText } = setup({
      items: getMockProperties(),
      renderFunction: (item: IProperty) => <span>{item.pid}</span>,
      keyFunction: (item: IProperty, index: number) => item.id?.toString() ?? index.toString(),
      delimiter: '|',
      maxCollapsedLength: 4,
    });
    expect(getByText('000-000-000')).toBeVisible();
    expect(getByText('000-000-001')).toBeVisible();
    expect(getByText('000-000-002')).toBeVisible();
  });

  it('renders all items if more button clicked', async () => {
    const { getByText } = setup({
      items: getMockProperties(),
      renderFunction: (item: IProperty) => <span>{item.pid}</span>,
      keyFunction: (item: IProperty, index: number) => item.id?.toString() ?? index.toString(),
      delimiter: '|',
      maxCollapsedLength: 2,
    });

    const moreButton = getByText('[+1 more...]');
    act(() => {
      userEvent.click(moreButton);
    });
    expect(getByText('000-000-000')).toBeVisible();
    expect(getByText('000-000-001')).toBeVisible();
    expect(getByText('000-000-002')).toBeVisible();
  });

  it('renders maxCollapsedLength items if collapsed', async () => {
    const { getByTestId, queryByText, getByText } = setup({
      items: getMockProperties(),
      renderFunction: (item: IProperty) => <span>{item.pid}</span>,
      keyFunction: (item: IProperty, index: number) => item.id?.toString() ?? index.toString(),
      delimiter: '|',
      maxCollapsedLength: 2,
    });

    const moreButton = getByTestId('expand');
    act(() => {
      userEvent.click(moreButton);
    });
    const hideButton = getByText('hide');
    act(() => {
      userEvent.click(hideButton);
    });
    expect(getByText('000-000-000')).toBeVisible();
    expect(getByText('000-000-001')).toBeVisible();
    expect(queryByText('000-000-002')).toBeNull();
  });
});
