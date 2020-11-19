﻿using System.Text.Json.Serialization;

namespace WebApplication.Network
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RequestOperationEnum
    {
        EDITINTRODUCTION,
        UPLOADPICTURE,
        EDITABOUT,
        REVIEWS
    }
}