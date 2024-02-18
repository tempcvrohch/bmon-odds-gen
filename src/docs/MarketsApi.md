# Org.OpenAPITools.Api.MarketsApi

All URIs are relative to *http://petstore.swagger.io/v2*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetLatestMarketsByMatchId**](MarketsApi.md#getlatestmarketsbymatchid) | **GET** /match/{id}/markets/latest | Get the latest market states on match. |

<a id="getlatestmarketsbymatchid"></a>
# **GetLatestMarketsByMatchId**
> List&lt;MarketStateDto&gt; GetLatestMarketsByMatchId (long id)

Get the latest market states on match.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class GetLatestMarketsByMatchIdExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://petstore.swagger.io/v2";
            var apiInstance = new MarketsApi(config);
            var id = 789L;  // long | id of the match

            try
            {
                // Get the latest market states on match.
                List<MarketStateDto> result = apiInstance.GetLatestMarketsByMatchId(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MarketsApi.GetLatestMarketsByMatchId: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetLatestMarketsByMatchIdWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get the latest market states on match.
    ApiResponse<List<MarketStateDto>> response = apiInstance.GetLatestMarketsByMatchIdWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MarketsApi.GetLatestMarketsByMatchIdWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **long** | id of the match |  |

### Return type

[**List&lt;MarketStateDto&gt;**](MarketStateDto.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | the latest markets |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

