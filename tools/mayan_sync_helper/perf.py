from http.client import HTTPConnection
from base64 import b64encode
import requests
import time
import json
import os
import xlwt
from multiprocessing import Pool

authtoken = "eyJhbGciOiJSUzI1NiIsInR5cCIgOiAiSldUIiwia2lkIiA6ICJxUWlFWDB2T2Z1SlBuWUw4MWo0Q2tDOHVPdEJ1aFZvM0xBd2ppczZWbHRzIn0.eyJleHAiOjE3MjI1Mjg5MDgsImlhdCI6MTcyMjUyODYwOCwiYXV0aF90aW1lIjoxNzIyNTI2NTcyLCJqdGkiOiI0ZmQyZjA2OS1hZjRmLTQxMmItOWVmZC1lNzQzNGI5NTZjNjkiLCJpc3MiOiJodHRwczovL2Rldi5sb2dpbnByb3h5Lmdvdi5iYy5jYS9hdXRoL3JlYWxtcy9zdGFuZGFyZCIsImF1ZCI6InByb3BlcnR5LXNlcnZpY2VzLXByb2plY3QtYXBwLWRldi1vbmx5LTQ2OTkiLCJzdWIiOiI1ZDY2MWQxZTE0ZjA0NzdjYTdmYjMxZjIxZThiNGZkYUBpZGlyIiwidHlwIjoiQmVhcmVyIiwiYXpwIjoicHJvcGVydHktc2VydmljZXMtcHJvamVjdC1hcHAtZGV2LW9ubHktNDY5OSIsIm5vbmNlIjoiMDVmZDZkOTItMjQ4Zi00MzVmLTgwNDItMzgxZjFmZjFjZDE4Iiwic2Vzc2lvbl9zdGF0ZSI6Ijg0YjQ1MjlkLTFiMmQtNDYzYS05MGVmLTI1NDNmYjhkYjhiNyIsInNjb3BlIjoib3BlbmlkIGlkaXIgZW1haWwgcHJvZmlsZSIsInNpZCI6Ijg0YjQ1MjlkLTFiMmQtNDYzYS05MGVmLTI1NDNmYjhkYjhiNyIsImlkaXJfdXNlcl9ndWlkIjoiNUQ2NjFEMUUxNEYwNDc3Q0E3RkIzMUYyMUU4QjRGREEiLCJjbGllbnRfcm9sZXMiOlsiZGlzcG9zaXRpb24tZWRpdCIsInByb2plY3QtYWRkIiwicHJvamVjdC1lZGl0IiwiYWRtaW4tcHJvcGVydGllcyIsImxlYXNlLWRlbGV0ZSIsInJlc2VhcmNoZmlsZS12aWV3IiwicmVzZWFyY2hmaWxlLWVkaXQiLCJhZG1pbi1wcm9qZWN0cyIsImZvcm0tdmlldyIsImFncmVlbWVudC12aWV3IiwibWFuYWdlbWVudC1kZWxldGUiLCJtYW5hZ2VtZW50LWFkZCIsInByb2plY3QtdmlldyIsImFjcXVpc2l0aW9uZmlsZS1kZWxldGUiLCJkaXNwb3NpdGlvbi1kZWxldGUiLCJkaXNwb3NpdGlvbi12aWV3IiwiYWNxdWlzaXRpb25maWxlLWVkaXQiLCJsZWFzZS1lZGl0IiwicmVzZWFyY2hmaWxlLWFkZCIsImNvbnRhY3QtYWRkIiwicHJvcGVydHktZGVsZXRlIiwicHJvamVjdC1kZWxldGUiLCJjb21wZW5zYXRpb24tcmVxdWlzaXRpb24tYWRkIiwiYWNxdWlzaXRpb25maWxlLXZpZXciLCJmb3JtLWFkZCIsIm5vdGUtZGVsZXRlIiwiYWRtaW4tdXNlcnMiLCJhY3F1aXNpdGlvbmZpbGUtYWRkIiwiY29udGFjdC1kZWxldGUiLCJtYW5hZ2VtZW50LWVkaXQiLCJkb2N1bWVudC1lZGl0Iiwibm90ZS12aWV3IiwiY29tcGVuc2F0aW9uLXJlcXVpc2l0aW9uLWRlbGV0ZSIsImRvY3VtZW50LWFkbWluIiwiY29tcGVuc2F0aW9uLXJlcXVpc2l0aW9uLXZpZXciLCJwcm9wZXJ0eS1hZGQiLCJjb21wZW5zYXRpb24tcmVxdWlzaXRpb24tZWRpdCIsIlJPTEVfUElNU19SIiwiZG9jdW1lbnQtdmlldyIsImNvbnRhY3QtdmlldyIsInByb3BlcnR5LXZpZXciLCJub3RlLWVkaXQiLCJmb3JtLWVkaXQiLCJub3RlLWFkZCIsIlN5c3RlbSBBZG1pbmlzdHJhdG9yIiwiZG9jdW1lbnQtYWRkIiwiY29udGFjdC1lZGl0IiwicHJvcGVydHktZWRpdCIsIm1hbmFnZW1lbnQtdmlldyIsImRpc3Bvc2l0aW9uLWFkZCIsInJlc2VhcmNoZmlsZS1kZWxldGUiLCJkb2N1bWVudC1kZWxldGUiLCJsZWFzZS1hZGQiLCJmb3JtLWRlbGV0ZSIsImxlYXNlLXZpZXciLCJzeXN0ZW0tYWRtaW5pc3RyYXRvciJdLCJpZGVudGl0eV9wcm92aWRlciI6ImlkaXIiLCJpZGlyX3VzZXJuYW1lIjoiTUFST0RSSUciLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsIm5hbWUiOiJSb2RyaWd1ZXosIE1hbnVlbCBNT1RJOkVYIiwicHJlZmVycmVkX3VzZXJuYW1lIjoiNWQ2NjFkMWUxNGYwNDc3Y2E3ZmIzMWYyMWU4YjRmZGFAaWRpciIsImRpc3BsYXlfbmFtZSI6IlJvZHJpZ3VleiwgTWFudWVsIE1PVEk6RVgiLCJnaXZlbl9uYW1lIjoiTWFudWVsIiwiZmFtaWx5X25hbWUiOiJSb2RyaWd1ZXoiLCJlbWFpbCI6Im1hbnVlbC4xLnJvZHJpZ3VlekBnb3YuYmMuY2EifQ.WdHiqk3Buslomrh0m3qQ_qEyiSnsTSRgJaSO4TdaXdopn_eeEmdo3NFqyEttRztDA8PNJtL0-C0C8NXNNAl3M6WcK1jQDTMfo40gtTRkYsMbyhtyoHOIrLSQd85pz4tW3Ysz1r59dBdQr_pqv6qeJyUrgGq-d8t-3fEPLL4ctExsIeopF-lUymjMG9MTAP046X4yXLQHRJSl9dj7dNMHlAyyIiV1BfTKVJbINmBlrxn_77bN4BhO83XEXIXCLay4OapEWNiXieby3UAS6cgr5r_AqPrkFnP3XmiCXqgP-HFmnzoYySRhH7TvCKHlZG4UCXsUbDOYL9L_imFpNCWOYA"



