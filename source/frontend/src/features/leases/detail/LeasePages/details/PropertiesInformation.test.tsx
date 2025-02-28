import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { mockLeaseProperty } from '@/mocks/filterData.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { getEmptyLease } from '@/models/defaultInitializers';
import { toTypeCode } from '@/utils/formUtils';
import { render, RenderOptions, waitForEffects } from '@/utils/test-utils';

import PropertiesInformation, { IPropertiesInformationProps } from './PropertiesInformation';
import { ApiGen_CodeTypes_LeaseLicenceTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseLicenceTypes';
import { ApiGen_Concepts_FinancialCodeTypes } from '@/models/api/generated/ApiGen_Concepts_FinancialCodeTypes';

const history = createMemoryHistory();

const customSetFilePropertyLocations = vi.fn();

describe('LeasePropertiesInformation component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & IPropertiesInformationProps = { lease: getEmptyLease() },
  ) => {
    const utils = render(
        <PropertiesInformation lease={renderOptions.lease} />,
      {
        ...renderOptions,
        history,
        mockMapMachine: {
          ...mapMachineBaseMock,
          setFilePropertyLocations: customSetFilePropertyLocations,
        },
      },
    );

    return { ...utils };
  };

  beforeEach(() => {
    vi.resetAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({
      lease: {
        ...getEmptyLease(),
        fileProperties: [
          {
            ...mockLeaseProperty(1),
            areaUnitType: toTypeCode('test'),
            leaseArea: 123,
            file: null,
            fileId: 1,
          },
        ],
      },
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders one Property Information section for all properties', () => {
    const { getAllByText } = setup({
      lease: {
        ...getEmptyLease(),
        fileProperties: [
          {
            ...mockLeaseProperty(1),
            areaUnitType: toTypeCode('test'),
            leaseArea: 123,
            file: null,
            fileId: 1,
            id: 1,
          },
          {
            ...mockLeaseProperty(2),
            areaUnitType: toTypeCode('test'),
            leaseArea: 123,
            file: null,
            fileId: 2,
            id: 2,
          },
        ],
      },
    });

    const propertyHeaders = getAllByText('Property Information');
    expect(propertyHeaders).toHaveLength(1);
  });

  it('renders no property information section if there are no properties', () => {
    const { queryByText } = setup({
      lease: { ...getEmptyLease(), fileProperties: [] },
    });

    const propertyHeader = queryByText('Property Information');
    expect(propertyHeader).toBeNull();
  });
});
