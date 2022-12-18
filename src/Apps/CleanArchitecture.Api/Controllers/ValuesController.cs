using BkashSNS.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System;
using BkashSNS.Application.Common.Interfaces;
using BkashSNS.Infrastructure.Services.Helper;

namespace BkashSNS.Api.Controllers
{
    [Route("api/awssns")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly ILogger<ValuesController> _logger;
        private readonly ILogService _logService;
        private readonly IClientService _clientService;

        public ValuesController(ILogger<ValuesController> logger,ILogService logService,IClientService clientService)
        {
            _logger = logger;
            _logService = logService;
            _clientService = clientService;
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> webhook()
        {

            var re = Request;
            var headers = re.Headers;
            string body = "";
            try
            {

                string messagetype = Request.Headers["x-amz-sns-message-type"];
                //if (headers.Contains("Custom"))
                //{
                //    string token = headers.TryGetValue("Custom").First();
                //}
                //Logger.Debug(messagetype);

                _logger.LogInformation("**** MSDSL BEGIN LOG ****");
                _logger.LogInformation(messagetype);


                //HttpContext.Request.Body.Seek(0, SeekOrigin.Begin);

                using (StreamReader stream = new StreamReader(HttpContext.Request.Body))
                {
                    body = stream.ReadToEnd();
                    // body = "param=somevalue&param2=someothervalue"
                    _logger.LogInformation(body);




                    Messages msg = JsonConvert.DeserializeObject<Messages>(body);

                    if (msg != null)
                    {
                        if (msg.SignatureVersion.Equals("1"))
                        {
                            //temp
                            {
                                var aws = Amazon.SimpleNotificationService.Util.Message.ParseMessage(body);
                                bool r = aws.IsMessageSignatureValid();
                                if (r)
                                {
                                    _logger.LogInformation("Signature verification succeeded");
                                }
                                else
                                {
                                    Message message = JsonConvert.DeserializeObject<Message>(msg.Message.ToString());
                                    _logger.LogInformation("Signature verification failed");
                                    await _logService.Insert(new ClientLog { Message = JsonConvert.SerializeObject(msg.Message), Response = body, Error = "Signature verification failed", Timestamp = DateTime.Now, MerchantWallet = message.MerchantWallet });
                                    return BadRequest("Signature verification failed");
                                }
                            }
                            if (messagetype.Equals("Notification"))
                            {
                                _logger.LogInformation("Subject : " + msg.Subject);


                                Message message = JsonConvert.DeserializeObject<Message>(msg.Message.ToString()); //todo

                                _logger.LogInformation("Message : " + msg.Message.ToString());
                                //_logger.LogInformation("Message : " + JsonConvert.SerializeObject(message) );

                                if (message.Timestamp.Contains("+"))
                                {
                                    string temp = message.Timestamp.Split("+")[0];
                                    message.Timestamp2 = //DateTime.ParseExact(msg.Message.timestamp, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                                    DateTime.Parse(temp.Substring(0, temp.Length - 5));
                                }
                                else // bkash last response 20210809174716   yyyy mm dd
                                {
                                    message.Timestamp2 = DateTime.ParseExact(message.Timestamp, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                                }

                                await _clientService.Insert(message);
                                // r.Wait();
                                //if (r)
                                //{
                                //    _logger.LogInformation("ClientManager().Insert : Success " );
                                //}else
                                //{
                                //    _logger.LogInformation("ClientManager().Insert : false ");
                                //}

                                await _logService.Insert(new ClientLog { Message = JsonConvert.SerializeObject(msg.Message), Response = body, Error = "Ok", Timestamp = DateTime.Now, MerchantWallet = message.MerchantWallet });

                                //todo log save

                            }
                            else if (messagetype.Equals("SubscriptionConfirmation"))
                            {


                                await _logService.Insert(new ClientLog { Message = JsonConvert.SerializeObject(msg.Message), Response = body, Error = msg.Message.ToString(), Timestamp = DateTime.Now });
                                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                                var response = await client.GetAsync(msg.SubscribeURL);
                                response.EnsureSuccessStatusCode();
                            }
                            else if (messagetype.Equals("UnsubscribeConfirmation"))
                            {
                                Message message = JsonConvert.DeserializeObject<Message>(msg.Message.ToString());
                                await _logService.Insert(new ClientLog { Message = JsonConvert.SerializeObject(msg.Message), Response = body, Error = "UnsubscribeConfirmation", Timestamp = DateTime.Now, MerchantWallet = message.MerchantWallet });

                                System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

                                var response = await client.GetAsync(msg.UnsubscribeURL);
                                response.EnsureSuccessStatusCode();
                            }
                        }
                        else
                        {
                            Message message = JsonConvert.DeserializeObject<Message>(msg.Message.ToString());
                            _logger.LogInformation("Unexpected signature version. Unable to verify signature.");
                            await _logService.Insert(new ClientLog { Message = JsonConvert.SerializeObject(msg.Message), Response = body, Error = "Unexpected signature version. Unable to verify signature.", Timestamp = DateTime.Now, MerchantWallet = message.MerchantWallet });
                            return BadRequest("Unexpected signature version. Unable to verify signature.");



                        }
                    }
                    else
                    {
                        //if (messagetype.Equals("Notification"))
                        //{
                        //    Message message = JsonConvert.DeserializeObject<Message>(body);



                        //    // Message message = JsonConvert.DeserializeObject<Message>(msg.Message);

                        //    string temp = message.timestamp.Split("+")[0];
                        //    message.timestamp2 = //DateTime.ParseExact(msg.Message.timestamp, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        //    DateTime.Parse(temp.Substring(0, temp.Length - 5));

                        //    await new ClientManager().Insert(message);

                        //    await new LogManager().Insert(new Client_Log { Message = JsonConvert.SerializeObject(message), Response = body, Error = "Ok", timestamp = DateTime.Now }, msg.Message.merchantWallet);

                        //    //todo log save

                        //}
                        await _logService.Insert(new ClientLog { Response = body, Error = "Body Deserialize fail move to line 141", Timestamp = DateTime.Now, Message = "" });

                    }

                }



                _logger.LogInformation("**** MSDSL END LOG ****");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _logService.Insert(new ClientLog { Response = body, Error = ex.Message + ex.StackTrace, Timestamp = DateTime.Now, Message = "" });
                return StatusCode(500, ex);
                //todo db save

            }

            return Ok();

        }

        [HttpGet("[action]")]
        public async Task<IActionResult> getLastPayment(string merchantWallet, string counterNo)
        {
            try
            {
                if (StaticData.IsAuthorized(Request.Headers["Authorization"]) == false)
                {
                    return Unauthorized();
                }



                var data = await _clientService.GetLastPaymentInfo(merchantWallet, counterNo);

                if (data == null)
                {
                    return NotFound("no record found");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetPaymentInfoByDate(string merchantWallet, string fromdate, string toDate, string top)
        {
            try
            {
                if (StaticData.IsAuthorized(Request.Headers["Authorization"]) == false)
                {
                    return Unauthorized();
                }



                if (string.IsNullOrEmpty(top))
                {
                    top = "100";
                }

                var data = await _clientService.GetPaymentInfoByDate(merchantWallet, fromdate, toDate, top);

                if (data == null)
                {
                    return NotFound("no record found");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetLogByDate(string merchantWallet, string fromdate, string toDate, string top)
        {
            try
            {
                if (StaticData.IsAuthorized(Request.Headers["Authorization"]) == false)
                {
                    return Unauthorized();
                }

                if (string.IsNullOrEmpty(top))
                {
                    top = "100";
                }


                var data = await _logService.GetLogByDate(merchantWallet, fromdate, toDate, top);

                if (data == null)
                {
                    return NotFound("no record found");
                }

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }


        //[HttpGet("[action]")]
        //public async Task<IActionResult> GetLastPaymentFromBkashC(string merchantWallet, string counterNo,string customerWallet)
        //{
        //    try
        //    {
        //        if (StaticData.IsAuthorized(Request.Headers["Authorization"]) == false)
        //        {
        //            return Unauthorized();
        //        }

        //        BkashManager bkashManager = new BkashManager();

        //        var data =  bkashManager.GetLastPaymentFromBkash(merchantWallet, counterNo,customerWallet);

        //        if (data == null)
        //        {
        //            return NotFound("no record found");
        //        }

        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex);
        //    }

        //}


        //[HttpGet("[action]")]
        //public async Task<IActionResult> GetLastPaymentFromBkash(string merchantWallet,  string customerWallet)
        //{
        //    try
        //    {
        //        if (StaticData.IsAuthorized(Request.Headers["Authorization"]) == false)
        //        {
        //            return Unauthorized();
        //        }

        //        BkashManager bkashManager = new BkashManager();

        //        var data = bkashManager.GetLastPaymentFromBkash(merchantWallet, customerWallet);

        //        if (data == null)
        //        {
        //            return NotFound("no record found");
        //        }

        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex);
        //    }

        //}


    }
}
