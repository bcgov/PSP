import { Button } from 'components/common/buttons/Button';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { AreaUnitTypes } from 'constants/areaUnitTypes';
import { FieldArray, Formik, FormikHelpers, FormikProps } from 'formik';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import { Api_Take } from 'models/api/Take';
import * as React from 'react';
import { FaPlus } from 'react-icons/fa';
import styled from 'styled-components';
import { getApiPropertyName } from 'utils/mapPropertyUtils';

import { StyledSummarySection } from '../styles';
import { TakeModel, TakesYupSchema } from './models';
import TakeSubForm from './TakeSubForm';

export interface ITakesUpdateFormProps {
  fileProperty: Api_PropertyFile;
  takes: TakeModel[];
  loading: boolean;
  onSubmit: (model: TakesForm, formikHelpers: FormikHelpers<TakesForm>) => void;
}

export interface TakesForm {
  takes: TakeModel[];
}

export const TakesUpdateForm = React.forwardRef<FormikProps<any>, ITakesUpdateFormProps>(
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
                      key={`take-${index}`}
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

export const emptyTake: Api_Take = {
  areaUnitTypeCode: AreaUnitTypes.SquareMeters.toString(),
  description: '',
  isSurplusSeverance: false,
  isSection16: false,
  isStatutoryRightOfWay: false,
  isNewRightOfWay: false,
  isLicenseToConstruct: false,
  licenseToConstructArea: null,
  ltcEndDt: null,
  newRightOfWayArea: null,
  section16Area: null,
  section16EndDt: null,
  srwEndDt: null,
  statutoryRightOfWayArea: null,
  surplusSeveranceArea: null,
  propertyAcquisitionFileId: null,
  takeSiteContamTypeCode: null,
  takeTypeCode: null,
  takeStatusTypeCode: 'INPROGRESS',
};

export default TakesUpdateForm;
