import React from 'react';
import { create } from 'react-test-renderer';

import { TablePageSizeSelector } from './PageSizeSelector';

const componentRender = () => {
  const component = create(
    <div>
      <TablePageSizeSelector alignTop value={1} options={[1, 2, 3, 4, 5]} onChange={() => {}} />
    </div>,
  );
  return component;
};

describe('Page size selector', () => {
  it('Snapshot matches', () => {
    const component = componentRender();
    expect(component.toJSON()).toMatchSnapshot();
  });
});
