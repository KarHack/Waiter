# Waiter
A simple but guaranteed API Call function with callbacks for ease of programming.
It allows for Synchronous & A-Synchronous Calls.

Implementation of the Library.

>        new Waiter()
>               .Http() or .Https()
>               .Url("URL OF THE API")
>               .Endpoint("ENDPOINT OF THE API")
>               .AddParam(new ParamData("Param Name").Put("Param Value"))
>               .AddParam("Param Name", "Param Value")
>               .SetOnResponse((bool isSuccess, string response) => {
>                   try
>                   {
>                       // Here we will perform actions on the response.
>                       // TODO : To be Implemented.
>                   } catch (Exception e)
>                   {
>                       // Here we have an Error.
>                   }
>               })
>               .Execute();   or .ExecuteAsync();

We have a WaiterManager to allow you to interact with the current running calls.

We are also planning to build features like Guaranteed API Calling by linking with an embedded database like SQLite.
