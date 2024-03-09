import { Section } from '@/components/common/Section/Section';
import { StyledNoData } from '@/features/documents/commonStyles';

import { OperationSet } from './OperationContainer';
import { OperationView } from './OperationView';

export interface IOperationSectionViewProps {
  subdivisionOperations: OperationSet[];
  consolidationOperations: OperationSet[];
}

export const OperationSectionView: React.FunctionComponent<IOperationSectionViewProps> = ({
  subdivisionOperations,
  consolidationOperations,
}) => {
  const hasSubdivisions = subdivisionOperations.length > 0;
  const hasConsolidations = consolidationOperations.length > 0;
  return (
    <>
      <Section header="Subdivision History">
        {hasSubdivisions ? (
          subdivisionOperations.map((operationSet, index) => (
            <>
              <OperationView
                key={index}
                operationTimeStamp={operationSet.operationDateTime ?? ''}
                sourceProperties={operationSet.sourceProperties}
                destinationProperties={operationSet.destinationProperties}
              />
              {index < subdivisionOperations.length - 1 && <br />}
            </>
          ))
        ) : (
          <StyledNoData>This property is not part of a subdivision</StyledNoData>
        )}
      </Section>
      <Section header="Consolidation History">
        {hasConsolidations ? (
          consolidationOperations.map((operationSet, index) => (
            <OperationView
              key={index}
              operationTimeStamp={operationSet.operationDateTime ?? ''}
              sourceProperties={operationSet.sourceProperties}
              destinationProperties={operationSet.destinationProperties}
            />
          ))
        ) : (
          <StyledNoData>This property is not part of a consolidation</StyledNoData>
        )}
      </Section>
    </>
  );
};
