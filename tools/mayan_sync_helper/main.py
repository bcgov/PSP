from importlib.metadata import metadata
import xlrd
import json


# Give the location of the file
loc = ("doc_metadata.xlsx")

# To open Workbook
wb = xlrd.open_workbook(loc)
sheet = wb.sheet_by_index(0)


class MetadataType:
    #label = fields.Str()
    #required = fields.Boolean()

    def __init__(self, label, required):
        self.label = label
        self.required = required

    def toJson(self):
        return {"label": self.label,
                "required": self.required}


class DocumentType:
    #label = fields.Str()
    #metadata_types = fields.List(fields.Nested(MetadataType))

    def __init__(self, label):
        self.label = label
        self.metadata_types = []

    def add_metadata(self, metadata_type):
        self.metadata_types.append(metadata_type)

    def toJson(self):
        return {"label": self.label,
                "metadata_types": [value.toJson() for value in self.metadata_types]}


def toJsonFile(document_types, metdada_types):
    return {"metadata_types": [value for value in metdada_types],
            "document_types": [value.toJson() for value in document_types.items()]}


document_types = {}
metadata_types = {}

print('Number of rows: ', sheet.nrows)

# Get the document Type
for i in range(3, sheet.nrows):
    current_row_name = sheet.cell_value(i, 1)
    print(current_row_name)
    if current_row_name != "":
        doc_type_name = current_row_name

    # Add doc type if it does not exist
    if doc_type_name not in document_types:
        document_types[doc_type_name] = DocumentType(doc_type_name)

    # process metadata type
    # get label field
    meta_data_label = sheet.cell_value(i, 2)

    # get required field
    meta_data_required = sheet.cell_value(i, 3)
    if meta_data_required != "":
        meta_data_required = meta_data_required.lower() == "yes"
    else:
        meta_data_required = False

    meta_data_type = MetadataType(meta_data_label, meta_data_required)

    document_types[doc_type_name].add_metadata(meta_data_type)

    # add to the list of metadata only
    if meta_data_label not in metadata_types:
        metadata_types[meta_data_label] = meta_data_label

# convert to JSON string
#jsonStr = json.dumps(document_types, many=True)
# print(document_types.items())
# jsonStr = json.dumps([value.dump()
#                     for key, value in document_types.items()], indent=3)
#jsonStr = DocumentType.dumps(document_types, many=True)

# jsonStr = json.dumps([value.toJson()
#                     for key, value in document_types.items()], indent=3)


jsonStr = json.dumps(toJsonFile(document_types, metadata_types), indent=3)

print(jsonStr)

with open('doc_metadata.json', 'w', encoding='utf-8') as f:
    f.write(jsonStr)
#    json.dump(jsonStr, f, ensure_ascii=False, indent=4)
#    json.dumps([value.dump()
#                for key, value in document_types.items()], ensure_ascii=False, indent=3)

