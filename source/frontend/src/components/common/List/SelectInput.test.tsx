import { render } from '@testing-library/react';
import { useFormikContext } from 'formik';
import React from 'react';

import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';

import { SelectInput } from './SelectInput';

jest.mock('formik');

(useFormikContext as jest.Mock).mockReturnValue({
  values: {},
  setFieldValue: jest.fn(),
});
describe('SelectInput tests', () => {
  it('renders correctly...', () => {
    const { asFragment } = render(
      <SelectInput<
        {
          pinOrPid: string;
          address: string;
        },
        IPropertyFilter
      >
        field="searchBy"
        defaultKey="pinOrPid"
        selectOptions={[
          { label: 'PID/PIN', key: 'pinOrPid', placeholder: 'Enter a PID or PIN' },
          { label: 'Address', key: 'address', placeholder: 'Enter an address' },
        ]}
        className="idir-input-group"
      />,
    );
    expect(asFragment()).toMatchSnapshot();
  });

  it('disabled prop works as intended...', () => {
    const { container } = render(
      <SelectInput<
        {
          pinOrPid: string;
          address: string;
        },
        IPropertyFilter
      >
        field="searchBy"
        defaultKey="pinOrPid"
        selectOptions={[
          { label: 'PID/PIN', key: 'pinOrPid', placeholder: 'Enter a PID or PIN' },
          { label: 'Address', key: 'address', placeholder: 'Enter an address' },
        ]}
        className="idir-input-group"
        disabled
      />,
    );
    const searchBy = container.querySelector('select[name="searchBy"]');
    expect(searchBy).toHaveAttribute('disabled');
  });
});
