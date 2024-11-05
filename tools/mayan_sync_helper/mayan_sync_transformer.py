
from importlib.metadata import metadata
import xlrd
import json


class MetadataType:
    def __init__(self, name, label):
        self.name = name
        self.label = label

    def toJson(self):
        return {"label": self.label,
                "name": self.name}


class MetadataReference:
    def __init__(self, name, required):
        self.name = name
        self.required = required

    def toJson(self):
        return {"name": self.name,
                "required": self.required}


class DocumentType:
    # label = fields.Str()
    # metadata_types = fields.List(fields.Nested(MetadataType))

    def __init__(self, name, label, purpose, display_order, categories):
        self.name = name
        self.label = label
        self.purpose = purpose
        self.display_order = display_order
        self.categories = categories
        self.metadata_references = []

    def add_metadata(self, metadata_reference):
        self.metadata_references.append(metadata_reference)

    def sort_internal(self):
        self.categories.sort(reverse=False)
        self.metadata_references.sort(key=lambda x: x.name, reverse=False)

    def toJson(self):
        return {
            "categories": [value for value in self.categories],
            "display_order": self.display_order,
            "label": self.label,
            "purpose": self.purpose,
            "metadata_types": [value.toJson() for value in self.metadata_references],
            "name": self.name
        }


class MayanRequest:
    def __init__(self, document_types, metadata_types):
        self.document_types = document_types
        self.metadata_types = metadata_types
        self.remove_lingering_document_types = True
        self.remove_lingering_metadata_types = True

    def toJson(self):
        return {"document_types": [value.toJson() for value in self.document_types],
                "metadata_types": [value.toJson() for value in self.metadata_types],
                "remove_lingering_document_types": self.remove_lingering_document_types,
                "remove_lingering_metadata_types": self.remove_lingering_metadata_types
                }


def load_document_definitions():
    # Give the location of the file
    doctype_location = ("xml_definitions/doctype_definitions.xlsx")

    # To open Workbook
    wb = xlrd.open_workbook(doctype_location)
    doctype_sheet = wb.sheet_by_index(0)

    document_types = []

    # Get the document Type
    for i in range(2, doctype_sheet.nrows):
        doc_type_code = doctype_sheet.cell_value(i, 1)
        doc_type_name = doctype_sheet.cell_value(i, 2)
        doc_type_purpose = doctype_sheet.cell_value(i, 3)
        # display_order = int(doctype_sheet.cell_value(i, 3))
        display_order = 8
        project = doctype_sheet.cell_value(i, 4)
        research = doctype_sheet.cell_value(i, 5)
        acquisition = doctype_sheet.cell_value(i, 6)
        lease = doctype_sheet.cell_value(i, 7)
        disposition = doctype_sheet.cell_value(i, 8)
        management = doctype_sheet.cell_value(i, 9)

        print(doc_type_code,
              doc_type_name,
              doc_type_purpose,
              # display_order,
              project,
              research,
              acquisition,
              lease, disposition, management)

        categories = []

        if project == 'x':
            categories.append('PROJECT')
        if research == 'x':
            categories.append('RESEARCH')
        if acquisition == 'x':
            categories.append('ACQUIRE')
        if lease == 'x':
            categories.append('LEASLIC')
        if disposition == 'x':
            categories.append('DISPOSE')
        if management == 'x':
            categories.append('MANAGEMENT')

        document_types.append(DocumentType(
            doc_type_code, doc_type_name, doc_type_purpose, display_order, categories))

    # Order by label
    document_types.sort(key=lambda x: x.label, reverse=False)
    for i in range(0, len(document_types)):
        document_types[i].display_order = i

    return document_types


def load_metadata_definitions():
    # Give the location of the file
    metadata_file_location = ("xml_definitions/metadata_definitions.xlsx")

    # To open Workbook
    wb = xlrd.open_workbook(metadata_file_location)
    metadata_sheet = wb.sheet_by_index(0)

    metadata_types = []

    # Get the document Type
    for i in range(3, metadata_sheet.nrows):
        metadata_code = metadata_sheet.cell_value(i, 1)
        metadata_label = metadata_sheet.cell_value(i, 2)

        print(metadata_code, metadata_label)

        metadata_types.append(MetadataType(metadata_code, metadata_label))

    return metadata_types


def match_doc_metadata(document_types, metadata_types):
    # Give the location of the file
    loc_match = ("xml_definitions/doc_metadata.xlsx")

    # To open Workbook
    wb = xlrd.open_workbook(loc_match)
    sheet = wb.sheet_by_index(0)

    doctype_sort_order = []

    first_row = 2

    document_type_col = 1
    metadata_type_col = 2
    metadata_required_col = 3

    for i in range(first_row, sheet.nrows):
        print("--------------" + str(i))
        current_row_doctype = sheet.cell_value(i, document_type_col)

        if current_row_doctype == "":
            raise Exception('Invalid document type', current_row_doctype)

        # for debugging
        for a in document_types:
            print(a.name + "|" + current_row_doctype +
                  "|" + str(a.name == current_row_doctype))

        # Get document object from list based on the current document type
        doc_type = next(x for x in document_types if x.name ==
                        current_row_doctype)

        metadata_type = sheet.cell_value(i, metadata_type_col)

        # skip empty metadata associations
        if metadata_type != "":

            # for debugging
            for a in metadata_types:
                print(a.name + "|" + metadata_type +
                      "|" + str(a.name == metadata_type))

            # Get metadata object from list based on the current metadata type
            metadata_type = next(
                x for x in metadata_types if x.name == metadata_type)

            # get required field
            meta_data_required = sheet.cell_value(i, metadata_required_col)
            if meta_data_required != "":
                meta_data_required = meta_data_required.lower() == "yes"
            else:
                meta_data_required = False

            meta_data_type = MetadataReference(
                metadata_type.name, meta_data_required)

            doc_type.add_metadata(meta_data_type)

    # sort the documents for the request by document type
    document_types.sort(key=lambda x: x.name, reverse=False)
    [doctype.sort_internal() for doctype in document_types]

    metadata_types.sort(key=lambda x: x.name, reverse=False)

    return MayanRequest(document_types, metadata_types)


meta_types = load_metadata_definitions()
doc_types = load_document_definitions()
mayan_request = match_doc_metadata(doc_types, meta_types)

jsonStr = json.dumps(mayan_request.toJson(), indent=3)

with open('json_output/mayan_sync.json', 'w', encoding='utf-8') as f:
    f.write(jsonStr)
#    json.dump(jsonStr, f, ensure_ascii=False, indent=4)
#    json.dumps([value.dump()
#                for key, value in document_types.items()], ensure_ascii=False, indent=3)
