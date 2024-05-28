import { useEffect } from 'react';

import { StyledSectionHeader } from '@/components/common/Section/Section';
import { Table } from '@/components/Table';
import { FileTypes } from '@/constants';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';

import { PropertyOperationResult } from './OperationView';
import getPropertyOperationAssociationColumns, {
  PropertyOperationTypedAssociation,
} from './propertyOperationAssociationColumns';

export interface IOperationFileAssociationsContainerProps {
  operation: PropertyOperationResult | undefined;
}

export const OperationFileAssociationsContainer: React.FunctionComponent<
  IOperationFileAssociationsContainerProps
> = ({ operation }) => {
  const { execute, loading, response } = usePropertyAssociations();
  useEffect(() => {
    if (operation?.id) {
      execute(operation.id);
    }
  }, [operation?.id, execute]);
  const typedOperationAssociations = [
    ...(response?.researchAssociations ?? []).map(a => ({ ...a, type: FileTypes.Research })),
    ...(response?.acquisitionAssociations ?? []).map(a => ({ ...a, type: FileTypes.Acquisition })),
    ...(response?.leaseAssociations ?? []).map(a => ({ ...a, type: FileTypes.Lease })),
    ...(response?.dispositionAssociations ?? []).map(a => ({ ...a, type: FileTypes.Disposition })),
  ];
  return (
    <>
      <StyledSectionHeader>PIMS Files</StyledSectionHeader>
      <Table<PropertyOperationTypedAssociation>
        columns={getPropertyOperationAssociationColumns()}
        data={typedOperationAssociations}
        name="PropertyOperationAssociations"
        loading={loading}
        noRowsMessage="There are no files associated to the property."
        hideToolbar
      />
    </>
  );
};

export default OperationFileAssociationsContainer;
