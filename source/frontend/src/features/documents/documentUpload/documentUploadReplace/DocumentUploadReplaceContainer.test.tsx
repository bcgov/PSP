import { act, render, RenderOptions } from '@/utils/test-utils';
import DocumentUploadReplaceContainer, {
  IDocumentUploadReplaceContainerProps,
} from './DocumentUploadReplaceContainer';
import { IDocumentUploadReplaceViewProps } from './DocumentUploadReplaceView';

// eslint-disable-next-line @typescript-eslint/no-unused-vars
let viewProps: IDocumentUploadReplaceViewProps | undefined;
const TestView: React.FC<IDocumentUploadReplaceViewProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

const onReplaceDocumentFile = vi.fn();
const onCancelReplaceFile = vi.fn();

describe('document upload replace container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IDocumentUploadReplaceContainerProps>;
    } = {},
  ) => {
    const utils = render(
      <DocumentUploadReplaceContainer
        index={renderOptions?.props?.index ?? 0}
        onReplaceDocumentFile={onReplaceDocumentFile}
        onCancelReplaceFile={onCancelReplaceFile}
        View={TestView}
      ></DocumentUploadReplaceContainer>,
      {
        ...renderOptions,
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = await setup();

    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('handles cancel replacement', async () => {
    const {} = await setup();

    await act(async () => viewProps.onCancelReplace());
    expect(onCancelReplaceFile).toHaveBeenCalled();
  });

    it('handles file replacement on confirm', async () => {
    const {} = await setup();

    await act(async () => viewProps.onConfirmReplace());
    expect(onReplaceDocumentFile).toHaveBeenCalled();
  });
});
