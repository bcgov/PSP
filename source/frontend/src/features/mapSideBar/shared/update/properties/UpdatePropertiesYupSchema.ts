import * as Yup from 'yup';

export const UpdatePropertiesYupSchema = (context?: any) =>
  Yup.object().shape({
    properties: Yup.array()
      .of(
        Yup.object().shape({
          name: Yup.string().max(500, 'Property name must be less than 500 characters'),
          isRetired: Yup.boolean(),
          id: Yup.mixed(),
        }),
      )
      .test(
        'no-new-retired-properties',
        'Selected property is retired and can not be added to the file',
        function (properties) {
          const { originalPropertyIds } = context || {};
          if (!properties || !Array.isArray(properties)) return true;
          return properties.every(
            p => !p.isRetired || (originalPropertyIds && originalPropertyIds.includes(p.id)),
          );
        },
      ),
  });
