import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { mockAcquisitionFileResponse } from 'mocks/acquisitionFiles.mock';
import { getMockApiInterestHolders } from 'mocks/interestHolders.mock';
import { mockLookups } from 'mocks/lookups.mock';
import { Api_InterestHolder } from 'models/api/InterestHolder';
import { forwardRef } from 'react';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from 'utils/test-utils';

import {
  emptyApiInterestHolder,
  emptyInterestHolderProperty,
} from '../../update/stakeholders/models';
import StakeHolderContainer, { IStakeHolderContainerProps } from './StakeHolderContainer';
import { IStakeHolderViewProps } from './StakeHolderView';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockGetApi = {
  error: undefined,
  response: [] as Api_InterestHolder[],
  execute: jest.fn().mockResolvedValue(getMockApiInterestHolders()),
  loading: false,
};

const mockUpdateApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn().mockResolvedValue(getMockApiInterestHolders()),
  loading: false,
};

jest.mock('hooks/repositories/useInterestHolderRepository', () => ({
  useInterestHolderRepository: () => {
    return {
      getAcquisitionInterestHolders: mockGetApi,
      updateAcquisitionInterestHolders: mockUpdateApi,
    };
  },
}));

describe('StakeHolderContainer component', () => {
  // render component under test

  let viewProps: IStakeHolderViewProps;
  const View = forwardRef<FormikProps<any>, IStakeHolderViewProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  const onEdit = jest.fn();

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IStakeHolderContainerProps> },
  ) => {
    const utils = render(
      <StakeHolderContainer
        {...renderOptions.props}
        View={View}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? mockAcquisitionFileResponse()}
        onEdit={onEdit}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('groups multiple interest holder properties by acquisition file id', async () => {
    mockGetApi.response = getMockApiInterestHolders();
    setup({});
    await waitFor(async () => {
      expect(viewProps.groupedInterestProperties).toHaveLength(2);
      expect(viewProps.groupedInterestProperties[0].groupedPropertyInterests).toHaveLength(2);
    });
  });

  it('does not group interest and non-interests for the same property', async () => {
    mockGetApi.response = [
      {
        ...emptyApiInterestHolder,
        interestHolderProperties: [
          {
            ...emptyInterestHolderProperty,
            interestTypeCode: { id: 'NIP' },
            acquisitionFilePropertyId: 1,
          },
        ],
      },
      {
        ...emptyApiInterestHolder,
        interestHolderProperties: [
          {
            ...emptyInterestHolderProperty,
            interestTypeCode: { id: 'IP' },
            acquisitionFilePropertyId: 1,
          },
        ],
      },
    ];
    setup({});
    await waitFor(async () => {
      expect(viewProps.groupedInterestProperties).toHaveLength(1);
      expect(viewProps.groupedNonInterestProperties).toHaveLength(1);
    });
  });

  it('does not group interest holders for different properties', async () => {
    mockGetApi.response = [
      {
        ...emptyApiInterestHolder,
        interestHolderProperties: [
          {
            ...emptyInterestHolderProperty,
            acquisitionFilePropertyId: 1,
          },
        ],
      },
      {
        ...emptyApiInterestHolder,
        interestHolderProperties: [
          {
            ...emptyInterestHolderProperty,
            acquisitionFilePropertyId: 2,
          },
        ],
      },
    ];
    setup({});
    await waitFor(async () => {
      expect(viewProps.groupedInterestProperties).toHaveLength(2);
      expect(viewProps.groupedInterestProperties[0].groupedPropertyInterests).toHaveLength(1);
    });
  });
});
