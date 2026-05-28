import {
  act,
  fireEvent,
  render,
  RenderOptions,
  userEvent,
  waitFor,
  waitForEffects,
} from '@/utils/test-utils';
import DocumentUploadReplaceView, {
  IDocumentUploadReplaceViewProps,
} from './DocumentUploadReplaceView';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockLookups } from '@/mocks/lookups.mock';

const onSelectedReplacementFile = vi.fn();
const onConfirmReplace = vi.fn();
const onCancelReplace = vi.fn();

describe('Document upload replacement view', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IDocumentUploadReplaceViewProps> },
  ) => {
    const utils = render(
      <DocumentUploadReplaceView
        index={renderOptions?.props?.index ?? 0}
        file={renderOptions?.props?.file ?? null}
        onSelectedReplacementFile={onSelectedReplacementFile}
        onConfirmReplace={onConfirmReplace}
        onCancelReplace={onCancelReplace}
      />,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
      },
    );

    return {
      ...utils,
      useMockAuthentication: true,
    };
  };

  let file: File;

  beforeEach(() => {
    vi.restoreAllMocks();
    file = new File(['(⌐□_□)'], 'test.png', { type: 'image/png' });
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({});
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('call on cancel when clicked', async () => {
    const { getByTitle } = await setup({});

    const cancelButton = getByTitle(/cancel-replace/i);
    expect(cancelButton).toBeVisible();

    await act(async () => userEvent.click(cancelButton));

    expect(onCancelReplace).toHaveBeenCalled();
  });

  it('replaces the file when uploaded and confirm replace', async () => {
    const { getByTitle, getByTestId } = await setup({
      props: { file: file },
    });

    const replaceButton = getByTitle(/confirm-replace/i);
    expect(replaceButton).toBeVisible();
    expect(replaceButton).not.toBeDisabled();
    expect(getByTestId('file-name')).toHaveTextContent('test.png');

    await act(async () => userEvent.click(replaceButton));

    expect(onConfirmReplace).toHaveBeenCalled();
  });

  it('prepares the file replacement', async () => {
    const { getByTitle, getByTestId } = await setup({});

    const replaceButton = getByTitle(/confirm-replace/i);
    expect(replaceButton).toBeVisible();
    expect(replaceButton).toBeDisabled();

    // get the upload button
    const uploader = getByTestId('upload-input');

    // simulate upload event and wait until finish
    await act(async () => {
      fireEvent.change(uploader, {
        target: { files: [file] },
      });
    });
    await waitForEffects();

    expect(onSelectedReplacementFile).toHaveBeenCalled();
  });
});
