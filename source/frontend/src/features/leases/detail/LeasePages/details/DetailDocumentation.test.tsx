import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { mockApiProperty } from '@/mocks/filterData.mock';
import { getEmptyPropertyLease } from '@/mocks/properties.mock';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { getEmptyLease } from '@/models/defaultInitializers';
import { toTypeCode } from '@/utils/formUtils';
import { render, RenderOptions } from '@/utils/test-utils';

import DetailDocumentation, { IDetailDocumentationProps } from './DetailDocumentation';

const history = createMemoryHistory();

describe('DetailDocumentation component', () => {
  const setup = (
    renderOptions: RenderOptions &
      IDetailDocumentationProps & { lease?: ApiGen_Concepts_Lease } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? getEmptyLease()}>
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
        ...getEmptyLease(),
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            property: {
              ...mockApiProperty,
              areaUnit: toTypeCode('test'),
              landArea: 123,
            },
            fileId: 1,
          },
        ],
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders a complete lease as expected', () => {
    const { component } = setup({
      lease: {
        ...getEmptyLease(),
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            property: {
              ...mockApiProperty,
              areaUnit: toTypeCode('test'),
              landArea: 123,
            },
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
        ...getEmptyLease(),
        hasPhysicalLicense: true,
      },
    });
    expect(getByDisplayValue('Yes')).toBeVisible();
  });

  it('renders the Digital lease/license exists field', () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        ...getEmptyLease(),
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
        ...getEmptyLease(),
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
        ...getEmptyLease(),
        psFileNo: 'A PS File No',
      },
    });
    expect(getByDisplayValue('A PS File No')).toBeVisible();
  });
});
