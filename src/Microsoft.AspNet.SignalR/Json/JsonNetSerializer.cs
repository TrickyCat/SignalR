﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Microsoft.AspNet.SignalR
{
    /// <summary>
    /// Default <see cref="IJsonSerializer"/> implementation over Json.NET.
    /// </summary>
    public class JsonNetSerializer : IJsonSerializer
    {
        private readonly JsonSerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetSerializer"/> class.
        /// </summary>
        public JsonNetSerializer()
            : this(new JsonSerializerSettings())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonNetSerializer"/> class.
        /// </summary>
        /// <param name="settings">The <see cref="T:Newtonsoft.Json.JsonSerializerSettings"/> to use when serializing and deserializing.</param>
        public JsonNetSerializer(JsonSerializerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            _serializer = JsonSerializer.Create(settings);
        }

        /// <summary>
        /// Deserializes the JSON to a .NET object.
        /// </summary>
        /// <param name="json">The JSON to deserialize.</param>
        /// <param name="targetType">The <see cref="System.Type"/> of object being deserialized.</param>
        /// <returns>The deserialized object from the JSON string.</returns>
        public object Parse(string json, Type targetType)
        {
            using (var reader = new StringReader(json))
            {
                return _serializer.Deserialize(reader, targetType);
            }
        }

        /// <summary>
        /// Serializes the specified object to a <see cref="TextWriter"/>.
        /// </summary>
        /// <param name="value">The object to serialize</param>
        /// <param name="writer">The <see cref="TextWriter"/> to serialize the object to.</param>
        public void Serialize(object value, TextWriter writer)
        {
            var selfSerializer = value as IJsonWritable;
            if (selfSerializer != null)
            {
                selfSerializer.WriteJson(writer);
            }
            else
            {
                _serializer.Serialize(writer, value);
            }
        }
    }
}