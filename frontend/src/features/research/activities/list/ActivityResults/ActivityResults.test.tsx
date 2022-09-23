import { mockActivitiesResponse } from 'mocks/mockActivities';
import React from 'react';
import { render, RenderOptions } from 'utils/test-utils';

import { ActivityResults, IActivityResultProps } from './ActivityResults';

const setSort = jest.fn();
// mock auth library
jest.mock('@react-keycloak/web');

// render component under test
const setup = (renderOptions: RenderOptions & Partial<IActivityResultProps> = { results: [] }) => {
  const { results, ...rest } = renderOptions;

  const utils = render(
    <ActivityResults
      sort={{}}
      results={results ?? []}
      setSort={setSort}
      onShowActivity={jest.fn()}
      onDelete={jest.fn()}
    />,
    {
      ...rest,
      claims: [],
    },
  );
  const tableRows = utils.container.querySelectorAll('.table .tbody .tr-wrapper');
  // long css selector to: get the FIRST header or table, then get the SVG up/down sort buttons
  const sortButtons = utils.container.querySelector(
    '.table .thead .tr .sortable-column div',
  ) as HTMLElement;
  return {
    ...utils,
    tableRows,
    sortButtons,
  };
};

describe('Activity Results Table', () => {
  beforeEach(() => {
    setSort.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup({ results: mockActivitiesResponse() });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a message when no matching records found', async () => {
    const { tableRows, findAllByText } = setup({ results: [] });

    expect(tableRows.length).toBe(0);
    const toasts = await findAllByText('No matching Activity found');
    expect(toasts[0]).toBeVisible();
  });
});
