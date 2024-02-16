import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import * as API from '@/constants/API';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_PropertyImprovement } from '@/models/api/generated/ApiGen_Concepts_PropertyImprovement';

import Improvement from './components/Improvement/Improvement';
import { ILeaseImprovementForm } from './models';
import * as Styled from './styles';

export interface IImprovementsProps {
  improvements: ApiGen_Concepts_PropertyImprovement[];
  loading: boolean;
}

export const Improvements: React.FunctionComponent<IImprovementsProps> = ({
  improvements,
  loading,
}) => {
  const { getByType } = useLookupCodeHelpers();
  const improvementTypeCodes = getByType(API.PROPERTY_IMPROVEMENT_TYPES);
  const formImprovements = improvements.map((improvement: ApiGen_Concepts_PropertyImprovement) =>
    ILeaseImprovementForm.fromApi(improvement),
  );

  formImprovements.sort(
    (improvementOne: ILeaseImprovementForm, improvementTwo: ILeaseImprovementForm) => {
      const findDisplayOrder = (x: ILeaseImprovementForm): number => {
        for (const typeCode of improvementTypeCodes) {
          if (x.propertyImprovementTypeId === typeCode.id) {
            return typeCode.displayOrder;
          }
        }
        return 0;
      };
      return findDisplayOrder(improvementOne) - findDisplayOrder(improvementTwo);
    },
  );

  return (
    <Styled.ImprovementsContainer className="improvements">
      <LoadingBackdrop show={loading} parentScreen />
      {formImprovements.map((improvement: ILeaseImprovementForm) => (
        <Improvement improvement={improvement} key={`improvement-${improvement?.id}`} />
      ))}
    </Styled.ImprovementsContainer>
  );
};

export default Improvements;
