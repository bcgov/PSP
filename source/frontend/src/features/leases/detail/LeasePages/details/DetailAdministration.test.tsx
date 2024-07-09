import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { DetailAdministration, IDetailAdministrationProps } from '@/features/leases';
import { mockApiProperty } from '@/mocks/filterData.mock';
import { getEmptyPropertyLease } from '@/mocks/properties.mock';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { getEmptyLease } from '@/models/defaultInitializers';
import { toTypeCode } from '@/utils/formUtils';
import { render, RenderOptions } from '@/utils/test-utils';

const history = createMemoryHistory();

describe('DetailAdministration component', () => {
  const setup = (
    renderOptions: RenderOptions &
      IDetailAdministrationProps & { lease?: ApiGen_Concepts_Lease } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? getEmptyLease()}>
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
        primaryArbitrationCity: 'VICTORIA',
        expiryDate: '2022-01-01',
        hasDigitalLicense: true,
        hasPhysicalLicense: false,
        startDate: '2020-01-01',
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders all other fields', () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        categoryType: { id: 'OTHER' },
        otherCategoryType: 'other category text',
        purposeType: { id: 'OTHER' },
        otherPurposeType: 'other purpose type',
        programType: { id: 'OTHER' },
        otherProgramType: 'other program type',
        type: { id: 'OTHER' },
        otherType: 'other type',
      } as any,
    });
    expect(getByDisplayValue('other category text')).toBeVisible();
    expect(getByDisplayValue('other purpose type')).toBeVisible();
    expect(getByDisplayValue('other program type')).toBeVisible();
    expect(getByDisplayValue('other type')).toBeVisible();
  });

  it('does not render other fields if values not set to other', () => {
    const {
      component: { queryByDisplayValue },
    } = setup({
      lease: {
        otherCategoryType: 'other category text',
        otherPurposeType: 'other purpose type',
        otherProgramType: 'other program type',
        otherType: 'other type',
      } as any,
    });
    expect(queryByDisplayValue('other category text')).toBeNull();
    expect(queryByDisplayValue('other purpose type')).toBeNull();
    expect(queryByDisplayValue('other program type')).toBeNull();
    expect(queryByDisplayValue('other type')).toBeNull();
  });

  it('renders the program name', () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        ...getEmptyLease(),
        programName: 'A program',
      },
    });
    expect(getByDisplayValue('A program')).toBeVisible();
  });

  it('renders the primary arbitration city', async () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        ...getEmptyLease(),
        primaryArbitrationCity: 'Vancouver',
      },
    });

    expect(getByDisplayValue('Vancouver')).toBeVisible();
  });
});
