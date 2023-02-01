import { AddFinancialCodeYupSchema } from '../add/AddFinancialCodeYupSchema';

// The code type value can not be updated from the update screen - it can only be set while adding the code value
export const UpdateFinancialCodeYupSchema = AddFinancialCodeYupSchema.omit(['type']);
