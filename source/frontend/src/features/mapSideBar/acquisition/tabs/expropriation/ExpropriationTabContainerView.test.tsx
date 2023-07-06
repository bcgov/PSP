import { EnumAcquisitionFileType } from '@/models/api/AcquisitionFile';
import { render, RenderOptions } from '@/utils/test-utils';

import ExpropiationTabcontainerView, {
  IExpropriationTabcontainerViewProps,
} from './ExpropriationTabContainerView';

describe('Expropriation Tab Container View', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IExpropriationTabcontainerViewProps> },
  ) => {
    const utils = render(
      <ExpropiationTabcontainerView
        {...renderOptions.props}
        loading={renderOptions.props?.loading ?? false}
        acquisitionFileTypeCode={
          renderOptions.props?.acquisitionFileTypeCode ?? EnumAcquisitionFileType.SECTN6
        }
      />,
      {
        ...renderOptions,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('displays a loading spinner when loading', async () => {
    const { getByTestId } = await setup({ props: { loading: true } });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('shows the sections for Acquisition file type "Section 6"', async () => {
    const { queryByTestId } = await setup({});
    expect(queryByTestId('form-1-section')).toBeInTheDocument();
    expect(queryByTestId('form-5-section')).toBeInTheDocument();
    expect(queryByTestId('form-8-section')).toBeInTheDocument();
    expect(queryByTestId('form-9-section')).toBeInTheDocument();
  });

  it('shows the sections for Acquisition file type "Section 3"', async () => {
    const { queryByTestId } = await setup({
      props: { acquisitionFileTypeCode: EnumAcquisitionFileType.SECTN3 },
    });

    expect(queryByTestId('form-1-section')).not.toBeInTheDocument();
    expect(queryByTestId('form-5-section')).not.toBeInTheDocument();
    expect(queryByTestId('form-8-section')).toBeInTheDocument();
    expect(queryByTestId('form-9-section')).not.toBeInTheDocument();
  });
});