file_root = "E:/Development/pims/mayan_helper/performance_test"
file_name = "test-1kb.txt"
#file_name = "test-1mb.txt"
#file_name = "test-5mb.txt"
#file_name = "test-10mb.txt"
#file_name = "test-50mb.txt"
#file_name = "test-100mb.txt"
#file_name = "test-500mb.txt"
file_path = file_root +'/'+ file_name

#url_root = 'http://localhost'
url_root = 'https://pims-app-3cd915-dev.apps.silver.devops.gov.bc.ca/'
url_port = ''
url_path = '/api/documents/upload/AcquisitionFiles/1'

number_of_calls = 5

class RequestResult:
  def __init__(self, start_time, end_time, result_status, file_size, file_name):
    self.start_time = start_time
    self.end_time = end_time
    self.status = result_status
    self.elapsed_time = end_time - start_time
    self.file_size = file_size
    self.file_name = file_name


def write_results(results):
   workbook = xlwt.Workbook()
   sheet = workbook.add_sheet('results')
   #for index, value in enumerate(result):
      #sheet.write(0, index, value)
   
   sheet.write(0, 0, 'status')
   sheet.write(0, 1, 'start_time')
   sheet.write(0, 2, 'end_time')
   sheet.write(0, 3, 'elapsed_time')
   sheet.write(0, 4, 'file_name')
   sheet.write(0, 5, 'file_size')
   for index, result in enumerate(results):
     sheet.write(index + 1, 0, result.status)
     sheet.write(index + 1, 1, result.start_time)
     sheet.write(index + 1, 2, result.end_time)
     sheet.write(index + 1, 3, result.elapsed_time)
     sheet.write(index + 1, 4, result.file_name)
     sheet.write(index + 1, 5, result.file_size)

   workbook.save('results.xls')


def send_request(param):
   with open(file_path, "rb") as file:
      files=(
        ('documentTypeMayanId', (None, '143')),
        ('documentTypeId', (None, '11')),
        ('documentStatusCode', (None, 'DRAFT')),
        ('DocumentMetadata[0].MetadataTypeId', (None, '55')),
        ('DocumentMetadata[0].Value', (None, 'testing descriptor')),
        ('file', (file)),)

      #url = url_root + ':' + url_port + url_path
      url = url_root + url_path
      headers = {'Authorization': f'Bearer {authtoken}'}

      start_time = time.time()
      response = requests.post(url, files=files, headers=headers)
      end_time = time.time()

      # to debug the request itself
      #int(requests.Request('POST', url, files=files).prepare().body.decode('utf8'))
      result_status = 'unset'
      try:
        result_status = json.loads(response.content)['uploadResponse']['documentExternalResponse']['status']
      except Exception as e:
        result_status = 'failure'
        print('-ERROR-')
        print(response.content)
        print('-------------------------------')
        #print(e)
      file_size = os.path.getsize(file_path)
      #file_size = 0
      return RequestResult(start_time, end_time, result_status, file_size, file_name)




import sys

if __name__ =="__main__":


    arglen = len(sys.argv)
    isParallel = False 
    if arglen > 0: 
        s = sys.argv[1]
        isParallel = s.lower() in ['true', '1', 't', 'y', 'yes', 'yeah', 'yup', 'certainly', 'uh-huh']

    if isParallel:
       print('Running paralel tests')
       args = [None] * number_of_calls
       pool = Pool()
       results = pool.map(send_request, args)
       pool.close()
    else:
       print('Running sequential tests')
       results = []
       for i in range(0, number_of_calls):
            results.append(send_request(None))

    print('Done')
    
    write_results(results)

