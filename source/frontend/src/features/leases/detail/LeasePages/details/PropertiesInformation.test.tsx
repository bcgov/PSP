import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { mockLeaseProperty } from '@/mocks/filterData.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { getEmptyLease } from '@/models/defaultInitializers';
import { toTypeCode } from '@/utils/formUtils';
import { render, RenderOptions } from '@/utils/test-utils';

import PropertiesInformation, { IPropertiesInformationProps } from './PropertiesInformation';

const history = createMemoryHistory();

jest.mock('@/components/common/mapFSM/MapStateMachineContext');

describe('PropertiesInformation component', () => {
  const setup = (
    renderOptions: RenderOptions &
      IPropertiesInformationProps & { lease?: ApiGen_Concepts_Lease } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? getEmptyLease()}>
        <PropertiesInformation nameSpace={renderOptions.nameSpace} />
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

  beforeEach(() => {
    jest.resetAllMocks();
    (useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);
  });

  it('renders as expected', () => {
    const { component } = setup({
      lease: {
        ...getEmptyLease(),
        fileProperties: [
          {
            ...mockLeaseProperty(),
            areaUnitType: toTypeCode('test'),
            leaseArea: 123,
            file: null,
            fileId: 0,
          },
        ],
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('renders one Property Information section per property', () => {
    const { component } = setup({
      lease: {
        ...getEmptyLease(),
        fileProperties: [
          {
            ...mockLeaseProperty(),
            areaUnitType: toTypeCode('test'),
            leaseArea: 123,
            file: null,
            fileId: 0,
          },
          {
            ...mockLeaseProperty(),
            areaUnitType: toTypeCode('test'),
            leaseArea: 123,
            file: null,
            fileId: 0,
          },
        ],
      },
    });
    const { getAllByText } = component;
    const propertyHeaders = getAllByText('Property Information');

    expect(propertyHeaders).toHaveLength(1);
  });

  it('renders no property information section if there are no properties', () => {
    const { component } = setup({
      lease: { ...getEmptyLease(), fileProperties: [] },
    });
    const { queryByText } = component;
    const propertyHeader = queryByText('Property Information');

    expect(propertyHeader).toBeNull();
  });
});
