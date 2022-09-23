import React from 'react';
import { cleanup, render } from 'utils/test-utils';

import FilterBar from './FilterBar';

const componentRender = () => {
  return render(
    <div>
      <FilterBar
        initialValues={{ businessIdentifierValue: 'test', firstName: 'user' }}
        onChange={() => {}}
      />
    </div>,
  );
};

describe('Filter Bar', () => {
  afterEach(cleanup);

  it('Renders correctly', () => {
    const { asFragment } = componentRender();
    expect(asFragment()).toMatchSnapshot();
  });
});
