from importlib.metadata import metadata
import xlrd
import json


from importlib.metadata import metadata
import xlrd
import json


# Give the location of the file
loc = ("doctype_definitions.xlsx")

# To open Workbook
wb = xlrd.open_workbook(loc)
sheet = wb.sheet_by_index(0)

print('Number of rows: ', sheet.nrows)


def openExcel(json_data):
    json_doc_types = json_data['document_types']
    # Get the document Type
    for i in range(2, sheet.nrows):
        doc_type_code = sheet.cell_value(i, 1)
        doc_type_name = sheet.cell_value(i, 2)
        #display_order = sheet.cell_value(i, 3)
        project = sheet.cell_value(i, 3)
        research = sheet.cell_value(i, 4)
        acquisition = sheet.cell_value(i, 5)
        lease = sheet.cell_value(i, 6)
        disposition = sheet.cell_value(i, 7)
        print(doc_type_code,
              doc_type_name,
              #display_order,
              project,
              research,
              acquisition,
              lease, disposition)

        for i in json_doc_types:
            if i['label'] == doc_type_code:

                #i['display_order'] = int(display_order)
                if 'categories' not in i:
                    i['categories'] = []

                if project == 'x':
                    i['categories'].append('PROJECT')
                if research == 'x':
                    i['categories'].append('RESEARCH')
                if acquisition == 'x':
                    i['categories'].append('ACQUIRE')
                if lease == 'x':
                    i['categories'].append('LEASLIC')
                if disposition == 'x':
                    i['categories'].append('DISPOSE')

    return json_data


def openJson():
    f = open('doc_metadata.json')
    data = json.load(f)
    # for i in data['document_types']:
    #print(i['name'] + ';' + i['label'] + ';' + str(i['display_order']))
    #i['test'] = 1235
    # print(i)
    return data


json_data = openJson()

updated_json = openExcel(json_data)

jsonStr = json.dumps(updated_json, indent=3)

#print(json.dumps(updated_json, indent=4))

with open('with_doc_types.json', 'w', encoding='utf-8') as f:
    f.write(jsonStr)
#    json.dump(jsonStr, f, ensure_ascii=False, indent=4)
#    json.dumps([value.dump()
#                for key, value in document_types.items()], ensure_ascii=False, indent=3)
