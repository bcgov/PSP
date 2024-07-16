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

const history = createMemoryHistory();

const customSetFilePropertyLocations = vi.fn();

describe('LeasePropertiesInformation component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions &
      IPropertiesInformationProps & { lease?: ApiGen_Concepts_Lease } = {},
  ) => {
    const utils = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? getEmptyLease()}>
        <PropertiesInformation nameSpace={renderOptions.nameSpace} />
      </Formik>,
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

  it('renders draft markers with provided lat/lng', async () => {
    setup({
      lease: {
        ...getEmptyLease(),
        fileProperties: [
          {
            ...mockLeaseProperty(1),
            areaUnitType: toTypeCode('test'),
            leaseArea: 123,
            file: null,
            fileId: 1,
            location: { coordinate: { x: 123, y: 48 } },
          },
        ],
      },
    });

    await waitForEffects();
    expect(customSetFilePropertyLocations).toHaveBeenCalledWith([{ lat: 48, lng: 123 }]);
  });
});
