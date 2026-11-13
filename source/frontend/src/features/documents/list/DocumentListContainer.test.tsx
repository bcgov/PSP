import { act, screen } from '@testing-library/react';

import { ApiGen_CodeTypes_DocumentRelationType } from '@/models/api/generated/ApiGen_CodeTypes_DocumentRelationType';
import { render } from '@/utils/test-utils';

import DocumentListContainer from './DocumentListContainer';

vi.mock('./DocumentListView', () => ({
  default: ({ canEditDocuments }: { canEditDocuments: boolean }) => (
    <div data-testid="document-edit-status">{canEditDocuments ? 'editable' : 'read-only'}</div>
  ),
}));

const mockRetrieveDocumentRelationship = vi.fn().mockResolvedValue([]);
const mockDeleteDocumentRelationship = vi.fn();

vi.mock('../hooks/useDocumentRelationshipProvider', () => ({
  useDocumentRelationshipProvider: () => ({
    retrieveDocumentRelationship: mockRetrieveDocumentRelationship,
    retrieveDocumentRelationshipLoading: false,
    deleteDocumentRelationship: mockDeleteDocumentRelationship,
  }),
}));

describe('DocumentListContainer edit permissions', () => {
  const setup = async (canEdit?: boolean) => {
    await act(async () => {
      render(
        <DocumentListContainer
          parentId="1"
          relationshipType={ApiGen_CodeTypes_DocumentRelationType.AcquisitionFiles}
          canEdit={canEdit}
        />,
        { useMockAuthentication: true },
      );
    });
  };

  it('allows editing when canEdit is not provided (Acquisition compatibility)', async () => {
    await setup();

    expect(screen.getByTestId('document-edit-status')).toHaveTextContent('editable');
  });

  it('allows editing when canEdit is true', async () => {
    await setup(true);

    expect(screen.getByTestId('document-edit-status')).toHaveTextContent('editable');
  });

  it('disables editing when canEdit is false', async () => {
    await setup(false);

    expect(screen.getByTestId('document-edit-status')).toHaveTextContent('read-only');
  });
});
