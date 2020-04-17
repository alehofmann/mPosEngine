Imports System.Net
Imports Newtonsoft.Json.Linq

Namespace Infrastructure
    Public Class HttpExternalSystemConnector
        Implements DCS.mPosEngine.Core.DataInterfaces.IExternalSystemConnector

		Public Function TestExportOperation(ByVal url As String, ByVal params As IList(Of KeyValuePair(Of String, String))) As String
            Dim response As String = PostHttp(url, params)
            Dim retVal As Boolean
            Dim errorMessage As String

            Dim json As JToken = JToken.Parse(response)
            Dim header As JToken = json.SelectToken("header")

            If header Is Nothing Then
                Throw New ApplicationException("Missing 'header' token from web service response [" & json.ToString & "]")
            End If

            Dim commandSuccess As JToken = header.SelectToken("commandSuccess")
            If commandSuccess Is Nothing Then
                Throw New ApplicationException("Missing 'commandSuccess' token from web service response [" & json.ToString & "]")
            End If

            If commandSuccess Then
                Dim body As JToken = json.SelectToken("body")
                If body Is Nothing Then
                    Throw New ApplicationException("Missing 'body' token from web service response [" & json.ToString & "]")
                End If
                Dim statusCode As JToken = body.SelectToken("statusCode")
                If statusCode Is Nothing Then
                    Throw New ApplicationException("Missing 'resultCode' token from web service response [" & json.ToString & "]")
                End If
                If Val(statusCode.ToString) = 0 Then
                    retVal = True
                Else
                    Dim msg As JToken = body.SelectToken("msg")
                    If msg IsNot Nothing Then
                        errorMessage = msg.ToString
                    End If
                    retVal = False
                End If
            Else
                retVal = False
            End If
        End Function

        Private Function PostHttp(ByVal url As String, ByVal content As IDictionary(Of String, String)) As String
            Dim client As New WebClient()
            Dim response As String

			Dim json As New JObject
			For Each item In content
                json.Add(item.Key, item.Value)
            Next

			'Method 1*******************
			'Dim http As HttpWebRequest = WebRequest.Create(New Uri(url))
			'http.Accept = "application/json"
			'http.ContentType = "application/json"
			'http.Method = "POST"

			'Dim contentBytes As Byte() = (New ASCIIEncoding).GetBytes(json.ToString)
			'Dim newStream As Stream = http.GetRequestStream
			'newStream.Write(contentBytes, 0, contentBytes.Length)
			'newStream.Close()

			'response = New StreamReader(http.GetResponse.GetResponseStream).ReadToEnd
			'***************************

			client.Headers.Add(HttpRequestHeader.ContentType, "application/json")
            client.Headers.Add(HttpRequestHeader.Accept, "application/json")
            'client.Headers.Add(HttpRequestHeader.ContentType, "POST")
            response = client.UploadString(New Uri(url), "POST", json.ToString)

			Return response
		End Function

		Public Function CommitOperation(operation As Core.Domain.Sales.ProductSellOperation, Optional ByRef errorMessage As String = "", Optional ByVal paymentId As Integer = 0) As Boolean Implements Core.DataInterfaces.IExternalSystemConnector.CommitOperation
			Dim response As String

			If operation Is Nothing Then
				Throw New ArgumentNullException("operation")
			End If
			If operation.ExternalSystemInfo Is Nothing Then
				Throw New ArgumentException("Operation's TransactionExporter is null")
			End If

			With operation.ExternalSystemInfo
				Dim url As String = .GetParsedUrl
				Try
					If paymentId > 0 Then
						.Params.Add("paymentId", paymentId)
					End If

					response = PostHttp(url, .Params)
				Catch ex As Exception
					errorMessage = "Error posting to url [" & url & "]: " & ex.Message
					Return False
				End Try
			End With

			'Parse Response********************************************************
			Dim json As JToken = JToken.Parse(response)
			Dim header As JToken = json.SelectToken("header")
			Dim commandSuccess As JToken = header.SelectToken("commandSuccess")

			If commandSuccess Is Nothing Then
				errorMessage = "Missing 'commandSuccess' token from web service response"
				Return False
			End If

			If commandSuccess Then
				Dim body As JToken = json.SelectToken("body")
				If body Is Nothing Then
					errorMessage = "Missing 'body' token from web service response [" & json.ToString & "]"
					Return False
				End If

				Dim statusCode As JToken = body.SelectToken("statusCode")
				If statusCode Is Nothing Then
					errorMessage = "Missing 'resultCode' token from web service response [" & json.ToString & "]"
					Return False
				End If

				If Val(statusCode.ToString) = 0 Then
					Return True
				Else
					Dim msg As JToken = body.SelectToken("msg")
					If msg IsNot Nothing Then
						errorMessage = msg.ToString
					Else
						errorMessage = "statusCode returned " & Val(statusCode.ToString) & " but msg is empty"
					End If

					Return False
				End If
			Else
				errorMessage = "Command Failed. Something went wrong on the server side"
				Return False
			End If
			'**********************************************************************            
		End Function
	End Class
End Namespace