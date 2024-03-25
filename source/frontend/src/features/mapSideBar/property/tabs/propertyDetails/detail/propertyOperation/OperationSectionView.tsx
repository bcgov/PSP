import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { StyledNoData } from '@/features/documents/commonStyles';
import { ApiGen_CodeTypes_PropertyOperationTypes } from '@/models/api/generated/ApiGen_CodeTypes_PropertyOperationTypes';

import { OperationSet } from './OperationContainer';
import OperationFileAssociationsContainer from './OperationFileAssocationsContainer';
import { OperationView } from './OperationView';

export interface IOperationSectionViewProps {
  subdivisionOperations: OperationSet[];
  consolidationOperations: OperationSet[];
  loading: boolean;
}

export const OperationSectionView: React.FunctionComponent<IOperationSectionViewProps> = ({
  subdivisionOperations,
  consolidationOperations,
  loading,
}) => {
  const hasSubdivisions = subdivisionOperations.length > 0;
  const hasConsolidations = consolidationOperations.length > 0;
  return (
    <>
      <Section header="Subdivision History" className="position-relative">
        {hasSubdivisions ? (
          <>
            <LoadingBackdrop show={loading} parentScreen />
            {subdivisionOperations.map((operationSet, index) => (
              <>
                <OperationView
                  key={index}
                  operationType={ApiGen_CodeTypes_PropertyOperationTypes.SUBDIVIDE}
                  operationTimeStamp={operationSet.operationDateTime ?? ''}
                  sourceProperties={operationSet.sourceProperties}
                  destinationProperties={operationSet.destinationProperties}
                  ExpandedRowComponent={OperationFileAssociationsContainer}
                />
                {index < subdivisionOperations.length - 1 && <br />}
              </>
            ))}
          </>
        ) : (
          <StyledNoData>This property is not part of a subdivision</StyledNoData>
        )}
      </Section>
      <Section header="Consolidation History" className="position-relative">
        {hasConsolidations ? (
          <>
            <LoadingBackdrop show={loading} parentScreen />
            {consolidationOperations.map((operationSet, index) => (
              <OperationView
                key={index}
                operationType={ApiGen_CodeTypes_PropertyOperationTypes.CONSOLIDATE}
                operationTimeStamp={operationSet.operationDateTime ?? ''}
                sourceProperties={operationSet.sourceProperties}
                destinationProperties={operationSet.destinationProperties}
                ExpandedRowComponent={OperationFileAssociationsContainer}
              />
            ))}
          </>
        ) : (
          <StyledNoData>This property is not part of a consolidation</StyledNoData>
        )}
      </Section>
    </>
  );
};
