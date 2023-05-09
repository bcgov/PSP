import { mockAcquisitionFileResponse } from 'mocks/mockAcquisitionFiles';
import { getMockApiCompensation } from 'mocks/mockCompensations';
import { render, RenderOptions, waitFor } from 'utils/test-utils';

import {
  CompensationRequisitionDetailContainer,
  CompensationRequisitionDetailContainerProps,
} from './CompensationRequisitionDetailContainer';
import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

const CompensationViewComponent = (props: CompensationRequisitionDetailViewProps) => {
  return <></>;
};

const setEditMode = jest.fn();

describe('Compensation Detail View container', () => {
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<CompensationRequisitionDetailContainerProps> },
  ) => {
    const utils = render(
      <CompensationRequisitionDetailContainer
        View={CompensationViewComponent}
        {...renderOptions.props}
        loading={renderOptions.props?.loading ?? false}
        setEditMode={setEditMode}
        compensation={renderOptions.props?.compensation ?? getMockApiCompensation()}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? mockAcquisitionFileResponse()}
        gstConstant={renderOptions.props?.gstConstant ?? 0}
        clientConstant={renderOptions.props?.clientConstant ?? '034'}
      />,
      {
        ...renderOptions,
        claims: renderOptions?.claims ?? [],
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup({
      claims: [],
    });
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });
});
