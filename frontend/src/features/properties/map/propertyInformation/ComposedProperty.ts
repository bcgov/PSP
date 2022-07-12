import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { Api_PropertyAssociations } from 'models/api/Property';

export default interface ComposedProperty {
  pid?: string;
  ltsaData?: LtsaOrders;
  ltsaDataRequestedOn?: Date;
  ltsaLoading: boolean;
  apiProperty?: IPropertyApiModel;
  propertyAssociations?: Api_PropertyAssociations;
}
