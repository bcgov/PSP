import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { renderAsync, RenderOptions } from '@/utils/test-utils';
import PropertySelectorPidSearchContainer, {
  PropertySelectorPidSearchContainerProps,
} from './PropertySelectorPidSearchContainer';
import { IPropertySearchSelectorFormViewProps } from './PropertySearchSelectorFormView';
import { IPropertySearchSelectorPidFormViewProps } from './PropertySelectorPidSearchView';
import { IAddSubdivisionViewProps } from '@/features/mapSideBar/subdivision/AddSubdivisionView';
import { SubdivisionFormModel } from '@/features/mapSideBar/subdivision/AddSubdivisionModel';

jest.mock('@react-keycloak/web');
jest.mock('@/components/common/mapFSM/MapStateMachineContext');
const history = createMemoryHistory();

const onClose = jest.fn();

let viewProps: IPropertySearchSelectorPidFormViewProps | undefined;
const TestView: React.FunctionComponent<
  React.PropsWithChildren<IPropertySearchSelectorPidFormViewProps>
> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

const mockGetByPidWrapper = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

const setSelectProperty = jest.fn();

jest.mock('@/hooks/repositories/usePimsPropertyRepository', () => ({
  usePimsPropertyRepository: () => {
    return {
      getPropertyByPidWrapper: mockGetByPidWrapper,
    };
  },
}));

describe('PropertySearchPidSelector component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<PropertySelectorPidSearchContainerProps>;
    } = {},
  ) => {
    const ref = createRef<FormikProps<SubdivisionFormModel>>();
    const component = await renderAsync(
      <PropertySelectorPidSearchContainer
        PropertySelectorPidSearchView={TestView}
        setSelectProperty={setSelectProperty}
      />,
      {
        history,
        useMockAuthentication: true,
        claims: [],
        ...renderOptions,
      },
    );

    return {
      ...component,
      getFormikRef: () => ref,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
    history.location.pathname = '/';
    jest.resetAllMocks();
    (useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('calls setSelectedProperty when onSearch', async () => {
    await setup();

    viewProps?.onSearch({ pid: '111-111-111' });
    mockGetByPidWrapper.execute.mockResolvedValue({ id: 1 });

    expect(mockGetByPidWrapper.execute).toHaveBeenCalledWith('111-111-111');
  });
});
