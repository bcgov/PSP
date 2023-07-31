import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { LeaseFormModel } from '@/features/leases/models';
import { mockParcel } from '@/mocks/filterData.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import PropertiesInformation, { IPropertiesInformationProps } from './PropertiesInformation';

const history = createMemoryHistory();

jest.mock('@/components/common/mapFSM/MapStateMachineContext');

describe('PropertiesInformation component', () => {
  const setup = (
    renderOptions: RenderOptions & IPropertiesInformationProps & { lease?: LeaseFormModel } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? new LeaseFormModel()}>
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
        ...new LeaseFormModel(),
        properties: [{ ...mockParcel, areaUnitTypeCode: 'test', landArea: '123', leaseId: null }],
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('renders one Property Information section per property', () => {
    const { component } = setup({
      lease: {
        ...new LeaseFormModel(),
        properties: [
          { ...mockParcel, areaUnitTypeCode: 'test', landArea: '123', leaseId: null },
          { ...mockParcel, areaUnitTypeCode: 'test', landArea: '123', leaseId: null },
        ],
      },
    });
    const { getAllByText } = component;
    const propertyHeaders = getAllByText('Property Information');

    expect(propertyHeaders).toHaveLength(1);
  });

  it('renders no property information section if there are no properties', () => {
    const { component } = setup({
      lease: { ...new LeaseFormModel(), properties: [] },
    });
    const { queryByText } = component;
    const propertyHeader = queryByText('Property Information');

    expect(propertyHeader).toBeNull();
  });
});
