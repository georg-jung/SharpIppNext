using SharpIpp.Models;
using SharpIpp.Protocol;
using SharpIpp.Protocol.Models;
using System;
using System.Linq;

namespace SharpIpp.Mapping.Profiles
{
    // ReSharper disable once UnusedMember.Global
    internal class IppProfile : IProfile
    {
        public void CreateMaps(IMapperConstructor mapper)
        {
            mapper.CreateMap<IIppRequest, IppRequestMessage>((src, dst, map) =>
            {
                dst.Version = src.Version;
                dst.RequestId = src.RequestId;
                var operation = dst.OperationAttributes;
                operation.Add(new IppAttribute(Tag.Charset, JobAttribute.AttributesCharset, "utf-8"));
                operation.Add(new IppAttribute(Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en"));
                return dst;
            });

            mapper.CreateMap<IIppRequestMessage, IIppRequest>( ( src, dst, map ) =>
            {
                dst.Version = src.Version;
                dst.RequestId = src.RequestId;
                return dst;
            } );

            mapper.CreateMap<IppResponseMessage, IIppResponseMessage>((src, dst, map) =>
            {
                dst.Version = src.Version;
                dst.RequestId = src.RequestId;
                dst.Sections.AddRange(src.Sections);
                return dst;
            });

            mapper.CreateMap<IIppResponseMessage, IppResponseMessage>( ( src, dst, map ) =>
            {
                dst.Version = src.Version;
                dst.RequestId = src.RequestId;
                dst.StatusCode = src.StatusCode;
                if ( !src.Sections.Any( x => x.Tag == SectionTag.OperationAttributesTag ) )
                {
                    var section = new IppSection { Tag = SectionTag.OperationAttributesTag };
                    section.Attributes.Add( new IppAttribute( Tag.Charset, JobAttribute.AttributesCharset, "utf-8" ) );
                    section.Attributes.Add( new IppAttribute( Tag.NaturalLanguage, JobAttribute.AttributesNaturalLanguage, "en" ) );
                    dst.Sections.Add( section );
                }
                dst.Sections.AddRange( src.Sections );
                return dst;
            } );

            mapper.CreateMap<IIppPrinterRequest, IppRequestMessage>((src, dst, map) =>
            {
                map.Map<IIppRequest, IppRequestMessage>(src, dst);
                return dst;
            });

            mapper.CreateMap<IIppRequestMessage, IIppPrinterRequest>( ( src, dst, map ) =>
            {
                map.Map<IIppRequestMessage, IIppRequest>( src, dst );
                return dst;
            } );
        }
    }
}
