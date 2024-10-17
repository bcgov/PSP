from importlib.metadata import metadata
import xlrd
import json


from importlib.metadata import metadata
import xlrd
import json


# Give the location of the file
loc = ("mayan_definitions.xlsx")

# To open Workbook
wb = xlrd.open_workbook(loc)
sheet = wb.sheet_by_index(0)

print('Number of rows: ', sheet.nrows)


def openExcel(json_data):
    json_doc_with_types = json_data['metadata_types']
    # Get the document Type
    for i in range(2, sheet.nrows):
        meta_name = sheet.cell_value(i, 1)
        meta_label = sheet.cell_value(i, 2)

        print(meta_name, meta_label)

        for i in json_doc_with_types:
            if i['label'] == doc_type_code:

                i['display_order'] = int(display_order)
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
    f = open('with_doctypes.json')
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

with open('data_updated.json', 'w', encoding='utf-8') as f:
    f.write(jsonStr)
#    json.dump(jsonStr, f, ensure_ascii=False, indent=4)
#    json.dumps([value.dump()
#                for key, value in document_types.items()], ensure_ascii=False, indent=3)
