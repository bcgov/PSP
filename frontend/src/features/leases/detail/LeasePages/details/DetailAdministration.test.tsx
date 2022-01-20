import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { mockParcel } from 'mocks/filterDataMock';
import { render, RenderOptions } from 'utils/test-utils';

import DetailAdministration, { IDetailAdministrationProps } from './DetailAdministration';

const history = createMemoryHistory();

describe('DetailAdministration component', () => {
  const setup = (
    renderOptions: RenderOptions & IDetailAdministrationProps & { lease?: IFormLease } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? defaultFormLease}>
        <DetailAdministration
          disabled={renderOptions.disabled}
          nameSpace={renderOptions.nameSpace}
        />
      </Formik>,
      {
        ...renderOptions,
        history,
      },
    );

    return {
      component,
    };
  };
  it('renders minimally as expected', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, properties: [mockParcel] },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders a complete lease as expected', () => {
    const { component } = setup({
      lease: {
        ...defaultFormLease,
        properties: [mockParcel],
        amount: 1,
        description: 'a test description',
        landArea: 111,
        areaUnit: 'Hectares',
        programName: 'A program',
        lFileNo: '222',
        tfaFileNo: 333,
        psFileNo: '444',
        motiName: 'test moti name',
        note: 'a test note',
        expiryDate: '2022-01-01',
        startDate: '2020-01-01',
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders the program name', () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        ...defaultFormLease,
        programName: 'A program',
      },
    });
    expect(getByDisplayValue('A program')).toBeVisible();
  });
});
