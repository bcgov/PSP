import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { mockApiProperty } from '@/mocks/filterData.mock';
import { getEmptyPropertyLease } from '@/mocks/properties.mock';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { getEmptyLease } from '@/models/defaultInitializers';
import { toTypeCode } from '@/utils/formUtils';
import { render, RenderOptions } from '@/utils/test-utils';

import { DetailFeeDetermination, IDetailFeeDeterminationProps } from './DetailFeeDetermination';

const history = createMemoryHistory();

describe('DetailFeeDetermination component', () => {
  const setup = (
    renderOptions: RenderOptions &
      IDetailFeeDeterminationProps & { lease?: ApiGen_Concepts_Lease } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? getEmptyLease()}>
        <DetailFeeDetermination
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
        isPublicBenefit: true,
        isFinancialGain: false,
        feeDeterminationNote: 'fee determination test note',
        startDate: '2020-01-01',
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders the Public Benefit exists field', () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        ...getEmptyLease(),
        isPublicBenefit: true,
      },
    });
    expect(getByDisplayValue('Yes')).toBeVisible();
  });

  it('renders Financial Gain exists field', () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        ...getEmptyLease(),
        isFinancialGain: true,
      },
    });
    expect(getByDisplayValue('Yes')).toBeVisible();
  });

  it('renders the suggested Fee field', () => {
    const {
      component: { getByText },
    } = setup({
      lease: {
        ...getEmptyLease(),
        isPublicBenefit: true,
        isFinancialGain: false,
      },
    });
    expect(getByText('$1 - Nominal')).toBeVisible();
  });

  it('renders the Fee Notes field', () => {
    const {
      component: { getByText },
    } = setup({
      lease: {
        ...getEmptyLease(),
        feeDeterminationNote: 'fee determination test note',
      },
    });
    expect(getByText('fee determination test note')).toBeVisible();
  });
});
