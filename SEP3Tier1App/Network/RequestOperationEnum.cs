﻿﻿using System.Text.Json.Serialization;

namespace WebApplication.Network
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RequestOperationEnum
    {
        EDITINTRODUCTION,
        PROFILE, 
        COVER,
        PICTURES,
        MATCHES,
        UPLOADPICTURE,
        EDITABOUT,
        REVIEWS,
        UPDATECOVER,
        PROFILEPIC,
        UPDATEPROFILEPIC, 
        SUCCESS,
        ERROR,
        CREATEPROFILE,
        CREATEPREFERENCE,
        DELETEPHOTO,
        GETPREFERENCE, 
        GETCONNECTIONS,
        EDITPROFILE,
        EDITPREFERENCE,
        GETALLMESSAGES,
        REGISTERUSER,
        GETPROFILEID,
        CHANGEPASSWORD,
        CHANGEUSERNAME,
        SENDMESSAGE,
        NOTIFYABOUTMESSAGES,
        SETUSERNAME,
        REPORTREVIEW,
        WARNUSER,
        REMOVEWARNING,
    }
}