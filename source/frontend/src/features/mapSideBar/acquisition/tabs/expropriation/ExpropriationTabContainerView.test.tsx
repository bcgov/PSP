import { mockAcquisitionFileResponse } from '@/mocks/index.mock';
import { Api_AcquisitionFile, EnumAcquisitionFileType } from '@/models/api/AcquisitionFile';
import { render, RenderOptions } from '@/utils/test-utils';

import {
  ExpropriationTabContainerView,
  IExpropriationTabContainerViewProps,
} from './ExpropriationTabContainerView';

function getMockExpropriationFile(
  fileType: string = EnumAcquisitionFileType.SECTN6,
): Api_AcquisitionFile {
  const mockAcquisitionFile = mockAcquisitionFileResponse();
  mockAcquisitionFile.acquisitionTypeCode = {
    id: fileType,
  };
  return mockAcquisitionFile;
}

describe('Expropriation Tab Container View', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IExpropriationTabContainerViewProps> } = {},
  ) => {
    const utils = render(
      <ExpropriationTabContainerView
        {...renderOptions.props}
        loading={renderOptions.props?.loading ?? false}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? getMockExpropriationFile()}
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
      props: { acquisitionFile: getMockExpropriationFile(EnumAcquisitionFileType.SECTN3) },
    });

    expect(queryByTestId('form-1-section')).not.toBeInTheDocument();
    expect(queryByTestId('form-5-section')).not.toBeInTheDocument();
    expect(queryByTestId('form-8-section')).toBeInTheDocument();
    expect(queryByTestId('form-9-section')).not.toBeInTheDocument();
  });
});
