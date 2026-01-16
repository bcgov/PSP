import { FormikHelpers } from 'formik';
import { useCallback, useEffect, useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';

import { usePropertyImprovementRepository } from '@/hooks/repositories/usePropertyImprovementRepository';
import { isValidId } from '@/utils';

import { IPropertyImprovementFormProps } from '../form/PropertyImprovementForm';
import { PropertyImprovementFormModel } from '../models/PropertyImprovementFormModel';

export interface IUpdatePropertyImprovementContainerProps {
  propertyId: number;
  propertyImprovementId: number;
  View: React.FC<IPropertyImprovementFormProps>;
  onSuccess: () => void;
}

const UpdatePropertyImprovementContainer: React.FunctionComponent<
  IUpdatePropertyImprovementContainerProps
> = ({ propertyId, propertyImprovementId, View, onSuccess }) => {
  const [initialValues, setInitialValues] = useState<PropertyImprovementFormModel>(null);

  const history = useHistory();
  const location = useLocation();

  const backUrl = location.pathname.split(`/${propertyImprovementId}/update`)[0];

  const {
    putPropertyImprovement: { execute: updatePropertyImprovement, loading: updatingImprovement },
    getPropertyImprovement: { execute: fetchPropertyImprovement, loading: fetchingImprovement },
  } = usePropertyImprovementRepository();

  const getImprovement = useCallback(async () => {
    const improvement = await fetchPropertyImprovement(propertyId, propertyImprovementId);

    if (improvement) {
      const improvementFormModel = PropertyImprovementFormModel.fromApi(improvement);
      setInitialValues(improvementFormModel);
    }
  }, [fetchPropertyImprovement, propertyId, propertyImprovementId]);

  const handleSubmit = async (
    values: PropertyImprovementFormModel,
    formikHelpers: FormikHelpers<PropertyImprovementFormModel>,
  ) => {
    const agreementSaved = await updatePropertyImprovement(
      propertyId,
      propertyImprovementId,
      values.toApi(),
    );

    if (agreementSaved) {
      onSuccess();
      history.push(backUrl);
    }
    formikHelpers.setSubmitting(false);
  };

  useEffect(() => {
    if (initialValues === null && isValidId(propertyImprovementId) && isValidId(propertyId)) {
      getImprovement();
    }
  }, [getImprovement, initialValues, propertyId, propertyImprovementId]);

  return (
    <View
      isLoading={fetchingImprovement || updatingImprovement}
      initialValues={initialValues}
      onSubmit={handleSubmit}
      onCancel={() => history.push(backUrl)}
    ></View>
  );
};

export default UpdatePropertyImprovementContainer;
