import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { mockParcel } from 'mocks/filterDataMock';
import { render, RenderOptions } from 'utils/test-utils';

import DetailDocumentation, { IDetailDocumentationProps } from './DetailDocumentation';

const history = createMemoryHistory();

describe('DetailDocumentation component', () => {
  const setup = (
    renderOptions: RenderOptions & IDetailDocumentationProps & { lease?: IFormLease } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? defaultFormLease}>
        <DetailDocumentation
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
        programName: 'A program',
        lFileNo: '222',
        tfaFileNumber: '333',
        psFileNo: '444',
        motiName: 'test moti name',
        note: 'a test note',
        expiryDate: '2022-01-01',
        hasDigitalLicense: 'Yes',
        hasPhysicalLicense: 'No',
        startDate: '2020-01-01',
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders the Physical lease/license exists field', () => {
    const {
      component: { getAllByDisplayValue },
    } = setup({
      lease: {
        ...defaultFormLease,
        hasPhysicalLicense: 'Unknown',
        hasDigitalLicense: 'Yes',
      },
    });
    expect(getAllByDisplayValue('Unknown')[1]).toBeVisible();
  });

  it('renders the Digital lease/license exists field', () => {
    const {
      component: { getAllByDisplayValue },
    } = setup({
      lease: {
        ...defaultFormLease,
        hasPhysicalLicense: 'Yes',
        hasDigitalLicense: 'Unknown',
      },
    });
    expect(getAllByDisplayValue('Unknown')[0]).toBeVisible();
  });

  it('renders the Location of documents field', () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        ...defaultFormLease,
        documentationReference: 'documentation Reference',
      },
    });
    expect(getByDisplayValue('documentation Reference')).toBeVisible();
  });

  it('renders the PS # name', () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        ...defaultFormLease,
        psFileNo: 'A PS File No',
      },
    });
    expect(getByDisplayValue('A PS File No')).toBeVisible();
  });
});
