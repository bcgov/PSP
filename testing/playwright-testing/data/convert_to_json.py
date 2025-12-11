import pandas as pd

# Load the Excel file
excel_file = r'C:\Users\SueT\projects\PSP\testing\PIMS.Tests.Automation\Data\PIMS_Testing_Data.xlsx'
xlsx = pd.ExcelFile(excel_file)

# Convert each sheet to a separate JSON file
for sheet in xlsx.sheet_names:
    df = pd.read_excel(xlsx, sheet_name=sheet)
    json_file = f"{sheet}.json"
    df.to_json(json_file, orient='records', indent=2)
    print(f"Saved: {json_file}")
