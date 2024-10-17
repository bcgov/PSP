from importlib.metadata import metadata

import xlrd

import json
import os


from importlib.metadata import metadata

import xlrd

import json


# Give the location of the file

path = ("D:/Profesional/Quartech/Pims/Tmesheets/2024")


warning_ticket_count = 0
tickets = set()


def openExcel(location):
    global warning_ticket_count

    # To open Workbook

    wb = xlrd.open_workbook(location)

    sheet = wb.sheet_by_index(0)

    # print('Number of rows: ', sheet.nrows)

    # print(

    # sheet.cell_value(0, 0),

    # sheet.cell_value(0, 2),

    # sheet.cell_value(0, 4),

    # sheet.cell_value(0, 5))

    for i in range(2, sheet.nrows):

        task_date = sheet.cell_value(i, 0)

        task_type = sheet.cell_value(i, 2)

        task_comment = sheet.cell_value(i, 4)

        task_hours = sheet.cell_value(i, 5)

        # print(task_date, task_type, task_comment, task_hours)

        if 'psp-' in task_comment:
            tickets.add(task_comment)

            if ('Non' in task_type):

                warning_ticket_count += 1

                # print('WARNING')

    return ""


def openSubPath(path):

    print('**', path, '**')

    subfolders = [f.path for f in os.scandir(path) if not f.is_dir(

    ) and f.path.endswith('rodriguez.xlsx') and not f.name.startswith('~')]

    print(subfolders)

    for i in subfolders:
        print(i)

        openExcel(i)


def openPath(path):

    subfolders = [f.path for f in os.scandir(path) if f.is_dir()]

    for i in subfolders:

        openSubPath(i)

    # jsonStr = json.dumps(updated_json, indent=3)


# print(json.dumps(updated_json, indent=4))


# with open('data_updated.json', 'w', encoding='utf-8') as f:

    # f.write(jsonStr)

#    json.dump(jsonStr, f, ensure_ascii=False, indent=4)

#    json.dumps([value.dump()

#                for key, value in document_types.items()], ensure_ascii=False, indent=3)


openPath(path)
print(tickets)

print(warning_ticket_count)


jira_query = ['key = ' + t for t in tickets]

print(' OR '.join(jira_query))
