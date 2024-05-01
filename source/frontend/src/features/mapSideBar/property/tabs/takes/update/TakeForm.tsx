import { Formik, FormikHelpers, FormikProps } from 'formik';
import * as React from 'react';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { AreaUnitTypes } from '@/constants/areaUnitTypes';
import { ApiGen_Concepts_Take } from '@/models/api/generated/ApiGen_Concepts_Take';
import { getEmptyBaseAudit } from '@/models/defaultInitializers';
import { toTypeCode } from '@/utils/formUtils';

import { TakeModel, TakesYupSchema } from '../models';
import TakeSubForm from './TakeSubForm';

export interface ITakesFormProps {
  take: TakeModel;
  loading: boolean;
  onSubmit: (model: TakeModel, formikHelpers: FormikHelpers<TakeModel>) => Promise<void>;
}

export const TakeForm = React.forwardRef<FormikProps<any>, ITakesFormProps>(
  ({ take, loading, onSubmit }, ref) => {
    return (
      <StyledSummarySection>
        <LoadingBackdrop show={loading} parentScreen={true} />
        <Formik<TakeModel>
          onSubmit={onSubmit}
          initialValues={take}
          innerRef={ref}
          validationSchema={TakesYupSchema}
          enableReinitialize
        >
          {({ values }) => <TakeSubForm take={values} />}
        </Formik>
      </StyledSummarySection>
    );
  },
);

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

export default TakeForm;
