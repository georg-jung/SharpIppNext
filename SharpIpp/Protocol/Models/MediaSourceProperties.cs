using SharpIpp.Mapping;
using SharpIpp.Protocol.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SharpIpp.Protocol.Models;
public class MediaSourceProperties
{
    /// <summary>
    /// type2 keyword
    /// </summary>
    public MediaSourceFeedDirection? MediaSourceFeedDirection { get; set; }

    /// <summary>
    /// type2 enum
    /// </summary>
    public Orientation? MediaSourceFeedOrientation { get; set; }

    public IEnumerable<IppAttribute> GetIppAttributes(IMapperApplier mapper)
    {
        if (MediaSourceFeedDirection.HasValue)
            yield return new IppAttribute(Tag.Keyword, nameof(MediaSourceFeedDirection).ConvertCamelCaseToDash(), mapper.Map<string>(MediaSourceFeedDirection.Value));
        if (MediaSourceFeedOrientation.HasValue)
            yield return new IppAttribute(Tag.Enum, nameof(MediaSourceFeedOrientation).ConvertCamelCaseToDash(), mapper.Map<string>(MediaSourceFeedOrientation.Value));
    }

    public static MediaSourceProperties Create(IMapperApplier mapper, IppAttribute[] attributes)
    {
        if (attributes.Length < 3)
            return new MediaSourceProperties();
        var dict = attributes.Skip(1).Take(attributes.Length - 2).ToIppDictionary();
        return new MediaSourceProperties
        {
            MediaSourceFeedDirection = mapper.MapFromDic<MediaSourceFeedDirection?>(dict, nameof(MediaSourceFeedDirection).ConvertCamelCaseToDash()),
            MediaSourceFeedOrientation = mapper.MapFromDic<Orientation?>(dict, nameof(MediaSourceFeedOrientation).ConvertCamelCaseToDash())
        };
    }
}
