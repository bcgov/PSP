import { FieldArray, Formik, FormikHelpers, FormikProps } from 'formik';
import { forwardRef } from 'react';
import { FaPlus } from 'react-icons/fa';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons/Button';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { AreaUnitTypes } from '@/constants/areaUnitTypes';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Take } from '@/models/api/generated/ApiGen_Concepts_Take';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { toTypeCode } from '@/utils/formUtils';
import { getApiPropertyName } from '@/utils/mapPropertyUtils';

import { TakeModel, TakesYupSchema } from './models';
import TakeSubForm from './TakeSubForm';

export interface ITakesUpdateFormProps {
  fileProperty: ApiGen_Concepts_FileProperty;
  takes: TakeModel[];
  loading: boolean;
  onSubmit: (model: TakesForm, formikHelpers: FormikHelpers<TakesForm>) => Promise<void>;
}

export interface TakesForm {
  takes: TakeModel[];
}

export const TakesUpdateForm = forwardRef<FormikProps<any>, ITakesUpdateFormProps>(
  ({ takes, loading, fileProperty, onSubmit }, ref) => {
    if (loading) {
      return (
        <StyledSummarySection>
          <LoadingBackdrop show={loading} parentScreen={true} />
        </StyledSummarySection>
      );
    }

    return (
      <StyledSummarySection>
        <StyledStickyWrapper>
          <StyledBlueCenteredText>
            Takes for&nbsp;<b>{getApiPropertyName(fileProperty.property).value}</b>
          </StyledBlueCenteredText>
        </StyledStickyWrapper>
        <Formik<TakesForm>
          onSubmit={onSubmit}
          initialValues={{ takes: takes ?? [] }}
          innerRef={ref}
          validationSchema={TakesYupSchema}
        >
          {({ values }) => (
            <FieldArray
              name="takes"
              render={arrayHelpers => (
                <>
                  {values.takes.map((t, index) => (
                    <TakeSubForm
                      key={`take-${t.id ?? index}`}
                      nameSpace={`takes.${index}`}
                      takeIndex={index}
                      onRemove={(index: number) => arrayHelpers.remove(index)}
                    />
                  ))}
                  <Button
                    variant="success"
                    onClick={() => arrayHelpers.push(new TakeModel({ ...emptyTake }))}
                    className="ml-4 mb-4"
                  >
                    <FaPlus />
                    &nbsp;Create a Take
                  </Button>
                </>
              )}
            />
          )}
        </Formik>
      </StyledSummarySection>
    );
  },
);

const StyledBlueCenteredText = styled.div`
  display: flex;
  justify-content: center;
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  margin: 1rem;
  border-radius: 0.5rem;
  padding: 0.5rem;
`;

const StyledStickyWrapper = styled.div`
  z-index: 1;
  position: sticky;
  top: 0;
  padding-bottom: 0.5rem;
  background-color: ${({ theme }) => theme.css.filterBackgroundColor};
`;

export const emptyTake: ApiGen_Concepts_Take = {
  id: 0,
  description: '',
  newHighwayDedicationArea: null,
  areaUnitTypeCode: toTypeCode(AreaUnitTypes.SquareMeters.toString()),
  isAcquiredForInventory: null,
  isThereSurplus: null,
  isNewLicenseToConstruct: null,
  isNewHighwayDedication: null,
  isNewLandAct: null,
  isNewInterestInSrw: null,
  isLeasePayable: null,
  licenseToConstructArea: null,
  ltcEndDt: null,
  landActArea: null,
  landActEndDt: null,
  propertyAcquisitionFile: null,
  propertyAcquisitionFileId: 0,
  statutoryRightOfWayArea: null,
  srwEndDt: null,
  surplusArea: null,
  leasePayableArea: null,
  leasePayableEndDt: null,
  takeSiteContamTypeCode: null,
  takeTypeCode: null,
  takeStatusTypeCode: toTypeCode('INPROGRESS'),
  landActTypeCode: null,
  completionDt: null,
  ...getEmptyBaseAudit(),
};

export default TakesUpdateForm;
