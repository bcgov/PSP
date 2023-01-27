import { LinkButton } from 'components/common/buttons';
import { Section } from 'features/mapSideBar/tabs/Section';
import { FieldArray, FormikProps, useFormikContext } from 'formik';
import React from 'react';
import styled from 'styled-components';

import { ProductForm, ProjectForm } from './models';
import ProductSubForm from './ProductSubForm';

export interface IProductsArrayFormProps {
  field: string;
  formikProps: FormikProps<ProjectForm>;
}

export const ProductsArrayForm: React.FunctionComponent<IProductsArrayFormProps> = ({
  field,
  formikProps,
}) => {
  const { values } = useFormikContext<ProjectForm>();

  const products = values.products || [];

  return (
    <Section header="Associated products">
      <FieldArray name={field}>
        {arrayHelpers => (
          <>
            {products.map((product, index, array) => (
              <>
                <ProductSubForm nameSpace={`${field}.${index}`} formikProps={formikProps} />
                {index !== products.length - 1 && <StyledSpacer className="my-5" />}
              </>
            ))}
            <LinkButton onClick={() => arrayHelpers.push(new ProductForm())}>
              + Add another product
            </LinkButton>
          </>
        )}
      </FieldArray>
    </Section>
  );
};

export default ProductsArrayForm;

const StyledSpacer = styled.div`
  border-bottom: 0.1rem solid grey;
`;
