import { FormikHelpers } from 'formik';
import { useHistory, useLocation } from 'react-router-dom';

import { usePropertyImprovementRepository } from '@/hooks/repositories/usePropertyImprovementRepository';
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

  const initialValues = new PropertyImprovementFormModel(null, propertyId);
  const backUrl = location.pathname.split('/add')[0];

  const {
    postPropertyImprovement: { execute: addPropertyImprovement, loading },
  } = usePropertyImprovementRepository();

  const handleSubmit = async (
    values: PropertyImprovementFormModel,
    formikHelpers: FormikHelpers<PropertyImprovementFormModel>,
  ) => {
    const newPropertyImprovement = await addPropertyImprovement(propertyId, values.toApi());
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
