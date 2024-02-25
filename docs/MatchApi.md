# Org.OpenAPITools.Api.MatchApi

All URIs are relative to *http://petstore.swagger.io/v2*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateMatch**](MatchApi.md#creatematch) | **POST** /match | Create a new match |
| [**GetMatchById**](MatchApi.md#getmatchbyid) | **GET** /match/{id} | Get match on id. |
| [**UpdateMatchAndStates**](MatchApi.md#updatematchandstates) | **PUT** /match/{id} | Update a live match with a new matchState |

<a id="creatematch"></a>
# **CreateMatch**
> MatchDto CreateMatch (string X_XSRF_TOKEN, MatchUpsertDto matchUpsertDto)

Create a new match

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class CreateMatchExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://petstore.swagger.io/v2";
            var apiInstance = new MatchApi(config);
            var X_XSRF_TOKEN = "X_XSRF_TOKEN_example";  // string | 
            var matchUpsertDto = new MatchUpsertDto(); // MatchUpsertDto | 

            try
            {
                // Create a new match
                MatchDto result = apiInstance.CreateMatch(X_XSRF_TOKEN, matchUpsertDto);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MatchApi.CreateMatch: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateMatchWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a new match
    ApiResponse<MatchDto> response = apiInstance.CreateMatchWithHttpInfo(X_XSRF_TOKEN, matchUpsertDto);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MatchApi.CreateMatchWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **X_XSRF_TOKEN** | **string** |  |  |
| **matchUpsertDto** | [**MatchUpsertDto**](MatchUpsertDto.md) |  |  |

### Return type

[**MatchDto**](MatchDto.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | A match, matchState and marketStates were successfully created. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmatchbyid"></a>
# **GetMatchById**
> MatchDto GetMatchById (long id)

Get match on id.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class GetMatchByIdExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://petstore.swagger.io/v2";
            var apiInstance = new MatchApi(config);
            var id = 789L;  // long | Id of match to return.

            try
            {
                // Get match on id.
                MatchDto result = apiInstance.GetMatchById(id);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MatchApi.GetMatchById: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMatchByIdWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get match on id.
    ApiResponse<MatchDto> response = apiInstance.GetMatchByIdWithHttpInfo(id);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MatchApi.GetMatchByIdWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **long** | Id of match to return. |  |

### Return type

[**MatchDto**](MatchDto.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | the found match |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="updatematchandstates"></a>
# **UpdateMatchAndStates**
> void UpdateMatchAndStates (string X_XSRF_TOKEN, long id, MatchUpsertDto matchUpsertDto)

Update a live match with a new matchState

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using Org.OpenAPITools.Api;
using Org.OpenAPITools.Client;
using Org.OpenAPITools.Model;

namespace Example
{
    public class UpdateMatchAndStatesExample
    {
        public static void Main()
        {
            Configuration config = new Configuration();
            config.BasePath = "http://petstore.swagger.io/v2";
            var apiInstance = new MatchApi(config);
            var X_XSRF_TOKEN = "X_XSRF_TOKEN_example";  // string | 
            var id = 789L;  // long | Id of match to update.
            var matchUpsertDto = new MatchUpsertDto(); // MatchUpsertDto | 

            try
            {
                // Update a live match with a new matchState
                apiInstance.UpdateMatchAndStates(X_XSRF_TOKEN, id, matchUpsertDto);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MatchApi.UpdateMatchAndStates: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateMatchAndStatesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update a live match with a new matchState
    apiInstance.UpdateMatchAndStatesWithHttpInfo(X_XSRF_TOKEN, id, matchUpsertDto);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MatchApi.UpdateMatchAndStatesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **X_XSRF_TOKEN** | **string** |  |  |
| **id** | **long** | Id of match to update. |  |
| **matchUpsertDto** | [**MatchUpsertDto**](MatchUpsertDto.md) |  |  |

### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: Not defined


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | The match was updated and new matchstate and marketstates inserted. |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

