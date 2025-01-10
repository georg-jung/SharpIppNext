using SharpIpp.Mapping;
using SharpIpp.Protocol.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SharpIpp.Models;
public class OperationAttributes
{
    public Uri PrinterUri { get; set; } = null!;

    public string? RequestingUserName { get; set; }

    public static T Create<T>(Dictionary<string, IppAttribute[]> dict, IMapperApplier mapper) where T : OperationAttributes, new()
    {
        var attributes = new T();
        var printerUri = mapper.MapFromDic<string?>(dict, JobAttribute.PrinterUri);
        if(printerUri != null && Uri.TryCreate(printerUri, UriKind.RelativeOrAbsolute, out Uri uri))
            attributes.PrinterUri = uri;
        attributes.RequestingUserName = mapper.MapFromDic<string?>(dict, JobAttribute.RequestingUserName);
        return attributes;
    }

    public virtual IEnumerable<IppAttribute> GetIppAttributes(IMapperApplier mapper)
    {
        if (PrinterUri != null)
            yield return new IppAttribute(Tag.Uri, JobAttribute.PrinterUri, PrinterUri.ToString());
        if (RequestingUserName != null)
            yield return new IppAttribute(Tag.NameWithoutLanguage, JobAttribute.RequestingUserName, RequestingUserName);
    }
}
