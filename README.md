# Pet Store API

## Prerequisites

This project was created with .NET Core 2.1. To build and run it you may need to install the following tools:

* [Visual Studio 2017](https://visualstudio.microsoft.com/downloads/)
* [.NET CORE 2.1 SDK and Runtime](https://dotnet.microsoft.com/download/dotnet-core/2.1)

## Routes

API exposes three routes:

  - **POST Orders** route accepts the following request body in JSON format:
  ```
  {
  "customerId": "string",
  "items": [
    {
      "productId": "string",
      "quantity": 0
    }
   ]
  }

  ```
  returns order summary as successful response.
  
  - **GET Orders Summary** by customerId accepts customerId in URL and returns collection of order summaries for the 
  specific customer.
  - **GET Orders Summary** by Id accepts summary Id in URL and returns the order summary for the specific Id.
  
  ## Storage
  
  API uses **LiteDB** as a temporary storage. Db file being erazed/created within project root with each run, so there should be no need for the additional 
  database setup. Please check *appsettings.development.json* for details.
