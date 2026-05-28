import { FormikHelpers } from 'formik';
import { useHistory, useLocation } from 'react-router-dom';

import { usePropertyImprovementRepository } from '@/hooks/repositories/usePropertyImprovementRepository';
import { ApiGen_CodeTypes_PropertyImprovementStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_PropertyImprovementStatusTypes';
import { isValidId } from '@/utils';

import { IPropertyImprovementFormProps } from '../form/PropertyImprovementForm';
import { PropertyImprovementFormModel } from '../models/PropertyImprovementFormModel';

export interface IAddPropertyImprovementContainerProps {
  propertyId: number;
  View: React.FC<IPropertyImprovementFormProps>;
  onSuccess: () => void;
}

export const AddPropertyImprovementsContainer: React.FunctionComponent<
  IAddPropertyImprovementContainerProps
> = ({ propertyId, View, onSuccess }) => {
  const history = useHistory();
  const location = useLocation();
  const backUrl = location.pathname.split('/add')[0];

  const initialValues: PropertyImprovementFormModel = new PropertyImprovementFormModel(
    null,
    propertyId,
  );
  initialValues.improvementStatusCode = ApiGen_CodeTypes_PropertyImprovementStatusTypes.ACTIVE;

  const {
    postPropertyImprovement: { execute: addPropertyImprovement, loading },
  } = usePropertyImprovementRepository();

  const handleSubmit = async (
    values: PropertyImprovementFormModel,
    formikHelpers: FormikHelpers<PropertyImprovementFormModel>,
  ) => {
    const entity = values.toApi();
    const newPropertyImprovement = await addPropertyImprovement(propertyId, entity);
    if (isValidId(newPropertyImprovement.id)) {
      onSuccess();
      history.push(backUrl);
    }
    formikHelpers.setSubmitting(false);
  };

  return (
    <View
      isLoading={loading}
      initialValues={initialValues}
      onSubmit={handleSubmit}
      onCancel={() => history.push(backUrl)}
    ></View>
  );
};

export default AddPropertyImprovementsContainer;
