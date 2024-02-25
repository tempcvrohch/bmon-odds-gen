# Org.OpenAPITools.Api.BetApi

All URIs are relative to *http://petstore.swagger.io/v2*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**PlaceBet**](BetApi.md#placebet) | **POST** /bet/place/{marketStateId} | Place a wager. |

<a id="placebet"></a>
# **PlaceBet**
> BetDto PlaceBet (string X_XSRF_TOKEN, long marketStateId, BetPlaceDto betPlaceDto)

Place a wager.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class PlaceBetExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://petstore.swagger.io/v2";
            var apiInstance = new BetApi(config);
            var X_XSRF_TOKEN = "X_XSRF_TOKEN_example";  // string | 
            var marketStateId = 789L;  // long | Id of the market state to place the bet on.
            var betPlaceDto = new BetPlaceDto(); // BetPlaceDto | 

            try
            {
                // Place a wager.
                BetDto result = apiInstance.PlaceBet(X_XSRF_TOKEN, marketStateId, betPlaceDto);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BetApi.PlaceBet: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the PlaceBetWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Place a wager.
    ApiResponse<BetDto> response = apiInstance.PlaceBetWithHttpInfo(X_XSRF_TOKEN, marketStateId, betPlaceDto);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BetApi.PlaceBetWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **X_XSRF_TOKEN** | **string** |  |  |
| **marketStateId** | **long** | Id of the market state to place the bet on. |  |
| **betPlaceDto** | [**BetPlaceDto**](BetPlaceDto.md) |  |  |

### Return type

[**BetDto**](BetDto.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A bet was successfully placed and balance was substracted. |  -  |
| **400** | The bet was already placed or insufficient funds or stake out of bounds or unknown market. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

