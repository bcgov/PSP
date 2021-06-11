import { fireEvent, render } from '@testing-library/react';
import noop from 'lodash/noop';
import React from 'react';
import renderer from 'react-test-renderer';

import SubmitPropertySelector from './SubmitPropertySelector';

describe('SubmitPropertySelector', () => {
  it('component renders correctly', () => {
    const tree = renderer.create(<SubmitPropertySelector addProperty={noop} />).toJSON();
    expect(tree).toMatchSnapshot();
  });

  it('calls addProperty when Add Property is clicked', () => {
    const addProperty = jest.fn();
    const { getByText } = render(<SubmitPropertySelector addProperty={addProperty} />);

    const addPropertyButton = getByText('Add Titled Property');
    fireEvent.click(addPropertyButton);

    expect(addProperty).toHaveBeenCalled();
  });
});
