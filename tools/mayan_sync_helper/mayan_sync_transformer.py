
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

    def __init__(self, name, label, display_order, categories):
        self.name = name
        self.label = label
        self.display_order = display_order
        self.categories = categories
        self.metadata_references = []

    def add_metadata(self, metadata_reference):
        self.metadata_references.append(metadata_reference)

    def toJson(self):
        return {
            "categories": [value for value in self.categories],
            "display_order": self.display_order,
            "label": self.label,
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
        # display_order = int(doctype_sheet.cell_value(i, 3))
        display_order = 8
        project = doctype_sheet.cell_value(i, 3)
        research = doctype_sheet.cell_value(i, 4)
        acquisition = doctype_sheet.cell_value(i, 5)
        lease = doctype_sheet.cell_value(i, 6)
        disposition = doctype_sheet.cell_value(i, 7)
        management = doctype_sheet.cell_value(i, 8)

        print(doc_type_code,
              doc_type_name,
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
            doc_type_code, doc_type_name, display_order, categories))

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

    for i in range(3, sheet.nrows):
        print("--------------" + str(i))
        current_row_name = sheet.cell_value(i, 1)

        if current_row_name != "":
            doc_type_name = current_row_name

        if doc_type_name not in doctype_sort_order:
            doctype_sort_order.append(doc_type_name)

        for a in document_types:
            print(a.label + "|" + doc_type_name +
                  "|" + str(a.label == doc_type_name))
        # Get document object from list based on the current label
        doc_type = next(x for x in document_types if x.label == doc_type_name)

        metadata_label = sheet.cell_value(i, 2)
        if metadata_label != "":

            for a in metadata_types:
                print(a.label + "|" + metadata_label +
                      "|" + str(a.label == metadata_label))
            metadata_type = next(
                x for x in metadata_types if x.label == metadata_label)

            # get required field
            meta_data_required = sheet.cell_value(i, 3)
            if meta_data_required != "":
                meta_data_required = meta_data_required.lower() == "yes"
            else:
                meta_data_required = False

            meta_data_type = MetadataReference(
                metadata_type.name, meta_data_required)

            doc_type.add_metadata(meta_data_type)

    sorted_docs = []
    for name in doctype_sort_order:
        doc = next(x for x in document_types if x.label == name)
        index = document_types.index(doc)
        sorted_docs.append(doc)
        del document_types[index]

    # add the reamining doc types
    for doc in document_types:
        sorted_docs.append(doc)

    return MayanRequest(sorted_docs, metadata_types)


meta_types = load_metadata_definitions()
doc_types = load_document_definitions()
mayan_request = match_doc_metadata(doc_types, meta_types)

jsonStr = json.dumps(mayan_request.toJson(), indent=3)

with open('json_output/all_the_dile.json', 'w', encoding='utf-8') as f:
    f.write(jsonStr)
#    json.dump(jsonStr, f, ensure_ascii=False, indent=4)
#    json.dumps([value.dump()
#                for key, value in document_types.items()], ensure_ascii=False, indent=3)
