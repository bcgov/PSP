import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { getDefaultFormLease, LeaseFormModel } from '@/features/leases/models';
import { mockParcel } from '@/mocks/filterData.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import DetailDocumentation, { IDetailDocumentationProps } from './DetailDocumentation';

const history = createMemoryHistory();

describe('DetailDocumentation component', () => {
  const setup = (
    renderOptions: RenderOptions & IDetailDocumentationProps & { lease?: LeaseFormModel } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? new LeaseFormModel()}>
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
      lease: {
        ...new LeaseFormModel(),
        properties: [
          {
            ...mockParcel,
            areaUnitTypeCode: 'test',
            landArea: '123',
            leaseId: null,
          },
        ],
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders a complete lease as expected', () => {
    const { component } = setup({
      lease: {
        ...new LeaseFormModel(),
        properties: [
          {
            ...mockParcel,
            areaUnitTypeCode: 'test',
            landArea: '123',
            leaseId: null,
          },
        ],
        amount: 1,
        description: 'a test description',
        programName: 'A program',
        lFileNo: '222',
        tfaFileNumber: '333',
        psFileNo: '444',
        motiName: 'test moti name',
        note: 'a test note',
        expiryDate: '2022-01-01',
        hasDigitalLicense: true,
        hasPhysicalLicense: false,
        startDate: '2020-01-01',
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders the Physical lease/license exists field', () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        ...getDefaultFormLease(),
        hasPhysicalLicense: true,
        hasDigitalLicense: undefined,
      },
    });
    expect(getByDisplayValue('Yes')).toBeVisible();
  });

  it('renders the Digital lease/license exists field', () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        ...getDefaultFormLease(),
        hasPhysicalLicense: undefined,
        hasDigitalLicense: true,
      },
    });
    expect(getByDisplayValue('Yes')).toBeVisible();
  });

  it('renders the Location of documents field', () => {
    const {
      component: { getByText },
    } = setup({
      lease: {
        ...getDefaultFormLease(),
        documentationReference: 'documentation Reference',
      },
    });
    expect(getByText('documentation Reference')).toBeVisible();
  });

  it('renders the PS # name', () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        ...getDefaultFormLease(),
        psFileNo: 'A PS File No',
      },
    });
    expect(getByDisplayValue('A PS File No')).toBeVisible();
  });
});
