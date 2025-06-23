using iHC.Models.Forms;
using Microsoft.AspNetCore.Mvc;
using System.IO.Compression;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using iHC.Models.ProfilePage;

namespace IHCApis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ERPController : ControllerBase
    {


        private readonly IHttpClientFactory _httpClientFactory;

        public string[] DocTypeAliases { get; set; }

        public ERPController(IHttpClientFactory httpClientFactory)
        {

            _httpClientFactory = httpClientFactory;

        }


        [HttpPost("postAbsenceBalance")]
        public async Task<IActionResult> PostAbsenceBalance([FromBody] AbsenceRequestModel request)
        {
            var client = _httpClientFactory.CreateClient("iHC");

            var body = new
            {
                personId = request.PersonId,
                typeId = request.TypeId,
                effectiveDate = request.EffectiveDate
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/hcmRestApi/resources/latest/absences/action/getAbsenceTypeBalance", content);

            var stream = await response.Content.ReadAsStreamAsync();
            using var decompressed = new GZipStream(stream, CompressionMode.Decompress);
            using var reader = new StreamReader(decompressed);
            string errorMessage = await reader.ReadToEndAsync();

            Console.WriteLine("Oracle API Error: " + errorMessage); // Shows the real problem



            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, error);
            }

            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }



        [HttpGet("getworkers")]
        public async Task<IActionResult> GetWorkers(string personNumber)
        {
            var client = _httpClientFactory.CreateClient("iHC");
            var qs = "?expand=workRelationships.assignments,names,emails,addresses,nationalIdentifiers,phones,legislativeInfo,photos,passports,religions"
                   + $"&onlyData=true&q=PersonNumber={personNumber}&limit=500";

            var resp = await client.GetAsync($"/hcmRestApi/resources/11.13.18.05/workers/{qs}");
            if (!resp.IsSuccessStatusCode)
                return StatusCode((int)resp.StatusCode, "Error fetching workers");

            var json = await resp.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }
        [HttpGet("getEmployeeCertificates")]
        public async Task<IActionResult> GetCertificateTypes()
        {
            var client = _httpClientFactory.CreateClient("iHC");
            var resp = await client.GetAsync($"fscmRestApi/resources/11.13.18.05/valueSets?q=ValueSetCode=XXX_EMPLOYMENT_CERTIFICATE_DOR&expand=values&onlyData=true");
            if (!resp.IsSuccessStatusCode) return BadRequest();

            var json = await resp.Content.ReadAsStringAsync();
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<CertificateTypes>(json); // Create a class to map the response

            var enabledValues = data.Items
                .SelectMany(x => x.Values)
                .Where(v => v.EnabledFlag == "Y")
                .Select(v => new { v.Value, v.Description });

            return Ok(enabledValues);
        }
        [HttpGet("getToWhomConcerns")]
        public async Task<IActionResult> GetToWhomConcerns()
        {
            var client = _httpClientFactory.CreateClient("iHC");

            // Make a request to the API endpoint
            var resp = await client.GetAsync("fscmRestApi/resources/latest/commonLookups?onlyData=true&q=LookupType=XXX_EMP_CERT_TO_LIST&expand=lookupCodes,translations,lookupCodes.translations");

            // Check if the response was successful
            if (!resp.IsSuccessStatusCode)
                return BadRequest();

            // Read the response content as string
            var json = await resp.Content.ReadAsStringAsync();

            // Deserialize the JSON response into CertificateTypes object
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<ToWhomConcerns>(json);

            // Filter enabled values and map them to a simplified structure
            var enabledValues = data.Items
                .SelectMany(x => x.lookupCodes) // Flatten the list of values
                .Where(v => v.EnabledFlag == "Y") // Filter values that are enabled
                .Select(v => new
                {
                    v.LookupCode,
                    v.Meaning,
                    v.Description
                });

            // Return the filtered enabled values as a response
            return Ok(enabledValues);
        }

        [HttpPost("postleave")]
        public async Task<IActionResult> PostLeaves([FromBody] LeaveRequestModel model)
        {
            var client = _httpClientFactory.CreateClient("iHC");

            var jsonContent = new StringContent(
                System.Text.Json.JsonSerializer.Serialize(model),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            var resp = await client.PostAsync("hcmRestApi/resources/latest/absences", jsonContent);
            if (!resp.IsSuccessStatusCode)
                return StatusCode((int)resp.StatusCode, "Error posting leave");

            var responseJson = await resp.Content.ReadAsStringAsync();
            return Content(responseJson, "application/json");
        }
        [HttpPost("records")]
        public async Task<IActionResult> PostDocumentRecord([FromBody] DocumentRecordRequestModel model)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("iHC");

                // Add correlation ID for tracking
                var correlationId = Guid.NewGuid().ToString();
                client.DefaultRequestHeaders.Add("X-Correlation-ID", correlationId);

                // Serialize with case-insensitive options
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                var payload = JsonSerializer.Serialize(model, options);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                var resp = await client.PostAsync("hcmRestApi/resources/latest/documentRecords", content);
                var respBody = await resp.Content.ReadAsStringAsync();



                // Handle empty responses
                if (string.IsNullOrWhiteSpace(respBody))
                {
                    return StatusCode((int)resp.StatusCode, new
                    {
                        CorrelationId = correlationId,
                        Message = "Empty response from Oracle HCM",
                        Diagnostic = new
                        {
                            StatusCode = resp.StatusCode,
                            RequestUrl = resp.RequestMessage?.RequestUri,
                            Headers = resp.RequestMessage?.Headers
                        }
                    });
                }

                // Attempt JSON parse
                try
                {
                    var jsonDoc = JsonDocument.Parse(respBody);
                    return new JsonResult(jsonDoc, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    })
                    {
                        StatusCode = (int)resp.StatusCode
                    };
                }
                catch (JsonException ex)
                {

                    return StatusCode((int)resp.StatusCode, new
                    {
                        CorrelationId = correlationId,
                        Message = "Invalid JSON response from Oracle HCM",
                        RawResponse = respBody,
                        Diagnostic = new
                        {
                            Headers = resp.Headers,
                            ContentType = resp.Content.Headers.ContentType
                        }
                    });
                }
            }
            catch (HttpRequestException ex)
            {

                return StatusCode(500, new
                {
                    Message = "Connection to Oracle HCM failed",
                    Error = ex.Message,
                    InnerError = ex.InnerException?.Message
                });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    Message = "Internal server error",
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }
        [HttpGet("getDocumentOfRecordId")]
        public async Task<IActionResult> GetDocumentOfRecordId(string personNumber, DateTime requestDate)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("iHC");

                var response = await client.GetAsync($"hcmRestApi/resources/latest/documentRecords?q=PersonNumber={personNumber};SystemDocumentType=SA_OVERTIME_ASSIGNMENT&limit=300&onlyData=true&expand=documentRecordsDFF,documentRecordsDDF");
                var responseBody = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(responseBody))
                {
                    return StatusCode((int)response.StatusCode, new
                    {
                        Message = "No document records found for the provided PersonNumber",
                    });
                }

                var jsonDoc = JsonDocument.Parse(responseBody);
                JsonElement items = jsonDoc.RootElement.GetProperty("items");

                (string docId, DateTime toDate, int hours)? latestMatch = null;

                foreach (JsonElement item in items.EnumerateArray())
                {
                    if (item.TryGetProperty("documentRecordsDFF", out JsonElement dffArray) &&
                        dffArray.ValueKind == JsonValueKind.Array)
                    {
                        foreach (JsonElement dffItem in dffArray.EnumerateArray())
                        {
                            if (dffItem.TryGetProperty("toDate", out JsonElement toDateElement) &&
                                DateTime.TryParse(toDateElement.GetString(), out DateTime toDate) &&
                                toDate.Year == requestDate.Year && toDate.Month == requestDate.Month)
                            {
                                var docId = item.GetProperty("DocumentsOfRecordId").ToString();
                                var hours = dffItem.TryGetProperty("overtimeHours", out JsonElement hoursElem)
                                    ? hoursElem.GetInt32()
                                    : 0;

                                if (latestMatch == null || toDate > latestMatch.Value.toDate)
                                {
                                    latestMatch = (docId, toDate, hours);
                                }
                            }
                        }
                    }
                }

                if (latestMatch != null)
                {
                    return Ok(new
                    {
                        DocumentOfRecordId = latestMatch.Value.docId,
                        ToDate = latestMatch.Value.toDate.ToString("yyyy-MM-dd"),
                        OvertimeHours = latestMatch.Value.hours
                    });
                }

                return BadRequest("No matching document found for the given month and year.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Error fetching DocumentOfRecordId",
                    Error = ex.Message,
                });
            }
        }



        [HttpPost("submitovertime")]
        public async Task<IActionResult> PostOvertimePaymentRecord([FromBody] OvertimePaymentRequest model)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("iHC");

                // Add correlation ID for tracking
                var correlationId = Guid.NewGuid().ToString();
                client.DefaultRequestHeaders.Add("X-Correlation-ID", correlationId);

                // Serialize with case-insensitive options
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };

                var payload = JsonSerializer.Serialize(model, options);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                // Post the overtime payment data to the external API
                var resp = await client.PostAsync("hcmRestApi/resources/latest/documentRecords", content);
                // Handle empty response
                if (resp.Content == null)
                {
                    return StatusCode((int)resp.StatusCode, new
                    {
                        CorrelationId = correlationId,
                        Message = "Empty response from Oracle HCM"
                    });
                }
                byte[] responseBytes = await resp.Content.ReadAsByteArrayAsync();
                string respBody;
                try
                {
                    if (IsGzipped(responseBytes))
                    {
                        using var memoryStream = new MemoryStream(responseBytes);
                        using var gzipStream = new System.IO.Compression.GZipStream(memoryStream, CompressionMode.Decompress);
                        using var streamReader = new StreamReader(gzipStream, Encoding.UTF8);
                        respBody = await streamReader.ReadToEndAsync();
                    }
                    else
                    {
                        respBody = Encoding.UTF8.GetString(responseBytes);
                    }
                }
                catch (Exception decompressEx)
                {

                    return StatusCode(500, new
                    {
                        CorrelationId = correlationId,
                        Message = "Failed to process Oracle HCM response",
                        Diagnostic = "Decompression error"
                    });
                }
                // Parse response
                try
                {
                    return StatusCode((int)resp.StatusCode, new
                    {
                        CorrelationId = correlationId,
                        Message = respBody,
                        Diagnostic = "Decompression error"
                    });
                }
                catch (JsonException)
                {
                    // Check if response is actually JSON with unexpected encoding
                    var contentType = resp.Content.Headers.ContentType?.MediaType ?? "unknown";
                    return StatusCode((int)resp.StatusCode, new
                    {
                        CorrelationId = correlationId,
                        Message = contentType.StartsWith("application/json")
                            ? "Invalid JSON format from Oracle HCM"
                            : "Non-JSON response from Oracle HCM",
                        RawResponse = respBody
                    });
                }
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    Message = "Connection to Oracle HCM failed",
                    Error = ex.Message,
                    InnerError = ex.InnerException?.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Internal server error",
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }

        [HttpPost("submitabsence")]
        public async Task<IActionResult> PostAbsence([FromBody] AbsenceRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Message = "Validation failed",
                    Errors = ModelState.Values.SelectMany(v => v.Errors)
                });
            }

            var correlationId = Guid.NewGuid().ToString();
            try
            {
                using var client = _httpClientFactory.CreateClient("iHC");
                client.DefaultRequestHeaders.Add("X-Correlation-ID", correlationId);

                // Serialize and send request
                var payload = JsonSerializer.Serialize(model, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                var content = new StringContent(payload, Encoding.UTF8, "application/json");
                var resp = await client.PostAsync("hcmRestApi/resources/latest/absences", content);

                // Handle empty response
                if (resp.Content == null)
                {
                    return StatusCode((int)resp.StatusCode, new
                    {
                        CorrelationId = correlationId,
                        Message = "Empty response from Oracle HCM"
                    });
                }

                // Decompress response
                byte[] responseBytes = await resp.Content.ReadAsByteArrayAsync();
                string respBody;
                try
                {
                    if (IsGzipped(responseBytes))
                    {
                        using var memoryStream = new MemoryStream(responseBytes);
                        using var gzipStream = new System.IO.Compression.GZipStream(memoryStream, CompressionMode.Decompress);
                        using var streamReader = new StreamReader(gzipStream, Encoding.UTF8);
                        respBody = await streamReader.ReadToEndAsync();
                    }
                    else
                    {
                        respBody = Encoding.UTF8.GetString(responseBytes);
                    }
                }
                catch (Exception decompressEx)
                {

                    return StatusCode(500, new
                    {
                        CorrelationId = correlationId,
                        Message = "Failed to process Oracle HCM response",
                        Diagnostic = "Decompression error"
                    });
                }

                // Parse response
                try
                {
                    return StatusCode((int)resp.StatusCode, new
                    {
                        CorrelationId = correlationId,
                        Message = respBody,
                        Diagnostic = "Decompression error"
                    });
                }
                catch (JsonException)
                {
                    // Check if response is actually JSON with unexpected encoding
                    var contentType = resp.Content.Headers.ContentType?.MediaType ?? "unknown";
                    return StatusCode((int)resp.StatusCode, new
                    {
                        CorrelationId = correlationId,
                        Message = contentType.StartsWith("application/json")
                            ? "Invalid JSON format from Oracle HCM"
                            : "Non-JSON response from Oracle HCM",
                        RawResponse = respBody
                    });
                }
            }
            catch (HttpRequestException ex)
            {

                return StatusCode(502, new
                {
                    CorrelationId = correlationId,
                    Message = "Connection to Oracle HCM failed",
                    Error = ex.Message
                });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    CorrelationId = correlationId,
                    Message = "Internal server error",
                    Error = ex.Message
                });
            }
        }

        // Helper method to detect GZIP compression
        private static bool IsGzipped(byte[] bytes)
        {
            return bytes.Length >= 2 && bytes[0] == 0x1F && bytes[1] == 0x8B;
        }



        [HttpGet("absenceTypes")]
        public async Task<IActionResult> GetAbsenceTypes()
        {
            using var client = _httpClientFactory.CreateClient("iHC");

            // Update the URL with finder and fields
            var endpoint = "hcmRestApi/resources/latest/absenceTypesLOV?onlyData=true&finder=findByWord;PersonId=300000007320021&fields=AbsenceTypeId,AbsenceTypeName,Description";

            var resp = await client.GetAsync(endpoint);
            if (!resp.IsSuccessStatusCode)
                return StatusCode((int)resp.StatusCode, "Failed to load absence types.");

            var json = await resp.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<AbsenceTypeResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            // Assuming AbsenceTypeResponse.Items is a list of objects with AbsenceTypeName and Description
            var options = data.Items
                .Select(x => new { value = x.AbsenceTypeId, text = x.Name, description = x.Description })
                .ToList();

            return Ok(options);
        }
        [HttpPost("absenceTypeBalance")]
        public async Task<IActionResult> GetAbsenceTypeBalance([FromBody] AbsenceBalanceRequest model)
        {
            var client = _httpClientFactory.CreateClient("iHC");

            var request = new HttpRequestMessage(HttpMethod.Post, "hcmRestApi/resources/latest/absences/action/getAbsenceTypeBalance");
            request.Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/vnd.oracle.adf.action+json");

            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to retrieve balance.");

            var json = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AbsenceBalanceResult>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return Ok(result?.Result);
        }
        [HttpGet("businessTripTypes")]
        public async Task<IActionResult> GetBusinessTripTypes()
        {
            var client = _httpClientFactory.CreateClient("iHC");
            var response = await client.GetAsync(
                "/fscmRestApi/resources/latest/commonLookups" +
                "?onlyData=true" +
                "&q=LookupType=XXX_BUSINESS_TRIP_TYPE" +
                "&expand=lookupCodes,translations,lookupCodes.translations"
            );

            if (!response.IsSuccessStatusCode)
                return BadRequest("Failed to retrieve business trip types.");

            var json = await response.Content.ReadAsStringAsync();

            // Use System.Text.Json for deserialization
            var data = JsonSerializer.Deserialize<BuisnessTripType>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            if (data?.Items == null)
                return Ok(Enumerable.Empty<object>());

            var enabled = data.Items
                .SelectMany(item => item.LookupCodes)
                .Where(code => code.EnabledFlag == "Y")
                .Select(code => new
                {
                    Value = code.LookupCode,
                    Text = code.Meaning
                });

            return Ok(enabled);
        }
        [HttpGet("countryFrom")]
        public async Task<IActionResult> GetUserTableDetailValues()
        {
            var client = _httpClientFactory.CreateClient("iHC");

            // Build the query string
            var url = $"/hcmRestApi/resources/11.13.18.05/userTableDetailValues" +
                      $"?onlyData=true&q=UserTableId=300000002077130";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to retrieve user-table values.");

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<CountryFrom>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            if (data?.Items == null || !data.Items.Any())
                return Ok(Enumerable.Empty<object>());

            // Optionally sort by DisplaySequence
            var options = data.Items
                .OrderBy(item => item.DisplaySequence)
                .Select(item => new
                {
                    Value = item.UserRowId,
                    Text = item.RowName
                });

            return Ok(options);
        }

        [HttpGet("cityList")]
        public async Task<IActionResult> GetCityList()
        {
            var client = _httpClientFactory.CreateClient("iHC");
            var url = "/fscmRestApi/resources/11.13.18.05/valueSets" +
                      "?onlyData=true" +
                      "&q=ValueSetCode=XXX_CITY_LIST" +
                      "&expand=values";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Unable to retrieve city list.");

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<CityLists>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );
            if (data?.Items == null || !data.Items.Any())
                return Ok(Enumerable.Empty<object>());

            var cities = data.Items
                .SelectMany(vs => vs.Values)
                .Where(v => v.EnabledFlag == "Y")
                .Select(v => new
                {
                    Value = v.Value,
                    Text = v.Description ?? v.Value
                });

            return Ok(cities);
        }

        [HttpGet("VisaType")]
        public async Task<IActionResult> GetYesNoOptions([FromHeader(Name = "Accept-Language")] string acceptLanguage = "US")
        {
            var client = _httpClientFactory.CreateClient("iHC");
            var url = "/fscmRestApi/resources/latest/commonLookups" +
                      "?onlyData=true" +
                      "&q=LookupType=HRC_YES_NO" +
                      "&expand=lookupCodes,translations,lookupCodes.translations";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Unable to retrieve Yes/No options.");

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<VisaType>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            var options = data.Items
                .SelectMany(item => item.LookupCodes)
                .Where(code => code.EnabledFlag == "Y")
                .Select(code =>
                {
                    // Try to find translation matching the requested language (e.g. "AR" or "US")
                    var translation = code.Translations?
                        .FirstOrDefault(t => t.Language.Equals(acceptLanguage, StringComparison.OrdinalIgnoreCase))
                        ?.Meaning;

                    return new
                    {
                        Value = code.LookupCode,
                        Text = translation ?? code.Meaning  // fallback to default Meaning
                    };
                });

            return Ok(options);
        }

        [HttpGet("GetJob")]
        public async Task<IActionResult> GetJob([FromQuery] long jobId)
        {
            var client = _httpClientFactory.CreateClient("iHC");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = $"https://fa-euvo-test-saasfaprod1.fa.ocs.oraclecloud.com/hcmRestApi/resources/latest/jobsLov?onlyData=true&q=JobId={jobId}&fields=JobId,JobCode,JobName";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "API call failed.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var jobResponse = JsonSerializer.Deserialize<JobApiResponse>(json, options);

            return Ok(jobResponse.Items);
        }
        [HttpGet("SubstituteEmployee")]
        public async Task<IActionResult> SubstituteEmployee(int personNumber, string culture)
        {
            var client = _httpClientFactory.CreateClient("iHC");
            var url = "/hcmRestApi/resources/11.13.18.05/workers" +
                      "?onlyData=true" +
                      "&q=PersonNumber!=" + personNumber +
                      "&expand=names";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Error fetching workers.");

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<WorkerResponse>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            // Project into simple value/text pairs
            var result = data.Items
                .Select(w =>
                {
                    // pick the first name record (e.g., NameLanguage=="US")
                    var name = w.Names.FirstOrDefault(n => n.NameLanguage == culture) ?? w.Names.First();
                    return new
                    {
                        Value = w.PersonId,
                        Text = $"{name.FirstName} {name.LastName}"
                    };
                })
                .ToList();

            return Ok(result);
        }
        [HttpPost]
        [Route("SubmitBusinessTrip")]
        public async Task<IActionResult> SubmitBusinessTrip([FromBody] BusinessTripRequest requestModel)
        {
            using var client = _httpClientFactory.CreateClient("iHC");

            // Add correlation ID for tracking
            var correlationId = Guid.NewGuid().ToString();
            client.DefaultRequestHeaders.Add("X-Correlation-ID", correlationId);

            // Serialize with case-insensitive options
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null,
                WriteIndented = true
            };

            var payload = JsonSerializer.Serialize(requestModel, options);
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            var drResponse = await client.PostAsync(
                "/hcmRestApi/resources/latest/documentRecords",
                content
            );

            if (!drResponse.IsSuccessStatusCode)
            {

                // Handle empty response
                if (drResponse.Content == null)
                {
                    return StatusCode((int)drResponse.StatusCode, new
                    {
                        CorrelationId = correlationId,
                        Message = "Empty response from Oracle HCM"
                    });
                }

                // Decompress response
                byte[] responseBytes = await drResponse.Content.ReadAsByteArrayAsync();
                string respBody;
                try
                {
                    if (IsGzipped(responseBytes))
                    {
                        using var memoryStream = new MemoryStream(responseBytes);
                        using var gzipStream = new System.IO.Compression.GZipStream(memoryStream, CompressionMode.Decompress);
                        using var streamReader = new StreamReader(gzipStream, Encoding.UTF8);
                        respBody = await streamReader.ReadToEndAsync();
                    }
                    else
                    {
                        respBody = Encoding.UTF8.GetString(responseBytes);
                    }
                }
                catch (Exception decompressEx)
                {

                    return StatusCode(500, new
                    {
                        CorrelationId = correlationId,
                        Message = "Failed to process Oracle HCM response",
                        Diagnostic = "Decompression error"
                    });
                }

                // Parse response
                try
                {
                    return StatusCode((int)drResponse.StatusCode, new
                    {
                        CorrelationId = correlationId,
                        Message = respBody,
                        Diagnostic = "Decompression error"
                    });
                }
                catch (JsonException)
                {
                    // Check if response is actually JSON with unexpected encoding
                    var contentType = drResponse.Content.Headers.ContentType?.MediaType ?? "unknown";
                    return StatusCode((int)drResponse.StatusCode, new
                    {
                        CorrelationId = correlationId,
                        Message = contentType.StartsWith("application/json")
                            ? "Invalid JSON format from Oracle HCM"
                            : "Non-JSON response from Oracle HCM",
                        RawResponse = respBody
                    });
                }
            }


            var drJson = await drResponse.Content.ReadAsStringAsync();
            var drResult = JsonSerializer.Deserialize<DocumentRecordResponse>(
                drJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            var recordId = drResult.DocumentsOfRecordId;


            return Ok(new { DocumentsOfRecordId = recordId });
        }
        [HttpGet("GetWayOfTravel")]
        public async Task<IActionResult> GetWayOfTravel()
        {
            var client = _httpClientFactory.CreateClient("iHC");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var url = "/fscmRestApi/resources/11.13.18.05/valueSets?q=ValueSetCode=XXX_Way_Of_Travel&expand=values&onlyData=true&fields=ValueSetCode;values:Value";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Way Of Travel API call failed.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<WayOfTravel>(
               json,
               new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
           );
            var options = data.Items
                .SelectMany(item => item.Values)
                .Select(code =>
                {
                    var ApiValue = code.Value;

                    return new
                    {
                        Value = ApiValue,
                        Text = ApiValue
                    };
                });
            return Ok(options);
        }

        [HttpGet("GetDependantsData")]
        public async Task<IActionResult> GetDependantsData([FromQuery] string personNumber)
        {
            var client = _httpClientFactory.CreateClient("iHC"); // Ensure this client is configured in Startup

            var url = $"hcmRestApi/resources/latest/hcmContacts?" +
                      $"finder=findContactsByWorker;RelatedPersonNumber={personNumber}" +
                      $"&expand=names,contactRelationships,nationalIdentifiers,legislativeInfo,addresses&onlyData=true" +
                      $"&fields=names:DisplayName;contactRelationships:ContactTypeMeaning;addresses;phones";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Failed to retrieve contact information.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var result = JsonSerializer.Deserialize<Dependents>(json, options);

            return Ok(result?.Items);
        }
        [HttpGet("GetManagerName")]
        public async Task<IActionResult> GetManagerName([FromQuery] string personNumber)
        {
            var client = _httpClientFactory.CreateClient("iHC");

            var url = $"hcmRestApi/resources/latest/publicWorkers?q=PersonNumber={Uri.EscapeDataString(personNumber)}" +
                      $"&expand=assignments&onlyData=true&fields=PersonId,PersonNumber,LastName,FirstName;assignments:ManagerName";

            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to retrieve manager name.");

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<PublicWorkerResponse>(json, options);

            return Ok(result?.Items?.FirstOrDefault()?.Assignments?.FirstOrDefault()?.ManagerName);
        }
        [HttpGet("GetExperience")]
        public async Task<IActionResult> GetExperience([FromQuery] string personNumber)
        {
            var client = _httpClientFactory.CreateClient("iHC");

            var url = $"hcmRestApi/resources/latest/talentPersonProfiles?q=PersonNumber={Uri.EscapeDataString(personNumber)}" +
                      $"&expand=workHistorySections.workHistoryItems&onlyData=true";

            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "Failed to retrieve experience data.");

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<TalentProfileResponse>(json, options);

            var workItems = result?.Items?
                .SelectMany(p => p.WorkHistorySections)
                .SelectMany(s => s.WorkHistoryItems)
                .Select(item => new
                {
                    item.JobTitle,
                    item.EmployerName,
                    StartDate = item.StartDate?.ToString("yyyy-MM-dd"),
                    EndDate = item.EndDate?.ToString("yyyy-MM-dd")
                })
                .ToList();

            return Ok(workItems);
        }
        [HttpPost]
        [Route("SubmitBusinessTripReturn")]
        public async Task<IActionResult> SubmitBusinessTripReturn([FromBody] BuisnessTripReturn requestModel)
        {
            try
            {
                using var client = _httpClientFactory.CreateClient("iHC");

                var correlationId = Guid.NewGuid().ToString();
                client.DefaultRequestHeaders.Add("X-Correlation-ID", correlationId);

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = null,
                    WriteIndented = true
                };

                var payload = JsonSerializer.Serialize(requestModel, options);
                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                var drResponse = await client.PostAsync("/hcmRestApi/resources/latest/documentRecords", content);

                if (!drResponse.IsSuccessStatusCode)
                {

                    // Handle empty response
                    if (drResponse.Content == null)
                    {
                        return StatusCode((int)drResponse.StatusCode, new
                        {
                            CorrelationId = correlationId,
                            Message = "Empty response from Oracle HCM"
                        });
                    }

                    // Decompress response
                    byte[] responseBytes = await drResponse.Content.ReadAsByteArrayAsync();
                    string respBody;
                    try
                    {
                        if (IsGzipped(responseBytes))
                        {
                            using var memoryStream = new MemoryStream(responseBytes);
                            using var gzipStream = new System.IO.Compression.GZipStream(memoryStream, CompressionMode.Decompress);
                            using var streamReader = new StreamReader(gzipStream, Encoding.UTF8);
                            respBody = await streamReader.ReadToEndAsync();
                        }
                        else
                        {
                            respBody = Encoding.UTF8.GetString(responseBytes);
                        }
                    }
                    catch (Exception decompressEx)
                    {

                        return StatusCode(500, new
                        {
                            CorrelationId = correlationId,
                            Message = "Failed to process Oracle HCM response",
                            Diagnostic = "Decompression error"
                        });
                    }

                    // Parse response
                    try
                    {
                        return StatusCode((int)drResponse.StatusCode, new
                        {
                            CorrelationId = correlationId,
                            Message = respBody,
                            Diagnostic = "Decompression error"
                        });
                    }
                    catch (JsonException)
                    {
                        // Check if response is actually JSON with unexpected encoding
                        var contentType = drResponse.Content.Headers.ContentType?.MediaType ?? "unknown";
                        return StatusCode((int)drResponse.StatusCode, new
                        {
                            CorrelationId = correlationId,
                            Message = contentType.StartsWith("application/json")
                                ? "Invalid JSON format from Oracle HCM"
                                : "Non-JSON response from Oracle HCM",
                            RawResponse = respBody
                        });
                    }
                }


                var drJson = await drResponse.Content.ReadAsStringAsync();
                var drResult = JsonSerializer.Deserialize<DocumentRecordResponse>(
                    drJson,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                var recordId = drResult.DocumentsOfRecordId;


                return Ok(new { DocumentsOfRecordId = recordId });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
