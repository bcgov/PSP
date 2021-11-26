import React from 'react';
import { create } from 'react-test-renderer';

import FilterBar from './FilterBar';

const componentRender = () => {
  let component = create(
    <div>
      <FilterBar
        initialValues={{ businessIdentifierValue: 'test', firstName: 'user' }}
        onChange={() => {}}
      />
    </div>,
  );
  return component;
};

describe('Filter Bar', () => {
  it('Snapshot matches', () => {
    const component = componentRender();
    expect(component.toJSON()).toMatchSnapshot();
  });
});
