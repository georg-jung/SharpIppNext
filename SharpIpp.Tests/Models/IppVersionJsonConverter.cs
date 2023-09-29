using SharpIpp.Protocol.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharpIpp.Tests.Models;

public class IppVersionJsonConverter : JsonConverter<IppVersion>
{
    public override IppVersion Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        return new IppVersion( reader.GetInt16() );
    }

    public override void Write( Utf8JsonWriter writer, IppVersion value, JsonSerializerOptions options )
    {
        writer.WriteNumberValue( value.ToInt16BigEndian() );
    }
}