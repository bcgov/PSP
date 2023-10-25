// import React from 'react';

// import { PropertyClassificationTypes } from '@/constants/propertyClassificationTypes';
// import { getIn, useFormikContext } from 'formik';
// import renderer from 'react-test-renderer';

// import { FastSelect } from './FastSelect';

// jest.mock('formik');

// (useFormikContext as jest.Mock).mockReturnValue({
//   values: {
//     classificationId: PropertyClassificationTypes.CoreOperational,
//     classification: 'zero',
//   },
//   registerField: jest.fn(),
//   unregisterField: jest.fn(),
// });
// (getIn as jest.Mock).mockReturnValue(0);

// const options = [
//   {
//     label: 'zero',
//     value: '0',
//     selected: true,
//   },
//   {
//     label: 'one',
//     value: '1',
//     selected: true,
//   },
//   {
//     label: 'two',
//     value: '2',
//     selected: true,
//   },
// ];

describe('LimitedSelect - Enzyme Tests - NEEDS REFACTORING', () => {
  it('should be implemented', async () => {});
  // it('limited fast select renders correctly', () => {
  //   const context = useFormikContext();
  //   const tree = renderer
  //     .create(
  //       <FastSelect
  //         limitLabels={['one']}
  //         formikProps={context}
  //         type="number"
  //         options={options}
  //         field={'TestField'}
  //       />,
  //     )
  //     .toJSON();
  //   expect(tree).toMatchSnapshot();
  // });
  //
  // it('only renders the limited options + the previous value', async () => {
  //   const context = useFormikContext();
  //   const component = mount(
  //     <FastSelect
  //       limitLabels={['one']}
  //       formikProps={context}
  //       type="number"
  //       options={options}
  //       field={'TestField'}
  //     />,
  //   );
  //   expect(component.find('option')).toHaveLength(2);
  // });
});

// TODO: Remove this line when unit tests above are fixed
export {};
