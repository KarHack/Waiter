using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Waiter
{
    /*
     * 
     * This Class will be responsible for Calling the Required API and sending the Params.
     * It will make sure that the API will be called.
     * It will also return the response to the user asynchronous using a callback.
     * This class will talk to the Waiter Manager.
     * The waiter will keep a status of API Call.
     * 
     */
    public class Waiter
    {

        // Implementations
        /*
         // Simple Implementation of the Library.
         new Waiter()
                .Http()
                .Url("URL OF THE API")
                .Endpoint("ENDPOINT OF THE API")
                .AddParam(new ParamData("Param Name").Put("Param Value"))
                .SetOnResponse((bool isSuccess, string response) => {
                    try
                    {
                        // Here we will perform actions on the response.
                        // TODO : To be Implemented.
                    } catch (Exception e)
                    {
                        // Here we have an Error.
                    }
                })
                .Execute();   

         */

        // Variables
        internal string url { get; set; }
        internal string endpoint { get; set; }
        private List<ParamData> paramDatas;
        private string Status { get; set; }
        private StringBuilder StatusTrace;
        internal CallMethod callMethod = CallMethod.GET;
        internal string ID { get; set; }
        private string UID { get; set; }
        internal string data { get; set; }
        internal CallType callType = CallType.HTTP;

        // The Callback related Methods and Variables.
        public delegate void OnResponse(bool successful, string response);
        private OnResponse onResponse;  // The method will be set here.

        // Set the Method Enums
        public enum CallMethod
        {
            GET, POST, PUT, DELETE
        }

        // Set the Type Enum.
        public enum CallType
        {
            HTTP, HTTPS
        }

        public Waiter()
        {
            try
            {
                // Initialize the Required Variables.
                paramDatas = new List<ParamData>();
                // Now we will communicate with the Waiter Manager to Clear all the Running Queries from the system.
                ID = WaiterManager.Init(this);
                // Add the Status.
                AddStatus("Initialized");
            }
            catch (Exception e)
            {
                // There was an Error.
            }
        }

        // The Getter & Setter of the UID set by the user.
        // Lets set the UID.
        public Waiter SetUID(string uid)
        {
            this.UID = uid;
            return this;
        }

        // Lets get the UID.
        public string GetUID()
        {
            return UID;
        }


        // Lets set the API Call type (HTTP / HTTPS)
        // Lets set the Type to HTTP.
        public Waiter Http()
        {
            // Lets set the Call type to be HTTP.
            callType = CallType.HTTP;
            return this;
        }

        // Lets set the Type to HTTPS.
        public Waiter Https()
        {
            // Lets set the Call type to be HTTPS.
            callType = CallType.HTTPS;
            return this;
        }

        // Lets set the URL of the API that needs to be called.
        public Waiter Url(string urlP)
        {
            try
            {
                // Lets set the API.
                url = urlP;
                // Add the Status.
                AddStatus("URL Added");
                // Lets update our status with the manager.
                WaiterManager.Update(this);
                return this;
            }
            catch (Exception e)
            {
                // There was an Error.
                return null;
            }
        }

        // Lets set the Endpoint of the API.
        public Waiter Endpoint(string endpointT)
        {
            try
            {
                // Lets set the API.
                endpoint = endpointT;
                AddStatus("Endpoint Added");
                // Lets update our status with the manager.
                WaiterManager.Update(this);
                return this;
            }
            catch (Exception e)
            {
                // There was an Error.
                return null;
            }
        }

        // Set the Method to be used to make the request.
        public Waiter Method(CallMethod method)
        {
            try
            {
                // Lets set the Method.
                callMethod = method;
                AddStatus("Method Set");
            }
            catch (Exception e)
            {
                // There was an Error.
                AddStatus("Method Not Set");
            }
            // Lets update our status with the manager.
            WaiterManager.Update(this);
            return this;
        }

        // Lets allow the user to set the params that he wants to send to the API.
        // We will also allow the user to add params one at a time.
        public Waiter AddParam(ParamData paramData)
        {
            try
            {
                // Lets try to add the params to the list.
                paramDatas.Add(paramData);
            }
            catch (Exception e)
            {
                // There was an Error.
                AddStatus("Param Err : " + e.Message);
            }
            // Lets update our status with the manager.
            WaiterManager.Update(this);
            return this;
        }

        // Set the Callback of the User.
        public Waiter SetOnResponse(OnResponse onResponse)
        {
            this.onResponse = onResponse;
            // Lets update our status with the manager.
            WaiterManager.Update(this);
            return this;
        }

        // The callback for the user.
        private void SendToUser(bool successful, string response)
        {
            onResponse(successful, response);
        }

        // Lets Execute the Request.
        public bool Execute()
        {
            try
            {
                // Here we will validate the run, and execute the query.
                // Lets validate if the Request is allowed to be run.
                bool canExecute = true;
                if (url == null || url.Trim().Length == 0)
                {
                    // The URL is not provided.
                    canExecute = false;
                }
                // Lets run the Request if Allowed.
                if (canExecute)
                {
                    // Execute the Request.
                    AddStatus("Executing");
                    // Setup the Waiter Manager to Insert into the database.
                    WaiterManager.Begin(this);
                    // Lets create another thread.
                    AsyncStart();
                    return true;
                }
                else
                {
                    // There was an Error, and we cannot execute the request.
                    return false;
                }
            }
            catch (Exception e)
            {
                // There was an Error.
                return false;
            }
        }

        // Execute Request Async.
        public bool ExecuteAsync()
        {
            try
            {
                // Here we will validate the run, and execute the query.
                // Lets validate if the Request is allowed to be run.
                bool canExecute = true;
                if (url == null || url.Trim().Length == 0)
                {
                    // The URL is not provided.
                    canExecute = false;
                }
                // Lets run the Request if Allowed.
                if (canExecute)
                {
                    // Execute the Request.
                    // Lets create another thread.
                    AddStatus("Executing Async");
                    // Setup the Waiter Manager to Insert into the database.
                    WaiterManager.Begin(this);
                    Thread t = new Thread(new ThreadStart(AsyncStart));
                    t.Start();
                    return true;
                }
                else
                {
                    // There was an Error, and we cannot execute the request.
                    return false;
                }
            }
            catch (Exception e)
            {
                // There was an Error.
                return false;
            }
        }

        // Here we set the status and the status trace.
        private void AddStatus(string status)
        {
            try
            {
                // Here we append the status to the status trace and status.
                if (StatusTrace == null)
                {
                    StatusTrace = new StringBuilder();
                }
                this.Status = status;
                StatusTrace.Append(status)
                    .Append('\n');
            }
            catch (Exception e)
            {
                // There was an Error.
            }
        }

        // This will retrieve the whole status trace.
        public string GetStatusTrace()
        {
            return StatusTrace.ToString();
        }

        // This will retrieve just the status of this object.
        public string GetStatus()
        {
            return Status;
        }

        // This will just call the Async task.
        private void AsyncStart()
        {
            try
            {
                // Lets call the Async task.
                Async(this).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                // There was an Error.
            }
        }

        /*
         * 
         * This class will make the Calls to the Server.
         * Only one instance of this Class will be Created.
         * The Waiter will call this class for Hitting the API.
         * 
         */
        private async Task Async(Waiter waiter)
        {
            try
            {
                // Lets create the Caller, and Call the API with the params.
                // Lets Get the Ready to call the API.
                HttpClient client = new HttpClient();

                // Add the Base Address and the End Point
                client.BaseAddress = new Uri((callType.Equals(CallType.HTTPS) ? "https://" : "http://")
                                    + waiter.url + "/" + waiter.endpoint);

                // Lets Add the Params.
                StringBuilder paramBuild = new StringBuilder();
                foreach (ParamData paramData in paramDatas)
                {
                    if (paramBuild.Length == 0)
                        paramBuild.Append("?");
                    paramBuild.Append(paramData.GetParam())
                        .Append("=")
                        .Append(paramData.GetData())
                        .Append('&');
                }

                HttpResponseMessage response = null;
                // Set the Method of the Request.
                switch (waiter.callMethod)
                {
                    case CallMethod.GET:
                        response = client.GetAsync(paramBuild.ToString()).Result;
                        break;
                    case CallMethod.POST:
                        response = client.PostAsync(paramBuild.ToString(), null).Result;
                        break;
                    case CallMethod.PUT:
                        response = client.PutAsync(paramBuild.ToString(), null).Result;
                        break;
                    case CallMethod.DELETE:
                        response = client.DeleteAsync(paramBuild.ToString()).Result;
                        break;
                }

                // Lets Execute the Request.
                if (response.IsSuccessStatusCode)
                {
                    // Get the response
                    string responseJsonString = await response.Content.ReadAsStringAsync();

                    // Sent the data to the user.
                    if (waiter.onResponse != null)
                    {
                        waiter.SendToUser(true, responseJsonString);
                    }
                    waiter.AddStatus("Executed Successfully");
                }
                else
                {
                    // The Data was not sent.
                    if (waiter.onResponse != null)
                    {
                        waiter.SendToUser(false, "Err : Not Writend Null");
                    }
                    waiter.AddStatus("Executed With an Error");
                }
                // Set to the Waiter Manager, that the tast has been ended.
                WaiterManager.End(waiter);
            }
            catch (Exception e)
            {
                // There was an Error.
                if (waiter.onResponse != null)
                {
                    waiter.SendToUser(false, "Err : Async : " + e.Message);
                }
                waiter.AddStatus("Executed With an Error : " + e.Message);
                // Set to the Waiter Manager, that the tast has been ended.
                WaiterManager.End(waiter);
            }
            // Set to the Waiter Manager, that the tast has been ended.
            WaiterManager.End(waiter);
        }

    }

}
